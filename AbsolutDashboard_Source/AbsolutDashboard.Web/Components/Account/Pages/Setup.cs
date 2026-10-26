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
using Microsoft.Extensions.Logging;
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Setup;

namespace AbsolutDashboard.Web.Components.Account.Pages
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[AllowAnonymous]
	[Route("/setup")]
	public class Setup : ComponentBase
	{
		// Token: 0x060007E1 RID: 2017 RVA: 0x0002EFB8 File Offset: 0x0002D1B8
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Set up — Absolut Dashboard");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "section");
			__builder.AddAttribute(5, "class", "flex min-h-screen items-center justify-center px-4 py-12");
			__builder.OpenElement(6, "div");
			__builder.AddAttribute(7, "class", "card w-full max-w-md p-6");
			__builder.AddMarkupContent(8, "<h1 class=\"text-center text-3xl text-blush-600\">Welcome</h1>\n        ");
			__builder.AddMarkupContent(9, "<p class=\"mt-1 mb-5 text-center text-sm text-cocoa-700\">\n            Create the owner account to finish setting up the dashboard. This is a one-time step.\n        </p>\n\n        ");
			__builder.OpenComponent<StatusMessage>(10);
			__builder.AddComponentParameter(11, "Message", RuntimeHelpers.TypeCheck<string>(this.errorMessage));
			__builder.CloseComponent();
			__builder.AddMarkupContent(12, "\n        ");
			__builder.OpenComponent<EditForm>(13);
			__builder.AddComponentParameter(14, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(15, "method", "post");
			__builder.AddComponentParameter(16, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.CreateOwner))));
			__builder.AddComponentParameter(17, "FormName", "setup");
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
				__builder2.AddComponentParameter(34, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_Email())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(35, "\n                ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 36, 37, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_Email())), Array.Empty<ParameterExpression>()), 38, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(39, "\n            ");
				__builder2.OpenElement(40, "label");
				__builder2.AddAttribute(41, "class", "mt-4 block");
				__builder2.AddMarkupContent(42, "<span class=\"text-sm font-medium\">Password</span>\n                ");
				__builder2.OpenComponent<InputText>(43);
				__builder2.AddComponentParameter(44, "type", "password");
				__builder2.AddComponentParameter(45, "class", "input");
				__builder2.AddComponentParameter(46, "autocomplete", "new-password");
				__builder2.AddComponentParameter(47, "placeholder", "••••••••");
				__builder2.AddComponentParameter(48, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.Password));
				__builder2.AddComponentParameter(49, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.Password = __value;
				}, this.Input.Password))));
				__builder2.AddComponentParameter(50, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_Password())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(51, "\n                ");
				TypeInference.CreateValidationMessage_1<string>(__builder2, 52, 53, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_Password())), Array.Empty<ParameterExpression>()), 54, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(55, "\n            ");
				__builder2.OpenElement(56, "label");
				__builder2.AddAttribute(57, "class", "mt-4 block");
				__builder2.AddMarkupContent(58, "<span class=\"text-sm font-medium\">Confirm password</span>\n                ");
				__builder2.OpenComponent<InputText>(59);
				__builder2.AddComponentParameter(60, "type", "password");
				__builder2.AddComponentParameter(61, "class", "input");
				__builder2.AddComponentParameter(62, "autocomplete", "new-password");
				__builder2.AddComponentParameter(63, "placeholder", "••••••••");
				__builder2.AddComponentParameter(64, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.ConfirmPassword));
				__builder2.AddComponentParameter(65, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.ConfirmPassword = __value;
				}, this.Input.ConfirmPassword))));
				__builder2.AddComponentParameter(66, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(67, "\n                ");
				TypeInference.CreateValidationMessage_2<string>(__builder2, 68, 69, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(Setup)), methodof(Setup.get_Input())), methodof(Setup.InputModel.get_ConfirmPassword())), Array.Empty<ParameterExpression>()), 70, "text-xs text-blush-700");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(71, "\n            ");
				__builder2.AddMarkupContent(72, "<button type=\"submit\" class=\"btn-primary mt-6 w-full\">Create owner account</button>");
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0002F12C File Offset: 0x0002D32C
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0002F134 File Offset: 0x0002D334
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0002F13D File Offset: 0x0002D33D
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x0002F145 File Offset: 0x0002D345
		[SupplyParameterFromForm]
		private Setup.InputModel Input { get; set; } = new Setup.InputModel();

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002F14E File Offset: 0x0002D34E
		protected override void OnInitialized()
		{
			if (this.UserManager.Users.Any<ApplicationUser>())
			{
				this.RedirectManager.RedirectTo("Account/Login");
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002F174 File Offset: 0x0002D374
		private async Task CreateOwner()
		{
			if (this.UserManager.Users.Any<ApplicationUser>())
			{
				this.RedirectManager.RedirectTo("Account/Login");
			}
			else
			{
				string text = this.Input.Email.Trim();
				ApplicationUser user = new ApplicationUser
				{
					UserName = text,
					Email = text,
					EmailConfirmed = true,
					IsOwner = true
				};
				IdentityResult identityResult = await this.UserManager.CreateAsync(user, this.Input.Password);
				if (!identityResult.Succeeded)
				{
					this.errorMessage = "Error: " + string.Join(" ", identityResult.Errors.Select<IdentityError, string>((IdentityError e) => e.Description));
				}
				else
				{
					await this.UserManager.AddToRoleAsync(user, "Admin");
					this.Logger.LogInformation("Owner account {Email} created via first-run setup.", new object[] { user.Email });
					await this.SignInManager.SignInAsync(user, false, null);
					this.RedirectManager.RedirectTo("");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0002F1B7 File Offset: 0x0002D3B7
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0002F1BF File Offset: 0x0002D3BF
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0002F1C8 File Offset: 0x0002D3C8
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0002F1D0 File Offset: 0x0002D3D0
		[Inject]
		private ILogger<Setup> Logger { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002F1D9 File Offset: 0x0002D3D9
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0002F1E1 File Offset: 0x0002D3E1
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002F1EA File Offset: 0x0002D3EA
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0002F1F2 File Offset: 0x0002D3F2
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x04000368 RID: 872
		[Nullable(2)]
		private string errorMessage;

		// Token: 0x020001EC RID: 492
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0005F8C2 File Offset: 0x0005DAC2
			// (set) Token: 0x06000E47 RID: 3655 RVA: 0x0005F8CA File Offset: 0x0005DACA
			[Required]
			[EmailAddress]
			public string Email { get; set; } = "";

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0005F8D3 File Offset: 0x0005DAD3
			// (set) Token: 0x06000E49 RID: 3657 RVA: 0x0005F8DB File Offset: 0x0005DADB
			[Required]
			[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
			[DataType(DataType.Password)]
			public string Password { get; set; } = "";

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0005F8E4 File Offset: 0x0005DAE4
			// (set) Token: 0x06000E4B RID: 3659 RVA: 0x0005F8EC File Offset: 0x0005DAEC
			[DataType(DataType.Password)]
			[Compare("Password", ErrorMessage = "The passwords don't match.")]
			public string ConfirmPassword { get; set; } = "";
		}
	}
}
