using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/TwoFactorAuthentication")]
	public class TwoFactorAuthentication : ComponentBase
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x0003258C File Offset: 0x0003078C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Zwei-Faktor-Authentifizierung" : "Two-factor authentication (2FA)");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "div");
			__builder.AddAttribute(5, "class", "space-y-6");
			__builder.OpenElement(6, "div");
			__builder.OpenElement(7, "h2");
			__builder.AddAttribute(8, "class", "font-display text-2xl text-cocoa-900");
			__builder.AddContent(9, Loc.IsGerman ? "Zwei-Faktor-Authentifizierung" : "Two-factor authentication (2FA)");
			__builder.CloseElement();
			__builder.AddMarkupContent(10, "\n        ");
			__builder.OpenElement(11, "p");
			__builder.AddAttribute(12, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(13, Loc.IsGerman ? "Schütze dein Konto mit einer zusätzlichen Sicherheitsebene." : "Add an extra layer of security to your account.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(14, "\n\n    ");
			__builder.OpenComponent<StatusMessage>(15);
			__builder.CloseComponent();
			if (this.canTrack)
			{
				if (this.is2faEnabled)
				{
					if (this.recoveryCodesLeft == 0)
					{
						__builder.OpenElement(16, "div");
						__builder.AddAttribute(17, "class", "rounded-xl bg-red-50 px-4 py-3 text-sm text-red-700 ring-1 ring-red-200");
						__builder.OpenElement(18, "strong");
						__builder.AddContent(19, Loc.IsGerman ? "Du hast keine Wiederherstellungscodes mehr." : "You have no recovery codes left.");
						__builder.CloseElement();
						__builder.AddMarkupContent(20, "\n                    ");
						__builder.OpenElement(21, "p");
						__builder.AddAttribute(22, "class", "mt-1");
						__builder.AddContent(23, Loc.IsGerman ? "Du musst" : "You must");
						__builder.AddMarkupContent(24, "\n                        ");
						__builder.OpenElement(25, "a");
						__builder.AddAttribute(26, "class", "font-medium underline");
						__builder.AddAttribute(27, "href", "Account/Manage/GenerateRecoveryCodes");
						__builder.AddContent(28, Loc.IsGerman ? "neue Wiederherstellungscodes erzeugen" : "generate a new set of recovery codes");
						__builder.CloseElement();
						__builder.AddMarkupContent(29, ",\n                        ");
						__builder.AddContent(30, Loc.IsGerman ? "bevor du dich mit einem Wiederherstellungscode anmelden kannst." : "before you can log in with a recovery code.");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					else if (this.recoveryCodesLeft == 1)
					{
						__builder.OpenElement(31, "div");
						__builder.AddAttribute(32, "class", "rounded-xl bg-red-50 px-4 py-3 text-sm text-red-700 ring-1 ring-red-200");
						__builder.OpenElement(33, "strong");
						__builder.AddContent(34, Loc.IsGerman ? "Du hast noch 1 Wiederherstellungscode." : "You have 1 recovery code left.");
						__builder.CloseElement();
						__builder.AddMarkupContent(35, "\n                    ");
						__builder.OpenElement(36, "p");
						__builder.AddAttribute(37, "class", "mt-1");
						__builder.AddContent(38, Loc.IsGerman ? "Du kannst" : "You can");
						__builder.AddMarkupContent(39, "\n                        ");
						__builder.OpenElement(40, "a");
						__builder.AddAttribute(41, "class", "font-medium underline");
						__builder.AddAttribute(42, "href", "Account/Manage/GenerateRecoveryCodes");
						__builder.AddContent(43, Loc.IsGerman ? "neue Wiederherstellungscodes erzeugen" : "generate a new set of recovery codes");
						__builder.CloseElement();
						__builder.AddContent(44, ".");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					else if (this.recoveryCodesLeft <= 3)
					{
						__builder.OpenElement(45, "div");
						__builder.AddAttribute(46, "class", "rounded-xl bg-amber-50 px-4 py-3 text-sm text-amber-800 ring-1 ring-amber-200");
						__builder.OpenElement(47, "strong");
						int num = 48;
						string text;
						if (!Loc.IsGerman)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
							defaultInterpolatedStringHandler.AppendLiteral("You have ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(this.recoveryCodesLeft);
							defaultInterpolatedStringHandler.AppendLiteral(" recovery codes left.");
							text = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(38, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("Du hast noch ");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(this.recoveryCodesLeft);
							defaultInterpolatedStringHandler2.AppendLiteral(" Wiederherstellungscodes.");
							text = defaultInterpolatedStringHandler2.ToStringAndClear();
						}
						__builder.AddContent(num, text);
						__builder.CloseElement();
						__builder.AddMarkupContent(49, "\n                    ");
						__builder.OpenElement(50, "p");
						__builder.AddAttribute(51, "class", "mt-1");
						__builder.AddContent(52, Loc.IsGerman ? "Du solltest" : "You should");
						__builder.AddMarkupContent(53, "\n                        ");
						__builder.OpenElement(54, "a");
						__builder.AddAttribute(55, "class", "font-medium underline");
						__builder.AddAttribute(56, "href", "Account/Manage/GenerateRecoveryCodes");
						__builder.AddContent(57, Loc.IsGerman ? "neue Wiederherstellungscodes erzeugen" : "generate a new set of recovery codes");
						__builder.CloseElement();
						__builder.AddContent(58, ".");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					__builder.OpenElement(59, "div");
					__builder.AddAttribute(60, "class", "flex flex-wrap gap-3");
					if (this.isMachineRemembered)
					{
						__builder.OpenElement(61, "form");
						string text2 = RuntimeHelpers.TypeCheck<string>("forget-browser");
						__builder.AddAttribute<EventArgs>(62, "onsubmit", EventCallback.Factory.Create<EventArgs>(this, new Func<Task>(this.OnSubmitForgetBrowserAsync)));
						__builder.AddAttribute(63, "method", "post");
						__builder.AddNamedEvent("onsubmit", text2);
						__builder.OpenComponent<AntiforgeryToken>(64);
						__builder.CloseComponent();
						__builder.AddMarkupContent(65, "\n                        ");
						__builder.OpenElement(66, "button");
						__builder.AddAttribute(67, "type", "submit");
						__builder.AddAttribute(68, "class", "btn-secondary");
						__builder.AddContent(69, Loc.IsGerman ? "Diesen Browser vergessen" : "Forget this browser");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					__builder.OpenElement(70, "a");
					__builder.AddAttribute(71, "href", "Account/Manage/Disable2fa");
					__builder.AddAttribute(72, "class", "btn-secondary");
					__builder.AddContent(73, Loc.IsGerman ? "2FA deaktivieren" : "Disable 2FA");
					__builder.CloseElement();
					__builder.AddMarkupContent(74, "\n                ");
					__builder.OpenElement(75, "a");
					__builder.AddAttribute(76, "href", "Account/Manage/GenerateRecoveryCodes");
					__builder.AddAttribute(77, "class", "btn-secondary");
					__builder.AddContent(78, Loc.IsGerman ? "Wiederherstellungscodes zurücksetzen" : "Reset recovery codes");
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.OpenElement(79, "div");
				__builder.AddAttribute(80, "class", "border-t border-blush-100 pt-6");
				__builder.OpenElement(81, "h3");
				__builder.AddAttribute(82, "class", "font-display text-lg text-cocoa-900");
				__builder.AddContent(83, Loc.IsGerman ? "Authenticator-App" : "Authenticator app");
				__builder.CloseElement();
				__builder.AddMarkupContent(84, "\n            ");
				__builder.OpenElement(85, "div");
				__builder.AddAttribute(86, "class", "mt-3 flex flex-wrap gap-3");
				if (!this.hasAuthenticator)
				{
					__builder.OpenElement(87, "a");
					__builder.AddAttribute(88, "href", "Account/Manage/EnableAuthenticator");
					__builder.AddAttribute(89, "class", "btn-primary");
					__builder.AddContent(90, Loc.IsGerman ? "Authenticator-App hinzufügen" : "Add authenticator app");
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(91, "a");
					__builder.AddAttribute(92, "href", "Account/Manage/EnableAuthenticator");
					__builder.AddAttribute(93, "class", "btn-primary");
					__builder.AddContent(94, Loc.IsGerman ? "Authenticator-App einrichten" : "Set up authenticator app");
					__builder.CloseElement();
					__builder.AddMarkupContent(95, "\n                    ");
					__builder.OpenElement(96, "a");
					__builder.AddAttribute(97, "href", "Account/Manage/ResetAuthenticator");
					__builder.AddAttribute(98, "class", "btn-secondary");
					__builder.AddContent(99, Loc.IsGerman ? "Authenticator-App zurücksetzen" : "Reset authenticator app");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
			}
			else
			{
				__builder.OpenElement(100, "div");
				__builder.AddAttribute(101, "class", "rounded-xl bg-red-50 px-4 py-3 text-sm text-red-700 ring-1 ring-red-200");
				__builder.OpenElement(102, "strong");
				__builder.AddContent(103, Loc.IsGerman ? "Datenschutz- und Cookie-Richtlinie wurden nicht akzeptiert." : "Privacy and cookie policy have not been accepted.");
				__builder.CloseElement();
				__builder.AddMarkupContent(104, "\n            ");
				__builder.OpenElement(105, "p");
				__builder.AddAttribute(106, "class", "mt-1");
				__builder.AddContent(107, Loc.IsGerman ? "Du musst die Richtlinie akzeptieren, bevor du die Zwei-Faktor-Authentifizierung aktivieren kannst." : "You must accept the policy before you can enable two factor authentication.");
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00032E96 File Offset: 0x00031096
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x00032E9E File Offset: 0x0003109E
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x0600088F RID: 2191 RVA: 0x00032EA8 File Offset: 0x000310A8
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			ApplicationUser user = applicationUser;
			ITrackingConsentFeature trackingConsentFeature = this.HttpContext.Features.Get<ITrackingConsentFeature>();
			this.canTrack = trackingConsentFeature == null || trackingConsentFeature.CanTrack;
			this.hasAuthenticator = await this.UserManager.GetAuthenticatorKeyAsync(user) != null;
			this.is2faEnabled = await this.UserManager.GetTwoFactorEnabledAsync(user);
			this.isMachineRemembered = await this.SignInManager.IsTwoFactorClientRememberedAsync(user);
			this.recoveryCodesLeft = await this.UserManager.CountRecoveryCodesAsync(user);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00032EEC File Offset: 0x000310EC
		private async Task OnSubmitForgetBrowserAsync()
		{
			await this.SignInManager.ForgetTwoFactorClientAsync();
			this.RedirectManager.RedirectToCurrentPageWithStatus("The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.", this.HttpContext);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00032F2F File Offset: 0x0003112F
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x00032F37 File Offset: 0x00031137
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00032F40 File Offset: 0x00031140
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x00032F48 File Offset: 0x00031148
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00032F51 File Offset: 0x00031151
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x00032F59 File Offset: 0x00031159
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00032F62 File Offset: 0x00031162
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x00032F6A File Offset: 0x0003116A
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x040003B7 RID: 951
		private bool canTrack;

		// Token: 0x040003B8 RID: 952
		private bool hasAuthenticator;

		// Token: 0x040003B9 RID: 953
		private int recoveryCodesLeft;

		// Token: 0x040003BA RID: 954
		private bool is2faEnabled;

		// Token: 0x040003BB RID: 955
		private bool isMachineRemembered;
	}
}
