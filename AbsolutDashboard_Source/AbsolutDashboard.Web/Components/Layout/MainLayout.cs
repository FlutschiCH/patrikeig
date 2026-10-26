using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components.Layout
{
	// Token: 0x0200009E RID: 158
	public class MainLayout : LayoutComponentBase
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x00020D94 File Offset: 0x0001EF94
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<AuthorizeView>(0);
			__builder.AddAttribute(1, "Authorized", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(2, "div");
				__builder2.AddAttribute(3, "class", "flex min-h-screen bg-cream-100");
				__builder2.AddAttribute(4, "b-oawuskbhm5");
				__builder2.OpenComponent<NavMenu>(5);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(6, "\n            ");
				__builder2.OpenElement(7, "main");
				__builder2.AddAttribute(8, "class", "min-w-0 flex-1 px-4 py-6 sm:px-8");
				__builder2.AddAttribute(9, "b-oawuskbhm5");
				__builder2.AddContent(10, base.Body);
				__builder2.CloseElement();
				__builder2.CloseElement();
			}));
			__builder.AddAttribute(11, "NotAuthorized", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(12, "div");
				__builder2.AddAttribute(13, "class", "min-h-screen bg-cream-100");
				__builder2.AddAttribute(14, "b-oawuskbhm5");
				__builder2.AddContent(15, base.Body);
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(16, "\n\n");
			__builder.AddMarkupContent(17, "<div id=\"blazor-error-ui\" data-nosnippet class=\"fixed bottom-0 left-0 z-50 hidden w-full bg-blush-700 px-4 py-3 text-center text-sm text-white\" b-oawuskbhm5>\n    An unhandled error has occurred.\n    <a href=\".\" class=\"reload underline\" b-oawuskbhm5>Reload</a>\n    <span class=\"dismiss ml-2 cursor-pointer\" b-oawuskbhm5>\ud83d\uddd9</span></div>");
		}
	}
}
