using System;
using AbsolutDashboard.Web.Components.Account.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000B9 RID: 185
	[ExcludeFromInteractiveRouting]
	[Route("/Account/InvalidUser")]
	public class InvalidUser : ComponentBase
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x0002D958 File Offset: 0x0002BB58
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Invalid user");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<h3>Invalid user</h3>\n\n");
			__builder.OpenComponent<StatusMessage>(5);
			__builder.CloseComponent();
		}
	}
}
