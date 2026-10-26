using System;
using System.Reflection;
using System.Security.Principal;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000099 RID: 153
	public class Routes : ComponentBase
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<Router>(0);
			__builder.AddComponentParameter(1, "AppAssembly", RuntimeHelpers.TypeCheck<Assembly>(typeof(Program).Assembly));
			__builder.AddAttribute(2, "Found", new RenderFragment<RouteData>((RouteData routeData) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<AuthorizeRouteView>(3);
				__builder2.AddComponentParameter(4, "RouteData", RuntimeHelpers.TypeCheck<RouteData>(routeData));
				__builder2.AddComponentParameter(5, "DefaultLayout", RuntimeHelpers.TypeCheck<Type>(typeof(MainLayout)));
				__builder2.AddAttribute(6, "NotAuthorized", new RenderFragment<AuthenticationState>((AuthenticationState authState) => delegate(RenderTreeBuilder __builder3)
				{
					IIdentity identity = authState.User.Identity;
					if (identity != null && identity.IsAuthenticated)
					{
						__builder3.AddMarkupContent(7, "<div class=\"mx-auto max-w-lg px-4 py-16 text-center\"><h1 class=\"text-2xl font-semibold text-cocoa-900\">Access denied</h1>\n                        <p class=\"mt-2 text-cocoa-700\">You don't have permission to view this page.</p>\n                        <a class=\"btn-primary mt-6 !px-5 !py-2.5 text-sm\" href>Back to dashboard</a></div>");
						return;
					}
					__builder3.OpenComponent<RedirectToLogin>(8);
					__builder3.CloseComponent();
				}));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(9, "\n        ");
				__builder2.OpenComponent<FocusOnNavigate>(10);
				__builder2.AddComponentParameter(11, "RouteData", RuntimeHelpers.TypeCheck<RouteData>(routeData));
				__builder2.AddComponentParameter(12, "Selector", "h1");
				__builder2.CloseComponent();
			}));
			__builder.CloseComponent();
		}
	}
}
