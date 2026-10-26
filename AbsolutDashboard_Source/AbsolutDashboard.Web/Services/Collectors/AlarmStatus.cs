using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000047 RID: 71
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class AlarmStatus
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000ABCB File Offset: 0x00008DCB
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000ABD3 File Offset: 0x00008DD3
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000ABDC File Offset: 0x00008DDC
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public string Severity { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000ABED File Offset: 0x00008DED
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000ABF5 File Offset: 0x00008DF5
		public string Description { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000ABFE File Offset: 0x00008DFE
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000AC06 File Offset: 0x00008E06
		public string RaisedAt { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000AC0F File Offset: 0x00008E0F
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000AC17 File Offset: 0x00008E17
		public string Id { get; set; }
	}
}
