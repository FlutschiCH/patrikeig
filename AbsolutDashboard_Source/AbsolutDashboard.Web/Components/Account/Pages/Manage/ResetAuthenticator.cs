using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
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
	// Token: 0x020000C7 RID: 199
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/ResetAuthenticator")]
	public class ResetAuthenticator : ComponentBase
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x00031D38 File Offset: 0x0002FF38
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Reset authenticator key");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenComponent<StatusMessage>(4);
			__builder.CloseComponent();
			__builder.AddMarkupContent(5, "\n");
			__builder.AddMarkupContent(6, "<h3>Reset authenticator key</h3>\n");
			__builder.AddMarkupContent(7, "<div class=\"alert alert-warning\" role=\"alert\"><p><span class=\"glyphicon glyphicon-warning-sign\"></span>\n        <strong>If you reset your authenticator key your authenticator app will not work until you reconfigure it.</strong></p>\n    <p>\n        This process disables 2FA until you verify your authenticator app.\n        If you do not complete your authenticator app configuration you may lose access to your account.\n    </p></div>\n");
			__builder.OpenElement(8, "div");
			__builder.OpenElement(9, "form");
			string text = RuntimeHelpers.TypeCheck<string>("reset-authenticator");
			__builder.AddAttribute<EventArgs>(10, "onsubmit", EventCallback.Factory.Create<EventArgs>(this, new Func<Task>(this.OnSubmitAsync)));
			__builder.AddAttribute(11, "method", "post");
			__builder.AddNamedEvent("onsubmit", text);
			__builder.OpenComponent<AntiforgeryToken>(12);
			__builder.CloseComponent();
			__builder.AddMarkupContent(13, "\n        ");
			__builder.AddMarkupContent(14, "<button class=\"btn btn-danger\" type=\"submit\">Reset authenticator key</button>");
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00031E54 File Offset: 0x00030054
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00031E5C File Offset: 0x0003005C
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x0600086C RID: 2156 RVA: 0x00031E68 File Offset: 0x00030068
		private async Task OnSubmitAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			ApplicationUser user = applicationUser;
			await this.UserManager.SetTwoFactorEnabledAsync(user, false);
			await this.UserManager.ResetAuthenticatorKeyAsync(user);
			string text = await this.UserManager.GetUserIdAsync(user);
			this.Logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", new object[] { text });
			await this.SignInManager.RefreshSignInAsync(user);
			this.RedirectManager.RedirectToWithStatus("Account/Manage/EnableAuthenticator", "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.", this.HttpContext);
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x00031EAB File Offset: 0x000300AB
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x00031EB3 File Offset: 0x000300B3
		[Inject]
		private ILogger<ResetAuthenticator> Logger { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00031EBC File Offset: 0x000300BC
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00031EC4 File Offset: 0x000300C4
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00031ECD File Offset: 0x000300CD
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x00031ED5 File Offset: 0x000300D5
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x00031EDE File Offset: 0x000300DE
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x00031EE6 File Offset: 0x000300E6
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00031EEF File Offset: 0x000300EF
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x00031EF7 File Offset: 0x000300F7
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }
	}
}
