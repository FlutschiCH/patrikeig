using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components.Account.Shared
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	[Nullable(0)]
	public class RedirectToLogin : ComponentBase
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x0002D675 File Offset: 0x0002B875
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002D677 File Offset: 0x0002B877
		protected override void OnInitialized()
		{
			this.NavigationManager.NavigateTo("Account/Login?returnUrl=" + Uri.EscapeDataString(this.NavigationManager.Uri), true);
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0002D69F File Offset: 0x0002B89F
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0002D6A7 File Offset: 0x0002B8A7
		[Inject]
		private NavigationManager NavigationManager { get; set; }
	}
}
