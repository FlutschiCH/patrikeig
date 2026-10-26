using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Routing
{
	// Token: 0x02000023 RID: 35
	internal static class IdentityComponentsEndpointRouteBuilderExtensions
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00005848 File Offset: 0x00003A48
		[NullableContext(1)]
		public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
		{
			IdentityComponentsEndpointRouteBuilderExtensions.<>c__DisplayClass0_0 CS$<>8__locals1 = new IdentityComponentsEndpointRouteBuilderExtensions.<>c__DisplayClass0_0();
			ArgumentNullException.ThrowIfNull(endpoints, "endpoints");
			RouteGroupBuilder routeGroupBuilder = endpoints.MapGroup("/Account");
			routeGroupBuilder.MapPost("/Logout", new Func<ClaimsPrincipal, SignInManager<ApplicationUser>, string, Task<RedirectHttpResult>>(async delegate(ClaimsPrincipal user, [FromServices] SignInManager<ApplicationUser> signInManager, [FromForm] string returnUrl)
			{
				await signInManager.SignOutAsync();
				return TypedResults.LocalRedirect("~/" + returnUrl, false, false);
			}));
			IEndpointRouteBuilder endpointRouteBuilder = routeGroupBuilder.MapGroup("/Manage").RequireAuthorization<RouteGroupBuilder>();
			ILoggerFactory requiredService = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();
			CS$<>8__locals1.downloadLogger = requiredService.CreateLogger("DownloadPersonalData");
			endpointRouteBuilder.MapPost("/DownloadPersonalData", new Func<HttpContext, UserManager<ApplicationUser>, AuthenticationStateProvider, Task<IResult>>(delegate(HttpContext context, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] AuthenticationStateProvider authenticationStateProvider)
			{
				IdentityComponentsEndpointRouteBuilderExtensions.<>c__DisplayClass0_0.<<MapAdditionalIdentityEndpoints>b__1>d <<MapAdditionalIdentityEndpoints>b__1>d;
				<<MapAdditionalIdentityEndpoints>b__1>d.<>t__builder = AsyncTaskMethodBuilder<IResult>.Create();
				<<MapAdditionalIdentityEndpoints>b__1>d.<>4__this = CS$<>8__locals1;
				<<MapAdditionalIdentityEndpoints>b__1>d.context = context;
				<<MapAdditionalIdentityEndpoints>b__1>d.userManager = userManager;
				<<MapAdditionalIdentityEndpoints>b__1>d.<>1__state = -1;
				<<MapAdditionalIdentityEndpoints>b__1>d.<>t__builder.Start<IdentityComponentsEndpointRouteBuilderExtensions.<>c__DisplayClass0_0.<<MapAdditionalIdentityEndpoints>b__1>d>(ref <<MapAdditionalIdentityEndpoints>b__1>d);
				return <<MapAdditionalIdentityEndpoints>b__1>d.<>t__builder.Task;
			}));
			return routeGroupBuilder;
		}
	}
}
