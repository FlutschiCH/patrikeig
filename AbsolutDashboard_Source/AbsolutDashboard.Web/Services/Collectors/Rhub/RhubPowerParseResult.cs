using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x0200005F RID: 95
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RhubPowerParseResult : IEquatable<RhubPowerParseResult>
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0000E557 File Offset: 0x0000C757
		public RhubPowerParseResult(IReadOnlyList<RhubPort> Ports, bool TimedOut, bool PasswordPromptMidStream, bool Truncated, string RawResponse)
		{
			this.Ports = Ports;
			this.TimedOut = TimedOut;
			this.PasswordPromptMidStream = PasswordPromptMidStream;
			this.Truncated = Truncated;
			this.RawResponse = RawResponse;
			base..ctor();
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000E584 File Offset: 0x0000C784
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(RhubPowerParseResult);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000E590 File Offset: 0x0000C790
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000E598 File Offset: 0x0000C798
		public IReadOnlyList<RhubPort> Ports { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000E5A1 File Offset: 0x0000C7A1
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000E5A9 File Offset: 0x0000C7A9
		public bool TimedOut { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000E5B2 File Offset: 0x0000C7B2
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000E5BA File Offset: 0x0000C7BA
		public bool PasswordPromptMidStream { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000E5C3 File Offset: 0x0000C7C3
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000E5CB File Offset: 0x0000C7CB
		public bool Truncated { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public string RawResponse { get; set; }

		// Token: 0x06000304 RID: 772 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("RhubPowerParseResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000E634 File Offset: 0x0000C834
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Ports = ");
			builder.Append(this.Ports);
			builder.Append(", TimedOut = ");
			builder.Append(this.TimedOut.ToString());
			builder.Append(", PasswordPromptMidStream = ");
			builder.Append(this.PasswordPromptMidStream.ToString());
			builder.Append(", Truncated = ");
			builder.Append(this.Truncated.ToString());
			builder.Append(", RawResponse = ");
			builder.Append(this.RawResponse);
			return true;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000E6EE File Offset: 0x0000C8EE
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(RhubPowerParseResult left, RhubPowerParseResult right)
		{
			return !(left == right);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000E6FA File Offset: 0x0000C8FA
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(RhubPowerParseResult left, RhubPowerParseResult right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000E710 File Offset: 0x0000C910
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<IReadOnlyList<RhubPort>>.Default.GetHashCode(this.<Ports>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<TimedOut>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<PasswordPromptMidStream>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Truncated>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RawResponse>k__BackingField);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as RhubPowerParseResult);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000E7B0 File Offset: 0x0000C9B0
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(RhubPowerParseResult other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<IReadOnlyList<RhubPort>>.Default.Equals(this.<Ports>k__BackingField, other.<Ports>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<TimedOut>k__BackingField, other.<TimedOut>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<PasswordPromptMidStream>k__BackingField, other.<PasswordPromptMidStream>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Truncated>k__BackingField, other.<Truncated>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RawResponse>k__BackingField, other.<RawResponse>k__BackingField));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E860 File Offset: 0x0000CA60
		[CompilerGenerated]
		private RhubPowerParseResult(RhubPowerParseResult original)
		{
			this.Ports = original.<Ports>k__BackingField;
			this.TimedOut = original.<TimedOut>k__BackingField;
			this.PasswordPromptMidStream = original.<PasswordPromptMidStream>k__BackingField;
			this.Truncated = original.<Truncated>k__BackingField;
			this.RawResponse = original.<RawResponse>k__BackingField;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E8AF File Offset: 0x0000CAAF
		[CompilerGenerated]
		public void Deconstruct(out IReadOnlyList<RhubPort> Ports, out bool TimedOut, out bool PasswordPromptMidStream, out bool Truncated, out string RawResponse)
		{
			Ports = this.Ports;
			TimedOut = this.TimedOut;
			PasswordPromptMidStream = this.PasswordPromptMidStream;
			Truncated = this.Truncated;
			RawResponse = this.RawResponse;
		}
	}
}
