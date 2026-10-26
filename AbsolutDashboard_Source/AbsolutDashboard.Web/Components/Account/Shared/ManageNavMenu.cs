using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace AbsolutDashboard.Web.Components.Account.Shared
{
	// Token: 0x020000B4 RID: 180
	public class ManageNavMenu : ComponentBase
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x0002D48C File Offset: 0x0002B68C
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "nav");
			__builder.AddAttribute(1, "class", "flex flex-col gap-1");
			__builder.OpenComponent<NavLink>(2);
			__builder.AddComponentParameter(3, "class", "acct-nav-link");
			__builder.AddComponentParameter(4, "href", "Account/Manage");
			__builder.AddComponentParameter(5, "Match", RuntimeHelpers.TypeCheck<NavLinkMatch>(NavLinkMatch.All));
			__builder.AddAttribute(6, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(7, "Profile");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(8, "\n    ");
			__builder.OpenComponent<NavLink>(9);
			__builder.AddComponentParameter(10, "class", "acct-nav-link");
			__builder.AddComponentParameter(11, "href", "Account/Manage/ChangePassword");
			__builder.AddAttribute(12, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(13, "Password");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(14, "\n    ");
			__builder.OpenComponent<NavLink>(15);
			__builder.AddComponentParameter(16, "class", "acct-nav-link");
			__builder.AddComponentParameter(17, "href", "Account/Manage/TwoFactorAuthentication");
			__builder.AddAttribute(18, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(19, "Two-factor authentication");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(20, "\n    ");
			__builder.OpenComponent<NavLink>(21);
			__builder.AddComponentParameter(22, "class", "acct-nav-link");
			__builder.AddComponentParameter(23, "href", "Account/Manage/PersonalData");
			__builder.AddAttribute(24, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(25, "Personal data");
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
		}
	}
}
