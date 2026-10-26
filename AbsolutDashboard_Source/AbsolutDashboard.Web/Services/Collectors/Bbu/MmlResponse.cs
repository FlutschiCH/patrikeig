using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MmlResponse
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000157A3 File Offset: 0x000139A3
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000157AB File Offset: 0x000139AB
		public int RetCode { get; set; } = -1;

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000157B4 File Offset: 0x000139B4
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x000157BC File Offset: 0x000139BC
		public string Message { get; set; } = "";

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x000157C5 File Offset: 0x000139C5
		public bool Success
		{
			get
			{
				return this.RetCode == 0;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000157D0 File Offset: 0x000139D0
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x000157D8 File Offset: 0x000139D8
		public Dictionary<string, string> Scalars { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x000157E1 File Offset: 0x000139E1
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x000157E9 File Offset: 0x000139E9
		public List<string> Columns { get; set; } = new List<string>();

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000157F2 File Offset: 0x000139F2
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x000157FA File Offset: 0x000139FA
		public List<Dictionary<string, string>> Rows { get; set; } = new List<Dictionary<string, string>>();
	}
}
