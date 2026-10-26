using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Http;

namespace AbsolutDashboard.Web.Components.Account.Shared
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(2)]
	[Nullable(0)]
	public class StatusMessage : ComponentBase
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x0002D7E4 File Offset: 0x0002B9E4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			if (!string.IsNullOrEmpty(this.DisplayMessage))
			{
				bool flag = this.DisplayMessage.StartsWith("Error");
				__builder.OpenElement(0, "div");
				__builder.AddAttribute(1, "role", "alert");
				__builder.AddAttribute(2, "class", "mb-4 rounded-xl px-4 py-3 text-sm ring-1 " + (flag ? "bg-red-50 text-red-700 ring-red-200" : "bg-green-50 text-green-700 ring-green-200"));
				__builder.AddContent(3, this.DisplayMessage);
				__builder.CloseElement();
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002D864 File Offset: 0x0002BA64
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0002D86C File Offset: 0x0002BA6C
		[Parameter]
		public string Message { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002D875 File Offset: 0x0002BA75
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0002D87D File Offset: 0x0002BA7D
		[Nullable(1)]
		[CascadingParameter]
		private HttpContext HttpContext
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0002D886 File Offset: 0x0002BA86
		private string DisplayMessage
		{
			get
			{
				return this.Message ?? this.messageFromCookie;
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002D898 File Offset: 0x0002BA98
		protected override void OnInitialized()
		{
			this.messageFromCookie = this.HttpContext.Request.Cookies["Identity.StatusMessage"];
			if (this.messageFromCookie != null)
			{
				this.HttpContext.Response.Cookies.Delete("Identity.StatusMessage");
			}
		}

		// Token: 0x04000349 RID: 841
		private string messageFromCookie;
	}
}
