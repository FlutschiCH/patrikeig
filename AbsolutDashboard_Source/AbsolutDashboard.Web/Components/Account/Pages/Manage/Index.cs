using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C5 RID: 197
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage")]
	public class Index : ComponentBase
	{
		// Token: 0x06000859 RID: 2137 RVA: 0x000316FC File Offset: 0x0002F8FC
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Profile");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "div");
			__builder.AddAttribute(5, "class", "space-y-6");
			__builder.AddMarkupContent(6, "<div><h2 class=\"text-2xl font-semibold text-cocoa-900\">Profile</h2>\n        <p class=\"mt-1 text-sm text-cocoa-700\">Your account details.</p></div>\n\n    ");
			__builder.OpenComponent<StatusMessage>(7);
			__builder.CloseComponent();
			__builder.AddMarkupContent(8, "\n\n    ");
			__builder.OpenElement(9, "div");
			__builder.AddAttribute(10, "class", "space-y-5");
			__builder.OpenElement(11, "div");
			__builder.AddMarkupContent(12, "<label for=\"username\" class=\"block text-sm font-medium text-cocoa-900\">Username</label>\n            ");
			__builder.OpenElement(13, "input");
			__builder.AddAttribute(14, "type", "text");
			__builder.AddAttribute(15, "value", this.username);
			__builder.AddAttribute(16, "id", "username");
			__builder.AddAttribute(17, "class", "input bg-cream-100 text-cocoa-700");
			__builder.AddAttribute(18, "disabled");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(19, "\n\n        ");
			__builder.OpenElement(20, "div");
			__builder.AddMarkupContent(21, "<label for=\"email\" class=\"block text-sm font-medium text-cocoa-900\">Email</label>\n            ");
			__builder.OpenElement(22, "input");
			__builder.AddAttribute(23, "type", "text");
			__builder.AddAttribute(24, "value", this.email);
			__builder.AddAttribute(25, "id", "email");
			__builder.AddAttribute(26, "class", "input bg-cream-100 text-cocoa-700");
			__builder.AddAttribute(27, "disabled");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(28, "\n\n        ");
			__builder.AddMarkupContent(29, "<p class=\"text-sm text-cocoa-700\">\n            Use <a class=\"text-blush-700 hover:underline\" href=\"Account/Manage/ChangePassword\">Password</a> to change your\n            password, or <a class=\"text-blush-700 hover:underline\" href=\"Account/Manage/TwoFactorAuthentication\">Two-factor\n            authentication</a> to secure your sign-in. To change your email, contact an administrator.\n        </p>");
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000318F3 File Offset: 0x0002FAF3
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x000318FB File Offset: 0x0002FAFB
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x0600085C RID: 2140 RVA: 0x00031904 File Offset: 0x0002FB04
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			this.username = await this.UserManager.GetUserNameAsync(this.user);
			this.email = await this.UserManager.GetEmailAsync(this.user);
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00031947 File Offset: 0x0002FB47
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0003194F File Offset: 0x0002FB4F
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00031958 File Offset: 0x0002FB58
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00031960 File Offset: 0x0002FB60
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x040003A0 RID: 928
		private ApplicationUser user;

		// Token: 0x040003A1 RID: 929
		[Nullable(2)]
		private string username;

		// Token: 0x040003A2 RID: 930
		[Nullable(2)]
		private string email;
	}
}
