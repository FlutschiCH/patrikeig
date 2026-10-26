using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200003A RID: 58
	public readonly struct HistoryPoint : IEquatable<HistoryPoint>
	{
		// Token: 0x0600017F RID: 383 RVA: 0x000089FE File Offset: 0x00006BFE
		public HistoryPoint(DateTime Ts, double Value)
		{
			this.Ts = Ts;
			this.Value = Value;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00008A0E File Offset: 0x00006C0E
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00008A16 File Offset: 0x00006C16
		public DateTime Ts { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00008A1F File Offset: 0x00006C1F
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00008A27 File Offset: 0x00006C27
		public double Value { get; set; }

		// Token: 0x06000184 RID: 388 RVA: 0x00008A30 File Offset: 0x00006C30
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("HistoryPoint");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008A7C File Offset: 0x00006C7C
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Ts = ");
			builder.Append(this.Ts.ToString());
			builder.Append(", Value = ");
			builder.Append(this.Value.ToString());
			return true;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008AD8 File Offset: 0x00006CD8
		[CompilerGenerated]
		public static bool operator !=(HistoryPoint left, HistoryPoint right)
		{
			return !(left == right);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008AE4 File Offset: 0x00006CE4
		[CompilerGenerated]
		public static bool operator ==(HistoryPoint left, HistoryPoint right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008AEE File Offset: 0x00006CEE
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<DateTime>.Default.GetHashCode(this.<Ts>k__BackingField) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Value>k__BackingField);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008B17 File Offset: 0x00006D17
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is HistoryPoint && this.Equals((HistoryPoint)obj);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008B2F File Offset: 0x00006D2F
		[CompilerGenerated]
		public bool Equals(HistoryPoint other)
		{
			return EqualityComparer<DateTime>.Default.Equals(this.<Ts>k__BackingField, other.<Ts>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Value>k__BackingField, other.<Value>k__BackingField);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008B61 File Offset: 0x00006D61
		[CompilerGenerated]
		public void Deconstruct(out DateTime Ts, out double Value)
		{
			Ts = this.Ts;
			Value = this.Value;
		}
	}
}
