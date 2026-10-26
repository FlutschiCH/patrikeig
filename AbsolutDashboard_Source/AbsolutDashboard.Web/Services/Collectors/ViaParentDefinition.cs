using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ViaParentDefinition
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000DAAA File Offset: 0x0000BCAA
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000DAB2 File Offset: 0x0000BCB2
		[JsonPropertyName("kind")]
		public string Kind { get; set; } = "";
	}
}
