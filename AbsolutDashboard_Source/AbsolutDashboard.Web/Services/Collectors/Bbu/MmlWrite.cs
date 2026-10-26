using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MmlWrite : IEquatable<MmlWrite>
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x00013CE0 File Offset: 0x00011EE0
		public MmlWrite(string Label, string Command, bool RequiresRestart = true)
		{
			this.Label = Label;
			this.Command = Command;
			this.RequiresRestart = RequiresRestart;
			base..ctor();
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00013CFD File Offset: 0x00011EFD
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(MmlWrite);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00013D09 File Offset: 0x00011F09
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00013D11 File Offset: 0x00011F11
		public string Label { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00013D1A File Offset: 0x00011F1A
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00013D22 File Offset: 0x00011F22
		public string Command { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00013D2B File Offset: 0x00011F2B
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00013D33 File Offset: 0x00011F33
		public bool RequiresRestart { get; set; }

		// Token: 0x06000456 RID: 1110 RVA: 0x00013D3C File Offset: 0x00011F3C
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MmlWrite");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00013D88 File Offset: 0x00011F88
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Label = ");
			builder.Append(this.Label);
			builder.Append(", Command = ");
			builder.Append(this.Command);
			builder.Append(", RequiresRestart = ");
			builder.Append(this.RequiresRestart.ToString());
			return true;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00013DF4 File Offset: 0x00011FF4
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(MmlWrite left, MmlWrite right)
		{
			return !(left == right);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00013E00 File Offset: 0x00012000
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(MmlWrite left, MmlWrite right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00013E14 File Offset: 0x00012014
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Command>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<RequiresRestart>k__BackingField);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00013E76 File Offset: 0x00012076
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as MmlWrite);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00013E84 File Offset: 0x00012084
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(MmlWrite other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Command>k__BackingField, other.<Command>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<RequiresRestart>k__BackingField, other.<RequiresRestart>k__BackingField));
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00013EFD File Offset: 0x000120FD
		[CompilerGenerated]
		private MmlWrite(MmlWrite original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Command = original.<Command>k__BackingField;
			this.RequiresRestart = original.<RequiresRestart>k__BackingField;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00013F29 File Offset: 0x00012129
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Command, out bool RequiresRestart)
		{
			Label = this.Label;
			Command = this.Command;
			RequiresRestart = this.RequiresRestart;
		}
	}
}
