using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Services.Collectors.Rhub;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeviceSnapshot
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A72C File Offset: 0x0000892C
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000A734 File Offset: 0x00008934
		public List<Metric> Metrics { get; set; } = new List<Metric>();

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A73D File Offset: 0x0000893D
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000A745 File Offset: 0x00008945
		public List<PortStatus> Ports { get; set; } = new List<PortStatus>();

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000A74E File Offset: 0x0000894E
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000A756 File Offset: 0x00008956
		public List<ChildStatus> Children { get; set; } = new List<ChildStatus>();

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000A75F File Offset: 0x0000895F
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000A767 File Offset: 0x00008967
		public List<AlarmStatus> Alarms { get; set; } = new List<AlarmStatus>();

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A770 File Offset: 0x00008970
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000A778 File Offset: 0x00008978
		public List<RhubPort> HubPorts { get; set; } = new List<RhubPort>();
	}
}
