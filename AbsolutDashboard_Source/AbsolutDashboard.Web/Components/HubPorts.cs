using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	public class HubPorts : ComponentBase
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			if (this.Data.Count > 0 || this.ShowMissingData)
			{
				__builder.OpenElement(0, "div");
				__builder.AddAttribute(1, "class", "hub-row");
				__builder.AddMarkupContent(2, "<span class=\"hub-label\">Data</span>");
				if (this.Data.Count > 0)
				{
					using (List<RhubPort>.Enumerator enumerator = this.Data.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RhubPort rhubPort = enumerator.Current;
							__builder.AddContent(3, this.Tile(rhubPort, "p"));
						}
						goto IL_00C2;
					}
				}
				__builder.OpenElement(4, "span");
				__builder.AddAttribute(5, "class", "text-xs text-cocoa-700");
				__builder.AddContent(6, this.MissingDataText);
				__builder.CloseElement();
				IL_00C2:
				__builder.CloseElement();
			}
			if (this.Power.Count > 0 || this.ShowMissingPower)
			{
				__builder.OpenElement(7, "div");
				__builder.AddAttribute(8, "class", "hub-row " + ((this.Data.Count > 0 || this.ShowMissingData) ? "mt-1" : ""));
				__builder.AddMarkupContent(9, "<span class=\"hub-label\">Power</span>");
				if (this.Power.Count > 0)
				{
					using (List<RhubPort>.Enumerator enumerator = this.Power.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RhubPort rhubPort2 = enumerator.Current;
							__builder.AddContent(10, this.Tile(rhubPort2, "V"));
						}
						goto IL_01B6;
					}
				}
				__builder.OpenElement(11, "span");
				__builder.AddAttribute(12, "class", "text-xs text-cocoa-700");
				__builder.AddContent(13, this.MissingPowerText);
				__builder.CloseElement();
				IL_01B6:
				__builder.CloseElement();
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001D1BC File Offset: 0x0001B3BC
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001D1C4 File Offset: 0x0001B3C4
		[Parameter]
		public List<RhubPort> Ports { get; set; } = new List<RhubPort>();

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001D1CD File Offset: 0x0001B3CD
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0001D1D5 File Offset: 0x0001B3D5
		[Parameter]
		public bool Compact { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001D1DE File Offset: 0x0001B3DE
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001D1E6 File Offset: 0x0001B3E6
		[Parameter]
		public bool ShowMissingData { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001D1EF File Offset: 0x0001B3EF
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x0001D1F7 File Offset: 0x0001B3F7
		[Parameter]
		public bool ShowMissingPower { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001D200 File Offset: 0x0001B400
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x0001D208 File Offset: 0x0001B408
		[Parameter]
		public string MissingDataText { get; set; } = "not reported by the base station";

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001D211 File Offset: 0x0001B411
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001D219 File Offset: 0x0001B419
		[Parameter]
		public string MissingPowerText { get; set; } = "not measured";

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001D224 File Offset: 0x0001B424
		private List<RhubPort> Data
		{
			get
			{
				return (from p in this.Ports
					where p.Kind == PortKind.Data
					orderby p.Index
					select p).ToList<RhubPort>();
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001D284 File Offset: 0x0001B484
		private List<RhubPort> Power
		{
			get
			{
				return (from p in this.Ports
					where p.Kind == PortKind.Power
					orderby p.Index
					select p).ToList<RhubPort>();
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001D2E4 File Offset: 0x0001B4E4
		private RenderFragment Tile(RhubPort p, string prefix)
		{
			return delegate(RenderTreeBuilder __builder)
			{
				__builder.OpenElement(14, "span");
				__builder.AddAttribute(15, "class", "hub-tile " + (this.Compact ? "hub-tile-sm" : "") + " " + HubPorts.Css(p.State));
				__builder.AddAttribute(16, "title", HubPorts.Tip(p, prefix));
				int num = 17;
				string text;
				if (!this.Compact)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
					defaultInterpolatedStringHandler.AppendFormatted(prefix);
					defaultInterpolatedStringHandler.AppendFormatted<int>(p.Index);
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					text = "";
				}
				__builder.AddContent(num, text);
				__builder.CloseElement();
			};
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001D30C File Offset: 0x0001B50C
		private static string Css(PortState s)
		{
			string text;
			switch (s)
			{
			case PortState.Ok:
				text = "hub-ok";
				break;
			case PortState.Idle:
				text = "hub-idle";
				break;
			case PortState.Fault:
				text = "hub-fault";
				break;
			case PortState.Off:
				text = "hub-off";
				break;
			default:
				text = "hub-unknown";
				break;
			}
			return text;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001D358 File Offset: 0x0001B558
		private static string Tip(RhubPort p, string prefix)
		{
			string text = ((p.LastSeen == null) ? "no reading" : DeviceStatusView.Ago(new DateTime?(p.LastSeen.Value.UtcDateTime)));
			string text2 = (string.IsNullOrWhiteSpace(p.RawStatus) ? "" : (" — " + p.RawStatus));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 5);
			defaultInterpolatedStringHandler.AppendFormatted(prefix);
			defaultInterpolatedStringHandler.AppendFormatted<int>(p.Index);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<PortState>(p.State);
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
