using System;
using AbsolutDashboard.Web.Components.Layout;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components.Account.Shared
{
	// Token: 0x020000B3 RID: 179
	[Layout(typeof(MainLayout))]
	public class ManageLayout : LayoutComponentBase
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0002D314 File Offset: 0x0002B514
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "section");
			__builder.AddAttribute(1, "class", "mx-auto max-w-6xl px-4 py-12");
			__builder.OpenElement(2, "header");
			__builder.AddAttribute(3, "class", "mb-8");
			__builder.OpenElement(4, "h1");
			__builder.AddAttribute(5, "class", "font-display text-3xl text-blush-600");
			__builder.AddContent(6, Loc.IsGerman ? "Mein Konto" : "My account");
			__builder.CloseElement();
			__builder.AddMarkupContent(7, "\n        ");
			__builder.OpenElement(8, "p");
			__builder.AddAttribute(9, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(10, Loc.IsGerman ? "Verwalte deine Kontoeinstellungen und Daten." : "Manage your account settings and data.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(11, "\n\n    ");
			__builder.OpenElement(12, "div");
			__builder.AddAttribute(13, "class", "grid gap-8 md:grid-cols-[16rem_1fr]");
			__builder.OpenElement(14, "aside");
			__builder.OpenComponent<ManageNavMenu>(15);
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.AddMarkupContent(16, "\n        ");
			__builder.OpenElement(17, "div");
			__builder.AddAttribute(18, "class", "card p-6 sm:p-8");
			__builder.AddContent(19, base.Body);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}
	}
}
