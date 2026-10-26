using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	public interface ISshOutputParser
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000262 RID: 610
		string Key { get; }

		// Token: 0x06000263 RID: 611
		DeviceSnapshot Parse(IReadOnlyDictionary<string, string> commandOutputs);
	}
}
