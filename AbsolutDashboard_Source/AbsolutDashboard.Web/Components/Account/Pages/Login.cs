using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Login;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Route("/Account/Login")]
	public class Login : ComponentBase
	{
		// Token: 0x0600079F RID: 1951 RVA: 0x0002DC58 File Offset: 0x0002BE58
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Log in — Absolut Dashboard");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "section");
			__builder.AddAttribute(5, "class", "flex min-h-screen items-center justify-center px-4 py-12");
			__builder.OpenElement(6, "div");
			__builder.AddAttribute(7, "class", "card w-full max-w-md p-6");
			__builder.AddMarkupContent(8, "<h1 class=\"text-center text-3xl text-blush-600\">Login</h1>\n        ");
			__builder.AddMarkupContent(9, "<p class=\"mt-1 mb-5 text-center text-sm text-cocoa-700\">absolute monitoring dashboard</p>\n\n        ");
			__builder.OpenComponent<StatusMessage>(10);
			__builder.AddComponentParameter(11, "Message", RuntimeHelpers.TypeCheck<string>(this.errorMessage));
			__builder.CloseComponent();
			__builder.AddMarkupContent(12, "\n        ");
			__builder.OpenComponent<EditForm>(13);
			__builder.AddComponentParameter(14, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(15, "method", "post");
			__builder.AddComponentParameter(16, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.LoginUser))));
			__builder.AddComponentParameter(17, "FormName", "login");
			__builder.AddAttribute(18, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(19);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(20, "\n            ");
				__builder2.OpenComponent<ValidationSummary>(21);
				__builder2.AddComponentParameter(22, "class", "mb-3 text-sm text-blush-700");
				__builder2.AddComponentParameter(23, "role", "alert");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(24, "\n            ");
				__builder2.OpenElement(25, "label");
				__builder2.AddAttribute(26, "class", "block");
				__builder2.AddMarkupContent(27, "<span class=\"text-sm font-medium\">Email</span>\n                ");
				__builder2.OpenComponent<InputText>(28);
				__builder2.AddComponentParameter(29, "class", "input");
				__builder2.AddComponentParameter(30, "autocomplete", "username");
				__builder2.AddComponentParameter(31, "placeholder", "name@example.com");
				__builder2.AddComponentParameter(32, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.Email));
				__builder2.AddComponentParameter(33, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.Email = __value;
				}, this.Input.Email))));
				__builder2.AddComponentParameter(34, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Login)), methodof(Login.get_Input())), methodof(Login.InputModel.get_Email())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(35, "\n                ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 36, 37, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Login)), methodof(Login.get_Input())), methodof(Login.InputModel.get_Email())), Array.Empty<ParameterExpression>()), 38, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(39, "\n            ");
				__builder2.OpenElement(40, "label");
				__builder2.AddAttribute(41, "class", "mt-4 block");
				__builder2.AddMarkupContent(42, "<span class=\"text-sm font-medium\">Password</span>\n                ");
				__builder2.OpenComponent<InputText>(43);
				__builder2.AddComponentParameter(44, "type", "password");
				__builder2.AddComponentParameter(45, "class", "input");
				__builder2.AddComponentParameter(46, "autocomplete", "current-password");
				__builder2.AddComponentParameter(47, "placeholder", "••••••••");
				__builder2.AddComponentParameter(48, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.Password));
				__builder2.AddComponentParameter(49, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.Password = __value;
				}, this.Input.Password))));
				__builder2.AddComponentParameter(50, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Login)), methodof(Login.get_Input())), methodof(Login.InputModel.get_Password())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(51, "\n                ");
				TypeInference.CreateValidationMessage_1<string>(__builder2, 52, 53, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Login)), methodof(Login.get_Input())), methodof(Login.InputModel.get_Password())), Array.Empty<ParameterExpression>()), 54, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(55, "\n            ");
				__builder2.OpenElement(56, "label");
				__builder2.AddAttribute(57, "class", "mt-4 flex items-center gap-2 text-sm");
				__builder2.OpenComponent<InputCheckbox>(58);
				__builder2.AddComponentParameter(59, "Value", RuntimeHelpers.TypeCheck<bool>(this.Input.RememberMe));
				__builder2.AddComponentParameter(60, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<bool>>(EventCallback.Factory.Create<bool>(this, RuntimeHelpers.CreateInferredEventCallback<bool>(this, delegate(bool __value)
				{
					this.Input.RememberMe = __value;
				}, this.Input.RememberMe))));
				__builder2.AddComponentParameter(61, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<bool>>>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Login)), methodof(Login.get_Input())), methodof(Login.InputModel.get_RememberMe())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(62, " Remember me\n            ");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(63, "\n            ");
				__builder2.AddMarkupContent(64, "<button type=\"submit\" class=\"btn-primary mt-6 w-full\">Log in</button>");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(65, "\n\n        ");
			__builder.AddMarkupContent(66, "<p class=\"mt-5 text-center text-sm text-cocoa-700\">\n            No account? Ask an administrator to create one for you.\n        </p>");
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0002DDE6 File Offset: 0x0002BFE6
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0002DDEE File Offset: 0x0002BFEE
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0002DDF7 File Offset: 0x0002BFF7
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0002DDFF File Offset: 0x0002BFFF
		[SupplyParameterFromForm]
		private Login.InputModel Input { get; set; } = new Login.InputModel();

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0002DE08 File Offset: 0x0002C008
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0002DE10 File Offset: 0x0002C010
		[Nullable(2)]
		[SupplyParameterFromQuery]
		private string ReturnUrl
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002DE1C File Offset: 0x0002C01C
		protected override async Task OnInitializedAsync()
		{
			if (!this.UserManager.Users.Any<ApplicationUser>())
			{
				this.RedirectManager.RedirectTo("setup");
			}
			if (HttpMethods.IsGet(this.HttpContext.Request.Method))
			{
				IIdentity identity = this.HttpContext.User.Identity;
				if (identity != null && identity.IsAuthenticated)
				{
					this.RedirectManager.RedirectTo("");
				}
				await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002DE60 File Offset: 0x0002C060
		public async Task LoginUser()
		{
			SignInResult signInResult = await this.SignInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, this.Input.RememberMe, false);
			if (signInResult.Succeeded)
			{
				this.Logger.LogInformation("User logged in.", Array.Empty<object>());
				this.RedirectManager.RedirectTo(this.ReturnUrl);
			}
			else if (signInResult.RequiresTwoFactor)
			{
				IdentityRedirectManager redirectManager = this.RedirectManager;
				string text = "Account/LoginWith2fa";
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["returnUrl"] = this.ReturnUrl;
				dictionary["rememberMe"] = this.Input.RememberMe;
				redirectManager.RedirectTo(text, dictionary);
			}
			else if (signInResult.IsLockedOut)
			{
				this.Logger.LogWarning("User account locked out.", Array.Empty<object>());
				this.RedirectManager.RedirectTo("Account/Lockout");
			}
			else
			{
				this.errorMessage = "Error: Invalid login attempt.";
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002DEA3 File Offset: 0x0002C0A3
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0002DEAB File Offset: 0x0002C0AB
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002DEB4 File Offset: 0x0002C0B4
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0002DEBC File Offset: 0x0002C0BC
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002DEC5 File Offset: 0x0002C0C5
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0002DECD File Offset: 0x0002C0CD
		[Inject]
		private ILogger<Login> Logger { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0002DED6 File Offset: 0x0002C0D6
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0002DEDE File Offset: 0x0002C0DE
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0002DEE7 File Offset: 0x0002C0E7
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0002DEEF File Offset: 0x0002C0EF
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x0400034E RID: 846
		[Nullable(2)]
		private string errorMessage;

		// Token: 0x020001E0 RID: 480
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0005EEC1 File Offset: 0x0005D0C1
			// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0005EEC9 File Offset: 0x0005D0C9
			[Required]
			[EmailAddress]
			public string Email { get; set; } = "";

			// Token: 0x170002E6 RID: 742
			// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0005EED2 File Offset: 0x0005D0D2
			// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0005EEDA File Offset: 0x0005D0DA
			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; } = "";

			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0005EEE3 File Offset: 0x0005D0E3
			// (set) Token: 0x06000E27 RID: 3623 RVA: 0x0005EEEB File Offset: 0x0005D0EB
			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}
	}
}
