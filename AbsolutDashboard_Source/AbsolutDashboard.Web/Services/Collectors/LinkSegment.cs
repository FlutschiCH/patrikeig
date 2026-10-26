using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LinkSegment : IEquatable<LinkSegment>
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00007132 File Offset: 0x00005332
		public LinkSegment(DateTime FromUtc, DateTime ToUtc, LinkState State)
		{
			this.FromUtc = FromUtc;
			this.ToUtc = ToUtc;
			this.State = State;
			base..ctor();
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000714F File Offset: 0x0000534F
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(LinkSegment);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000715B File Offset: 0x0000535B
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00007163 File Offset: 0x00005363
		public DateTime FromUtc { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000716C File Offset: 0x0000536C
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00007174 File Offset: 0x00005374
		public DateTime ToUtc { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000717D File Offset: 0x0000537D
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00007185 File Offset: 0x00005385
		public LinkState State { get; set; }

		// Token: 0x06000152 RID: 338 RVA: 0x00007190 File Offset: 0x00005390
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("LinkSegment");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000071DC File Offset: 0x000053DC
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("FromUtc = ");
			builder.Append(this.FromUtc.ToString());
			builder.Append(", ToUtc = ");
			builder.Append(this.ToUtc.ToString());
			builder.Append(", State = ");
			builder.Append(this.State.ToString());
			return true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007264 File Offset: 0x00005464
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(LinkSegment left, LinkSegment right)
		{
			return !(left == right);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007270 File Offset: 0x00005470
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(LinkSegment left, LinkSegment right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007284 File Offset: 0x00005484
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<FromUtc>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<ToUtc>k__BackingField)) * -1521134295 + EqualityComparer<LinkState>.Default.GetHashCode(this.<State>k__BackingField);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000072E6 File Offset: 0x000054E6
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as LinkSegment);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000072F4 File Offset: 0x000054F4
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(LinkSegment other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<DateTime>.Default.Equals(this.<FromUtc>k__BackingField, other.<FromUtc>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<ToUtc>k__BackingField, other.<ToUtc>k__BackingField) && EqualityComparer<LinkState>.Default.Equals(this.<State>k__BackingField, other.<State>k__BackingField));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000736D File Offset: 0x0000556D
		[CompilerGenerated]
		private LinkSegment(LinkSegment original)
		{
			this.FromUtc = original.<FromUtc>k__BackingField;
			this.ToUtc = original.<ToUtc>k__BackingField;
			this.State = original.<State>k__BackingField;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007399 File Offset: 0x00005599
		[CompilerGenerated]
		public void Deconstruct(out DateTime FromUtc, out DateTime ToUtc, out LinkState State)
		{
			FromUtc = this.FromUtc;
			ToUtc = this.ToUtc;
			State = this.State;
		}
	}
}
