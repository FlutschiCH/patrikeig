using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using AbsolutDashboard.Web.Services.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Route("/devices/detail/{Id:int}")]
	[DeviceDetail.__PrivateComponentRenderModeAttribute]
	public class DeviceDetail : ComponentBase, IDisposable
	{
		// Token: 0x060006D4 RID: 1748 RVA: 0x000252B4 File Offset: 0x000234B4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				int num12 = 2;
				Device device2 = this._device;
				__builder2.AddContent(num12, ((device2 != null) ? device2.Name : null) ?? "Device");
				__builder2.AddMarkupContent(3, " — Absolut");
			}));
			__builder.CloseComponent();
			if (this._notFound)
			{
				__builder.AddMarkupContent(4, "<h1 class=\"text-2xl font-semibold text-cocoa-900\">Device not found</h1>\n    ");
				__builder.AddMarkupContent(5, "<p class=\"mt-4\"><a class=\"btn-back\" href>&larr; Dashboard</a></p>");
				return;
			}
			if (this._device == null)
			{
				__builder.AddMarkupContent(6, "<p class=\"text-cocoa-700\">Loading…</p>");
				return;
			}
			DeviceCategory category = this._device.Category;
			ValueTuple<string, string> valueTuple;
			if (!(((category != null) ? category.Key : null) == "ue"))
			{
				valueTuple = DeviceStatusView.Pill(this._device.LastReachable);
			}
			else
			{
				bool? lastReachable = this._device.LastReachable;
				DeviceSnapshot snapshot = this._snapshot;
				string text;
				if (snapshot == null)
				{
					text = null;
				}
				else
				{
					Metric metric = snapshot.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "State");
					text = ((metric != null) ? metric.Value : null);
				}
				valueTuple = DeviceStatusView.UeBadge(lastReachable, text);
			}
			ValueTuple<string, string> valueTuple2 = valueTuple;
			RenderFragment<IGrouping<string, Metric>> renderFragment = (IGrouping<string, Metric> group) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(7, "div");
				__builder2.AddAttribute(8, "class", "card mb-4 p-5");
				__builder2.OpenElement(9, "h2");
				__builder2.AddAttribute(10, "class", "mb-3 text-lg font-semibold text-cocoa-900");
				__builder2.AddContent(11, group.Key);
				__builder2.CloseElement();
				__builder2.AddMarkupContent(12, "\n        ");
				__builder2.OpenElement(13, "dl");
				__builder2.AddAttribute(14, "class", "grid gap-x-6 gap-y-2 text-sm sm:grid-cols-2");
				foreach (Metric metric2 in MetricHistory.InDisplayOrder(group.Where<Metric>((Metric m) => this._slices.Count <= 0 || !DeviceDetail.SliceLabels.Contains(m.Label))))
				{
					__builder2.OpenElement(15, "div");
					__builder2.AddAttribute(16, "class", "flex justify-between gap-4 border-b border-cream-100 py-1");
					__builder2.OpenElement(17, "dt");
					__builder2.AddAttribute(18, "class", "text-cocoa-700");
					__builder2.AddContent(19, metric2.Label);
					__builder2.CloseElement();
					__builder2.AddMarkupContent(20, "\n                    ");
					__builder2.OpenElement(21, "dd");
					__builder2.AddAttribute(22, "class", "text-right font-medium text-cocoa-900");
					string text8 = this.MetricHref(metric2);
					if (text8 != null)
					{
						__builder2.OpenElement(23, "a");
						__builder2.AddAttribute(24, "class", "text-blush-700 hover:underline");
						__builder2.AddAttribute(25, "href", text8);
						__builder2.AddAttribute(26, "title", "Open this base station");
						__builder2.AddContent(27, metric2.Value);
						__builder2.CloseElement();
					}
					else
					{
						__builder2.AddContent(28, metric2.Value);
						__builder2.AddContent(29, " ");
						__builder2.AddContent(30, metric2.Unit);
					}
					__builder2.CloseElement();
					__builder2.CloseElement();
				}
				__builder2.CloseElement();
				__builder2.CloseElement();
			};
			__builder.OpenElement(31, "div");
			__builder.AddAttribute(32, "class", "mb-6 flex flex-wrap items-start justify-between gap-3");
			__builder.OpenElement(33, "div");
			__builder.OpenElement(34, "a");
			__builder.AddAttribute(35, "class", "btn-back mb-2");
			__builder.AddAttribute(36, "href", "devices/" + this._device.Category.Key);
			__builder.AddMarkupContent(37, "← ");
			__builder.AddContent(38, this._device.Category.Name);
			__builder.CloseElement();
			__builder.AddMarkupContent(39, "\n            ");
			__builder.OpenElement(40, "h1");
			__builder.AddAttribute(41, "class", "flex items-center gap-3 text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(42, this._device.Name);
			__builder.AddContent(43, " ");
			__builder.OpenElement(44, "span");
			__builder.AddAttribute(45, "class", valueTuple2.Item1);
			__builder.AddAttribute(46, "title", this._device.LastError);
			__builder.AddContent(47, valueTuple2.Item2);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(48, "\n            ");
			__builder.OpenElement(49, "p");
			__builder.AddAttribute(50, "class", "mt-1 text-sm text-cocoa-700");
			int num = 51;
			DeviceTemplate template = this._device.Template;
			__builder.AddContent(num, (template != null) ? template.Name : null);
			__builder.AddMarkupContent(52, " · ");
			__builder.AddContent(53, DeviceStatusView.MethodLabel(this._device.CollectionMethod));
			if (this._device.CollectionMethod == CollectionMethod.ViaParent)
			{
				int? num2 = this._device.ParentDeviceId;
				if (num2 != null)
				{
					__builder.OpenElement(54, "span");
					__builder.AddMarkupContent(55, "· via ");
					__builder.OpenElement(56, "a");
					__builder.AddAttribute(57, "class", "text-blush-700 hover:underline");
					int num3 = 58;
					string text2 = "href";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
					defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
					defaultInterpolatedStringHandler.AppendFormatted<int?>(this._device.ParentDeviceId);
					__builder.AddAttribute(num3, text2, defaultInterpolatedStringHandler.ToStringAndClear());
					int num4 = 59;
					Device parentDevice = this._device.ParentDevice;
					__builder.AddContent(num4, ((parentDevice != null) ? parentDevice.Name : null) ?? "parent");
					__builder.CloseElement();
					__builder.CloseElement();
				}
				else
				{
					__builder.AddMarkupContent(60, "<span>· via —</span>");
				}
			}
			else if (this._device.IpAddress != null)
			{
				__builder.OpenElement(61, "span");
				__builder.AddMarkupContent(62, "· ");
				__builder.AddContent(63, this._device.IpAddress);
				int num5 = 64;
				string text3;
				if (this._device.Port <= 0)
				{
					text3 = "";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler2.AppendLiteral(":");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(this._device.Port);
					text3 = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				__builder.AddContent(num5, text3);
				__builder.CloseElement();
			}
			__builder.AddMarkupContent(65, "                · polled ");
			__builder.AddContent(66, DeviceStatusView.Ago(this._device.LastPolledAt));
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(67, "\n        ");
			__builder.OpenComponent<AuthorizeView>(68);
			__builder.AddComponentParameter(69, "Roles", "Admin");
			__builder.AddAttribute(70, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(71, "div");
				__builder2.AddAttribute(72, "class", "flex flex-wrap gap-2");
				if (this._device.CollectionMethod != CollectionMethod.ViaParent)
				{
					__builder2.OpenElement(73, "button");
					__builder2.AddAttribute(74, "class", "btn-secondary !px-4 !py-2 text-sm");
					__builder2.AddAttribute<MouseEventArgs>(75, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.PollNow)));
					__builder2.AddAttribute(76, "disabled", this._busy);
					__builder2.AddContent(77, "Poll now");
					__builder2.CloseElement();
					__builder2.AddMarkupContent(78, "\n                    ");
					__builder2.OpenElement(79, "button");
					__builder2.AddAttribute(80, "class", "btn-secondary !px-4 !py-2 text-sm");
					__builder2.AddAttribute<MouseEventArgs>(81, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.ToggleEnabled)));
					__builder2.AddAttribute(82, "disabled", this._busy);
					__builder2.AddContent(83, this._device.Enabled ? "Pause" : "Resume");
					__builder2.CloseElement();
				}
				__builder2.OpenElement(84, "a");
				__builder2.AddAttribute(85, "class", "btn-secondary !px-4 !py-2 text-sm");
				int num13 = 86;
				string text9 = "href";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler6.AppendLiteral("devices/detail/");
				defaultInterpolatedStringHandler6.AppendFormatted<int>(this._device.Id);
				defaultInterpolatedStringHandler6.AppendLiteral("/edit");
				__builder2.AddAttribute(num13, text9, defaultInterpolatedStringHandler6.ToStringAndClear());
				__builder2.AddContent(87, "Edit");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(88, "\n                ");
				__builder2.OpenElement(89, "button");
				__builder2.AddAttribute(90, "class", "btn-danger !px-4 !py-2 text-sm");
				__builder2.AddAttribute<MouseEventArgs>(91, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.Delete)));
				__builder2.AddAttribute(92, "disabled", this._busy);
				__builder2.AddContent(93, "Delete");
				__builder2.CloseElement();
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			if (this._message != null)
			{
				__builder.OpenElement(94, "div");
				__builder.AddAttribute(95, "class", "card mb-4 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
				__builder.AddContent(96, this._message);
				__builder.CloseElement();
			}
			bool? lastReachable2 = this._device.LastReachable;
			bool flag = false;
			if (((lastReachable2.GetValueOrDefault() == flag) & (lastReachable2 != null)) && this._device.LastError != null)
			{
				__builder.OpenElement(97, "div");
				__builder.AddAttribute(98, "class", "card mb-4 border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
				__builder.AddContent(99, this._device.LastError);
				__builder.CloseElement();
			}
			if (!string.IsNullOrWhiteSpace(this._device.Notes))
			{
				__builder.OpenElement(100, "div");
				__builder.AddAttribute(101, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(102, "<h2 class=\"mb-2 text-lg font-semibold text-cocoa-900\">Description</h2>\n            ");
				__builder.OpenElement(103, "p");
				__builder.AddAttribute(104, "class", "whitespace-pre-line text-sm text-cocoa-700");
				__builder.AddContent(105, this._device.Notes);
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._device.Category.Key == "ru" && this._isAdmin && this.Runner.PrruPower.Enabled)
			{
				__builder.OpenElement(106, "div");
				__builder.AddAttribute(107, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(108, "<h2 class=\"mb-2 text-lg font-semibold text-cocoa-900\">Power</h2>");
				if (this._powerTarget == null || this._powerTarget.Blocker != null)
				{
					__builder.OpenElement(109, "p");
					__builder.AddAttribute(110, "class", "text-sm text-cocoa-700");
					int num6 = 111;
					OperationRunner.PrruPowerTarget powerTarget = this._powerTarget;
					__builder.AddContent(num6, ((powerTarget != null) ? powerTarget.Blocker : null) ?? "Looking up the power board…");
					__builder.CloseElement();
					__builder.AddMarkupContent(112, "\n                ");
					__builder.AddMarkupContent(113, "<div class=\"mt-3 flex gap-2\"><button type=\"button\" class=\"btn-danger !px-4 !py-2 text-sm\" disabled>Power off</button>\n                    <button type=\"button\" class=\"btn-secondary !px-4 !py-2 text-sm\" disabled>Power on</button></div>");
				}
				else
				{
					__builder.OpenElement(114, "p");
					__builder.AddAttribute(115, "class", "text-sm text-cocoa-700");
					__builder.AddMarkupContent(116, "\n                    Switches this PICO sender's supply on the power board of\n                    ");
					__builder.OpenElement(117, "a");
					__builder.AddAttribute(118, "class", "font-medium text-cocoa-900 underline");
					__builder.AddAttribute(119, "href", "/devices/" + this._powerTarget.ExtensionUnit.Id.ToString());
					__builder.AddContent(120, this._powerTarget.ExtensionUnit.Name);
					__builder.CloseElement();
					__builder.AddMarkupContent(121, ",\n                    channel ");
					__builder.OpenElement(122, "b");
					__builder.AddContent(123, this._powerChannel);
					__builder.CloseElement();
					int num7 = 124;
					int? num2 = this._powerTarget.ChannelCount;
					string text4;
					if (num2 != null)
					{
						int valueOrDefault = num2.GetValueOrDefault();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(4, 1);
						defaultInterpolatedStringHandler3.AppendLiteral(" of ");
						defaultInterpolatedStringHandler3.AppendFormatted<int>(valueOrDefault);
						text4 = defaultInterpolatedStringHandler3.ToStringAndClear();
					}
					else
					{
						text4 = "";
					}
					__builder.AddContent(num7, text4);
					__builder.AddMarkupContent(125, ".\n");
					num2 = this._powerTarget.DefaultChannel;
					if (num2 != null)
					{
						int valueOrDefault2 = num2.GetValueOrDefault();
						__builder.AddContent(126, "Channel ");
						__builder.AddContent(127, valueOrDefault2);
						__builder.AddContent(128, " is the port this unit hangs on in the topology; correct it here if the power wiring differs.");
					}
					else
					{
						__builder.AddContent(129, "The unit's port could not be read from the topology, so enter the power channel by hand.");
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(130, "\n                ");
					__builder.OpenElement(131, "label");
					__builder.AddAttribute(132, "class", "mt-2 block w-40");
					__builder.AddMarkupContent(133, "<span class=\"text-sm font-medium\">Power channel</span>\n                    ");
					__builder.OpenElement(134, "input");
					__builder.AddAttribute(135, "type", "number");
					__builder.AddAttribute(136, "min", "1");
					int num8 = 137;
					string text5 = "max";
					num2 = this._powerTarget.ChannelCount;
					__builder.AddAttribute(num8, text5, num2.GetValueOrDefault(16));
					__builder.AddAttribute(138, "class", "input");
					__builder.AddAttribute(139, "disabled", this._powerBusy);
					__builder.AddAttribute(140, "value", BindConverter.FormatValue(this._powerChannel, CultureInfo.InvariantCulture));
					__builder.AddAttribute<ChangeEventArgs>(141, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int __value)
					{
						this._powerChannel = __value;
					}, this._powerChannel, CultureInfo.InvariantCulture));
					__builder.SetUpdatesAttributeName("value");
					__builder.CloseElement();
					__builder.CloseElement();
					if (this._powerBusy)
					{
						__builder.AddMarkupContent(142, "<div class=\"card my-3 flex items-center gap-3 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800\"><span class=\"inline-block h-4 w-4 shrink-0 animate-spin rounded-full border-2 border-blush-200 border-t-blush-600\"></span>\n                        <span>Sending to the power board and reading it back…</span></div>");
					}
					else if (this._powerConfirmOff)
					{
						__builder.OpenElement(143, "div");
						__builder.AddAttribute(144, "class", "card my-3 border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
						__builder.AddMarkupContent(145, "\n                        This cuts the power to channel ");
						__builder.AddContent(146, this._powerChannel);
						__builder.AddMarkupContent(147, "; every handset on that PICO sender loses service. Continue?\n                        ");
						__builder.OpenElement(148, "div");
						__builder.AddAttribute(149, "class", "mt-2 flex gap-2");
						__builder.OpenElement(150, "button");
						__builder.AddAttribute(151, "type", "button");
						__builder.AddAttribute(152, "class", "btn-danger !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(153, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.PowerAsync(false)));
						__builder.AddContent(154, "Yes, power off");
						__builder.CloseElement();
						__builder.AddMarkupContent(155, "\n                            ");
						__builder.OpenElement(156, "button");
						__builder.AddAttribute(157, "type", "button");
						__builder.AddAttribute(158, "class", "btn-secondary !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(159, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
						{
							this._powerConfirmOff = false;
						}));
						__builder.AddContent(160, "Cancel");
						__builder.CloseElement();
						__builder.CloseElement();
						__builder.CloseElement();
					}
					else
					{
						__builder.OpenElement(161, "div");
						__builder.AddAttribute(162, "class", "mt-3 flex gap-2");
						__builder.OpenElement(163, "button");
						__builder.AddAttribute(164, "type", "button");
						__builder.AddAttribute(165, "class", "btn-danger !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(166, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
						{
							this._powerConfirmOff = true;
						}));
						__builder.AddAttribute(167, "disabled", this._powerChannel < 1);
						__builder.AddContent(168, "Power off");
						__builder.CloseElement();
						__builder.AddMarkupContent(169, "\n                        ");
						__builder.OpenElement(170, "button");
						__builder.AddAttribute(171, "type", "button");
						__builder.AddAttribute(172, "class", "btn-secondary !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(173, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.PowerAsync(true)));
						__builder.AddAttribute(174, "disabled", this._powerChannel < 1);
						__builder.AddContent(175, "Power on");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					OperationResult powerResult = this._powerResult;
					if (powerResult != null)
					{
						__builder.OpenElement(176, "p");
						__builder.AddAttribute(177, "class", "mt-3 text-xs");
						__builder.OpenElement(178, "span");
						__builder.AddAttribute(179, "class", powerResult.Ok ? "pill pill-up" : "pill pill-down");
						__builder.AddContent(180, powerResult.Ok ? "ok" : "failed");
						__builder.CloseElement();
						__builder.AddMarkupContent(181, "\n                        ");
						__builder.OpenElement(182, "span");
						__builder.AddAttribute(183, "class", "ml-2 font-medium text-cocoa-900");
						__builder.AddContent(184, powerResult.Label);
						__builder.CloseElement();
						__builder.AddMarkupContent(185, "\n                        ");
						__builder.OpenElement(186, "code");
						__builder.AddAttribute(187, "class", "ml-2 text-cocoa-700");
						__builder.AddContent(188, powerResult.Command);
						__builder.CloseElement();
						__builder.AddMarkupContent(189, "\n                        ");
						__builder.OpenElement(190, "span");
						__builder.AddAttribute(191, "class", "ml-2 " + (powerResult.Ok ? "text-cocoa-700" : "text-red-700"));
						__builder.AddContent(192, powerResult.Detail);
						__builder.CloseElement();
						__builder.CloseElement();
					}
				}
				__builder.CloseElement();
			}
			if (this._device.Category.Key == "mobile")
			{
				IGrouping<string, Metric> grouping = this.SystemGroup();
				if (grouping != null)
				{
					__builder.AddContent(193, renderFragment(grouping));
				}
			}
			if (this._device.Category.Key == "ru")
			{
				bool flag2 = this._device.CoordinateX != null || this._device.CoordinateY != null || this._device.CoordinateZ != null;
				__builder.OpenElement(194, "div");
				__builder.AddAttribute(195, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(196, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">Position</h2>");
				if (flag2)
				{
					__builder.OpenElement(197, "dl");
					__builder.AddAttribute(198, "class", "grid grid-cols-3 gap-4");
					foreach (ValueTuple<string, double?> valueTuple3 in new ValueTuple<string, double?>[]
					{
						new ValueTuple<string, double?>("X", this._device.CoordinateX),
						new ValueTuple<string, double?>("Y", this._device.CoordinateY),
						new ValueTuple<string, double?>("Z", this._device.CoordinateZ)
					})
					{
						string item = valueTuple3.Item1;
						double? item2 = valueTuple3.Item2;
						__builder.OpenElement(199, "div");
						__builder.OpenElement(200, "dt");
						__builder.AddAttribute(201, "class", "text-sm text-cocoa-700");
						__builder.AddContent(202, item);
						__builder.CloseElement();
						__builder.AddMarkupContent(203, "\n                            ");
						__builder.OpenElement(204, "dd");
						__builder.AddAttribute(205, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
						__builder.AddContent(206, ((item2 != null) ? item2.GetValueOrDefault().ToString() : null) ?? "—");
						__builder.CloseElement();
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
				else
				{
					__builder.OpenElement(207, "p");
					__builder.AddAttribute(208, "class", "text-sm text-cocoa-700");
					__builder.AddMarkupContent(209, "\n                    No coordinates set.\n");
					if (this._isAdmin)
					{
						__builder.OpenElement(210, "a");
						__builder.AddAttribute(211, "class", "text-blush-700 hover:underline");
						int num9 = 212;
						string text6 = "href";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler4.AppendLiteral("devices/detail/");
						defaultInterpolatedStringHandler4.AppendFormatted<int>(this._device.Id);
						defaultInterpolatedStringHandler4.AppendLiteral("/edit");
						__builder.AddAttribute(num9, text6, defaultInterpolatedStringHandler4.ToStringAndClear());
						__builder.AddContent(213, "Add them");
						__builder.CloseElement();
						__builder.AddContent(214, " to place this unit.");
					}
					__builder.CloseElement();
				}
				__builder.CloseElement();
			}
			if (this._device.Category.Key == "eu")
			{
				IGrouping<string, Metric> grouping2 = this.SystemGroup();
				if (grouping2 != null)
				{
					__builder.AddContent(215, renderFragment(grouping2));
				}
			}
			if (this._device.Category.Key == "eu")
			{
				__builder.OpenElement(216, "div");
				__builder.AddAttribute(217, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(218, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">Ports</h2>\n            ");
				__builder.OpenComponent<HubPorts>(219);
				__builder.AddComponentParameter(220, "Ports", RuntimeHelpers.TypeCheck<List<RhubPort>>(this._hubPorts));
				__builder.AddComponentParameter(221, "ShowMissingData", RuntimeHelpers.TypeCheck<bool>(true));
				__builder.AddComponentParameter(222, "ShowMissingPower", RuntimeHelpers.TypeCheck<bool>(true));
				__builder.AddComponentParameter(223, "MissingPowerText", RuntimeHelpers.TypeCheck<string>(this.PowerMissingReason()));
				__builder.CloseComponent();
				__builder.AddMarkupContent(224, "\n            ");
				__builder.AddMarkupContent(225, "<div class=\"mt-4 flex flex-wrap items-center gap-4 text-xs text-cocoa-700\"><span class=\"inline-flex items-center gap-1.5\"><span class=\"hub-tile hub-ok !h-3 !w-5\"></span> up</span>\n                <span class=\"inline-flex items-center gap-1.5\"><span class=\"hub-tile hub-idle !h-3 !w-5\"></span> idle / nothing connected</span>\n                <span class=\"inline-flex items-center gap-1.5\"><span class=\"hub-tile hub-fault !h-3 !w-5\"></span> fault</span>\n                <span class=\"inline-flex items-center gap-1.5\"><span class=\"hub-tile hub-unknown !h-3 !w-5\"></span> unknown (not measured)</span></div>");
				__builder.CloseElement();
			}
			if (this._mobileNetwork.Count > 0)
			{
				__builder.OpenElement(226, "div");
				__builder.AddAttribute(227, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(228, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">Mobile network</h2>\n            ");
				__builder.OpenElement(229, "dl");
				__builder.AddAttribute(230, "class", "grid gap-x-6 gap-y-2 text-sm sm:grid-cols-2");
				foreach (ValueTuple<string, string> valueTuple4 in this._mobileNetwork)
				{
					string item3 = valueTuple4.Item1;
					string item4 = valueTuple4.Item2;
					__builder.OpenElement(231, "div");
					__builder.AddAttribute(232, "class", "flex justify-between gap-4 border-b border-cream-100 py-1");
					__builder.OpenElement(233, "dt");
					__builder.AddAttribute(234, "class", "text-cocoa-700");
					__builder.AddContent(235, item3);
					__builder.CloseElement();
					__builder.AddMarkupContent(236, "\n                        ");
					__builder.OpenElement(237, "dd");
					__builder.AddAttribute(238, "class", "text-right font-medium text-cocoa-900");
					__builder.AddContent(239, item4);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._slices.Count > 0)
			{
				bool flag3 = this._slices.Any<DeviceDetail.SliceRow>((DeviceDetail.SliceRow x) => x.ReportedBy != null);
				bool flag4 = this._device.Category.Key != "mobile";
				__builder.OpenElement(240, "div");
				__builder.AddAttribute(241, "class", "card mb-4 overflow-x-auto p-5");
				__builder.OpenElement(242, "h2");
				__builder.AddAttribute(243, "class", "mb-3 text-lg font-semibold text-cocoa-900");
				__builder.AddContent(244, "Network slices (");
				__builder.AddContent(245, this._slices.Count);
				__builder.AddContent(246, ")");
				__builder.CloseElement();
				__builder.AddMarkupContent(247, "\n            ");
				__builder.OpenElement(248, "table");
				__builder.AddAttribute(249, "class", "w-full text-left text-sm");
				__builder.OpenElement(250, "thead");
				__builder.AddAttribute(251, "class", "border-b border-cream-200 text-cocoa-700");
				__builder.OpenElement(252, "tr");
				__builder.AddMarkupContent(253, "<th class=\"py-2 pr-4\">SST</th>");
				if (flag4)
				{
					__builder.AddMarkupContent(254, "<th class=\"py-2 pr-4\">Slice/Service Type</th>");
				}
				__builder.AddMarkupContent(255, "<th class=\"py-2 pr-4\">SD</th>");
				if (flag4)
				{
					__builder.AddMarkupContent(256, "<th class=\"py-2 pr-4\">S-NSSAI</th>");
				}
				if (flag3)
				{
					__builder.AddMarkupContent(257, "<th class=\"py-2 pr-4\">Reported by</th>");
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(258, "\n                ");
				__builder.OpenElement(259, "tbody");
				foreach (DeviceDetail.SliceRow sliceRow in this._slices)
				{
					__builder.OpenElement(260, "tr");
					__builder.AddAttribute(261, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(262, "td");
					__builder.AddAttribute(263, "class", "py-2 pr-4 font-medium text-cocoa-900");
					int num10 = 264;
					int? num2 = sliceRow.Sst;
					__builder.AddContent(num10, ((num2 != null) ? num2.GetValueOrDefault().ToString() : null) ?? "—");
					__builder.CloseElement();
					if (flag4)
					{
						__builder.OpenElement(265, "td");
						__builder.AddAttribute(266, "class", "py-2 pr-4 text-cocoa-700");
						__builder.AddContent(267, sliceRow.SstName ?? "—");
						__builder.CloseElement();
					}
					__builder.OpenElement(268, "td");
					__builder.AddAttribute(269, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(270, sliceRow.Sd ?? "no SD");
					__builder.CloseElement();
					if (flag4)
					{
						__builder.OpenElement(271, "td");
						__builder.AddAttribute(272, "class", "py-2 pr-4 font-mono text-xs text-cocoa-700");
						__builder.AddContent(273, sliceRow.Raw);
						__builder.CloseElement();
					}
					if (flag3)
					{
						__builder.OpenElement(274, "td");
						__builder.AddAttribute(275, "class", "py-2 pr-4 text-cocoa-700");
						__builder.AddContent(276, sliceRow.ReportedBy ?? "—");
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._trends.Count > 0)
			{
				__builder.OpenElement(277, "div");
				__builder.AddAttribute(278, "class", "card mb-4 p-5");
				__builder.OpenElement(279, "div");
				__builder.AddAttribute(280, "class", "mb-3 flex items-baseline justify-between gap-2");
				__builder.AddMarkupContent(281, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Trends</h2>\n                ");
				__builder.OpenElement(282, "span");
				__builder.AddAttribute(283, "class", "text-xs text-cocoa-700");
				__builder.AddContent(284, "last ");
				__builder.AddContent(285, this._trends.Max<Trend>((Trend t) => t.Points.Count));
				__builder.AddContent(286, " samples");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(287, "\n            ");
				__builder.OpenElement(288, "div");
				__builder.AddAttribute(289, "class", "grid gap-3 sm:grid-cols-2 lg:grid-cols-3");
				foreach (Trend trend in this._trends)
				{
					__builder.OpenComponent<Sparkline>(290);
					__builder.AddComponentParameter(291, "Title", RuntimeHelpers.TypeCheck<string>(trend.Title));
					__builder.AddComponentParameter(292, "Unit", RuntimeHelpers.TypeCheck<string>(trend.Unit));
					__builder.AddComponentParameter(293, "Color", RuntimeHelpers.TypeCheck<string>(DeviceDetail.TrendColor(trend)));
					__builder.AddComponentParameter(294, "Points", RuntimeHelpers.TypeCheck<List<HistoryPoint>>(trend.Points));
					__builder.CloseComponent();
				}
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._snapshot == null || (this._snapshot.Metrics.Count == 0 && this._snapshot.Ports.Count == 0 && this._snapshot.Children.Count == 0))
			{
				__builder.AddMarkupContent(295, "<div class=\"card p-8 text-center text-cocoa-700\">No data collected yet. The poller runs on the device's interval.</div>");
				return;
			}
			foreach (IGrouping<string, Metric> grouping3 in from m in this._snapshot.Metrics
				group m by m.Group ?? "Metrics" into g
				where !this.IsGroupHidden(g.Key)
				select g)
			{
				__builder.AddContent(296, renderFragment(grouping3));
			}
			if (this._snapshot.Ports.Count > 0)
			{
				List<PortStatus> list = this._snapshot.Ports.Where<PortStatus>((PortStatus p) => p.Physical).ToList<PortStatus>();
				if (list.Count == 0)
				{
					list = this._snapshot.Ports;
				}
				__builder.OpenElement(297, "div");
				__builder.AddAttribute(298, "class", "card mb-4 p-5");
				__builder.AddMarkupContent(299, "<div class=\"mb-3 flex flex-wrap items-center justify-between gap-2\"><h2 class=\"text-lg font-semibold text-cocoa-900\">Port matrix</h2>\n                    <div class=\"flex items-center gap-3 text-xs text-cocoa-700\"><span class=\"inline-flex items-center gap-1\"><span class=\"h-3 w-3 rounded bg-moss-100 ring-1 ring-moss-500\"></span> up</span>\n                        <span class=\"inline-flex items-center gap-1\"><span class=\"h-3 w-3 rounded bg-cream-100 ring-1 ring-cream-300\"></span> idle</span>\n                        <span class=\"inline-flex items-center gap-1\"><span class=\"h-3 w-3 rounded bg-red-100 ring-1 ring-red-300\"></span> disabled</span></div></div>\n                ");
				__builder.OpenElement(300, "div");
				__builder.AddAttribute(301, "class", "port-grid");
				foreach (PortStatus portStatus in list)
				{
					ValueTuple<string, string> valueTuple5 = DeviceStatusView.PortCell(portStatus);
					__builder.OpenElement(302, "div");
					__builder.AddAttribute(303, "class", valueTuple5.Item1);
					__builder.AddAttribute(304, "title", portStatus.Name + " — " + valueTuple5.Item2);
					__builder.AddContent(305, portStatus.Index);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._snapshot.Ports.Count > 0)
			{
				__builder.OpenElement(306, "div");
				__builder.AddAttribute(307, "class", "card mb-4 overflow-x-auto p-5");
				__builder.OpenElement(308, "h2");
				__builder.AddAttribute(309, "class", "mb-3 text-lg font-semibold text-cocoa-900");
				__builder.AddContent(310, "Ports (");
				__builder.AddContent(311, this._snapshot.Ports.Count);
				__builder.AddContent(312, ")");
				__builder.CloseElement();
				__builder.AddMarkupContent(313, "\n                ");
				__builder.OpenElement(314, "table");
				__builder.AddAttribute(315, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(316, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"py-2 pr-4\">#</th><th class=\"py-2 pr-4\">Interface</th><th class=\"py-2 pr-4\">Oper</th><th class=\"py-2 pr-4\">Admin</th></tr></thead>\n                    ");
				__builder.OpenElement(317, "tbody");
				foreach (PortStatus portStatus2 in this._snapshot.Ports)
				{
					__builder.OpenElement(318, "tr");
					__builder.AddAttribute(319, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(320, "td");
					__builder.AddAttribute(321, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(322, portStatus2.Index);
					__builder.CloseElement();
					__builder.AddMarkupContent(323, "\n                                ");
					__builder.OpenElement(324, "td");
					__builder.AddAttribute(325, "class", "py-2 pr-4 font-medium text-cocoa-900");
					__builder.AddContent(326, portStatus2.Name);
					__builder.CloseElement();
					__builder.AddMarkupContent(327, "\n                                ");
					__builder.OpenElement(328, "td");
					__builder.AddAttribute(329, "class", "py-2 pr-4");
					__builder.OpenElement(330, "span");
					__builder.AddAttribute(331, "class", portStatus2.Up ? "pill pill-up" : "pill pill-down");
					__builder.AddContent(332, portStatus2.Detail ?? (portStatus2.Up ? "up" : "down"));
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(333, "\n                                ");
					__builder.OpenElement(334, "td");
					__builder.AddAttribute(335, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(336, portStatus2.AdminStatus);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._snapshot.Alarms.Count > 0)
			{
				__builder.OpenElement(337, "div");
				__builder.AddAttribute(338, "class", "card mb-4 overflow-x-auto p-5");
				__builder.OpenElement(339, "h2");
				__builder.AddAttribute(340, "class", "mb-3 text-lg font-semibold text-cocoa-900");
				__builder.AddContent(341, "Active alarms (");
				__builder.AddContent(342, this._snapshot.Alarms.Count);
				__builder.AddContent(343, ")");
				__builder.CloseElement();
				__builder.AddMarkupContent(344, "\n                ");
				__builder.OpenElement(345, "table");
				__builder.AddAttribute(346, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(347, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"py-2 pr-4\">Severity</th>\n                            <th class=\"py-2 pr-4\">Alarm</th>\n                            <th class=\"py-2 pr-4\">Description</th>\n                            <th class=\"py-2 pr-4\">Raised</th></tr></thead>\n                    ");
				__builder.OpenElement(348, "tbody");
				foreach (AlarmStatus alarmStatus in this._snapshot.Alarms)
				{
					__builder.OpenElement(349, "tr");
					__builder.AddAttribute(350, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(351, "td");
					__builder.AddAttribute(352, "class", "py-2 pr-4");
					__builder.OpenElement(353, "span");
					__builder.AddAttribute(354, "class", DeviceDetail.SeverityPill(alarmStatus.Severity));
					__builder.AddContent(355, alarmStatus.Severity ?? "—");
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(356, "\n                                ");
					__builder.OpenElement(357, "td");
					__builder.AddAttribute(358, "class", "py-2 pr-4 font-medium text-cocoa-900");
					__builder.AddContent(359, alarmStatus.Name);
					__builder.CloseElement();
					__builder.AddMarkupContent(360, "\n                                ");
					__builder.OpenElement(361, "td");
					__builder.AddAttribute(362, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(363, alarmStatus.Description);
					__builder.CloseElement();
					__builder.AddMarkupContent(364, "\n                                ");
					__builder.OpenElement(365, "td");
					__builder.AddAttribute(366, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(367, alarmStatus.RaisedAt);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			if (this._snapshot.Children.Count > 0 && !DeviceDetail.NoConnectedUnits.Contains(this._device.Category.Key))
			{
				bool flag5 = this._snapshot.Children.Any<ChildStatus>((ChildStatus c) => !string.IsNullOrWhiteSpace(c.Model));
				bool flag6 = this._snapshot.Children.Any<ChildStatus>((ChildStatus c) => !string.IsNullOrWhiteSpace(c.SerialNumber));
				bool flag7 = this._snapshot.Children.Any<ChildStatus>((ChildStatus c) => !string.IsNullOrWhiteSpace(c.Version));
				__builder.OpenElement(368, "div");
				__builder.AddAttribute(369, "class", "card mb-4 overflow-x-auto p-5");
				__builder.OpenElement(370, "h2");
				__builder.AddAttribute(371, "class", "mb-3 text-lg font-semibold text-cocoa-900");
				__builder.AddContent(372, "Connected units (");
				__builder.AddContent(373, this._snapshot.Children.Count);
				__builder.AddContent(374, ")");
				__builder.CloseElement();
				__builder.AddMarkupContent(375, "\n                ");
				__builder.AddMarkupContent(376, "<p class=\"mb-3 text-xs text-cocoa-700\">Nodes discovered through this device. They are tracked automatically in their own categories.</p>\n                ");
				__builder.OpenElement(377, "table");
				__builder.AddAttribute(378, "class", "w-full text-left text-sm");
				__builder.OpenElement(379, "thead");
				__builder.AddAttribute(380, "class", "border-b border-cream-200 text-cocoa-700");
				__builder.OpenElement(381, "tr");
				__builder.AddMarkupContent(382, "<th class=\"py-2 pr-4\">Unit</th>\n                            ");
				__builder.AddMarkupContent(383, "<th class=\"py-2 pr-4\">Kind</th>");
				if (flag5)
				{
					__builder.AddMarkupContent(384, "<th class=\"hidden py-2 pr-4 sm:table-cell\">Model</th>");
				}
				if (flag6)
				{
					__builder.AddMarkupContent(385, "<th class=\"hidden py-2 pr-4 md:table-cell\">Serial</th>");
				}
				if (flag7)
				{
					__builder.AddMarkupContent(386, "<th class=\"hidden py-2 pr-4 lg:table-cell\">Version</th>");
				}
				__builder.AddMarkupContent(387, "<th class=\"py-2 pr-4\">State</th>");
				if (this._isAdmin)
				{
					__builder.AddMarkupContent(388, "<th class=\"py-2 pr-4\"></th>");
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(389, "\n                    ");
				__builder.OpenElement(390, "tbody");
				foreach (ChildStatus childStatus in this._snapshot.Children)
				{
					ValueTuple<string, string> valueTuple6 = (string.Equals(childStatus.Kind, "ue", StringComparison.OrdinalIgnoreCase) ? DeviceStatusView.UeBadge(childStatus.Up, childStatus.State) : DeviceStatusView.StateBadge(childStatus.Up, childStatus.State));
					__builder.OpenElement(391, "tr");
					__builder.AddAttribute(392, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(393, "td");
					__builder.AddAttribute(394, "class", "py-2 pr-4 font-medium text-cocoa-900");
					__builder.AddContent(395, childStatus.Identifier);
					__builder.CloseElement();
					__builder.AddMarkupContent(396, "\n                                ");
					__builder.OpenElement(397, "td");
					__builder.AddAttribute(398, "class", "py-2 pr-4 text-cocoa-700");
					__builder.AddContent(399, childStatus.Kind);
					__builder.CloseElement();
					if (flag5)
					{
						__builder.OpenElement(400, "td");
						__builder.AddAttribute(401, "class", "hidden py-2 pr-4 text-cocoa-700 sm:table-cell");
						__builder.AddContent(402, childStatus.Model);
						__builder.CloseElement();
					}
					if (flag6)
					{
						__builder.OpenElement(403, "td");
						__builder.AddAttribute(404, "class", "hidden py-2 pr-4 text-cocoa-700 md:table-cell");
						__builder.AddContent(405, childStatus.SerialNumber);
						__builder.CloseElement();
					}
					if (flag7)
					{
						__builder.OpenElement(406, "td");
						__builder.AddAttribute(407, "class", "hidden py-2 pr-4 text-cocoa-700 lg:table-cell");
						__builder.AddContent(408, childStatus.Version);
						__builder.CloseElement();
					}
					__builder.OpenElement(409, "td");
					__builder.AddAttribute(410, "class", "py-2 pr-4");
					__builder.OpenElement(411, "span");
					__builder.AddAttribute(412, "class", valueTuple6.Item1);
					__builder.AddContent(413, valueTuple6.Item2);
					__builder.CloseElement();
					__builder.CloseElement();
					if (this._isAdmin)
					{
						__builder.OpenElement(414, "td");
						__builder.AddAttribute(415, "class", "py-2 pr-4 text-right");
						Device device = this.FindDevice(childStatus);
						if (device != null)
						{
							__builder.OpenElement(416, "a");
							__builder.AddAttribute(417, "class", "text-blush-700 hover:underline");
							int num11 = 418;
							string text7 = "href";
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(15, 1);
							defaultInterpolatedStringHandler5.AppendLiteral("devices/detail/");
							defaultInterpolatedStringHandler5.AppendFormatted<int>(device.Id);
							__builder.AddAttribute(num11, text7, defaultInterpolatedStringHandler5.ToStringAndClear());
							__builder.AddContent(419, "View");
							__builder.CloseElement();
						}
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00027AB4 File Offset: 0x00025CB4
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00027ABC File Offset: 0x00025CBC
		[Parameter]
		public int Id { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00027AC5 File Offset: 0x00025CC5
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00027ACD File Offset: 0x00025CCD
		[Nullable(new byte[] { 2, 1 })]
		[CascadingParameter]
		private Task<AuthenticationState> AuthState
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00027AD8 File Offset: 0x00025CD8
		private bool IsGroupHidden(string group)
		{
			HashSet<string> hashSet;
			return (DeviceDetail.HiddenGroups.TryGetValue(this._device.Category.Key, out hashSet) && hashSet.Contains(group)) || (this._mobileNetwork.Count > 0 && group == "Mobile network");
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00027B2C File Offset: 0x00025D2C
		[return: Nullable(new byte[] { 2, 1, 1 })]
		private IGrouping<string, Metric> SystemGroup()
		{
			DeviceSnapshot snapshot = this._snapshot;
			if (snapshot == null)
			{
				return null;
			}
			return (from m in snapshot.Metrics
				group m by m.Group ?? "Metrics").FirstOrDefault<IGrouping<string, Metric>>((IGrouping<string, Metric> g) => g.Key == "System");
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00027B94 File Offset: 0x00025D94
		protected override async Task OnParametersSetAsync()
		{
			await this.LoadAsync();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00027BD7 File Offset: 0x00025DD7
		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender && base.RendererInfo.IsInteractive)
			{
				this.RefreshLoopAsync();
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00027BF0 File Offset: 0x00025DF0
		private async Task LoadAsync()
		{
			DeviceDetail.<>c__DisplayClass34_0 CS$<>8__locals1 = new DeviceDetail.<>c__DisplayClass34_0();
			CS$<>8__locals1.<>4__this = this;
			this._confirmDelete = false;
			if (this.AuthState != null)
			{
				AuthenticationState authenticationState = await this.AuthState;
				this._isAdmin = authenticationState.User.IsInRole("Admin");
			}
			ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable3 = queryable2.Include<Device, Device>(Expression.Lambda<Func<Device, Device>>(Expression.Property(parameterExpression, methodof(Device.get_ParentDevice())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				this._device = await queryable3.FirstOrDefaultAsync<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(DeviceDetail)), methodof(DeviceDetail.get_Id()))), new ParameterExpression[] { parameterExpression }), this._cts.Token);
				if (this._device == null)
				{
					this._notFound = true;
				}
				else
				{
					this._snapshot = SnapshotSerializer.Deserialize(this._device.LastSnapshotJson);
					DeviceSnapshot snapshot = this._snapshot;
					this._hubPorts = ((snapshot != null) ? snapshot.HubPorts : null) ?? new List<RhubPort>();
					if (this._device.Category.Key == "ru" && this._isAdmin)
					{
						this._powerTarget = await this.Runner.DescribePrruPowerAsync(this._device.Id, this._cts.Token);
						if (this._powerChannel < 1)
						{
							int? defaultChannel = this._powerTarget.DefaultChannel;
							if (defaultChannel != null)
							{
								this._powerChannel = defaultChannel.GetValueOrDefault();
							}
						}
					}
					if (this._device.Category.Key == "mobile" && this._snapshot != null)
					{
						await DeviceDetail.AddUeCountsAsync(db, this._device.Id, this._snapshot);
					}
					string key = this._device.Category.Key;
					ValueTuple<List<ValueTuple<string, string>>, List<DeviceDetail.SliceRow>> valueTuple = ((key == "mobile") ? (await this.BuildMobileNetworkAsync(db, this._snapshot)) : ((key == "bbu") ? new ValueTuple<List<ValueTuple<string, string>>, List<DeviceDetail.SliceRow>>(new List<ValueTuple<string, string>>(), DeviceDetail.BuildSlices(DeviceDetail.OwnMobileNetwork(this._device.Name, this._snapshot))) : new ValueTuple<List<ValueTuple<string, string>>, List<DeviceDetail.SliceRow>>(new List<ValueTuple<string, string>>(), new List<DeviceDetail.SliceRow>())));
					this._mobileNetwork = valueTuple.Item1;
					this._slices = valueTuple.Item2;
					this._baseStationId = ((!(this._device.Category.Key == "ue")) ? null : BaseStationLookup.Resolve(await BaseStationLookup.ByGnbIdAsync(db, this._cts.Token), this._snapshot));
					IQueryable<DevicePollResult> devicePollResults = db.DevicePollResults;
					parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
					IQueryable<DevicePollResult> queryable4 = devicePollResults.Where<DevicePollResult>(Expression.Lambda<Func<DevicePollResult, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(DevicePollResult.get_DeviceId())), Expression.Property(Expression.Constant(this, typeof(DeviceDetail)), methodof(DeviceDetail.get_Id()))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
					List<DevicePollResult> list = await queryable4.OrderByDescending<DevicePollResult, DateTime>(Expression.Lambda<Func<DevicePollResult, DateTime>>(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), new ParameterExpression[] { parameterExpression })).Take<DevicePollResult>(200).ToListAsync<DevicePollResult>(this._cts.Token);
					IReadOnlyCollection<DevicePollResult> readOnlyCollection = list;
					DeviceCategory category = this._device.Category;
					this._trends = MetricHistory.SelectTrends(readOnlyCollection, (category != null) ? category.Key : null, 12);
					DeviceDetail.<>c__DisplayClass34_0 CS$<>8__locals2 = CS$<>8__locals1;
					DeviceSnapshot snapshot2 = this._snapshot;
					List<string> list2;
					if (snapshot2 == null)
					{
						list2 = null;
					}
					else
					{
						list2 = (from c in snapshot2.Children
							select c.SerialNumber into s
							where !string.IsNullOrWhiteSpace(s)
							select s).ToList<string>();
					}
					CS$<>8__locals2.serials = list2 ?? new List<string>();
					DeviceDetail.<>c__DisplayClass34_0 CS$<>8__locals3 = CS$<>8__locals1;
					DeviceSnapshot snapshot3 = this._snapshot;
					List<string> list3;
					if (snapshot3 == null)
					{
						list3 = null;
					}
					else
					{
						list3 = (from c in snapshot3.Children
							select c.Identifier into s
							where !string.IsNullOrWhiteSpace(s)
							select s).ToList<string>();
					}
					CS$<>8__locals3.idents = list3 ?? new List<string>();
					IQueryable<Device> devices2 = db.Devices;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					List<Device> list4 = await devices2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(DeviceDetail)), methodof(DeviceDetail.get_Id()))), Expression.OrElse(Expression.AndAlso(Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_SerialNumber())), Expression.Constant(null, typeof(string))), Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DeviceDetail.<>c__DisplayClass34_0)), fieldof(DeviceDetail.<>c__DisplayClass34_0.serials)), methodof(List<string>.Contains(T)), new Expression[] { Expression.Property(parameterExpression, methodof(Device.get_SerialNumber())) })), Expression.AndAlso(Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_ChildIdentifier())), Expression.Constant(null, typeof(string))), Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DeviceDetail.<>c__DisplayClass34_0)), fieldof(DeviceDetail.<>c__DisplayClass34_0.idents)), methodof(List<string>.Contains(T)), new Expression[] { Expression.Property(parameterExpression, methodof(Device.get_ChildIdentifier())) })))), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(this._cts.Token);
					this._deviceByKey = new Dictionary<string, Device>(StringComparer.OrdinalIgnoreCase);
					foreach (Device device in list4)
					{
						if (!string.IsNullOrWhiteSpace(device.SerialNumber))
						{
							this._deviceByKey[device.SerialNumber.Trim()] = device;
						}
						if (!string.IsNullOrWhiteSpace(device.ChildIdentifier))
						{
							this._deviceByKey.TryAdd(device.ChildIdentifier.Trim(), device);
						}
					}
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
			CS$<>8__locals1 = null;
			db = null;
			throw null;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00027C34 File Offset: 0x00025E34
		[return: TupleElementNames(new string[] { "Rows", "Slices", "Label", "Value" })]
		[return: Nullable(new byte[] { 1, 0, 1, 0, 1, 1, 1, 1 })]
		private async Task<ValueTuple<List<ValueTuple<string, string>>, List<DeviceDetail.SliceRow>>> BuildMobileNetworkAsync(ApplicationDbContext db, [Nullable(2)] DeviceSnapshot core)
		{
			List<Metric> list;
			if (core == null)
			{
				list = null;
			}
			else
			{
				list = core.Metrics.Where<Metric>((Metric m) => m.Group == "Mobile network").ToList<Metric>();
			}
			List<Metric> coreRows = list ?? new List<Metric>();
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			IQueryable<Device> queryable = devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string))), Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_LastSnapshotJson())), Expression.Constant(null, typeof(string)))), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			var list2 = await queryable.Select(Expression.Lambda(Expression.New(methodof(<>f__AnonymousType17<string, string>..ctor(<Name>j__TPar, <LastSnapshotJson>j__TPar)), new Expression[]
			{
				Expression.Property(parameterExpression, methodof(Device.get_Name())),
				Expression.Property(parameterExpression, methodof(Device.get_LastSnapshotJson()))
			}, new MemberInfo[]
			{
				methodof(<>f__AnonymousType17<string, string>.get_Name()),
				methodof(<>f__AnonymousType17<string, string>.get_LastSnapshotJson())
			}), new ParameterExpression[] { parameterExpression })).ToListAsync(this._cts.Token);
			Dictionary<string, List<ValueTuple<string, string>>> dictionary = new Dictionary<string, List<ValueTuple<string, string>>>(StringComparer.Ordinal);
			foreach (var anon in list2)
			{
				DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(anon.LastSnapshotJson);
				foreach (Metric metric in (((deviceSnapshot != null) ? deviceSnapshot.Metrics : null) ?? new List<Metric>()))
				{
					if (!(metric.Group != "Mobile network") && !string.IsNullOrWhiteSpace(metric.Value))
					{
						List<ValueTuple<string, string>> list3;
						if (!dictionary.TryGetValue(metric.Label, out list3))
						{
							list3 = (dictionary[metric.Label] = new List<ValueTuple<string, string>>());
						}
						list3.Add(new ValueTuple<string, string>(anon.Name, metric.Value));
					}
				}
			}
			List<ValueTuple<string, string>> list4 = new List<ValueTuple<string, string>>();
			string[] mobileNetworkOrder = DeviceDetail.MobileNetworkOrder;
			for (int i = 0; i < mobileNetworkOrder.Length; i++)
			{
				string label = mobileNetworkOrder[i];
				Metric metric2 = coreRows.FirstOrDefault<Metric>((Metric m) => m.Label == label);
				string text = ((metric2 != null) ? metric2.Value : null);
				List<ValueTuple<string, string>> list5;
				if (!string.IsNullOrWhiteSpace(text))
				{
					list4.Add(new ValueTuple<string, string>(label, text));
				}
				else if (dictionary.TryGetValue(label, out list5))
				{
					list4.Add(new ValueTuple<string, string>(label, DeviceDetail.MergeBbuValues(list5)));
				}
			}
			foreach (Metric metric3 in coreRows.Where<Metric>((Metric m) => !DeviceDetail.MobileNetworkOrder.Contains(m.Label) && !DeviceDetail.SliceLabels.Contains(m.Label)))
			{
				list4.Add(new ValueTuple<string, string>(metric3.Label, metric3.Value));
			}
			return new ValueTuple<List<ValueTuple<string, string>>, List<DeviceDetail.SliceRow>>(list4, DeviceDetail.BuildSlices(dictionary));
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00027C88 File Offset: 0x00025E88
		[return: TupleElementNames(new string[] { "Bbu", "Value" })]
		[return: Nullable(new byte[] { 1, 1, 1, 0, 1, 1 })]
		private static Dictionary<string, List<ValueTuple<string, string>>> OwnMobileNetwork(string name, [Nullable(2)] DeviceSnapshot snapshot)
		{
			Dictionary<string, List<ValueTuple<string, string>>> dictionary = new Dictionary<string, List<ValueTuple<string, string>>>(StringComparer.Ordinal);
			foreach (Metric metric in (((snapshot != null) ? snapshot.Metrics : null) ?? new List<Metric>()))
			{
				if (metric.Group == "Mobile network" && !string.IsNullOrWhiteSpace(metric.Value))
				{
					dictionary[metric.Label] = new List<ValueTuple<string, string>>
					{
						new ValueTuple<string, string>(name, metric.Value)
					};
				}
			}
			return dictionary;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00027D34 File Offset: 0x00025F34
		private static List<DeviceDetail.SliceRow> BuildSlices([TupleElementNames(new string[] { "Bbu", "Value" })] [Nullable(new byte[] { 1, 1, 1, 0, 1, 1 })] Dictionary<string, List<ValueTuple<string, string>>> fromBbu)
		{
			List<ValueTuple<string, string>> list;
			if (!fromBbu.TryGetValue("SNSSAI", out list))
			{
				return new List<DeviceDetail.SliceRow>();
			}
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
			foreach (ValueTuple<string, string> valueTuple in list)
			{
				string item = valueTuple.Item1;
				foreach (string text in valueTuple.Item2.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
				{
					List<string> list2;
					if (!dictionary.TryGetValue(text, out list2))
					{
						list2 = (dictionary[text] = new List<string>());
					}
					if (!list2.Contains(item))
					{
						list2.Add(item);
					}
				}
			}
			int bbuCount = list.Select<ValueTuple<string, string>, string>(([TupleElementNames(new string[] { "Bbu", "Value" })] ValueTuple<string, string> r) => r.Item1).Distinct<string>(StringComparer.Ordinal).Count<string>();
			bool agreed = dictionary.Values.All<List<string>>((List<string> w) => w.Count == bbuCount);
			return (from r in dictionary.Select<KeyValuePair<string, List<string>>, DeviceDetail.SliceRow>(delegate(KeyValuePair<string, List<string>> kv)
				{
					Snssai? snssai = MobileNetworkFormat.ParseSnssai(kv.Key);
					return new DeviceDetail.SliceRow(kv.Key, (snssai != null) ? new int?(snssai.GetValueOrDefault().Sst) : null, (snssai == null) ? null : MobileNetworkFormat.SstName(snssai.GetValueOrDefault().Sst), (snssai != null) ? snssai.GetValueOrDefault().Sd : null, agreed ? null : string.Join(", ", kv.Value));
				})
				orderby r.Sst.GetValueOrDefault(int.MaxValue)
				select r).ThenBy<DeviceDetail.SliceRow, string>((DeviceDetail.SliceRow r) => r.Raw, StringComparer.Ordinal).ToList<DeviceDetail.SliceRow>();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00027EBC File Offset: 0x000260BC
		private static string MergeBbuValues([TupleElementNames(new string[] { "Bbu", "Value" })] [Nullable(new byte[] { 1, 0, 1, 1 })] List<ValueTuple<string, string>> reported)
		{
			List<IGrouping<string, ValueTuple<string, string>>> list = reported.GroupBy<ValueTuple<string, string>, string>(([TupleElementNames(new string[] { "Bbu", "Value" })] ValueTuple<string, string> r) => r.Item2, StringComparer.OrdinalIgnoreCase).ToList<IGrouping<string, ValueTuple<string, string>>>();
			if (list.Count == 1)
			{
				return list[0].Key;
			}
			return string.Join(", ", list.Select<IGrouping<string, ValueTuple<string, string>>, string>(([TupleElementNames(new string[] { "Bbu", "Value" })] IGrouping<string, ValueTuple<string, string>> g) => g.Key + " (" + string.Join("/", g.Select<ValueTuple<string, string>, string>(([TupleElementNames(new string[] { "Bbu", "Value" })] ValueTuple<string, string> r) => r.Item1).Distinct<string>(StringComparer.Ordinal)) + ")"));
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00027F40 File Offset: 0x00026140
		[return: Nullable(2)]
		private string MetricHref(Metric m)
		{
			if (this._device.Category.Key == "ue" && m.Label == "Base station")
			{
				int? baseStationId = this._baseStationId;
				if (baseStationId != null)
				{
					int valueOrDefault = baseStationId.GetValueOrDefault();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
					defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return null;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00027FBC File Offset: 0x000261BC
		private string PowerMissingReason()
		{
			Device device = this._device;
			if (!string.IsNullOrWhiteSpace((device != null) ? device.SerialPort : null))
			{
				return "nothing read from " + this._device.SerialPort + " — check the port, baud/framing and password";
			}
			return "no serial console configured — add one under Edit";
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00027FF8 File Offset: 0x000261F8
		[return: Nullable(2)]
		private Device FindDevice(ChildStatus ch)
		{
			Device device;
			if (!string.IsNullOrWhiteSpace(ch.SerialNumber) && this._deviceByKey.TryGetValue(ch.SerialNumber.Trim(), out device))
			{
				return device;
			}
			Device device2;
			if (!string.IsNullOrWhiteSpace(ch.Identifier) && this._deviceByKey.TryGetValue(ch.Identifier.Trim(), out device2))
			{
				return device2;
			}
			return null;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00028058 File Offset: 0x00026258
		private static string SeverityPill([Nullable(2)] string severity)
		{
			string text = ((severity != null) ? severity.ToLowerInvariant() : null);
			string text2;
			if (!(text == "critical") && !(text == "major"))
			{
				if (!(text == "minor") && !(text == "warning"))
				{
					text2 = "pill pill-unknown";
				}
				else
				{
					text2 = "pill pill-unknown";
				}
			}
			else
			{
				text2 = "pill pill-down";
			}
			return text2;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000280C0 File Offset: 0x000262C0
		private static string TrendColor(Trend t)
		{
			string title = t.Title;
			bool flag = title == "Active alarms" || title == "Alarms";
			if (flag)
			{
				return "#dc2626";
			}
			string text = t.Group.ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 2:
					if (text == "ue")
					{
						return "#059669";
					}
					break;
				case 5:
					if (text == "power")
					{
						return "#d97706";
					}
					break;
				case 6:
				{
					char c = text[0];
					if (c != 'h')
					{
						if (c == 's')
						{
							if (text == "system")
							{
								return "#0f766e";
							}
						}
					}
					else if (text == "health")
					{
						return "#db2777";
					}
					break;
				}
				case 7:
					if (text == "optical")
					{
						return "#0f766e";
					}
					break;
				case 9:
				{
					char c = text[0];
					if (c != 'c')
					{
						if (c != 'r')
						{
							break;
						}
						if (!(text == "resources"))
						{
							break;
						}
					}
					else if (!(text == "cpu cores"))
					{
						break;
					}
					return "#7c3aed";
				}
				case 10:
					if (text == "throughput")
					{
						return "#059669";
					}
					break;
				case 11:
					if (text == "performance")
					{
						return "#2563eb";
					}
					break;
				}
			}
			return "#2563eb";
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00028268 File Offset: 0x00026468
		private async Task<bool> EnsureAdminAsync()
		{
			bool flag;
			if (this.AuthState == null)
			{
				flag = false;
			}
			else
			{
				flag = (await this.AuthState).User.IsInRole("Admin");
			}
			return flag;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000282AC File Offset: 0x000264AC
		private async Task PollNow()
		{
			TaskAwaiter<bool> taskAwaiter = this.EnsureAdminAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult() && this._device != null)
			{
				this._confirmDelete = false;
				this._busy = true;
				try
				{
					ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
					object obj = null;
					try
					{
						IQueryable<Device> devices = db.Devices;
						ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
						await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.OrElse(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Field(Expression.Constant(this, typeof(DeviceDetail)), fieldof(DeviceDetail._device)), methodof(Device.get_Id()))), Expression.Equal(Expression.Convert(Expression.Property(parameterExpression, methodof(Device.get_Id())), typeof(int?)), Expression.Property(Expression.Field(Expression.Constant(this, typeof(DeviceDetail)), fieldof(DeviceDetail._device)), methodof(Device.get_ParentDeviceId())))), new ParameterExpression[] { parameterExpression })).ExecuteUpdateAsync<Device>(delegate([Nullable(new byte[] { 1, 0 })] UpdateSettersBuilder<Device> s)
						{
							s.SetProperty<DateTime?>((Device d) => d.LastPolledAt, null);
						}, this._cts.Token);
						this._message = "Poll requested — refreshing shortly.";
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
					obj = null;
					db = null;
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000282F0 File Offset: 0x000264F0
		private async Task ToggleEnabled()
		{
			TaskAwaiter<bool> taskAwaiter = this.EnsureAdminAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult() && this._device != null)
			{
				this._busy = true;
				try
				{
					DeviceDetail.<>c__DisplayClass46_0 CS$<>8__locals1 = new DeviceDetail.<>c__DisplayClass46_0();
					ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
					object obj = null;
					try
					{
						CS$<>8__locals1.newValue = !this._device.Enabled;
						IQueryable<Device> devices = db.Devices;
						ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
						await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Field(Expression.Constant(this, typeof(DeviceDetail)), fieldof(DeviceDetail._device)), methodof(Device.get_Id()))), new ParameterExpression[] { parameterExpression })).ExecuteUpdateAsync<Device>(delegate([Nullable(new byte[] { 1, 0 })] UpdateSettersBuilder<Device> s)
						{
							s.SetProperty<bool>((Device d) => d.Enabled, CS$<>8__locals1.newValue);
						}, this._cts.Token);
						await this.LoadAsync();
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
					obj = null;
					CS$<>8__locals1 = null;
					db = null;
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00028334 File Offset: 0x00026534
		private async Task Delete()
		{
			TaskAwaiter<bool> taskAwaiter = this.EnsureAdminAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult() && this._device != null)
			{
				this._busy = true;
				try
				{
					ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
					object obj = null;
					int num = 0;
					try
					{
						if (!this._confirmDelete)
						{
							int num2 = await DeviceDetail.CountDescendantsAsync(db, this._device.Id, this._cts.Token);
							this._confirmDelete = true;
							string text;
							if (num2 <= 0)
							{
								text = "Delete this device? Click Delete again to confirm.";
							}
							else
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(97, 2);
								defaultInterpolatedStringHandler.AppendLiteral("This will also delete ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
								defaultInterpolatedStringHandler.AppendLiteral(" dependent unit");
								defaultInterpolatedStringHandler.AppendFormatted((num2 == 1) ? "" : "s");
								defaultInterpolatedStringHandler.AppendLiteral(" (AU/EU/RU/cells) beneath it. Click Delete again to confirm.");
								text = defaultInterpolatedStringHandler.ToStringAndClear();
							}
							this._message = text;
							this._busy = false;
							num = 1;
						}
						else
						{
							IQueryable<Device> devices = db.Devices;
							ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
							await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Field(Expression.Constant(this, typeof(DeviceDetail)), fieldof(DeviceDetail._device)), methodof(Device.get_Id()))), new ParameterExpression[] { parameterExpression })).ExecuteDeleteAsync<Device>(this._cts.Token);
							this.Nav.NavigateTo("devices/" + this._device.Category.Key, false, false);
						}
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
					if (num != 1)
					{
						obj = null;
						db = null;
					}
				}
				catch
				{
					this._busy = false;
					throw;
				}
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00028378 File Offset: 0x00026578
		private static async Task<int> CountDescendantsAsync(ApplicationDbContext db, int rootId, CancellationToken ct)
		{
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			Dictionary<int, List<int>> dictionary = (from l in await devices.Select(Expression.Lambda(Expression.New(methodof(<>f__AnonymousType18<int, int?>..ctor(<Id>j__TPar, <ParentDeviceId>j__TPar)), new Expression[]
				{
					Expression.Property(parameterExpression, methodof(Device.get_Id())),
					Expression.Property(parameterExpression, methodof(Device.get_ParentDeviceId()))
				}, new MemberInfo[]
				{
					methodof(<>f__AnonymousType18<int, int?>.get_Id()),
					methodof(<>f__AnonymousType18<int, int?>.get_ParentDeviceId())
				}), new ParameterExpression[] { parameterExpression })).ToListAsync(ct)
				where l.ParentDeviceId != null
				group l by l.ParentDeviceId.Value).ToDictionary(g => g.Key, g => g.Select(x => x.Id).ToList<int>());
			int num = 0;
			Stack<int> stack = new Stack<int>();
			stack.Push(rootId);
			while (stack.Count > 0)
			{
				List<int> list;
				if (dictionary.TryGetValue(stack.Pop(), out list))
				{
					foreach (int num2 in list)
					{
						num++;
						stack.Push(num2);
					}
				}
			}
			return num;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000283CC File Offset: 0x000265CC
		private static async Task AddUeCountsAsync(ApplicationDbContext db, int coreId, DeviceSnapshot snap)
		{
			DeviceDetail.<>c__DisplayClass49_0 CS$<>8__locals1 = new DeviceDetail.<>c__DisplayClass49_0();
			CS$<>8__locals1.snap = snap;
			CS$<>8__locals1.coreId = coreId;
			int? num = CS$<>8__locals1.<AddUeCountsAsync>g__Num|0("Registered UEs");
			int? num2 = CS$<>8__locals1.<AddUeCountsAsync>g__Num|0("Active UEs");
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				if (num2 != null)
				{
					int valueOrDefault2 = num2.GetValueOrDefault();
					int num3 = CS$<>8__locals1.snap.Metrics.FindIndex((Metric m) => m.Label == "Active UEs");
					CS$<>8__locals1.snap.Metrics.Insert((num3 < 0) ? CS$<>8__locals1.snap.Metrics.Count : (num3 + 1), new Metric("Idle UEs", Math.Max(0, valueOrDefault - valueOrDefault2).ToString(), null, "System"));
				}
			}
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			int num4 = await devices.CountAsync<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.AndAlso(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_ParentDeviceId())), Expression.Convert(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DeviceDetail.<>c__DisplayClass49_0)), fieldof(DeviceDetail.<>c__DisplayClass49_0.coreId)), typeof(int?))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("ue", typeof(string)))), Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_LastReachable())), Expression.Convert(Expression.Constant(true, typeof(bool)), typeof(bool?)))), new ParameterExpression[] { parameterExpression }), default(CancellationToken));
			int num5 = CS$<>8__locals1.snap.Metrics.FindIndex((Metric m) => m.Label == "Idle UEs");
			CS$<>8__locals1.snap.Metrics.Insert((num5 < 0) ? CS$<>8__locals1.snap.Metrics.Count : (num5 + 1), new Metric("Disconnected UEs", num4.ToString(), null, "System"));
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00028420 File Offset: 0x00026620
		private async Task PowerAsync(bool on)
		{
			this._powerConfirmOff = false;
			this._powerBusy = true;
			try
			{
				OperationResult operationResult = await this.Runner.RunPrruPowerAsync(this.Id, on, this._powerChannel, CancellationToken.None);
				this._powerResult = operationResult;
			}
			finally
			{
				this._powerBusy = false;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002846C File Offset: 0x0002666C
		private async Task RefreshLoopAsync()
		{
			try
			{
				using (PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5L)))
				{
					for (;;)
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter = timer.WaitForNextTickAsync(this._cts.Token).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							await valueTaskAwaiter;
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						}
						if (!valueTaskAwaiter.GetResult())
						{
							break;
						}
						await this.LoadAsync();
						await base.InvokeAsync(new Action(base.StateHasChanged));
					}
				}
				PeriodicTimer timer = null;
			}
			catch (OperationCanceledException)
			{
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000284AF File Offset: 0x000266AF
		public void Dispose()
		{
			this._cts.Cancel();
			this._cts.Dispose();
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000284C7 File Offset: 0x000266C7
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x000284CF File Offset: 0x000266CF
		[Inject]
		private OperationRunner Runner { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000284D8 File Offset: 0x000266D8
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x000284E0 File Offset: 0x000266E0
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000284E9 File Offset: 0x000266E9
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x000284F1 File Offset: 0x000266F1
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x060006F7 RID: 1783 RVA: 0x00028558 File Offset: 0x00026758
		// Note: this type is marked as 'beforefieldinit'.
		static DeviceDetail()
		{
			Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
			dictionary["bbu"] = new HashSet<string>(StringComparer.Ordinal) { "Resources", "System", "CPU cores", "Performance" };
			dictionary["eu"] = new HashSet<string>(StringComparer.Ordinal) { "Port alarms", "System" };
			dictionary["mobile"] = new HashSet<string>(StringComparer.Ordinal) { "Resources", "Radio", "Throughput", "System" };
			dictionary["ru"] = new HashSet<string>(StringComparer.Ordinal) { "Metrics" };
			DeviceDetail.HiddenGroups = dictionary;
			DeviceDetail.NoConnectedUnits = new HashSet<string>(StringComparer.Ordinal) { "bbu", "mobile" };
			DeviceDetail.MobileNetworkOrder = new string[] { "MCC", "MNC", "Network name", "PLMN ID" };
			DeviceDetail.SliceLabels = new string[] { "Network slices", "SNSSAI" };
		}

		// Token: 0x040002E7 RID: 743
		[Nullable(2)]
		private Device _device;

		// Token: 0x040002E8 RID: 744
		[Nullable(2)]
		private DeviceSnapshot _snapshot;

		// Token: 0x040002E9 RID: 745
		private List<Trend> _trends = new List<Trend>();

		// Token: 0x040002EA RID: 746
		[TupleElementNames(new string[] { "Label", "Value" })]
		[Nullable(new byte[] { 1, 0, 1, 1 })]
		private List<ValueTuple<string, string>> _mobileNetwork = new List<ValueTuple<string, string>>();

		// Token: 0x040002EB RID: 747
		private List<DeviceDetail.SliceRow> _slices = new List<DeviceDetail.SliceRow>();

		// Token: 0x040002EC RID: 748
		private List<RhubPort> _hubPorts = new List<RhubPort>();

		// Token: 0x040002ED RID: 749
		private const string MobileNetworkGroup = "Mobile network";

		// Token: 0x040002EE RID: 750
		private const string SystemGroupName = "System";

		// Token: 0x040002EF RID: 751
		private static readonly Dictionary<string, HashSet<string>> HiddenGroups;

		// Token: 0x040002F0 RID: 752
		private static readonly HashSet<string> NoConnectedUnits;

		// Token: 0x040002F1 RID: 753
		private static readonly string[] MobileNetworkOrder;

		// Token: 0x040002F2 RID: 754
		private static readonly string[] SliceLabels;

		// Token: 0x040002F3 RID: 755
		private int? _baseStationId;

		// Token: 0x040002F4 RID: 756
		private bool _notFound;

		// Token: 0x040002F5 RID: 757
		private bool _busy;

		// Token: 0x040002F6 RID: 758
		private bool _isAdmin;

		// Token: 0x040002F7 RID: 759
		private bool _confirmDelete;

		// Token: 0x040002F8 RID: 760
		[Nullable(2)]
		private string _message;

		// Token: 0x040002F9 RID: 761
		private Dictionary<string, Device> _deviceByKey = new Dictionary<string, Device>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002FA RID: 762
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040002FB RID: 763
		private bool _powerBusy;

		// Token: 0x040002FC RID: 764
		private bool _powerConfirmOff;

		// Token: 0x040002FD RID: 765
		private int _powerChannel;

		// Token: 0x040002FE RID: 766
		[Nullable(2)]
		private OperationRunner.PrruPowerTarget _powerTarget;

		// Token: 0x040002FF RID: 767
		[Nullable(2)]
		private OperationResult _powerResult;

		// Token: 0x020001AA RID: 426
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class SliceRow : IEquatable<DeviceDetail.SliceRow>
		{
			// Token: 0x06000D34 RID: 3380 RVA: 0x000576FE File Offset: 0x000558FE
			public SliceRow([Nullable(1)] string Raw, int? Sst, string SstName, string Sd, string ReportedBy)
			{
				this.Raw = Raw;
				this.Sst = Sst;
				this.SstName = SstName;
				this.Sd = Sd;
				this.ReportedBy = ReportedBy;
				base..ctor();
			}

			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0005772B File Offset: 0x0005592B
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(DeviceDetail.SliceRow);
				}
			}

			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000D36 RID: 3382 RVA: 0x00057737 File Offset: 0x00055937
			// (set) Token: 0x06000D37 RID: 3383 RVA: 0x0005773F File Offset: 0x0005593F
			[Nullable(1)]
			public string Raw
			{
				[NullableContext(1)]
				get;
				[NullableContext(1)]
				set;
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00057748 File Offset: 0x00055948
			// (set) Token: 0x06000D39 RID: 3385 RVA: 0x00057750 File Offset: 0x00055950
			public int? Sst { get; set; }

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00057759 File Offset: 0x00055959
			// (set) Token: 0x06000D3B RID: 3387 RVA: 0x00057761 File Offset: 0x00055961
			public string SstName { get; set; }

			// Token: 0x170002D2 RID: 722
			// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0005776A File Offset: 0x0005596A
			// (set) Token: 0x06000D3D RID: 3389 RVA: 0x00057772 File Offset: 0x00055972
			public string Sd { get; set; }

			// Token: 0x170002D3 RID: 723
			// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0005777B File Offset: 0x0005597B
			// (set) Token: 0x06000D3F RID: 3391 RVA: 0x00057783 File Offset: 0x00055983
			public string ReportedBy { get; set; }

			// Token: 0x06000D40 RID: 3392 RVA: 0x0005778C File Offset: 0x0005598C
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SliceRow");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000D41 RID: 3393 RVA: 0x000577D8 File Offset: 0x000559D8
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Raw = ");
				builder.Append(this.Raw);
				builder.Append(", Sst = ");
				builder.Append(this.Sst.ToString());
				builder.Append(", SstName = ");
				builder.Append(this.SstName);
				builder.Append(", Sd = ");
				builder.Append(this.Sd);
				builder.Append(", ReportedBy = ");
				builder.Append(this.ReportedBy);
				return true;
			}

			// Token: 0x06000D42 RID: 3394 RVA: 0x00057876 File Offset: 0x00055A76
			[CompilerGenerated]
			public static bool operator !=(DeviceDetail.SliceRow left, DeviceDetail.SliceRow right)
			{
				return !(left == right);
			}

			// Token: 0x06000D43 RID: 3395 RVA: 0x00057882 File Offset: 0x00055A82
			[CompilerGenerated]
			public static bool operator ==(DeviceDetail.SliceRow left, DeviceDetail.SliceRow right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000D44 RID: 3396 RVA: 0x00057898 File Offset: 0x00055A98
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Raw>k__BackingField)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.<Sst>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SstName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Sd>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ReportedBy>k__BackingField);
			}

			// Token: 0x06000D45 RID: 3397 RVA: 0x00057928 File Offset: 0x00055B28
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as DeviceDetail.SliceRow);
			}

			// Token: 0x06000D46 RID: 3398 RVA: 0x00057938 File Offset: 0x00055B38
			[CompilerGenerated]
			public bool Equals(DeviceDetail.SliceRow other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Raw>k__BackingField, other.<Raw>k__BackingField) && EqualityComparer<int?>.Default.Equals(this.<Sst>k__BackingField, other.<Sst>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SstName>k__BackingField, other.<SstName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Sd>k__BackingField, other.<Sd>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ReportedBy>k__BackingField, other.<ReportedBy>k__BackingField));
			}

			// Token: 0x06000D48 RID: 3400 RVA: 0x000579E8 File Offset: 0x00055BE8
			[CompilerGenerated]
			private SliceRow([Nullable(1)] DeviceDetail.SliceRow original)
			{
				this.Raw = original.<Raw>k__BackingField;
				this.Sst = original.<Sst>k__BackingField;
				this.SstName = original.<SstName>k__BackingField;
				this.Sd = original.<Sd>k__BackingField;
				this.ReportedBy = original.<ReportedBy>k__BackingField;
			}

			// Token: 0x06000D49 RID: 3401 RVA: 0x00057A37 File Offset: 0x00055C37
			[CompilerGenerated]
			public void Deconstruct([Nullable(1)] out string Raw, out int? Sst, out string SstName, out string Sd, out string ReportedBy)
			{
				Raw = this.Raw;
				Sst = this.Sst;
				SstName = this.SstName;
				Sd = this.Sd;
				ReportedBy = this.ReportedBy;
			}
		}

		// Token: 0x020001AB RID: 427
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00057A67 File Offset: 0x00055C67
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return RenderMode.InteractiveServer;
				}
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00057A6E File Offset: 0x00055C6E
			public override IComponentRenderMode Mode
			{
				get
				{
					return DeviceDetail.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
