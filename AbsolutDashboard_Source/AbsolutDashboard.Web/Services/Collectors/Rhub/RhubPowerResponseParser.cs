using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	public static class RhubPowerResponseParser
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000E8DC File Offset: 0x0000CADC
		public static RhubPowerParseResult Parse([Nullable(2)] string raw, DateTimeOffset? asOf = null)
		{
			if (raw == null)
			{
				raw = string.Empty;
			}
			string[] array = raw.Replace("\r", "").Split('\n', StringSplitOptions.None);
			List<RhubPort> list = new List<RhubPort>();
			HashSet<int> hashSet = new HashSet<int>();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i].Trim();
				if (text.Length != 0)
				{
					if (text.Equals("timeout!", StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
					else if (text.StartsWith("Enter Password->", StringComparison.OrdinalIgnoreCase))
					{
						if (flag4)
						{
							flag2 = true;
						}
					}
					else if (text.StartsWith("->"))
					{
						flag3 = true;
					}
					else if (!text.Equals("noloadget", StringComparison.OrdinalIgnoreCase))
					{
						Match match = RhubPowerResponseParser.ChannelLine.Match(text);
						if (match.Success)
						{
							int num = int.Parse(match.Groups[1].Value);
							string text2 = match.Groups[2].Value.Trim();
							flag4 = true;
							if (hashSet.Add(num))
							{
								Match match2 = RhubPowerResponseParser.LoadStatus.Match(text2);
								PortState portState;
								if (!match2.Success)
								{
									portState = PortState.Unknown;
								}
								else
								{
									string text3 = match2.Groups["v"].Value.Trim();
									PortState portState2;
									if (text3.Equals("load", StringComparison.OrdinalIgnoreCase))
									{
										portState2 = PortState.Ok;
									}
									else if (text3.Equals("no-load", StringComparison.OrdinalIgnoreCase))
									{
										portState2 = PortState.Idle;
									}
									else
									{
										portState2 = PortState.Unknown;
									}
									portState = portState2;
								}
								PortState portState3 = portState;
								list.Add(new RhubPort(num, PortKind.Power, portState3, text2, asOf));
							}
						}
					}
				}
			}
			list.Sort((RhubPort a, RhubPort b) => a.Index.CompareTo(b.Index));
			bool flag5 = !flag3 && !flag;
			return new RhubPowerParseResult(list, flag, flag2, flag5, raw);
		}

		// Token: 0x04000158 RID: 344
		private static readonly Regex ChannelLine = new Regex("^\\s*Channel\\[(\\d+)\\]\\s+(.*\\S)\\s*$", RegexOptions.Compiled);

		// Token: 0x04000159 RID: 345
		private static readonly Regex LoadStatus = new Regex("^is\\s+(?<v>\\S+)\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
