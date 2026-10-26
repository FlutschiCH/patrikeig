using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	[Nullable(0)]
	public class N6Uplink : ComponentBase, IDisposable
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x0001E760 File Offset: 0x0001C960
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card max-w-2xl p-5");
			__builder.OpenElement(2, "div");
			__builder.AddAttribute(3, "class", "mb-1 flex flex-wrap items-baseline justify-between gap-3");
			__builder.AddMarkupContent(4, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Customer uplink (N6)</h2>");
			if (this._core != null)
			{
				__builder.OpenElement(5, "span");
				__builder.AddAttribute(6, "class", "text-xs text-cocoa-700");
				__builder.AddContent(7, this._core.Name);
				__builder.CloseElement();
			}
			__builder.CloseElement();
			if (this._core == null)
			{
				__builder.OpenElement(8, "p");
				__builder.AddAttribute(9, "class", "text-sm text-cocoa-700");
				__builder.AddContent(10, this._loadError ?? "Loading…");
				__builder.CloseElement();
			}
			else
			{
				if (this._busy)
				{
					__builder.OpenElement(11, "div");
					__builder.AddAttribute(12, "class", "card mb-4 flex items-center gap-3 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
					__builder.AddMarkupContent(13, "<span class=\"inline-block h-4 w-4 shrink-0 animate-spin rounded-full border-2 border-blush-200 border-t-blush-600\"></span>\n                ");
					__builder.OpenElement(14, "span");
					__builder.AddContent(15, this._status);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				if (this._errors.Count > 0)
				{
					__builder.OpenElement(16, "div");
					__builder.AddAttribute(17, "class", "card mb-4 border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
					foreach (string text in this._errors)
					{
						__builder.OpenElement(18, "div");
						__builder.AddContent(19, text);
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
				if (this._message != null)
				{
					__builder.OpenElement(20, "div");
					__builder.AddAttribute(21, "class", "card mb-4 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
					__builder.AddContent(22, this._message);
					__builder.CloseElement();
				}
				if (this._managementWarning != null)
				{
					__builder.OpenElement(23, "div");
					__builder.AddAttribute(24, "class", "card mb-4 border-l-4 border-amber-500 bg-amber-50 p-3 text-sm text-amber-800");
					__builder.AddMarkupContent(25, "<b>Not safe to change from here yet.</b> ");
					__builder.AddContent(26, this._managementWarning);
					__builder.CloseElement();
				}
				N6State state = this._state;
				if (state != null)
				{
					__builder.OpenElement(27, "dl");
					__builder.AddAttribute(28, "class", "mb-4 grid gap-x-6 gap-y-1 text-sm sm:grid-cols-2");
					__builder.AddMarkupContent(29, "<dt class=\"text-cocoa-700\">Port</dt>");
					__builder.OpenElement(30, "dd");
					__builder.AddAttribute(31, "class", "font-medium text-cocoa-900");
					__builder.AddContent(32, state.Interface.Name);
					__builder.CloseElement();
					__builder.AddMarkupContent(33, "\n                ");
					__builder.AddMarkupContent(34, "<dt class=\"text-cocoa-700\">Base station side (primary)</dt>");
					__builder.OpenElement(35, "dd");
					__builder.AddAttribute(36, "class", "font-medium text-cocoa-900");
					int num = 37;
					IpWithMask primary = state.Interface.Primary;
					__builder.AddContent(num, ((primary != null) ? primary.ToString() : null) ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(38, "\n                ");
					__builder.AddMarkupContent(39, "<dt class=\"text-cocoa-700\">Customer side (secondary)</dt>\n                ");
					__builder.OpenElement(40, "dd");
					__builder.AddAttribute(41, "class", "font-medium text-cocoa-900");
					if (state.Interface.Secondaries.Count == 0)
					{
						__builder.AddContent(42, "none");
					}
					else
					{
						__builder.AddContent(43, string.Join<IpWithMask>(", ", state.Interface.Secondaries));
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(44, "\n                ");
					__builder.AddMarkupContent(45, "<dt class=\"text-cocoa-700\">Default gateway</dt>\n                ");
					__builder.OpenElement(46, "dd");
					__builder.AddAttribute(47, "class", "font-medium text-cocoa-900");
					__builder.AddContent(48, (state.DefaultGateways.Count == 0) ? "none" : string.Join(", ", state.DefaultGateways));
					__builder.CloseElement();
					__builder.AddMarkupContent(49, "\n                ");
					__builder.AddMarkupContent(50, "<dt class=\"text-cocoa-700\">NAT source</dt>\n                ");
					__builder.OpenElement(51, "dd");
					__builder.AddAttribute(52, "class", "font-medium text-cocoa-900");
					__builder.AddContent(53, (state.NatSourceAddresses.Count == 0) ? "no NAT on this port" : string.Join(", ", state.NatSourceAddresses));
					__builder.CloseElement();
					__builder.CloseElement();
					if (state.Interface.Secondaries.Count > 1)
					{
						__builder.OpenElement(54, "p");
						__builder.AddAttribute(55, "class", "mb-4 text-xs text-amber-800");
						__builder.AddMarkupContent(56, "\n                    The port carries ");
						__builder.AddContent(57, state.Interface.Secondaries.Count);
						__builder.AddMarkupContent(58, " secondary addresses — leftovers from earlier sites. A write removes all of them.\n                ");
						__builder.CloseElement();
					}
					if (state.DefaultGateways.Count > 1)
					{
						__builder.OpenElement(59, "p");
						__builder.AddAttribute(60, "class", "mb-4 text-xs text-amber-800");
						__builder.AddMarkupContent(61, "\n                    The core has ");
						__builder.AddContent(62, state.DefaultGateways.Count);
						__builder.AddMarkupContent(63, " default routes; traffic is being shared between them. A write leaves only the new one.\n                ");
						__builder.CloseElement();
					}
				}
				else if (!this._busy)
				{
					__builder.AddMarkupContent(64, "<p class=\"mb-4 text-sm text-cocoa-700\">Nothing read from the core yet.</p>");
				}
				__builder.OpenElement(65, "fieldset");
				__builder.AddAttribute(66, "disabled", this._busy || this._state == null);
				__builder.AddAttribute(67, "class", "space-y-5");
				__builder.OpenElement(68, "div");
				__builder.AddAttribute(69, "class", "grid gap-4 sm:grid-cols-3");
				__builder.OpenElement(70, "label");
				__builder.AddAttribute(71, "class", "block");
				__builder.AddMarkupContent(72, "<span class=\"text-sm font-medium\">New address</span>\n                    ");
				__builder.OpenElement(73, "input");
				__builder.AddAttribute(74, "type", "text");
				__builder.AddAttribute(75, "class", "input");
				__builder.AddAttribute(76, "placeholder", "192.0.2.30");
				__builder.AddAttribute(77, "value", BindConverter.FormatValue(this._address, null));
				__builder.AddAttribute<ChangeEventArgs>(78, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._address = __value;
				}, this._address, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(79, "\n                ");
				__builder.OpenElement(80, "label");
				__builder.AddAttribute(81, "class", "block");
				__builder.AddMarkupContent(82, "<span class=\"text-sm font-medium\">Mask</span>\n                    ");
				__builder.OpenElement(83, "input");
				__builder.AddAttribute(84, "type", "text");
				__builder.AddAttribute(85, "class", "input");
				__builder.AddAttribute(86, "placeholder", "255.255.255.0 or 24");
				__builder.AddAttribute(87, "value", BindConverter.FormatValue(this._mask, null));
				__builder.AddAttribute<ChangeEventArgs>(88, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._mask = __value;
				}, this._mask, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(89, "\n                ");
				__builder.OpenElement(90, "label");
				__builder.AddAttribute(91, "class", "block");
				__builder.AddMarkupContent(92, "<span class=\"text-sm font-medium\">Gateway</span>\n                    ");
				__builder.OpenElement(93, "input");
				__builder.AddAttribute(94, "type", "text");
				__builder.AddAttribute(95, "class", "input");
				__builder.AddAttribute(96, "placeholder", "192.0.2.1");
				__builder.AddAttribute(97, "value", BindConverter.FormatValue(this._gateway, null));
				__builder.AddAttribute<ChangeEventArgs>(98, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._gateway = __value;
				}, this._gateway, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(99, "\n            ");
				__builder.AddMarkupContent(100, "<p class=\"text-xs text-cocoa-700\">\n                The address the customer assigns to the core in their network, and their gateway. Static only.\n                The old secondary and the old default route are removed, and the NAT group is set to the new\n                address, all in the same operation.\n            </p>");
				__builder.CloseElement();
				__builder.OpenElement(101, "div");
				__builder.AddAttribute(102, "class", "mt-5 flex flex-wrap items-center gap-3");
				if (!this._confirm)
				{
					__builder.OpenElement(103, "button");
					__builder.AddAttribute(104, "type", "button");
					__builder.AddAttribute(105, "class", "btn-primary !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(106, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
					{
						this.ClearResult();
						this._confirm = true;
					}));
					__builder.AddAttribute(107, "disabled", this._busy || this._state == null || this._managementWarning != null || !this.HasInput);
					__builder.AddMarkupContent(108, "\n                    Write to core\n                ");
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(109, "span");
					__builder.AddAttribute(110, "class", "text-sm text-cocoa-900");
					__builder.AddMarkupContent(111, "\n                    This moves the core's customer address to ");
					__builder.OpenElement(112, "b");
					int num2 = 113;
					string address = this._address;
					__builder.AddContent(num2, (address != null) ? address.Trim() : null);
					__builder.CloseElement();
					__builder.AddContent(114, " via ");
					__builder.OpenElement(115, "b");
					int num3 = 116;
					string gateway = this._gateway;
					__builder.AddContent(num3, (gateway != null) ? gateway.Trim() : null);
					__builder.CloseElement();
					__builder.AddMarkupContent(117, ". Handsets lose the outside world for a few seconds. Continue?\n                ");
					__builder.CloseElement();
					__builder.AddMarkupContent(118, "\n                ");
					__builder.OpenElement(119, "button");
					__builder.AddAttribute(120, "type", "button");
					__builder.AddAttribute(121, "class", "btn-danger !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(122, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.ApplyAsync)));
					__builder.AddAttribute(123, "disabled", this._busy);
					__builder.AddContent(124, "Yes, write");
					__builder.CloseElement();
					__builder.AddMarkupContent(125, "\n                ");
					__builder.OpenElement(126, "button");
					__builder.AddAttribute(127, "type", "button");
					__builder.AddAttribute(128, "class", "btn-secondary !px-4 !py-2 text-sm");
					__builder.AddAttribute<MouseEventArgs>(129, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
					{
						this._confirm = false;
					}));
					__builder.AddAttribute(130, "disabled", this._busy);
					__builder.AddContent(131, "Cancel");
					__builder.CloseElement();
				}
				__builder.OpenElement(132, "button");
				__builder.AddAttribute(133, "type", "button");
				__builder.AddAttribute(134, "class", "btn-secondary !px-4 !py-2 text-sm");
				__builder.AddAttribute<MouseEventArgs>(135, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.ReadAsync)));
				__builder.AddAttribute(136, "disabled", this._busy);
				__builder.AddMarkupContent(137, "\n                Read from core\n            ");
				__builder.CloseElement();
				if (this._steps.Count > 0)
				{
					__builder.OpenElement(138, "label");
					__builder.AddAttribute(139, "class", "ml-auto flex items-center gap-2 text-xs text-cocoa-700");
					__builder.OpenElement(140, "input");
					__builder.AddAttribute(141, "type", "checkbox");
					__builder.AddAttribute(142, "checked", BindConverter.FormatValue(this._showCommands, null));
					__builder.AddAttribute<ChangeEventArgs>(143, "onchange", EventCallback.Factory.CreateBinder(this, delegate(bool __value)
					{
						this._showCommands = __value;
					}, this._showCommands, null));
					__builder.SetUpdatesAttributeName("checked");
					__builder.CloseElement();
					__builder.AddMarkupContent(144, " Show commands\n                ");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				if (this._steps.Count > 0)
				{
					__builder.OpenElement(145, "ul");
					__builder.AddAttribute(146, "class", "mt-4 space-y-1 text-xs");
					foreach (CoreStep coreStep in this._steps)
					{
						__builder.OpenElement(147, "li");
						__builder.AddAttribute(148, "class", "flex flex-wrap items-start gap-2");
						__builder.OpenElement(149, "span");
						__builder.AddAttribute(150, "class", coreStep.Ok ? "pill pill-up" : "pill pill-down");
						__builder.AddContent(151, coreStep.Ok ? "ok" : "failed");
						__builder.CloseElement();
						__builder.AddMarkupContent(152, "\n                        ");
						__builder.OpenElement(153, "span");
						__builder.AddAttribute(154, "class", "font-medium text-cocoa-900");
						__builder.AddContent(155, coreStep.Label);
						__builder.CloseElement();
						if (this._showCommands)
						{
							__builder.OpenElement(156, "code");
							__builder.AddAttribute(157, "class", "min-w-0 break-all text-cocoa-700");
							__builder.AddContent(158, coreStep.Command);
							__builder.CloseElement();
						}
						if (!coreStep.Ok)
						{
							__builder.OpenElement(159, "span");
							__builder.AddAttribute(160, "class", "text-red-700");
							__builder.AddContent(161, coreStep.Reply);
							__builder.CloseElement();
						}
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
			}
			__builder.CloseElement();
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		[Parameter]
		public int CoreId { get; set; }

		// Token: 0x060005FD RID: 1533 RVA: 0x0001F5B9 File Offset: 0x0001D7B9
		public void Dispose()
		{
			this._cts.Cancel();
			this._cts.Dispose();
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001F5D1 File Offset: 0x0001D7D1
		private bool HasInput
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this._address) && !string.IsNullOrWhiteSpace(this._mask) && !string.IsNullOrWhiteSpace(this._gateway);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001F600 File Offset: 0x0001D800
		protected override async Task OnInitializedAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				this._core = await queryable.FirstOrDefaultAsync<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(N6Uplink)), methodof(N6Uplink.get_CoreId()))), new ParameterExpression[] { parameterExpression }), default(CancellationToken));
				if (this._core == null)
				{
					this._loadError = "Core not found.";
				}
				else
				{
					await this.ReadAsync();
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			if (db != null)
			{
				await db.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return;
			}
			obj = null;
			db = null;
			throw null;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001F643 File Offset: 0x0001D843
		private void ClearResult()
		{
			this._errors = new List<string>();
			this._steps = new List<CoreStep>();
			this._message = null;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001F664 File Offset: 0x0001D864
		private async Task ReadAsync()
		{
			if (this._core != null)
			{
				this._busy = true;
				this._status = "Reading the core's configuration…";
				this._confirm = false;
				this.ClearResult();
				try
				{
					CoreN6Outcome coreN6Outcome;
					try
					{
						coreN6Outcome = await this.Writer.ReadAsync(this._core, this._cts.Token);
					}
					catch (OperationCanceledException)
					{
						return;
					}
					if (!coreN6Outcome.Ok || coreN6Outcome.State == null)
					{
						this._errors = new List<string> { coreN6Outcome.Error ?? "Read failed." };
					}
					else
					{
						this._state = coreN6Outcome.State;
						if (this._mask == null)
						{
							IpWithMask currentN = this._state.CurrentN6;
							this._mask = ((currentN != null) ? currentN.Mask : null) ?? "255.255.255.0";
						}
						string text;
						this._managementWarning = (CoreN6Config.ManagementAtRisk(this._state, CoreN6Writer.LocalAddressToward(this._core.IpAddress), out text) ? text : null);
					}
				}
				finally
				{
					this._busy = false;
					this._status = "";
				}
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001F6A8 File Offset: 0x0001D8A8
		private async Task ApplyAsync()
		{
			if (this._core != null && this._state != null)
			{
				this._confirm = false;
				this._errors = CoreN6Config.Validate(this._state, this._address, this._mask, this._gateway);
				if (this._errors.Count <= 0)
				{
					this._busy = true;
					this._status = "Writing to the core…";
					this._steps = new List<CoreStep>();
					this._message = null;
					try
					{
						CoreN6Outcome coreN6Outcome = await this.Writer.ApplyAsync(this._core, this._address.Trim(), this._mask.Trim(), this._gateway.Trim(), CancellationToken.None);
						this._steps = coreN6Outcome.Steps;
						if (coreN6Outcome.State != null)
						{
							this._state = coreN6Outcome.State;
						}
						if (!coreN6Outcome.Ok)
						{
							this._errors = new List<string> { coreN6Outcome.Error ?? "The write failed." };
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
							defaultInterpolatedStringHandler.AppendFormatted<int>(coreN6Outcome.Steps.Count);
							defaultInterpolatedStringHandler.AppendLiteral(" commands accepted and saved by ");
							defaultInterpolatedStringHandler.AppendFormatted(this._core.Name);
							defaultInterpolatedStringHandler.AppendLiteral(". The values above are what it reports now.");
							this._message = defaultInterpolatedStringHandler.ToStringAndClear();
							this._address = null;
							this._gateway = null;
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0001F6EB File Offset: 0x0001D8EB
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x0001F6F3 File Offset: 0x0001D8F3
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0001F6FC File Offset: 0x0001D8FC
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x0001F704 File Offset: 0x0001D904
		[Inject]
		private CoreN6Writer Writer { get; set; }

		// Token: 0x04000272 RID: 626
		[Nullable(2)]
		private Device _core;

		// Token: 0x04000273 RID: 627
		[Nullable(2)]
		private string _loadError;

		// Token: 0x04000274 RID: 628
		[Nullable(2)]
		private N6State _state;

		// Token: 0x04000275 RID: 629
		[Nullable(2)]
		private string _address;

		// Token: 0x04000276 RID: 630
		[Nullable(2)]
		private string _mask;

		// Token: 0x04000277 RID: 631
		[Nullable(2)]
		private string _gateway;

		// Token: 0x04000278 RID: 632
		[Nullable(2)]
		private string _managementWarning;

		// Token: 0x04000279 RID: 633
		private bool _busy;

		// Token: 0x0400027A RID: 634
		private bool _confirm;

		// Token: 0x0400027B RID: 635
		private bool _showCommands = true;

		// Token: 0x0400027C RID: 636
		private string _status = "";

		// Token: 0x0400027D RID: 637
		private List<string> _errors = new List<string>();

		// Token: 0x0400027E RID: 638
		[Nullable(2)]
		private string _message;

		// Token: 0x0400027F RID: 639
		private List<CoreStep> _steps = new List<CoreStep>();

		// Token: 0x04000280 RID: 640
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
	}
}
