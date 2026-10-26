using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Operations
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OperationsOptions
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000612B File Offset: 0x0000432B
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00006133 File Offset: 0x00004333
		public ShutdownAllOptions ShutdownAll { get; set; } = new ShutdownAllOptions();

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000613C File Offset: 0x0000433C
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00006144 File Offset: 0x00004344
		public PrruPowerOptions PrruPower { get; set; } = new PrruPowerOptions();
	}
}
