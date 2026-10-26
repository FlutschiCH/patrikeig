using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components.Layout
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	public class NavMenu : ComponentBase, IDisposable
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x00020F04 File Offset: 0x0001F104
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "aside");
			__builder.AddAttribute(1, "class", "flex w-60 shrink-0 flex-col border-r border-cream-300 bg-white");
			__builder.AddAttribute(2, "b-fmphmuxk3u");
			__builder.OpenElement(3, "div");
			__builder.AddAttribute(4, "class", "border-b border-cream-200 px-5 py-4");
			__builder.AddAttribute(5, "b-fmphmuxk3u");
			__builder.OpenElement(6, "a");
			__builder.AddAttribute(7, "href");
			__builder.AddAttribute(8, "class", "flex flex-col leading-tight");
			__builder.AddAttribute(9, "b-fmphmuxk3u");
			__builder.AddMarkupContent(10, "<span class=\"text-lg font-semibold text-cocoa-900\" b-fmphmuxk3u>Absolut</span>\n            ");
			__builder.AddMarkupContent(11, "<span class=\"text-xs tracking-wide text-cocoa-700\" b-fmphmuxk3u>5G Network Monitor</span>\n            ");
			__builder.OpenElement(12, "span");
			__builder.AddAttribute(13, "class", "mt-0.5 text-[10px] text-cocoa-500");
			__builder.AddAttribute(14, "title", AppVersion.Full);
			__builder.AddAttribute(15, "b-fmphmuxk3u");
			__builder.AddContent(16, AppVersion.Display);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(17, "\n\n    ");
			__builder.OpenElement(18, "nav");
			__builder.AddAttribute(19, "class", "flex-1 overflow-y-auto px-3 py-4");
			__builder.AddAttribute(20, "b-fmphmuxk3u");
			__builder.OpenComponent<NavLink>(21);
			__builder.AddComponentParameter(22, "class", "nav-item");
			__builder.AddComponentParameter(23, "href", "");
			__builder.AddComponentParameter(24, "Match", RuntimeHelpers.TypeCheck<NavLinkMatch>(NavLinkMatch.All));
			__builder.AddAttribute(25, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(26, "<span class=\"nav-icon\" b-fmphmuxk3u>▣</span> Dashboard\n        ");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(27, "\n\n        ");
			__builder.AddMarkupContent(28, "<p class=\"mt-5 mb-1 px-3 text-xs font-semibold uppercase tracking-wide text-cocoa-700\" b-fmphmuxk3u>Devices</p>");
			if (this._categories != null)
			{
				using (IEnumerator<DeviceCategory> enumerator = this._categories.Where<DeviceCategory>((DeviceCategory c) => c.Key != "ims").GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DeviceCategory c = enumerator.Current;
						__builder.OpenComponent<NavLink>(29);
						__builder.AddComponentParameter(30, "class", "nav-item");
						__builder.AddComponentParameter(31, "href", "devices/" + c.Key);
						__builder.AddAttribute(32, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
						{
							__builder2.OpenElement(33, "span");
							__builder2.AddAttribute(34, "class", "nav-chip");
							__builder2.AddAttribute(35, "b-fmphmuxk3u");
							__builder2.AddContent(36, c.ShortName);
							__builder2.CloseElement();
							__builder2.AddContent(37, " ");
							__builder2.AddContent(38, c.Name);
						}));
						__builder.CloseComponent();
					}
				}
			}
			__builder.AddMarkupContent(39, "<p class=\"mt-5 mb-1 px-3 text-xs font-semibold uppercase tracking-wide text-cocoa-700\" b-fmphmuxk3u>Absolut Apps</p>\n        ");
			__builder.OpenComponent<NavLink>(40);
			__builder.AddComponentParameter(41, "class", "nav-item");
			__builder.AddComponentParameter(42, "href", "apps/api-access");
			__builder.AddAttribute(43, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(44, "<span class=\"nav-icon\" b-fmphmuxk3u>⇄</span> API Access\n        ");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(45, "\n        ");
			__builder.OpenComponent<NavLink>(46);
			__builder.AddComponentParameter(47, "class", "nav-item");
			__builder.AddComponentParameter(48, "href", "apps/ue-localization");
			__builder.AddAttribute(49, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(50, "<span class=\"nav-icon\" b-fmphmuxk3u>◎</span> UE Localization\n        ");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(51, "\n        ");
			__builder.OpenComponent<NavLink>(52);
			__builder.AddComponentParameter(53, "class", "nav-item");
			__builder.AddComponentParameter(54, "href", "devices/ims");
			__builder.AddAttribute(55, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(56, "<span class=\"nav-icon\" b-fmphmuxk3u>☏</span> Voice server (IMS)\n        ");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(57, "\n\n        ");
			__builder.OpenComponent<AuthorizeView>(58);
			__builder.AddComponentParameter(59, "Roles", "Admin");
			__builder.AddAttribute(60, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(61, "<p class=\"mt-5 mb-1 px-3 text-xs font-semibold uppercase tracking-wide text-cocoa-700\" b-fmphmuxk3u>Admin</p>\n            ");
				__builder2.OpenComponent<NavLink>(62);
				__builder2.AddComponentParameter(63, "class", "nav-item");
				__builder2.AddComponentParameter(64, "href", "users");
				__builder2.AddAttribute(65, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder3)
				{
					__builder3.AddMarkupContent(66, "<span class=\"nav-icon\" b-fmphmuxk3u>◷</span> Users\n            ");
				}));
				__builder2.CloseComponent();
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.AddMarkupContent(67, "\n\n    ");
			__builder.OpenComponent<AuthorizeView>(68);
			__builder.AddAttribute(69, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(70, "div");
				__builder2.AddAttribute(71, "class", "border-t border-cream-200 px-3 py-3");
				__builder2.AddAttribute(72, "b-fmphmuxk3u");
				__builder2.OpenElement(73, "a");
				__builder2.AddAttribute(74, "href", "Account/Manage");
				__builder2.AddAttribute(75, "class", "nav-item");
				int num = 76;
				string text = "title";
				IIdentity identity = context.User.Identity;
				__builder2.AddAttribute(num, text, (identity != null) ? identity.Name : null);
				__builder2.AddAttribute(77, "b-fmphmuxk3u");
				__builder2.AddMarkupContent(78, "<span class=\"nav-icon\" b-fmphmuxk3u>●</span>\n                ");
				__builder2.OpenElement(79, "span");
				__builder2.AddAttribute(80, "class", "truncate");
				__builder2.AddAttribute(81, "b-fmphmuxk3u");
				int num2 = 82;
				IIdentity identity2 = context.User.Identity;
				__builder2.AddContent(num2, (identity2 != null) ? identity2.Name : null);
				__builder2.CloseElement();
				__builder2.CloseElement();
				__builder2.AddMarkupContent(83, "\n            ");
				__builder2.OpenElement(84, "form");
				__builder2.AddAttribute(85, "action", "Account/Logout");
				__builder2.AddAttribute(86, "method", "post");
				__builder2.AddAttribute(87, "class", "mt-1");
				__builder2.AddAttribute(88, "b-fmphmuxk3u");
				__builder2.OpenComponent<AntiforgeryToken>(89);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(90, "\n                ");
				__builder2.OpenElement(91, "input");
				__builder2.AddAttribute(92, "type", "hidden");
				__builder2.AddAttribute(93, "name", "ReturnUrl");
				__builder2.AddAttribute(94, "value", this._currentUrl);
				__builder2.AddAttribute(95, "b-fmphmuxk3u");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(96, "\n                ");
				__builder2.AddMarkupContent(97, "<button type=\"submit\" class=\"nav-item w-full text-left text-blush-700 hover:bg-blush-50\" b-fmphmuxk3u><span class=\"nav-icon\" b-fmphmuxk3u>⮌</span> Log out\n                </button>");
				__builder2.CloseElement();
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00021384 File Offset: 0x0001F584
		protected override async Task OnInitializedAsync()
		{
			this._currentUrl = this.Nav.ToBaseRelativePath(this.Nav.Uri);
			this.Nav.LocationChanged += this.OnLocationChanged;
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<DeviceCategory> deviceCategories = db.DeviceCategories;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(DeviceCategory), "c");
				this._categories = (await deviceCategories.OrderBy<DeviceCategory, int>(Expression.Lambda<Func<DeviceCategory, int>>(Expression.Property(parameterExpression, methodof(DeviceCategory.get_DisplayOrder())), new ParameterExpression[] { parameterExpression })).ToListAsync<DeviceCategory>(default(CancellationToken))).Where<DeviceCategory>((DeviceCategory c) => !c.IsNavHidden).ToList<DeviceCategory>();
				num = 1;
			}
			catch (object obj)
			{
			}
			if (db != null)
			{
				await db.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return;
			}
			obj = null;
			db = null;
			throw null;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000213C7 File Offset: 0x0001F5C7
		private void OnLocationChanged([Nullable(2)] object sender, LocationChangedEventArgs e)
		{
			this._currentUrl = this.Nav.ToBaseRelativePath(e.Location);
			base.StateHasChanged();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000213E6 File Offset: 0x0001F5E6
		public void Dispose()
		{
			this.Nav.LocationChanged -= this.OnLocationChanged;
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x000213FF File Offset: 0x0001F5FF
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00021407 File Offset: 0x0001F607
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00021410 File Offset: 0x0001F610
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00021418 File Offset: 0x0001F618
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x04000299 RID: 665
		[Nullable(new byte[] { 2, 1 })]
		private List<DeviceCategory> _categories;

		// Token: 0x0400029A RID: 666
		[Nullable(2)]
		private string _currentUrl;
	}
}
