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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C2 RID: 194
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/Disable2fa")]
	public class Disable2fa : ComponentBase
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x000306C0 File Offset: 0x0002E8C0
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "2FA deaktivieren" : "Disable two-factor authentication (2FA)");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "div");
			__builder.AddAttribute(5, "class", "space-y-6");
			__builder.OpenElement(6, "div");
			__builder.OpenElement(7, "h2");
			__builder.AddAttribute(8, "class", "font-display text-2xl text-cocoa-900");
			__builder.AddContent(9, Loc.IsGerman ? "Zwei-Faktor-Authentifizierung deaktivieren" : "Disable two-factor authentication");
			__builder.CloseElement();
			__builder.AddMarkupContent(10, "\n        ");
			__builder.OpenElement(11, "p");
			__builder.AddAttribute(12, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(13, Loc.IsGerman ? "Schalte 2FA für dein Konto aus." : "Turn off 2FA for your account.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(14, "\n\n    ");
			__builder.OpenComponent<StatusMessage>(15);
			__builder.CloseComponent();
			__builder.AddMarkupContent(16, "\n\n    ");
			__builder.OpenElement(17, "div");
			__builder.AddAttribute(18, "class", "space-y-2 rounded-xl bg-amber-50 px-4 py-3 text-sm text-amber-800 ring-1 ring-amber-200");
			__builder.AddAttribute(19, "role", "alert");
			__builder.OpenElement(20, "p");
			__builder.OpenElement(21, "strong");
			__builder.AddContent(22, Loc.IsGerman ? "Diese Aktion deaktiviert lediglich 2FA." : "This action only disables 2FA.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(23, "\n        ");
			__builder.OpenElement(24, "p");
			__builder.AddContent(25, Loc.IsGerman ? "Das Deaktivieren ändert nicht die in Authenticator-Apps verwendeten Schlüssel. Wenn du den Schlüssel ändern möchtest, solltest du " : "Disabling 2FA does not change the keys used in authenticator apps. If you wish to change the key used in an authenticator app you should ");
			__builder.OpenElement(26, "a");
			__builder.AddAttribute(27, "class", "font-medium underline");
			__builder.AddAttribute(28, "href", "Account/Manage/ResetAuthenticator");
			__builder.AddContent(29, Loc.IsGerman ? "deine Authenticator-Schlüssel zurücksetzen" : "reset your authenticator keys");
			__builder.CloseElement();
			__builder.AddContent(30, ".");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(31, "\n\n    ");
			__builder.OpenElement(32, "form");
			string text = RuntimeHelpers.TypeCheck<string>("disable-2fa");
			__builder.AddAttribute<EventArgs>(33, "onsubmit", EventCallback.Factory.Create<EventArgs>(this, new Func<Task>(this.OnSubmitAsync)));
			__builder.AddAttribute(34, "method", "post");
			__builder.AddNamedEvent("onsubmit", text);
			__builder.OpenComponent<AntiforgeryToken>(35);
			__builder.CloseComponent();
			__builder.AddMarkupContent(36, "\n        ");
			__builder.OpenElement(37, "button");
			__builder.AddAttribute(38, "class", "btn-danger");
			__builder.AddAttribute(39, "type", "submit");
			__builder.AddContent(40, Loc.IsGerman ? "2FA deaktivieren" : "Disable 2FA");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000309E5 File Offset: 0x0002EBE5
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x000309ED File Offset: 0x0002EBED
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x06000827 RID: 2087 RVA: 0x000309F8 File Offset: 0x0002EBF8
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			bool flag = HttpMethods.IsGet(this.HttpContext.Request.Method);
			if (flag)
			{
				TaskAwaiter<bool> taskAwaiter = this.UserManager.GetTwoFactorEnabledAsync(this.user).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				flag = !taskAwaiter.GetResult();
			}
			if (flag)
			{
				throw new InvalidOperationException("Cannot disable 2FA for user as it's not currently enabled.");
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00030A3C File Offset: 0x0002EC3C
		private async Task OnSubmitAsync()
		{
			TaskAwaiter<IdentityResult> taskAwaiter = this.UserManager.SetTwoFactorEnabledAsync(this.user, false).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<IdentityResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<IdentityResult>);
			}
			if (!taskAwaiter.GetResult().Succeeded)
			{
				throw new InvalidOperationException("Unexpected error occurred disabling 2FA.");
			}
			string text = await this.UserManager.GetUserIdAsync(this.user);
			this.Logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", new object[] { text });
			this.RedirectManager.RedirectToWithStatus("Account/Manage/TwoFactorAuthentication", "2fa has been disabled. You can reenable 2fa when you setup an authenticator app", this.HttpContext);
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00030A7F File Offset: 0x0002EC7F
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x00030A87 File Offset: 0x0002EC87
		[Inject]
		private ILogger<Disable2fa> Logger { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00030A90 File Offset: 0x0002EC90
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x00030A98 File Offset: 0x0002EC98
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00030AA1 File Offset: 0x0002ECA1
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x00030AA9 File Offset: 0x0002ECA9
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00030AB2 File Offset: 0x0002ECB2
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x00030ABA File Offset: 0x0002ECBA
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x04000384 RID: 900
		private ApplicationUser user;
	}
}
