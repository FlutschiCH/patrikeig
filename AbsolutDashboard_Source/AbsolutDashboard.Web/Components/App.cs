using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000091 RID: 145
	public class App : ComponentBase
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x000193A8 File Offset: 0x000175A8
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.AddMarkupContent(0, "<!DOCTYPE html>\n");
			__builder.OpenElement(1, "html");
			__builder.AddAttribute(2, "lang", "en");
			__builder.OpenElement(3, "head");
			__builder.AddMarkupContent(4, "<meta charset=\"utf-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <base href=\"/\">\n    ");
			__builder.OpenElement(5, "link");
			__builder.AddAttribute(6, "rel", "stylesheet");
			__builder.AddAttribute(7, "href", base.Assets["app.css"]);
			__builder.CloseElement();
			__builder.AddMarkupContent(8, "\n    ");
			__builder.OpenElement(9, "link");
			__builder.AddAttribute(10, "rel", "stylesheet");
			__builder.AddAttribute(11, "href", base.Assets["AbsolutDashboard.Web.styles.css"]);
			__builder.CloseElement();
			__builder.AddMarkupContent(12, "\n    ");
			__builder.OpenComponent<ImportMap>(13);
			__builder.CloseComponent();
			__builder.AddMarkupContent(14, "\n    <link rel=\"icon\" type=\"image/png\" href=\"favicon.png\">\n    ");
			__builder.OpenComponent<HeadOutlet>(15);
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.AddMarkupContent(16, "\n\n");
			__builder.OpenElement(17, "body");
			__builder.OpenComponent<Routes>(18);
			__builder.CloseComponent();
			__builder.AddMarkupContent(19, "\n    <script src=\"_framework/blazor.web.js\"></script>");
			__builder.CloseElement();
			__builder.CloseElement();
		}
	}
}
