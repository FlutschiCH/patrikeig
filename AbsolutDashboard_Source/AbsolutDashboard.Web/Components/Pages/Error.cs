using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000AB RID: 171
	[NullableContext(2)]
	[Nullable(0)]
	[Authorize]
	[AllowAnonymous]
	[Route("/Error")]
	public class Error : ComponentBase
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0002AE30 File Offset: 0x00029030
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Fehler" : "Error");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.OpenElement(4, "h1");
			__builder.AddAttribute(5, "class", "text-danger");
			__builder.AddContent(6, Loc.IsGerman ? "Fehler." : "Error.");
			__builder.CloseElement();
			__builder.AddMarkupContent(7, "\n");
			__builder.OpenElement(8, "h2");
			__builder.AddAttribute(9, "class", "text-danger");
			__builder.AddContent(10, Loc.IsGerman ? "Bei der Verarbeitung deiner Anfrage ist ein Fehler aufgetreten." : "An error occurred while processing your request.");
			__builder.CloseElement();
			if (this.ShowRequestId)
			{
				__builder.OpenElement(11, "p");
				__builder.AddMarkupContent(12, "<strong>Request ID:</strong> ");
				__builder.OpenElement(13, "code");
				__builder.AddContent(14, this.RequestId);
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.AddMarkupContent(15, "<h3>Development Mode</h3>\n");
			__builder.AddMarkupContent(16, "<p>\n    Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.\n</p>\n");
			__builder.AddMarkupContent(17, "<p><strong>The Development environment shouldn't be enabled for deployed applications.</strong>\n    It can result in displaying sensitive information from exceptions to end users.\n    For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>\n    and restarting the app.\n</p>");
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002AF79 File Offset: 0x00029179
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0002AF81 File Offset: 0x00029181
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002AF8A File Offset: 0x0002918A
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0002AF92 File Offset: 0x00029192
		private string RequestId { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002AF9B File Offset: 0x0002919B
		private bool ShowRequestId
		{
			get
			{
				return !string.IsNullOrEmpty(this.RequestId);
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002AFAB File Offset: 0x000291AB
		protected override void OnInitialized()
		{
			Activity activity = Activity.Current;
			string text;
			if ((text = ((activity != null) ? activity.Id : null)) == null)
			{
				HttpContext httpContext = this.HttpContext;
				text = ((httpContext != null) ? httpContext.TraceIdentifier : null);
			}
			this.RequestId = text;
		}
	}
}
