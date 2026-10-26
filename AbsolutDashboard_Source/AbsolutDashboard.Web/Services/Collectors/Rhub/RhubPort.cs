using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x0200005E RID: 94
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class RhubPort : IEquatable<RhubPort>
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x0000E1C6 File Offset: 0x0000C3C6
		public RhubPort(int Index, PortKind Kind, PortState State, string RawStatus, DateTimeOffset? LastSeen)
		{
			this.Index = Index;
			this.Kind = Kind;
			this.State = State;
			this.RawStatus = RawStatus;
			this.LastSeen = LastSeen;
			base..ctor();
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000E1F3 File Offset: 0x0000C3F3
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(RhubPort);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000E1FF File Offset: 0x0000C3FF
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000E207 File Offset: 0x0000C407
		public int Index { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000E210 File Offset: 0x0000C410
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000E218 File Offset: 0x0000C418
		public PortKind Kind { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000E221 File Offset: 0x0000C421
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000E229 File Offset: 0x0000C429
		public PortState State { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000E232 File Offset: 0x0000C432
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000E23A File Offset: 0x0000C43A
		public string RawStatus { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000E243 File Offset: 0x0000C443
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000E24B File Offset: 0x0000C44B
		public DateTimeOffset? LastSeen { get; set; }

		// Token: 0x060002EE RID: 750 RVA: 0x0000E254 File Offset: 0x0000C454
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("RhubPort");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Index = ");
			builder.Append(this.Index.ToString());
			builder.Append(", Kind = ");
			builder.Append(this.Kind.ToString());
			builder.Append(", State = ");
			builder.Append(this.State.ToString());
			builder.Append(", RawStatus = ");
			builder.Append(this.RawStatus);
			builder.Append(", LastSeen = ");
			builder.Append(this.LastSeen.ToString());
			return true;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000E368 File Offset: 0x0000C568
		[CompilerGenerated]
		public static bool operator !=(RhubPort left, RhubPort right)
		{
			return !(left == right);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E374 File Offset: 0x0000C574
		[CompilerGenerated]
		public static bool operator ==(RhubPort left, RhubPort right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000E388 File Offset: 0x0000C588
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Index>k__BackingField)) * -1521134295 + EqualityComparer<PortKind>.Default.GetHashCode(this.<Kind>k__BackingField)) * -1521134295 + EqualityComparer<PortState>.Default.GetHashCode(this.<State>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RawStatus>k__BackingField)) * -1521134295 + EqualityComparer<DateTimeOffset?>.Default.GetHashCode(this.<LastSeen>k__BackingField);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000E418 File Offset: 0x0000C618
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as RhubPort);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000E428 File Offset: 0x0000C628
		[CompilerGenerated]
		public bool Equals(RhubPort other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<Index>k__BackingField, other.<Index>k__BackingField) && EqualityComparer<PortKind>.Default.Equals(this.<Kind>k__BackingField, other.<Kind>k__BackingField) && EqualityComparer<PortState>.Default.Equals(this.<State>k__BackingField, other.<State>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RawStatus>k__BackingField, other.<RawStatus>k__BackingField) && EqualityComparer<DateTimeOffset?>.Default.Equals(this.<LastSeen>k__BackingField, other.<LastSeen>k__BackingField));
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
		[CompilerGenerated]
		private RhubPort([Nullable(1)] RhubPort original)
		{
			this.Index = original.<Index>k__BackingField;
			this.Kind = original.<Kind>k__BackingField;
			this.State = original.<State>k__BackingField;
			this.RawStatus = original.<RawStatus>k__BackingField;
			this.LastSeen = original.<LastSeen>k__BackingField;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000E527 File Offset: 0x0000C727
		[CompilerGenerated]
		public void Deconstruct(out int Index, out PortKind Kind, out PortState State, out string RawStatus, out DateTimeOffset? LastSeen)
		{
			Index = this.Index;
			Kind = this.Kind;
			State = this.State;
			RawStatus = this.RawStatus;
			LastSeen = this.LastSeen;
		}
	}
}
