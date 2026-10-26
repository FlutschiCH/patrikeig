using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000B8 RID: 184
	[ExcludeFromInteractiveRouting]
	[Route("/Account/AccessDenied")]
	public class AccessDenied : ComponentBase
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x0002D8F0 File Offset: 0x0002BAF0
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Access denied");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<header><h1 class=\"text-danger\">Access denied</h1>\n    <p class=\"text-danger\">You do not have access to this resource.</p></header>");
		}
	}
}
