using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IpWithMask : IEquatable<IpWithMask>
	{
		// Token: 0x0600038C RID: 908 RVA: 0x00010681 File Offset: 0x0000E881
		public IpWithMask(string Address, string Mask)
		{
			this.Address = Address;
			this.Mask = Mask;
			base..ctor();
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00010697 File Offset: 0x0000E897
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(IpWithMask);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000106A3 File Offset: 0x0000E8A3
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000106AB File Offset: 0x0000E8AB
		public string Address { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000106B4 File Offset: 0x0000E8B4
		// (set) Token: 0x06000391 RID: 913 RVA: 0x000106BC File Offset: 0x0000E8BC
		public string Mask { get; set; }

		// Token: 0x06000392 RID: 914 RVA: 0x000106C5 File Offset: 0x0000E8C5
		public override string ToString()
		{
			return this.Address + " " + this.Mask;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000106DD File Offset: 0x0000E8DD
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Address = ");
			builder.Append(this.Address);
			builder.Append(", Mask = ");
			builder.Append(this.Mask);
			return true;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010717 File Offset: 0x0000E917
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(IpWithMask left, IpWithMask right)
		{
			return !(left == right);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00010723 File Offset: 0x0000E923
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(IpWithMask left, IpWithMask right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00010737 File Offset: 0x0000E937
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Address>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Mask>k__BackingField);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00010777 File Offset: 0x0000E977
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as IpWithMask);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010788 File Offset: 0x0000E988
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(IpWithMask other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Address>k__BackingField, other.<Address>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Mask>k__BackingField, other.<Mask>k__BackingField));
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000107E9 File Offset: 0x0000E9E9
		[CompilerGenerated]
		private IpWithMask(IpWithMask original)
		{
			this.Address = original.<Address>k__BackingField;
			this.Mask = original.<Mask>k__BackingField;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00010809 File Offset: 0x0000EA09
		[CompilerGenerated]
		public void Deconstruct(out string Address, out string Mask)
		{
			Address = this.Address;
			Mask = this.Mask;
		}
	}
}
