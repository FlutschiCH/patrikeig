using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Trend : IEquatable<Trend>
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008B77 File Offset: 0x00006D77
		public Trend(string Title, [Nullable(2)] string Unit, string Group, List<HistoryPoint> Points)
		{
			this.Title = Title;
			this.Unit = Unit;
			this.Group = Group;
			this.Points = Points;
			base..ctor();
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008B9C File Offset: 0x00006D9C
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(Trend);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00008BA8 File Offset: 0x00006DA8
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00008BB0 File Offset: 0x00006DB0
		public string Title { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00008BB9 File Offset: 0x00006DB9
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00008BC1 File Offset: 0x00006DC1
		[Nullable(2)]
		public string Unit
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00008BCA File Offset: 0x00006DCA
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00008BD2 File Offset: 0x00006DD2
		public string Group { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00008BDB File Offset: 0x00006DDB
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00008BE3 File Offset: 0x00006DE3
		public List<HistoryPoint> Points { get; set; }

		// Token: 0x06000196 RID: 406 RVA: 0x00008BEC File Offset: 0x00006DEC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Trend");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008C38 File Offset: 0x00006E38
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Title = ");
			builder.Append(this.Title);
			builder.Append(", Unit = ");
			builder.Append(this.Unit);
			builder.Append(", Group = ");
			builder.Append(this.Group);
			builder.Append(", Points = ");
			builder.Append(this.Points);
			return true;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008CAF File Offset: 0x00006EAF
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(Trend left, Trend right)
		{
			return !(left == right);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008CBB File Offset: 0x00006EBB
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(Trend left, Trend right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008CD0 File Offset: 0x00006ED0
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Title>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Unit>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Group>k__BackingField)) * -1521134295 + EqualityComparer<List<HistoryPoint>>.Default.GetHashCode(this.<Points>k__BackingField);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008D49 File Offset: 0x00006F49
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Trend);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008D58 File Offset: 0x00006F58
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(Trend other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Title>k__BackingField, other.<Title>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Unit>k__BackingField, other.<Unit>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Group>k__BackingField, other.<Group>k__BackingField) && EqualityComparer<List<HistoryPoint>>.Default.Equals(this.<Points>k__BackingField, other.<Points>k__BackingField));
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008DE9 File Offset: 0x00006FE9
		[CompilerGenerated]
		private Trend(Trend original)
		{
			this.Title = original.<Title>k__BackingField;
			this.Unit = original.<Unit>k__BackingField;
			this.Group = original.<Group>k__BackingField;
			this.Points = original.<Points>k__BackingField;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008E21 File Offset: 0x00007021
		[CompilerGenerated]
		public void Deconstruct(out string Title, [Nullable(2)] out string Unit, out string Group, out List<HistoryPoint> Points)
		{
			Title = this.Title;
			Unit = this.Unit;
			Group = this.Group;
			Points = this.Points;
		}
	}
}
