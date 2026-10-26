using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000077 RID: 119
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuWriteOutcome : IEquatable<BbuWriteOutcome>
	{
		// Token: 0x06000476 RID: 1142 RVA: 0x000142B7 File Offset: 0x000124B7
		public BbuWriteOutcome(bool Ok, [Nullable(2)] string Error, List<MmlStep> Steps)
		{
			this.Ok = Ok;
			this.Error = Error;
			this.Steps = Steps;
			base..ctor();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x000142D4 File Offset: 0x000124D4
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(BbuWriteOutcome);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x000142E0 File Offset: 0x000124E0
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x000142E8 File Offset: 0x000124E8
		public bool Ok { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x000142F1 File Offset: 0x000124F1
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x000142F9 File Offset: 0x000124F9
		[Nullable(2)]
		public string Error
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00014302 File Offset: 0x00012502
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001430A File Offset: 0x0001250A
		public List<MmlStep> Steps { get; set; }

		// Token: 0x0600047E RID: 1150 RVA: 0x00014313 File Offset: 0x00012513
		public static BbuWriteOutcome Failed(string error)
		{
			return new BbuWriteOutcome(false, error, new List<MmlStep>());
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00014324 File Offset: 0x00012524
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BbuWriteOutcome");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00014370 File Offset: 0x00012570
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Ok = ");
			builder.Append(this.Ok.ToString());
			builder.Append(", Error = ");
			builder.Append(this.Error);
			builder.Append(", Steps = ");
			builder.Append(this.Steps);
			return true;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000143DC File Offset: 0x000125DC
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(BbuWriteOutcome left, BbuWriteOutcome right)
		{
			return !(left == right);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000143E8 File Offset: 0x000125E8
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(BbuWriteOutcome left, BbuWriteOutcome right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000143FC File Offset: 0x000125FC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Ok>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Error>k__BackingField)) * -1521134295 + EqualityComparer<List<MmlStep>>.Default.GetHashCode(this.<Steps>k__BackingField);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001445E File Offset: 0x0001265E
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as BbuWriteOutcome);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001446C File Offset: 0x0001266C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(BbuWriteOutcome other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<bool>.Default.Equals(this.<Ok>k__BackingField, other.<Ok>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Error>k__BackingField, other.<Error>k__BackingField) && EqualityComparer<List<MmlStep>>.Default.Equals(this.<Steps>k__BackingField, other.<Steps>k__BackingField));
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000144E5 File Offset: 0x000126E5
		[CompilerGenerated]
		private BbuWriteOutcome(BbuWriteOutcome original)
		{
			this.Ok = original.<Ok>k__BackingField;
			this.Error = original.<Error>k__BackingField;
			this.Steps = original.<Steps>k__BackingField;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014511 File Offset: 0x00012711
		[CompilerGenerated]
		public void Deconstruct(out bool Ok, [Nullable(2)] out string Error, out List<MmlStep> Steps)
		{
			Ok = this.Ok;
			Error = this.Error;
			Steps = this.Steps;
		}
	}
}
