using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000052 RID: 82
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class TemplateDefinition
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000D741 File Offset: 0x0000B941
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000D749 File Offset: 0x0000B949
		[JsonPropertyName("snmp")]
		public SnmpDefinition Snmp { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000D752 File Offset: 0x0000B952
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000D75A File Offset: 0x0000B95A
		[JsonPropertyName("ssh")]
		public SshDefinition Ssh { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000D763 File Offset: 0x0000B963
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000D76B File Offset: 0x0000B96B
		[JsonPropertyName("viaParent")]
		public ViaParentDefinition ViaParent { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000D774 File Offset: 0x0000B974
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000D77C File Offset: 0x0000B97C
		[JsonPropertyName("bbu")]
		public BbuDefinition Bbu { get; set; }

		// Token: 0x06000299 RID: 665 RVA: 0x0000D788 File Offset: 0x0000B988
		[NullableContext(1)]
		public static TemplateDefinition Parse([Nullable(2)] string json)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return new TemplateDefinition();
			}
			TemplateDefinition templateDefinition;
			try
			{
				templateDefinition = JsonSerializer.Deserialize<TemplateDefinition>(json, TemplateDefinition.Opts) ?? new TemplateDefinition();
			}
			catch
			{
				templateDefinition = new TemplateDefinition();
			}
			return templateDefinition;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		[NullableContext(1)]
		public static string Serialize(TemplateDefinition def)
		{
			return JsonSerializer.Serialize<TemplateDefinition>(def, TemplateDefinition.Opts);
		}

		// Token: 0x04000127 RID: 295
		[Nullable(1)]
		private static readonly JsonSerializerOptions Opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
	}
}
