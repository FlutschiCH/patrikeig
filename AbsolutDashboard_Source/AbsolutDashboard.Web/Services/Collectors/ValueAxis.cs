using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ValueAxis : IEquatable<ValueAxis>
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00006B49 File Offset: 0x00004D49
		public ValueAxis(double Top, double Step, IReadOnlyList<double> Ticks)
		{
			this.Top = Top;
			this.Step = Step;
			this.Ticks = Ticks;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006B60 File Offset: 0x00004D60
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00006B68 File Offset: 0x00004D68
		public double Top { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006B71 File Offset: 0x00004D71
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00006B79 File Offset: 0x00004D79
		public double Step { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006B82 File Offset: 0x00004D82
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00006B8A File Offset: 0x00004D8A
		public IReadOnlyList<double> Ticks { get; set; }

		// Token: 0x06000139 RID: 313 RVA: 0x00006B94 File Offset: 0x00004D94
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ValueAxis");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006BE0 File Offset: 0x00004DE0
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Top = ");
			builder.Append(this.Top.ToString());
			builder.Append(", Step = ");
			builder.Append(this.Step.ToString());
			builder.Append(", Ticks = ");
			builder.Append(this.Ticks);
			return true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006C55 File Offset: 0x00004E55
		[CompilerGenerated]
		public static bool operator !=(ValueAxis left, ValueAxis right)
		{
			return !(left == right);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006C61 File Offset: 0x00004E61
		[CompilerGenerated]
		public static bool operator ==(ValueAxis left, ValueAxis right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006C6B File Offset: 0x00004E6B
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<double>.Default.GetHashCode(this.<Top>k__BackingField) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Step>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<double>>.Default.GetHashCode(this.<Ticks>k__BackingField);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006CAB File Offset: 0x00004EAB
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is ValueAxis && this.Equals((ValueAxis)obj);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006CC4 File Offset: 0x00004EC4
		[CompilerGenerated]
		public bool Equals(ValueAxis other)
		{
			return EqualityComparer<double>.Default.Equals(this.<Top>k__BackingField, other.<Top>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Step>k__BackingField, other.<Step>k__BackingField) && EqualityComparer<IReadOnlyList<double>>.Default.Equals(this.<Ticks>k__BackingField, other.<Ticks>k__BackingField);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006D19 File Offset: 0x00004F19
		[CompilerGenerated]
		public void Deconstruct(out double Top, out double Step, out IReadOnlyList<double> Ticks)
		{
			Top = this.Top;
			Step = this.Step;
			Ticks = this.Ticks;
		}
	}
}
