using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	[Nullable(0)]
	public class LineChart : ComponentBase
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0001D454 File Offset: 0x0001B654
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "card p-5");
			__builder.OpenElement(2, "div");
			__builder.AddAttribute(3, "class", "mb-2 flex items-baseline justify-between gap-2");
			__builder.OpenElement(4, "h3");
			__builder.AddAttribute(5, "class", "text-sm font-semibold text-cocoa-900");
			__builder.AddContent(6, this.Title);
			__builder.CloseElement();
			if (!string.IsNullOrEmpty(this.Unit))
			{
				__builder.OpenElement(7, "span");
				__builder.AddAttribute(8, "class", "text-xs text-cocoa-700");
				__builder.AddContent(9, this.Unit);
				__builder.CloseElement();
			}
			__builder.CloseElement();
			if (this._hasLine)
			{
				__builder.OpenElement(10, "svg");
				__builder.AddAttribute(11, "class", "block w-full");
				__builder.AddAttribute(12, "viewBox", "0 0 640 210");
				__builder.AddAttribute(13, "style", "aspect-ratio:640/210;max-height:15rem");
				__builder.AddAttribute(14, "role", "group");
				__builder.AddAttribute(15, "aria-label", this.Title + " chart");
				foreach (LineChart.ValueTick valueTick in this._valueTicks)
				{
					__builder.OpenElement(16, "line");
					__builder.AddAttribute(17, "class", "chart-grid");
					__builder.AddAttribute(18, "x1", LineChart.F(46.0));
					__builder.AddAttribute(19, "y1", LineChart.F(valueTick.Y));
					__builder.AddAttribute(20, "x2", LineChart.F(634.0));
					__builder.AddAttribute(21, "y2", LineChart.F(valueTick.Y));
					__builder.AddAttribute(22, "vector-effect", "non-scaling-stroke");
					__builder.CloseElement();
					__builder.AddContent(23, LineChart.Label(40.0, valueTick.Y, "end", "middle", valueTick.Text, "chart-axis-text"));
				}
				foreach (LineChart.TimeTick timeTick in this._timeTicks)
				{
					__builder.OpenElement(24, "line");
					__builder.AddAttribute(25, "class", "chart-grid");
					__builder.AddAttribute(26, "x1", LineChart.F(timeTick.X));
					__builder.AddAttribute(27, "y1", LineChart.F(10.0));
					__builder.AddAttribute(28, "x2", LineChart.F(timeTick.X));
					__builder.AddAttribute(29, "y2", LineChart.F(176.0));
					__builder.AddAttribute(30, "vector-effect", "non-scaling-stroke");
					__builder.CloseElement();
					__builder.AddContent(31, LineChart.Label(timeTick.X, 192.0, timeTick.Anchor, "auto", timeTick.Text, "chart-axis-text"));
				}
				foreach (LineChart.Line line in this._lines.OrderBy<LineChart.Line, bool>((LineChart.Line x) => x.Emphasis))
				{
					__builder.OpenElement(32, "polyline");
					__builder.AddAttribute(33, "points", line.Poly);
					__builder.AddAttribute(34, "fill", "none");
					__builder.AddAttribute(35, "stroke", line.Color);
					__builder.AddAttribute(36, "stroke-width", line.Emphasis ? "2.5" : "1.5");
					__builder.AddAttribute(37, "vector-effect", "non-scaling-stroke");
					__builder.CloseElement();
				}
				foreach (LineChart.Column column in this._columns)
				{
					__builder.OpenElement(38, "g");
					__builder.AddAttribute(39, "class", "chart-point");
					__builder.OpenElement(40, "rect");
					__builder.AddAttribute(41, "class", "chart-hit");
					__builder.AddAttribute(42, "x", LineChart.F(column.Left));
					__builder.AddAttribute(43, "y", LineChart.F(10.0));
					__builder.AddAttribute(44, "width", LineChart.F(column.Right - column.Left));
					__builder.AddAttribute(45, "height", LineChart.F(166.0));
					__builder.CloseElement();
					__builder.AddMarkupContent(46, "\n                    ");
					__builder.OpenElement(47, "title");
					__builder.AddContent(48, column.Summary);
					__builder.CloseElement();
					__builder.AddMarkupContent(49, "\n                    ");
					__builder.OpenElement(50, "line");
					__builder.AddAttribute(51, "class", "chart-guide");
					__builder.AddAttribute(52, "x1", LineChart.F(column.X));
					__builder.AddAttribute(53, "y1", LineChart.F(10.0));
					__builder.AddAttribute(54, "x2", LineChart.F(column.X));
					__builder.AddAttribute(55, "y2", LineChart.F(176.0));
					__builder.AddAttribute(56, "vector-effect", "non-scaling-stroke");
					__builder.CloseElement();
					foreach (LineChart.Dot dot in column.Dots)
					{
						__builder.OpenElement(57, "circle");
						__builder.AddAttribute(58, "class", "chart-dot");
						__builder.AddAttribute(59, "cx", LineChart.F(column.X));
						__builder.AddAttribute(60, "cy", LineChart.F(dot.Y));
						__builder.AddAttribute(61, "r", "3");
						__builder.AddAttribute(62, "fill", dot.Color);
						__builder.CloseElement();
					}
					for (int i = 0; i < column.Tip.Count; i++)
					{
						__builder.AddContent(63, LineChart.Label(column.TipX, 22.0 + (double)(i * 13), column.TipAnchor, "auto", column.Tip[i], "chart-tip"));
					}
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.AddMarkupContent(64, "\n        ");
				__builder.OpenElement(65, "div");
				__builder.AddAttribute(66, "class", "mt-3 flex flex-wrap gap-x-4 gap-y-1 text-xs");
				foreach (LineChart.Line line2 in this._lines)
				{
					__builder.OpenElement(67, "span");
					__builder.AddAttribute(68, "class", "inline-flex items-center gap-1.5 text-cocoa-700");
					__builder.OpenElement(69, "span");
					__builder.AddAttribute(70, "class", "inline-block " + (line2.Emphasis ? "h-3 w-3" : "h-2.5 w-2.5") + " rounded-full");
					__builder.AddAttribute(71, "style", "background-color:" + line2.Color);
					__builder.CloseElement();
					__builder.AddMarkupContent(72, "\n                    ");
					__builder.OpenElement(73, "span");
					__builder.AddAttribute(74, "class", "font-medium text-cocoa-900");
					__builder.AddContent(75, line2.Label);
					__builder.CloseElement();
					__builder.AddMarkupContent(76, "\n                    ");
					__builder.OpenElement(77, "span");
					__builder.AddContent(78, line2.Last);
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
			}
			else
			{
				__builder.AddMarkupContent(79, "<div class=\"flex items-center justify-center text-xs text-cocoa-700\" style=\"aspect-ratio:640/210;max-height:15rem\">collecting…</div>");
			}
			__builder.CloseElement();
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001DD5C File Offset: 0x0001BF5C
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001DD64 File Offset: 0x0001BF64
		[Parameter]
		public string Title { get; set; } = "";

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001DD6D File Offset: 0x0001BF6D
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001DD75 File Offset: 0x0001BF75
		[Nullable(2)]
		[Parameter]
		public string Unit
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001DD7E File Offset: 0x0001BF7E
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0001DD86 File Offset: 0x0001BF86
		[Nullable(new byte[] { 2, 1 })]
		[Parameter]
		public List<ChartSeries> Series
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001DD8F File Offset: 0x0001BF8F
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0001DD97 File Offset: 0x0001BF97
		[Parameter]
		public DateTime? From { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001DDA0 File Offset: 0x0001BFA0
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0001DDA8 File Offset: 0x0001BFA8
		[Parameter]
		public DateTime? To { get; set; }

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
		protected override void OnParametersSet()
		{
			LineChart.<>c__DisplayClass37_0 CS$<>8__locals1 = new LineChart.<>c__DisplayClass37_0();
			this._lines = new List<LineChart.Line>();
			this._valueTicks = new List<LineChart.ValueTick>();
			this._timeTicks = new List<LineChart.TimeTick>();
			this._columns = new List<LineChart.Column>();
			this._hasLine = false;
			List<ChartSeries> series = this.Series;
			List<ChartSeries> list;
			if (series == null)
			{
				list = null;
			}
			else
			{
				list = series.Where<ChartSeries>(delegate(ChartSeries s)
				{
					List<HistoryPoint> points2 = s.Points;
					return points2 != null && points2.Count > 0;
				}).ToList<ChartSeries>();
			}
			List<ChartSeries> list2 = list ?? new List<ChartSeries>();
			if (list2.Count == 0)
			{
				return;
			}
			List<HistoryPoint> list3 = list2.SelectMany<ChartSeries, HistoryPoint>((ChartSeries s) => s.Points).ToList<HistoryPoint>();
			CS$<>8__locals1.axis = ChartAxis.Values(list3.Max<HistoryPoint>((HistoryPoint p) => p.Value));
			DateTime? dateTime = this.From;
			bool flag;
			if (dateTime != null)
			{
				DateTime valueOrDefault = dateTime.GetValueOrDefault();
				dateTime = this.To;
				if (dateTime != null)
				{
					DateTime valueOrDefault2 = dateTime.GetValueOrDefault();
					flag = valueOrDefault2 > valueOrDefault;
					goto IL_011D;
				}
			}
			flag = false;
			IL_011D:
			bool flag2 = flag;
			LineChart.<>c__DisplayClass37_0 CS$<>8__locals2 = CS$<>8__locals1;
			long num;
			if (!flag2)
			{
				num = list3.Min<HistoryPoint>((HistoryPoint p) => p.Ts.Ticks);
			}
			else
			{
				num = this.From.Value.Ticks;
			}
			CS$<>8__locals2.minT = num;
			long num2;
			if (!flag2)
			{
				num2 = list3.Max<HistoryPoint>((HistoryPoint p) => p.Ts.Ticks);
			}
			else
			{
				num2 = this.To.Value.Ticks;
			}
			long num3 = num2;
			CS$<>8__locals1.rangeT = (double)(num3 - CS$<>8__locals1.minT);
			foreach (double num4 in CS$<>8__locals1.axis.Ticks)
			{
				this._valueTicks.Add(new LineChart.ValueTick(CS$<>8__locals1.<OnParametersSet>g__Y|6(num4), ChartAxis.FormatValue(num4)));
			}
			foreach (DateTime dateTime2 in ChartAxis.TimeTicks(new DateTime(CS$<>8__locals1.minT), new DateTime(num3), 6))
			{
				double num5 = CS$<>8__locals1.<OnParametersSet>g__X|5(dateTime2.ToUniversalTime());
				string text = ((num5 <= 58.0) ? "start" : ((num5 >= 622.0) ? "end" : "middle"));
				this._timeTicks.Add(new LineChart.TimeTick(num5, dateTime2.ToString("HH:mm", CultureInfo.InvariantCulture), text));
			}
			foreach (ChartSeries chartSeries in list2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (HistoryPoint historyPoint in chartSeries.Points)
				{
					stringBuilder.Append(LineChart.F(CS$<>8__locals1.<OnParametersSet>g__X|5(historyPoint.Ts))).Append(',').Append(LineChart.F(CS$<>8__locals1.<OnParametersSet>g__Y|6(historyPoint.Value)))
						.Append(' ');
				}
				List<HistoryPoint> points = chartSeries.Points;
				string text2 = LineChart.Fmt(points[points.Count - 1].Value) + (string.IsNullOrEmpty(this.Unit) ? "" : (" " + this.Unit));
				this._lines.Add(new LineChart.Line(chartSeries.Color, stringBuilder.ToString().TrimEnd(), chartSeries.Label, text2, chartSeries.Points.Count, chartSeries.Emphasis));
			}
			this._hasLine = this._lines.Any<LineChart.Line>((LineChart.Line l) => l.Count >= 2);
			if (this._hasLine)
			{
				this.BuildColumns(list2, new Func<DateTime, double>(CS$<>8__locals1.<OnParametersSet>g__X|5), new Func<double, double>(CS$<>8__locals1.<OnParametersSet>g__Y|6));
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001E268 File Offset: 0x0001C468
		private void BuildColumns(List<ChartSeries> series, Func<DateTime, double> X, Func<double, double> Y)
		{
			List<DateTime> stamps = MetricHistory.Stamps(series.Select<ChartSeries, List<HistoryPoint>>((ChartSeries s) => s.Points));
			if (stamps.Count == 0)
			{
				return;
			}
			if (stamps.Count > 80)
			{
				int num = (int)Math.Ceiling((double)stamps.Count / 80.0);
				List<DateTime> list = new List<DateTime>();
				for (int i = 0; i < stamps.Count; i += num)
				{
					list.Add(stamps[i]);
				}
				List<DateTime> list2 = list;
				DateTime dateTime = list2[list2.Count - 1];
				List<DateTime> stamps3 = stamps;
				if (dateTime != stamps3[stamps3.Count - 1])
				{
					List<DateTime> list3 = list;
					List<DateTime> stamps2 = stamps;
					list3.Add(stamps2[stamps2.Count - 1]);
				}
				stamps = list;
			}
			List<double?[]> list4 = series.Select<ChartSeries, double?[]>((ChartSeries s) => MetricHistory.ValuesAt(s.Points, stamps)).ToList<double?[]>();
			List<double> list5 = stamps.Select<DateTime, double>(X).ToList<double>();
			string text = (string.IsNullOrEmpty(this.Unit) ? "" : (" " + this.Unit));
			double num2;
			if (list5.Count <= 1)
			{
				num2 = 588.0;
			}
			else
			{
				num2 = LineChart.Median(list5.Zip<double, double, double>(list5.Skip<double>(1), (double a, double b) => b - a).ToList<double>()) / 2.0;
			}
			double num3 = num2;
			for (int j = 0; j < stamps.Count; j++)
			{
				double num4 = list5[j];
				double num5 = ((j == 0) ? Math.Max(46.0, num4 - num3) : ((list5[j - 1] + num4) / 2.0));
				double num6 = ((j == stamps.Count - 1) ? Math.Min(634.0, num4 + num3) : ((num4 + list5[j + 1]) / 2.0));
				List<LineChart.Dot> list6 = new List<LineChart.Dot>();
				List<string> list7 = new List<string>();
				for (int k = 0; k < series.Count; k++)
				{
					double? num7 = list4[k][j];
					if (num7 != null)
					{
						double valueOrDefault = num7.GetValueOrDefault();
						list6.Add(new LineChart.Dot(Y(valueOrDefault), series[k].Color));
						list7.Add(series[k].Label + "  " + LineChart.Fmt(valueOrDefault) + text);
					}
				}
				string text2 = ChartAxis.ToLocal(stamps[j]).ToString("HH:mm:ss", CultureInfo.InvariantCulture);
				List<string> list8 = new List<string> { text2 };
				list8.AddRange(list7.Take<string>(8));
				if (list7.Count > 8)
				{
					List<string> list9 = list8;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
					defaultInterpolatedStringHandler.AppendLiteral("+");
					defaultInterpolatedStringHandler.AppendFormatted<int>(list7.Count - 8);
					defaultInterpolatedStringHandler.AppendLiteral(" more");
					list9.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = num4 > 340.0;
				this._columns.Add(new LineChart.Column(num4, num5, num6, flag ? (num4 - 8.0) : (num4 + 8.0), flag ? "end" : "start", list6, list8, string.Join(" · ", list8)));
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001E628 File Offset: 0x0001C828
		private static RenderFragment Label(double x, double y, string anchor, string baseline, string content, string cls = "chart-axis-text")
		{
			return delegate(RenderTreeBuilder builder)
			{
				builder.OpenElement(0, "text");
				builder.AddAttribute(1, "x", LineChart.F(x));
				builder.AddAttribute(2, "y", LineChart.F(y));
				builder.AddAttribute(3, "class", cls);
				builder.AddAttribute(4, "text-anchor", anchor);
				builder.AddAttribute(5, "dominant-baseline", baseline);
				builder.AddContent(6, content);
				builder.CloseElement();
			};
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001E668 File Offset: 0x0001C868
		private static double Median(List<double> values)
		{
			List<double> list = values.OrderBy<double, double>((double v) => v).ToList<double>();
			int num = list.Count / 2;
			if (list.Count % 2 != 1)
			{
				return (list[num - 1] + list[num]) / 2.0;
			}
			return list[num];
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001E6D7 File Offset: 0x0001C8D7
		private static string F(double v)
		{
			return v.ToString("0.#", CultureInfo.InvariantCulture);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001E6EA File Offset: 0x0001C8EA
		private static string Fmt(double v)
		{
			if (Math.Abs(v) < 1000.0)
			{
				return v.ToString("0.##", CultureInfo.InvariantCulture);
			}
			return v.ToString("0", CultureInfo.InvariantCulture);
		}

		// Token: 0x04000266 RID: 614
		private const double PlotLeft = 46.0;

		// Token: 0x04000267 RID: 615
		private const double PlotRight = 634.0;

		// Token: 0x04000268 RID: 616
		private const double PlotTop = 10.0;

		// Token: 0x04000269 RID: 617
		private const double PlotBottom = 176.0;

		// Token: 0x0400026A RID: 618
		private const int MaxColumns = 80;

		// Token: 0x0400026B RID: 619
		private const int MaxTipLines = 8;

		// Token: 0x0400026C RID: 620
		private List<LineChart.Line> _lines = new List<LineChart.Line>();

		// Token: 0x0400026D RID: 621
		private List<LineChart.ValueTick> _valueTicks = new List<LineChart.ValueTick>();

		// Token: 0x0400026E RID: 622
		private List<LineChart.TimeTick> _timeTicks = new List<LineChart.TimeTick>();

		// Token: 0x0400026F RID: 623
		private List<LineChart.Column> _columns = new List<LineChart.Column>();

		// Token: 0x04000270 RID: 624
		private bool _hasLine;

		// Token: 0x02000175 RID: 373
		[Nullable(0)]
		private sealed class Line : IEquatable<LineChart.Line>
		{
			// Token: 0x06000C0D RID: 3085 RVA: 0x00051DE6 File Offset: 0x0004FFE6
			public Line(string Color, string Poly, string Label, string Last, int Count, bool Emphasis)
			{
				this.Color = Color;
				this.Poly = Poly;
				this.Label = Label;
				this.Last = Last;
				this.Count = Count;
				this.Emphasis = Emphasis;
				base..ctor();
			}

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00051E1B File Offset: 0x0005001B
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(LineChart.Line);
				}
			}

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00051E27 File Offset: 0x00050027
			// (set) Token: 0x06000C10 RID: 3088 RVA: 0x00051E2F File Offset: 0x0005002F
			public string Color { get; set; }

			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00051E38 File Offset: 0x00050038
			// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00051E40 File Offset: 0x00050040
			public string Poly { get; set; }

			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00051E49 File Offset: 0x00050049
			// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00051E51 File Offset: 0x00050051
			public string Label { get; set; }

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00051E5A File Offset: 0x0005005A
			// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00051E62 File Offset: 0x00050062
			public string Last { get; set; }

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00051E6B File Offset: 0x0005006B
			// (set) Token: 0x06000C18 RID: 3096 RVA: 0x00051E73 File Offset: 0x00050073
			public int Count { get; set; }

			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00051E7C File Offset: 0x0005007C
			// (set) Token: 0x06000C1A RID: 3098 RVA: 0x00051E84 File Offset: 0x00050084
			public bool Emphasis { get; set; }

			// Token: 0x06000C1B RID: 3099 RVA: 0x00051E90 File Offset: 0x00050090
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Line");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000C1C RID: 3100 RVA: 0x00051EDC File Offset: 0x000500DC
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Color = ");
				builder.Append(this.Color);
				builder.Append(", Poly = ");
				builder.Append(this.Poly);
				builder.Append(", Label = ");
				builder.Append(this.Label);
				builder.Append(", Last = ");
				builder.Append(this.Last);
				builder.Append(", Count = ");
				builder.Append(this.Count.ToString());
				builder.Append(", Emphasis = ");
				builder.Append(this.Emphasis.ToString());
				return true;
			}

			// Token: 0x06000C1D RID: 3101 RVA: 0x00051FA1 File Offset: 0x000501A1
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(LineChart.Line left, LineChart.Line right)
			{
				return !(left == right);
			}

			// Token: 0x06000C1E RID: 3102 RVA: 0x00051FAD File Offset: 0x000501AD
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(LineChart.Line left, LineChart.Line right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000C1F RID: 3103 RVA: 0x00051FC4 File Offset: 0x000501C4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Color>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Poly>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Last>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Count>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Emphasis>k__BackingField);
			}

			// Token: 0x06000C20 RID: 3104 RVA: 0x0005206B File Offset: 0x0005026B
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as LineChart.Line);
			}

			// Token: 0x06000C21 RID: 3105 RVA: 0x0005207C File Offset: 0x0005027C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(LineChart.Line other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Color>k__BackingField, other.<Color>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Poly>k__BackingField, other.<Poly>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Last>k__BackingField, other.<Last>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Count>k__BackingField, other.<Count>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Emphasis>k__BackingField, other.<Emphasis>k__BackingField));
			}

			// Token: 0x06000C23 RID: 3107 RVA: 0x00052148 File Offset: 0x00050348
			[CompilerGenerated]
			private Line(LineChart.Line original)
			{
				this.Color = original.<Color>k__BackingField;
				this.Poly = original.<Poly>k__BackingField;
				this.Label = original.<Label>k__BackingField;
				this.Last = original.<Last>k__BackingField;
				this.Count = original.<Count>k__BackingField;
				this.Emphasis = original.<Emphasis>k__BackingField;
			}

			// Token: 0x06000C24 RID: 3108 RVA: 0x000521A3 File Offset: 0x000503A3
			[CompilerGenerated]
			public void Deconstruct(out string Color, out string Poly, out string Label, out string Last, out int Count, out bool Emphasis)
			{
				Color = this.Color;
				Poly = this.Poly;
				Label = this.Label;
				Last = this.Last;
				Count = this.Count;
				Emphasis = this.Emphasis;
			}
		}

		// Token: 0x02000176 RID: 374
		[Nullable(0)]
		private sealed class ValueTick : IEquatable<LineChart.ValueTick>
		{
			// Token: 0x06000C25 RID: 3109 RVA: 0x000521D8 File Offset: 0x000503D8
			public ValueTick(double Y, string Text)
			{
				this.Y = Y;
				this.Text = Text;
				base..ctor();
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000521EE File Offset: 0x000503EE
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(LineChart.ValueTick);
				}
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000C27 RID: 3111 RVA: 0x000521FA File Offset: 0x000503FA
			// (set) Token: 0x06000C28 RID: 3112 RVA: 0x00052202 File Offset: 0x00050402
			public double Y { get; set; }

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0005220B File Offset: 0x0005040B
			// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00052213 File Offset: 0x00050413
			public string Text { get; set; }

			// Token: 0x06000C2B RID: 3115 RVA: 0x0005221C File Offset: 0x0005041C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ValueTick");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000C2C RID: 3116 RVA: 0x00052268 File Offset: 0x00050468
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Y = ");
				builder.Append(this.Y.ToString());
				builder.Append(", Text = ");
				builder.Append(this.Text);
				return true;
			}

			// Token: 0x06000C2D RID: 3117 RVA: 0x000522BB File Offset: 0x000504BB
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(LineChart.ValueTick left, LineChart.ValueTick right)
			{
				return !(left == right);
			}

			// Token: 0x06000C2E RID: 3118 RVA: 0x000522C7 File Offset: 0x000504C7
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(LineChart.ValueTick left, LineChart.ValueTick right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000C2F RID: 3119 RVA: 0x000522DB File Offset: 0x000504DB
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Y>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Text>k__BackingField);
			}

			// Token: 0x06000C30 RID: 3120 RVA: 0x0005231B File Offset: 0x0005051B
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as LineChart.ValueTick);
			}

			// Token: 0x06000C31 RID: 3121 RVA: 0x0005232C File Offset: 0x0005052C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(LineChart.ValueTick other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.<Y>k__BackingField, other.<Y>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Text>k__BackingField, other.<Text>k__BackingField));
			}

			// Token: 0x06000C33 RID: 3123 RVA: 0x0005238D File Offset: 0x0005058D
			[CompilerGenerated]
			private ValueTick(LineChart.ValueTick original)
			{
				this.Y = original.<Y>k__BackingField;
				this.Text = original.<Text>k__BackingField;
			}

			// Token: 0x06000C34 RID: 3124 RVA: 0x000523AD File Offset: 0x000505AD
			[CompilerGenerated]
			public void Deconstruct(out double Y, out string Text)
			{
				Y = this.Y;
				Text = this.Text;
			}
		}

		// Token: 0x02000177 RID: 375
		[Nullable(0)]
		private sealed class TimeTick : IEquatable<LineChart.TimeTick>
		{
			// Token: 0x06000C35 RID: 3125 RVA: 0x000523BF File Offset: 0x000505BF
			public TimeTick(double X, string Text, string Anchor)
			{
				this.X = X;
				this.Text = Text;
				this.Anchor = Anchor;
				base..ctor();
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06000C36 RID: 3126 RVA: 0x000523DC File Offset: 0x000505DC
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(LineChart.TimeTick);
				}
			}

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06000C37 RID: 3127 RVA: 0x000523E8 File Offset: 0x000505E8
			// (set) Token: 0x06000C38 RID: 3128 RVA: 0x000523F0 File Offset: 0x000505F0
			public double X { get; set; }

			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000C39 RID: 3129 RVA: 0x000523F9 File Offset: 0x000505F9
			// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00052401 File Offset: 0x00050601
			public string Text { get; set; }

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0005240A File Offset: 0x0005060A
			// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00052412 File Offset: 0x00050612
			public string Anchor { get; set; }

			// Token: 0x06000C3D RID: 3133 RVA: 0x0005241C File Offset: 0x0005061C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("TimeTick");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000C3E RID: 3134 RVA: 0x00052468 File Offset: 0x00050668
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("X = ");
				builder.Append(this.X.ToString());
				builder.Append(", Text = ");
				builder.Append(this.Text);
				builder.Append(", Anchor = ");
				builder.Append(this.Anchor);
				return true;
			}

			// Token: 0x06000C3F RID: 3135 RVA: 0x000524D4 File Offset: 0x000506D4
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(LineChart.TimeTick left, LineChart.TimeTick right)
			{
				return !(left == right);
			}

			// Token: 0x06000C40 RID: 3136 RVA: 0x000524E0 File Offset: 0x000506E0
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(LineChart.TimeTick left, LineChart.TimeTick right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000C41 RID: 3137 RVA: 0x000524F4 File Offset: 0x000506F4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<X>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Text>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Anchor>k__BackingField);
			}

			// Token: 0x06000C42 RID: 3138 RVA: 0x00052556 File Offset: 0x00050756
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as LineChart.TimeTick);
			}

			// Token: 0x06000C43 RID: 3139 RVA: 0x00052564 File Offset: 0x00050764
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(LineChart.TimeTick other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.<X>k__BackingField, other.<X>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Text>k__BackingField, other.<Text>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Anchor>k__BackingField, other.<Anchor>k__BackingField));
			}

			// Token: 0x06000C45 RID: 3141 RVA: 0x000525DD File Offset: 0x000507DD
			[CompilerGenerated]
			private TimeTick(LineChart.TimeTick original)
			{
				this.X = original.<X>k__BackingField;
				this.Text = original.<Text>k__BackingField;
				this.Anchor = original.<Anchor>k__BackingField;
			}

			// Token: 0x06000C46 RID: 3142 RVA: 0x00052609 File Offset: 0x00050809
			[CompilerGenerated]
			public void Deconstruct(out double X, out string Text, out string Anchor)
			{
				X = this.X;
				Text = this.Text;
				Anchor = this.Anchor;
			}
		}

		// Token: 0x02000178 RID: 376
		[Nullable(0)]
		private sealed class Dot : IEquatable<LineChart.Dot>
		{
			// Token: 0x06000C47 RID: 3143 RVA: 0x00052623 File Offset: 0x00050823
			public Dot(double Y, string Color)
			{
				this.Y = Y;
				this.Color = Color;
				base..ctor();
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00052639 File Offset: 0x00050839
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(LineChart.Dot);
				}
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00052645 File Offset: 0x00050845
			// (set) Token: 0x06000C4A RID: 3146 RVA: 0x0005264D File Offset: 0x0005084D
			public double Y { get; set; }

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00052656 File Offset: 0x00050856
			// (set) Token: 0x06000C4C RID: 3148 RVA: 0x0005265E File Offset: 0x0005085E
			public string Color { get; set; }

			// Token: 0x06000C4D RID: 3149 RVA: 0x00052668 File Offset: 0x00050868
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Dot");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000C4E RID: 3150 RVA: 0x000526B4 File Offset: 0x000508B4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Y = ");
				builder.Append(this.Y.ToString());
				builder.Append(", Color = ");
				builder.Append(this.Color);
				return true;
			}

			// Token: 0x06000C4F RID: 3151 RVA: 0x00052707 File Offset: 0x00050907
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(LineChart.Dot left, LineChart.Dot right)
			{
				return !(left == right);
			}

			// Token: 0x06000C50 RID: 3152 RVA: 0x00052713 File Offset: 0x00050913
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(LineChart.Dot left, LineChart.Dot right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000C51 RID: 3153 RVA: 0x00052727 File Offset: 0x00050927
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Y>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Color>k__BackingField);
			}

			// Token: 0x06000C52 RID: 3154 RVA: 0x00052767 File Offset: 0x00050967
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as LineChart.Dot);
			}

			// Token: 0x06000C53 RID: 3155 RVA: 0x00052778 File Offset: 0x00050978
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(LineChart.Dot other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.<Y>k__BackingField, other.<Y>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Color>k__BackingField, other.<Color>k__BackingField));
			}

			// Token: 0x06000C55 RID: 3157 RVA: 0x000527D9 File Offset: 0x000509D9
			[CompilerGenerated]
			private Dot(LineChart.Dot original)
			{
				this.Y = original.<Y>k__BackingField;
				this.Color = original.<Color>k__BackingField;
			}

			// Token: 0x06000C56 RID: 3158 RVA: 0x000527F9 File Offset: 0x000509F9
			[CompilerGenerated]
			public void Deconstruct(out double Y, out string Color)
			{
				Y = this.Y;
				Color = this.Color;
			}
		}

		// Token: 0x02000179 RID: 377
		[Nullable(0)]
		private sealed class Column : IEquatable<LineChart.Column>
		{
			// Token: 0x06000C57 RID: 3159 RVA: 0x0005280C File Offset: 0x00050A0C
			public Column(double X, double Left, double Right, double TipX, string TipAnchor, List<LineChart.Dot> Dots, List<string> Tip, string Summary)
			{
				this.X = X;
				this.Left = Left;
				this.Right = Right;
				this.TipX = TipX;
				this.TipAnchor = TipAnchor;
				this.Dots = Dots;
				this.Tip = Tip;
				this.Summary = Summary;
				base..ctor();
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0005285C File Offset: 0x00050A5C
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(LineChart.Column);
				}
			}

			// Token: 0x170002AE RID: 686
			// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00052868 File Offset: 0x00050A68
			// (set) Token: 0x06000C5A RID: 3162 RVA: 0x00052870 File Offset: 0x00050A70
			public double X { get; set; }

			// Token: 0x170002AF RID: 687
			// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00052879 File Offset: 0x00050A79
			// (set) Token: 0x06000C5C RID: 3164 RVA: 0x00052881 File Offset: 0x00050A81
			public double Left { get; set; }

			// Token: 0x170002B0 RID: 688
			// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0005288A File Offset: 0x00050A8A
			// (set) Token: 0x06000C5E RID: 3166 RVA: 0x00052892 File Offset: 0x00050A92
			public double Right { get; set; }

			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0005289B File Offset: 0x00050A9B
			// (set) Token: 0x06000C60 RID: 3168 RVA: 0x000528A3 File Offset: 0x00050AA3
			public double TipX { get; set; }

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x06000C61 RID: 3169 RVA: 0x000528AC File Offset: 0x00050AAC
			// (set) Token: 0x06000C62 RID: 3170 RVA: 0x000528B4 File Offset: 0x00050AB4
			public string TipAnchor { get; set; }

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000528BD File Offset: 0x00050ABD
			// (set) Token: 0x06000C64 RID: 3172 RVA: 0x000528C5 File Offset: 0x00050AC5
			public List<LineChart.Dot> Dots { get; set; }

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06000C65 RID: 3173 RVA: 0x000528CE File Offset: 0x00050ACE
			// (set) Token: 0x06000C66 RID: 3174 RVA: 0x000528D6 File Offset: 0x00050AD6
			public List<string> Tip { get; set; }

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000C67 RID: 3175 RVA: 0x000528DF File Offset: 0x00050ADF
			// (set) Token: 0x06000C68 RID: 3176 RVA: 0x000528E7 File Offset: 0x00050AE7
			public string Summary { get; set; }

			// Token: 0x06000C69 RID: 3177 RVA: 0x000528F0 File Offset: 0x00050AF0
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Column");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000C6A RID: 3178 RVA: 0x0005293C File Offset: 0x00050B3C
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("X = ");
				builder.Append(this.X.ToString());
				builder.Append(", Left = ");
				builder.Append(this.Left.ToString());
				builder.Append(", Right = ");
				builder.Append(this.Right.ToString());
				builder.Append(", TipX = ");
				builder.Append(this.TipX.ToString());
				builder.Append(", TipAnchor = ");
				builder.Append(this.TipAnchor);
				builder.Append(", Dots = ");
				builder.Append(this.Dots);
				builder.Append(", Tip = ");
				builder.Append(this.Tip);
				builder.Append(", Summary = ");
				builder.Append(this.Summary);
				return true;
			}

			// Token: 0x06000C6B RID: 3179 RVA: 0x00052A4F File Offset: 0x00050C4F
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(LineChart.Column left, LineChart.Column right)
			{
				return !(left == right);
			}

			// Token: 0x06000C6C RID: 3180 RVA: 0x00052A5B File Offset: 0x00050C5B
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(LineChart.Column left, LineChart.Column right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000C6D RID: 3181 RVA: 0x00052A70 File Offset: 0x00050C70
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<X>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Left>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Right>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<TipX>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<TipAnchor>k__BackingField)) * -1521134295 + EqualityComparer<List<LineChart.Dot>>.Default.GetHashCode(this.<Dots>k__BackingField)) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.<Tip>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Summary>k__BackingField);
			}

			// Token: 0x06000C6E RID: 3182 RVA: 0x00052B45 File Offset: 0x00050D45
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as LineChart.Column);
			}

			// Token: 0x06000C6F RID: 3183 RVA: 0x00052B54 File Offset: 0x00050D54
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(LineChart.Column other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<double>.Default.Equals(this.<X>k__BackingField, other.<X>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Left>k__BackingField, other.<Left>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Right>k__BackingField, other.<Right>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<TipX>k__BackingField, other.<TipX>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<TipAnchor>k__BackingField, other.<TipAnchor>k__BackingField) && EqualityComparer<List<LineChart.Dot>>.Default.Equals(this.<Dots>k__BackingField, other.<Dots>k__BackingField) && EqualityComparer<List<string>>.Default.Equals(this.<Tip>k__BackingField, other.<Tip>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Summary>k__BackingField, other.<Summary>k__BackingField));
			}

			// Token: 0x06000C71 RID: 3185 RVA: 0x00052C54 File Offset: 0x00050E54
			[CompilerGenerated]
			private Column(LineChart.Column original)
			{
				this.X = original.<X>k__BackingField;
				this.Left = original.<Left>k__BackingField;
				this.Right = original.<Right>k__BackingField;
				this.TipX = original.<TipX>k__BackingField;
				this.TipAnchor = original.<TipAnchor>k__BackingField;
				this.Dots = original.<Dots>k__BackingField;
				this.Tip = original.<Tip>k__BackingField;
				this.Summary = original.<Summary>k__BackingField;
			}

			// Token: 0x06000C72 RID: 3186 RVA: 0x00052CC8 File Offset: 0x00050EC8
			[CompilerGenerated]
			public void Deconstruct(out double X, out double Left, out double Right, out double TipX, out string TipAnchor, out List<LineChart.Dot> Dots, out List<string> Tip, out string Summary)
			{
				X = this.X;
				Left = this.Left;
				Right = this.Right;
				TipX = this.TipX;
				TipAnchor = this.TipAnchor;
				Dots = this.Dots;
				Tip = this.Tip;
				Summary = this.Summary;
			}
		}
	}
}
