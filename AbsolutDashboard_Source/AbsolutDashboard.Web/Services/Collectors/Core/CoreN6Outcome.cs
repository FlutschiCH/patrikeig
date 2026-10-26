using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000071 RID: 113
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class CoreN6Outcome : IEquatable<CoreN6Outcome>
	{
		// Token: 0x06000402 RID: 1026 RVA: 0x000124D0 File Offset: 0x000106D0
		public CoreN6Outcome(bool Ok, string Error, N6State State, [Nullable(1)] List<CoreStep> Steps)
		{
			this.Ok = Ok;
			this.Error = Error;
			this.State = State;
			this.Steps = Steps;
			base..ctor();
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000124F5 File Offset: 0x000106F5
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(CoreN6Outcome);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00012501 File Offset: 0x00010701
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00012509 File Offset: 0x00010709
		public bool Ok { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00012512 File Offset: 0x00010712
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0001251A File Offset: 0x0001071A
		public string Error { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00012523 File Offset: 0x00010723
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0001252B File Offset: 0x0001072B
		public N6State State { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00012534 File Offset: 0x00010734
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0001253C File Offset: 0x0001073C
		[Nullable(1)]
		public List<CoreStep> Steps
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00012545 File Offset: 0x00010745
		[NullableContext(1)]
		public static CoreN6Outcome Failed(string error)
		{
			return new CoreN6Outcome(false, error, null, new List<CoreStep>());
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00012554 File Offset: 0x00010754
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CoreN6Outcome");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000125A0 File Offset: 0x000107A0
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Ok = ");
			builder.Append(this.Ok.ToString());
			builder.Append(", Error = ");
			builder.Append(this.Error);
			builder.Append(", State = ");
			builder.Append(this.State);
			builder.Append(", Steps = ");
			builder.Append(this.Steps);
			return true;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00012625 File Offset: 0x00010825
		[CompilerGenerated]
		public static bool operator !=(CoreN6Outcome left, CoreN6Outcome right)
		{
			return !(left == right);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00012631 File Offset: 0x00010831
		[CompilerGenerated]
		public static bool operator ==(CoreN6Outcome left, CoreN6Outcome right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00012648 File Offset: 0x00010848
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Ok>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Error>k__BackingField)) * -1521134295 + EqualityComparer<N6State>.Default.GetHashCode(this.<State>k__BackingField)) * -1521134295 + EqualityComparer<List<CoreStep>>.Default.GetHashCode(this.<Steps>k__BackingField);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000126C1 File Offset: 0x000108C1
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CoreN6Outcome);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000126D0 File Offset: 0x000108D0
		[CompilerGenerated]
		public bool Equals(CoreN6Outcome other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<bool>.Default.Equals(this.<Ok>k__BackingField, other.<Ok>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Error>k__BackingField, other.<Error>k__BackingField) && EqualityComparer<N6State>.Default.Equals(this.<State>k__BackingField, other.<State>k__BackingField) && EqualityComparer<List<CoreStep>>.Default.Equals(this.<Steps>k__BackingField, other.<Steps>k__BackingField));
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00012761 File Offset: 0x00010961
		[CompilerGenerated]
		private CoreN6Outcome([Nullable(1)] CoreN6Outcome original)
		{
			this.Ok = original.<Ok>k__BackingField;
			this.Error = original.<Error>k__BackingField;
			this.State = original.<State>k__BackingField;
			this.Steps = original.<Steps>k__BackingField;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00012799 File Offset: 0x00010999
		[CompilerGenerated]
		public void Deconstruct(out bool Ok, out string Error, out N6State State, [Nullable(1)] out List<CoreStep> Steps)
		{
			Ok = this.Ok;
			Error = this.Error;
			State = this.State;
			Steps = this.Steps;
		}
	}
}
