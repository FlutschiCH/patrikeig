using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Data.Entities
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public class DeviceCategory
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00019058 File Offset: 0x00017258
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00019060 File Offset: 0x00017260
		public int Id { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00019069 File Offset: 0x00017269
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00019071 File Offset: 0x00017271
		public string Key { get; set; } = "";

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001907A File Offset: 0x0001727A
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00019082 File Offset: 0x00017282
		public string Name { get; set; } = "";

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001908B File Offset: 0x0001728B
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00019093 File Offset: 0x00017293
		public string ShortName { get; set; } = "";

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001909C File Offset: 0x0001729C
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x000190A4 File Offset: 0x000172A4
		[Nullable(2)]
		public string Description
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000190AD File Offset: 0x000172AD
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x000190B5 File Offset: 0x000172B5
		public int DisplayOrder { get; set; }

		// Token: 0x06000550 RID: 1360 RVA: 0x000190BE File Offset: 0x000172BE
		public static bool IsHiddenKey(string key)
		{
			return key == "switch";
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x000190CB File Offset: 0x000172CB
		public bool IsHidden
		{
			get
			{
				return DeviceCategory.IsHiddenKey(this.Key);
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000190D8 File Offset: 0x000172D8
		public static bool IsNavHiddenKey(string key)
		{
			return DeviceCategory.IsHiddenKey(key) || key == "au";
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000190EF File Offset: 0x000172EF
		public bool IsNavHidden
		{
			get
			{
				return DeviceCategory.IsNavHiddenKey(this.Key);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000190FC File Offset: 0x000172FC
		public bool AllowsManualAdd
		{
			get
			{
				bool flag = !this.IsHidden;
				if (flag)
				{
					string key = this.Key;
					bool flag2 = key == "bbu" || key == "switch" || key == "ims" || key == "mobile";
					flag = flag2;
				}
				return flag;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001915A File Offset: 0x0001735A
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00019162 File Offset: 0x00017362
		public ICollection<DeviceTemplate> Templates { get; set; } = new List<DeviceTemplate>();

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001916B File Offset: 0x0001736B
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x00019173 File Offset: 0x00017373
		public ICollection<Device> Devices { get; set; } = new List<Device>();
	}
}
