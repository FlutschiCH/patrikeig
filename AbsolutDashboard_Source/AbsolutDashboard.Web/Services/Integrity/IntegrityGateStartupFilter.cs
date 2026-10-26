using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace AbsolutDashboard.Web.Services.Integrity
{
	// Token: 0x0200002D RID: 45
	internal sealed class IntegrityGateStartupFilter : IStartupFilter
	{
		// Token: 0x06000125 RID: 293 RVA: 0x000065D3 File Offset: 0x000047D3
		[NullableContext(1)]
		public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
		{
			return delegate(IApplicationBuilder app)
			{
				app.UseMiddleware<IntegrityGateMiddleware>(Array.Empty<object>());
				next(app);
			};
		}
	}
}
