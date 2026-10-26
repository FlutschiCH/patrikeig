using System;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000041 RID: 65
	public sealed class PollingOptions
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000A51D File Offset: 0x0000871D
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000A525 File Offset: 0x00008725
		public bool Enabled { get; set; } = true;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A52E File Offset: 0x0000872E
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000A536 File Offset: 0x00008736
		public int TickSeconds { get; set; } = 5;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A53F File Offset: 0x0000873F
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000A547 File Offset: 0x00008747
		public int DefaultIntervalSeconds { get; set; } = 30;

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000A550 File Offset: 0x00008750
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000A558 File Offset: 0x00008758
		public int TimeoutSeconds { get; set; } = 15;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A561 File Offset: 0x00008761
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000A569 File Offset: 0x00008769
		public int BbuTimeoutSeconds { get; set; } = 60;

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000A572 File Offset: 0x00008772
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000A57A File Offset: 0x0000877A
		public int SshTimeoutSeconds { get; set; } = 30;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A583 File Offset: 0x00008783
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000A58B File Offset: 0x0000878B
		public int SerialTimeoutSeconds { get; set; } = 15;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A594 File Offset: 0x00008794
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000A59C File Offset: 0x0000879C
		public int SerialCharGapMs { get; set; } = 100;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000A5A5 File Offset: 0x000087A5
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000A5AD File Offset: 0x000087AD
		public int MaxConcurrency { get; set; } = 8;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000A5B6 File Offset: 0x000087B6
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000A5BE File Offset: 0x000087BE
		public bool CoreLiveEnabled { get; set; } = true;

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000A5C7 File Offset: 0x000087C7
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000A5CF File Offset: 0x000087CF
		public int CoreLiveIntervalSeconds { get; set; } = 5;

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000A5D8 File Offset: 0x000087D8
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public int CoreLiveBudgetSeconds { get; set; } = 4;

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000A5E9 File Offset: 0x000087E9
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000A5F1 File Offset: 0x000087F1
		public int CoreLiveReconnectSeconds { get; set; } = 15;

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000A5FA File Offset: 0x000087FA
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000A602 File Offset: 0x00008802
		public int CoreLivePingSeconds { get; set; } = 15;

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000A60B File Offset: 0x0000880B
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000A613 File Offset: 0x00008813
		public int HistoryHours { get; set; } = 24;

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A61C File Offset: 0x0000881C
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000A624 File Offset: 0x00008824
		public int HistoryPerDevice { get; set; } = 5000;
	}
}
