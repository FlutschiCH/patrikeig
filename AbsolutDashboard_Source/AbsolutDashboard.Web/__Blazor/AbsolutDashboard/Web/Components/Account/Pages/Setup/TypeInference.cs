using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Setup
{
	// Token: 0x02000019 RID: 25
	internal static class TypeInference
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x000055DE File Offset: 0x000037DE
		public static void CreateValidationMessage_0<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005609 File Offset: 0x00003809
		public static void CreateValidationMessage_1<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005634 File Offset: 0x00003834
		public static void CreateValidationMessage_2<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}
	}
}
