import os
import sys
import paramiko

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    print(f"[*] Task worker connecting to {REMOTE_USER}@{REMOTE_HOST}...")
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

    try:
        client.connect(
            hostname=REMOTE_HOST,
            port=22,
            username=REMOTE_USER,
            password=REMOTE_PASS,
            timeout=8,
            look_for_keys=True,
            allow_agent=True
        )
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    # Let's inspect where InvokeAsync is defined in MethodDef table!
    # A .NET metadata parser in python takes only 30 lines!
    # Let's read the MethodDef table to get the exact RVA of InvokeAsync and Configure.
    py_code = """
import struct

with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    data = f.read()

# PE Header parsing
pe_offset = struct.unpack_from('<I', data, 0x3c)[0]
optional_header_offset = pe_offset + 24
magic = struct.unpack_from('<H', data, optional_header_offset)[0]
is_64 = (magic == 0x20b)

# CLI Header is DataDirectory[14]
data_dir_offset = optional_header_offset + (112 if is_64 else 96)
cli_rva, cli_size = struct.unpack_from('<II', data, data_dir_offset + 14 * 8)

# Section Headers
num_sections = struct.unpack_from('<H', data, pe_offset + 6)[0]
opt_hdr_size = struct.unpack_from('<H', data, pe_offset + 20)[0]
sec_start = optional_header_offset + opt_hdr_size

sections = []
for i in range(num_sections):
    sec = data[sec_start + i*40 : sec_start + (i+1)*40]
    name = sec[:8].rstrip(b'\\x00').decode('latin1')
    vsize, vrva, rawsize, rawptr = struct.unpack_from('<IIII', sec, 8)
    sections.append((name, vrva, vsize, rawptr, rawsize))

def rva_to_offset(rva):
    for name, vrva, vsize, rawptr, rawsize in sections:
        if vrva <= rva < vrva + max(vsize, rawsize):
            return rawptr + (rva - vrva)
    return None

cli_offset = rva_to_offset(cli_rva)
meta_rva, meta_size = struct.unpack_from('<II', data, cli_offset + 8)
meta_offset = rva_to_offset(meta_rva)

# Metadata Header
magic = struct.unpack_from('<I', data, meta_offset)[0]
version_len = struct.unpack_from('<I', data, meta_offset + 12)[0]
version_str = data[meta_offset + 16 : meta_offset + 16 + version_len].rstrip(b'\\x00').decode('latin1')
flags = struct.unpack_from('<H', data, meta_offset + 16 + version_len)[0]
streams_count = struct.unpack_from('<H', data, meta_offset + 18 + version_len)[0]

stream_pos = meta_offset + 20 + version_len
streams = {}
for _ in range(streams_count):
    s_offset, s_size = struct.unpack_from('<II', data, stream_pos)
    s_name_raw = data[stream_pos + 8 : stream_pos + 40].split(b'\\x00')[0]
    s_name = s_name_raw.decode('latin1')
    streams[s_name] = (meta_offset + s_offset, s_size)
    stream_pos += 8 + ((len(s_name_raw) + 4) & ~3)

print("Streams:", {k: hex(v[0]) for k, v in streams.items()})

# Strings heap
strings_offset, strings_size = streams['#Strings']
def get_string(idx):
    if idx >= strings_size: return ""
    end = data.find(b'\\x00', strings_offset + idx)
    return data[strings_offset + idx : end].decode('utf-8', errors='replace')

# Tables stream #~
tables_offset, tables_size = streams['#~']
heap_sizes = data[tables_offset + 6]
valid_mask = struct.unpack_from('<Q', data, tables_offset + 8)[0]

pos = tables_offset + 24
table_counts = {}
for i in range(64):
    if (valid_mask >> i) & 1:
        cnt = struct.unpack_from('<I', data, pos)[0]
        table_counts[i] = cnt
        pos += 4

# Table IDs:
# TypeDef: 0x02
# MethodDef: 0x06
# Param: 0x08
# Field: 0x04

string_index_size = 4 if (heap_sizes & 0x01) else 2
guid_index_size = 4 if (heap_sizes & 0x02) else 2
blob_index_size = 4 if (heap_sizes & 0x04) else 2

def read_idx(buf, offset, size):
    if size == 2:
        return struct.unpack_from('<H', buf, offset)[0]
    else:
        return struct.unpack_from('<I', buf, offset)[0]

# Calculate table offsets
table_offsets = {}
curr = pos
for i in range(64):
    if (valid_mask >> i) & 1:
        table_offsets[i] = curr
        cnt = table_counts[i]
        # Calculate row size
        if i == 0x02: # TypeDef
            row_size = 4 + string_index_size*2 + 2 + 2 + 2 # approximate or standard
        # Let's inspect MethodDef directly!
        # MethodDef row: RVA(4) + ImplFlags(2) + Flags(2) + Name(string_index) + Signature(blob_index) + ParamList(2 or 4)
        pass

# Let's find MethodDef table directly by looking for methods named 'InvokeAsync' and 'Configure'
# In strings heap, find offset of 'InvokeAsync' and 'Configure'
s_invoke = data[strings_offset:strings_offset+strings_size].find(b'InvokeAsync\\x00')
s_config = data[strings_offset:strings_offset+strings_size].find(b'Configure\\x00')
print(f"'InvokeAsync' string index: {s_invoke}")
print(f"'Configure' string index: {s_config}")

# Now scan MethodDef rows for these string indices!
# MethodDef count:
m_cnt = table_counts.get(0x06, 0)
print(f"MethodDef count: {m_cnt}")

# Let's find MethodDef table offset
# Tables before 0x06: 0x00(Module), 0x01(TypeRef), 0x02(TypeDef), 0x04(Field)
# Let's compute their exact row sizes:
def coded_token_size(tables):
    max_rows = max(table_counts.get(t, 0) for t in tables)
    tag_bits = (len(tables) - 1).bit_length()
    return 4 if (max_rows << tag_bits) > 65535 else 2

type_def_or_ref_size = coded_token_size([0x02, 0x01, 0x1b])
param_size = 4 if table_counts.get(0x08, 0) > 65535 else 2
field_size = 4 if table_counts.get(0x04, 0) > 65535 else 2
method_size = 4 if table_counts.get(0x06, 0) > 65535 else 2

row_sizes = {
    0x00: 2 + string_index_size + guid_index_size*3,
    0x01: coded_token_size([0x00, 0x1a, 0x23, 0x01]) + string_index_size*2,
    0x02: 4 + string_index_size*2 + type_def_or_ref_size + field_size + method_size,
    0x04: 2 + string_index_size + blob_index_size,
    0x06: 4 + 2 + 2 + string_index_size + blob_index_size + param_size,
}

m_offset = pos
for t in range(0x06):
    if (valid_mask >> t) & 1:
        m_offset += table_counts[t] * row_sizes[t]

m_row_size = row_sizes[0x06]
print(f"MethodDef table at {m_offset}, row size {m_row_size}")

for row_i in range(m_cnt):
    row_addr = m_offset + row_i * m_row_size
    rva = struct.unpack_from('<I', data, row_addr)[0]
    name_idx = read_idx(data, row_addr + 8, string_index_size)
    if name_idx in (s_invoke, s_config):
        name = get_string(name_idx)
        file_off = rva_to_offset(rva)
        print(f"Method {row_i+1} [{name}]: RVA=0x{rva:x}, FileOffset={file_off}")
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

    _, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        sys.stdout.buffer.write(out.encode('utf-8', errors='replace'))
        sys.stdout.buffer.write(b'\n')
    if err:
        sys.stderr.buffer.write(err.encode('utf-8', errors='replace'))
        sys.stderr.buffer.write(b'\n')

if __name__ == "__main__":
    run()
