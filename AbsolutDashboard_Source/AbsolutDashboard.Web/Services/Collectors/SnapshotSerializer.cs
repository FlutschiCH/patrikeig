using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	public static class SnapshotSerializer
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000AD54 File Offset: 0x00008F54
		public static string Serialize(DeviceSnapshot snapshot)
		{
			return JsonSerializer.Serialize<DeviceSnapshot>(snapshot, SnapshotSerializer.Opts);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AD64 File Offset: 0x00008F64
		[NullableContext(2)]
		public static DeviceSnapshot Deserialize(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}
			DeviceSnapshot deviceSnapshot;
			try
			{
				deviceSnapshot = JsonSerializer.Deserialize<DeviceSnapshot>(json, SnapshotSerializer.Opts);
			}
			catch
			{
				deviceSnapshot = null;
			}
			return deviceSnapshot;
		}

		// Token: 0x040000F0 RID: 240
		private static readonly JsonSerializerOptions Opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
	}
}
