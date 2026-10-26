using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AbsolutDashboard.Web.Components.Account
{
	// Token: 0x020000B1 RID: 177
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IdentityRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x0002D1EE File Offset: 0x0002B3EE
		public IdentityRevalidatingAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory, IOptions<IdentityOptions> options)
			: base(loggerFactory)
		{
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002D205 File Offset: 0x0002B405
		protected override TimeSpan RevalidationInterval
		{
			get
			{
				return TimeSpan.FromMinutes(30L);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002D210 File Offset: 0x0002B410
		protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
		{
			AsyncServiceScope scope = this.<scopeFactory>P.CreateAsyncScope();
			object obj = null;
			int num = 0;
			bool flag2;
			try
			{
				UserManager<ApplicationUser> requiredService = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				bool flag = await this.ValidateSecurityStampAsync(requiredService, authenticationState.User);
				flag2 = flag;
				num = 1;
			}
			catch (object obj)
			{
			}
			await scope.DisposeAsync();
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return flag2;
			}
			obj = null;
			scope = default(AsyncServiceScope);
			throw null;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002D25C File Offset: 0x0002B45C
		private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
		{
			ApplicationUser applicationUser = await userManager.GetUserAsync(principal);
			bool flag;
			if (applicationUser == null)
			{
				flag = false;
			}
			else if (!userManager.SupportsUserSecurityStamp)
			{
				flag = true;
			}
			else
			{
				string principalStamp = principal.FindFirstValue(this.<options>P.Value.ClaimsIdentity.SecurityStampClaimType);
				flag = principalStamp == await userManager.GetSecurityStampAsync(applicationUser);
			}
			return flag;
		}

		// Token: 0x04000342 RID: 834
		[CompilerGenerated]
		private IServiceScopeFactory <scopeFactory>P = scopeFactory;

		// Token: 0x04000343 RID: 835
		[CompilerGenerated]
		private IOptions<IdentityOptions> <options>P = options;
	}
}
