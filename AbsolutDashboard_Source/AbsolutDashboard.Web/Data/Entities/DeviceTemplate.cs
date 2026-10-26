using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Data.Entities
{
	// Token: 0x0200008E RID: 142
	[NullableContext(1)]
	[Nullable(0)]
	public class DeviceTemplate
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001924B File Offset: 0x0001744B
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x00019253 File Offset: 0x00017453
		public int Id { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001925C File Offset: 0x0001745C
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00019264 File Offset: 0x00017464
		public int CategoryId { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001926D File Offset: 0x0001746D
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00019275 File Offset: 0x00017475
		public DeviceCategory Category { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001927E File Offset: 0x0001747E
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00019286 File Offset: 0x00017486
		public string Name { get; set; } = "";

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001928F File Offset: 0x0001748F
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00019297 File Offset: 0x00017497
		[Nullable(2)]
		public string Vendor
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x000192A0 File Offset: 0x000174A0
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x000192A8 File Offset: 0x000174A8
		[Nullable(2)]
		public string Model
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x000192B1 File Offset: 0x000174B1
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x000192B9 File Offset: 0x000174B9
		[Nullable(2)]
		public string Description
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x000192C2 File Offset: 0x000174C2
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x000192CA File Offset: 0x000174CA
		public CollectionMethod CollectionMethod { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x000192D3 File Offset: 0x000174D3
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x000192DB File Offset: 0x000174DB
		public int DefaultPort { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x000192E4 File Offset: 0x000174E4
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x000192EC File Offset: 0x000174EC
		public int DefaultPollIntervalSeconds { get; set; } = 30;

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x000192F5 File Offset: 0x000174F5
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x000192FD File Offset: 0x000174FD
		public bool IsGeneric { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00019306 File Offset: 0x00017506
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0001930E File Offset: 0x0001750E
		public string DefinitionJson { get; set; } = "{}";

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00019317 File Offset: 0x00017517
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001931F File Offset: 0x0001751F
		public ICollection<Device> Devices { get; set; } = new List<Device>();
	}
}
