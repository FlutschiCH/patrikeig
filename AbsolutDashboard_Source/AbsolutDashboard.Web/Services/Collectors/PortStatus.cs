using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PortStatus
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000AA90 File Offset: 0x00008C90
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000AA98 File Offset: 0x00008C98
		public string Index { get; set; } = "";

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000AAA1 File Offset: 0x00008CA1
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000AAA9 File Offset: 0x00008CA9
		public string Name { get; set; } = "";

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000AAB2 File Offset: 0x00008CB2
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000AABA File Offset: 0x00008CBA
		public bool Up { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000AAC3 File Offset: 0x00008CC3
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000AACB File Offset: 0x00008CCB
		[Nullable(2)]
		public string AdminStatus
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000AADC File Offset: 0x00008CDC
		[Nullable(2)]
		public string Detail
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000AAE5 File Offset: 0x00008CE5
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000AAED File Offset: 0x00008CED
		public bool Physical { get; set; }
	}
}
