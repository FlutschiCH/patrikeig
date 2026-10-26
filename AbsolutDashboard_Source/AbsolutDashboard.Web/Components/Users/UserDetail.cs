using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Components.Users
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize(Roles = "Admin")]
	[Route("/users/{Id}")]
	[UserDetail.__PrivateComponentRenderModeAttribute]
	public class UserDetail : ComponentBase
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x00021B18 File Offset: 0x0001FD18
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "User — Absolut");
			}));
			__builder.CloseComponent();
			if (this._user == null)
			{
				__builder.OpenElement(3, "p");
				__builder.AddAttribute(4, "class", "text-cocoa-700");
				__builder.AddContent(5, this._loaded ? "User not found." : "Loading…");
				__builder.CloseElement();
				return;
			}
			__builder.OpenElement(6, "div");
			__builder.AddAttribute(7, "class", "flex items-center justify-between");
			__builder.OpenElement(8, "h1");
			__builder.AddAttribute(9, "class", "text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(10, this._user.Email);
			__builder.CloseElement();
			__builder.AddMarkupContent(11, "\n        ");
			__builder.AddMarkupContent(12, "<a class=\"btn-back\" href=\"users\">← Users</a>");
			__builder.CloseElement();
			if (this._isSelf)
			{
				__builder.AddMarkupContent(13, "<div class=\"card mt-4 border-l-4 border-amber-500 bg-amber-100 p-3 text-sm text-amber-800\">\n            This is your own account. Manage it from your\n            <a class=\"font-medium underline\" href=\"Account/Manage\">account page</a>.\n        </div>");
			}
			else if (this._isOwner)
			{
				__builder.AddMarkupContent(14, "<div class=\"card mt-4 border-l-4 border-moss-500 bg-moss-100 p-3 text-sm text-moss-700\">\n            This is the <b>owner</b> account. Only the owner can change it, and it can't be deleted.\n        </div>");
			}
			else if (!this._canManage)
			{
				__builder.AddMarkupContent(15, "<div class=\"card mt-4 border-l-4 border-amber-500 bg-amber-100 p-3 text-sm text-amber-800\">\n            Only the owner can manage other admin accounts.\n        </div>");
			}
			if (this._status != null)
			{
				__builder.OpenElement(16, "div");
				__builder.AddAttribute(17, "class", "card mt-4 border-l-4 border-moss-500 bg-moss-100 p-3 text-sm text-moss-700");
				__builder.AddContent(18, this._status);
				__builder.CloseElement();
			}
			if (this._error != null)
			{
				__builder.OpenElement(19, "div");
				__builder.AddAttribute(20, "class", "card mt-4 border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
				__builder.AddContent(21, this._error);
				__builder.CloseElement();
			}
			__builder.OpenElement(22, "div");
			__builder.AddAttribute(23, "class", "mt-6 grid gap-6 " + (this._canManage ? "lg:grid-cols-3" : ""));
			__builder.OpenElement(24, "div");
			__builder.AddAttribute(25, "class", "card p-6 " + (this._canManage ? "lg:col-span-2" : ""));
			__builder.AddMarkupContent(26, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Account</h2>\n            ");
			__builder.OpenElement(27, "dl");
			__builder.AddAttribute(28, "class", "mt-3 grid grid-cols-3 gap-y-2 text-sm");
			__builder.AddMarkupContent(29, "<dt class=\"text-cocoa-700\">Role</dt>\n                ");
			__builder.OpenElement(30, "dd");
			__builder.AddAttribute(31, "class", "col-span-2");
			if (this._isAdmin)
			{
				__builder.AddMarkupContent(32, "<span class=\"pill bg-blush-100 text-blush-700\">Admin</span>");
			}
			else
			{
				__builder.AddMarkupContent(33, "<span class=\"pill pill-unknown\">User</span>");
			}
			__builder.CloseElement();
			__builder.AddMarkupContent(34, "\n                ");
			__builder.AddMarkupContent(35, "<dt class=\"text-cocoa-700\">Two-factor</dt>\n                ");
			__builder.OpenElement(36, "dd");
			__builder.AddAttribute(37, "class", "col-span-2");
			__builder.AddContent(38, this._user.TwoFactorEnabled ? "Enabled" : "Off");
			__builder.CloseElement();
			__builder.AddMarkupContent(39, "\n                ");
			__builder.AddMarkupContent(40, "<dt class=\"text-cocoa-700\">Lockout</dt>\n                ");
			__builder.OpenElement(41, "dd");
			__builder.AddAttribute(42, "class", "col-span-2");
			int num = 43;
			string text;
			if (!this._lockedOut)
			{
				text = "Active";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Locked (until ");
				defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset?>(this._user.LockoutEnd, "dd.MM.yyyy");
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			__builder.AddContent(num, text);
			__builder.CloseElement();
			__builder.AddMarkupContent(44, "\n                ");
			__builder.AddMarkupContent(45, "<dt class=\"text-cocoa-700\">Failed logins</dt>\n                ");
			__builder.OpenElement(46, "dd");
			__builder.AddAttribute(47, "class", "col-span-2");
			__builder.AddContent(48, this._user.AccessFailedCount);
			__builder.CloseElement();
			__builder.AddMarkupContent(49, "\n                ");
			__builder.AddMarkupContent(50, "<dt class=\"text-cocoa-700\">User ID</dt>\n                ");
			__builder.OpenElement(51, "dd");
			__builder.AddAttribute(52, "class", "col-span-2 break-all text-xs text-cocoa-700");
			__builder.AddContent(53, this._user.Id);
			__builder.CloseElement();
			__builder.CloseElement();
			if (this._canManage)
			{
				__builder.AddMarkupContent(54, "<hr class=\"my-5 border-cream-200\">\n                ");
				__builder.AddMarkupContent(55, "<h3 class=\"font-medium text-cocoa-900\">Change email</h3>\n                ");
				__builder.OpenElement(56, "div");
				__builder.AddAttribute(57, "class", "mt-2 flex flex-col gap-2 sm:flex-row sm:items-end");
				__builder.OpenElement(58, "label");
				__builder.AddAttribute(59, "class", "flex-1 text-sm");
				__builder.AddMarkupContent(60, "Email address\n                        ");
				__builder.OpenElement(61, "input");
				__builder.AddAttribute(62, "type", "email");
				__builder.AddAttribute(63, "class", "input");
				__builder.AddAttribute(64, "value", BindConverter.FormatValue(this._email, null));
				__builder.AddAttribute<ChangeEventArgs>(65, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._email = __value;
				}, this._email, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(66, "\n                    ");
				__builder.OpenElement(67, "button");
				__builder.AddAttribute(68, "class", "btn-secondary !px-4 !py-2 text-sm");
				__builder.AddAttribute<MouseEventArgs>(69, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.SaveEmail)));
				__builder.AddAttribute(70, "disabled", this._busy);
				__builder.AddContent(71, "Save email");
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			if (this._canManage)
			{
				__builder.OpenElement(72, "div");
				__builder.AddAttribute(73, "class", "space-y-6");
				__builder.OpenElement(74, "div");
				__builder.AddAttribute(75, "class", "card p-6");
				__builder.AddMarkupContent(76, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Role</h2>\n                    ");
				__builder.AddMarkupContent(77, "<p class=\"mt-1 text-xs text-cocoa-700\">Admins can manage users and add/edit devices.</p>");
				if (this._isAdmin)
				{
					__builder.OpenElement(78, "button");
					__builder.AddAttribute(79, "class", "btn-secondary mt-3 w-full");
					__builder.AddAttribute<MouseEventArgs>(80, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.SetAdmin(false)));
					__builder.AddAttribute(81, "disabled", this._busy);
					__builder.AddContent(82, "Remove admin role");
					__builder.CloseElement();
				}
				else if (this._actorIsOwner)
				{
					__builder.OpenElement(83, "button");
					__builder.AddAttribute(84, "class", "btn-primary mt-3 w-full");
					__builder.AddAttribute<MouseEventArgs>(85, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.SetAdmin(true)));
					__builder.AddAttribute(86, "disabled", this._busy);
					__builder.AddContent(87, "Promote to admin");
					__builder.CloseElement();
				}
				else
				{
					__builder.AddMarkupContent(88, "<div class=\"mt-3 rounded-lg border-l-4 border-amber-500 bg-amber-100 p-3 text-sm text-amber-800\">\n                            Only the owner can promote users to admin.\n                        </div>");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(89, "\n\n                ");
				__builder.OpenElement(90, "div");
				__builder.AddAttribute(91, "class", "card p-6");
				__builder.AddMarkupContent(92, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Access</h2>\n                    ");
				__builder.AddMarkupContent(93, "<p class=\"mt-1 text-xs text-cocoa-700\">Locking blocks sign-in without deleting the account.</p>");
				if (this._lockedOut)
				{
					__builder.OpenElement(94, "button");
					__builder.AddAttribute(95, "class", "btn-secondary mt-3 w-full");
					__builder.AddAttribute<MouseEventArgs>(96, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.SetLock(false)));
					__builder.AddAttribute(97, "disabled", this._busy);
					__builder.AddContent(98, "Unlock account");
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(99, "button");
					__builder.AddAttribute(100, "class", "btn-secondary mt-3 w-full");
					__builder.AddAttribute<MouseEventArgs>(101, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.SetLock(true)));
					__builder.AddAttribute(102, "disabled", this._busy);
					__builder.AddContent(103, "Lock account");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(104, "\n\n                ");
				__builder.OpenElement(105, "div");
				__builder.AddAttribute(106, "class", "card p-6");
				__builder.AddMarkupContent(107, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Reset password</h2>\n                    ");
				__builder.AddMarkupContent(108, "<p class=\"mt-1 text-xs text-cocoa-700\">Sets a new password immediately and signs the user out everywhere.</p>\n                    ");
				__builder.OpenElement(109, "label");
				__builder.AddAttribute(110, "class", "mt-3 block text-sm");
				__builder.AddMarkupContent(111, "New password\n                        ");
				__builder.OpenElement(112, "input");
				__builder.AddAttribute(113, "type", "text");
				__builder.AddAttribute(114, "class", "input");
				__builder.AddAttribute(115, "placeholder", "At least 6 characters");
				__builder.AddAttribute(116, "value", BindConverter.FormatValue(this._newPassword, null));
				__builder.AddAttribute<ChangeEventArgs>(117, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._newPassword = __value;
				}, this._newPassword, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(118, "\n                    ");
				__builder.OpenElement(119, "button");
				__builder.AddAttribute(120, "class", "btn-primary mt-3 w-full");
				__builder.AddAttribute<MouseEventArgs>(121, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.ResetPassword)));
				__builder.AddAttribute(122, "disabled", this._busy);
				__builder.AddContent(123, "Set password");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			if (this._canManage)
			{
				__builder.OpenElement(124, "div");
				__builder.AddAttribute(125, "class", "card mt-6 border border-red-200 p-6");
				__builder.AddMarkupContent(126, "<h2 class=\"text-lg font-semibold text-red-700\">Danger zone</h2>\n            ");
				__builder.AddMarkupContent(127, "<p class=\"mt-1 text-sm text-cocoa-700\">Permanently delete this account. This can't be undone.</p>");
				if (!this._confirmDelete)
				{
					__builder.OpenElement(128, "button");
					__builder.AddAttribute(129, "class", "btn-danger mt-3");
					__builder.AddAttribute<MouseEventArgs>(130, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
					{
						this._confirmDelete = true;
					}));
					__builder.AddAttribute(131, "disabled", this._busy);
					__builder.AddContent(132, "Delete account");
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(133, "p");
					__builder.AddAttribute(134, "class", "mt-3 text-sm font-medium text-red-700");
					__builder.AddContent(135, "Are you sure? This permanently removes ");
					__builder.AddContent(136, this._user.Email);
					__builder.AddContent(137, ".");
					__builder.CloseElement();
					__builder.AddMarkupContent(138, "\n                ");
					__builder.OpenElement(139, "div");
					__builder.AddAttribute(140, "class", "mt-2 flex gap-3");
					__builder.OpenElement(141, "button");
					__builder.AddAttribute(142, "class", "btn-danger");
					__builder.AddAttribute<MouseEventArgs>(143, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.Delete)));
					__builder.AddAttribute(144, "disabled", this._busy);
					__builder.AddContent(145, "Yes, delete");
					__builder.CloseElement();
					__builder.AddMarkupContent(146, "\n                    ");
					__builder.OpenElement(147, "button");
					__builder.AddAttribute(148, "class", "btn-secondary");
					__builder.AddAttribute<MouseEventArgs>(149, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
					{
						this._confirmDelete = false;
					}));
					__builder.AddAttribute(150, "disabled", this._busy);
					__builder.AddContent(151, "Cancel");
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00022770 File Offset: 0x00020970
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00022778 File Offset: 0x00020978
		[Parameter]
		public string Id { get; set; } = "";

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00022781 File Offset: 0x00020981
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00022789 File Offset: 0x00020989
		[Nullable(new byte[] { 2, 1 })]
		[CascadingParameter]
		private Task<AuthenticationState> AuthState
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00022794 File Offset: 0x00020994
		protected override async Task OnInitializedAsync()
		{
			await this.LoadAsync();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000227D8 File Offset: 0x000209D8
		private async Task LoadAsync()
		{
			ApplicationUser applicationUser = await this.Users.FindByIdAsync(this.Id);
			this._user = applicationUser;
			this._loaded = true;
			if (this._user != null)
			{
				IList<string> list = await this.Users.GetRolesAsync(this._user);
				this._isAdmin = list.Contains("Admin");
				DateTimeOffset? lockoutEnd = this._user.LockoutEnd;
				bool flag;
				if (lockoutEnd != null)
				{
					DateTimeOffset valueOrDefault = lockoutEnd.GetValueOrDefault();
					flag = valueOrDefault > DateTimeOffset.UtcNow;
				}
				else
				{
					flag = false;
				}
				this._lockedOut = flag;
				this._email = this._user.Email ?? "";
				this._isOwner = this._user.IsOwner;
				if (this.AuthState != null)
				{
					UserManager<ApplicationUser> userManager = this.Users;
					ApplicationUser applicationUser2 = await userManager.GetUserAsync((await this.AuthState).User);
					userManager = null;
					this._isSelf = applicationUser2 != null && applicationUser2.Id == this._user.Id;
					this._actorIsOwner = applicationUser2 != null && applicationUser2.IsOwner;
				}
				this._canManage = !this._isSelf && !this._isOwner && (this._actorIsOwner || !this._isAdmin);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002281B File Offset: 0x00020A1B
		private bool EnsureManageable()
		{
			if (this._canManage)
			{
				return true;
			}
			this._error = "You don't have permission to manage this account.";
			this._status = null;
			return false;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002283C File Offset: 0x00020A3C
		private async Task SetAdmin(bool makeAdmin)
		{
			if (this._user != null && this.EnsureManageable())
			{
				if (makeAdmin && !this._actorIsOwner)
				{
					this._error = "Only the owner can promote users to admin.";
					this._status = null;
				}
				else
				{
					this._busy = true;
					this._error = (this._status = null);
					try
					{
						IdentityResult identityResult;
						if (makeAdmin)
						{
							identityResult = await this.Users.AddToRoleAsync(this._user, "Admin");
						}
						else
						{
							identityResult = await this.Users.RemoveFromRoleAsync(this._user, "Admin");
						}
						IdentityResult identityResult2 = identityResult;
						if (!identityResult2.Succeeded)
						{
							this._error = UserDetail.Describe(identityResult2);
						}
						else
						{
							await this.Users.UpdateSecurityStampAsync(this._user);
							this.Logger.LogInformation("Admin role for {Email} set to {Value}.", new object[]
							{
								this._user.Email,
								makeAdmin
							});
							this._status = (makeAdmin ? "Promoted to admin." : "Admin role removed.");
							await this.LoadAsync();
						}
					}
					finally
					{
						this._busy = false;
					}
				}
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022888 File Offset: 0x00020A88
		private async Task SetLock(bool locked)
		{
			if (this._user != null && this.EnsureManageable())
			{
				this._busy = true;
				this._error = (this._status = null);
				try
				{
					if (locked)
					{
						await this.Users.SetLockoutEnabledAsync(this._user, true);
						await this.Users.SetLockoutEndDateAsync(this._user, new DateTimeOffset?(DateTimeOffset.UtcNow.AddYears(100)));
						this._status = "Account locked.";
					}
					else
					{
						await this.Users.SetLockoutEndDateAsync(this._user, null);
						await this.Users.ResetAccessFailedCountAsync(this._user);
						this._status = "Account unlocked.";
					}
					await this.LoadAsync();
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000228D4 File Offset: 0x00020AD4
		private async Task ResetPassword()
		{
			if (this._user != null && this.EnsureManageable())
			{
				if (string.IsNullOrWhiteSpace(this._newPassword))
				{
					this._error = "Enter a new password.";
					this._status = null;
				}
				else
				{
					this._busy = true;
					this._error = (this._status = null);
					try
					{
						string text = await this.Users.GeneratePasswordResetTokenAsync(this._user);
						IdentityResult identityResult = await this.Users.ResetPasswordAsync(this._user, text, this._newPassword);
						if (identityResult.Succeeded)
						{
							this.Logger.LogInformation("Password reset for {Email} by admin.", new object[] { this._user.Email });
							this._newPassword = "";
							this._status = "Password reset. The user is signed out everywhere.";
						}
						else
						{
							this._error = UserDetail.Describe(identityResult);
						}
						await this.LoadAsync();
					}
					finally
					{
						this._busy = false;
					}
				}
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00022918 File Offset: 0x00020B18
		private async Task SaveEmail()
		{
			if (this._user != null && this.EnsureManageable())
			{
				string email = this._email.Trim();
				if (string.IsNullOrWhiteSpace(email))
				{
					this._error = "Enter an email address.";
					this._status = null;
				}
				else if (!string.Equals(email, this._user.Email, StringComparison.OrdinalIgnoreCase))
				{
					this._busy = true;
					this._error = (this._status = null);
					try
					{
						IdentityResult identityResult = await this.Users.SetEmailAsync(this._user, email);
						if (!identityResult.Succeeded)
						{
							this._error = UserDetail.Describe(identityResult);
						}
						else
						{
							IdentityResult identityResult2 = await this.Users.SetUserNameAsync(this._user, email);
							if (!identityResult2.Succeeded)
							{
								this._error = UserDetail.Describe(identityResult2);
							}
							else
							{
								this._user.EmailConfirmed = true;
								await this.Users.UpdateAsync(this._user);
								this._status = "Email updated.";
								await this.LoadAsync();
							}
						}
					}
					finally
					{
						this._busy = false;
					}
				}
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002295C File Offset: 0x00020B5C
		private async Task Delete()
		{
			if (this._user != null && this.EnsureManageable())
			{
				this._busy = true;
				this._error = (this._status = null);
				try
				{
					IdentityResult identityResult = await this.Users.DeleteAsync(this._user);
					if (identityResult.Succeeded)
					{
						this.Logger.LogInformation("User {Email} deleted by admin.", new object[] { this._user.Email });
						this.Nav.NavigateTo("users", false, false);
					}
					else
					{
						this._error = UserDetail.Describe(identityResult);
						this._busy = false;
					}
				}
				catch
				{
					this._busy = false;
					throw;
				}
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002299F File Offset: 0x00020B9F
		private static string Describe(IdentityResult r)
		{
			return string.Join(" ", r.Errors.Select<IdentityError, string>((IdentityError e) => e.Description));
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x000229D5 File Offset: 0x00020BD5
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x000229DD File Offset: 0x00020BDD
		[Inject]
		private ILogger<UserDetail> Logger { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x000229E6 File Offset: 0x00020BE6
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x000229EE File Offset: 0x00020BEE
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x000229F7 File Offset: 0x00020BF7
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x000229FF File Offset: 0x00020BFF
		[Inject]
		private UserManager<ApplicationUser> Users { get; set; }

		// Token: 0x040002A7 RID: 679
		[Nullable(2)]
		private ApplicationUser _user;

		// Token: 0x040002A8 RID: 680
		private bool _loaded;

		// Token: 0x040002A9 RID: 681
		private bool _busy;

		// Token: 0x040002AA RID: 682
		private bool _confirmDelete;

		// Token: 0x040002AB RID: 683
		private bool _isAdmin;

		// Token: 0x040002AC RID: 684
		private bool _isOwner;

		// Token: 0x040002AD RID: 685
		private bool _isSelf;

		// Token: 0x040002AE RID: 686
		private bool _actorIsOwner;

		// Token: 0x040002AF RID: 687
		private bool _lockedOut;

		// Token: 0x040002B0 RID: 688
		[Nullable(2)]
		private string _status;

		// Token: 0x040002B1 RID: 689
		[Nullable(2)]
		private string _error;

		// Token: 0x040002B2 RID: 690
		private string _email = "";

		// Token: 0x040002B3 RID: 691
		private string _newPassword = "";

		// Token: 0x040002B4 RID: 692
		private bool _canManage;

		// Token: 0x02000192 RID: 402
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0005495E File Offset: 0x00052B5E
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return new InteractiveServerRenderMode(false);
				}
			}

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00054966 File Offset: 0x00052B66
			public override IComponentRenderMode Mode
			{
				get
				{
					return UserDetail.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
