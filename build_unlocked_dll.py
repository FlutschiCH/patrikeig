import os

source_dll = os.path.join("SSH Root", "SSH Root", "opt", "absolutdashboard", "AbsolutDashboard.Web.dll")
if not os.path.exists(source_dll):
    source_dll = os.path.join("AbsolutDashboard_Source", "AbsolutDashboard.Web.dll")

with open(source_dll, "rb") as f:
    dll = bytearray(f.read())

offset = 18387
print("Original bytes at 18387:", ' '.join(f"{b:02x}" for b in dll[offset:offset+26]))

# Patch Method 293 (IntegrityGateStartupFilter.Configure) with:
# 0a (Tiny header, code size 2)
# 03 (ldarg.1 -> returns 'next' Action<IApplicationBuilder>)
# 2a (ret)
# + 22 nops (0x00)
patched_body = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)
dll[offset:offset+25] = patched_body

os.makedirs("deploy", exist_ok=True)
output_path = os.path.join("deploy", "AbsolutDashboard.Web.dll")
with open(output_path, "wb") as f:
    f.write(dll)

print(f"[SUCCESS] Generated cracked/unlocked DLL: {output_path}")
print("Patched bytes:          ", ' '.join(f"{b:02x}" for b in dll[offset:offset+25]))
