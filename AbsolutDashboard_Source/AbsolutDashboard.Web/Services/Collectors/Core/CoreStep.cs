using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoreStep : IEquatable<CoreStep>
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x000121F2 File Offset: 0x000103F2
		public CoreStep(string Label, string Command, bool Ok, string Reply)
		{
			this.Label = Label;
			this.Command = Command;
			this.Ok = Ok;
			this.Reply = Reply;
			base..ctor();
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00012217 File Offset: 0x00010417
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CoreStep);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00012223 File Offset: 0x00010423
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0001222B File Offset: 0x0001042B
		public string Label { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00012234 File Offset: 0x00010434
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0001223C File Offset: 0x0001043C
		public string Command { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00012245 File Offset: 0x00010445
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0001224D File Offset: 0x0001044D
		public bool Ok { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00012256 File Offset: 0x00010456
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0001225E File Offset: 0x0001045E
		public string Reply { get; set; }

		// Token: 0x060003F8 RID: 1016 RVA: 0x00012268 File Offset: 0x00010468
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CoreStep");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000122B4 File Offset: 0x000104B4
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
			builder.Append(", Reply = ");
			builder.Append(this.Reply);
			return true;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00012339 File Offset: 0x00010539
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(CoreStep left, CoreStep right)
		{
			return !(left == right);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00012345 File Offset: 0x00010545
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(CoreStep left, CoreStep right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001235C File Offset: 0x0001055C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Label>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Command>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Ok>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Reply>k__BackingField);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000123D5 File Offset: 0x000105D5
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CoreStep);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000123E4 File Offset: 0x000105E4
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(CoreStep other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Label>k__BackingField, other.<Label>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Command>k__BackingField, other.<Command>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Ok>k__BackingField, other.<Ok>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Reply>k__BackingField, other.<Reply>k__BackingField));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00012475 File Offset: 0x00010675
		[CompilerGenerated]
		private CoreStep(CoreStep original)
		{
			this.Label = original.<Label>k__BackingField;
			this.Command = original.<Command>k__BackingField;
			this.Ok = original.<Ok>k__BackingField;
			this.Reply = original.<Reply>k__BackingField;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000124AD File Offset: 0x000106AD
		[CompilerGenerated]
		public void Deconstruct(out string Label, out string Command, out bool Ok, out string Reply)
		{
			Label = this.Label;
			Command = this.Command;
			Ok = this.Ok;
			Reply = this.Reply;
		}
	}
}
