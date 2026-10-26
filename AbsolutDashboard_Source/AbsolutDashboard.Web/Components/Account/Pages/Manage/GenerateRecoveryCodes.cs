using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020000C4 RID: 196
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/GenerateRecoveryCodes")]
	public class GenerateRecoveryCodes : ComponentBase
	{
		// Token: 0x0600084B RID: 2123 RVA: 0x000314D4 File Offset: 0x0002F6D4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Generate two-factor authentication (2FA) recovery codes");
			}));
			__builder.CloseComponent();
			if (this.recoveryCodes != null)
			{
				__builder.OpenComponent<ShowRecoveryCodes>(3);
				__builder.AddComponentParameter(4, "RecoveryCodes", RuntimeHelpers.TypeCheck<string[]>(this.recoveryCodes.ToArray<string>()));
				__builder.AddComponentParameter(5, "StatusMessage", RuntimeHelpers.TypeCheck<string>(this.message));
				__builder.CloseComponent();
				return;
			}
			__builder.AddMarkupContent(6, "<h3>Generate two-factor authentication (2FA) recovery codes</h3>\n    ");
			__builder.AddMarkupContent(7, "<div class=\"alert alert-warning\" role=\"alert\"><p><span class=\"glyphicon glyphicon-warning-sign\"></span>\n            <strong>Put these codes in a safe place.</strong></p>\n        <p>\n            If you lose your device and don't have the recovery codes you will lose access to your account.\n        </p>\n        <p>\n            Generating new recovery codes does not change the keys used in authenticator apps. If you wish to change the key\n            used in an authenticator app you should <a href=\"Account/Manage/ResetAuthenticator\">reset your authenticator keys.</a></p></div>\n    ");
			__builder.OpenElement(8, "div");
			__builder.OpenElement(9, "form");
			string text = RuntimeHelpers.TypeCheck<string>("generate-recovery-codes");
			__builder.AddAttribute<EventArgs>(10, "onsubmit", EventCallback.Factory.Create<EventArgs>(this, new Func<Task>(this.OnSubmitAsync)));
			__builder.AddAttribute(11, "method", "post");
			__builder.AddNamedEvent("onsubmit", text);
			__builder.OpenComponent<AntiforgeryToken>(12);
			__builder.CloseComponent();
			__builder.AddMarkupContent(13, "\n            ");
			__builder.AddMarkupContent(14, "<button class=\"btn btn-danger\" type=\"submit\">Generate Recovery Codes</button>");
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00031614 File Offset: 0x0002F814
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0003161C File Offset: 0x0002F81C
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x0600084E RID: 2126 RVA: 0x00031628 File Offset: 0x0002F828
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			TaskAwaiter<bool> taskAwaiter = this.UserManager.GetTwoFactorEnabledAsync(this.user).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				throw new InvalidOperationException("Cannot generate recovery codes for user because they do not have 2FA enabled.");
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0003166C File Offset: 0x0002F86C
		private async Task OnSubmitAsync()
		{
			string text = await this.UserManager.GetUserIdAsync(this.user);
			string userId = text;
			this.recoveryCodes = await this.UserManager.GenerateNewTwoFactorRecoveryCodesAsync(this.user, 10);
			this.message = "You have generated new recovery codes.";
			this.Logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", new object[] { userId });
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x000316AF File Offset: 0x0002F8AF
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x000316B7 File Offset: 0x0002F8B7
		[Inject]
		private ILogger<GenerateRecoveryCodes> Logger { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x000316C0 File Offset: 0x0002F8C0
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x000316C8 File Offset: 0x0002F8C8
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x000316D1 File Offset: 0x0002F8D1
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x000316D9 File Offset: 0x0002F8D9
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x000316E2 File Offset: 0x0002F8E2
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x000316EA File Offset: 0x0002F8EA
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x04000398 RID: 920
		[Nullable(2)]
		private string message;

		// Token: 0x04000399 RID: 921
		private ApplicationUser user;

		// Token: 0x0400039A RID: 922
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<string> recoveryCodes;
	}
}
