using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Manage.ChangePassword;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/ChangePassword")]
	public class ChangePassword : ComponentBase
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x0002F7A4 File Offset: 0x0002D9A4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Passwort ändern" : "Change password");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "div");
			__builder.AddAttribute(5, "class", "space-y-6");
			__builder.OpenElement(6, "div");
			__builder.OpenElement(7, "h2");
			__builder.AddAttribute(8, "class", "font-display text-2xl text-cocoa-900");
			__builder.AddContent(9, Loc.IsGerman ? "Passwort ändern" : "Change password");
			__builder.CloseElement();
			__builder.AddMarkupContent(10, "\n        ");
			__builder.OpenElement(11, "p");
			__builder.AddAttribute(12, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(13, Loc.IsGerman ? "Aktualisiere dein Passwort." : "Update your account password.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(14, "\n\n    ");
			__builder.OpenComponent<StatusMessage>(15);
			__builder.AddComponentParameter(16, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(17, "\n\n    ");
			__builder.OpenComponent<EditForm>(18);
			__builder.AddComponentParameter(19, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(20, "FormName", "change-password");
			__builder.AddComponentParameter(21, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.OnValidSubmitAsync))));
			__builder.AddComponentParameter(22, "method", "post");
			__builder.AddComponentParameter(23, "class", "space-y-5");
			__builder.AddAttribute(24, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(25);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(26, "\n        ");
				__builder2.OpenComponent<ValidationSummary>(27);
				__builder2.AddComponentParameter(28, "class", "rounded-xl bg-red-50 px-4 py-3 text-sm text-red-700 ring-1 ring-red-200");
				__builder2.AddComponentParameter(29, "role", "alert");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(30, "\n\n        ");
				__builder2.OpenElement(31, "div");
				__builder2.OpenElement(32, "label");
				__builder2.AddAttribute(33, "for", "Input.OldPassword");
				__builder2.AddAttribute(34, "class", "block text-sm font-medium text-cocoa-900");
				__builder2.AddContent(35, Loc.IsGerman ? "Aktuelles Passwort" : "Old password");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(36, "\n            ");
				__builder2.OpenComponent<InputText>(37);
				__builder2.AddComponentParameter(38, "type", "password");
				__builder2.AddComponentParameter(39, "id", "Input.OldPassword");
				__builder2.AddComponentParameter(40, "class", "input");
				__builder2.AddComponentParameter(41, "autocomplete", "current-password");
				__builder2.AddComponentParameter(42, "aria-required", "true");
				__builder2.AddComponentParameter(43, "placeholder", Loc.IsGerman ? "Aktuelles Passwort eingeben" : "Enter the old password");
				__builder2.AddComponentParameter(44, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.OldPassword));
				__builder2.AddComponentParameter(45, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.OldPassword = __value;
				}, this.Input.OldPassword))));
				__builder2.AddComponentParameter(46, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_OldPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(47, "\n            ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 48, 49, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_OldPassword())), Array.Empty<ParameterExpression>()), 50, "mt-1 block text-sm text-red-600");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(51, "\n\n        ");
				__builder2.OpenElement(52, "div");
				__builder2.OpenElement(53, "label");
				__builder2.AddAttribute(54, "for", "Input.NewPassword");
				__builder2.AddAttribute(55, "class", "block text-sm font-medium text-cocoa-900");
				__builder2.AddContent(56, Loc.IsGerman ? "Neues Passwort" : "New password");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(57, "\n            ");
				__builder2.OpenComponent<InputText>(58);
				__builder2.AddComponentParameter(59, "type", "password");
				__builder2.AddComponentParameter(60, "id", "Input.NewPassword");
				__builder2.AddComponentParameter(61, "class", "input");
				__builder2.AddComponentParameter(62, "autocomplete", "new-password");
				__builder2.AddComponentParameter(63, "aria-required", "true");
				__builder2.AddComponentParameter(64, "placeholder", Loc.IsGerman ? "Neues Passwort eingeben" : "Enter the new password");
				__builder2.AddComponentParameter(65, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.NewPassword));
				__builder2.AddComponentParameter(66, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.NewPassword = __value;
				}, this.Input.NewPassword))));
				__builder2.AddComponentParameter(67, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_NewPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(68, "\n            ");
				TypeInference.CreateValidationMessage_1<string>(__builder2, 69, 70, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_NewPassword())), Array.Empty<ParameterExpression>()), 71, "mt-1 block text-sm text-red-600");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(72, "\n\n        ");
				__builder2.OpenElement(73, "div");
				__builder2.OpenElement(74, "label");
				__builder2.AddAttribute(75, "for", "Input.ConfirmPassword");
				__builder2.AddAttribute(76, "class", "block text-sm font-medium text-cocoa-900");
				__builder2.AddContent(77, Loc.IsGerman ? "Passwort bestätigen" : "Confirm password");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(78, "\n            ");
				__builder2.OpenComponent<InputText>(79);
				__builder2.AddComponentParameter(80, "type", "password");
				__builder2.AddComponentParameter(81, "id", "Input.ConfirmPassword");
				__builder2.AddComponentParameter(82, "class", "input");
				__builder2.AddComponentParameter(83, "autocomplete", "new-password");
				__builder2.AddComponentParameter(84, "aria-required", "true");
				__builder2.AddComponentParameter(85, "placeholder", Loc.IsGerman ? "Neues Passwort bestätigen" : "Enter the new password");
				__builder2.AddComponentParameter(86, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.ConfirmPassword));
				__builder2.AddComponentParameter(87, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.ConfirmPassword = __value;
				}, this.Input.ConfirmPassword))));
				__builder2.AddComponentParameter(88, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(89, "\n            ");
				TypeInference.CreateValidationMessage_2<string>(__builder2, 90, 91, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(ChangePassword)), methodof(ChangePassword.get_Input())), methodof(ChangePassword.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>()), 92, "mt-1 block text-sm text-red-600");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(93, "\n\n        ");
				__builder2.OpenElement(94, "div");
				__builder2.AddAttribute(95, "class", "pt-2");
				__builder2.OpenElement(96, "button");
				__builder2.AddAttribute(97, "type", "submit");
				__builder2.AddAttribute(98, "class", "btn-primary");
				__builder2.AddContent(99, Loc.IsGerman ? "Passwort aktualisieren" : "Update password");
				__builder2.CloseElement();
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0002F998 File Offset: 0x0002DB98
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0002F9A0 File Offset: 0x0002DBA0
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002F9A9 File Offset: 0x0002DBA9
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0002F9B1 File Offset: 0x0002DBB1
		[SupplyParameterFromForm]
		private ChangePassword.InputModel Input { get; set; } = new ChangePassword.InputModel();

		// Token: 0x060007FD RID: 2045 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			this.hasPassword = await this.UserManager.HasPasswordAsync(this.user);
			if (!this.hasPassword)
			{
				this.RedirectManager.RedirectTo("Account/Manage/SetPassword");
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002FA00 File Offset: 0x0002DC00
		private async Task OnValidSubmitAsync()
		{
			IdentityResult identityResult = await this.UserManager.ChangePasswordAsync(this.user, this.Input.OldPassword, this.Input.NewPassword);
			if (!identityResult.Succeeded)
			{
				this.message = "Error: " + string.Join(",", identityResult.Errors.Select<IdentityError, string>((IdentityError error) => error.Description));
			}
			else
			{
				await this.SignInManager.RefreshSignInAsync(this.user);
				this.Logger.LogInformation("User changed their password successfully.", Array.Empty<object>());
				this.RedirectManager.RedirectToCurrentPageWithStatus("Your password has been changed", this.HttpContext);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0002FA43 File Offset: 0x0002DC43
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0002FA4B File Offset: 0x0002DC4B
		[Inject]
		private ILogger<ChangePassword> Logger { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0002FA54 File Offset: 0x0002DC54
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x0002FA5C File Offset: 0x0002DC5C
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002FA65 File Offset: 0x0002DC65
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0002FA6D File Offset: 0x0002DC6D
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002FA76 File Offset: 0x0002DC76
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x0002FA7E File Offset: 0x0002DC7E
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0002FA87 File Offset: 0x0002DC87
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x0002FA8F File Offset: 0x0002DC8F
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x0400036F RID: 879
		[Nullable(2)]
		private string message;

		// Token: 0x04000370 RID: 880
		private ApplicationUser user;

		// Token: 0x04000371 RID: 881
		private bool hasPassword;

		// Token: 0x020001EF RID: 495
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0005FC0E File Offset: 0x0005DE0E
			// (set) Token: 0x06000E54 RID: 3668 RVA: 0x0005FC16 File Offset: 0x0005DE16
			[Required]
			[DataType(DataType.Password)]
			[Display(Name = "Current password")]
			public string OldPassword { get; set; } = "";

			// Token: 0x170002EF RID: 751
			// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0005FC1F File Offset: 0x0005DE1F
			// (set) Token: 0x06000E56 RID: 3670 RVA: 0x0005FC27 File Offset: 0x0005DE27
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "New password")]
			public string NewPassword { get; set; } = "";

			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0005FC30 File Offset: 0x0005DE30
			// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0005FC38 File Offset: 0x0005DE38
			[DataType(DataType.Password)]
			[Display(Name = "Confirm new password")]
			[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; } = "";
		}
	}
}
