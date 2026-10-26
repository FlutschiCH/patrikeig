using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class N6Interface : IEquatable<N6Interface>
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0001081B File Offset: 0x0000EA1B
		public N6Interface(string Name, [Nullable(2)] IpWithMask Primary, IReadOnlyList<IpWithMask> Secondaries, int? NatOutboundAcl, int? NatAddressGroup)
		{
			this.Name = Name;
			this.Primary = Primary;
			this.Secondaries = Secondaries;
			this.NatOutboundAcl = NatOutboundAcl;
			this.NatAddressGroup = NatAddressGroup;
			base..ctor();
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00010848 File Offset: 0x0000EA48
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(N6Interface);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00010854 File Offset: 0x0000EA54
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0001085C File Offset: 0x0000EA5C
		public string Name { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00010865 File Offset: 0x0000EA65
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0001086D File Offset: 0x0000EA6D
		[Nullable(2)]
		public IpWithMask Primary
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00010876 File Offset: 0x0000EA76
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0001087E File Offset: 0x0000EA7E
		public IReadOnlyList<IpWithMask> Secondaries { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00010887 File Offset: 0x0000EA87
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0001088F File Offset: 0x0000EA8F
		public int? NatOutboundAcl { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00010898 File Offset: 0x0000EA98
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x000108A0 File Offset: 0x0000EAA0
		public int? NatAddressGroup { get; set; }

		// Token: 0x060003A8 RID: 936 RVA: 0x000108AC File Offset: 0x0000EAAC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("N6Interface");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000108F8 File Offset: 0x0000EAF8
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Name = ");
			builder.Append(this.Name);
			builder.Append(", Primary = ");
			builder.Append(this.Primary);
			builder.Append(", Secondaries = ");
			builder.Append(this.Secondaries);
			builder.Append(", NatOutboundAcl = ");
			builder.Append(this.NatOutboundAcl.ToString());
			builder.Append(", NatAddressGroup = ");
			builder.Append(this.NatAddressGroup.ToString());
			return true;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000109A4 File Offset: 0x0000EBA4
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(N6Interface left, N6Interface right)
		{
			return !(left == right);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000109B0 File Offset: 0x0000EBB0
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(N6Interface left, N6Interface right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000109C4 File Offset: 0x0000EBC4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Name>k__BackingField)) * -1521134295 + EqualityComparer<IpWithMask>.Default.GetHashCode(this.<Primary>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<IpWithMask>>.Default.GetHashCode(this.<Secondaries>k__BackingField)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.<NatOutboundAcl>k__BackingField)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.<NatAddressGroup>k__BackingField);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00010A54 File Offset: 0x0000EC54
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as N6Interface);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00010A64 File Offset: 0x0000EC64
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(N6Interface other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Name>k__BackingField, other.<Name>k__BackingField) && EqualityComparer<IpWithMask>.Default.Equals(this.<Primary>k__BackingField, other.<Primary>k__BackingField) && EqualityComparer<IReadOnlyList<IpWithMask>>.Default.Equals(this.<Secondaries>k__BackingField, other.<Secondaries>k__BackingField) && EqualityComparer<int?>.Default.Equals(this.<NatOutboundAcl>k__BackingField, other.<NatOutboundAcl>k__BackingField) && EqualityComparer<int?>.Default.Equals(this.<NatAddressGroup>k__BackingField, other.<NatAddressGroup>k__BackingField));
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00010B14 File Offset: 0x0000ED14
		[CompilerGenerated]
		private N6Interface(N6Interface original)
		{
			this.Name = original.<Name>k__BackingField;
			this.Primary = original.<Primary>k__BackingField;
			this.Secondaries = original.<Secondaries>k__BackingField;
			this.NatOutboundAcl = original.<NatOutboundAcl>k__BackingField;
			this.NatAddressGroup = original.<NatAddressGroup>k__BackingField;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00010B63 File Offset: 0x0000ED63
		[CompilerGenerated]
		public void Deconstruct(out string Name, [Nullable(2)] out IpWithMask Primary, out IReadOnlyList<IpWithMask> Secondaries, out int? NatOutboundAcl, out int? NatAddressGroup)
		{
			Name = this.Name;
			Primary = this.Primary;
			Secondaries = this.Secondaries;
			NatOutboundAcl = this.NatOutboundAcl;
			NatAddressGroup = this.NatAddressGroup;
		}
	}
}
