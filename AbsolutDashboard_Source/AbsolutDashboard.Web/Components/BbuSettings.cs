using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Bbu;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000092 RID: 146
	[NullableContext(1)]
	[Nullable(0)]
	public class BbuSettings : ComponentBase
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x00019508 File Offset: 0x00017708
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card max-w-2xl p-5");
			__builder.OpenElement(2, "div");
			__builder.AddAttribute(3, "class", "mb-1 flex flex-wrap items-baseline justify-between gap-3");
			__builder.AddMarkupContent(4, "<h2 class=\"text-lg font-semibold text-cocoa-900\">Radio configuration</h2>");
			if (this._bbus.Count > 0)
			{
				__builder.OpenElement(5, "div");
				__builder.AddAttribute(6, "class", "flex flex-wrap items-center gap-3");
				if (this.BbuId == null && this._bbus.Count > 1)
				{
					__builder.OpenElement(7, "label");
					__builder.AddAttribute(8, "class", "flex items-center gap-2 text-sm text-cocoa-700");
					__builder.AddMarkupContent(9, "\n                        Base station\n                        ");
					__builder.OpenElement(10, "select");
					__builder.AddAttribute(11, "class", "input mt-0 w-auto");
					__builder.AddAttribute(12, "value", this._bbuId);
					__builder.AddAttribute<ChangeEventArgs>(13, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, new Action<ChangeEventArgs>(this.OnBbuChanged)));
					foreach (Device device in this._bbus)
					{
						__builder.OpenElement(14, "option");
						__builder.AddAttribute(15, "value", device.Id);
						__builder.AddContent(16, device.Name);
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.CloseElement();
				}
				if (this._cells.Count > 1)
				{
					__builder.OpenElement(17, "label");
					__builder.AddAttribute(18, "class", "flex items-center gap-2 text-sm text-cocoa-700");
					__builder.AddMarkupContent(19, "\n                        Cell\n                        ");
					__builder.OpenElement(20, "select");
					__builder.AddAttribute(21, "class", "input mt-0 w-auto");
					__builder.AddAttribute(22, "value", this._cellIndex);
					__builder.AddAttribute<ChangeEventArgs>(23, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, new Action<ChangeEventArgs>(this.OnCellChanged)));
					foreach (BbuSettings.CellOption cellOption in this._cells)
					{
						__builder.OpenElement(24, "option");
						__builder.AddAttribute(25, "value", cellOption.Index);
						__builder.AddContent(26, "Cell ");
						__builder.AddContent(27, cellOption.Index);
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
			}
			__builder.CloseElement();
			Device selected = this.Selected;
			if (selected != null)
			{
				__builder.OpenElement(28, "p");
				__builder.AddAttribute(29, "class", "mb-4 text-xs text-cocoa-700");
				__builder.AddContent(30, selected.Name);
				int num = 31;
				string text;
				if (this._cells.Count <= 0)
				{
					text = "";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
					defaultInterpolatedStringHandler.AppendLiteral(", Cell ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._cellIndex);
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				__builder.AddContent(num, text);
				__builder.CloseElement();
			}
			if (this._bbus.Count == 0)
			{
				__builder.AddMarkupContent(32, "<div class=\"card p-8 text-center text-cocoa-700\">\n            No base stations yet. Add one before configuring cells.\n        </div>");
			}
			else if (this._cells.Count == 0)
			{
				__builder.OpenElement(33, "p");
				__builder.AddAttribute(34, "class", "text-sm text-cocoa-700");
				int num2 = 35;
				Device selected2 = this.Selected;
				__builder.AddContent(num2, ((selected2 != null) ? selected2.Name : null) ?? "This base station");
				__builder.AddMarkupContent(36, " has reported no cells yet. Settings appear once it\n            has been polled at least once.\n        ");
				__builder.CloseElement();
			}
			else
			{
				Device selected3 = this.Selected;
				if (selected3 != null && selected3.ConfigRestartPending && !this._busy)
				{
					__builder.OpenElement(37, "div");
					__builder.AddAttribute(38, "class", "card mb-4 border-l-4 border-amber-500 bg-amber-50 p-3 text-sm text-amber-800");
					__builder.AddMarkupContent(39, "<b>Changes saved but not yet active.</b>\n                ");
					__builder.AddContent(40, this.Selected.Name);
					__builder.AddMarkupContent(41, " must restart before the new cell parameters take effect.\n                ");
					__builder.OpenElement(42, "div");
					__builder.AddAttribute(43, "class", "mt-2 flex flex-wrap items-center gap-2");
					if (!this._confirmRestart)
					{
						__builder.OpenElement(44, "button");
						__builder.AddAttribute(45, "type", "button");
						__builder.AddAttribute(46, "class", "btn-secondary !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(47, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
						{
							this._confirmRestart = true;
						}));
						__builder.AddAttribute(48, "disabled", this._busy);
						__builder.AddContent(49, "Restart base station");
						__builder.CloseElement();
					}
					else
					{
						__builder.AddMarkupContent(50, "<span>This drops every connected handset. Continue?</span>\n                        ");
						__builder.OpenElement(51, "button");
						__builder.AddAttribute(52, "type", "button");
						__builder.AddAttribute(53, "class", "btn-danger !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(54, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.RestartAsync)));
						__builder.AddAttribute(55, "disabled", this._busy);
						__builder.AddContent(56, "Yes, restart");
						__builder.CloseElement();
						__builder.AddMarkupContent(57, "\n                        ");
						__builder.OpenElement(58, "button");
						__builder.AddAttribute(59, "type", "button");
						__builder.AddAttribute(60, "class", "btn-secondary !px-4 !py-2 text-sm");
						__builder.AddAttribute<MouseEventArgs>(61, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
						{
							this._confirmRestart = false;
						}));
						__builder.AddAttribute(62, "disabled", this._busy);
						__builder.AddContent(63, "Cancel");
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.CloseElement();
				}
				List<string> list = this.Unread().ToList<string>();
				if (list != null && list.Count > 0)
				{
					__builder.OpenElement(64, "div");
					__builder.AddAttribute(65, "class", "card mb-4 border-l-4 border-amber-500 bg-amber-50 p-3 text-sm text-amber-800");
					__builder.OpenElement(66, "b");
					__builder.AddContent(67, "Not read from ");
					int num3 = 68;
					Device selected4 = this.Selected;
					__builder.AddContent(num3, ((selected4 != null) ? selected4.Name : null) ?? "this base station");
					__builder.AddContent(69, ":");
					__builder.CloseElement();
					__builder.AddContent(70, " ");
					__builder.AddContent(71, string.Join(", ", list));
					__builder.AddMarkupContent(72, ".\n                Those fields show ");
					__builder.AddMarkupContent(73, "<i>not read</i> below and are left alone when you save — what is\n                shown for them is not this base station's current configuration.\n            ");
					__builder.CloseElement();
				}
				if (this._busy)
				{
					__builder.OpenElement(74, "div");
					__builder.AddAttribute(75, "class", "card mb-4 flex items-center gap-3 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
					__builder.AddMarkupContent(76, "<span class=\"inline-block h-4 w-4 shrink-0 animate-spin rounded-full border-2 border-blush-200 border-t-blush-600\"></span>\n                ");
					__builder.OpenElement(77, "span");
					__builder.AddContent(78, this._status);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				if (this._errors.Count > 0)
				{
					__builder.OpenElement(79, "div");
					__builder.AddAttribute(80, "class", "card mb-4 border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
					foreach (string text2 in this._errors)
					{
						__builder.OpenElement(81, "div");
						__builder.AddContent(82, text2);
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
				if (this._message != null)
				{
					__builder.OpenElement(83, "div");
					__builder.AddAttribute(84, "class", "card mb-4 border-l-4 border-blush-400 bg-blush-50 p-3 text-sm text-blush-800");
					__builder.AddContent(85, this._message);
					__builder.CloseElement();
				}
				__builder.OpenElement(86, "fieldset");
				__builder.AddAttribute(87, "disabled", this._busy);
				__builder.AddAttribute(88, "class", "space-y-5");
				__builder.OpenElement(89, "div");
				__builder.AddAttribute(90, "class", "grid gap-4 sm:grid-cols-2");
				__builder.OpenElement(91, "label");
				__builder.AddAttribute(92, "class", "block");
				__builder.AddMarkupContent(93, "<span class=\"text-sm font-medium\">Band</span>\n                    ");
				__builder.OpenElement(94, "select");
				__builder.AddAttribute(95, "class", "input");
				__builder.AddAttribute(96, "value", BindConverter.FormatValue(this._edit.Band, null));
				__builder.AddAttribute<ChangeEventArgs>(97, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int? __value)
				{
					this._edit.Band = __value;
				}, this._edit.Band, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.Band == null)
				{
					__builder.OpenElement(98, "option");
					__builder.AddAttribute(99, "value");
					__builder.AddMarkupContent(100, "— not read —");
					__builder.CloseElement();
				}
				foreach (BbuCellSettings.BandWindow bandWindow in BbuCellSettings.Bands)
				{
					__builder.OpenElement(101, "option");
					__builder.AddAttribute(102, "value", bandWindow.Band);
					__builder.AddContent(103, bandWindow.Label);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(104, "\n\n                ");
				__builder.OpenElement(105, "label");
				__builder.AddAttribute(106, "class", "block");
				__builder.AddMarkupContent(107, "<span class=\"text-sm font-medium\">Bandwidth</span>\n                    ");
				__builder.OpenElement(108, "select");
				__builder.AddAttribute(109, "class", "input");
				__builder.AddAttribute(110, "value", BindConverter.FormatValue(this._edit.Bandwidth, null));
				__builder.AddAttribute<ChangeEventArgs>(111, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int? __value)
				{
					this._edit.Bandwidth = __value;
				}, this._edit.Bandwidth, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.Bandwidth == null)
				{
					__builder.OpenElement(112, "option");
					__builder.AddAttribute(113, "value");
					__builder.AddMarkupContent(114, "— not read —");
					__builder.CloseElement();
				}
				foreach (int num4 in BbuCellSettings.Bandwidths)
				{
					__builder.OpenElement(115, "option");
					__builder.AddAttribute(116, "value", num4);
					__builder.AddContent(117, num4);
					__builder.AddContent(118, " MHz");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(119, "\n                    ");
				__builder.AddMarkupContent(120, "<span class=\"mt-1 block text-xs text-cocoa-700\">Channel width in MHz. Uplink and downlink always use the same width; the base station refuses anything else.</span>");
				__builder.CloseElement();
				__builder.AddMarkupContent(121, "\n\n                ");
				__builder.OpenElement(122, "label");
				__builder.AddAttribute(123, "class", "block");
				__builder.AddMarkupContent(124, "<span class=\"text-sm font-medium\">Centre frequency</span>\n                    ");
				__builder.OpenElement(125, "div");
				__builder.AddAttribute(126, "class", "flex items-center gap-2");
				__builder.OpenElement(127, "input");
				__builder.AddAttribute(128, "type", "number");
				__builder.AddAttribute(129, "step", "0.001");
				__builder.AddAttribute(130, "class", "input");
				__builder.AddAttribute(131, "placeholder", "— not read —");
				__builder.AddAttribute(132, "value", BindConverter.FormatValue(this._edit.CentreMhz, CultureInfo.InvariantCulture));
				__builder.AddAttribute<ChangeEventArgs>(133, "onchange", EventCallback.Factory.CreateBinder(this, delegate(double? __value)
				{
					this._edit.CentreMhz = __value;
				}, this._edit.CentreMhz, CultureInfo.InvariantCulture));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.AddMarkupContent(134, "\n                        ");
				__builder.AddMarkupContent(135, "<span class=\"mt-1 shrink-0 text-sm text-cocoa-700\">MHz</span>");
				__builder.CloseElement();
				__builder.AddMarkupContent(136, "\n                    ");
				__builder.OpenElement(137, "span");
				__builder.AddAttribute(138, "class", "mt-1 block text-xs text-cocoa-700");
				double? centreMhz = this._edit.CentreMhz;
				if (centreMhz != null)
				{
					double valueOrDefault = centreMhz.GetValueOrDefault();
					__builder.AddContent(139, "Written to the base station as channel number ");
					__builder.OpenElement(140, "b");
					__builder.AddContent(141, BbuCellSettings.MhzToArfcn(valueOrDefault));
					__builder.CloseElement();
					__builder.AddContent(142, " (NR-ARFCN), the same for uplink and downlink. ");
				}
				else
				{
					__builder.AddContent(143, "Centre of the carrier in MHz. ");
				}
				int? num5 = this._edit.Band;
				if (num5 != null)
				{
					int valueOrDefault2 = num5.GetValueOrDefault();
					BbuCellSettings.BandWindow bandWindow2 = BbuCellSettings.Band(valueOrDefault2);
					if (bandWindow2 != null)
					{
						__builder.AddContent(144, "Band n");
						__builder.AddContent(145, bandWindow2.Band);
						__builder.AddContent(146, " allows ");
						__builder.AddContent(147, BbuCellSettings.Mhz(BbuCellSettings.ArfcnToMhz(bandWindow2.Low)));
						__builder.AddMarkupContent(148, "–");
						__builder.AddContent(149, BbuCellSettings.Mhz(BbuCellSettings.ArfcnToMhz(bandWindow2.High)));
						__builder.AddContent(150, " MHz.");
					}
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(151, "\n\n                ");
				__builder.OpenElement(152, "label");
				__builder.AddAttribute(153, "class", "block");
				__builder.AddMarkupContent(154, "<span class=\"text-sm font-medium\">Max TX power</span>\n                    ");
				__builder.OpenElement(155, "div");
				__builder.AddAttribute(156, "class", "flex items-center gap-2");
				__builder.OpenElement(157, "input");
				__builder.AddAttribute(158, "type", "number");
				__builder.AddAttribute(159, "class", "input");
				__builder.AddAttribute(160, "placeholder", "— not read —");
				__builder.AddAttribute(161, "value", BindConverter.FormatValue(this._edit.MaxTxPower, CultureInfo.InvariantCulture));
				__builder.AddAttribute<ChangeEventArgs>(162, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int? __value)
				{
					this._edit.MaxTxPower = __value;
				}, this._edit.MaxTxPower, CultureInfo.InvariantCulture));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.AddMarkupContent(163, "\n                        ");
				__builder.AddMarkupContent(164, "<span class=\"mt-1 shrink-0 text-sm text-cocoa-700\">dBm, for each channel</span>");
				__builder.CloseElement();
				num5 = this._edit.MaxTxPower;
				if (num5 != null)
				{
					int valueOrDefault3 = num5.GetValueOrDefault();
					__builder.OpenElement(165, "span");
					__builder.AddAttribute(166, "class", "mt-1 block text-sm text-cocoa-900");
					__builder.AddMarkupContent(167, "\n                            = ");
					__builder.OpenElement(168, "b");
					__builder.AddContent(169, BbuCellSettings.FormatMilliwatts(BbuCellSettings.DbmToMilliwatts(valueOrDefault3)));
					__builder.CloseElement();
					__builder.AddMarkupContent(170, " for each channel\n                        ");
					__builder.CloseElement();
				}
				__builder.OpenElement(171, "details");
				__builder.AddAttribute(172, "class", "mt-1 text-xs text-cocoa-700");
				__builder.AddMarkupContent(173, "<summary class=\"cursor-pointer\">dBm → mW table</summary>\n                        ");
				__builder.OpenElement(174, "div");
				__builder.AddAttribute(175, "class", "mt-1 grid grid-cols-2 gap-x-4 sm:grid-cols-3");
				foreach (int num6 in BbuCellSettings.PowerTableDbm)
				{
					__builder.OpenElement(176, "span");
					__builder.AddContent(177, num6);
					__builder.AddContent(178, " dBm = ");
					__builder.AddContent(179, BbuCellSettings.FormatMilliwatts(BbuCellSettings.DbmToMilliwatts(num6)));
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(180, "\n                        ");
				__builder.AddMarkupContent(181, "<span class=\"mt-1 block\">The value is per antenna channel. With four antennas the base station radiates four times this power in total.</span>");
				__builder.CloseElement();
				__builder.AddMarkupContent(182, "\n                    ");
				__builder.OpenElement(183, "span");
				__builder.AddAttribute(184, "class", "mt-1 block text-xs text-cocoa-700");
				if (this.RadioUnitRanges.Count > 0)
				{
					__builder.AddContent(185, "Allowed range depends on the radio unit: ");
					foreach (ValueTuple<string, BbuCellSettings.PowerRange> valueTuple in this.RadioUnitRanges)
					{
						string item = valueTuple.Item1;
						BbuCellSettings.PowerRange item2 = valueTuple.Item2;
						if (item2 != null)
						{
							BbuCellSettings.PowerRange powerRange = item2;
							__builder.AddContent(186, item);
							__builder.AddContent(187, " ");
							__builder.OpenElement(188, "b");
							__builder.AddContent(189, powerRange.Min);
							__builder.AddMarkupContent(190, "–");
							__builder.AddContent(191, powerRange.Max);
							__builder.AddContent(192, " dBm");
							__builder.CloseElement();
							__builder.AddContent(193, ". ");
						}
						else
						{
							__builder.AddContent(194, item);
							__builder.AddContent(195, ": not known, the base station itself accepts ");
							__builder.AddContent(196, 0);
							__builder.AddMarkupContent(197, "–");
							__builder.AddContent(198, 50);
							__builder.AddContent(199, " dBm. ");
						}
					}
					__builder.AddContent(200, "A value outside the unit's range is refused.");
				}
				else
				{
					__builder.AddContent(201, "The base station accepts ");
					__builder.AddContent(202, 0);
					__builder.AddMarkupContent(203, "–");
					__builder.AddContent(204, 50);
					__builder.AddContent(205, " dBm; the radio unit may allow less.");
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(206, "\n\n                ");
				__builder.OpenElement(207, "label");
				__builder.AddAttribute(208, "class", "block");
				__builder.AddMarkupContent(209, "<span class=\"text-sm font-medium\">TDD DL/UL ratio</span>\n                    ");
				__builder.OpenElement(210, "select");
				__builder.AddAttribute(211, "class", "input");
				__builder.AddAttribute(212, "value", this._edit.TddPattern);
				__builder.AddAttribute<ChangeEventArgs>(213, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, new Action<ChangeEventArgs>(this.OnPatternChanged)));
				if (this._edit.TddPattern == null)
				{
					__builder.OpenElement(214, "option");
					__builder.AddAttribute(215, "value");
					__builder.AddMarkupContent(216, "— not read —");
					__builder.CloseElement();
				}
				foreach (string text3 in BbuCellSettings.TddPatterns.Keys)
				{
					__builder.OpenElement(217, "option");
					__builder.AddAttribute(218, "value", text3);
					__builder.AddContent(219, text3);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				ValueTuple<string, bool> valueTuple2 = BbuCellSettings.TddPatternNote(this._edit.TddPattern, this._edit.Band);
				if (valueTuple2.Item1.Length > 0)
				{
					__builder.OpenElement(220, "span");
					__builder.AddAttribute(221, "class", "mt-1 block text-xs " + (valueTuple2.Item2 ? "text-amber-800" : "text-cocoa-700"));
					__builder.AddContent(222, valueTuple2.Item1);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(223, "\n\n                ");
				__builder.OpenElement(224, "label");
				__builder.AddAttribute(225, "class", "block");
				__builder.AddMarkupContent(226, "<span class=\"text-sm font-medium\">Sync slot (special slot)</span>\n                    ");
				__builder.OpenElement(227, "select");
				__builder.AddAttribute(228, "class", "input");
				__builder.AddAttribute(229, "value", BindConverter.FormatValue(this._edit.SpecialSlot, null));
				__builder.AddAttribute<ChangeEventArgs>(230, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._edit.SpecialSlot = __value;
				}, this._edit.SpecialSlot, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.SpecialSlot == null)
				{
					__builder.OpenElement(231, "option");
					__builder.AddAttribute(232, "value");
					__builder.AddMarkupContent(233, "— not read —");
					__builder.CloseElement();
				}
				foreach (string text4 in this.LegalSlots)
				{
					__builder.OpenElement(234, "option");
					__builder.AddAttribute(235, "value", text4);
					__builder.AddContent(236, text4);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(237, "\n                    ");
				__builder.AddMarkupContent(238, "<span class=\"mt-1 block text-xs text-cocoa-700\">\n                        How the switching slot (the \"S\" in the pattern) is split: downlink symbols : uplink symbols, the rest is guard time. Only the splits allowed with the chosen pattern are listed.\n                    </span>");
				__builder.CloseElement();
				__builder.AddMarkupContent(239, "\n\n                ");
				__builder.OpenElement(240, "label");
				__builder.AddAttribute(241, "class", "block");
				__builder.AddMarkupContent(242, "<span class=\"text-sm font-medium\">Cell admin state</span>\n                    ");
				__builder.OpenElement(243, "select");
				__builder.AddAttribute(244, "class", "input");
				__builder.AddAttribute(245, "value", BindConverter.FormatValue(this._edit.AdminState, null));
				__builder.AddAttribute<ChangeEventArgs>(246, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._edit.AdminState = __value;
				}, this._edit.AdminState, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.AdminState == null)
				{
					__builder.OpenElement(247, "option");
					__builder.AddAttribute(248, "value");
					__builder.AddMarkupContent(249, "— not read —");
					__builder.CloseElement();
				}
				foreach (string text5 in BbuCellSettings.AdminStates)
				{
					__builder.OpenElement(250, "option");
					__builder.AddAttribute(251, "value", text5);
					__builder.AddContent(252, text5);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(253, "\n\n                ");
				__builder.OpenElement(254, "label");
				__builder.AddAttribute(255, "class", "block");
				__builder.AddMarkupContent(256, "<span class=\"text-sm font-medium\">Base station name</span>\n                    ");
				__builder.OpenElement(257, "input");
				__builder.AddAttribute(258, "type", "text");
				__builder.AddAttribute(259, "class", "input");
				__builder.AddAttribute(260, "maxlength", 32);
				__builder.AddAttribute(261, "placeholder", "— not read —");
				__builder.AddAttribute(262, "value", BindConverter.FormatValue(this._sysName, null));
				__builder.AddAttribute<ChangeEventArgs>(263, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._sysName = __value;
				}, this._sysName, null));
				__builder.SetUpdatesAttributeName("value");
				__builder.CloseElement();
				__builder.AddMarkupContent(264, "\n                    ");
				__builder.OpenElement(265, "span");
				__builder.AddAttribute(266, "class", "mt-1 block text-xs text-cocoa-700");
				__builder.AddMarkupContent(267, "\n                        Name of the whole base station, as it reports itself. Letters, digits, \"-\" and \"_\", up to ");
				__builder.AddContent(268, 32);
				__builder.AddMarkupContent(269, " characters. Applies at once, no restart.\n                    ");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(270, "\n\n                ");
				__builder.OpenElement(271, "label");
				__builder.AddAttribute(272, "class", "block");
				__builder.AddMarkupContent(273, "<span class=\"text-sm font-medium\">MIMO antennas</span>\n                    ");
				__builder.OpenElement(274, "select");
				__builder.AddAttribute(275, "class", "input");
				__builder.AddAttribute(276, "value", BindConverter.FormatValue(this._edit.Antennas, null));
				__builder.AddAttribute<ChangeEventArgs>(277, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int? __value)
				{
					this._edit.Antennas = __value;
				}, this._edit.Antennas, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.Antennas == null)
				{
					__builder.OpenElement(278, "option");
					__builder.AddAttribute(279, "value");
					__builder.AddMarkupContent(280, "— not read —");
					__builder.CloseElement();
				}
				foreach (int num7 in BbuCellSettings.AntennaCounts)
				{
					__builder.OpenElement(281, "option");
					__builder.AddAttribute(282, "value", num7);
					__builder.AddContent(283, num7);
					__builder.AddContent(284, "x");
					__builder.AddContent(285, num7);
					__builder.AddContent(286, " (");
					__builder.AddContent(287, num7);
					__builder.AddContent(288, "T");
					__builder.AddContent(289, num7);
					__builder.AddContent(290, "R)");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(291, "\n                    ");
				__builder.OpenElement(292, "span");
				__builder.AddAttribute(293, "class", "mt-1 block text-xs text-cocoa-700");
				__builder.AddMarkupContent(294, "\n                        Transmit and receive antennas used by this cell, uplink and downlink alike. Every active cell on the base station must use the same count.\n                        Currently DL ");
				__builder.AddContent(295, this.MimoRaw("DL antennas"));
				__builder.AddContent(296, ", UL ");
				__builder.AddContent(297, this.MimoRaw("UL antennas"));
				__builder.AddMarkupContent(298, ".\n                    ");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(299, "\n\n                ");
				__builder.OpenElement(300, "label");
				__builder.AddAttribute(301, "class", "block");
				__builder.AddMarkupContent(302, "<span class=\"text-sm font-medium\">MIMO layers</span>\n                    ");
				__builder.OpenElement(303, "select");
				__builder.AddAttribute(304, "class", "input");
				__builder.AddAttribute(305, "value", BindConverter.FormatValue(this._edit.Layers, null));
				__builder.AddAttribute<ChangeEventArgs>(306, "onchange", EventCallback.Factory.CreateBinder(this, delegate(int? __value)
				{
					this._edit.Layers = __value;
				}, this._edit.Layers, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._edit.Layers == null)
				{
					__builder.OpenElement(307, "option");
					__builder.AddAttribute(308, "value");
					__builder.AddMarkupContent(309, "— not read —");
					__builder.CloseElement();
				}
				foreach (int num8 in BbuCellSettings.LayerCounts)
				{
					__builder.OpenElement(310, "option");
					__builder.AddAttribute(311, "value", num8);
					__builder.AddContent(312, (num8 == 0) ? "0 (automatic)" : num8.ToString());
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(313, "\n                    ");
				__builder.OpenElement(314, "span");
				__builder.AddAttribute(315, "class", "mt-1 block text-xs text-cocoa-700");
				__builder.AddMarkupContent(316, "\n                        Parallel data streams per direction, uplink and downlink alike, at most one per antenna.\n                        Currently DL ");
				__builder.AddContent(317, this.MimoRaw("DL layers"));
				__builder.AddContent(318, ", UL ");
				__builder.AddContent(319, this.MimoRaw("UL layers"));
				__builder.AddMarkupContent(320, ".\n                    ");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(321, "\n\n                ");
				__builder.OpenElement(322, "label");
				__builder.AddAttribute(323, "class", "block");
				__builder.AddMarkupContent(324, "<span class=\"text-sm font-medium\">Clock sync mode</span>\n                    ");
				__builder.OpenElement(325, "select");
				__builder.AddAttribute(326, "class", "input");
				__builder.AddAttribute(327, "value", BindConverter.FormatValue(this._clockSync, null));
				__builder.AddAttribute<ChangeEventArgs>(328, "onchange", EventCallback.Factory.CreateBinder(this, delegate(string __value)
				{
					this._clockSync = __value;
				}, this._clockSync, null));
				__builder.SetUpdatesAttributeName("value");
				if (this._clockSync == null)
				{
					__builder.OpenElement(329, "option");
					__builder.AddAttribute(330, "value");
					__builder.AddMarkupContent(331, "— not read —");
					__builder.CloseElement();
				}
				foreach (string text6 in BbuCellSettings.ClockSyncModes)
				{
					__builder.OpenElement(332, "option");
					__builder.AddAttribute(333, "value", text6);
					__builder.AddContent(334, BbuSettings.ClockLabel(text6));
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(335, "\n                    ");
				__builder.OpenElement(336, "span");
				__builder.AddAttribute(337, "class", "mt-1 block text-xs text-cocoa-700");
				__builder.AddMarkupContent(338, "\n                        Where the base station takes its time reference from, for the whole unit. Currently ");
				__builder.AddContent(339, this._clockCurrent ?? "not read");
				int num9 = 340;
				string clockLock = this._clockLock;
				__builder.AddContent(num9, (clockLock != null) ? (", " + clockLock) : "");
				__builder.AddMarkupContent(341, ".\n                        GPS needs the GPS antenna connected; PTP needs an IEEE 1588 time server reachable on the SFP1 port; free run uses the internal clock only.\n                    ");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.OpenElement(342, "div");
				__builder.AddAttribute(343, "class", "mt-5 flex flex-wrap items-center gap-3");
				__builder.OpenElement(344, "button");
				__builder.AddAttribute(345, "type", "button");
				__builder.AddAttribute(346, "class", "btn-primary !px-4 !py-2 text-sm");
				__builder.AddAttribute<MouseEventArgs>(347, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.SaveAsync)));
				__builder.AddAttribute(348, "disabled", this._busy || !this.Dirty);
				__builder.AddMarkupContent(349, "\n                Write to base station\n            ");
				__builder.CloseElement();
				__builder.AddMarkupContent(350, "\n            ");
				__builder.OpenElement(351, "button");
				__builder.AddAttribute(352, "type", "button");
				__builder.AddAttribute(353, "class", "btn-secondary !px-4 !py-2 text-sm");
				__builder.AddAttribute<MouseEventArgs>(354, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.RefreshAsync)));
				__builder.AddAttribute(355, "disabled", this._busy);
				__builder.AddMarkupContent(356, "\n                Refresh from last poll\n            ");
				__builder.CloseElement();
				if (!this._busy && !this.Dirty)
				{
					__builder.AddMarkupContent(357, "<span class=\"text-sm text-cocoa-700\">No changes to write.</span>");
				}
				if (this._steps.Count > 0)
				{
					__builder.OpenElement(358, "label");
					__builder.AddAttribute(359, "class", "ml-auto flex items-center gap-2 text-xs text-cocoa-700");
					__builder.OpenElement(360, "input");
					__builder.AddAttribute(361, "type", "checkbox");
					__builder.AddAttribute(362, "checked", BindConverter.FormatValue(this._showCommands, null));
					__builder.AddAttribute<ChangeEventArgs>(363, "onchange", EventCallback.Factory.CreateBinder(this, delegate(bool __value)
					{
						this._showCommands = __value;
					}, this._showCommands, null));
					__builder.SetUpdatesAttributeName("checked");
					__builder.CloseElement();
					__builder.AddMarkupContent(364, " Show commands\n                ");
					__builder.CloseElement();
				}
				__builder.CloseElement();
				if (this._steps.Count > 0)
				{
					__builder.OpenElement(365, "ul");
					__builder.AddAttribute(366, "class", "mt-4 space-y-1 text-xs");
					foreach (MmlStep mmlStep in this._steps)
					{
						__builder.OpenElement(367, "li");
						__builder.AddAttribute(368, "class", "flex flex-wrap items-start gap-2");
						__builder.OpenElement(369, "span");
						__builder.AddAttribute(370, "class", mmlStep.Ok ? "pill pill-up" : "pill pill-down");
						__builder.AddContent(371, mmlStep.Ok ? "ok" : "failed");
						__builder.CloseElement();
						__builder.AddMarkupContent(372, "\n                        ");
						__builder.OpenElement(373, "span");
						__builder.AddAttribute(374, "class", "font-medium text-cocoa-900");
						__builder.AddContent(375, mmlStep.Label);
						__builder.CloseElement();
						if (this._showCommands)
						{
							__builder.OpenElement(376, "code");
							__builder.AddAttribute(377, "class", "min-w-0 break-all text-cocoa-700");
							__builder.AddContent(378, mmlStep.Command);
							__builder.CloseElement();
						}
						if (!mmlStep.Ok)
						{
							__builder.OpenElement(379, "span");
							__builder.AddAttribute(380, "class", "text-red-700");
							__builder.AddContent(381, "RETCODE ");
							__builder.AddContent(382, mmlStep.RetCode);
							__builder.AddContent(383, " ");
							__builder.AddContent(384, mmlStep.Message);
							__builder.CloseElement();
						}
						__builder.CloseElement();
					}
					__builder.CloseElement();
				}
			}
			__builder.CloseElement();
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001BA90 File Offset: 0x00019C90
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x0001BA98 File Offset: 0x00019C98
		[Parameter]
		public int? BbuId { get; set; }

		// Token: 0x06000594 RID: 1428 RVA: 0x0001BAA4 File Offset: 0x00019CA4
		private static string ClockLabel(string mode)
		{
			string text;
			if (!(mode == "GNSS_mode"))
			{
				if (!(mode == "1588_mode"))
				{
					if (!(mode == "free_run_mode"))
					{
						text = mode;
					}
					else
					{
						text = "Free-running (internal clock)";
					}
				}
				else
				{
					text = "PTP (IEEE 1588)";
				}
			}
			else
			{
				text = "GPS antenna (GNSS)";
			}
			return text;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001BAF8 File Offset: 0x00019CF8
		private string MimoRaw(string label)
		{
			BbuSettings.CellOption cellOption = this._cells.FirstOrDefault<BbuSettings.CellOption>((BbuSettings.CellOption c) => c.Index == this._cellIndex);
			ChildStatus childStatus = ((cellOption != null) ? cellOption.Node : null);
			string text;
			if (childStatus == null)
			{
				text = null;
			}
			else
			{
				Metric metric = childStatus.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == label);
				text = ((metric != null) ? metric.Value : null);
			}
			string text2 = text;
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2;
			}
			return "not read";
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001BB75 File Offset: 0x00019D75
		[Nullable(2)]
		private Device Selected
		{
			[NullableContext(2)]
			get
			{
				return this._bbus.FirstOrDefault<Device>((Device b) => b.Id == this._bbuId);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001BB8E File Offset: 0x00019D8E
		[Nullable(2)]
		private DeviceSnapshot SelectedSnapshot
		{
			[NullableContext(2)]
			get
			{
				return this._snapshots.GetValueOrDefault(this._bbuId);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001BBA4 File Offset: 0x00019DA4
		[TupleElementNames(new string[] { "Model", "Range" })]
		[Nullable(new byte[] { 1, 0, 1, 2 })]
		private List<ValueTuple<string, BbuCellSettings.PowerRange>> RadioUnitRanges
		{
			[return: TupleElementNames(new string[] { "Model", "Range" })]
			[return: Nullable(new byte[] { 1, 0, 1, 2 })]
			get
			{
				DeviceSnapshot selectedSnapshot = this.SelectedSnapshot;
				return (from m in (from c in ((selectedSnapshot != null) ? selectedSnapshot.Children : null) ?? new List<ChildStatus>()
						where string.Equals(c.Kind, "ru", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(c.Model)
						select c.Model.Trim()).Distinct<string>(StringComparer.OrdinalIgnoreCase).OrderBy<string, string>((string m) => m, StringComparer.OrdinalIgnoreCase)
					select new ValueTuple<string, BbuCellSettings.PowerRange>(m, BbuCellSettings.PowerRangeFor(m))).ToList<ValueTuple<string, BbuCellSettings.PowerRange>>();
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001BC70 File Offset: 0x00019E70
		private string[] LegalSlots
		{
			get
			{
				string tddPattern = this._edit.TddPattern;
				string[] array;
				if (tddPattern == null || !BbuCellSettings.TddPatterns.TryGetValue(tddPattern, out array))
				{
					return BbuCellSettings.TddPatterns.Values.First<string[]>();
				}
				return array;
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001BCAC File Offset: 0x00019EAC
		private IEnumerable<string> Unread()
		{
			BbuSettings.<Unread>d__36 <Unread>d__ = new BbuSettings.<Unread>d__36(-2);
			<Unread>d__.<>4__this = this;
			return <Unread>d__;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001BCBC File Offset: 0x00019EBC
		[NullableContext(0)]
		private static bool Changed<T>(T? now, T? was) where T : struct
		{
			return now != null && !Nullable.Equals<T>(now, was);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001BCD3 File Offset: 0x00019ED3
		[NullableContext(2)]
		private static bool Changed(string now, string was)
		{
			return now != null && !string.Equals(now, was, StringComparison.Ordinal);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001BCE8 File Offset: 0x00019EE8
		private static int? Arfcn(double? mhz)
		{
			if (mhz != null)
			{
				double valueOrDefault = mhz.GetValueOrDefault();
				return new int?(BbuCellSettings.MhzToArfcn(valueOrDefault));
			}
			return null;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001BD20 File Offset: 0x00019F20
		private bool TddChanged
		{
			get
			{
				return (BbuSettings.Changed(this._edit.TddPattern, this._baseline.TddPattern) || BbuSettings.Changed(this._edit.SpecialSlot, this._baseline.SpecialSlot)) && this._edit.TddPattern != null && this._edit.SpecialSlot != null;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001BD88 File Offset: 0x00019F88
		private bool Dirty
		{
			get
			{
				if (!BbuSettings.Changed<int>(this._edit.Band, this._baseline.Band) && !BbuSettings.Changed<int>(this._edit.Bandwidth, this._baseline.Bandwidth) && !BbuSettings.Changed<int>(BbuSettings.Arfcn(this._edit.CentreMhz), BbuSettings.Arfcn(this._baseline.CentreMhz)) && !this.TddChanged && !BbuSettings.Changed<int>(this._edit.MaxTxPower, this._baseline.MaxTxPower) && !BbuSettings.Changed(this._edit.AdminState, this._baseline.AdminState) && !BbuSettings.Changed<int>(this._edit.Antennas, this._baseline.Antennas) && !BbuSettings.Changed<int>(this._edit.Layers, this._baseline.Layers))
				{
					string sysName = this._sysName;
					if (!BbuSettings.Changed((sysName != null) ? sysName.Trim() : null, this._sysNameBaseline))
					{
						return BbuSettings.Changed(this._clockSync, this._clockBaseline);
					}
				}
				return true;
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		protected override async Task OnInitializedAsync()
		{
			await this.LoadAsync();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
		private async Task LoadAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string))), Expression.OrElse(Expression.Equal(Expression.Property(Expression.Constant(this, typeof(BbuSettings)), methodof(BbuSettings.get_BbuId())), Expression.Constant(null, typeof(int?))), Expression.Equal(Expression.Convert(Expression.Property(parameterExpression, methodof(Device.get_Id())), typeof(int?)), Expression.Property(Expression.Constant(this, typeof(BbuSettings)), methodof(BbuSettings.get_BbuId()))))), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				this._bbus = await queryable2.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(default(CancellationToken));
				this._snapshots = this._bbus.ToDictionary<Device, int, DeviceSnapshot>((Device d) => d.Id, (Device d) => SnapshotSerializer.Deserialize(d.LastSnapshotJson));
				if (this._bbus.Count == 0)
				{
					this._cells = new List<BbuSettings.CellOption>();
				}
				else
				{
					if (this._bbus.All<Device>((Device b) => b.Id != this._bbuId))
					{
						this._bbuId = this._bbus[0].Id;
					}
					this.SelectCells();
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

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001BF3C File Offset: 0x0001A13C
		private void SelectCells()
		{
			DeviceSnapshot selectedSnapshot = this.SelectedSnapshot;
			this._cells = (from c in ((selectedSnapshot != null) ? selectedSnapshot.Children : null) ?? new List<ChildStatus>()
				where string.Equals(c.Kind, "cell", StringComparison.OrdinalIgnoreCase)
				select new BbuSettings.CellOption(BbuSettings.ParseCellIndex(c.Identifier), c) into c
				where c.Index >= 1
				orderby c.Index
				select c).ToList<BbuSettings.CellOption>();
			if (this._cells.Count == 0)
			{
				return;
			}
			if (this._cells.All<BbuSettings.CellOption>((BbuSettings.CellOption c) => c.Index != this._cellIndex))
			{
				this._cellIndex = this._cells[0].Index;
			}
			this.LoadFromSnapshot();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001C044 File Offset: 0x0001A244
		[NullableContext(2)]
		private static int ParseCellIndex(string identifier)
		{
			IEnumerable<char> enumerable = identifier ?? "";
			Func<char, bool> func;
			if ((func = BbuSettings.<>O.<0>__IsDigit) == null)
			{
				func = (BbuSettings.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
			}
			int num;
			if (!int.TryParse(new string(enumerable.Where<char>(func).ToArray<char>()), out num))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001C094 File Offset: 0x0001A294
		private void LoadFromSnapshot()
		{
			BbuSettings.<>c__DisplayClass48_0 CS$<>8__locals1 = new BbuSettings.<>c__DisplayClass48_0();
			CS$<>8__locals1.<>4__this = this;
			BbuSettings.<>c__DisplayClass48_0 CS$<>8__locals2 = CS$<>8__locals1;
			BbuSettings.CellOption cellOption = this._cells.FirstOrDefault<BbuSettings.CellOption>((BbuSettings.CellOption c) => c.Index == CS$<>8__locals1.<>4__this._cellIndex);
			CS$<>8__locals2.node = ((cellOption != null) ? cellOption.Node : null);
			string text = BbuCellSettings.ParseTddPattern(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("TDD pattern"));
			string text2 = ((text != null && BbuCellSettings.TddPatterns.ContainsKey(text)) ? text : null);
			string text4;
			if (text2 != null)
			{
				string text3 = CS$<>8__locals1.<LoadFromSnapshot>g__M|1("Special slot");
				if (text3 != null && text3.Length > 0 && BbuCellSettings.TddPatterns[text2].Contains(text3, StringComparer.Ordinal))
				{
					text4 = text3;
					goto IL_009D;
				}
			}
			text4 = null;
			IL_009D:
			string text5 = text4;
			string text6 = CS$<>8__locals1.<LoadFromSnapshot>g__M|1("Admin state");
			string text7 = ((text6 != null && text6.Length > 0 && BbuCellSettings.AdminStates.Contains(text6, StringComparer.OrdinalIgnoreCase)) ? text6.ToLowerInvariant() : null);
			CellConfigEdit cellConfigEdit = new CellConfigEdit();
			cellConfigEdit.CellIndex = this._cellIndex;
			cellConfigEdit.Band = BbuCellSettings.ParseBand(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("Band"));
			cellConfigEdit.Bandwidth = BbuCellSettings.ParseBandwidth(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("DL bandwidth"));
			int? num = BbuCellSettings.ParseInt(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("DL ARFCN"));
			double? num2;
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				num2 = new double?(Math.Round(BbuCellSettings.ArfcnToMhz(valueOrDefault), 3));
			}
			else
			{
				num2 = null;
			}
			cellConfigEdit.CentreMhz = num2;
			cellConfigEdit.TddPattern = text2;
			cellConfigEdit.SpecialSlot = text5;
			cellConfigEdit.MaxTxPower = BbuCellSettings.ParseInt(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("Max TX power"));
			cellConfigEdit.AdminState = text7;
			num = BbuCellSettings.ParseInt(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("DL antennas"));
			int? num3;
			if (num != null)
			{
				int valueOrDefault2 = num.GetValueOrDefault();
				if (BbuCellSettings.AntennaCounts.Contains(valueOrDefault2))
				{
					num3 = new int?(valueOrDefault2);
					goto IL_01D7;
				}
			}
			num3 = null;
			IL_01D7:
			cellConfigEdit.Antennas = num3;
			num = BbuCellSettings.ParseInt(CS$<>8__locals1.<LoadFromSnapshot>g__M|1("DL layers"));
			int? num4;
			if (num != null)
			{
				int valueOrDefault3 = num.GetValueOrDefault();
				if (BbuCellSettings.LayerCounts.Contains(valueOrDefault3))
				{
					num4 = new int?(valueOrDefault3);
					goto IL_0227;
				}
			}
			num4 = null;
			IL_0227:
			cellConfigEdit.Layers = num4;
			this._edit = cellConfigEdit;
			this._baseline = BbuSettings.Clone(this._edit);
			DeviceSnapshot selectedSnapshot = this.SelectedSnapshot;
			string text8;
			if (selectedSnapshot == null)
			{
				text8 = null;
			}
			else
			{
				Metric metric = selectedSnapshot.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "System name");
				text8 = ((metric != null) ? metric.Value : null);
			}
			string text9 = text8;
			this._sysName = ((text9 != null && text9.Length > 0) ? text9 : null);
			this._sysNameBaseline = this._sysName;
			DeviceSnapshot selectedSnapshot2 = this.SelectedSnapshot;
			string text10;
			if (selectedSnapshot2 == null)
			{
				text10 = null;
			}
			else
			{
				Metric metric2 = selectedSnapshot2.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "Clock sync");
				text10 = ((metric2 != null) ? metric2.Value : null);
			}
			string text11 = text10;
			this._clockCurrent = ((text11 != null && text11.Length > 0) ? text11 : null);
			DeviceSnapshot selectedSnapshot3 = this.SelectedSnapshot;
			string text12;
			if (selectedSnapshot3 == null)
			{
				text12 = null;
			}
			else
			{
				Metric metric3 = selectedSnapshot3.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "Clock lock");
				text12 = ((metric3 != null) ? metric3.Value : null);
			}
			string text13 = text12;
			this._clockLock = ((text13 != null && text13.Length > 0) ? text13 : null);
			string clockCurrent = this._clockCurrent;
			this._clockSync = ((clockCurrent != null && BbuCellSettings.ClockSyncModes.Contains(clockCurrent, StringComparer.Ordinal)) ? clockCurrent : null);
			this._clockBaseline = this._clockSync;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001C43C File Offset: 0x0001A63C
		private static CellConfigEdit Clone(CellConfigEdit e)
		{
			return new CellConfigEdit
			{
				CellIndex = e.CellIndex,
				Band = e.Band,
				Bandwidth = e.Bandwidth,
				CentreMhz = e.CentreMhz,
				TddPattern = e.TddPattern,
				SpecialSlot = e.SpecialSlot,
				MaxTxPower = e.MaxTxPower,
				AdminState = e.AdminState,
				Antennas = e.Antennas,
				Layers = e.Layers
			};
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001C4C8 File Offset: 0x0001A6C8
		private void OnBbuChanged(ChangeEventArgs e)
		{
			object value = e.Value;
			int num;
			if (!int.TryParse((value != null) ? value.ToString() : null, out num))
			{
				return;
			}
			this._bbuId = num;
			this.ClearResult();
			this._cellIndex = 0;
			this.SelectCells();
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001C50C File Offset: 0x0001A70C
		private void OnCellChanged(ChangeEventArgs e)
		{
			object value = e.Value;
			int num;
			if (!int.TryParse((value != null) ? value.ToString() : null, out num))
			{
				return;
			}
			this._cellIndex = num;
			this.ClearResult();
			this.LoadFromSnapshot();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001C548 File Offset: 0x0001A748
		private void ClearResult()
		{
			this._errors = new List<string>();
			this._steps = new List<MmlStep>();
			this._message = null;
			this._confirmRestart = false;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001C570 File Offset: 0x0001A770
		private void OnPatternChanged(ChangeEventArgs e)
		{
			object value = e.Value;
			string text = ((value != null) ? value.ToString() : null);
			this._edit.TddPattern = (string.IsNullOrEmpty(text) ? null : text);
			string specialSlot = this._edit.SpecialSlot;
			if (specialSlot != null && !this.LegalSlots.Contains(specialSlot, StringComparer.Ordinal))
			{
				this._edit.SpecialSlot = this.LegalSlots[0];
			}
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001C5E4 File Offset: 0x0001A7E4
		private List<MmlWrite> BuildWrites()
		{
			List<MmlWrite> list = new List<MmlWrite>();
			this._edit.CellIndex = this._cellIndex;
			string text2;
			string text = BbuCellSettings.CarrierCommand(this._edit, this._baseline, out text2);
			if (text != null)
			{
				List<MmlWrite> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cell ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._cellIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" band / bandwidth / frequency");
				list2.Add(new MmlWrite(defaultInterpolatedStringHandler.ToStringAndClear(), text, true));
			}
			else if (text2 != null)
			{
				this._errors.Add(text2);
			}
			if (this.TddChanged)
			{
				List<MmlWrite> list3 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Cell ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this._cellIndex);
				defaultInterpolatedStringHandler2.AppendLiteral(" TDD frame");
				list3.Add(new MmlWrite(defaultInterpolatedStringHandler2.ToStringAndClear(), BbuCellSettings.ModPhyTddUlDl(this._cellIndex, this._edit.TddPattern, this._edit.SpecialSlot), true));
			}
			if (BbuSettings.Changed<int>(this._edit.MaxTxPower, this._baseline.MaxTxPower))
			{
				int? num = this._edit.MaxTxPower;
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					List<MmlWrite> list4 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(18, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("Cell ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(this._cellIndex);
					defaultInterpolatedStringHandler3.AppendLiteral(" max TX power");
					list4.Add(new MmlWrite(defaultInterpolatedStringHandler3.ToStringAndClear(), BbuCellSettings.ModMaxTxPower(this._cellIndex, valueOrDefault), true));
				}
			}
			if (BbuSettings.Changed(this._edit.AdminState, this._baseline.AdminState))
			{
				string adminState = this._edit.AdminState;
				if (adminState != null)
				{
					List<MmlWrite> list5 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(17, 1);
					defaultInterpolatedStringHandler4.AppendLiteral("Cell ");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(this._cellIndex);
					defaultInterpolatedStringHandler4.AppendLiteral(" admin state");
					list5.Add(new MmlWrite(defaultInterpolatedStringHandler4.ToStringAndClear(), BbuCellSettings.ModOpState(this._cellIndex, adminState), true));
				}
			}
			if (BbuSettings.Changed<int>(this._edit.Antennas, this._baseline.Antennas))
			{
				int? num = this._edit.Antennas;
				if (num != null)
				{
					int valueOrDefault2 = num.GetValueOrDefault();
					List<MmlWrite> list6 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler5.AppendLiteral("Cell ");
					defaultInterpolatedStringHandler5.AppendFormatted<int>(this._cellIndex);
					defaultInterpolatedStringHandler5.AppendLiteral(" MIMO antennas");
					list6.Add(new MmlWrite(defaultInterpolatedStringHandler5.ToStringAndClear(), BbuCellSettings.ModCellAntennas(this._cellIndex, valueOrDefault2), true));
				}
			}
			if (BbuSettings.Changed<int>(this._edit.Layers, this._baseline.Layers))
			{
				int? num = this._edit.Layers;
				if (num != null)
				{
					int valueOrDefault3 = num.GetValueOrDefault();
					List<MmlWrite> list7 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(17, 1);
					defaultInterpolatedStringHandler6.AppendLiteral("Cell ");
					defaultInterpolatedStringHandler6.AppendFormatted<int>(this._cellIndex);
					defaultInterpolatedStringHandler6.AppendLiteral(" MIMO layers");
					list7.Add(new MmlWrite(defaultInterpolatedStringHandler6.ToStringAndClear(), BbuCellSettings.ModCellLayers(this._cellIndex, valueOrDefault3), true));
				}
			}
			if (BbuSettings.Changed(this._clockSync, this._clockBaseline))
			{
				string clockSync = this._clockSync;
				if (clockSync != null)
				{
					list.Add(new MmlWrite("Clock sync mode", BbuCellSettings.SetClockSync(clockSync), true));
				}
			}
			string sysName = this._sysName;
			if (BbuSettings.Changed((sysName != null) ? sysName.Trim() : null, this._sysNameBaseline))
			{
				string sysName2 = this._sysName;
				if (sysName2 != null)
				{
					string text3 = BbuCellSettings.ValidateSysName(sysName2);
					if (text3 != null)
					{
						this._errors.Add(text3);
					}
					else
					{
						list.Add(new MmlWrite("Base station name", BbuCellSettings.SetSysName(sysName2), false));
					}
				}
			}
			return list;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001C994 File Offset: 0x0001AB94
		private async Task RefreshAsync()
		{
			this._busy = true;
			this._status = "Reading the latest poll…";
			try
			{
				this.ClearResult();
				await this.LoadAsync();
			}
			finally
			{
				this._busy = false;
				this._status = "";
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001C9D8 File Offset: 0x0001ABD8
		private async Task SaveAsync()
		{
			BbuSettings.<>c__DisplayClass56_0 CS$<>8__locals1 = new BbuSettings.<>c__DisplayClass56_0();
			CS$<>8__locals1.bbu = this.Selected;
			if (CS$<>8__locals1.bbu != null)
			{
				this._errors = BbuCellSettings.Validate(this._edit);
				this._steps = new List<MmlStep>();
				this._message = null;
				if (this._errors.Count <= 0)
				{
					List<MmlWrite> writes = this.BuildWrites();
					if (this._errors.Count <= 0)
					{
						if (writes.Count == 0)
						{
							this._message = "Nothing changed.";
						}
						else
						{
							this._busy = true;
							this._status = (this.Writer.IsBusy(CS$<>8__locals1.bbu) ? "Waiting for the base station session…" : "Writing…");
							try
							{
								BbuWriteOutcome bbuWriteOutcome = await this.Writer.ApplyAsync(CS$<>8__locals1.bbu, writes, CancellationToken.None);
								this._steps = bbuWriteOutcome.Steps;
								if (!bbuWriteOutcome.Ok)
								{
									this._errors = new List<string> { bbuWriteOutcome.Error ?? "The write failed." };
								}
								else
								{
									bool needsRestart = writes.Any<MmlWrite>((MmlWrite w) => w.RequiresRestart);
									if (needsRestart)
									{
										BbuSettings.<>c__DisplayClass56_1 CS$<>8__locals2 = new BbuSettings.<>c__DisplayClass56_1();
										CS$<>8__locals2.now = DateTime.UtcNow;
										ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
										object obj = null;
										try
										{
											IQueryable<Device> devices = db.Devices;
											ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
											await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(BbuSettings.<>c__DisplayClass56_0)), fieldof(BbuSettings.<>c__DisplayClass56_0.bbu)), methodof(Device.get_Id()))), new ParameterExpression[] { parameterExpression })).ExecuteUpdateAsync<Device>(delegate([Nullable(new byte[] { 1, 0 })] UpdateSettersBuilder<Device> s)
											{
												s.SetProperty<DateTime?>((Device d) => d.ConfigChangedAt, new DateTime?(CS$<>8__locals2.now));
											}, default(CancellationToken));
											CS$<>8__locals1.bbu.ConfigChangedAt = new DateTime?(CS$<>8__locals2.now);
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
										CS$<>8__locals2 = null;
										db = null;
									}
									this._baseline = BbuSettings.Clone(this._edit);
									string sysName = this._sysName;
									this._sysNameBaseline = ((sysName != null) ? sysName.Trim() : null);
									this._clockBaseline = this._clockSync;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 3);
									defaultInterpolatedStringHandler.AppendFormatted<int>(writes.Count);
									defaultInterpolatedStringHandler.AppendLiteral(" command");
									defaultInterpolatedStringHandler.AppendFormatted((writes.Count == 1) ? "" : "s");
									defaultInterpolatedStringHandler.AppendLiteral(" accepted by ");
									defaultInterpolatedStringHandler.AppendFormatted(CS$<>8__locals1.bbu.Name);
									defaultInterpolatedStringHandler.AppendLiteral(". ");
									this._message = defaultInterpolatedStringHandler.ToStringAndClear() + (needsRestart ? "Restart it to apply, then use Refresh to confirm what it reports." : "Use Refresh after the next poll to confirm what it reports.");
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
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001CA1C File Offset: 0x0001AC1C
		private async Task RestartAsync()
		{
			BbuSettings.<>c__DisplayClass57_0 CS$<>8__locals1 = new BbuSettings.<>c__DisplayClass57_0();
			CS$<>8__locals1.bbu = this.Selected;
			if (CS$<>8__locals1.bbu != null)
			{
				this._confirmRestart = false;
				this._busy = true;
				this._status = "Restarting the base station…";
				this._errors = new List<string>();
				this._message = null;
				try
				{
					BbuSettings.<>c__DisplayClass57_1 CS$<>8__locals2 = new BbuSettings.<>c__DisplayClass57_1();
					BbuWriteOutcome bbuWriteOutcome = await this.Writer.RestartAsync(CS$<>8__locals1.bbu, CancellationToken.None);
					this._steps = bbuWriteOutcome.Steps;
					if (!bbuWriteOutcome.Ok)
					{
						this._errors = new List<string> { bbuWriteOutcome.Error ?? "The restart command failed." };
					}
					else
					{
						CS$<>8__locals2.now = DateTime.UtcNow;
						ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
						object obj = null;
						try
						{
							IQueryable<Device> devices = db.Devices;
							ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
							await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(BbuSettings.<>c__DisplayClass57_0)), fieldof(BbuSettings.<>c__DisplayClass57_0.bbu)), methodof(Device.get_Id()))), new ParameterExpression[] { parameterExpression })).ExecuteUpdateAsync<Device>(delegate([Nullable(new byte[] { 1, 0 })] UpdateSettersBuilder<Device> s)
							{
								s.SetProperty<DateTime?>((Device d) => d.ConfigRestartedAt, new DateTime?(CS$<>8__locals2.now));
							}, default(CancellationToken));
							CS$<>8__locals1.bbu.ConfigRestartedAt = new DateTime?(CS$<>8__locals2.now);
							this._message = CS$<>8__locals1.bbu.Name + " is restarting. It will be unreachable for a short while.";
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
						CS$<>8__locals2 = null;
						db = null;
					}
				}
				finally
				{
					this._busy = false;
					this._status = "";
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001CA5F File Offset: 0x0001AC5F
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0001CA67 File Offset: 0x0001AC67
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001CA70 File Offset: 0x0001AC70
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001CA78 File Offset: 0x0001AC78
		[Inject]
		private BbuConfigWriter Writer { get; set; }

		// Token: 0x0400023D RID: 573
		private List<Device> _bbus = new List<Device>();

		// Token: 0x0400023E RID: 574
		[Nullable(new byte[] { 1, 2 })]
		private Dictionary<int, DeviceSnapshot> _snapshots = new Dictionary<int, DeviceSnapshot>();

		// Token: 0x0400023F RID: 575
		private int _bbuId;

		// Token: 0x04000240 RID: 576
		private List<BbuSettings.CellOption> _cells = new List<BbuSettings.CellOption>();

		// Token: 0x04000241 RID: 577
		private int _cellIndex = 1;

		// Token: 0x04000242 RID: 578
		private CellConfigEdit _edit = new CellConfigEdit();

		// Token: 0x04000243 RID: 579
		private CellConfigEdit _baseline = new CellConfigEdit();

		// Token: 0x04000244 RID: 580
		[Nullable(2)]
		private string _sysName;

		// Token: 0x04000245 RID: 581
		[Nullable(2)]
		private string _sysNameBaseline;

		// Token: 0x04000246 RID: 582
		[Nullable(2)]
		private string _clockSync;

		// Token: 0x04000247 RID: 583
		[Nullable(2)]
		private string _clockBaseline;

		// Token: 0x04000248 RID: 584
		[Nullable(2)]
		private string _clockCurrent;

		// Token: 0x04000249 RID: 585
		[Nullable(2)]
		private string _clockLock;

		// Token: 0x0400024B RID: 587
		private bool _busy;

		// Token: 0x0400024C RID: 588
		private bool _confirmRestart;

		// Token: 0x0400024D RID: 589
		private bool _showCommands = true;

		// Token: 0x0400024E RID: 590
		private string _status = "";

		// Token: 0x0400024F RID: 591
		[Nullable(2)]
		private string _message;

		// Token: 0x04000250 RID: 592
		private List<string> _errors = new List<string>();

		// Token: 0x04000251 RID: 593
		private List<MmlStep> _steps = new List<MmlStep>();

		// Token: 0x0200015F RID: 351
		[Nullable(0)]
		private sealed class CellOption : IEquatable<BbuSettings.CellOption>
		{
			// Token: 0x06000BBD RID: 3005 RVA: 0x0004FFB3 File Offset: 0x0004E1B3
			public CellOption(int Index, ChildStatus Node)
			{
				this.Index = Index;
				this.Node = Node;
				base..ctor();
			}

			// Token: 0x17000297 RID: 663
			// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0004FFC9 File Offset: 0x0004E1C9
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(BbuSettings.CellOption);
				}
			}

			// Token: 0x17000298 RID: 664
			// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0004FFD5 File Offset: 0x0004E1D5
			// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0004FFDD File Offset: 0x0004E1DD
			public int Index { get; set; }

			// Token: 0x17000299 RID: 665
			// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0004FFE6 File Offset: 0x0004E1E6
			// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0004FFEE File Offset: 0x0004E1EE
			public ChildStatus Node { get; set; }

			// Token: 0x06000BC3 RID: 3011 RVA: 0x0004FFF8 File Offset: 0x0004E1F8
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("CellOption");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000BC4 RID: 3012 RVA: 0x00050044 File Offset: 0x0004E244
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Index = ");
				builder.Append(this.Index.ToString());
				builder.Append(", Node = ");
				builder.Append(this.Node);
				return true;
			}

			// Token: 0x06000BC5 RID: 3013 RVA: 0x00050097 File Offset: 0x0004E297
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(BbuSettings.CellOption left, BbuSettings.CellOption right)
			{
				return !(left == right);
			}

			// Token: 0x06000BC6 RID: 3014 RVA: 0x000500A3 File Offset: 0x0004E2A3
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(BbuSettings.CellOption left, BbuSettings.CellOption right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000BC7 RID: 3015 RVA: 0x000500B7 File Offset: 0x0004E2B7
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Index>k__BackingField)) * -1521134295 + EqualityComparer<ChildStatus>.Default.GetHashCode(this.<Node>k__BackingField);
			}

			// Token: 0x06000BC8 RID: 3016 RVA: 0x000500F7 File Offset: 0x0004E2F7
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as BbuSettings.CellOption);
			}

			// Token: 0x06000BC9 RID: 3017 RVA: 0x00050108 File Offset: 0x0004E308
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(BbuSettings.CellOption other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<Index>k__BackingField, other.<Index>k__BackingField) && EqualityComparer<ChildStatus>.Default.Equals(this.<Node>k__BackingField, other.<Node>k__BackingField));
			}

			// Token: 0x06000BCB RID: 3019 RVA: 0x00050169 File Offset: 0x0004E369
			[CompilerGenerated]
			private CellOption(BbuSettings.CellOption original)
			{
				this.Index = original.<Index>k__BackingField;
				this.Node = original.<Node>k__BackingField;
			}

			// Token: 0x06000BCC RID: 3020 RVA: 0x00050189 File Offset: 0x0004E389
			[CompilerGenerated]
			public void Deconstruct(out int Index, out ChildStatus Node)
			{
				Index = this.Index;
				Node = this.Node;
			}
		}

		// Token: 0x02000160 RID: 352
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000801 RID: 2049
			[Nullable(0)]
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
