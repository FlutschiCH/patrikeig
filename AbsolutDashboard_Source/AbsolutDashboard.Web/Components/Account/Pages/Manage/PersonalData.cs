using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/PersonalData")]
	public class PersonalData : ComponentBase
	{
		// Token: 0x06000862 RID: 2146 RVA: 0x00031974 File Offset: 0x0002FB74
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Persönliche Daten" : "Personal data");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "div");
			__builder.AddAttribute(5, "class", "space-y-6");
			__builder.OpenElement(6, "div");
			__builder.OpenElement(7, "h2");
			__builder.AddAttribute(8, "class", "font-display text-2xl text-cocoa-900");
			__builder.AddContent(9, Loc.IsGerman ? "Persönliche Daten" : "Personal data");
			__builder.CloseElement();
			__builder.AddMarkupContent(10, "\n        ");
			__builder.OpenElement(11, "p");
			__builder.AddAttribute(12, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(13, Loc.IsGerman ? "Lade deine Daten herunter oder lösche dein Konto." : "Download or delete your account data.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(14, "\n\n    ");
			__builder.OpenComponent<StatusMessage>(15);
			__builder.CloseComponent();
			if (this.isOwner)
			{
				__builder.OpenElement(16, "div");
				__builder.AddAttribute(17, "class", "rounded-xl bg-cream-100 px-4 py-3 text-sm text-cocoa-700 ring-1 ring-blush-100");
				__builder.AddContent(18, Loc.IsGerman ? "Dies ist das Inhaberkonto. Es kann nicht heruntergeladen oder gelöscht werden." : "This is the owner account. It can't be downloaded or deleted here.");
				__builder.CloseElement();
			}
			else
			{
				__builder.OpenElement(19, "div");
				__builder.AddAttribute(20, "class", "space-y-4 text-sm text-cocoa-700");
				__builder.OpenElement(21, "p");
				__builder.AddContent(22, Loc.IsGerman ? "Dein Konto enthält persönliche Daten, die du uns zur Verfügung gestellt hast. Hier kannst du diese Daten herunterladen oder löschen." : "Your account contains personal data that you have given us. This page allows you to download or delete that data.");
				__builder.CloseElement();
				__builder.AddMarkupContent(23, "\n            ");
				__builder.OpenElement(24, "div");
				__builder.AddAttribute(25, "class", "rounded-xl bg-amber-50 px-4 py-3 text-amber-800 ring-1 ring-amber-200");
				__builder.OpenElement(26, "strong");
				__builder.AddContent(27, Loc.IsGerman ? "Das Löschen entfernt dein Konto dauerhaft und kann nicht rückgängig gemacht werden." : "Deleting this data will permanently remove your account, and this cannot be recovered.");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.OpenElement(28, "div");
				__builder.AddAttribute(29, "class", "flex flex-wrap items-center gap-3");
				__builder.OpenElement(30, "form");
				__builder.AddAttribute(31, "action", "Account/Manage/DownloadPersonalData");
				__builder.AddAttribute(32, "method", "post");
				__builder.OpenComponent<AntiforgeryToken>(33);
				__builder.CloseComponent();
				__builder.AddMarkupContent(34, "\n                ");
				__builder.OpenElement(35, "button");
				__builder.AddAttribute(36, "class", "btn-primary");
				__builder.AddAttribute(37, "type", "submit");
				__builder.AddContent(38, Loc.IsGerman ? "Herunterladen" : "Download");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(39, "\n            ");
				__builder.OpenElement(40, "a");
				__builder.AddAttribute(41, "href", "Account/Manage/DeletePersonalData");
				__builder.AddAttribute(42, "class", "btn-danger");
				__builder.AddContent(43, Loc.IsGerman ? "Löschen" : "Delete");
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00031CC8 File Offset: 0x0002FEC8
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x00031CD0 File Offset: 0x0002FED0
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x06000865 RID: 2149 RVA: 0x00031CDC File Offset: 0x0002FEDC
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.isOwner = applicationUser.IsOwner;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00031D1F File Offset: 0x0002FF1F
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x00031D27 File Offset: 0x0002FF27
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x040003A6 RID: 934
		private bool isOwner;
	}
}
