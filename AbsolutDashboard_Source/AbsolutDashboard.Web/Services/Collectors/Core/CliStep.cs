using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CliStep : IEquatable<CliStep>
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00010F3C File Offset: 0x0000F13C
		public CliStep(string Label, string Command)
		{
			this.Label = Label;
			this.Command = Command;
			base..ctor();
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00010F52 File Offset: 0x0000F152
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CliStep);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00010F5E File Offset: 0x0000F15E
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00010F66 File Offset: 0x0000F166
		public string Label { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00010F6F File Offset: 0x0000F16F
		// (set) Token: 0x060003CE RID: 974 RVA: 0x00010F77 File Offset: 0x0000F177
		public string Command { get; set; }

		// Token: 0x060003CF RID: 975 RVA: 0x00010F80 File Offset: 0x0000F180
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CliStep");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00010FCC File Offset: 0x0000F1CC
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Label = ");
			builder.Append(this.Label);
			builder.Append(", Command = ");
			builder.Append(this.Command);
			return true;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011006 File Offset: 0x0000F206
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(CliStep left, CliStep right)
		{
			return !(left == right);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011012 File Offset: 0x0000F212
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(CliStep left, CliStep right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00011026 File Offset: 0x0000F226
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Command>k__BackingField);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00011066 File Offset: 0x0000F266
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CliStep);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00011074 File Offset: 0x0000F274
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(CliStep other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Command>k__BackingField, other.<Command>k__BackingField));
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000110D5 File Offset: 0x0000F2D5
		[CompilerGenerated]
		private CliStep(CliStep original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Command = original.<Command>k__BackingField;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000110F5 File Offset: 0x0000F2F5
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Command)
		{
			Label = this.Label;
			Command = this.Command;
		}
	}
}
