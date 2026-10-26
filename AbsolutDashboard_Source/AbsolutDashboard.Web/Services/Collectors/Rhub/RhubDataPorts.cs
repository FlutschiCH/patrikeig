using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	public static class RhubDataPorts
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000DF64 File Offset: 0x0000C164
		public static List<RhubPort> From(IEnumerable<Metric> metrics, DateTimeOffset? asOf = null)
		{
			SortedDictionary<int, Dictionary<string, string>> sortedDictionary = new SortedDictionary<int, Dictionary<string, string>>();
			foreach (Metric metric in metrics)
			{
				Match match = RhubDataPorts.AlarmLabel.Match(metric.Label);
				if (match.Success && !string.IsNullOrWhiteSpace(metric.Value))
				{
					int num = int.Parse(match.Groups["n"].Value);
					Dictionary<string, string> dictionary;
					if (!sortedDictionary.TryGetValue(num, out dictionary))
					{
						dictionary = (sortedDictionary[num] = new Dictionary<string, string>(StringComparer.Ordinal));
					}
					dictionary[match.Groups["flag"].Value] = metric.Value.Trim();
				}
			}
			List<RhubPort> list = new List<RhubPort>(sortedDictionary.Count);
			foreach (KeyValuePair<int, Dictionary<string, string>> keyValuePair in sortedDictionary)
			{
				int num2;
				Dictionary<string, string> dictionary2;
				keyValuePair.Deconstruct(out num2, out dictionary2);
				int num3 = num2;
				Dictionary<string, string> dictionary3 = dictionary2;
				string valueOrDefault = dictionary3.GetValueOrDefault("TransceiverFailure");
				string valueOrDefault2 = dictionary3.GetValueOrDefault("CPRISync");
				string valueOrDefault3 = dictionary3.GetValueOrDefault("LOS");
				string text2;
				PortState portState;
				if (!RhubDataPorts.Is(valueOrDefault, "Alarm"))
				{
					if (!RhubDataPorts.Is(valueOrDefault2, "Normal") || !RhubDataPorts.Is(valueOrDefault3, "Normal"))
					{
						if (!RhubDataPorts.Is(valueOrDefault3, "Alarm"))
						{
							string text = RhubDataPorts.Describe(dictionary3);
							text2 = text;
							portState = PortState.Unknown;
						}
						else
						{
							portState = PortState.Idle;
							text2 = "no signal (LOS)";
						}
					}
					else
					{
						portState = PortState.Ok;
						text2 = "link up";
					}
				}
				else
				{
					portState = PortState.Fault;
					text2 = "transceiver failure";
				}
				list.Add(new RhubPort(num3, PortKind.Data, portState, text2, asOf));
			}
			return list;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000E148 File Offset: 0x0000C348
		private static bool Is([Nullable(2)] string value, string expected)
		{
			return string.Equals(value, expected, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000E154 File Offset: 0x0000C354
		private static string Describe(Dictionary<string, string> flags)
		{
			return string.Join(", ", from f in flags
				orderby f.Key
				select f.Key + "=" + f.Value);
		}

		// Token: 0x04000143 RID: 323
		private static readonly Regex AlarmLabel = new Regex("^OP(?<n>\\d+)(?<flag>CPRISync|LOS|TransceiverFailure)Alarm$", RegexOptions.Compiled);

		// Token: 0x04000144 RID: 324
		private const string Alarm = "Alarm";
	}
}
