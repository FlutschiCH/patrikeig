using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace __Blazor.AbsolutDashboard.Web.Components.Account.Pages.LoginWith2fa
{
	// Token: 0x0200001F RID: 31
	internal static class TypeInference
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x000057B7 File Offset: 0x000039B7
		public static void CreateValidationMessage_0<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}
	}
}
