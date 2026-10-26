using System;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Route("/Account/Lockout")]
	public class Lockout : ComponentBase
	{
		// Token: 0x0600079A RID: 1946 RVA: 0x0002D9CC File Offset: 0x0002BBCC
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Konto deaktiviert" : "Account deactivated");
				__builder2.AddMarkupContent(3, " — Absolut Dashboard");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(4, "\n\n");
			__builder.OpenElement(5, "section");
			__builder.AddAttribute(6, "class", "mx-auto max-w-md px-4 py-12 text-center");
			__builder.OpenElement(7, "h1");
			__builder.AddAttribute(8, "class", "text-3xl text-blush-600");
			__builder.AddContent(9, Loc.IsGerman ? "Konto deaktiviert" : "Account deactivated");
			__builder.CloseElement();
			__builder.AddMarkupContent(10, "\n\n    ");
			__builder.OpenElement(11, "div");
			__builder.AddAttribute(12, "class", "card mt-6 p-8");
			__builder.AddMarkupContent(13, "<span class=\"mx-auto flex h-14 w-14 items-center justify-center rounded-full bg-cream-200 text-blush-600 ring-1 ring-blush-200\"><svg class=\"h-7 w-7\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" viewBox=\"0 0 24 24\" aria-hidden=\"true\"><path stroke-linecap=\"round\" stroke-linejoin=\"round\" d=\"M16 12V8a4 4 0 1 0-8 0m-1 4h10a1 1 0 0 1 1 1v6a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1v-6a1 1 0 0 1 1-1Z\"></path></svg></span>\n        ");
			__builder.OpenElement(14, "h2");
			__builder.AddAttribute(15, "class", "mt-5 text-xl text-cocoa-900");
			__builder.AddContent(16, Loc.IsGerman ? "Dein Konto ist derzeit deaktiviert" : "Your account is currently deactivated");
			__builder.CloseElement();
			__builder.AddMarkupContent(17, "\n        ");
			__builder.OpenElement(18, "p");
			__builder.AddAttribute(19, "class", "mt-2 text-sm text-cocoa-700");
			__builder.AddContent(20, Loc.IsGerman ? "Bitte wende dich an die Administratorin, um dein Konto wieder zu aktivieren." : "Please get in touch with the administrator to have it reactivated.");
			__builder.CloseElement();
			if (!string.IsNullOrWhiteSpace(this._contactEmail))
			{
				__builder.OpenElement(21, "a");
				__builder.AddAttribute(22, "class", "btn-primary mt-6 w-full");
				__builder.AddAttribute(23, "href", "mailto:" + this._contactEmail);
				__builder.AddContent(24, Loc.IsGerman ? "Administratorin kontaktieren" : "Contact the administrator");
				__builder.CloseElement();
			}
			__builder.OpenElement(25, "a");
			__builder.AddAttribute(26, "class", "btn-secondary mt-3 w-full");
			__builder.AddAttribute(27, "href");
			__builder.AddContent(28, Loc.IsGerman ? "Zur Startseite" : "Back to home");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002DC01 File Offset: 0x0002BE01
		protected override void OnInitialized()
		{
			this._contactEmail = this.Config["Email:OwnerNotificationEmail"];
			if (string.IsNullOrWhiteSpace(this._contactEmail))
			{
				this._contactEmail = this.Config["Email:FromEmail"];
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0002DC3C File Offset: 0x0002BE3C
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0002DC44 File Offset: 0x0002BE44
		[Inject]
		private IConfiguration Config { get; set; }

		// Token: 0x0400034C RID: 844
		[Nullable(2)]
		private string _contactEmail;
	}
}
