using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Services.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	[Nullable(0)]
	public class ShutdownAll : ComponentBase
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0001F858 File Offset: 0x0001DA58
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card mb-6 p-5");
			__builder.AddMarkupContent(2, "<div class=\"mb-1 flex flex-wrap items-baseline justify-between gap-3\"><h2 class=\"text-lg font-semibold text-cocoa-900\">System shutdown</h2>\n        <span class=\"text-xs text-cocoa-700\">Admin only · power-on is manual</span></div>");
			if (!this.Runner.ShutdownAll.Configured)
			{
				__builder.AddMarkupContent(3, "<p class=\"text-sm text-cocoa-700\">\n            The shutdown sequence is not configured on this site yet. The commands (base station, then\n            the Proxmox virtual machines, then the rest) are to be provided by Absolut and entered in the\n            site configuration under <code>Operations__ShutdownAll__Steps__…</code>.\n        </p>\n        ");
				__builder.AddMarkupContent(4, "<button type=\"button\" class=\"btn-danger mt-3 !px-4 !py-2 text-sm\" disabled>Shut down all systems</button>");
			}
			else
			{
				__builder.OpenElement(5, "ol");
				__builder.AddAttribute(6, "class", "mb-3 list-decimal space-y-0.5 pl-5 text-sm text-cocoa-700");
				foreach (OperationStep operationStep in this.Runner.ShutdownAll.Steps)
				{
					__builder.OpenElement(7, "li");
					__builder.AddContent(8, operationStep.Label);
					__builder.AddContent(9, " ");
					__builder.OpenElement(10, "span");
					__builder.AddAttribute(11, "class", "text-xs");
					__builder.AddContent(12, "(");
					__builder.AddContent(13, operationStep.IsMml ? (string.IsNullOrWhiteSpace(operationStep.Device) ? "every base station" : operationStep.Device) : (operationStep.Device ?? operationStep.Host));
					__builder.AddContent(14, ")");
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				if (this._busy)
				{
					__builder.OpenElement(15, "div");
					__builder.AddAttribute(16, "class", "card mb-3 flex items-center gap-3 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
					__builder.AddMarkupContent(17, "<span class=\"inline-block h-4 w-4 shrink-0 animate-spin rounded-full border-2 border-blush-200 border-t-blush-600\"></span>\n                ");
					__builder.OpenElement(18, "span");
					__builder.AddContent(19, this._status);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				else if (this._confirm == 0)
				{
					__builder.OpenElement(20, "button");
					__builder.AddAttribute(21, "type", "button");
					__builder.AddAttribute(22, "class", "btn-danger !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(23, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
					{
						this._confirm = 1;
					}));
					__builder.AddContent(24, "Shut down all systems");
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(25, "div");
					__builder.AddAttribute(26, "class", "card border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
					__builder.AddMarkupContent(27, "<b>This shuts down the base station, the cores and everything else in the list.</b>\n                Every handset loses service, and power-on afterwards is manual. Type ");
					__builder.AddMarkupContent(28, "<code>SHUTDOWN</code> to confirm.\n                ");
					__builder.OpenElement(29, "div");
					__builder.AddAttribute(30, "class", "mt-2 flex flex-wrap items-center gap-2");
					__builder.OpenElement(31, "input");
					__builder.AddAttribute(32, "type", "text");
					__builder.AddAttribute(33, "class", "input mt-0 w-40");
					__builder.AddAttribute(34, "placeholder", "SHUTDOWN");
					__builder.AddAttribute(35, "value", BindConverter.FormatValue(this._typed, null));
					__builder.AddAttribute<ChangeEventArgs>(36, "oninput", EventCallback.Factory.CreateBinder(this, delegate(string __value)
					{
						this._typed = __value;
					}, this._typed, null));
					__builder.SetUpdatesAttributeName("value");
					__builder.CloseElement();
					__builder.AddMarkupContent(37, "\n                    ");
					__builder.OpenElement(38, "button");
					__builder.AddAttribute(39, "type", "button");
					__builder.AddAttribute(40, "class", "btn-danger !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(41, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.RunAsync)));
					__builder.AddAttribute(42, "disabled", this._typed != "SHUTDOWN");
					__builder.AddContent(43, "Yes, shut everything down");
					__builder.CloseElement();
					__builder.AddMarkupContent(44, "\n                    ");
					__builder.OpenElement(45, "button");
					__builder.AddAttribute(46, "type", "button");
					__builder.AddAttribute(47, "class", "btn-secondary !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(48, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Action(this.Cancel)));
					__builder.AddContent(49, "Cancel");
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.CloseElement();
				}
				if (this._results.Count > 0)
				{
					__builder.OpenElement(50, "ul");
					__builder.AddAttribute(51, "class", "mt-4 space-y-1 text-xs");
					foreach (OperationResult operationResult in this._results)
					{
						__builder.OpenElement(52, "li");
						__builder.AddAttribute(53, "class", "flex flex-wrap items-start gap-2");
						__builder.OpenElement(54, "span");
						__builder.AddAttribute(55, "class", operationResult.Ok ? "pill pill-up" : "pill pill-down");
						__builder.AddContent(56, operationResult.Ok ? "ok" : "failed");
						__builder.CloseElement();
						__builder.AddMarkupContent(57, "\n                        ");
						__builder.OpenElement(58, "span");
						__builder.AddAttribute(59, "class", "font-medium text-cocoa-900");
						__builder.AddContent(60, operationResult.Label);
						__builder.CloseElement();
						__builder.AddMarkupContent(61, "\n                        ");
						__builder.OpenElement(62, "code");
						__builder.AddAttribute(63, "class", "min-w-0 break-all text-cocoa-700");
						__builder.AddContent(64, operationResult.Command);
						__builder.CloseElement();
						__builder.AddMarkupContent(65, "\n                        ");
						__builder.OpenElement(66, "span");
						__builder.AddAttribute(67, "class", operationResult.Ok ? "text-cocoa-700" : "text-red-700");
						__builder.AddContent(68, operationResult.Detail);
						__builder.CloseElement();
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
			}
			__builder.CloseElement();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001FE88 File Offset: 0x0001E088
		private void Cancel()
		{
			this._confirm = 0;
			this._typed = "";
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001FE9C File Offset: 0x0001E09C
		private async Task RunAsync()
		{
			this._confirm = 0;
			this._typed = "";
			this._busy = true;
			this._status = "Shutting down, step by step…";
			this._results = new List<OperationResult>();
			try
			{
				List<OperationResult> list = await this.Runner.RunShutdownAllAsync(CancellationToken.None);
				this._results = list;
			}
			finally
			{
				this._busy = false;
				this._status = "";
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001FEDF File Offset: 0x0001E0DF
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x0001FEE7 File Offset: 0x0001E0E7
		[Inject]
		private OperationRunner Runner { get; set; }

		// Token: 0x04000284 RID: 644
		private bool _busy;

		// Token: 0x04000285 RID: 645
		private int _confirm;

		// Token: 0x04000286 RID: 646
		private string _typed = "";

		// Token: 0x04000287 RID: 647
		private string _status = "";

		// Token: 0x04000288 RID: 648
		private List<OperationResult> _results = new List<OperationResult>();
	}
}
