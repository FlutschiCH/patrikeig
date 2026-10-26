using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000058 RID: 88
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuDefinition
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000DACE File Offset: 0x0000BCCE
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000DAD6 File Offset: 0x0000BCD6
		[JsonPropertyName("extraCommands")]
		public List<string> ExtraCommands { get; set; } = new List<string>();
	}
}
