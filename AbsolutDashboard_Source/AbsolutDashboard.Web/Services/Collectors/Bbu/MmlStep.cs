using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MmlStep : IEquatable<MmlStep>
	{
		// Token: 0x06000460 RID: 1120 RVA: 0x00013F43 File Offset: 0x00012143
		public MmlStep(string Label, string Command, bool Ok, int RetCode, string Message)
		{
			this.Label = Label;
			this.Command = Command;
			this.Ok = Ok;
			this.RetCode = RetCode;
			this.Message = Message;
			base..ctor();
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00013F70 File Offset: 0x00012170
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(MmlStep);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00013F7C File Offset: 0x0001217C
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00013F84 File Offset: 0x00012184
		public string Label { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00013F8D File Offset: 0x0001218D
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00013F95 File Offset: 0x00012195
		public string Command { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00013F9E File Offset: 0x0001219E
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00013FA6 File Offset: 0x000121A6
		public bool Ok { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00013FAF File Offset: 0x000121AF
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00013FB7 File Offset: 0x000121B7
		public int RetCode { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00013FC0 File Offset: 0x000121C0
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00013FC8 File Offset: 0x000121C8
		public string Message { get; set; }

		// Token: 0x0600046C RID: 1132 RVA: 0x00013FD4 File Offset: 0x000121D4
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MmlStep");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00014020 File Offset: 0x00012220
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Label = ");
			builder.Append(this.Label);
			builder.Append(", Command = ");
			builder.Append(this.Command);
			builder.Append(", Ok = ");
			builder.Append(this.Ok.ToString());
			builder.Append(", RetCode = ");
			builder.Append(this.RetCode.ToString());
			builder.Append(", Message = ");
			builder.Append(this.Message);
			return true;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000140CC File Offset: 0x000122CC
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(MmlStep left, MmlStep right)
		{
			return !(left == right);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000140D8 File Offset: 0x000122D8
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(MmlStep left, MmlStep right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000140EC File Offset: 0x000122EC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Command>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Ok>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<RetCode>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Message>k__BackingField);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001417C File Offset: 0x0001237C
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as MmlStep);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001418C File Offset: 0x0001238C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(MmlStep other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Command>k__BackingField, other.<Command>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Ok>k__BackingField, other.<Ok>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<RetCode>k__BackingField, other.<RetCode>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Message>k__BackingField, other.<Message>k__BackingField));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001423C File Offset: 0x0001243C
		[CompilerGenerated]
		private MmlStep(MmlStep original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Command = original.<Command>k__BackingField;
			this.Ok = original.<Ok>k__BackingField;
			this.RetCode = original.<RetCode>k__BackingField;
			this.Message = original.<Message>k__BackingField;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001428B File Offset: 0x0001248B
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Command, out bool Ok, out int RetCode, out string Message)
		{
			Label = this.Label;
			Command = this.Command;
			Ok = this.Ok;
			RetCode = this.RetCode;
			Message = this.Message;
		}
	}
}
