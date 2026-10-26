using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace AbsolutDashboard.Web.Components.Users
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize(Roles = "Admin")]
	[Route("/users")]
	public class Users : ComponentBase
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x00022A7C File Offset: 0x00020C7C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Users — Absolut");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<div class=\"mb-6 flex flex-wrap items-start justify-between gap-3\"><div><h1 class=\"text-2xl font-semibold text-cocoa-900\">Users</h1>\n        <p class=\"mt-1 text-sm text-cocoa-700\">Accounts that can sign in to the dashboard.</p></div>\n    <a class=\"btn-primary !px-4 !py-2 text-sm\" href=\"users/new\">+ New user</a></div>");
			if (this._rows == null)
			{
				__builder.AddMarkupContent(5, "<p class=\"text-cocoa-700\">Loading…</p>");
				return;
			}
			if (this._rows.Count == 0)
			{
				__builder.AddMarkupContent(6, "<div class=\"card p-8 text-center text-cocoa-700\">No users yet.</div>");
				return;
			}
			__builder.OpenElement(7, "div");
			__builder.AddAttribute(8, "class", "card overflow-x-auto");
			__builder.OpenElement(9, "table");
			__builder.AddAttribute(10, "class", "w-full text-left text-sm");
			__builder.AddMarkupContent(11, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"px-4 py-3\">Email</th>\n                    <th class=\"hidden px-4 py-3 sm:table-cell\">Role</th>\n                    <th class=\"hidden px-4 py-3 lg:table-cell\">Status</th>\n                    <th class=\"px-4 py-3\"></th></tr></thead>\n            ");
			__builder.OpenElement(12, "tbody");
			foreach (Users.Row row in this._rows)
			{
				__builder.OpenElement(13, "tr");
				__builder.AddAttribute(14, "class", "border-b border-cream-100 last:border-0");
				__builder.OpenElement(15, "td");
				__builder.AddAttribute(16, "class", "px-4 py-3 font-medium text-cocoa-900");
				__builder.AddContent(17, row.Email);
				if (row.IsOwner)
				{
					__builder.AddMarkupContent(18, "<span class=\"ml-2 pill pill-up\">Owner</span>");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(19, "\n                        ");
				__builder.OpenElement(20, "td");
				__builder.AddAttribute(21, "class", "hidden px-4 py-3 sm:table-cell");
				if (row.IsAdmin)
				{
					__builder.AddMarkupContent(22, "<span class=\"pill bg-blush-100 text-blush-700\">Admin</span>");
				}
				else
				{
					__builder.AddMarkupContent(23, "<span class=\"pill pill-unknown\">User</span>");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(24, "\n                        ");
				__builder.OpenElement(25, "td");
				__builder.AddAttribute(26, "class", "hidden px-4 py-3 lg:table-cell");
				if (row.LockedOut)
				{
					__builder.AddMarkupContent(27, "<span class=\"pill pill-down\">Locked</span>");
				}
				else
				{
					__builder.AddMarkupContent(28, "<span class=\"text-cocoa-700\">Active</span>");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(29, "\n                        ");
				__builder.OpenElement(30, "td");
				__builder.AddAttribute(31, "class", "px-4 py-3 text-right");
				__builder.OpenElement(32, "a");
				__builder.AddAttribute(33, "class", "text-blush-700 hover:underline");
				__builder.AddAttribute(34, "href", "users/" + row.Id);
				__builder.AddContent(35, "Manage");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00022D5C File Offset: 0x00020F5C
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser u;
			List<ApplicationUser> list = this.UserMgr.Users.OrderBy<ApplicationUser, string>((ApplicationUser u) => u.Email).ToList<ApplicationUser>();
			List<Users.Row> rows = new List<Users.Row>(list.Count);
			using (List<ApplicationUser>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					u = enumerator.Current;
					IList<string> list2 = await this.UserMgr.GetRolesAsync(u);
					DateTimeOffset? lockoutEnd = u.LockoutEnd;
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
					bool flag2 = flag;
					rows.Add(new Users.Row(u.Id, u.Email, list2.Contains("Admin"), u.IsOwner, flag2));
					u = null;
				}
			}
			List<ApplicationUser>.Enumerator enumerator = default(List<ApplicationUser>.Enumerator);
			this._rows = rows;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00022D9F File Offset: 0x00020F9F
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00022DA7 File Offset: 0x00020FA7
		[Inject]
		private UserManager<ApplicationUser> UserMgr { get; set; }

		// Token: 0x040002B8 RID: 696
		[Nullable(new byte[] { 2, 1 })]
		private List<Users.Row> _rows;

		// Token: 0x0200019B RID: 411
		[Nullable(0)]
		private class Row : IEquatable<Users.Row>
		{
			// Token: 0x06000CF5 RID: 3317 RVA: 0x00055B16 File Offset: 0x00053D16
			public Row(string Id, [Nullable(2)] string Email, bool IsAdmin, bool IsOwner, bool LockedOut)
			{
				this.Id = Id;
				this.Email = Email;
				this.IsAdmin = IsAdmin;
				this.IsOwner = IsOwner;
				this.LockedOut = LockedOut;
				base..ctor();
			}

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00055B43 File Offset: 0x00053D43
			[CompilerGenerated]
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Users.Row);
				}
			}

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00055B4F File Offset: 0x00053D4F
			// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x00055B57 File Offset: 0x00053D57
			public string Id { get; set; }

			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00055B60 File Offset: 0x00053D60
			// (set) Token: 0x06000CFA RID: 3322 RVA: 0x00055B68 File Offset: 0x00053D68
			[Nullable(2)]
			public string Email
			{
				[NullableContext(2)]
				get;
				[NullableContext(2)]
				set;
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00055B71 File Offset: 0x00053D71
			// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00055B79 File Offset: 0x00053D79
			public bool IsAdmin { get; set; }

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00055B82 File Offset: 0x00053D82
			// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00055B8A File Offset: 0x00053D8A
			public bool IsOwner { get; set; }

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00055B93 File Offset: 0x00053D93
			// (set) Token: 0x06000D00 RID: 3328 RVA: 0x00055B9B File Offset: 0x00053D9B
			public bool LockedOut { get; set; }

			// Token: 0x06000D01 RID: 3329 RVA: 0x00055BA4 File Offset: 0x00053DA4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Row");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000D02 RID: 3330 RVA: 0x00055BF0 File Offset: 0x00053DF0
			[CompilerGenerated]
			protected virtual bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Id = ");
				builder.Append(this.Id);
				builder.Append(", Email = ");
				builder.Append(this.Email);
				builder.Append(", IsAdmin = ");
				builder.Append(this.IsAdmin.ToString());
				builder.Append(", IsOwner = ");
				builder.Append(this.IsOwner.ToString());
				builder.Append(", LockedOut = ");
				builder.Append(this.LockedOut.ToString());
				return true;
			}

			// Token: 0x06000D03 RID: 3331 RVA: 0x00055CAA File Offset: 0x00053EAA
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(Users.Row left, Users.Row right)
			{
				return !(left == right);
			}

			// Token: 0x06000D04 RID: 3332 RVA: 0x00055CB6 File Offset: 0x00053EB6
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(Users.Row left, Users.Row right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000D05 RID: 3333 RVA: 0x00055CCC File Offset: 0x00053ECC
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Id>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Email>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsAdmin>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsOwner>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<LockedOut>k__BackingField);
			}

			// Token: 0x06000D06 RID: 3334 RVA: 0x00055D5C File Offset: 0x00053F5C
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as Users.Row);
			}

			// Token: 0x06000D07 RID: 3335 RVA: 0x00055D6C File Offset: 0x00053F6C
			[NullableContext(2)]
			[CompilerGenerated]
			public virtual bool Equals(Users.Row other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Email>k__BackingField, other.<Email>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsAdmin>k__BackingField, other.<IsAdmin>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsOwner>k__BackingField, other.<IsOwner>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<LockedOut>k__BackingField, other.<LockedOut>k__BackingField));
			}

			// Token: 0x06000D09 RID: 3337 RVA: 0x00055E1C File Offset: 0x0005401C
			[CompilerGenerated]
			protected Row(Users.Row original)
			{
				this.Id = original.<Id>k__BackingField;
				this.Email = original.<Email>k__BackingField;
				this.IsAdmin = original.<IsAdmin>k__BackingField;
				this.IsOwner = original.<IsOwner>k__BackingField;
				this.LockedOut = original.<LockedOut>k__BackingField;
			}

			// Token: 0x06000D0A RID: 3338 RVA: 0x00055E6B File Offset: 0x0005406B
			[CompilerGenerated]
			public void Deconstruct(out string Id, [Nullable(2)] out string Email, out bool IsAdmin, out bool IsOwner, out bool LockedOut)
			{
				Id = this.Id;
				Email = this.Email;
				IsAdmin = this.IsAdmin;
				IsOwner = this.IsOwner;
				LockedOut = this.LockedOut;
			}
		}
	}
}
