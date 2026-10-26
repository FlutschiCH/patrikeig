using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace __Blazor.AbsolutDashboard.Web.Components.Account.Pages.LoginWithRecoveryCode
{
	// Token: 0x0200001E RID: 30
	internal static class TypeInference
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000578C File Offset: 0x0000398C
		public static void CreateValidationMessage_0<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}
	}
}
