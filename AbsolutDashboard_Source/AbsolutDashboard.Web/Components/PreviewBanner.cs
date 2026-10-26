using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public class PreviewBanner : ComponentBase
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x0001F784 File Offset: 0x0001D984
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card mb-4 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
			__builder.AddMarkupContent(2, "<b>Preview</b> — ");
			__builder.AddContent(3, this.Note);
			__builder.CloseElement();
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001F7C2 File Offset: 0x0001D9C2
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x0001F7CA File Offset: 0x0001D9CA
		[Parameter]
		public string Note { get; set; } = "nothing on this page is wired up yet.";
	}
}
