using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class N6State : IEquatable<N6State>
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x00010B97 File Offset: 0x0000ED97
		public N6State(N6Interface Interface, IReadOnlyList<string> DefaultGateways, IReadOnlyList<IpWithMask> OtherStaticRoutes, IReadOnlyDictionary<int, IReadOnlyList<string>> NatGroups)
		{
			this.Interface = Interface;
			this.DefaultGateways = DefaultGateways;
			this.OtherStaticRoutes = OtherStaticRoutes;
			this.NatGroups = NatGroups;
			base..ctor();
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00010BBC File Offset: 0x0000EDBC
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(N6State);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00010BC8 File Offset: 0x0000EDC8
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00010BD0 File Offset: 0x0000EDD0
		public N6Interface Interface { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00010BD9 File Offset: 0x0000EDD9
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00010BE1 File Offset: 0x0000EDE1
		public IReadOnlyList<string> DefaultGateways { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00010BEA File Offset: 0x0000EDEA
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00010BF2 File Offset: 0x0000EDF2
		public IReadOnlyList<IpWithMask> OtherStaticRoutes { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00010BFB File Offset: 0x0000EDFB
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00010C03 File Offset: 0x0000EE03
		public IReadOnlyDictionary<int, IReadOnlyList<string>> NatGroups { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00010C0C File Offset: 0x0000EE0C
		[Nullable(2)]
		public IpWithMask CurrentN6
		{
			[NullableContext(2)]
			get
			{
				if (this.Interface.Secondaries.Count <= 0)
				{
					return null;
				}
				return this.Interface.Secondaries[0];
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00010C34 File Offset: 0x0000EE34
		[Nullable(2)]
		public string CurrentGateway
		{
			[NullableContext(2)]
			get
			{
				if (this.DefaultGateways.Count <= 0)
				{
					return null;
				}
				return this.DefaultGateways[0];
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00010C54 File Offset: 0x0000EE54
		public IReadOnlyList<string> NatSourceAddresses
		{
			get
			{
				int? natAddressGroup = this.Interface.NatAddressGroup;
				if (natAddressGroup != null)
				{
					int valueOrDefault = natAddressGroup.GetValueOrDefault();
					IReadOnlyList<string> readOnlyList;
					if (this.NatGroups.TryGetValue(valueOrDefault, out readOnlyList))
					{
						return readOnlyList;
					}
				}
				return Array.Empty<string>();
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00010C98 File Offset: 0x0000EE98
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("N6State");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Interface = ");
			builder.Append(this.Interface);
			builder.Append(", DefaultGateways = ");
			builder.Append(this.DefaultGateways);
			builder.Append(", OtherStaticRoutes = ");
			builder.Append(this.OtherStaticRoutes);
			builder.Append(", NatGroups = ");
			builder.Append(this.NatGroups);
			builder.Append(", CurrentN6 = ");
			builder.Append(this.CurrentN6);
			builder.Append(", CurrentGateway = ");
			builder.Append(this.CurrentGateway);
			builder.Append(", NatSourceAddresses = ");
			builder.Append(this.NatSourceAddresses);
			return true;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00010DA6 File Offset: 0x0000EFA6
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(N6State left, N6State right)
		{
			return !(left == right);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00010DB2 File Offset: 0x0000EFB2
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(N6State left, N6State right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00010DC8 File Offset: 0x0000EFC8
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<N6Interface>.Default.GetHashCode(this.<Interface>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<string>>.Default.GetHashCode(this.<DefaultGateways>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<IpWithMask>>.Default.GetHashCode(this.<OtherStaticRoutes>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyDictionary<int, IReadOnlyList<string>>>.Default.GetHashCode(this.<NatGroups>k__BackingField);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00010E41 File Offset: 0x0000F041
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as N6State);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00010E50 File Offset: 0x0000F050
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(N6State other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<N6Interface>.Default.Equals(this.<Interface>k__BackingField, other.<Interface>k__BackingField) && EqualityComparer<IReadOnlyList<string>>.Default.Equals(this.<DefaultGateways>k__BackingField, other.<DefaultGateways>k__BackingField) && EqualityComparer<IReadOnlyList<IpWithMask>>.Default.Equals(this.<OtherStaticRoutes>k__BackingField, other.<OtherStaticRoutes>k__BackingField) && EqualityComparer<IReadOnlyDictionary<int, IReadOnlyList<string>>>.Default.Equals(this.<NatGroups>k__BackingField, other.<NatGroups>k__BackingField));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00010EE1 File Offset: 0x0000F0E1
		[CompilerGenerated]
		private N6State(N6State original)
		{
			this.Interface = original.<Interface>k__BackingField;
			this.DefaultGateways = original.<DefaultGateways>k__BackingField;
			this.OtherStaticRoutes = original.<OtherStaticRoutes>k__BackingField;
			this.NatGroups = original.<NatGroups>k__BackingField;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00010F19 File Offset: 0x0000F119
		[CompilerGenerated]
		public void Deconstruct(out N6Interface Interface, out IReadOnlyList<string> DefaultGateways, out IReadOnlyList<IpWithMask> OtherStaticRoutes, out IReadOnlyDictionary<int, IReadOnlyList<string>> NatGroups)
		{
			Interface = this.Interface;
			DefaultGateways = this.DefaultGateways;
			OtherStaticRoutes = this.OtherStaticRoutes;
			NatGroups = this.NatGroups;
		}
	}
}
