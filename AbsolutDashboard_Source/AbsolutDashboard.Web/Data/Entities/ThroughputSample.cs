using System;

namespace AbsolutDashboard.Web.Data.Entities
{
	// Token: 0x02000090 RID: 144
	public class ThroughputSample
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00019359 File Offset: 0x00017559
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00019361 File Offset: 0x00017561
		public int Id { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001936A File Offset: 0x0001756A
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00019372 File Offset: 0x00017572
		public DateTime Timestamp { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001937B File Offset: 0x0001757B
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00019383 File Offset: 0x00017583
		public double DlMbps { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001938C File Offset: 0x0001758C
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00019394 File Offset: 0x00017594
		public double UlMbps { get; set; }
	}
}
