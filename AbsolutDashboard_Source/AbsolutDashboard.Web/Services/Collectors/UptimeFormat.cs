using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	[Nullable(0)]
	public static class UptimeFormat
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		public static string Format(TimeSpan t)
		{
			if (t.TotalDays >= 1.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)t.TotalDays);
				defaultInterpolatedStringHandler.AppendLiteral("d ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(t.Hours);
				defaultInterpolatedStringHandler.AppendLiteral("h");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (t.TotalHours >= 1.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<int>(t.Hours);
				defaultInterpolatedStringHandler2.AppendLiteral("h ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(t.Minutes);
				defaultInterpolatedStringHandler2.AppendLiteral("m");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (t.TotalMinutes >= 1.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler3.AppendFormatted<int>(t.Minutes);
				defaultInterpolatedStringHandler3.AppendLiteral("m");
				return defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler4.AppendFormatted<int>(t.Seconds);
			defaultInterpolatedStringHandler4.AppendLiteral("s");
			return defaultInterpolatedStringHandler4.ToStringAndClear();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000DC17 File Offset: 0x0000BE17
		public static string FromSeconds(long seconds)
		{
			return UptimeFormat.Format(TimeSpan.FromSeconds(seconds));
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000DC24 File Offset: 0x0000BE24
		public static string FromTimeTicks([Nullable(2)] string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
			{
				return raw ?? "";
			}
			string text = raw.Trim();
			long num;
			if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) && num >= 0L)
			{
				return UptimeFormat.Format(TimeSpan.FromSeconds((double)num / 100.0));
			}
			TimeSpan timeSpan;
			if (TimeSpan.TryParse(text, CultureInfo.InvariantCulture, out timeSpan))
			{
				return UptimeFormat.Format(timeSpan);
			}
			return text;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000DC90 File Offset: 0x0000BE90
		[NullableContext(2)]
		public static double? ToSeconds(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			double num = 0.0;
			bool flag = false;
			foreach (object obj in UptimeFormat.UptimeToken.Matches(value))
			{
				Match match = (Match)obj;
				double num2;
				if (double.TryParse(match.Groups["n"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
				{
					char c = char.ToLowerInvariant(match.Groups["u"].Value[0]);
					double num3;
					if (c <= 'h')
					{
						if (c != 'd')
						{
							if (c != 'h')
							{
								goto IL_00F3;
							}
							num3 = 3600.0;
						}
						else
						{
							num3 = 86400.0;
						}
					}
					else if (c != 'm')
					{
						if (c != 's')
						{
							if (c != 'w')
							{
								goto IL_00F3;
							}
							num3 = 604800.0;
						}
						else
						{
							num3 = 1.0;
						}
					}
					else
					{
						num3 = 60.0;
					}
					IL_00FE:
					double num4 = num3;
					if (num4 != 0.0)
					{
						num += num2 * num4;
						flag = true;
						continue;
					}
					continue;
					IL_00F3:
					num3 = 0.0;
					goto IL_00FE;
				}
			}
			if (!flag)
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x04000141 RID: 321
		private static readonly Regex UptimeToken = new Regex("(?<n>\\d+)\\s*(?<u>weeks?|days?|hours?|minutes?|min|seconds?|sec|[wdhms])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
