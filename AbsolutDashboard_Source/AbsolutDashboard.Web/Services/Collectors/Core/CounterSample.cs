using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CounterSample : IEquatable<CounterSample>
	{
		// Token: 0x06000372 RID: 882 RVA: 0x000100E8 File Offset: 0x0000E2E8
		public CounterSample(DateTime AtUtc, long DlBytes, long UlBytes)
		{
			this.AtUtc = AtUtc;
			this.DlBytes = DlBytes;
			this.UlBytes = UlBytes;
			base..ctor();
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00010105 File Offset: 0x0000E305
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CounterSample);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00010111 File Offset: 0x0000E311
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00010119 File Offset: 0x0000E319
		public DateTime AtUtc { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00010122 File Offset: 0x0000E322
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0001012A File Offset: 0x0000E32A
		public long DlBytes { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00010133 File Offset: 0x0000E333
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0001013B File Offset: 0x0000E33B
		public long UlBytes { get; set; }

		// Token: 0x0600037A RID: 890 RVA: 0x00010144 File Offset: 0x0000E344
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CounterSample");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00010190 File Offset: 0x0000E390
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("AtUtc = ");
			builder.Append(this.AtUtc.ToString());
			builder.Append(", DlBytes = ");
			builder.Append(this.DlBytes.ToString());
			builder.Append(", UlBytes = ");
			builder.Append(this.UlBytes.ToString());
			return true;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00010218 File Offset: 0x0000E418
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(CounterSample left, CounterSample right)
		{
			return !(left == right);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00010224 File Offset: 0x0000E424
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(CounterSample left, CounterSample right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00010238 File Offset: 0x0000E438
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<AtUtc>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<DlBytes>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<UlBytes>k__BackingField);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001029A File Offset: 0x0000E49A
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CounterSample);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000102A8 File Offset: 0x0000E4A8
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(CounterSample other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<DateTime>.Default.Equals(this.<AtUtc>k__BackingField, other.<AtUtc>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<DlBytes>k__BackingField, other.<DlBytes>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<UlBytes>k__BackingField, other.<UlBytes>k__BackingField));
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00010321 File Offset: 0x0000E521
		[CompilerGenerated]
		private CounterSample(CounterSample original)
		{
			this.AtUtc = original.<AtUtc>k__BackingField;
			this.DlBytes = original.<DlBytes>k__BackingField;
			this.UlBytes = original.<UlBytes>k__BackingField;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001034D File Offset: 0x0000E54D
		[CompilerGenerated]
		public void Deconstruct(out DateTime AtUtc, out long DlBytes, out long UlBytes)
		{
			AtUtc = this.AtUtc;
			DlBytes = this.DlBytes;
			UlBytes = this.UlBytes;
		}
	}
}
