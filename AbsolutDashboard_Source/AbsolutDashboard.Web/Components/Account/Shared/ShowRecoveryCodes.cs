using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components.Account.Shared
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	[Nullable(0)]
	public class ShowRecoveryCodes : ComponentBase
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<StatusMessage>(0);
			__builder.AddComponentParameter(1, "Message", RuntimeHelpers.TypeCheck<string>(this.StatusMessage));
			__builder.CloseComponent();
			__builder.AddMarkupContent(2, "\n");
			__builder.AddMarkupContent(3, "<h3>Recovery codes</h3>\n");
			__builder.AddMarkupContent(4, "<div class=\"alert alert-warning\" role=\"alert\"><p><strong>Put these codes in a safe place.</strong></p>\n    <p>\n        If you lose your device and don't have the recovery codes you will lose access to your account.\n    </p></div>\n");
			__builder.OpenElement(5, "div");
			__builder.AddAttribute(6, "class", "row");
			__builder.OpenElement(7, "div");
			__builder.AddAttribute(8, "class", "col-md-12");
			foreach (string text in this.RecoveryCodes)
			{
				__builder.OpenElement(9, "div");
				__builder.OpenElement(10, "code");
				__builder.AddAttribute(11, "class", "recovery-code");
				__builder.AddContent(12, text);
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0002D7AD File Offset: 0x0002B9AD
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0002D7B5 File Offset: 0x0002B9B5
		[Parameter]
		public string[] RecoveryCodes { get; set; } = Array.Empty<string>();

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0002D7BE File Offset: 0x0002B9BE
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0002D7C6 File Offset: 0x0002B9C6
		[Nullable(2)]
		[Parameter]
		public string StatusMessage
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}
	}
}
