using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	[Nullable(0)]
	public class ComingSoon : ComponentBase
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card p-8 text-center");
			__builder.AddMarkupContent(2, "<p class=\"text-lg font-semibold text-cocoa-900\">Coming soon</p>\n    ");
			__builder.OpenElement(3, "p");
			__builder.AddAttribute(4, "class", "mx-auto mt-1 max-w-md text-sm text-cocoa-700");
			__builder.AddContent(5, this.Note);
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001CC3C File Offset: 0x0001AE3C
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x0001CC44 File Offset: 0x0001AE44
		[Parameter]
		public string Note { get; set; } = "This screen is not built yet.";
	}
}
