using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.LoginWith2fa;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Route("/Account/LoginWith2fa")]
	public class LoginWith2fa : ComponentBase
	{
		// Token: 0x060007B8 RID: 1976 RVA: 0x0002E3F4 File Offset: 0x0002C5F4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Two-factor authentication — Absolut Dashboard");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "section");
			__builder.AddAttribute(5, "class", "mx-auto max-w-md px-4 py-12");
			__builder.AddMarkupContent(6, "<h1 class=\"text-center text-3xl text-blush-600\">Two-factor authentication</h1>\n    ");
			__builder.AddMarkupContent(7, "<p class=\"mt-1 text-center text-sm text-cocoa-700\">Enter the code from your authenticator app to finish signing in.</p>\n\n    ");
			__builder.OpenElement(8, "div");
			__builder.AddAttribute(9, "class", "card mt-6 p-6");
			__builder.OpenComponent<StatusMessage>(10);
			__builder.AddComponentParameter(11, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(12, "\n        ");
			__builder.OpenComponent<EditForm>(13);
			__builder.AddComponentParameter(14, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(15, "FormName", "login-with-2fa");
			__builder.AddComponentParameter(16, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.OnValidSubmitAsync))));
			__builder.AddComponentParameter(17, "method", "post");
			__builder.AddAttribute(18, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(19, "input");
				__builder2.AddAttribute(20, "type", "hidden");
				__builder2.AddAttribute(21, "name", "ReturnUrl");
				__builder2.AddAttribute(22, "value", this.ReturnUrl);
				__builder2.CloseElement();
				__builder2.AddMarkupContent(23, "\n            ");
				__builder2.OpenElement(24, "input");
				__builder2.AddAttribute(25, "type", "hidden");
				__builder2.AddAttribute(26, "name", "RememberMe");
				__builder2.AddAttribute(27, "value", this.RememberMe);
				__builder2.CloseElement();
				__builder2.AddMarkupContent(28, "\n            ");
				__builder2.OpenComponent<DataAnnotationsValidator>(29);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(30, "\n            ");
				__builder2.OpenComponent<ValidationSummary>(31);
				__builder2.AddComponentParameter(32, "class", "mb-3 text-sm text-blush-700");
				__builder2.AddComponentParameter(33, "role", "alert");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(34, "\n            ");
				__builder2.OpenElement(35, "label");
				__builder2.AddAttribute(36, "class", "block");
				__builder2.AddMarkupContent(37, "<span class=\"text-sm font-medium\">Authenticator code</span>\n                ");
				__builder2.OpenComponent<InputText>(38);
				__builder2.AddComponentParameter(39, "class", "input tracking-widest");
				__builder2.AddComponentParameter(40, "autocomplete", "off");
				__builder2.AddComponentParameter(41, "inputmode", "numeric");
				__builder2.AddComponentParameter(42, "placeholder", "123456");
				__builder2.AddComponentParameter(43, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.TwoFactorCode));
				__builder2.AddComponentParameter(44, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.TwoFactorCode = __value;
				}, this.Input.TwoFactorCode))));
				__builder2.AddComponentParameter(45, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(LoginWith2fa)), methodof(LoginWith2fa.get_Input())), methodof(LoginWith2fa.InputModel.get_TwoFactorCode())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(46, "\n                ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 47, 48, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(LoginWith2fa)), methodof(LoginWith2fa.get_Input())), methodof(LoginWith2fa.InputModel.get_TwoFactorCode())), Array.Empty<ParameterExpression>()), 49, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(50, "\n            ");
				__builder2.OpenElement(51, "label");
				__builder2.AddAttribute(52, "class", "mt-4 flex items-center gap-2 text-sm");
				__builder2.OpenComponent<InputCheckbox>(53);
				__builder2.AddComponentParameter(54, "Value", RuntimeHelpers.TypeCheck<bool>(this.Input.RememberMachine));
				__builder2.AddComponentParameter(55, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<bool>>(EventCallback.Factory.Create<bool>(this, RuntimeHelpers.CreateInferredEventCallback<bool>(this, delegate(bool __value)
				{
					this.Input.RememberMachine = __value;
				}, this.Input.RememberMachine))));
				__builder2.AddComponentParameter(56, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<bool>>>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(LoginWith2fa)), methodof(LoginWith2fa.get_Input())), methodof(LoginWith2fa.InputModel.get_RememberMachine())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(57, " Remember this machine\n            ");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(58, "\n            ");
				__builder2.AddMarkupContent(59, "<button type=\"submit\" class=\"btn-primary mt-6 w-full\">Log in</button>");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(60, "\n\n        ");
			__builder.OpenElement(61, "p");
			__builder.AddAttribute(62, "class", "mt-5 text-center text-sm text-cocoa-700");
			__builder.AddMarkupContent(63, "\n            No access to your authenticator?\n            ");
			__builder.OpenElement(64, "a");
			__builder.AddAttribute(65, "class", "text-blush-700 hover:underline");
			__builder.AddAttribute(66, "href", "Account/LoginWithRecoveryCode?ReturnUrl=" + this.ReturnUrl);
			__builder.AddContent(67, "Use a recovery code");
			__builder.CloseElement();
			__builder.AddMarkupContent(68, ".\n        ");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0002E603 File Offset: 0x0002C803
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x0002E60B File Offset: 0x0002C80B
		[SupplyParameterFromForm]
		private LoginWith2fa.InputModel Input { get; set; } = new LoginWith2fa.InputModel();

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0002E614 File Offset: 0x0002C814
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0002E61C File Offset: 0x0002C81C
		[Nullable(2)]
		[SupplyParameterFromQuery]
		private string ReturnUrl
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002E625 File Offset: 0x0002C825
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0002E62D File Offset: 0x0002C82D
		[SupplyParameterFromQuery]
		private bool RememberMe { get; set; }

		// Token: 0x060007BF RID: 1983 RVA: 0x0002E638 File Offset: 0x0002C838
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.SignInManager.GetTwoFactorAuthenticationUserAsync();
			ApplicationUser applicationUser2 = applicationUser;
			if (applicationUser2 == null)
			{
				throw new InvalidOperationException("Unable to load two-factor authentication user.");
			}
			this.user = applicationUser2;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002E67C File Offset: 0x0002C87C
		private async Task OnValidSubmitAsync()
		{
			string text = this.Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
			SignInResult signInResult = await this.SignInManager.TwoFactorAuthenticatorSignInAsync(text, this.RememberMe, this.Input.RememberMachine);
			SignInResult result = signInResult;
			string text2 = await this.UserManager.GetUserIdAsync(this.user);
			if (result.Succeeded)
			{
				this.Logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", new object[] { text2 });
				this.RedirectManager.RedirectTo(this.ReturnUrl);
			}
			else if (result.IsLockedOut)
			{
				this.Logger.LogWarning("User with ID '{UserId}' account locked out.", new object[] { text2 });
				this.RedirectManager.RedirectTo("Account/Lockout");
			}
			else
			{
				this.Logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", new object[] { text2 });
				this.message = "Error: Invalid authenticator code.";
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0002E6BF File Offset: 0x0002C8BF
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0002E6C7 File Offset: 0x0002C8C7
		[Inject]
		private ILogger<LoginWith2fa> Logger { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0002E6D0 File Offset: 0x0002C8D0
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0002E6D8 File Offset: 0x0002C8D8
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002E6E1 File Offset: 0x0002C8E1
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x0002E6E9 File Offset: 0x0002C8E9
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002E6F2 File Offset: 0x0002C8F2
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0002E6FA File Offset: 0x0002C8FA
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x04000357 RID: 855
		[Nullable(2)]
		private string message;

		// Token: 0x04000358 RID: 856
		private ApplicationUser user;

		// Token: 0x020001E4 RID: 484
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0005F222 File Offset: 0x0005D422
			// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0005F22A File Offset: 0x0005D42A
			[Required]
			[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Text)]
			[Display(Name = "Authenticator code")]
			public string TwoFactorCode { get; set; }

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0005F233 File Offset: 0x0005D433
			// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0005F23B File Offset: 0x0005D43B
			[Display(Name = "Remember this machine")]
			public bool RememberMachine { get; set; }
		}
	}
}
