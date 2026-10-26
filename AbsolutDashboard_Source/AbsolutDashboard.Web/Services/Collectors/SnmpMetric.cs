using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000054 RID: 84
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class SnmpMetric
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D851 File Offset: 0x0000BA51
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000D859 File Offset: 0x0000BA59
		[Nullable(1)]
		[JsonPropertyName("label")]
		public string Label
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000D862 File Offset: 0x0000BA62
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x0000D86A File Offset: 0x0000BA6A
		[Nullable(1)]
		[JsonPropertyName("oid")]
		public string Oid
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000D873 File Offset: 0x0000BA73
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000D87B File Offset: 0x0000BA7B
		[JsonPropertyName("unit")]
		public string Unit { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000D884 File Offset: 0x0000BA84
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000D88C File Offset: 0x0000BA8C
		[JsonPropertyName("group")]
		public string Group { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000D895 File Offset: 0x0000BA95
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000D89D File Offset: 0x0000BA9D
		[JsonPropertyName("walk")]
		public bool Walk { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000D8A6 File Offset: 0x0000BAA6
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000D8AE File Offset: 0x0000BAAE
		[JsonPropertyName("aggregate")]
		public string Aggregate { get; set; }
	}
}
