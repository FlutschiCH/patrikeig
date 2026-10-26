using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace __Blazor.AbsolutDashboard.Web.Components.Users.CreateUser
{
	// Token: 0x02000017 RID: 23
	internal static class TypeInference
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000051E0 File Offset: 0x000033E0
		public static void CreateInputSelect_0<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue>(RenderTreeBuilder __builder, int seq, int __seq0, object __arg0, int __seq1, TValue __arg1, int __seq2, EventCallback<TValue> __arg2, int __seq3, Expression<Func<TValue>> __arg3, int __seq4, RenderFragment __arg4)
		{
			__builder.OpenComponent<InputSelect<TValue>>(seq);
			__builder.AddComponentParameter(__seq0, "class", __arg0);
			__builder.AddComponentParameter(__seq1, "Value", __arg1);
			__builder.AddComponentParameter(__seq2, "ValueChanged", __arg2);
			__builder.AddComponentParameter(__seq3, "ValueExpression", __arg3);
			__builder.AddComponentParameter(__seq4, "ChildContent", __arg4);
			__builder.CloseComponent();
		}
	}
}
