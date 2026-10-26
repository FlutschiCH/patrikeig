using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Manage.ChangePassword
{
	// Token: 0x0200001D RID: 29
	internal static class TypeInference
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000570B File Offset: 0x0000390B
		public static void CreateValidationMessage_0<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005736 File Offset: 0x00003936
		public static void CreateValidationMessage_1<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005761 File Offset: 0x00003961
		public static void CreateValidationMessage_2<TValue>(RenderTreeBuilder __builder, int seq, int __seq0, Expression<Func<TValue>> __arg0, int __seq1, object __arg1)
		{
			__builder.OpenComponent<ValidationMessage<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "For", __arg0);
			__builder.AddComponentParameter(__seq1, "class", __arg1);
			__builder.CloseComponent();
		}
	}
}
