using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Data.Entities
{
	// Token: 0x0200008D RID: 141
	[NullableContext(2)]
	[Nullable(0)]
	public class DevicePollResult
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000191BB File Offset: 0x000173BB
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x000191C3 File Offset: 0x000173C3
		public int Id { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000191CC File Offset: 0x000173CC
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x000191D4 File Offset: 0x000173D4
		public int DeviceId { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x000191DD File Offset: 0x000173DD
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x000191E5 File Offset: 0x000173E5
		[Nullable(1)]
		public Device Device
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000191EE File Offset: 0x000173EE
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000191F6 File Offset: 0x000173F6
		public DateTime Timestamp { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000191FF File Offset: 0x000173FF
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00019207 File Offset: 0x00017407
		public bool Reachable { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00019210 File Offset: 0x00017410
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00019218 File Offset: 0x00017418
		public int? LatencyMs { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00019221 File Offset: 0x00017421
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00019229 File Offset: 0x00017429
		public string SnapshotJson { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00019232 File Offset: 0x00017432
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001923A File Offset: 0x0001743A
		public string Error { get; set; }
	}
}
