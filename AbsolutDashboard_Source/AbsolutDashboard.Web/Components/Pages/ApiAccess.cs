using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000A6 RID: 166
	[Authorize]
	[Route("/apps/api-access")]
	public class ApiAccess : ComponentBase
	{
		// Token: 0x060006BE RID: 1726 RVA: 0x000237B8 File Offset: 0x000219B8
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "API Access — Absolut");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<header class=\"mb-6\"><h1 class=\"text-2xl font-semibold text-cocoa-900\">API Access</h1>\n    <p class=\"mt-1 text-sm text-cocoa-700\">Programmatic access to the dashboard's data.</p></header>\n\n");
			__builder.OpenComponent<PreviewBanner>(5);
			__builder.AddComponentParameter(6, "Note", "the API does not exist yet, so these controls are a layout preview only.");
			__builder.CloseComponent();
			__builder.AddMarkupContent(7, "\n\n");
			__builder.AddMarkupContent(8, "<div class=\"card mb-4 max-w-2xl space-y-5 p-6\"><h2 class=\"text-lg font-semibold text-cocoa-900\">Configuration</h2>\n\n    <label class=\"block\"><span class=\"text-sm font-medium\">API base URL</span>\n        <input class=\"input\" value=\"https://absolut.local/api/v1\" readonly>\n        <span class=\"mt-1 block text-xs text-cocoa-700\">Where clients reach this dashboard's API.</span></label>\n\n    <div class=\"block\"><span class=\"text-sm font-medium\">API key</span>\n        <div class=\"mt-1 flex items-center gap-2\"><input class=\"input mt-0 font-mono\" value=\"••••••••••••••••••••••••\" readonly>\n            <button type=\"button\" class=\"btn-secondary shrink-0 !px-4 !py-2 text-sm\">Copy</button></div>\n        <span class=\"mt-1 block text-xs text-cocoa-700\">Treat this like a password — anyone holding it can read the whole estate.</span></div>\n\n    <div><button type=\"button\" class=\"btn-primary !px-4 !py-2 text-sm\">Regenerate API key</button>\n        <span class=\"mt-1 block text-xs text-cocoa-700\">Regenerating takes effect at once and invalidates the previous key.</span></div></div>\n\n");
			__builder.AddMarkupContent(9, "<div class=\"card mb-4 max-w-2xl p-6\"><h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">API status</h2>\n    <div class=\"flex flex-wrap items-center gap-3\"><span class=\"pill pill-unknown\">Inactive</span>\n        <span class=\"text-sm text-cocoa-700\">No API service is running yet.</span></div></div>\n\n");
			__builder.AddMarkupContent(10, "<div class=\"card max-w-2xl p-6\"><h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">API usage</h2>\n    <div class=\"grid grid-cols-2 gap-4 sm:grid-cols-3\"><div><p class=\"text-sm text-cocoa-700\">Requests today</p>\n            <p class=\"mt-1 text-2xl font-semibold text-cocoa-900\">—</p></div>\n        <div><p class=\"text-sm text-cocoa-700\">Requests (30 d)</p>\n            <p class=\"mt-1 text-2xl font-semibold text-cocoa-900\">—</p></div>\n        <div><p class=\"text-sm text-cocoa-700\">Rate limit</p>\n            <p class=\"mt-1 text-2xl font-semibold text-cocoa-900\">—</p></div></div></div>");
		}
	}
}
