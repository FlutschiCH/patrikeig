using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Operations
{
	// Token: 0x02000029 RID: 41
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class OperationStep
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000061CB File Offset: 0x000043CB
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000061D3 File Offset: 0x000043D3
		[Nullable(1)]
		public string Label
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000061DC File Offset: 0x000043DC
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000061E4 File Offset: 0x000043E4
		[Nullable(1)]
		public string Kind
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000061ED File Offset: 0x000043ED
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000061F5 File Offset: 0x000043F5
		public string Device { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000061FE File Offset: 0x000043FE
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00006206 File Offset: 0x00004406
		public string Host { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000620F File Offset: 0x0000440F
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00006217 File Offset: 0x00004417
		public int Port { get; set; } = 22;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00006220 File Offset: 0x00004420
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00006228 File Offset: 0x00004428
		public string User { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006231 File Offset: 0x00004431
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00006239 File Offset: 0x00004439
		public string Password { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00006242 File Offset: 0x00004442
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000624A File Offset: 0x0000444A
		[Nullable(1)]
		public string Command
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00006253 File Offset: 0x00004453
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000625B File Offset: 0x0000445B
		public int WaitSeconds { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00006264 File Offset: 0x00004464
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000626C File Offset: 0x0000446C
		public bool ContinueOnError { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006275 File Offset: 0x00004475
		public bool IsMml
		{
			get
			{
				return string.Equals(this.Kind, "mml", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006288 File Offset: 0x00004488
		public bool IsSsh
		{
			get
			{
				return string.Equals(this.Kind, "ssh", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000629C File Offset: 0x0000449C
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.Command) && (this.IsMml || (this.IsSsh && (!string.IsNullOrWhiteSpace(this.Device) || (!string.IsNullOrWhiteSpace(this.Host) && !string.IsNullOrWhiteSpace(this.User) && !string.IsNullOrWhiteSpace(this.Password)))));
			}
		}
	}
}
