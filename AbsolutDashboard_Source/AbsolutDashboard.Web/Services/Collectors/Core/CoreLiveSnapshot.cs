using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoreLiveSnapshot : IEquatable<CoreLiveSnapshot>
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000FCCD File Offset: 0x0000DECD
		public CoreLiveSnapshot(int CoreDeviceId, DateTime AtUtc, IReadOnlyDictionary<string, UeLive> Ues, double TotalDlMbps, double TotalUlMbps, bool CountersAvailable)
		{
			this.CoreDeviceId = CoreDeviceId;
			this.AtUtc = AtUtc;
			this.Ues = Ues;
			this.TotalDlMbps = TotalDlMbps;
			this.TotalUlMbps = TotalUlMbps;
			this.CountersAvailable = CountersAvailable;
			base..ctor();
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000FD02 File Offset: 0x0000DF02
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CoreLiveSnapshot);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000FD0E File Offset: 0x0000DF0E
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000FD16 File Offset: 0x0000DF16
		public int CoreDeviceId { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000FD1F File Offset: 0x0000DF1F
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000FD27 File Offset: 0x0000DF27
		public DateTime AtUtc { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000FD30 File Offset: 0x0000DF30
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000FD38 File Offset: 0x0000DF38
		public IReadOnlyDictionary<string, UeLive> Ues { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000FD41 File Offset: 0x0000DF41
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0000FD49 File Offset: 0x0000DF49
		public double TotalDlMbps { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000FD52 File Offset: 0x0000DF52
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0000FD5A File Offset: 0x0000DF5A
		public double TotalUlMbps { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000FD63 File Offset: 0x0000DF63
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000FD6B File Offset: 0x0000DF6B
		public bool CountersAvailable { get; set; }

		// Token: 0x06000368 RID: 872 RVA: 0x0000FD74 File Offset: 0x0000DF74
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CoreLiveSnapshot");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("CoreDeviceId = ");
			builder.Append(this.CoreDeviceId.ToString());
			builder.Append(", AtUtc = ");
			builder.Append(this.AtUtc.ToString());
			builder.Append(", Ues = ");
			builder.Append(this.Ues);
			builder.Append(", TotalDlMbps = ");
			builder.Append(this.TotalDlMbps.ToString());
			builder.Append(", TotalUlMbps = ");
			builder.Append(this.TotalUlMbps.ToString());
			builder.Append(", CountersAvailable = ");
			builder.Append(this.CountersAvailable.ToString());
			return true;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000FEAF File Offset: 0x0000E0AF
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(CoreLiveSnapshot left, CoreLiveSnapshot right)
		{
			return !(left == right);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000FEBB File Offset: 0x0000E0BB
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(CoreLiveSnapshot left, CoreLiveSnapshot right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<CoreDeviceId>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<AtUtc>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyDictionary<string, UeLive>>.Default.GetHashCode(this.<Ues>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<TotalDlMbps>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<TotalUlMbps>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<CountersAvailable>k__BackingField);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000FF77 File Offset: 0x0000E177
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CoreLiveSnapshot);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000FF88 File Offset: 0x0000E188
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(CoreLiveSnapshot other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<CoreDeviceId>k__BackingField, other.<CoreDeviceId>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<AtUtc>k__BackingField, other.<AtUtc>k__BackingField) && EqualityComparer<IReadOnlyDictionary<string, UeLive>>.Default.Equals(this.<Ues>k__BackingField, other.<Ues>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<TotalDlMbps>k__BackingField, other.<TotalDlMbps>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<TotalUlMbps>k__BackingField, other.<TotalUlMbps>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<CountersAvailable>k__BackingField, other.<CountersAvailable>k__BackingField));
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00010054 File Offset: 0x0000E254
		[CompilerGenerated]
		private CoreLiveSnapshot(CoreLiveSnapshot original)
		{
			this.CoreDeviceId = original.<CoreDeviceId>k__BackingField;
			this.AtUtc = original.<AtUtc>k__BackingField;
			this.Ues = original.<Ues>k__BackingField;
			this.TotalDlMbps = original.<TotalDlMbps>k__BackingField;
			this.TotalUlMbps = original.<TotalUlMbps>k__BackingField;
			this.CountersAvailable = original.<CountersAvailable>k__BackingField;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000100AF File Offset: 0x0000E2AF
		[CompilerGenerated]
		public void Deconstruct(out int CoreDeviceId, out DateTime AtUtc, out IReadOnlyDictionary<string, UeLive> Ues, out double TotalDlMbps, out double TotalUlMbps, out bool CountersAvailable)
		{
			CoreDeviceId = this.CoreDeviceId;
			AtUtc = this.AtUtc;
			Ues = this.Ues;
			TotalDlMbps = this.TotalDlMbps;
			TotalUlMbps = this.TotalUlMbps;
			CountersAvailable = this.CountersAvailable;
		}
	}
}
