using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Metric : IEquatable<Metric>
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public Metric(string Label, string Value, [Nullable(2)] string Unit = null, [Nullable(2)] string Group = null)
		{
			this.Label = Label;
			this.Value = Value;
			this.Unit = Unit;
			this.Group = Group;
			base..ctor();
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A7E5 File Offset: 0x000089E5
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(Metric);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000A7F1 File Offset: 0x000089F1
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000A7F9 File Offset: 0x000089F9
		public string Label { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A802 File Offset: 0x00008A02
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000A80A File Offset: 0x00008A0A
		public string Value { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A813 File Offset: 0x00008A13
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000A81B File Offset: 0x00008A1B
		[Nullable(2)]
		public string Unit
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000A824 File Offset: 0x00008A24
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000A82C File Offset: 0x00008A2C
		[Nullable(2)]
		public string Group
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A838 File Offset: 0x00008A38
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Metric");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A884 File Offset: 0x00008A84
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Label = ");
			builder.Append(this.Label);
			builder.Append(", Value = ");
			builder.Append(this.Value);
			builder.Append(", Unit = ");
			builder.Append(this.Unit);
			builder.Append(", Group = ");
			builder.Append(this.Group);
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A8FB File Offset: 0x00008AFB
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(Metric left, Metric right)
		{
			return !(left == right);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A907 File Offset: 0x00008B07
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(Metric left, Metric right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A91C File Offset: 0x00008B1C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Value>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Unit>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Group>k__BackingField);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A995 File Offset: 0x00008B95
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Metric);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(Metric other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Value>k__BackingField, other.<Value>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Unit>k__BackingField, other.<Unit>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Group>k__BackingField, other.<Group>k__BackingField));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000AA35 File Offset: 0x00008C35
		[CompilerGenerated]
		private Metric(Metric original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Value = original.<Value>k__BackingField;
			this.Unit = original.<Unit>k__BackingField;
			this.Group = original.<Group>k__BackingField;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000AA6D File Offset: 0x00008C6D
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Value, [Nullable(2)] out string Unit, [Nullable(2)] out string Group)
		{
			Label = this.Label;
			Value = this.Value;
			Unit = this.Unit;
			Group = this.Group;
		}
	}
}
