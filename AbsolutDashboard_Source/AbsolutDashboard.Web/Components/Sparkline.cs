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
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	[Nullable(0)]
	public class Sparkline : ComponentBase
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0001FF2C File Offset: 0x0001E12C
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "rounded-xl bg-cream-50 p-3 ring-1 ring-blush-100");
			__builder.OpenElement(2, "div");
			__builder.AddAttribute(3, "class", "flex items-baseline justify-between gap-2");
			__builder.OpenElement(4, "span");
			__builder.AddAttribute(5, "class", "truncate text-xs font-medium text-cocoa-700");
			__builder.AddAttribute(6, "title", this.Title);
			__builder.AddContent(7, this.Title);
			__builder.CloseElement();
			__builder.AddMarkupContent(8, "\n        ");
			__builder.OpenElement(9, "span");
			__builder.AddAttribute(10, "class", "text-sm font-semibold");
			__builder.AddAttribute(11, "style", "color:" + this.Color);
			__builder.AddContent(12, this._last);
			__builder.AddContent(13, string.IsNullOrEmpty(this.Unit) ? "" : (" " + this.Unit));
			__builder.CloseElement();
			__builder.CloseElement();
			if (this._hasLine)
			{
				__builder.OpenElement(14, "svg");
				__builder.AddAttribute(15, "class", "mt-1 block h-8 w-full");
				__builder.AddAttribute(16, "viewBox", "0 0 100 30");
				__builder.AddAttribute(17, "preserveAspectRatio", "none");
				__builder.AddAttribute(18, "role", "img");
				__builder.AddAttribute(19, "aria-label", this.Title + " trend");
				__builder.OpenElement(20, "polyline");
				__builder.AddAttribute(21, "points", this._poly);
				__builder.AddAttribute(22, "fill", "none");
				__builder.AddAttribute(23, "stroke", this.Color);
				__builder.AddAttribute(24, "stroke-width", "1.5");
				__builder.AddAttribute(25, "vector-effect", "non-scaling-stroke");
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(26, "\n        ");
				__builder.OpenElement(27, "div");
				__builder.AddAttribute(28, "class", "mt-1 flex justify-between text-[10px] text-cocoa-700");
				__builder.OpenElement(29, "span");
				__builder.AddContent(30, "min ");
				__builder.AddContent(31, this._min);
				__builder.CloseElement();
				__builder.AddMarkupContent(32, "\n            ");
				__builder.OpenElement(33, "span");
				__builder.AddContent(34, "max ");
				__builder.AddContent(35, this._max);
				__builder.CloseElement();
				__builder.AddMarkupContent(36, "\n            ");
				__builder.OpenElement(37, "span");
				int num = 38;
				List<HistoryPoint> points = this.Points;
				__builder.AddContent(num, (points != null) ? points.Count : 0);
				__builder.AddContent(39, " pts");
				__builder.CloseElement();
				__builder.CloseElement();
			}
			else
			{
				__builder.OpenElement(40, "div");
				__builder.AddAttribute(41, "class", "mt-2 text-[10px] text-cocoa-700");
				__builder.AddMarkupContent(42, "collecting… (");
				int num2 = 43;
				List<HistoryPoint> points2 = this.Points;
				__builder.AddContent(num2, (points2 != null) ? points2.Count : 0);
				__builder.AddContent(44, " pt");
				int num3 = 45;
				List<HistoryPoint> points3 = this.Points;
				__builder.AddContent(num3, (points3 != null && points3.Count == 1) ? "" : "s");
				__builder.AddContent(46, ")");
				__builder.CloseElement();
			}
			__builder.CloseElement();
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000202BD File Offset: 0x0001E4BD
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x000202C5 File Offset: 0x0001E4C5
		[Parameter]
		public string Title { get; set; } = "";

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000202CE File Offset: 0x0001E4CE
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x000202D6 File Offset: 0x0001E4D6
		[Nullable(2)]
		[Parameter]
		public string Unit
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000202DF File Offset: 0x0001E4DF
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x000202E7 File Offset: 0x0001E4E7
		[Parameter]
		public string Color { get; set; } = "#2563eb";

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x000202F0 File Offset: 0x0001E4F0
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x000202F8 File Offset: 0x0001E4F8
		[Nullable(2)]
		[Parameter]
		public List<HistoryPoint> Points
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00020304 File Offset: 0x0001E504
		protected override void OnParametersSet()
		{
			this._hasLine = false;
			if (this.Points == null || this.Points.Count == 0)
			{
				this._last = (this._min = (this._max = "–"));
				return;
			}
			List<double> list = this.Points.Select<HistoryPoint, double>((HistoryPoint p) => p.Value).ToList<double>();
			double num = list.Min();
			double num2 = list.Max();
			this._min = Sparkline.Fmt(num);
			this._max = Sparkline.Fmt(num2);
			List<double> list2 = list;
			this._last = Sparkline.Fmt(list2[list2.Count - 1]);
			if (this.Points.Count < 2)
			{
				return;
			}
			double num3 = num2 - num;
			int count = this.Points.Count;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				double num4 = (double)i / (double)(count - 1) * 100.0;
				double num5 = ((num3 < 1E-09) ? 15.0 : (29.0 - (list[i] - num) / num3 * 28.0));
				stringBuilder.Append(num4.ToString("0.##", CultureInfo.InvariantCulture)).Append(',').Append(num5.ToString("0.##", CultureInfo.InvariantCulture))
					.Append(' ');
			}
			this._poly = stringBuilder.ToString().TrimEnd();
			this._hasLine = true;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000204A0 File Offset: 0x0001E6A0
		private static string Fmt(double v)
		{
			if (Math.Abs(v) < 1000.0)
			{
				return v.ToString("0.###", CultureInfo.InvariantCulture);
			}
			return v.ToString("0", CultureInfo.InvariantCulture);
		}

		// Token: 0x0400028E RID: 654
		private string _poly = "";

		// Token: 0x0400028F RID: 655
		private string _min = "–";

		// Token: 0x04000290 RID: 656
		private string _max = "–";

		// Token: 0x04000291 RID: 657
		private string _last = "–";

		// Token: 0x04000292 RID: 658
		private bool _hasLine;
	}
}
