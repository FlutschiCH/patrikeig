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
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.LoginWithRecoveryCode;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Route("/Account/LoginWithRecoveryCode")]
	public class LoginWithRecoveryCode : ComponentBase
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x0002EB0C File Offset: 0x0002CD0C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Recovery code verification");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<h1>Recovery code verification</h1>\n<hr>\n");
			__builder.OpenComponent<StatusMessage>(5);
			__builder.AddComponentParameter(6, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(7, "\n");
			__builder.AddMarkupContent(8, "<p>\n    You have requested to log in with a recovery code. This login will not be remembered until you provide\n    an authenticator app code at log in or disable 2FA and log in again.\n</p>\n");
			__builder.OpenElement(9, "div");
			__builder.AddAttribute(10, "class", "row");
			__builder.OpenElement(11, "div");
			__builder.AddAttribute(12, "class", "col-md-4");
			__builder.OpenComponent<EditForm>(13);
			__builder.AddComponentParameter(14, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(15, "FormName", "login-with-recovery-code");
			__builder.AddComponentParameter(16, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.OnValidSubmitAsync))));
			__builder.AddComponentParameter(17, "method", "post");
			__builder.AddAttribute(18, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(19);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(20, "\n            ");
				__builder2.OpenComponent<ValidationSummary>(21);
				__builder2.AddComponentParameter(22, "class", "text-danger");
				__builder2.AddComponentParameter(23, "role", "alert");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(24, "\n            ");
				__builder2.OpenElement(25, "div");
				__builder2.AddAttribute(26, "class", "form-floating mb-3");
				__builder2.OpenComponent<InputText>(27);
				__builder2.AddComponentParameter(28, "id", "Input.RecoveryCode");
				__builder2.AddComponentParameter(29, "class", "form-control");
				__builder2.AddComponentParameter(30, "autocomplete", "off");
				__builder2.AddComponentParameter(31, "placeholder", "RecoveryCode");
				__builder2.AddComponentParameter(32, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.RecoveryCode));
				__builder2.AddComponentParameter(33, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.RecoveryCode = __value;
				}, this.Input.RecoveryCode))));
				__builder2.AddComponentParameter(34, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(LoginWithRecoveryCode)), methodof(LoginWithRecoveryCode.get_Input())), methodof(LoginWithRecoveryCode.InputModel.get_RecoveryCode())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(35, "\n                ");
				__builder2.AddMarkupContent(36, "<label for=\"Input.RecoveryCode\" class=\"form-label\">Recovery Code</label>\n                ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 37, 38, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(LoginWithRecoveryCode)), methodof(LoginWithRecoveryCode.get_Input())), methodof(LoginWithRecoveryCode.InputModel.get_RecoveryCode())), Array.Empty<ParameterExpression>()), 39, "text-danger");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(40, "\n            ");
				__builder2.AddMarkupContent(41, "<button type=\"submit\" class=\"w-100 btn btn-lg btn-primary\">Log in</button>");
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0002EC80 File Offset: 0x0002CE80
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0002EC88 File Offset: 0x0002CE88
		[SupplyParameterFromForm]
		private LoginWithRecoveryCode.InputModel Input { get; set; } = new LoginWithRecoveryCode.InputModel();

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002EC91 File Offset: 0x0002CE91
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0002EC99 File Offset: 0x0002CE99
		[Nullable(2)]
		[SupplyParameterFromQuery]
		private string ReturnUrl
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002ECA4 File Offset: 0x0002CEA4
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

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002ECE8 File Offset: 0x0002CEE8
		private async Task OnValidSubmitAsync()
		{
			string text = this.Input.RecoveryCode.Replace(" ", string.Empty);
			SignInResult signInResult = await this.SignInManager.TwoFactorRecoveryCodeSignInAsync(text);
			SignInResult result = signInResult;
			string text2 = await this.UserManager.GetUserIdAsync(this.user);
			if (result.Succeeded)
			{
				this.Logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", new object[] { text2 });
				this.RedirectManager.RedirectTo(this.ReturnUrl);
			}
			else if (result.IsLockedOut)
			{
				this.Logger.LogWarning("User account locked out.", Array.Empty<object>());
				this.RedirectManager.RedirectTo("Account/Lockout");
			}
			else
			{
				this.Logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", new object[] { text2 });
				this.message = "Error: Invalid recovery code entered.";
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002ED2B File Offset: 0x0002CF2B
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0002ED33 File Offset: 0x0002CF33
		[Inject]
		private ILogger<LoginWithRecoveryCode> Logger { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002ED3C File Offset: 0x0002CF3C
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0002ED44 File Offset: 0x0002CF44
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0002ED4D File Offset: 0x0002CF4D
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0002ED55 File Offset: 0x0002CF55
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002ED5E File Offset: 0x0002CF5E
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0002ED66 File Offset: 0x0002CF66
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x04000360 RID: 864
		[Nullable(2)]
		private string message;

		// Token: 0x04000361 RID: 865
		private ApplicationUser user;

		// Token: 0x020001E8 RID: 488
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0005F58A File Offset: 0x0005D78A
			// (set) Token: 0x06000E3D RID: 3645 RVA: 0x0005F592 File Offset: 0x0005D792
			[Required]
			[DataType(DataType.Text)]
			[Display(Name = "Recovery Code")]
			public string RecoveryCode { get; set; } = "";
		}
	}
}
