using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Operations
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShutdownAllOptions
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000616B File Offset: 0x0000436B
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00006173 File Offset: 0x00004373
		public List<OperationStep> Steps { get; set; } = new List<OperationStep>();

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000617C File Offset: 0x0000437C
		public bool Configured
		{
			get
			{
				if (this.Steps.Count > 0)
				{
					return this.Steps.All<OperationStep>((OperationStep s) => s.IsValid);
				}
				return false;
			}
		}
	}
}
