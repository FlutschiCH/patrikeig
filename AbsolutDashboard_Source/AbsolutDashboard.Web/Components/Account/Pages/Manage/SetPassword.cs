using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Manage.SetPassword;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/SetPassword")]
	public class SetPassword : ComponentBase
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x00031F08 File Offset: 0x00030108
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Set password");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<h3>Set your password</h3>\n");
			__builder.OpenComponent<StatusMessage>(5);
			__builder.AddComponentParameter(6, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(7, "\n");
			__builder.AddMarkupContent(8, "<p class=\"text-info\">\n    You do not have a local username/password for this site. Add a local\n    account so you can log in without an external login.\n</p>\n");
			__builder.OpenElement(9, "div");
			__builder.AddAttribute(10, "class", "row");
			__builder.OpenElement(11, "div");
			__builder.AddAttribute(12, "class", "col-xl-6");
			__builder.OpenComponent<EditForm>(13);
			__builder.AddComponentParameter(14, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(15, "FormName", "set-password");
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
				__builder2.AddComponentParameter(28, "type", "password");
				__builder2.AddComponentParameter(29, "id", "Input.NewPassword");
				__builder2.AddComponentParameter(30, "class", "form-control");
				__builder2.AddComponentParameter(31, "autocomplete", "new-password");
				__builder2.AddComponentParameter(32, "placeholder", "Enter the new password");
				__builder2.AddComponentParameter(33, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.NewPassword));
				__builder2.AddComponentParameter(34, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.NewPassword = __value;
				}, this.Input.NewPassword))));
				__builder2.AddComponentParameter(35, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(SetPassword)), methodof(SetPassword.get_Input())), methodof(SetPassword.InputModel.get_NewPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(36, "\n                ");
				__builder2.AddMarkupContent(37, "<label for=\"Input.NewPassword\" class=\"form-label\">New password</label>\n                ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 38, 39, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(SetPassword)), methodof(SetPassword.get_Input())), methodof(SetPassword.InputModel.get_NewPassword())), Array.Empty<ParameterExpression>()), 40, "text-danger");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(41, "\n            ");
				__builder2.OpenElement(42, "div");
				__builder2.AddAttribute(43, "class", "form-floating mb-3");
				__builder2.OpenComponent<InputText>(44);
				__builder2.AddComponentParameter(45, "type", "password");
				__builder2.AddComponentParameter(46, "id", "Input.ConfirmPassword");
				__builder2.AddComponentParameter(47, "class", "form-control");
				__builder2.AddComponentParameter(48, "autocomplete", "new-password");
				__builder2.AddComponentParameter(49, "placeholder", "Enter the new password");
				__builder2.AddComponentParameter(50, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.ConfirmPassword));
				__builder2.AddComponentParameter(51, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.ConfirmPassword = __value;
				}, this.Input.ConfirmPassword))));
				__builder2.AddComponentParameter(52, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(SetPassword)), methodof(SetPassword.get_Input())), methodof(SetPassword.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(53, "\n                ");
				__builder2.AddMarkupContent(54, "<label for=\"Input.ConfirmPassword\" class=\"form-label\">Confirm password</label>\n                ");
				TypeInference.CreateValidationMessage_1<string>(__builder2, 55, 56, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(SetPassword)), methodof(SetPassword.get_Input())), methodof(SetPassword.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>()), 57, "text-danger");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(58, "\n            ");
				__builder2.AddMarkupContent(59, "<button type=\"submit\" class=\"w-100 btn btn-lg btn-primary\">Set password</button>");
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0003207C File Offset: 0x0003027C
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00032084 File Offset: 0x00030284
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0003208D File Offset: 0x0003028D
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00032095 File Offset: 0x00030295
		[SupplyParameterFromForm]
		private SetPassword.InputModel Input { get; set; } = new SetPassword.InputModel();

		// Token: 0x0600087D RID: 2173 RVA: 0x000320A0 File Offset: 0x000302A0
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			TaskAwaiter<bool> taskAwaiter = this.UserManager.HasPasswordAsync(this.user).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.RedirectManager.RedirectTo("Account/Manage/ChangePassword");
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000320E4 File Offset: 0x000302E4
		private async Task OnValidSubmitAsync()
		{
			IdentityResult identityResult = await this.UserManager.AddPasswordAsync(this.user, this.Input.NewPassword);
			if (!identityResult.Succeeded)
			{
				this.message = "Error: " + string.Join(",", identityResult.Errors.Select<IdentityError, string>((IdentityError error) => error.Description));
			}
			else
			{
				await this.SignInManager.RefreshSignInAsync(this.user);
				this.RedirectManager.RedirectToCurrentPageWithStatus("Your password has been set.", this.HttpContext);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00032127 File Offset: 0x00030327
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0003212F File Offset: 0x0003032F
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00032138 File Offset: 0x00030338
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x00032140 File Offset: 0x00030340
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00032149 File Offset: 0x00030349
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00032151 File Offset: 0x00030351
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0003215A File Offset: 0x0003035A
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x00032162 File Offset: 0x00030362
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x040003AF RID: 943
		[Nullable(2)]
		private string message;

		// Token: 0x040003B0 RID: 944
		private ApplicationUser user;

		// Token: 0x02000208 RID: 520
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002F3 RID: 755
			// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00061846 File Offset: 0x0005FA46
			// (set) Token: 0x06000E96 RID: 3734 RVA: 0x0006184E File Offset: 0x0005FA4E
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "New password")]
			public string NewPassword { get; set; }

			// Token: 0x170002F4 RID: 756
			// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00061857 File Offset: 0x0005FA57
			// (set) Token: 0x06000E98 RID: 3736 RVA: 0x0006185F File Offset: 0x0005FA5F
			[DataType(DataType.Password)]
			[Display(Name = "Confirm new password")]
			[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}
	}
}
