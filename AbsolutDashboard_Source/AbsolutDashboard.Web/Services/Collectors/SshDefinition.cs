using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SshDefinition
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000D8D5 File Offset: 0x0000BAD5
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000D8DD File Offset: 0x0000BADD
		[JsonPropertyName("parser")]
		public string Parser { get; set; } = "generic";

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000D8E6 File Offset: 0x0000BAE6
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000D8EE File Offset: 0x0000BAEE
		[JsonPropertyName("commands")]
		public List<string> Commands { get; set; } = new List<string>();

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000D8F7 File Offset: 0x0000BAF7
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000D8FF File Offset: 0x0000BAFF
		[JsonPropertyName("interactive")]
		public bool Interactive { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D908 File Offset: 0x0000BB08
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000D910 File Offset: 0x0000BB10
		[JsonPropertyName("hostKeyAlgorithms")]
		public List<string> HostKeyAlgorithms { get; set; } = new List<string>();

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000D919 File Offset: 0x0000BB19
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000D921 File Offset: 0x0000BB21
		[JsonPropertyName("keyExchangeAlgorithms")]
		public List<string> KeyExchangeAlgorithms { get; set; } = new List<string>();

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000D92A File Offset: 0x0000BB2A
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000D932 File Offset: 0x0000BB32
		[JsonPropertyName("encryptions")]
		public List<string> Encryptions { get; set; } = new List<string>();

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000D93B File Offset: 0x0000BB3B
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000D943 File Offset: 0x0000BB43
		[Nullable(2)]
		[JsonPropertyName("followUpCommandTemplate")]
		public string FollowUpCommandTemplate
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000D94C File Offset: 0x0000BB4C
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000D954 File Offset: 0x0000BB54
		[Nullable(2)]
		[JsonPropertyName("followUpTokenPattern")]
		public string FollowUpTokenPattern
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000D95D File Offset: 0x0000BB5D
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000D965 File Offset: 0x0000BB65
		[JsonPropertyName("followUpMax")]
		public int FollowUpMax { get; set; } = 32;

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000D96E File Offset: 0x0000BB6E
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000D976 File Offset: 0x0000BB76
		[JsonPropertyName("followUps")]
		public List<SshFollowUp> FollowUps { get; set; } = new List<SshFollowUp>();

		// Token: 0x060002C7 RID: 711 RVA: 0x0000D980 File Offset: 0x0000BB80
		public IEnumerable<SshFollowUp> EffectiveFollowUps()
		{
			if (this.FollowUps.Count > 0)
			{
				return this.FollowUps;
			}
			if (string.IsNullOrWhiteSpace(this.FollowUpCommandTemplate) || string.IsNullOrWhiteSpace(this.FollowUpTokenPattern))
			{
				return Array.Empty<SshFollowUp>();
			}
			return new SshFollowUp[]
			{
				new SshFollowUp
				{
					TokenPattern = this.FollowUpTokenPattern,
					CommandTemplate = this.FollowUpCommandTemplate,
					Max = this.FollowUpMax
				}
			};
		}
	}
}
