using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200003D RID: 61
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChartSeries : IEquatable<ChartSeries>
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x00008E44 File Offset: 0x00007044
		public ChartSeries(string Label, string Color, List<HistoryPoint> Points, bool Emphasis = false)
		{
			this.Label = Label;
			this.Color = Color;
			this.Points = Points;
			this.Emphasis = Emphasis;
			base..ctor();
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00008E69 File Offset: 0x00007069
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ChartSeries);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008E75 File Offset: 0x00007075
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00008E7D File Offset: 0x0000707D
		public string Label { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00008E86 File Offset: 0x00007086
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00008E8E File Offset: 0x0000708E
		public string Color { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00008E97 File Offset: 0x00007097
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008E9F File Offset: 0x0000709F
		public List<HistoryPoint> Points { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00008EA8 File Offset: 0x000070A8
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00008EB0 File Offset: 0x000070B0
		public bool Emphasis { get; set; }

		// Token: 0x060001AA RID: 426 RVA: 0x00008EBC File Offset: 0x000070BC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ChartSeries");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008F08 File Offset: 0x00007108
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Label = ");
			builder.Append(this.Label);
			builder.Append(", Color = ");
			builder.Append(this.Color);
			builder.Append(", Points = ");
			builder.Append(this.Points);
			builder.Append(", Emphasis = ");
			builder.Append(this.Emphasis.ToString());
			return true;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008F8D File Offset: 0x0000718D
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ChartSeries left, ChartSeries right)
		{
			return !(left == right);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008F99 File Offset: 0x00007199
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ChartSeries left, ChartSeries right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008FB0 File Offset: 0x000071B0
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Color>k__BackingField)) * -1521134295 + EqualityComparer<List<HistoryPoint>>.Default.GetHashCode(this.<Points>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Emphasis>k__BackingField);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009029 File Offset: 0x00007229
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ChartSeries);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009038 File Offset: 0x00007238
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ChartSeries other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Color>k__BackingField, other.<Color>k__BackingField) && EqualityComparer<List<HistoryPoint>>.Default.Equals(this.<Points>k__BackingField, other.<Points>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Emphasis>k__BackingField, other.<Emphasis>k__BackingField));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000090C9 File Offset: 0x000072C9
		[CompilerGenerated]
		private ChartSeries(ChartSeries original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Color = original.<Color>k__BackingField;
			this.Points = original.<Points>k__BackingField;
			this.Emphasis = original.<Emphasis>k__BackingField;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009101 File Offset: 0x00007301
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Color, out List<HistoryPoint> Points, out bool Emphasis)
		{
			Label = this.Label;
			Color = this.Color;
			Points = this.Points;
			Emphasis = this.Emphasis;
		}
	}
}
