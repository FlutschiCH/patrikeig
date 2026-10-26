using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000066 RID: 102
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class UeLive : IEquatable<UeLive>
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0000F838 File Offset: 0x0000DA38
		public UeLive([Nullable(1)] string Imsi, string State, string Ip, double DlMbps, double UlMbps, DateTime SeenUtc, DateTime? StateReadUtc)
		{
			this.Imsi = Imsi;
			this.State = State;
			this.Ip = Ip;
			this.DlMbps = DlMbps;
			this.UlMbps = UlMbps;
			this.SeenUtc = SeenUtc;
			this.StateReadUtc = StateReadUtc;
			base..ctor();
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F875 File Offset: 0x0000DA75
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(UeLive);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000F881 File Offset: 0x0000DA81
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000F889 File Offset: 0x0000DA89
		[Nullable(1)]
		public string Imsi
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000F892 File Offset: 0x0000DA92
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000F89A File Offset: 0x0000DA9A
		public string State { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000F8A3 File Offset: 0x0000DAA3
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000F8AB File Offset: 0x0000DAAB
		public string Ip { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000F8BC File Offset: 0x0000DABC
		public double DlMbps { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000F8C5 File Offset: 0x0000DAC5
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000F8CD File Offset: 0x0000DACD
		public double UlMbps { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000F8D6 File Offset: 0x0000DAD6
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000F8DE File Offset: 0x0000DADE
		public DateTime SeenUtc { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000F8E7 File Offset: 0x0000DAE7
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000F8EF File Offset: 0x0000DAEF
		public DateTime? StateReadUtc { get; set; }

		// Token: 0x06000350 RID: 848 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UeLive");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000F944 File Offset: 0x0000DB44
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Imsi = ");
			builder.Append(this.Imsi);
			builder.Append(", State = ");
			builder.Append(this.State);
			builder.Append(", Ip = ");
			builder.Append(this.Ip);
			builder.Append(", DlMbps = ");
			builder.Append(this.DlMbps.ToString());
			builder.Append(", UlMbps = ");
			builder.Append(this.UlMbps.ToString());
			builder.Append(", SeenUtc = ");
			builder.Append(this.SeenUtc.ToString());
			builder.Append(", StateReadUtc = ");
			builder.Append(this.StateReadUtc.ToString());
			return true;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000FA3E File Offset: 0x0000DC3E
		[CompilerGenerated]
		public static bool operator !=(UeLive left, UeLive right)
		{
			return !(left == right);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000FA4A File Offset: 0x0000DC4A
		[CompilerGenerated]
		public static bool operator ==(UeLive left, UeLive right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000FA60 File Offset: 0x0000DC60
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Imsi>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<State>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Ip>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<DlMbps>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<UlMbps>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<SeenUtc>k__BackingField)) * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(this.<StateReadUtc>k__BackingField);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000FB1E File Offset: 0x0000DD1E
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as UeLive);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000FB2C File Offset: 0x0000DD2C
		[CompilerGenerated]
		public bool Equals(UeLive other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Imsi>k__BackingField, other.<Imsi>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<State>k__BackingField, other.<State>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Ip>k__BackingField, other.<Ip>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<DlMbps>k__BackingField, other.<DlMbps>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<UlMbps>k__BackingField, other.<UlMbps>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<SeenUtc>k__BackingField, other.<SeenUtc>k__BackingField) && EqualityComparer<DateTime?>.Default.Equals(this.<StateReadUtc>k__BackingField, other.<StateReadUtc>k__BackingField));
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000FC14 File Offset: 0x0000DE14
		[CompilerGenerated]
		private UeLive([Nullable(1)] UeLive original)
		{
			this.Imsi = original.<Imsi>k__BackingField;
			this.State = original.<State>k__BackingField;
			this.Ip = original.<Ip>k__BackingField;
			this.DlMbps = original.<DlMbps>k__BackingField;
			this.UlMbps = original.<UlMbps>k__BackingField;
			this.SeenUtc = original.<SeenUtc>k__BackingField;
			this.StateReadUtc = original.<StateReadUtc>k__BackingField;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000FC7C File Offset: 0x0000DE7C
		[CompilerGenerated]
		public void Deconstruct([Nullable(1)] out string Imsi, out string State, out string Ip, out double DlMbps, out double UlMbps, out DateTime SeenUtc, out DateTime? StateReadUtc)
		{
			Imsi = this.Imsi;
			State = this.State;
			Ip = this.Ip;
			DlMbps = this.DlMbps;
			UlMbps = this.UlMbps;
			SeenUtc = this.SeenUtc;
			StateReadUtc = this.StateReadUtc;
		}
	}
}
