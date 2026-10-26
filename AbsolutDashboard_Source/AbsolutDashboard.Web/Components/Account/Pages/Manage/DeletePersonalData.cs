using System;
using System.ComponentModel.DataAnnotations;
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
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Manage.DeletePersonalData;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/DeletePersonalData")]
	public class DeletePersonalData : ComponentBase
	{
		// Token: 0x0600080F RID: 2063 RVA: 0x0003020C File Offset: 0x0002E40C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, "Delete Personal Data");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenComponent<StatusMessage>(4);
			__builder.AddComponentParameter(5, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(6, "\n\n");
			__builder.AddMarkupContent(7, "<h3>Delete Personal Data</h3>");
			if (this.isOwner)
			{
				__builder.AddMarkupContent(8, "<div class=\"rounded-xl bg-cream-100 px-4 py-3 text-sm text-cocoa-700 ring-1 ring-blush-100\">\n        This is the owner account. It is managed via server configuration and can't be deleted here.\n        Please contact your administrator.\n    </div>");
				return;
			}
			__builder.AddMarkupContent(9, "<div class=\"alert alert-warning\" role=\"alert\"><p><strong>Deleting this data will permanently remove your account, and this cannot be recovered.</strong></p></div>");
			__builder.OpenElement(10, "div");
			__builder.OpenComponent<EditForm>(11);
			__builder.AddComponentParameter(12, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(13, "FormName", "delete-user");
			__builder.AddComponentParameter(14, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.OnValidSubmitAsync))));
			__builder.AddComponentParameter(15, "method", "post");
			__builder.AddAttribute(16, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(17);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(18, "\n            ");
				__builder2.OpenComponent<ValidationSummary>(19);
				__builder2.AddComponentParameter(20, "class", "text-danger");
				__builder2.AddComponentParameter(21, "role", "alert");
				__builder2.CloseComponent();
				if (this.requirePassword)
				{
					__builder2.OpenElement(22, "div");
					__builder2.AddAttribute(23, "class", "form-floating mb-3");
					__builder2.OpenComponent<InputText>(24);
					__builder2.AddComponentParameter(25, "type", "password");
					__builder2.AddComponentParameter(26, "id", "Input.Password");
					__builder2.AddComponentParameter(27, "class", "form-control");
					__builder2.AddComponentParameter(28, "autocomplete", "current-password");
					__builder2.AddComponentParameter(29, "aria-required", "true");
					__builder2.AddComponentParameter(30, "placeholder", "Please enter your password.");
					__builder2.AddComponentParameter(31, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.Password));
					__builder2.AddComponentParameter(32, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
					{
						this.Input.Password = __value;
					}, this.Input.Password))));
					__builder2.AddComponentParameter(33, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeletePersonalData)), methodof(DeletePersonalData.get_Input())), methodof(DeletePersonalData.InputModel.get_Password())), Array.Empty<ParameterExpression>())));
					__builder2.CloseComponent();
					__builder2.AddMarkupContent(34, "\n                    ");
					__builder2.AddMarkupContent(35, "<label for=\"Input.Password\" class=\"form-label\">Password</label>\n                    ");
					TypeInference.CreateValidationMessage_0<string>(__builder2, 36, 37, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeletePersonalData)), methodof(DeletePersonalData.get_Input())), methodof(DeletePersonalData.InputModel.get_Password())), Array.Empty<ParameterExpression>()), 38, "text-danger");
					__builder2.CloseElement();
				}
				__builder2.AddMarkupContent(39, "<button class=\"w-100 btn btn-lg btn-danger\" type=\"submit\">Delete data and close my account</button>");
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0003035F File Offset: 0x0002E55F
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00030367 File Offset: 0x0002E567
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00030370 File Offset: 0x0002E570
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00030378 File Offset: 0x0002E578
		[SupplyParameterFromForm]
		private DeletePersonalData.InputModel Input { get; set; } = new DeletePersonalData.InputModel();

		// Token: 0x06000814 RID: 2068 RVA: 0x00030384 File Offset: 0x0002E584
		protected override async Task OnInitializedAsync()
		{
			if (this.Input == null)
			{
				this.Input = new DeletePersonalData.InputModel();
			}
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			this.requirePassword = await this.UserManager.HasPasswordAsync(this.user);
			this.isOwner = this.user.IsOwner;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000303C8 File Offset: 0x0002E5C8
		private async Task OnValidSubmitAsync()
		{
			if (this.isOwner)
			{
				this.message = "The owner account can't be deleted here.";
			}
			else
			{
				bool flag = this.requirePassword;
				if (flag)
				{
					TaskAwaiter<bool> taskAwaiter = this.UserManager.CheckPasswordAsync(this.user, this.Input.Password).GetAwaiter();
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
					this.message = "Error: Incorrect password.";
				}
				else
				{
					TaskAwaiter<IdentityResult> taskAwaiter3 = this.UserManager.DeleteAsync(this.user).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						await taskAwaiter3;
						TaskAwaiter<IdentityResult> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<IdentityResult>);
					}
					if (!taskAwaiter3.GetResult().Succeeded)
					{
						throw new InvalidOperationException("Unexpected error occurred deleting user.");
					}
					await this.SignInManager.SignOutAsync();
					string text = await this.UserManager.GetUserIdAsync(this.user);
					this.Logger.LogInformation("User with ID '{UserId}' deleted themselves.", new object[] { text });
					this.RedirectManager.RedirectToCurrentPage();
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0003040B File Offset: 0x0002E60B
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00030413 File Offset: 0x0002E613
		[Inject]
		private ILogger<DeletePersonalData> Logger { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0003041C File Offset: 0x0002E61C
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00030424 File Offset: 0x0002E624
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0003042D File Offset: 0x0002E62D
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00030435 File Offset: 0x0002E635
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0003043E File Offset: 0x0002E63E
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00030446 File Offset: 0x0002E646
		[Inject]
		private SignInManager<ApplicationUser> SignInManager { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0003044F File Offset: 0x0002E64F
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00030457 File Offset: 0x0002E657
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x04000379 RID: 889
		[Nullable(2)]
		private string message;

		// Token: 0x0400037A RID: 890
		private ApplicationUser user;

		// Token: 0x0400037B RID: 891
		private bool requirePassword;

		// Token: 0x0400037C RID: 892
		private bool isOwner;

		// Token: 0x020001F3 RID: 499
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0005FFFA File Offset: 0x0005E1FA
			// (set) Token: 0x06000E63 RID: 3683 RVA: 0x00060002 File Offset: 0x0005E202
			[DataType(DataType.Password)]
			public string Password { get; set; } = "";
		}
	}
}
