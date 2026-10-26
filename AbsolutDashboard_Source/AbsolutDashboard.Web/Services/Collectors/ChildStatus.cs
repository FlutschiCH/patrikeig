using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000046 RID: 70
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class ChildStatus
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000AB14 File Offset: 0x00008D14
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000AB1C File Offset: 0x00008D1C
		[Nullable(1)]
		public string Identifier
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000AB25 File Offset: 0x00008D25
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000AB2D File Offset: 0x00008D2D
		public string Kind { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000AB36 File Offset: 0x00008D36
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000AB3E File Offset: 0x00008D3E
		public bool? Up { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000AB47 File Offset: 0x00008D47
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000AB4F File Offset: 0x00008D4F
		public string State { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000AB58 File Offset: 0x00008D58
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000AB60 File Offset: 0x00008D60
		public string SerialNumber { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000AB69 File Offset: 0x00008D69
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000AB71 File Offset: 0x00008D71
		public string Model { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000AB7A File Offset: 0x00008D7A
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000AB82 File Offset: 0x00008D82
		public string Version { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000AB8B File Offset: 0x00008D8B
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000AB93 File Offset: 0x00008D93
		public string RouteAddr { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000AB9C File Offset: 0x00008D9C
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000ABA4 File Offset: 0x00008DA4
		[Nullable(1)]
		public List<Metric> Metrics
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = new List<Metric>();
	}
}
