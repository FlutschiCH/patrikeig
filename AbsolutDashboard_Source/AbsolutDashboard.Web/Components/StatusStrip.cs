using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x0200009C RID: 156
	[NullableContext(1)]
	[Nullable(0)]
	public class StatusStrip : ComponentBase
	{
		// Token: 0x06000628 RID: 1576 RVA: 0x00020530 File Offset: 0x0001E730
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card p-5");
			__builder.OpenElement(2, "div");
			__builder.AddAttribute(3, "class", "mb-3 flex items-baseline justify-between gap-2");
			__builder.OpenElement(4, "h3");
			__builder.AddAttribute(5, "class", "text-sm font-semibold text-cocoa-900");
			__builder.AddContent(6, this.Title);
			__builder.CloseElement();
			__builder.AddMarkupContent(7, "\n        ");
			__builder.OpenElement(8, "span");
			__builder.AddAttribute(9, "class", "text-xs text-cocoa-700");
			int num = 10;
			string text;
			if ((text = this.Subtitle) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Rows.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" device");
				defaultInterpolatedStringHandler.AppendFormatted((this.Rows.Count == 1) ? "" : "s");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			__builder.AddContent(num, text);
			__builder.CloseElement();
			__builder.CloseElement();
			if (this.Rows.Count > 0 && this.To > this.From)
			{
				__builder.OpenElement(11, "div");
				__builder.AddAttribute(12, "class", "space-y-1.5");
				foreach (StatusStrip.Row row in this.Rows)
				{
					__builder.OpenElement(13, "div");
					__builder.AddAttribute(14, "class", "flex items-center gap-3");
					__builder.OpenElement(15, "span");
					__builder.AddAttribute(16, "class", "w-36 shrink-0 truncate text-xs text-cocoa-700");
					__builder.AddAttribute(17, "title", row.Label);
					__builder.AddContent(18, row.Label);
					__builder.CloseElement();
					__builder.AddMarkupContent(19, "\n                    ");
					__builder.OpenElement(20, "div");
					__builder.AddAttribute(21, "class", "strip-track");
					foreach (LinkSegment linkSegment in row.Segments)
					{
						__builder.OpenElement(22, "div");
						__builder.AddAttribute(23, "class", "strip-seg " + StatusStrip.Css(linkSegment.State));
						__builder.AddAttribute(24, "style", "width:" + this.Pct(linkSegment) + "%");
						__builder.AddAttribute(25, "title", StatusStrip.Describe(row.Label, linkSegment));
						__builder.CloseElement();
					}
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.OpenElement(26, "div");
				__builder.AddAttribute(27, "class", "mt-1 flex items-start gap-3");
				__builder.AddMarkupContent(28, "<span class=\"w-36 shrink-0\"></span>\n            ");
				__builder.OpenElement(29, "div");
				__builder.AddAttribute(30, "class", "strip-ticks");
				foreach (StatusStrip.Tick tick in this.TimeTicks())
				{
					__builder.OpenElement(31, "span");
					__builder.AddAttribute(32, "class", "strip-tick");
					__builder.AddAttribute(33, "style", string.Concat(new string[]
					{
						"left:",
						StatusStrip.F(tick.Pct),
						"%;transform:translateX(",
						tick.Shift,
						")"
					}));
					__builder.AddContent(34, tick.Text);
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(35, "<div class=\"mt-3 flex flex-wrap items-center gap-x-4 gap-y-1 text-xs text-cocoa-700\"><span class=\"inline-flex items-center gap-1.5\"><span class=\"strip-key strip-connected\"></span> connected</span>\n            <span class=\"inline-flex items-center gap-1.5\"><span class=\"strip-key strip-idle\"></span> idle</span>\n            <span class=\"inline-flex items-center gap-1.5\"><span class=\"strip-key strip-unreachable\"></span> disconnected</span>\n            <span class=\"inline-flex items-center gap-1.5\"><span class=\"strip-key strip-nodata\"></span> no data</span></div>");
			}
			else
			{
				__builder.OpenElement(36, "div");
				__builder.AddAttribute(37, "class", "flex h-16 items-center justify-center text-xs text-cocoa-700");
				__builder.AddContent(38, this.EmptyText);
				__builder.CloseElement();
			}
			__builder.CloseElement();
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000209A4 File Offset: 0x0001EBA4
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x000209AC File Offset: 0x0001EBAC
		[Parameter]
		public string Title { get; set; } = "";

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x000209B5 File Offset: 0x0001EBB5
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x000209BD File Offset: 0x0001EBBD
		[Parameter]
		public List<StatusStrip.Row> Rows { get; set; } = new List<StatusStrip.Row>();

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000209C6 File Offset: 0x0001EBC6
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x000209CE File Offset: 0x0001EBCE
		[Nullable(2)]
		[Parameter]
		public string Subtitle
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000209D7 File Offset: 0x0001EBD7
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x000209DF File Offset: 0x0001EBDF
		[Parameter]
		public string EmptyText { get; set; } = "collecting…";

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000209E8 File Offset: 0x0001EBE8
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x000209F0 File Offset: 0x0001EBF0
		[Parameter]
		public DateTime From { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000209F9 File Offset: 0x0001EBF9
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x00020A01 File Offset: 0x0001EC01
		[Parameter]
		public DateTime To { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00020A0C File Offset: 0x0001EC0C
		private double WindowTicks
		{
			get
			{
				return (double)Math.Max(1L, (this.To - this.From).Ticks);
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00020A3C File Offset: 0x0001EC3C
		private string Pct(LinkSegment seg)
		{
			return StatusStrip.F(Math.Clamp((double)(seg.ToUtc - seg.FromUtc).Ticks / this.WindowTicks * 100.0, 0.0, 100.0));
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00020A90 File Offset: 0x0001EC90
		private List<StatusStrip.Tick> TimeTicks()
		{
			List<StatusStrip.Tick> list = new List<StatusStrip.Tick>();
			foreach (DateTime dateTime in ChartAxis.TimeTicks(this.From, this.To, 6))
			{
				double num = Math.Clamp((double)(dateTime.ToUniversalTime() - this.From).Ticks / this.WindowTicks * 100.0, 0.0, 100.0);
				string text = ((num <= 2.0) ? "0" : ((num >= 98.0) ? "-100%" : "-50%"));
				list.Add(new StatusStrip.Tick(num, dateTime.ToString("HH:mm", CultureInfo.InvariantCulture), text));
			}
			return list;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00020B88 File Offset: 0x0001ED88
		private static string Css(LinkState s)
		{
			string text;
			switch (s)
			{
			case LinkState.Connected:
				text = "strip-connected";
				break;
			case LinkState.Idle:
				text = "strip-idle";
				break;
			case LinkState.Unreachable:
				text = "strip-unreachable";
				break;
			default:
				text = "strip-nodata";
				break;
			}
			return text;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00020BC8 File Offset: 0x0001EDC8
		private static string Describe(string label, LinkSegment seg)
		{
			string text = ChartAxis.ToLocal(seg.FromUtc).ToString("HH:mm", CultureInfo.InvariantCulture);
			string text2 = ChartAxis.ToLocal(seg.ToUtc).ToString("HH:mm", CultureInfo.InvariantCulture);
			TimeSpan timeSpan = seg.ToUtc - seg.FromUtc;
			string text3;
			if (timeSpan.TotalHours < 1.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(Math.Max(1, (int)timeSpan.TotalMinutes));
				defaultInterpolatedStringHandler.AppendLiteral(" min");
				text3 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(7, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<int>((int)timeSpan.TotalHours);
				defaultInterpolatedStringHandler2.AppendLiteral(" h ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(timeSpan.Minutes);
				defaultInterpolatedStringHandler2.AppendLiteral(" min");
				text3 = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			string text4 = text3;
			string text5 = ((seg.State == LinkState.NoData) ? "No data" : seg.State.ToString());
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(10, 5);
			defaultInterpolatedStringHandler3.AppendFormatted(label);
			defaultInterpolatedStringHandler3.AppendLiteral(" — ");
			defaultInterpolatedStringHandler3.AppendFormatted(text5);
			defaultInterpolatedStringHandler3.AppendLiteral(" · ");
			defaultInterpolatedStringHandler3.AppendFormatted(text);
			defaultInterpolatedStringHandler3.AppendLiteral("–");
			defaultInterpolatedStringHandler3.AppendFormatted(text2);
			defaultInterpolatedStringHandler3.AppendLiteral(" (");
			defaultInterpolatedStringHandler3.AppendFormatted(text4);
			defaultInterpolatedStringHandler3.AppendLiteral(")");
			return defaultInterpolatedStringHandler3.ToStringAndClear();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00020D4D File Offset: 0x0001EF4D
		private static string F(double v)
		{
			return v.ToString("0.##", CultureInfo.InvariantCulture);
		}

		// Token: 0x02000187 RID: 391
		[Nullable(0)]
		public sealed class Row : IEquatable<StatusStrip.Row>
		{
			// Token: 0x06000C9B RID: 3227 RVA: 0x000539AF File Offset: 0x00051BAF
			public Row(string Label, List<LinkSegment> Segments)
			{
				this.Label = Label;
				this.Segments = Segments;
				base..ctor();
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000C9C RID: 3228 RVA: 0x000539C5 File Offset: 0x00051BC5
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(StatusStrip.Row);
				}
			}

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06000C9D RID: 3229 RVA: 0x000539D1 File Offset: 0x00051BD1
			// (set) Token: 0x06000C9E RID: 3230 RVA: 0x000539D9 File Offset: 0x00051BD9
			public string Label { get; set; }

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000C9F RID: 3231 RVA: 0x000539E2 File Offset: 0x00051BE2
			// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x000539EA File Offset: 0x00051BEA
			public List<LinkSegment> Segments { get; set; }

			// Token: 0x06000CA1 RID: 3233 RVA: 0x000539F4 File Offset: 0x00051BF4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Row");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000CA2 RID: 3234 RVA: 0x00053A40 File Offset: 0x00051C40
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Label = ");
				builder.Append(this.Label);
				builder.Append(", Segments = ");
				builder.Append(this.Segments);
				return true;
			}

			// Token: 0x06000CA3 RID: 3235 RVA: 0x00053A7A File Offset: 0x00051C7A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(StatusStrip.Row left, StatusStrip.Row right)
			{
				return !(left == right);
			}

			// Token: 0x06000CA4 RID: 3236 RVA: 0x00053A86 File Offset: 0x00051C86
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(StatusStrip.Row left, StatusStrip.Row right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000CA5 RID: 3237 RVA: 0x00053A9A File Offset: 0x00051C9A
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<List<LinkSegment>>.Default.GetHashCode(this.<Segments>k__BackingField);
			}

			// Token: 0x06000CA6 RID: 3238 RVA: 0x00053ADA File Offset: 0x00051CDA
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as StatusStrip.Row);
			}

			// Token: 0x06000CA7 RID: 3239 RVA: 0x00053AE8 File Offset: 0x00051CE8
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(StatusStrip.Row other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<List<LinkSegment>>.Default.Equals(this.<Segments>k__BackingField, other.<Segments>k__BackingField));
			}

			// Token: 0x06000CA9 RID: 3241 RVA: 0x00053B49 File Offset: 0x00051D49
			[CompilerGenerated]
			private Row(StatusStrip.Row original)
			{
				this.Label = original.<Label>k__BackingField;
				this.Segments = original.<Segments>k__BackingField;
			}

			// Token: 0x06000CAA RID: 3242 RVA: 0x00053B69 File Offset: 0x00051D69
			[CompilerGenerated]
			public void Deconstruct(out string Label, out List<LinkSegment> Segments)
			{
				Label = this.Label;
				Segments = this.Segments;
			}
		}

		// Token: 0x02000188 RID: 392
		[Nullable(0)]
		private sealed class Tick : IEquatable<StatusStrip.Tick>
		{
			// Token: 0x06000CAB RID: 3243 RVA: 0x00053B7B File Offset: 0x00051D7B
			public Tick(double Pct, string Text, string Shift)
			{
				this.Pct = Pct;
				this.Text = Text;
				this.Shift = Shift;
				base..ctor();
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00053B98 File Offset: 0x00051D98
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(StatusStrip.Tick);
				}
			}

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00053BA4 File Offset: 0x00051DA4
			// (set) Token: 0x06000CAE RID: 3246 RVA: 0x00053BAC File Offset: 0x00051DAC
			public double Pct { get; set; }

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00053BB5 File Offset: 0x00051DB5
			// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x00053BBD File Offset: 0x00051DBD
			public string Text { get; set; }

			// Token: 0x170002BC RID: 700
			// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00053BC6 File Offset: 0x00051DC6
			// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x00053BCE File Offset: 0x00051DCE
			public string Shift { get; set; }

			// Token: 0x06000CB3 RID: 3251 RVA: 0x00053BD8 File Offset: 0x00051DD8
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Tick");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000CB4 RID: 3252 RVA: 0x00053C24 File Offset: 0x00051E24
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Pct = ");
				builder.Append(this.Pct.ToString());
				builder.Append(", Text = ");
				builder.Append(this.Text);
				builder.Append(", Shift = ");
				builder.Append(this.Shift);
				return true;
			}

			// Token: 0x06000CB5 RID: 3253 RVA: 0x00053C90 File Offset: 0x00051E90
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(StatusStrip.Tick left, StatusStrip.Tick right)
			{
				return !(left == right);
			}

			// Token: 0x06000CB6 RID: 3254 RVA: 0x00053C9C File Offset: 0x00051E9C
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(StatusStrip.Tick left, StatusStrip.Tick right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000CB7 RID: 3255 RVA: 0x00053CB0 File Offset: 0x00051EB0
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Pct>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Text>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Shift>k__BackingField);
			}

			// Token: 0x06000CB8 RID: 3256 RVA: 0x00053D12 File Offset: 0x00051F12
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as StatusStrip.Tick);
			}

			// Token: 0x06000CB9 RID: 3257 RVA: 0x00053D20 File Offset: 0x00051F20
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(StatusStrip.Tick other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.<Pct>k__BackingField, other.<Pct>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Text>k__BackingField, other.<Text>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Shift>k__BackingField, other.<Shift>k__BackingField));
			}

			// Token: 0x06000CBB RID: 3259 RVA: 0x00053D99 File Offset: 0x00051F99
			[CompilerGenerated]
			private Tick(StatusStrip.Tick original)
			{
				this.Pct = original.<Pct>k__BackingField;
				this.Text = original.<Text>k__BackingField;
				this.Shift = original.<Shift>k__BackingField;
			}

			// Token: 0x06000CBC RID: 3260 RVA: 0x00053DC5 File Offset: 0x00051FC5
			[CompilerGenerated]
			public void Deconstruct(out double Pct, out string Text, out string Shift)
			{
				Pct = this.Pct;
				Text = this.Text;
				Shift = this.Shift;
			}
		}
	}
}
