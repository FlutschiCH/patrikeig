using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x02000063 RID: 99
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct RhubDialogStep : IEquatable<RhubDialogStep>
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000F0AA File Offset: 0x0000D2AA
		public RhubDialogStep(RhubDialogAction Action, string Send = null, string Reason = null)
		{
			this.Action = Action;
			this.Send = Send;
			this.Reason = Reason;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000F0C1 File Offset: 0x0000D2C1
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000F0C9 File Offset: 0x0000D2C9
		public RhubDialogAction Action { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000F0D2 File Offset: 0x0000D2D2
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000F0DA File Offset: 0x0000D2DA
		public string Send { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000F0E3 File Offset: 0x0000D2E3
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000F0EB File Offset: 0x0000D2EB
		public string Reason { get; set; }

		// Token: 0x0600031F RID: 799 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("RhubDialogStep");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000F140 File Offset: 0x0000D340
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Action = ");
			builder.Append(this.Action.ToString());
			builder.Append(", Send = ");
			builder.Append(this.Send);
			builder.Append(", Reason = ");
			builder.Append(this.Reason);
			return true;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000F1A7 File Offset: 0x0000D3A7
		[CompilerGenerated]
		public static bool operator !=(RhubDialogStep left, RhubDialogStep right)
		{
			return !(left == right);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000F1B3 File Offset: 0x0000D3B3
		[CompilerGenerated]
		public static bool operator ==(RhubDialogStep left, RhubDialogStep right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000F1BD File Offset: 0x0000D3BD
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<RhubDialogAction>.Default.GetHashCode(this.<Action>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Send>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Reason>k__BackingField);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000F1FD File Offset: 0x0000D3FD
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is RhubDialogStep && this.Equals((RhubDialogStep)obj);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000F218 File Offset: 0x0000D418
		[CompilerGenerated]
		public bool Equals(RhubDialogStep other)
		{
			return EqualityComparer<RhubDialogAction>.Default.Equals(this.<Action>k__BackingField, other.<Action>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Send>k__BackingField, other.<Send>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Reason>k__BackingField, other.<Reason>k__BackingField);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000F26D File Offset: 0x0000D46D
		[CompilerGenerated]
		public void Deconstruct(out RhubDialogAction Action, out string Send, out string Reason)
		{
			Action = this.Action;
			Send = this.Send;
			Reason = this.Reason;
		}
	}
}
