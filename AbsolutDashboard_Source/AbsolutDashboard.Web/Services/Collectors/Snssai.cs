using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000040 RID: 64
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct Snssai : IEquatable<Snssai>
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000A3B6 File Offset: 0x000085B6
		public Snssai(int Sst, string Sd)
		{
			this.Sst = Sst;
			this.Sd = Sd;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000A3C6 File Offset: 0x000085C6
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000A3CE File Offset: 0x000085CE
		public int Sst { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A3D7 File Offset: 0x000085D7
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000A3DF File Offset: 0x000085DF
		public string Sd { get; set; }

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A3E8 File Offset: 0x000085E8
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Snssai");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A434 File Offset: 0x00008634
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Sst = ");
			builder.Append(this.Sst.ToString());
			builder.Append(", Sd = ");
			builder.Append(this.Sd);
			return true;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A482 File Offset: 0x00008682
		[CompilerGenerated]
		public static bool operator !=(Snssai left, Snssai right)
		{
			return !(left == right);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A48E File Offset: 0x0000868E
		[CompilerGenerated]
		public static bool operator ==(Snssai left, Snssai right)
		{
			return left.Equals(right);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A498 File Offset: 0x00008698
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<int>.Default.GetHashCode(this.<Sst>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Sd>k__BackingField);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A4C1 File Offset: 0x000086C1
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is Snssai && this.Equals((Snssai)obj);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A4D9 File Offset: 0x000086D9
		[CompilerGenerated]
		public bool Equals(Snssai other)
		{
			return EqualityComparer<int>.Default.Equals(this.<Sst>k__BackingField, other.<Sst>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Sd>k__BackingField, other.<Sd>k__BackingField);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A50B File Offset: 0x0000870B
		[CompilerGenerated]
		public void Deconstruct(out int Sst, out string Sd)
		{
			Sst = this.Sst;
			Sd = this.Sd;
		}
	}
}
