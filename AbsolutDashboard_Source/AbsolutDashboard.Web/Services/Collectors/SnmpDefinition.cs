using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SnmpDefinition
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000D7FA File Offset: 0x0000B9FA
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000D802 File Offset: 0x0000BA02
		[JsonPropertyName("metrics")]
		public List<SnmpMetric> Metrics { get; set; } = new List<SnmpMetric>();

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000D80B File Offset: 0x0000BA0B
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000D813 File Offset: 0x0000BA13
		[JsonPropertyName("walkInterfaces")]
		public bool WalkInterfaces { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000D81C File Offset: 0x0000BA1C
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000D824 File Offset: 0x0000BA24
		[JsonPropertyName("vendorEnterprise")]
		public int? VendorEnterprise { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000D82D File Offset: 0x0000BA2D
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000D835 File Offset: 0x0000BA35
		[Nullable(2)]
		[JsonPropertyName("vendorName")]
		public string VendorName
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}
	}
}
