using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using __Blazor.AbsolutDashboard.Web.Components.Users.CreateUser;

namespace AbsolutDashboard.Web.Components.Users
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize(Roles = "Admin")]
	[Route("/users/new")]
	[CreateUser.__PrivateComponentRenderModeAttribute]
	public class CreateUser : ComponentBase
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x0002144C File Offset: 0x0001F64C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "New user — Absolut");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<header class=\"mb-6\"><a class=\"btn-back mb-2\" href=\"users\">← Users</a>\n    <h1 class=\"text-2xl font-semibold text-cocoa-900\">Create a user</h1>\n    <p class=\"mt-1 text-sm text-cocoa-700\">The account is active immediately — no email confirmation needed.</p></header>\n\n");
			__builder.OpenComponent<EditForm>(5);
			__builder.AddComponentParameter(6, "Model", RuntimeHelpers.TypeCheck<object>(this._input));
			__builder.AddComponentParameter(7, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.Create))));
			__builder.AddComponentParameter(8, "class", "card max-w-lg space-y-5 p-6");
			__builder.AddAttribute(9, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(10);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(11, "\n    ");
				__builder2.OpenComponent<ValidationSummary>(12);
				__builder2.AddComponentParameter(13, "class", "text-sm text-red-700");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(14, "\n\n    ");
				__builder2.OpenElement(15, "label");
				__builder2.AddAttribute(16, "class", "block");
				__builder2.AddMarkupContent(17, "<span class=\"text-sm font-medium\">Email</span>\n        ");
				__builder2.OpenComponent<InputText>(18);
				__builder2.AddComponentParameter(19, "class", "input");
				__builder2.AddComponentParameter(20, "autocomplete", "off");
				__builder2.AddComponentParameter(21, "placeholder", "name@example.com");
				__builder2.AddComponentParameter(22, "Value", RuntimeHelpers.TypeCheck<string>(this._input.Email));
				__builder2.AddComponentParameter(23, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this._input.Email = __value;
				}, this._input.Email))));
				__builder2.AddComponentParameter(24, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Field(Expression.Constant(this, typeof(CreateUser)), fieldof(CreateUser._input)), methodof(CreateUser.InputModel.get_Email())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.CloseElement();
				__builder2.AddMarkupContent(25, "\n    ");
				__builder2.OpenElement(26, "label");
				__builder2.AddAttribute(27, "class", "block");
				__builder2.AddMarkupContent(28, "<span class=\"text-sm font-medium\">Password</span>\n        ");
				__builder2.OpenComponent<InputText>(29);
				__builder2.AddComponentParameter(30, "type", "password");
				__builder2.AddComponentParameter(31, "class", "input");
				__builder2.AddComponentParameter(32, "autocomplete", "new-password");
				__builder2.AddComponentParameter(33, "Value", RuntimeHelpers.TypeCheck<string>(this._input.Password));
				__builder2.AddComponentParameter(34, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this._input.Password = __value;
				}, this._input.Password))));
				__builder2.AddComponentParameter(35, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Field(Expression.Constant(this, typeof(CreateUser)), fieldof(CreateUser._input)), methodof(CreateUser.InputModel.get_Password())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.CloseElement();
				__builder2.AddMarkupContent(36, "\n    ");
				__builder2.OpenElement(37, "label");
				__builder2.AddAttribute(38, "class", "block");
				__builder2.AddMarkupContent(39, "<span class=\"text-sm font-medium\">Role</span>\n        ");
				TypeInference.CreateInputSelect_0<string>(__builder2, 40, 41, "input", 42, this._input.Role, 43, EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this._input.Role = __value;
				}, this._input.Role)), 44, Expression.Lambda<Func<string>>(Expression.Property(Expression.Field(Expression.Constant(this, typeof(CreateUser)), fieldof(CreateUser._input)), methodof(CreateUser.InputModel.get_Role())), Array.Empty<ParameterExpression>()), 45, delegate(RenderTreeBuilder __builder3)
				{
					__builder3.OpenElement(46, "option");
					__builder3.AddAttribute(47, "value", "User");
					__builder3.AddMarkupContent(48, "User — view devices only");
					__builder3.CloseElement();
					if (this._actorIsOwner)
					{
						__builder3.OpenElement(49, "option");
						__builder3.AddAttribute(50, "value", "Admin");
						__builder3.AddMarkupContent(51, "Admin — full access");
						__builder3.CloseElement();
					}
				});
				if (!this._actorIsOwner)
				{
					__builder2.AddMarkupContent(52, "<span class=\"mt-1 block text-xs text-cocoa-700\">Only the owner can create admin accounts.</span>");
				}
				__builder2.CloseElement();
				if (this._error != null)
				{
					__builder2.OpenElement(53, "div");
					__builder2.AddAttribute(54, "class", "rounded-lg border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
					__builder2.AddContent(55, this._error);
					__builder2.CloseElement();
				}
				__builder2.OpenElement(56, "div");
				__builder2.AddAttribute(57, "class", "flex gap-3");
				__builder2.OpenElement(58, "button");
				__builder2.AddAttribute(59, "type", "submit");
				__builder2.AddAttribute(60, "class", "btn-primary !px-5 !py-2.5 text-sm");
				__builder2.AddAttribute(61, "disabled", this._busy);
				__builder2.AddContent(62, "Create user");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(63, "\n        ");
				__builder2.AddMarkupContent(64, "<a class=\"btn-secondary !px-5 !py-2.5 text-sm\" href=\"users\">Cancel</a>");
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00021524 File Offset: 0x0001F724
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0002152C File Offset: 0x0001F72C
		[Nullable(new byte[] { 2, 1 })]
		[CascadingParameter]
		private Task<AuthenticationState> AuthState
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00021538 File Offset: 0x0001F738
		protected override async Task OnInitializedAsync()
		{
			if (this.AuthState != null)
			{
				UserManager<ApplicationUser> userManager = this.Users;
				AuthenticationState authenticationState = await this.AuthState;
				ApplicationUser applicationUser = await userManager.GetUserAsync(authenticationState.User);
				userManager = null;
				ApplicationUser applicationUser2 = applicationUser;
				this._actorIsOwner = applicationUser2 != null && applicationUser2.IsOwner;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002157C File Offset: 0x0001F77C
		private async Task Create()
		{
			this._error = null;
			string role = this._input.Role;
			if (role == "Admin" && !this._actorIsOwner)
			{
				this._error = "Only the owner can create admin accounts.";
			}
			else
			{
				this._busy = true;
				try
				{
					ApplicationUser user = new ApplicationUser
					{
						UserName = this._input.Email.Trim(),
						Email = this._input.Email.Trim(),
						EmailConfirmed = true
					};
					IdentityResult identityResult = await this.Users.CreateAsync(user, this._input.Password);
					if (!identityResult.Succeeded)
					{
						this._error = string.Join(" ", identityResult.Errors.Select<IdentityError, string>((IdentityError e) => e.Description));
					}
					else
					{
						await this.Users.AddToRoleAsync(user, (role == "Admin") ? "Admin" : "User");
						this.Logger.LogInformation("Admin created user {Email} with role {Role}.", new object[] { user.Email, role });
						this.Nav.NavigateTo("users", false, false);
						user = null;
					}
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000215BF File Offset: 0x0001F7BF
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x000215C7 File Offset: 0x0001F7C7
		[Inject]
		private ILogger<CreateUser> Logger { get; set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x000215D0 File Offset: 0x0001F7D0
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x000215D8 File Offset: 0x0001F7D8
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x000215E1 File Offset: 0x0001F7E1
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x000215E9 File Offset: 0x0001F7E9
		[Inject]
		private UserManager<ApplicationUser> Users { get; set; }

		// Token: 0x0400029E RID: 670
		private readonly CreateUser.InputModel _input = new CreateUser.InputModel();

		// Token: 0x0400029F RID: 671
		private bool _busy;

		// Token: 0x040002A0 RID: 672
		private bool _actorIsOwner;

		// Token: 0x040002A1 RID: 673
		[Nullable(2)]
		private string _error;

		// Token: 0x0200018D RID: 397
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002BD RID: 701
			// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00054482 File Offset: 0x00052682
			// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0005448A File Offset: 0x0005268A
			[Required]
			[EmailAddress]
			public string Email { get; set; } = "";

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00054493 File Offset: 0x00052693
			// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0005449B File Offset: 0x0005269B
			[Required]
			[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
			public string Password { get; set; } = "";

			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x000544A4 File Offset: 0x000526A4
			// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x000544AC File Offset: 0x000526AC
			public string Role { get; set; } = "User";
		}

		// Token: 0x0200018E RID: 398
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000544DE File Offset: 0x000526DE
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return new InteractiveServerRenderMode(false);
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x000544E6 File Offset: 0x000526E6
			public override IComponentRenderMode Mode
			{
				get
				{
					return CreateUser.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
