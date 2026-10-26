using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PollOutcome
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A6C0 File Offset: 0x000088C0
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A6C8 File Offset: 0x000088C8
		public bool Reachable { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A6D1 File Offset: 0x000088D1
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A6D9 File Offset: 0x000088D9
		[Nullable(2)]
		public string Error
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A6E2 File Offset: 0x000088E2
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000A6EA File Offset: 0x000088EA
		public int? LatencyMs { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A6F3 File Offset: 0x000088F3
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000A6FB File Offset: 0x000088FB
		public DeviceSnapshot Snapshot { get; set; } = new DeviceSnapshot();

		// Token: 0x06000204 RID: 516 RVA: 0x0000A704 File Offset: 0x00008904
		public static PollOutcome Unreachable(string error)
		{
			return new PollOutcome
			{
				Reachable = false,
				Error = error
			};
		}
	}
}
