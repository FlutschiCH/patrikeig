using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000073 RID: 115
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class CellConfigEdit
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001299E File Offset: 0x00010B9E
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x000129A6 File Offset: 0x00010BA6
		public int CellIndex { get; set; } = 1;

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x000129AF File Offset: 0x00010BAF
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x000129B7 File Offset: 0x00010BB7
		public int? Band { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x000129C0 File Offset: 0x00010BC0
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x000129C8 File Offset: 0x00010BC8
		public int? Bandwidth { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000129D1 File Offset: 0x00010BD1
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x000129D9 File Offset: 0x00010BD9
		public double? CentreMhz { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x000129E2 File Offset: 0x00010BE2
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x000129EA File Offset: 0x00010BEA
		public string TddPattern { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x000129F3 File Offset: 0x00010BF3
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x000129FB File Offset: 0x00010BFB
		public string SpecialSlot { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00012A04 File Offset: 0x00010C04
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00012A0C File Offset: 0x00010C0C
		public int? MaxTxPower { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00012A15 File Offset: 0x00010C15
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00012A1D File Offset: 0x00010C1D
		public string AdminState { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00012A26 File Offset: 0x00010C26
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00012A2E File Offset: 0x00010C2E
		public int? Antennas { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00012A37 File Offset: 0x00010C37
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x00012A3F File Offset: 0x00010C3F
		public int? Layers { get; set; }
	}
}
