using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BtiAuParser : ISshOutputParser
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000B6E4 File Offset: 0x000098E4
		public string Key
		{
			get
			{
				return "bti-au";
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B6EC File Offset: 0x000098EC
		public DeviceSnapshot Parse(IReadOnlyDictionary<string, string> outputs)
		{
			DeviceSnapshot deviceSnapshot = new DeviceSnapshot();
			foreach (KeyValuePair<string, string> keyValuePair in outputs)
			{
				string text;
				string text2;
				keyValuePair.Deconstruct(out text, out text2);
				string text3 = text;
				string text4 = text2;
				string text5 = text3.ToLowerInvariant();
				if (text5.Contains("list") && (text5.Contains("eu") || text5.Contains("ru") || text5.Contains("ue")))
				{
					BtiAuParser.ParseChildTable(text4, BtiAuParser.KindFromCommand(text5), deviceSnapshot);
				}
				else
				{
					foreach (ValueTuple<string, string> valueTuple in CliParse.KeyValues(text4))
					{
						string item = valueTuple.Item1;
						string item2 = valueTuple.Item2;
						deviceSnapshot.Metrics.Add(new Metric(item, item2, null, "Cell"));
					}
				}
			}
			return deviceSnapshot;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B7FC File Offset: 0x000099FC
		private static string KindFromCommand(string c)
		{
			if (c.Contains("ru"))
			{
				return "ru";
			}
			if (!c.Contains("ue"))
			{
				return "eu";
			}
			return "ue";
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B82C File Offset: 0x00009A2C
		private static void ParseChildTable(string text, string kind, DeviceSnapshot s)
		{
			foreach (string text2 in CliParse.Lines(text))
			{
				if (!Regex.IsMatch(text2, "^[\\s\\-=+|]+$"))
				{
					string[] array = (from x in Regex.Split(text2.Trim(), "\\s{2,}|\\t|\\s\\|\\s|\\|")
						where x.Trim().Length > 0
						select x.Trim()).ToArray<string>();
					if (array.Length >= 2)
					{
						string text3 = array.FirstOrDefault<string>((string x) => CliParse.IsUp(x) != null);
						if (text3 == null)
						{
							if (array.Any<string>((string x) => x.Equals("status", StringComparison.OrdinalIgnoreCase)))
							{
								continue;
							}
						}
						List<ChildStatus> children = s.Children;
						ChildStatus childStatus = new ChildStatus();
						childStatus.Identifier = array[0];
						childStatus.Kind = kind;
						childStatus.Up = ((text3 == null) ? null : CliParse.IsUp(text3));
						childStatus.State = text3 ?? array.ElementAtOrDefault<string>(1);
						childStatus.Metrics = array.Skip<string>(1).Select<string, Metric>(delegate(string v, int i)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
							defaultInterpolatedStringHandler.AppendLiteral("col");
							defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
							return new Metric(defaultInterpolatedStringHandler.ToStringAndClear(), v, null, null);
						}).ToList<Metric>();
						children.Add(childStatus);
					}
				}
			}
		}
	}
}
