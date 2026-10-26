using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000056 RID: 86
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SshFollowUp
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000DA51 File Offset: 0x0000BC51
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000DA59 File Offset: 0x0000BC59
		[JsonPropertyName("tokenPattern")]
		public string TokenPattern { get; set; } = "";

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000DA62 File Offset: 0x0000BC62
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000DA6A File Offset: 0x0000BC6A
		[JsonPropertyName("commandTemplate")]
		public string CommandTemplate { get; set; } = "";

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000DA73 File Offset: 0x0000BC73
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000DA7B File Offset: 0x0000BC7B
		[JsonPropertyName("max")]
		public int Max { get; set; } = 32;
	}
}
