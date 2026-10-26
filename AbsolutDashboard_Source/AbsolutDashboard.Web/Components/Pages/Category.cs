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
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000A7 RID: 167
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Route("/devices/{key}")]
	[Category.__PrivateComponentRenderModeAttribute]
	public class Category : ComponentBase, IDisposable
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x00023870 File Offset: 0x00021A70
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				int num14 = 2;
				DeviceCategory category = this._category;
				__builder2.AddContent(num14, ((category != null) ? category.ShortName : null) ?? "Devices");
				__builder2.AddMarkupContent(3, " — Absolut");
			}));
			__builder.CloseComponent();
			if (this._notFound)
			{
				__builder.AddMarkupContent(4, "<h1 class=\"text-2xl font-semibold text-cocoa-900\">Unknown category</h1>\n    ");
				__builder.OpenElement(5, "p");
				__builder.AddAttribute(6, "class", "mt-2 text-cocoa-700");
				__builder.AddContent(7, "No category named \"");
				__builder.AddContent(8, this.Key);
				__builder.AddContent(9, "\".");
				__builder.CloseElement();
				__builder.AddMarkupContent(10, "\n    ");
				__builder.AddMarkupContent(11, "<p class=\"mt-4\"><a class=\"btn-back\" href>&larr; Dashboard</a></p>");
				return;
			}
			if (this._category == null)
			{
				__builder.AddMarkupContent(12, "<p class=\"text-cocoa-700\">Loading…</p>");
				return;
			}
			__builder.OpenElement(13, "header");
			__builder.AddAttribute(14, "class", "mb-6 flex flex-wrap items-start justify-between gap-3");
			__builder.OpenElement(15, "div");
			__builder.OpenElement(16, "h1");
			__builder.AddAttribute(17, "class", "text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(18, this._category.Name);
			__builder.CloseElement();
			__builder.AddMarkupContent(19, "\n            ");
			__builder.OpenElement(20, "p");
			__builder.AddAttribute(21, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(22, this._category.Description);
			__builder.CloseElement();
			__builder.CloseElement();
			if (this._category.AllowsManualAdd)
			{
				__builder.OpenComponent<AuthorizeView>(23);
				__builder.AddComponentParameter(24, "Roles", "Admin");
				__builder.AddAttribute(25, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
				{
					__builder2.OpenElement(26, "a");
					__builder2.AddAttribute(27, "class", "btn-primary !px-4 !py-2 text-sm");
					__builder2.AddAttribute(28, "href", "devices/" + this.Key + "/add");
					__builder2.AddContent(29, "+ Add device");
					__builder2.CloseElement();
				}));
				__builder.CloseComponent();
			}
			__builder.CloseElement();
			if (this._devices.Count == 0)
			{
				__builder.OpenElement(30, "div");
				__builder.AddAttribute(31, "class", "card p-8 text-center text-cocoa-700");
				if (this._category.AllowsManualAdd)
				{
					__builder.AddMarkupContent(32, "No devices in this category yet.\n                ");
					__builder.OpenComponent<AuthorizeView>(33);
					__builder.AddComponentParameter(34, "Roles", "Admin");
					__builder.AddAttribute(35, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
					{
						__builder2.AddMarkupContent(36, "<span> Use <b>Add device</b> to create one.</span>");
					}));
					__builder.CloseComponent();
				}
				else
				{
					__builder.AddMarkupContent(37, "No devices discovered yet — units appear automatically once the BBU reports them.");
				}
				__builder.CloseElement();
				return;
			}
			if (this._category.Key == "ue")
			{
				if (this.OfflineUeCount > 0)
				{
					__builder.OpenElement(38, "label");
					__builder.AddAttribute(39, "class", "mb-2 flex items-center gap-2 text-xs text-cocoa-700");
					__builder.OpenElement(40, "input");
					__builder.AddAttribute(41, "type", "checkbox");
					__builder.AddAttribute(42, "checked", BindConverter.FormatValue(this._showOfflineUes, null));
					__builder.AddAttribute<ChangeEventArgs>(43, "onchange", EventCallback.Factory.CreateBinder(this, delegate(bool __value)
					{
						this._showOfflineUes = __value;
					}, this._showOfflineUes, null));
					__builder.SetUpdatesAttributeName("checked");
					__builder.CloseElement();
					__builder.AddMarkupContent(44, "\n                Show ");
					__builder.AddContent(45, this.OfflineUeCount);
					__builder.AddContent(46, " offline handset");
					__builder.AddContent(47, (this.OfflineUeCount == 1) ? "" : "s");
					__builder.CloseElement();
				}
				__builder.OpenElement(48, "div");
				__builder.AddAttribute(49, "class", "card overflow-x-auto");
				__builder.OpenElement(50, "table");
				__builder.AddAttribute(51, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(52, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"px-4 py-3\">Name</th>\n                        <th class=\"hidden px-4 py-3 sm:table-cell\">MSIN</th>\n                        <th class=\"hidden px-4 py-3 md:table-cell\">IMSI</th>\n                        <th class=\"hidden px-4 py-3 lg:table-cell\">Description</th>\n                        <th class=\"hidden px-4 py-3 md:table-cell\">Uptime</th>\n                        <th class=\"px-4 py-3\">DL</th>\n                        <th class=\"px-4 py-3\">UL</th>\n                        <th class=\"hidden px-4 py-3 lg:table-cell\">Base station</th>\n                        <th class=\"hidden px-4 py-3 lg:table-cell\">Latency</th>\n                        <th class=\"hidden px-4 py-3 md:table-cell\">5G core</th>\n                        <th class=\"px-4 py-3\">Status</th>\n                        <th class=\"px-4 py-3\"></th></tr></thead>\n                ");
				__builder.OpenElement(53, "tbody");
				foreach (Device device in this.UeRows)
				{
					ValueTuple<string, string> valueTuple = DeviceStatusView.UeBadge(device.LastReachable, this.Metric(device, "State"));
					string text = this.Metric(device, "IP");
					__builder.OpenElement(54, "tr");
					__builder.AddAttribute(55, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(56, "td");
					__builder.AddAttribute(57, "class", "px-4 py-3 font-medium text-cocoa-900");
					__builder.AddContent(58, device.Name);
					if (!device.Enabled)
					{
						__builder.AddMarkupContent(59, "<span class=\"ml-2 pill pill-unknown\">paused</span>");
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(60, "\n                            ");
					__builder.OpenElement(61, "td");
					__builder.AddAttribute(62, "class", "hidden px-4 py-3 text-cocoa-700 sm:table-cell");
					__builder.AddContent(63, this.Metric(device, "MSIN") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(64, "\n                            ");
					__builder.OpenElement(65, "td");
					__builder.AddAttribute(66, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					__builder.AddContent(67, this.Metric(device, "IMSI") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(68, "\n                            ");
					__builder.OpenElement(69, "td");
					__builder.AddAttribute(70, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(71, string.IsNullOrWhiteSpace(device.Notes) ? "—" : device.Notes);
					__builder.CloseElement();
					__builder.AddMarkupContent(72, "\n                            ");
					__builder.OpenElement(73, "td");
					__builder.AddAttribute(74, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					__builder.AddContent(75, this.Metric(device, "Uptime") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(76, "\n                            ");
					__builder.OpenElement(77, "td");
					__builder.AddAttribute(78, "class", "px-4 py-3 text-cocoa-700");
					__builder.AddContent(79, this.Metric(device, "DL") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(80, "\n                            ");
					__builder.OpenElement(81, "td");
					__builder.AddAttribute(82, "class", "px-4 py-3 text-cocoa-700");
					__builder.AddContent(83, this.Metric(device, "UL") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(84, "\n                            ");
					__builder.OpenElement(85, "td");
					__builder.AddAttribute(86, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					string text2 = this.Metric(device, "Base station");
					if (text2 == null)
					{
						__builder.AddMarkupContent(87, "<span>—</span>");
					}
					else
					{
						int? num = this.BaseStationId(device);
						if (num != null)
						{
							int valueOrDefault = num.GetValueOrDefault();
							__builder.OpenElement(88, "a");
							__builder.AddAttribute(89, "class", "text-blush-700 hover:underline");
							int num2 = 90;
							string text3 = "href";
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
							defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
							defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault);
							__builder.AddAttribute(num2, text3, defaultInterpolatedStringHandler.ToStringAndClear());
							__builder.AddAttribute(91, "title", "Open this base station");
							__builder.AddContent(92, text2);
							__builder.CloseElement();
						}
						else
						{
							__builder.OpenElement(93, "span");
							__builder.AddContent(94, text2);
							__builder.CloseElement();
						}
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(95, "\n                            ");
					__builder.OpenElement(96, "td");
					__builder.AddAttribute(97, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					string text4 = this.Metric(device, "Latency");
					if (text4 == null)
					{
						__builder.AddMarkupContent(98, "<span>—</span>");
					}
					else if (text4 == "unreachable")
					{
						__builder.AddMarkupContent(99, "<span class=\"pill pill-down\" title=\"The 5G core got no reply from this UE\">unreachable</span>");
					}
					else
					{
						__builder.OpenElement(100, "span");
						__builder.AddContent(101, text4);
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(102, "\n                            ");
					__builder.OpenElement(103, "td");
					__builder.AddAttribute(104, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					if (device.ParentDeviceId != null)
					{
						__builder.OpenElement(105, "a");
						__builder.AddAttribute(106, "class", "text-blush-700 hover:underline");
						int num3 = 107;
						string text5 = "href";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("devices/detail/");
						defaultInterpolatedStringHandler2.AppendFormatted<int?>(device.ParentDeviceId);
						__builder.AddAttribute(num3, text5, defaultInterpolatedStringHandler2.ToStringAndClear());
						int num4 = 108;
						Device parentDevice = device.ParentDevice;
						__builder.AddContent(num4, ((parentDevice != null) ? parentDevice.Name : null) ?? "5G core");
						__builder.CloseElement();
					}
					else
					{
						__builder.AddMarkupContent(109, "<span>—</span>");
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(110, "\n                            ");
					__builder.OpenElement(111, "td");
					__builder.AddAttribute(112, "class", "px-4 py-3");
					__builder.OpenElement(113, "span");
					__builder.AddAttribute(114, "class", valueTuple.Item1);
					__builder.AddAttribute(115, "title", device.LastError);
					__builder.AddContent(116, valueTuple.Item2);
					__builder.CloseElement();
					if (text != null)
					{
						__builder.OpenElement(117, "span");
						__builder.AddAttribute(118, "class", "ml-2 text-xs text-cocoa-700");
						__builder.AddContent(119, text);
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(120, "\n                            ");
					__builder.OpenElement(121, "td");
					__builder.AddAttribute(122, "class", "px-4 py-3");
					__builder.OpenComponent<DeviceRowActions>(123);
					__builder.AddComponentParameter(124, "Device", RuntimeHelpers.TypeCheck<Device>(device));
					__builder.AddComponentParameter(125, "OnChanged", RuntimeHelpers.TypeCheck<EventCallback>(EventCallback.Factory.Create(this, new Func<Task>(this.LoadAsync))));
					__builder.CloseComponent();
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				return;
			}
			string text6 = this._category.Key;
			bool flag = text6 == "bbu" || text6 == "ims" || text6 == "mobile";
			bool flag2 = flag;
			text6 = this._category.Key;
			flag = text6 == "ims" || text6 == "mobile";
			bool flag3 = flag;
			text6 = this._category.Key;
			flag = text6 == "bbu" || text6 == "ims" || text6 == "mobile" || text6 == "switch";
			bool flag4 = flag;
			__builder.OpenElement(126, "div");
			__builder.AddAttribute(127, "class", "card overflow-x-auto");
			__builder.OpenElement(128, "table");
			__builder.AddAttribute(129, "class", "w-full text-left text-sm");
			__builder.OpenElement(130, "thead");
			__builder.AddAttribute(131, "class", "border-b border-cream-200 text-cocoa-700");
			__builder.OpenElement(132, "tr");
			__builder.AddMarkupContent(133, "<th class=\"px-4 py-3\">Name</th>\n                        ");
			__builder.AddMarkupContent(134, "<th class=\"hidden px-4 py-3 sm:table-cell\">Type</th>\n                        ");
			__builder.AddMarkupContent(135, "<th class=\"hidden px-4 py-3 md:table-cell\">Address</th>\n                        ");
			__builder.AddMarkupContent(136, "<th class=\"hidden px-4 py-3 lg:table-cell\">Description</th>");
			if (flag2)
			{
				__builder.AddMarkupContent(137, "<th class=\"hidden px-4 py-3 lg:table-cell\">CPU</th>");
			}
			if (flag3)
			{
				__builder.AddMarkupContent(138, "<th class=\"hidden px-4 py-3 lg:table-cell\">RAM</th>");
			}
			if (flag4)
			{
				__builder.AddMarkupContent(139, "<th class=\"hidden px-4 py-3 lg:table-cell\">Uptime</th>");
			}
			if (this._category.Key == "cell")
			{
				__builder.AddMarkupContent(140, "<th class=\"hidden px-4 py-3 lg:table-cell\">DL BW</th>\n                            ");
				__builder.AddMarkupContent(141, "<th class=\"hidden px-4 py-3 lg:table-cell\">UL BW</th>\n                            ");
				__builder.AddMarkupContent(142, "<th class=\"hidden px-4 py-3 lg:table-cell\">DL QAM</th>\n                            ");
				__builder.AddMarkupContent(143, "<th class=\"hidden px-4 py-3 lg:table-cell\">UL QAM</th>");
			}
			if (this._category.Key == "eu")
			{
				__builder.AddMarkupContent(144, "<th class=\"hidden px-4 py-3 lg:table-cell\">Ports</th>");
			}
			if (this._category.Key == "ru")
			{
				__builder.AddMarkupContent(145, "<th class=\"hidden px-4 py-3 lg:table-cell\">X</th>\n                            ");
				__builder.AddMarkupContent(146, "<th class=\"hidden px-4 py-3 lg:table-cell\">Y</th>\n                            ");
				__builder.AddMarkupContent(147, "<th class=\"hidden px-4 py-3 lg:table-cell\">Z</th>");
			}
			__builder.AddMarkupContent(148, "<th class=\"px-4 py-3\">Status</th>\n                        ");
			__builder.AddMarkupContent(149, "<th class=\"hidden px-4 py-3 md:table-cell\">Last polled</th>\n                        <th class=\"px-4 py-3\"></th>");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(150, "\n                ");
			__builder.OpenElement(151, "tbody");
			foreach (Device device2 in this._devices)
			{
				ValueTuple<string, string> valueTuple2 = DeviceStatusView.Pill(device2.LastReachable);
				__builder.OpenElement(152, "tr");
				__builder.AddAttribute(153, "class", "border-b border-cream-100 last:border-0");
				__builder.OpenElement(154, "td");
				__builder.AddAttribute(155, "class", "px-4 py-3 font-medium text-cocoa-900");
				__builder.AddContent(156, device2.Name);
				if (!device2.Enabled)
				{
					__builder.AddMarkupContent(157, "<span class=\"ml-2 pill pill-unknown\">paused</span>");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(158, "\n                            ");
				__builder.OpenElement(159, "td");
				__builder.AddAttribute(160, "class", "hidden px-4 py-3 text-cocoa-700 sm:table-cell");
				int num5 = 161;
				DeviceTemplate template = device2.Template;
				__builder.AddContent(num5, (template != null) ? template.Name : null);
				__builder.CloseElement();
				__builder.AddMarkupContent(162, "\n                            ");
				__builder.OpenElement(163, "td");
				__builder.AddAttribute(164, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
				if (device2.CollectionMethod == CollectionMethod.ViaParent)
				{
					if (device2.ParentDeviceId != null)
					{
						__builder.OpenElement(165, "a");
						__builder.AddAttribute(166, "class", "text-blush-700 hover:underline");
						int num6 = 167;
						string text7 = "href";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(15, 1);
						defaultInterpolatedStringHandler3.AppendLiteral("devices/detail/");
						defaultInterpolatedStringHandler3.AppendFormatted<int?>(device2.ParentDeviceId);
						__builder.AddAttribute(num6, text7, defaultInterpolatedStringHandler3.ToStringAndClear());
						__builder.AddContent(168, "via ");
						int num7 = 169;
						Device parentDevice2 = device2.ParentDevice;
						__builder.AddContent(num7, ((parentDevice2 != null) ? parentDevice2.Name : null) ?? "parent");
						__builder.CloseElement();
					}
					else
					{
						__builder.AddMarkupContent(170, "<span>via —</span>");
					}
				}
				else
				{
					__builder.AddContent(171, device2.IpAddress ?? "—");
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(172, "\n                            ");
				__builder.OpenElement(173, "td");
				__builder.AddAttribute(174, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
				__builder.AddContent(175, string.IsNullOrWhiteSpace(device2.Notes) ? "—" : device2.Notes);
				__builder.CloseElement();
				if (flag2)
				{
					__builder.OpenElement(176, "td");
					__builder.AddAttribute(177, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(178, this.Metric(device2, "CPU load") ?? "—");
					__builder.CloseElement();
				}
				if (flag3)
				{
					__builder.OpenElement(179, "td");
					__builder.AddAttribute(180, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(181, this.Metric(device2, "Memory usage") ?? "—");
					__builder.CloseElement();
				}
				if (flag4)
				{
					__builder.OpenElement(182, "td");
					__builder.AddAttribute(183, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(184, this.Metric(device2, "Uptime") ?? "—");
					__builder.CloseElement();
				}
				if (this._category.Key == "cell")
				{
					__builder.OpenElement(185, "td");
					__builder.AddAttribute(186, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					int num8 = 187;
					string text8 = this.Metric(device2, "DL bandwidth");
					__builder.AddContent(num8, (text8 != null) ? (text8 + " MHz") : "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(188, "\n                                ");
					__builder.OpenElement(189, "td");
					__builder.AddAttribute(190, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					int num9 = 191;
					string text9 = this.Metric(device2, "UL bandwidth");
					__builder.AddContent(num9, (text9 != null) ? (text9 + " MHz") : "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(192, "\n                                ");
					__builder.OpenElement(193, "td");
					__builder.AddAttribute(194, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(195, this.Metric(device2, "DL QAM") ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(196, "\n                                ");
					__builder.OpenElement(197, "td");
					__builder.AddAttribute(198, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(199, this.Metric(device2, "UL QAM") ?? "—");
					__builder.CloseElement();
				}
				if (this._category.Key == "eu")
				{
					__builder.OpenElement(200, "td");
					__builder.AddAttribute(201, "class", "hidden px-4 py-3 lg:table-cell");
					__builder.OpenComponent<HubPorts>(202);
					__builder.AddComponentParameter(203, "Ports", RuntimeHelpers.TypeCheck<List<RhubPort>>(this.HubPorts(device2)));
					__builder.AddComponentParameter(204, "Compact", RuntimeHelpers.TypeCheck<bool>(true));
					__builder.AddComponentParameter(205, "ShowMissingData", RuntimeHelpers.TypeCheck<bool>(true));
					__builder.AddComponentParameter(206, "ShowMissingPower", RuntimeHelpers.TypeCheck<bool>(true));
					__builder.AddComponentParameter(207, "MissingDataText", "—");
					__builder.AddComponentParameter(208, "MissingPowerText", RuntimeHelpers.TypeCheck<string>(string.IsNullOrWhiteSpace(device2.SerialPort) ? "no console" : "not read"));
					__builder.CloseComponent();
					__builder.CloseElement();
				}
				if (this._category.Key == "ru")
				{
					__builder.OpenElement(209, "td");
					__builder.AddAttribute(210, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					int num10 = 211;
					double? num11 = device2.CoordinateX;
					__builder.AddContent(num10, ((num11 != null) ? num11.GetValueOrDefault().ToString() : null) ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(212, "\n                                ");
					__builder.OpenElement(213, "td");
					__builder.AddAttribute(214, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					int num12 = 215;
					num11 = device2.CoordinateY;
					__builder.AddContent(num12, ((num11 != null) ? num11.GetValueOrDefault().ToString() : null) ?? "—");
					__builder.CloseElement();
					__builder.AddMarkupContent(216, "\n                                ");
					__builder.OpenElement(217, "td");
					__builder.AddAttribute(218, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					int num13 = 219;
					num11 = device2.CoordinateZ;
					__builder.AddContent(num13, ((num11 != null) ? num11.GetValueOrDefault().ToString() : null) ?? "—");
					__builder.CloseElement();
				}
				__builder.OpenElement(220, "td");
				__builder.AddAttribute(221, "class", "px-4 py-3");
				__builder.OpenElement(222, "span");
				__builder.AddAttribute(223, "class", valueTuple2.Item1);
				__builder.AddAttribute(224, "title", device2.LastError);
				__builder.AddContent(225, valueTuple2.Item2);
				__builder.CloseElement();
				string text10 = this.Metric(device2, "State");
				if (this._category.Key == "cell" && text10 != null)
				{
					__builder.OpenElement(226, "span");
					__builder.AddAttribute(227, "class", "ml-2 text-xs text-cocoa-700");
					__builder.AddContent(228, text10);
					__builder.CloseElement();
				}
				else if (DeviceStatusView.IsAlarmState(text10))
				{
					__builder.OpenElement(229, "span");
					__builder.AddAttribute(230, "class", "ml-2 pill pill-warn");
					__builder.AddAttribute(231, "title", "Flagged in the BBU topology; see the BBU's alarm list");
					__builder.AddContent(232, text10);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(233, "\n                            ");
				__builder.OpenElement(234, "td");
				__builder.AddAttribute(235, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
				__builder.AddContent(236, DeviceStatusView.Ago(device2.LastPolledAt));
				__builder.CloseElement();
				__builder.AddMarkupContent(237, "\n                            ");
				__builder.OpenElement(238, "td");
				__builder.AddAttribute(239, "class", "px-4 py-3");
				__builder.OpenComponent<DeviceRowActions>(240);
				__builder.AddComponentParameter(241, "Device", RuntimeHelpers.TypeCheck<Device>(device2));
				__builder.AddComponentParameter(242, "OnChanged", RuntimeHelpers.TypeCheck<EventCallback>(EventCallback.Factory.Create(this, new Func<Task>(this.LoadAsync))));
				__builder.CloseComponent();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00024F54 File Offset: 0x00023154
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00024F5C File Offset: 0x0002315C
		[Parameter]
		public string Key { get; set; } = "";

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00024F65 File Offset: 0x00023165
		private int OfflineUeCount
		{
			get
			{
				List<Device> devices = this._devices;
				if (devices == null)
				{
					return 0;
				}
				return devices.Count<Device>((Device d) => !d.LastReachable.GetValueOrDefault());
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00024F98 File Offset: 0x00023198
		private IEnumerable<Device> UeRows
		{
			get
			{
				if (!this._showOfflineUes)
				{
					return this._devices.Where<Device>((Device d) => d.LastReachable.GetValueOrDefault());
				}
				return this._devices;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00024FE0 File Offset: 0x000231E0
		private int? BaseStationId(Device d)
		{
			DeviceSnapshot deviceSnapshot;
			if (!this._snapshots.TryGetValue(d.Id, out deviceSnapshot))
			{
				return null;
			}
			return BaseStationLookup.Resolve(this._bbuByGnb, deviceSnapshot);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00025018 File Offset: 0x00023218
		private List<RhubPort> HubPorts(Device d)
		{
			DeviceSnapshot deviceSnapshot;
			if (!this._snapshots.TryGetValue(d.Id, out deviceSnapshot) || deviceSnapshot == null)
			{
				return new List<RhubPort>();
			}
			return deviceSnapshot.HubPorts;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002504C File Offset: 0x0002324C
		[return: Nullable(2)]
		private string Metric(Device d, string label)
		{
			DeviceSnapshot deviceSnapshot;
			if (!this._snapshots.TryGetValue(d.Id, out deviceSnapshot) || deviceSnapshot == null)
			{
				return null;
			}
			Metric metric = deviceSnapshot.Metrics.FirstOrDefault<Metric>((Metric x) => x.Label == label);
			if (metric == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(metric.Unit))
			{
				return metric.Value + " " + metric.Unit;
			}
			return metric.Value;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000250C8 File Offset: 0x000232C8
		protected override async Task OnParametersSetAsync()
		{
			await this.LoadAsync();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002510B File Offset: 0x0002330B
		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender && base.RendererInfo.IsInteractive)
			{
				this.RefreshLoopAsync();
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00025124 File Offset: 0x00023324
		private async Task LoadAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<DeviceCategory> deviceCategories = db.DeviceCategories;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(DeviceCategory), "c");
				this._category = await deviceCategories.FirstOrDefaultAsync<DeviceCategory>(Expression.Lambda<Func<DeviceCategory, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(DeviceCategory.get_Key())), Expression.Property(Expression.Constant(this, typeof(Category)), methodof(Category.get_Key()))), new ParameterExpression[] { parameterExpression }), this._cts.Token);
				if (this._category == null || this._category.IsHidden)
				{
					this._category = null;
					this._notFound = true;
				}
				else
				{
					this._notFound = false;
					IQueryable<Device> devices = db.Devices;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable = devices.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable2 = queryable.Include<Device, Device>(Expression.Lambda<Func<Device, Device>>(Expression.Property(parameterExpression, methodof(Device.get_ParentDevice())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable3 = queryable2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_CategoryId())), Expression.Property(Expression.Field(Expression.Constant(this, typeof(Category)), fieldof(Category._category)), methodof(DeviceCategory.get_Id()))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					this._devices = await queryable3.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(this._cts.Token);
					this._snapshots = this._devices.ToDictionary<Device, int, DeviceSnapshot>((Device d) => d.Id, (Device d) => SnapshotSerializer.Deserialize(d.LastSnapshotJson));
					this._bbuByGnb = ((!(this._category.Key == "ue")) ? new Dictionary<string, int>() : (await BaseStationLookup.ByGnbIdAsync(db, this._cts.Token)));
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

		// Token: 0x060006CB RID: 1739 RVA: 0x00025168 File Offset: 0x00023368
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

		// Token: 0x060006CC RID: 1740 RVA: 0x000251AB File Offset: 0x000233AB
		public void Dispose()
		{
			this._cts.Cancel();
			this._cts.Dispose();
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x000251C3 File Offset: 0x000233C3
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x000251CB File Offset: 0x000233CB
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x040002DD RID: 733
		[Nullable(2)]
		private DeviceCategory _category;

		// Token: 0x040002DE RID: 734
		[Nullable(new byte[] { 2, 1 })]
		private List<Device> _devices;

		// Token: 0x040002DF RID: 735
		private bool _showOfflineUes;

		// Token: 0x040002E0 RID: 736
		private bool _notFound;

		// Token: 0x040002E1 RID: 737
		[Nullable(new byte[] { 1, 2 })]
		private Dictionary<int, DeviceSnapshot> _snapshots = new Dictionary<int, DeviceSnapshot>();

		// Token: 0x040002E2 RID: 738
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040002E3 RID: 739
		private Dictionary<string, int> _bbuByGnb = new Dictionary<string, int>();

		// Token: 0x020001A4 RID: 420
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00056D8C File Offset: 0x00054F8C
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return RenderMode.InteractiveServer;
				}
			}

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00056D93 File Offset: 0x00054F93
			public override IComponentRenderMode Mode
			{
				get
				{
					return Category.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
