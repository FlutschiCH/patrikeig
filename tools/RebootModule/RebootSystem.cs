using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Services.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AbsolutDashboard.Web.Components
{
    public class RebootSystem : ComponentBase
    {
        [Inject]
        private OperationRunner Runner { get; set; } = null!;

        private readonly HashSet<int> _selected = new HashSet<int>();
        private int _confirm;
        private string _typed = "";
        private bool _busy;
        private string _status = "";
        private List<OperationResult> _results = new List<OperationResult>();

        public static List<OperationStep> GetRebootSteps(OperationRunner runner)
        {
            var optsProp = typeof(OperationRunner).GetProperty("Reboot");
            if (optsProp != null)
            {
                var opts = optsProp.GetValue(runner) as RebootOptions;
                if (opts != null) return opts.Steps;
            }
            return new List<OperationStep>();
        }

        public static bool IsConfigured(OperationRunner runner)
        {
            var steps = GetRebootSteps(runner);
            return steps.Count > 0 && steps.All(s => s.IsValid);
        }

        protected override void BuildRenderTree(RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "card mb-6 p-5");
            __builder.AddMarkupContent(2, "<div class=\"mb-1 flex flex-wrap items-baseline justify-between gap-3\"><h2 class=\"text-lg font-semibold text-cocoa-900\">System reboot</h2></div>");
            var steps = GetRebootSteps(this.Runner);
            if (steps.Count == 0 || !steps.All(s => s.IsValid))
            {
                __builder.AddMarkupContent(3, "<p class=\"text-sm text-cocoa-700\">\n            The reboot sequence is not configured on this site yet. Enter the commands in the\n            site configuration under <code>Operations__Reboot__Steps__…</code>.\n        </p>\n        ");
                __builder.AddMarkupContent(4, "<button type=\"button\" class=\"btn-danger mt-3 !px-4 !py-2 text-sm\" disabled>Reboot</button>");
            }
            else
            {
                __builder.OpenElement(5, "div");
                __builder.AddAttribute(6, "class", "mb-3 space-y-1.5 text-sm text-cocoa-700");
                for (int i = 0; i < steps.Count; i++)
                {
                    int index = i;
                    OperationStep operationStep = steps[i];
                    __builder.OpenElement(7, "label");
                    __builder.AddAttribute(8, "class", "flex cursor-pointer items-center gap-2");
                    __builder.OpenElement(9, "input");
                    __builder.AddAttribute(10, "type", "checkbox");
                    __builder.AddAttribute(11, "class", "h-4 w-4 rounded border-cocoa-300");
                    __builder.AddAttribute(12, "checked", this._selected.Contains(index));
                    __builder.AddAttribute(13, "disabled", this._busy);
                    __builder.AddAttribute(14, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, delegate (ChangeEventArgs e)
                    {
                        bool isChecked = e.Value is bool b && b;
                        if (isChecked) this._selected.Add(index);
                        else this._selected.Remove(index);
                    }));
                    __builder.CloseElement();
                    __builder.OpenElement(15, "span");
                    __builder.AddContent(16, operationStep.Label);
                    __builder.AddContent(17, " ");
                    __builder.OpenElement(18, "span");
                    __builder.AddAttribute(19, "class", "text-xs");
                    __builder.AddContent(20, "(");
                    __builder.AddContent(21, operationStep.IsMml ? (string.IsNullOrWhiteSpace(operationStep.Device) ? "every base station" : operationStep.Device) : (operationStep.Device ?? operationStep.Host));
                    __builder.AddContent(22, ")");
                    __builder.CloseElement();
                    __builder.CloseElement();
                    __builder.CloseElement();
                }
                __builder.CloseElement();
                if (this._busy)
                {
                    __builder.OpenElement(23, "div");
                    __builder.AddAttribute(24, "class", "card mb-3 flex items-center gap-3 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
                    __builder.AddMarkupContent(25, "<span class=\"inline-block h-4 w-4 shrink-0 animate-spin rounded-full border-2 border-blush-200 border-t-blush-600\"></span>\n                ");
                    __builder.OpenElement(26, "span");
                    __builder.AddContent(27, this._status);
                    __builder.CloseElement();
                    __builder.CloseElement();
                }
                else if (this._confirm == 0)
                {
                    __builder.OpenElement(28, "button");
                    __builder.AddAttribute(29, "type", "button");
                    __builder.AddAttribute(30, "class", "btn-danger !px-4 !py-2 text-sm");
                    __builder.AddAttribute(31, "disabled", this._selected.Count == 0);
                    __builder.AddAttribute(32, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Action(delegate
                    {
                        this._confirm = 1;
                    })));
                    __builder.AddContent(33, "Reboot");
                    __builder.CloseElement();
                }
                else
                {
                    __builder.OpenElement(34, "div");
                    __builder.AddAttribute(35, "class", "card border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
                    __builder.AddMarkupContent(36, "<b>This reboots the selected systems.</b>\n                Service will be interrupted temporarily. Type <code>REBOOT</code> to confirm.\n                ");
                    __builder.OpenElement(37, "div");
                    __builder.AddAttribute(38, "class", "mt-2 flex flex-wrap items-center gap-2");
                    __builder.OpenElement(39, "input");
                    __builder.AddAttribute(40, "type", "text");
                    __builder.AddAttribute(41, "class", "input mt-0 w-40");
                    __builder.AddAttribute(42, "placeholder", "REBOOT");
                    __builder.AddAttribute(43, "value", BindConverter.FormatValue(this._typed, null));
                    __builder.AddAttribute(44, "oninput", EventCallback.Factory.CreateBinder(this, delegate (string? __value)
                    {
                        this._typed = __value ?? "";
                    }, this._typed, null));
                    __builder.SetUpdatesAttributeName("value");
                    __builder.CloseElement();
                    __builder.AddMarkupContent(45, "\n                    ");
                    __builder.OpenElement(46, "button");
                    __builder.AddAttribute(47, "type", "button");
                    __builder.AddAttribute(48, "class", "btn-danger !px-4 !py-2 text-sm");
                    __builder.AddAttribute(49, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, RunAsync));
                    __builder.AddAttribute(50, "disabled", this._typed != "REBOOT");
                    __builder.AddContent(51, "Yes, reboot selected");
                    __builder.CloseElement();
                    __builder.AddMarkupContent(52, "\n                    ");
                    __builder.OpenElement(53, "button");
                    __builder.AddAttribute(54, "type", "button");
                    __builder.AddAttribute(55, "class", "btn-secondary !px-4 !py-2 text-sm");
                    __builder.AddAttribute(56, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, Cancel));
                    __builder.AddContent(57, "Cancel");
                    __builder.CloseElement();
                    __builder.CloseElement();
                    __builder.CloseElement();
                }
                if (this._results.Count > 0)
                {
                    __builder.OpenElement(58, "ul");
                    __builder.AddAttribute(59, "class", "mt-4 space-y-1 text-xs");
                    foreach (OperationResult operationResult in this._results)
                    {
                        __builder.OpenElement(60, "li");
                        __builder.AddAttribute(61, "class", "flex flex-wrap items-start gap-2");
                        __builder.OpenElement(62, "span");
                        __builder.AddAttribute(63, "class", operationResult.Ok ? "pill pill-up" : "pill pill-down");
                        __builder.AddContent(64, operationResult.Ok ? "ok" : "failed");
                        __builder.CloseElement();
                        __builder.AddMarkupContent(65, "\n                        ");
                        __builder.OpenElement(66, "span");
                        __builder.AddAttribute(67, "class", "font-medium text-cocoa-900");
                        __builder.AddContent(68, operationResult.Label);
                        __builder.CloseElement();
                        __builder.OpenElement(74, "span");
                        __builder.AddAttribute(75, "class", "text-cocoa-700");
                        __builder.AddContent(76, operationResult.Detail);
                        __builder.CloseElement();
                        __builder.CloseElement();
                    }
                    __builder.CloseElement();
                }
            }
            __builder.CloseElement();
        }

        private void Cancel()
        {
            this._confirm = 0;
            this._typed = "";
        }

        private async Task RunAsync()
        {
            this._busy = true;
            this._confirm = 0;
            this._typed = "";
            this._status = "Running reboot sequence…";
            this._results.Clear();
            await base.InvokeAsync(base.StateHasChanged);
            try
            {
                var method = typeof(OperationRunner).GetMethod("RunRebootAsync");
                if (method != null)
                {
                    var task = (Task<List<OperationResult>>?)method.Invoke(this.Runner, new object[] { this._selected, CancellationToken.None });
                    if (task != null)
                    {
                        this._results = await task;
                    }
                }
            }
            finally
            {
                this._busy = false;
                this._status = "";
            }
        }
    }
}
