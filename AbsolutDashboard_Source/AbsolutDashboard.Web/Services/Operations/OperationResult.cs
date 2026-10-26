using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Operations
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OperationResult : IEquatable<OperationResult>
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00005991 File Offset: 0x00003B91
		public OperationResult(string Label, string Command, bool Ok, string Detail)
		{
			this.Label = Label;
			this.Command = Command;
			this.Ok = Ok;
			this.Detail = Detail;
			base..ctor();
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000059B6 File Offset: 0x00003BB6
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(OperationResult);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000059C2 File Offset: 0x00003BC2
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000059CA File Offset: 0x00003BCA
		public string Label { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000059D3 File Offset: 0x00003BD3
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000059DB File Offset: 0x00003BDB
		public string Command { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000059E4 File Offset: 0x00003BE4
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000059EC File Offset: 0x00003BEC
		public bool Ok { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000059F5 File Offset: 0x00003BF5
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000059FD File Offset: 0x00003BFD
		public string Detail { get; set; }

		// Token: 0x060000E4 RID: 228 RVA: 0x00005A08 File Offset: 0x00003C08
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("OperationResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005A54 File Offset: 0x00003C54
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
			builder.Append(", Detail = ");
			builder.Append(this.Detail);
			return true;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005AD9 File Offset: 0x00003CD9
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(OperationResult left, OperationResult right)
		{
			return !(left == right);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005AE5 File Offset: 0x00003CE5
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(OperationResult left, OperationResult right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005AFC File Offset: 0x00003CFC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Command>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Ok>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Detail>k__BackingField);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005B75 File Offset: 0x00003D75
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as OperationResult);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005B84 File Offset: 0x00003D84
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(OperationResult other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Command>k__BackingField, other.<Command>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Ok>k__BackingField, other.<Ok>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Detail>k__BackingField, other.<Detail>k__BackingField));
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005C15 File Offset: 0x00003E15
		[CompilerGenerated]
		private OperationResult(OperationResult original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Command = original.<Command>k__BackingField;
			this.Ok = original.<Ok>k__BackingField;
			this.Detail = original.<Detail>k__BackingField;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005C4D File Offset: 0x00003E4D
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Command, out bool Ok, out string Detail)
		{
			Label = this.Label;
			Command = this.Command;
			Ok = this.Ok;
			Detail = this.Detail;
		}
	}
}
