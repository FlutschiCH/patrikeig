using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AbsolutDashboard.Web.Services.Integrity
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IntegrityGateMiddleware
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006571 File Offset: 0x00004771
		public IntegrityGateMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006580 File Offset: 0x00004780
		public async Task InvokeAsync(HttpContext context, IntegrityGate gate)
		{
			gate.Touch();
			if (gate.IsBlocked)
			{
				context.Response.StatusCode = 500;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync("500 - Internal Server Error", default(CancellationToken));
			}
			else
			{
				await this._next(context);
			}
		}

		// Token: 0x0400008B RID: 139
		private readonly RequestDelegate _next;
	}
}
