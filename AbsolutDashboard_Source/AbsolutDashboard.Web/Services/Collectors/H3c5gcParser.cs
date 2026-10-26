using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class H3c5gcParser : ISshOutputParser
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000B9DC File Offset: 0x00009BDC
		public string Key
		{
			get
			{
				return "h3c-5gc";
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B9E4 File Offset: 0x00009BE4
		public DeviceSnapshot Parse(IReadOnlyDictionary<string, string> outputs)
		{
			string text = string.Join("\n", outputs.Values);
			DeviceSnapshot deviceSnapshot = new DeviceSnapshot();
			H3c5gcParser.ParseHealth(text, deviceSnapshot);
			H3c5gcParser.ParseMobileNetwork(text, deviceSnapshot);
			List<H3c5gcParser.UpfSession> list = H3c5gcParser.ParseUpfSessions(text);
			Dictionary<string, List<H3c5gcParser.SmfSession>> dictionary = H3c5gcParser.ParseSmf(text);
			foreach (IGrouping<string, H3c5gcParser.UpfSession> grouping in from u in list
				group u by u.Imsi)
			{
				deviceSnapshot.Children.Add(H3c5gcParser.BuildUe(grouping.Key, grouping.ToList<H3c5gcParser.UpfSession>(), dictionary.GetValueOrDefault(grouping.Key)));
			}
			H3c5gcParser.EnrichFromAmf(text, deviceSnapshot);
			if (deviceSnapshot.Children.Count > 0)
			{
				double num = 0.0;
				double num2 = 0.0;
				foreach (ChildStatus childStatus in deviceSnapshot.Children)
				{
					num += H3c5gcParser.ChildMbps(childStatus, "DL");
					num2 += H3c5gcParser.ChildMbps(childStatus, "UL");
				}
				deviceSnapshot.Metrics.Add(new Metric("Total DL", num.ToString("0.##", CultureInfo.InvariantCulture), "Mbps", "Throughput"));
				deviceSnapshot.Metrics.Add(new Metric("Total UL", num2.ToString("0.##", CultureInfo.InvariantCulture), "Mbps", "Throughput"));
				int count = deviceSnapshot.Children.Count;
				int num3 = deviceSnapshot.Children.Count<ChildStatus>((ChildStatus c) => string.Equals(c.State, "Connected", StringComparison.OrdinalIgnoreCase));
				deviceSnapshot.Metrics.Add(new Metric("Registered UEs", count.ToString(), null, "System"));
				deviceSnapshot.Metrics.Add(new Metric("Active UEs", num3.ToString(), null, "System"));
			}
			return deviceSnapshot;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000BC18 File Offset: 0x00009E18
		private static void EnrichFromAmf(string text, DeviceSnapshot s)
		{
			Match match = H3c5gcParser.AmfGnbRow.Match(text);
			if (match.Success)
			{
				s.Metrics.Add(new Metric("gNB ID", match.Groups["gnb"].Value, null, "Radio"));
				s.Metrics.Add(new Metric("NG link", match.Groups["link"].Value, null, "Radio"));
			}
			Dictionary<string, string> dictionary = H3c5gcParser.ParseAmfRoster(text);
			Dictionary<string, H3c5gcParser.AmfUeContext> dictionary2 = H3c5gcParser.ParseAmfUeContexts(text);
			Dictionary<string, ValueTuple<string, string>> dictionary3 = H3c5gcParser.ParsePings(text);
			if (dictionary.Count == 0 && dictionary2.Count == 0 && dictionary3.Count == 0)
			{
				return;
			}
			foreach (ChildStatus childStatus in s.Children)
			{
				Metric metric = childStatus.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "IMSI");
				string text2 = ((metric != null) ? metric.Value : null);
				if (!string.IsNullOrEmpty(text2))
				{
					H3c5gcParser.AmfUeContext valueOrDefault = dictionary2.GetValueOrDefault(text2);
					string text3 = ((valueOrDefault != null) ? valueOrDefault.Cell : null) ?? dictionary.GetValueOrDefault(text2);
					string text4 = ((valueOrDefault != null) ? valueOrDefault.Gnb : null) ?? (match.Success ? match.Groups["gnb"].Value : null);
					if (text4 != null && text3 != null)
					{
						childStatus.Metrics.Add(new Metric("Base station", "gNB " + text4 + " / cell " + text3, null, "UE"));
					}
					else if (text3 != null)
					{
						childStatus.Metrics.Add(new Metric("Cell", text3, null, "UE"));
					}
					if (text4 != null)
					{
						childStatus.Metrics.Add(new Metric("gNB ID", text4, null, "UE"));
					}
					if (valueOrDefault != null)
					{
						H3c5gcParser.<EnrichFromAmf>g__Add|31_0(childStatus, "5G-TMSI", valueOrDefault.Tmsi);
						H3c5gcParser.<EnrichFromAmf>g__Add|31_0(childStatus, "RM state", valueOrDefault.RmState);
						H3c5gcParser.<EnrichFromAmf>g__Add|31_0(childStatus, "CM state", valueOrDefault.CmState);
						H3c5gcParser.<EnrichFromAmf>g__Add|31_0(childStatus, "RRC state", valueOrDefault.RrcState);
						H3c5gcParser.<EnrichFromAmf>g__Add|31_0(childStatus, "TAI", valueOrDefault.Tai);
						if (valueOrDefault.AmbrDl != null)
						{
							childStatus.Metrics.Add(new Metric("AMBR DL", valueOrDefault.AmbrDl, "kbps", "UE"));
						}
						if (valueOrDefault.AmbrUl != null)
						{
							childStatus.Metrics.Add(new Metric("AMBR UL", valueOrDefault.AmbrUl, "kbps", "UE"));
						}
					}
					Metric metric2 = childStatus.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "IP");
					string text5 = ((metric2 != null) ? metric2.Value : null);
					ValueTuple<string, string> valueTuple;
					if (text5 != null && dictionary3.TryGetValue(text5, out valueTuple))
					{
						double num2;
						double? num = (double.TryParse(valueTuple.Item2, NumberStyles.Any, CultureInfo.InvariantCulture, out num2) ? new double?(num2) : null);
						double? num3;
						double num4;
						if (valueTuple.Item1 != null)
						{
							childStatus.Metrics.Add(new Metric("Latency", valueTuple.Item1, "ms", "UE"));
						}
						else
						{
							num3 = num;
							num4 = (double)100;
							if ((num3.GetValueOrDefault() >= num4) & (num3 != null))
							{
								childStatus.Metrics.Add(new Metric("Latency", "unreachable", null, "UE"));
							}
						}
						num3 = num;
						num4 = 0.0;
						if ((num3.GetValueOrDefault() > num4) & (num3 != null))
						{
							childStatus.Metrics.Add(new Metric("Packet loss", valueTuple.Item2, "%", "UE"));
						}
					}
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C03C File Offset: 0x0000A23C
		private static Dictionary<string, string> ParseAmfRoster(string text)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			foreach (object obj in H3c5gcParser.AmfRosterRow.Matches(text))
			{
				Match match = (Match)obj;
				dictionary.TryAdd(match.Groups["supi"].Value, match.Groups["cell"].Value);
			}
			return dictionary;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		private static Dictionary<string, H3c5gcParser.AmfUeContext> ParseAmfUeContexts(string text)
		{
			Dictionary<string, H3c5gcParser.AmfUeContext> dictionary = new Dictionary<string, H3c5gcParser.AmfUeContext>(StringComparer.Ordinal);
			foreach (object obj in H3c5gcParser.AmfUeBlock.Matches(text))
			{
				Match match = (Match)obj;
				string text2 = H3c5gcParser.RadioCapability.Replace(match.Groups["body"].Value, "");
				Match match2 = H3c5gcParser.AmfGlobalGnb.Match(text2);
				Match match3 = H3c5gcParser.AmfTai.Match(text2);
				Match match4 = H3c5gcParser.AmfAmbr.Match(text2);
				Dictionary<string, H3c5gcParser.AmfUeContext> dictionary2 = dictionary;
				string value = match.Groups["supi"].Value;
				string text3 = (match2.Success ? match2.Groups["gnb"].Value : null);
				string text4 = H3c5gcParser.Field(text2, "Cell ID");
				string text5 = H3c5gcParser.Field(text2, "5G-TMSI");
				string text6 = H3c5gcParser.Field(text2, "RM state");
				string text7 = H3c5gcParser.Field(text2, "CM state");
				string text8 = H3c5gcParser.Field(text2, "RRC state");
				string text9;
				if (!match3.Success)
				{
					text9 = null;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 3);
					defaultInterpolatedStringHandler.AppendLiteral("MCC ");
					defaultInterpolatedStringHandler.AppendFormatted(match3.Groups["mcc"].Value);
					defaultInterpolatedStringHandler.AppendLiteral(" MNC ");
					defaultInterpolatedStringHandler.AppendFormatted(match3.Groups["mnc"].Value);
					defaultInterpolatedStringHandler.AppendLiteral(" TAC ");
					defaultInterpolatedStringHandler.AppendFormatted(match3.Groups["tac"].Value);
					text9 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				dictionary2.TryAdd(value, new H3c5gcParser.AmfUeContext(text3, text4, text5, text6, text7, text8, text9, match4.Success ? match4.Groups["ul"].Value : null, match4.Success ? match4.Groups["dl"].Value : null));
			}
			return dictionary;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000C2F0 File Offset: 0x0000A4F0
		[return: Nullable(2)]
		private static string Field(string body, string label)
		{
			Match match = Regex.Match(body, "^\\s*" + Regex.Escape(label) + "\\s*:\\s*(?<v>[^\\r\\n]+?)\\s*$", RegexOptions.Multiline);
			if (!match.Success)
			{
				return null;
			}
			return match.Groups["v"].Value;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000C33C File Offset: 0x0000A53C
		[return: TupleElementNames(new string[] { "AvgMs", "LossPct" })]
		[return: Nullable(new byte[] { 1, 1, 0, 2, 2 })]
		private static Dictionary<string, ValueTuple<string, string>> ParsePings(string text)
		{
			Dictionary<string, ValueTuple<string, string>> dictionary = new Dictionary<string, ValueTuple<string, string>>(StringComparer.Ordinal);
			List<Match> list = H3c5gcParser.PingTarget.Matches(text).ToList<Match>();
			for (int i = 0; i < list.Count; i++)
			{
				int index = list[i].Index;
				int num = ((i + 1 < list.Count) ? list[i + 1].Index : text.Length);
				int num2 = index;
				string text2 = text.Substring(num2, num - num2);
				Match match = H3c5gcParser.PingRtt.Match(text2);
				Match match2 = H3c5gcParser.PingLoss.Match(text2);
				if (match.Success || match2.Success)
				{
					dictionary[list[i].Groups["ip"].Value] = new ValueTuple<string, string>(match.Success ? match.Groups["avg"].Value : null, match2.Success ? match2.Groups["loss"].Value : null);
				}
			}
			return dictionary;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C458 File Offset: 0x0000A658
		private static double ChildMbps(ChildStatus ue, string label)
		{
			Metric metric = ue.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == label);
			double num;
			if (!double.TryParse((metric != null) ? metric.Value : null, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
			{
				return 0.0;
			}
			return num;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
		private static void ParseHealth(string text, DeviceSnapshot s)
		{
			Match match = H3c5gcParser.PromptName.Match(text);
			if (match.Success)
			{
				s.Metrics.Add(new Metric("System name", match.Groups[1].Value, null, "System"));
			}
			Match match2 = H3c5gcParser.Uptime.Match(text);
			if (match2.Success)
			{
				string text2 = match2.Groups["model"].Value.Trim();
				if (text2.Length > 0)
				{
					s.Metrics.Add(new Metric("Model", text2, null, "System"));
				}
				s.Metrics.Add(new Metric("Uptime", match2.Groups["up"].Value.Trim(), null, "System"));
			}
			Match match3 = H3c5gcParser.Version.Match(text);
			if (match3.Success)
			{
				s.Metrics.Add(new Metric("Software", match3.Groups["ver"].Value.Trim(), null, "System"));
			}
			Match match4 = H3c5gcParser.Cpu.Match(text);
			if (match4.Success)
			{
				s.Metrics.Add(new Metric("CPU load", match4.Groups["v"].Value, "%", "Resources"));
			}
			Match match5 = H3c5gcParser.Memory.Match(text);
			if (match5.Success)
			{
				s.Metrics.Add(new Metric("Memory usage", match5.Groups["v"].Value, "%", "Resources"));
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C66C File Offset: 0x0000A86C
		private static void ParseMobileNetwork(string text, DeviceSnapshot s)
		{
			Match match = H3c5gcParser.AmfNetwork.Match(text);
			if (match.Success)
			{
				s.Metrics.Add(new Metric("MCC", match.Groups["mcc"].Value, null, "Mobile network"));
				s.Metrics.Add(new Metric("MNC", match.Groups["mnc"].Value, null, "Mobile network"));
				List<string> list = new string[]
				{
					match.Groups["short"].Value,
					match.Groups["full"].Value
				}.Where<string>((string n) => n.Length > 0).Distinct<string>(StringComparer.Ordinal).ToList<string>();
				if (list.Count > 0)
				{
					s.Metrics.Add(new Metric("Network name", string.Join(" / ", list), null, "Mobile network"));
				}
			}
			foreach (object obj in H3c5gcParser.UeIpPool.Matches(text))
			{
				Match match2 = (Match)obj;
				List<string> list2 = (from m in H3c5gcParser.AddressRange.Matches(match2.Groups["body"].Value)
					select m.Groups["from"].Value + " – " + m.Groups["to"].Value).ToList<string>();
				if (list2.Count > 0)
				{
					s.Metrics.Add(new Metric("UE IP pool (" + match2.Groups["name"].Value + ")", string.Join(", ", list2), null, "Mobile network"));
				}
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000C878 File Offset: 0x0000AA78
		internal static List<H3c5gcParser.UpfSession> ParseUpfSessions(string text)
		{
			List<H3c5gcParser.UpfSession> list = new List<H3c5gcParser.UpfSession>();
			foreach (string text2 in H3c5gcParser.SessionSplit.Split(text))
			{
				if (text2.StartsWith("PFCP session ID"))
				{
					H3c5gcParser.UpfSession upfSession = H3c5gcParser.ParseUpfSession(text2);
					if (upfSession != null)
					{
						list.Add(upfSession);
					}
				}
			}
			return list;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C8CC File Offset: 0x0000AACC
		[NullableContext(2)]
		internal static string SmfState([Nullable(new byte[] { 2, 1 })] List<H3c5gcParser.SmfSession> smf)
		{
			if (smf == null || smf.Count <= 0)
			{
				return null;
			}
			if (!smf.Any<H3c5gcParser.SmfSession>((H3c5gcParser.SmfSession x) => string.Equals(x.UpCnxState, "ACTIVATED", StringComparison.OrdinalIgnoreCase)))
			{
				return "Idle";
			}
			return "Connected";
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C91C File Offset: 0x0000AB1C
		[NullableContext(2)]
		internal static string SmfIp([Nullable(new byte[] { 2, 1 })] List<H3c5gcParser.SmfSession> smf)
		{
			if (smf == null || smf.Count <= 0)
			{
				return null;
			}
			H3c5gcParser.SmfSession smfSession = smf.FirstOrDefault<H3c5gcParser.SmfSession>((H3c5gcParser.SmfSession x) => string.Equals(x.Dnn, "default", StringComparison.OrdinalIgnoreCase));
			return ((smfSession != null) ? smfSession.Ip : null) ?? smf[0].Ip;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000C978 File Offset: 0x0000AB78
		private static long? BytesValue(string sectionText)
		{
			Match match = H3c5gcParser.ForwardedBytes.Match(sectionText);
			long num;
			if (!match.Success || !long.TryParse(match.Groups["bytes"].Value, out num))
			{
				return null;
			}
			return new long?(num);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		[return: Nullable(2)]
		private static H3c5gcParser.UpfSession ParseUpfSession(string block)
		{
			Match match = H3c5gcParser.Imsi.Match(block);
			if (!match.Success)
			{
				return null;
			}
			int num = block.IndexOf("Session DL statistics", StringComparison.OrdinalIgnoreCase);
			string text = ((num >= 0) ? block.Substring(0, num) : block);
			string text2 = ((num >= 0) ? block.Substring(num) : "");
			Match match2 = H3c5gcParser.Msisdn.Match(block);
			DateTime? dateTime = null;
			Match match3 = H3c5gcParser.StartTime.Match((text.Length > 0) ? text : block);
			DateTime dateTime2;
			if (match3.Success && DateTime.TryParse(match3.Groups["v"].Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime2))
			{
				dateTime = new DateTime?(dateTime2);
			}
			Match match4 = H3c5gcParser.UeIpAddr.Match(block);
			return new H3c5gcParser.UpfSession(match.Groups["v"].Value, match2.Success ? match2.Groups["v"].Value : null, H3c5gcParser.MbpsValue(text2), H3c5gcParser.MbpsValue(text), dateTime, match4.Success ? match4.Groups["v"].Value : null, H3c5gcParser.BytesValue(text2), H3c5gcParser.BytesValue(text));
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000CB08 File Offset: 0x0000AD08
		internal static Dictionary<string, List<H3c5gcParser.SmfSession>> ParseSmf(string text)
		{
			Dictionary<string, List<H3c5gcParser.SmfSession>> dictionary = new Dictionary<string, List<H3c5gcParser.SmfSession>>(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in H3c5gcParser.SmfBlock.Matches(text))
			{
				Match match = (Match)obj;
				string value = match.Groups["imsi"].Value;
				List<H3c5gcParser.SmfSession> list;
				if (!dictionary.TryGetValue(value, out list))
				{
					list = (dictionary[value] = new List<H3c5gcParser.SmfSession>());
				}
				foreach (object obj2 in H3c5gcParser.SmfSessionRx.Matches(match.Groups["body"].Value))
				{
					Match match2 = (Match)obj2;
					list.Add(new H3c5gcParser.SmfSession(match2.Groups["dnn"].Value, match2.Groups["state"].Value, match2.Groups["ip"].Value));
				}
			}
			return dictionary;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000CC54 File Offset: 0x0000AE54
		private static ChildStatus BuildUe(string imsi, List<H3c5gcParser.UpfSession> upf, [Nullable(new byte[] { 2, 1 })] List<H3c5gcParser.SmfSession> smf)
		{
			string text = upf.Select<H3c5gcParser.UpfSession, string>((H3c5gcParser.UpfSession u) => u.Msisdn).FirstOrDefault<string>((string m) => !string.IsNullOrEmpty(m));
			double num = upf.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession u) => u.DlMbps);
			double num2 = upf.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession u) => u.UlMbps);
			DateTime dateTime = (from u in upf
				where u.Start != null
				select u.Start.Value).DefaultIfEmpty<DateTime>().Min<DateTime>();
			bool flag = smf != null && smf.Count > 0;
			string text2;
			string text3;
			int num3;
			if (flag)
			{
				text2 = (smf.Any<H3c5gcParser.SmfSession>((H3c5gcParser.SmfSession x) => string.Equals(x.UpCnxState, "ACTIVATED", StringComparison.OrdinalIgnoreCase)) ? "Connected" : "Idle");
				H3c5gcParser.SmfSession smfSession = smf.FirstOrDefault<H3c5gcParser.SmfSession>((H3c5gcParser.SmfSession x) => string.Equals(x.Dnn, "default", StringComparison.OrdinalIgnoreCase));
				text3 = ((smfSession != null) ? smfSession.Ip : null) ?? smf[0].Ip;
				num3 = smf.Count;
			}
			else
			{
				text2 = "online";
				text3 = upf.Select<H3c5gcParser.UpfSession, string>((H3c5gcParser.UpfSession u) => u.UeIp).FirstOrDefault<string>((string x) => !string.IsNullOrEmpty(x));
				num3 = upf.Count;
			}
			List<Metric> list = new List<Metric>
			{
				new Metric("MSIN", H3c5gcParser.Msin(imsi), null, "UE"),
				new Metric("IMSI", imsi, null, "UE")
			};
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(new Metric("MSISDN", text, null, "UE"));
			}
			list.Add(new Metric("DL", num.ToString("0.##", CultureInfo.InvariantCulture), "Mbps", "UE"));
			list.Add(new Metric("UL", num2.ToString("0.##", CultureInfo.InvariantCulture), "Mbps", "UE"));
			if (dateTime != default(DateTime))
			{
				TimeSpan timeSpan = DateTime.Now - dateTime;
				if (timeSpan > TimeSpan.Zero)
				{
					list.Add(new Metric("Uptime", UptimeFormat.Format(timeSpan), null, "UE"));
				}
			}
			if (!string.IsNullOrEmpty(text3))
			{
				list.Add(new Metric("IP", text3, null, "UE"));
			}
			list.Add(new Metric("PDU sessions", num3.ToString(), null, "UE"));
			if (flag)
			{
				list.Add(new Metric("Connection", text2, null, "UE"));
			}
			return new ChildStatus
			{
				Identifier = imsi,
				Kind = "ue",
				Up = new bool?(true),
				State = text2,
				Metrics = list
			};
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000CFC7 File Offset: 0x0000B1C7
		private static string Msin(string imsi)
		{
			if (imsi.Length <= 5)
			{
				return imsi;
			}
			return imsi.Substring(5);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		private static double MbpsValue(string sectionText)
		{
			Match match = H3c5gcParser.Forwarded.Match(sectionText);
			long num;
			if (!match.Success || !long.TryParse(match.Groups["bps"].Value, out num))
			{
				return 0.0;
			}
			return (double)num / 1000000.0;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000D203 File Offset: 0x0000B403
		[CompilerGenerated]
		internal static void <EnrichFromAmf>g__Add|31_0(ChildStatus ue, string label, [Nullable(2)] string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				ue.Metrics.Add(new Metric(label, value, null, "UE"));
			}
		}

		// Token: 0x04000103 RID: 259
		private const int MccMncDigits = 5;

		// Token: 0x04000104 RID: 260
		private static readonly Regex PromptName = new Regex("[<\\[]([^>\\]\\s]+?)(?:-probe)?[>\\]]", RegexOptions.Compiled);

		// Token: 0x04000105 RID: 261
		private static readonly Regex Uptime = new Regex("^(?<model>.*?)\\s+uptime is\\s+(?<up>.+?)\\s*$", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000106 RID: 262
		private static readonly Regex Version = new Regex("Comware Software,\\s*(?<ver>Version[^\\r\\n]+)", RegexOptions.Compiled);

		// Token: 0x04000107 RID: 263
		private static readonly Regex Cpu = new Regex("(?<v>\\d+)%\\s+in last 1 minute", RegexOptions.Compiled);

		// Token: 0x04000108 RID: 264
		private static readonly Regex Memory = new Regex("^Mem:\\s+\\d+\\s+\\d+\\s+\\d+\\s+(?<v>[\\d.]+)%", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000109 RID: 265
		private static readonly Regex SessionSplit = new Regex("(?=PFCP session ID\\s*:)", RegexOptions.Compiled);

		// Token: 0x0400010A RID: 266
		private static readonly Regex Imsi = new Regex("\\bIMSI\\s*:\\s*(?<v>\\d+)", RegexOptions.Compiled);

		// Token: 0x0400010B RID: 267
		private static readonly Regex Msisdn = new Regex("\\bMSISDN\\s*:\\s*(?<v>\\d+)", RegexOptions.Compiled);

		// Token: 0x0400010C RID: 268
		private static readonly Regex Forwarded = new Regex("Forwarded\\s*:\\s*\\d+/(?<bps>\\d+)\\s*\\(pps/bps\\)", RegexOptions.Compiled);

		// Token: 0x0400010D RID: 269
		private static readonly Regex ForwardedBytes = new Regex("Forwarded statistics\\s*:\\s*(?<bytes>\\d+)\\s*\\(bytes\\)", RegexOptions.Compiled);

		// Token: 0x0400010E RID: 270
		private static readonly Regex StartTime = new Regex("Start time\\s*:\\s*(?<v>\\d{4}-\\d{2}-\\d{2}\\s+\\d{2}:\\d{2}:\\d{2})", RegexOptions.Compiled);

		// Token: 0x0400010F RID: 271
		private static readonly Regex UeIpAddr = new Regex("\\bUE\\s+IP(?:v4)?\\s+address\\s*:\\s*(?<v>\\d{1,3}(?:\\.\\d{1,3}){3})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000110 RID: 272
		private static readonly Regex SmfBlock = new Regex("ue-context\\s+supi\\s+(?<imsi>\\d{6,15})\\s+brief[^\\r\\n]*(?<body>[\\s\\S]*?)(?=\\r?\\n\\s*\\[|ue-context\\s+supi\\s+\\d|\\z)", RegexOptions.Compiled);

		// Token: 0x04000111 RID: 273
		private static readonly Regex SmfSessionRx = new Regex("PduSessionId\\s*:\\s*\\d+[\\s\\S]*?Dnn\\s*:\\s*(?<dnn>\\S+)[\\s\\S]*?UpCnxState\\s*:\\s*(?<state>\\S+)[\\s\\S]*?IPv4\\s*:\\s*(?<ip>\\d{1,3}(?:\\.\\d{1,3}){3})", RegexOptions.Compiled);

		// Token: 0x04000112 RID: 274
		private static readonly Regex AmfNetwork = new Regex("^[ \\t]*amf[ \\t]+network[ \\t]+mcc[ \\t]+(?<mcc>\\d+)[ \\t]+mnc[ \\t]+(?<mnc>\\d+)(?:[ \\t]+shortname[ \\t]+(?<short>\\S+))?(?:[ \\t]+fullname[ \\t]+(?<full>\\S+))?", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000113 RID: 275
		private static readonly Regex UeIpPool = new Regex("^[ \\t]*smf\\s+ue-ipv4-pool\\s+(?<name>\\S+)[ \\t]*\\r?$(?<body>(?:\\r?\\n[ \\t]+\\S[^\\r\\n]*)*)", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000114 RID: 276
		private static readonly Regex AddressRange = new Regex("address\\s+range\\s+\\d+\\s+(?<from>\\d{1,3}(?:\\.\\d{1,3}){3})\\s+(?<to>\\d{1,3}(?:\\.\\d{1,3}){3})", RegexOptions.Compiled);

		// Token: 0x04000115 RID: 277
		private static readonly Regex AmfGnbRow = new Regex("^\\s*(?<num>\\d+)\\s+(?<mcc>\\d+)\\s+(?<mnc>\\d+)\\s+(?<gnb>\\d+)\\s*\\[bit length\\s+(?<bits>\\d+)\\]\\s+(?<link>\\S+)", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000116 RID: 278
		private static readonly Regex AmfRosterRow = new Regex("^\\s*\\d+\\s+(?<cell>\\d+)\\s+(?:imsi-)?(?<supi>\\d{5,})\\s+(?<msisdn>\\S+)", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x04000117 RID: 279
		private static readonly Regex AmfUeBlock = new Regex("SUPI:\\s*(?<supi>\\d{5,})(?<body>[\\s\\S]*?)(?=SUPI:\\s*\\d{5,}|\\z)", RegexOptions.Compiled);

		// Token: 0x04000118 RID: 280
		private static readonly Regex AmfGlobalGnb = new Regex("Global gNB ID:\\s*MCC:\\s*(?<mcc>\\d+)\\s+MNC:\\s*(?<mnc>\\d+)\\s+gNB ID:\\s*(?<gnb>\\d+)", RegexOptions.Compiled);

		// Token: 0x04000119 RID: 281
		private static readonly Regex AmfTai = new Regex("Current TAI:\\s*MCC:\\s*(?<mcc>\\d+)\\s+MNC:\\s*(?<mnc>\\d+)\\s+TAC:\\s*(?<tac>\\d+)", RegexOptions.Compiled);

		// Token: 0x0400011A RID: 282
		private static readonly Regex RadioCapability = new Regex("^[ \\t]*UE Radio Capability[ \\t]*:[^\\r\\n]*$", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x0400011B RID: 283
		private static readonly Regex AmfAmbr = new Regex("UE-AMBR:\\s*UPLINK:\\s*(?<ul>\\d+)\\s*\\(kbps\\)\\s*DOWNLINK:\\s*(?<dl>\\d+)", RegexOptions.Compiled);

		// Token: 0x0400011C RID: 284
		private static readonly Regex PingLoss = new Regex("(?<loss>\\d+(?:\\.\\d+)?)%\\s*packet loss", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400011D RID: 285
		private static readonly Regex PingRtt = new Regex("round-trip min/avg/max(?:/std-dev)?\\s*=\\s*[\\d.]+/(?<avg>[\\d.]+)/", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400011E RID: 286
		private static readonly Regex PingTarget = new Regex("[Pp]ing(?:ing)?\\s+(?:\\-\\S+\\s+|\\d+\\s+)*(?<ip>\\d{1,3}(?:\\.\\d{1,3}){3})", RegexOptions.Compiled);

		// Token: 0x0200010E RID: 270
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class AmfUeContext : IEquatable<H3c5gcParser.AmfUeContext>
		{
			// Token: 0x0600099C RID: 2460 RVA: 0x0003A79C File Offset: 0x0003899C
			public AmfUeContext(string Gnb, string Cell, string Tmsi, string RmState, string CmState, string RrcState, string Tai, string AmbrUl, string AmbrDl)
			{
				this.Gnb = Gnb;
				this.Cell = Cell;
				this.Tmsi = Tmsi;
				this.RmState = RmState;
				this.CmState = CmState;
				this.RrcState = RrcState;
				this.Tai = Tai;
				this.AmbrUl = AmbrUl;
				this.AmbrDl = AmbrDl;
				base..ctor();
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x0600099D RID: 2461 RVA: 0x0003A7F4 File Offset: 0x000389F4
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(H3c5gcParser.AmfUeContext);
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x0600099E RID: 2462 RVA: 0x0003A800 File Offset: 0x00038A00
			// (set) Token: 0x0600099F RID: 2463 RVA: 0x0003A808 File Offset: 0x00038A08
			public string Gnb { get; set; }

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0003A811 File Offset: 0x00038A11
			// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0003A819 File Offset: 0x00038A19
			public string Cell { get; set; }

			// Token: 0x17000278 RID: 632
			// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0003A822 File Offset: 0x00038A22
			// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0003A82A File Offset: 0x00038A2A
			public string Tmsi { get; set; }

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0003A833 File Offset: 0x00038A33
			// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0003A83B File Offset: 0x00038A3B
			public string RmState { get; set; }

			// Token: 0x1700027A RID: 634
			// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0003A844 File Offset: 0x00038A44
			// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0003A84C File Offset: 0x00038A4C
			public string CmState { get; set; }

			// Token: 0x1700027B RID: 635
			// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0003A855 File Offset: 0x00038A55
			// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0003A85D File Offset: 0x00038A5D
			public string RrcState { get; set; }

			// Token: 0x1700027C RID: 636
			// (get) Token: 0x060009AA RID: 2474 RVA: 0x0003A866 File Offset: 0x00038A66
			// (set) Token: 0x060009AB RID: 2475 RVA: 0x0003A86E File Offset: 0x00038A6E
			public string Tai { get; set; }

			// Token: 0x1700027D RID: 637
			// (get) Token: 0x060009AC RID: 2476 RVA: 0x0003A877 File Offset: 0x00038A77
			// (set) Token: 0x060009AD RID: 2477 RVA: 0x0003A87F File Offset: 0x00038A7F
			public string AmbrUl { get; set; }

			// Token: 0x1700027E RID: 638
			// (get) Token: 0x060009AE RID: 2478 RVA: 0x0003A888 File Offset: 0x00038A88
			// (set) Token: 0x060009AF RID: 2479 RVA: 0x0003A890 File Offset: 0x00038A90
			public string AmbrDl { get; set; }

			// Token: 0x060009B0 RID: 2480 RVA: 0x0003A89C File Offset: 0x00038A9C
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("AmfUeContext");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x0003A8E8 File Offset: 0x00038AE8
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Gnb = ");
				builder.Append(this.Gnb);
				builder.Append(", Cell = ");
				builder.Append(this.Cell);
				builder.Append(", Tmsi = ");
				builder.Append(this.Tmsi);
				builder.Append(", RmState = ");
				builder.Append(this.RmState);
				builder.Append(", CmState = ");
				builder.Append(this.CmState);
				builder.Append(", RrcState = ");
				builder.Append(this.RrcState);
				builder.Append(", Tai = ");
				builder.Append(this.Tai);
				builder.Append(", AmbrUl = ");
				builder.Append(this.AmbrUl);
				builder.Append(", AmbrDl = ");
				builder.Append(this.AmbrDl);
				return true;
			}

			// Token: 0x060009B2 RID: 2482 RVA: 0x0003A9DC File Offset: 0x00038BDC
			[CompilerGenerated]
			public static bool operator !=(H3c5gcParser.AmfUeContext left, H3c5gcParser.AmfUeContext right)
			{
				return !(left == right);
			}

			// Token: 0x060009B3 RID: 2483 RVA: 0x0003A9E8 File Offset: 0x00038BE8
			[CompilerGenerated]
			public static bool operator ==(H3c5gcParser.AmfUeContext left, H3c5gcParser.AmfUeContext right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060009B4 RID: 2484 RVA: 0x0003A9FC File Offset: 0x00038BFC
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Gnb>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Cell>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Tmsi>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RmState>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<CmState>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RrcState>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Tai>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<AmbrUl>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<AmbrDl>k__BackingField);
			}

			// Token: 0x060009B5 RID: 2485 RVA: 0x0003AAE8 File Offset: 0x00038CE8
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as H3c5gcParser.AmfUeContext);
			}

			// Token: 0x060009B6 RID: 2486 RVA: 0x0003AAF8 File Offset: 0x00038CF8
			[CompilerGenerated]
			public bool Equals(H3c5gcParser.AmfUeContext other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Gnb>k__BackingField, other.<Gnb>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Cell>k__BackingField, other.<Cell>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Tmsi>k__BackingField, other.<Tmsi>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RmState>k__BackingField, other.<RmState>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<CmState>k__BackingField, other.<CmState>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RrcState>k__BackingField, other.<RrcState>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Tai>k__BackingField, other.<Tai>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<AmbrUl>k__BackingField, other.<AmbrUl>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<AmbrDl>k__BackingField, other.<AmbrDl>k__BackingField));
			}

			// Token: 0x060009B8 RID: 2488 RVA: 0x0003AC14 File Offset: 0x00038E14
			[CompilerGenerated]
			private AmfUeContext([Nullable(1)] H3c5gcParser.AmfUeContext original)
			{
				this.Gnb = original.<Gnb>k__BackingField;
				this.Cell = original.<Cell>k__BackingField;
				this.Tmsi = original.<Tmsi>k__BackingField;
				this.RmState = original.<RmState>k__BackingField;
				this.CmState = original.<CmState>k__BackingField;
				this.RrcState = original.<RrcState>k__BackingField;
				this.Tai = original.<Tai>k__BackingField;
				this.AmbrUl = original.<AmbrUl>k__BackingField;
				this.AmbrDl = original.<AmbrDl>k__BackingField;
			}

			// Token: 0x060009B9 RID: 2489 RVA: 0x0003AC94 File Offset: 0x00038E94
			[CompilerGenerated]
			public void Deconstruct(out string Gnb, out string Cell, out string Tmsi, out string RmState, out string CmState, out string RrcState, out string Tai, out string AmbrUl, out string AmbrDl)
			{
				Gnb = this.Gnb;
				Cell = this.Cell;
				Tmsi = this.Tmsi;
				RmState = this.RmState;
				CmState = this.CmState;
				RrcState = this.RrcState;
				Tai = this.Tai;
				AmbrUl = this.AmbrUl;
				AmbrDl = this.AmbrDl;
			}
		}

		// Token: 0x0200010F RID: 271
		[NullableContext(2)]
		[Nullable(0)]
		internal sealed class UpfSession : IEquatable<H3c5gcParser.UpfSession>
		{
			// Token: 0x060009BA RID: 2490 RVA: 0x0003ACF0 File Offset: 0x00038EF0
			public UpfSession([Nullable(1)] string Imsi, string Msisdn, double DlMbps, double UlMbps, DateTime? Start, string UeIp, long? DlBytes = null, long? UlBytes = null)
			{
				this.Imsi = Imsi;
				this.Msisdn = Msisdn;
				this.DlMbps = DlMbps;
				this.UlMbps = UlMbps;
				this.Start = Start;
				this.UeIp = UeIp;
				this.DlBytes = DlBytes;
				this.UlBytes = UlBytes;
				base..ctor();
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x060009BB RID: 2491 RVA: 0x0003AD40 File Offset: 0x00038F40
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(H3c5gcParser.UpfSession);
				}
			}

			// Token: 0x17000280 RID: 640
			// (get) Token: 0x060009BC RID: 2492 RVA: 0x0003AD4C File Offset: 0x00038F4C
			// (set) Token: 0x060009BD RID: 2493 RVA: 0x0003AD54 File Offset: 0x00038F54
			[Nullable(1)]
			public string Imsi
			{
				[NullableContext(1)]
				get;
				[NullableContext(1)]
				set;
			}

			// Token: 0x17000281 RID: 641
			// (get) Token: 0x060009BE RID: 2494 RVA: 0x0003AD5D File Offset: 0x00038F5D
			// (set) Token: 0x060009BF RID: 2495 RVA: 0x0003AD65 File Offset: 0x00038F65
			public string Msisdn { get; set; }

			// Token: 0x17000282 RID: 642
			// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0003AD6E File Offset: 0x00038F6E
			// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0003AD76 File Offset: 0x00038F76
			public double DlMbps { get; set; }

			// Token: 0x17000283 RID: 643
			// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0003AD7F File Offset: 0x00038F7F
			// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0003AD87 File Offset: 0x00038F87
			public double UlMbps { get; set; }

			// Token: 0x17000284 RID: 644
			// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0003AD90 File Offset: 0x00038F90
			// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0003AD98 File Offset: 0x00038F98
			public DateTime? Start { get; set; }

			// Token: 0x17000285 RID: 645
			// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003ADA1 File Offset: 0x00038FA1
			// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0003ADA9 File Offset: 0x00038FA9
			public string UeIp { get; set; }

			// Token: 0x17000286 RID: 646
			// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0003ADB2 File Offset: 0x00038FB2
			// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0003ADBA File Offset: 0x00038FBA
			public long? DlBytes { get; set; }

			// Token: 0x17000287 RID: 647
			// (get) Token: 0x060009CA RID: 2506 RVA: 0x0003ADC3 File Offset: 0x00038FC3
			// (set) Token: 0x060009CB RID: 2507 RVA: 0x0003ADCB File Offset: 0x00038FCB
			public long? UlBytes { get; set; }

			// Token: 0x060009CC RID: 2508 RVA: 0x0003ADD4 File Offset: 0x00038FD4
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("UpfSession");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060009CD RID: 2509 RVA: 0x0003AE20 File Offset: 0x00039020
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Imsi = ");
				builder.Append(this.Imsi);
				builder.Append(", Msisdn = ");
				builder.Append(this.Msisdn);
				builder.Append(", DlMbps = ");
				builder.Append(this.DlMbps.ToString());
				builder.Append(", UlMbps = ");
				builder.Append(this.UlMbps.ToString());
				builder.Append(", Start = ");
				builder.Append(this.Start.ToString());
				builder.Append(", UeIp = ");
				builder.Append(this.UeIp);
				builder.Append(", DlBytes = ");
				builder.Append(this.DlBytes.ToString());
				builder.Append(", UlBytes = ");
				builder.Append(this.UlBytes.ToString());
				return true;
			}

			// Token: 0x060009CE RID: 2510 RVA: 0x0003AF41 File Offset: 0x00039141
			[CompilerGenerated]
			public static bool operator !=(H3c5gcParser.UpfSession left, H3c5gcParser.UpfSession right)
			{
				return !(left == right);
			}

			// Token: 0x060009CF RID: 2511 RVA: 0x0003AF4D File Offset: 0x0003914D
			[CompilerGenerated]
			public static bool operator ==(H3c5gcParser.UpfSession left, H3c5gcParser.UpfSession right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060009D0 RID: 2512 RVA: 0x0003AF64 File Offset: 0x00039164
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Imsi>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Msisdn>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<DlMbps>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<UlMbps>k__BackingField)) * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(this.<Start>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<UeIp>k__BackingField)) * -1521134295 + EqualityComparer<long?>.Default.GetHashCode(this.<DlBytes>k__BackingField)) * -1521134295 + EqualityComparer<long?>.Default.GetHashCode(this.<UlBytes>k__BackingField);
			}

			// Token: 0x060009D1 RID: 2513 RVA: 0x0003B039 File Offset: 0x00039239
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as H3c5gcParser.UpfSession);
			}

			// Token: 0x060009D2 RID: 2514 RVA: 0x0003B048 File Offset: 0x00039248
			[CompilerGenerated]
			public bool Equals(H3c5gcParser.UpfSession other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Imsi>k__BackingField, other.<Imsi>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Msisdn>k__BackingField, other.<Msisdn>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<DlMbps>k__BackingField, other.<DlMbps>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<UlMbps>k__BackingField, other.<UlMbps>k__BackingField) && EqualityComparer<DateTime?>.Default.Equals(this.<Start>k__BackingField, other.<Start>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<UeIp>k__BackingField, other.<UeIp>k__BackingField) && EqualityComparer<long?>.Default.Equals(this.<DlBytes>k__BackingField, other.<DlBytes>k__BackingField) && EqualityComparer<long?>.Default.Equals(this.<UlBytes>k__BackingField, other.<UlBytes>k__BackingField));
			}

			// Token: 0x060009D4 RID: 2516 RVA: 0x0003B148 File Offset: 0x00039348
			[CompilerGenerated]
			private UpfSession([Nullable(1)] H3c5gcParser.UpfSession original)
			{
				this.Imsi = original.<Imsi>k__BackingField;
				this.Msisdn = original.<Msisdn>k__BackingField;
				this.DlMbps = original.<DlMbps>k__BackingField;
				this.UlMbps = original.<UlMbps>k__BackingField;
				this.Start = original.<Start>k__BackingField;
				this.UeIp = original.<UeIp>k__BackingField;
				this.DlBytes = original.<DlBytes>k__BackingField;
				this.UlBytes = original.<UlBytes>k__BackingField;
			}

			// Token: 0x060009D5 RID: 2517 RVA: 0x0003B1BC File Offset: 0x000393BC
			[CompilerGenerated]
			public void Deconstruct([Nullable(1)] out string Imsi, out string Msisdn, out double DlMbps, out double UlMbps, out DateTime? Start, out string UeIp, out long? DlBytes, out long? UlBytes)
			{
				Imsi = this.Imsi;
				Msisdn = this.Msisdn;
				DlMbps = this.DlMbps;
				UlMbps = this.UlMbps;
				Start = this.Start;
				UeIp = this.UeIp;
				DlBytes = this.DlBytes;
				UlBytes = this.UlBytes;
			}
		}

		// Token: 0x02000110 RID: 272
		[Nullable(0)]
		internal sealed class SmfSession : IEquatable<H3c5gcParser.SmfSession>
		{
			// Token: 0x060009D6 RID: 2518 RVA: 0x0003B21A File Offset: 0x0003941A
			public SmfSession(string Dnn, string UpCnxState, string Ip)
			{
				this.Dnn = Dnn;
				this.UpCnxState = UpCnxState;
				this.Ip = Ip;
				base..ctor();
			}

			// Token: 0x17000288 RID: 648
			// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0003B237 File Offset: 0x00039437
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(H3c5gcParser.SmfSession);
				}
			}

			// Token: 0x17000289 RID: 649
			// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0003B243 File Offset: 0x00039443
			// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0003B24B File Offset: 0x0003944B
			public string Dnn { get; set; }

			// Token: 0x1700028A RID: 650
			// (get) Token: 0x060009DA RID: 2522 RVA: 0x0003B254 File Offset: 0x00039454
			// (set) Token: 0x060009DB RID: 2523 RVA: 0x0003B25C File Offset: 0x0003945C
			public string UpCnxState { get; set; }

			// Token: 0x1700028B RID: 651
			// (get) Token: 0x060009DC RID: 2524 RVA: 0x0003B265 File Offset: 0x00039465
			// (set) Token: 0x060009DD RID: 2525 RVA: 0x0003B26D File Offset: 0x0003946D
			public string Ip { get; set; }

			// Token: 0x060009DE RID: 2526 RVA: 0x0003B278 File Offset: 0x00039478
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SmfSession");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x0003B2C4 File Offset: 0x000394C4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Dnn = ");
				builder.Append(this.Dnn);
				builder.Append(", UpCnxState = ");
				builder.Append(this.UpCnxState);
				builder.Append(", Ip = ");
				builder.Append(this.Ip);
				return true;
			}

			// Token: 0x060009E0 RID: 2528 RVA: 0x0003B322 File Offset: 0x00039522
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(H3c5gcParser.SmfSession left, H3c5gcParser.SmfSession right)
			{
				return !(left == right);
			}

			// Token: 0x060009E1 RID: 2529 RVA: 0x0003B32E File Offset: 0x0003952E
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(H3c5gcParser.SmfSession left, H3c5gcParser.SmfSession right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x0003B344 File Offset: 0x00039544
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Dnn>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<UpCnxState>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Ip>k__BackingField);
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x0003B3A6 File Offset: 0x000395A6
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as H3c5gcParser.SmfSession);
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x0003B3B4 File Offset: 0x000395B4
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(H3c5gcParser.SmfSession other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Dnn>k__BackingField, other.<Dnn>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<UpCnxState>k__BackingField, other.<UpCnxState>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Ip>k__BackingField, other.<Ip>k__BackingField));
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x0003B42D File Offset: 0x0003962D
			[CompilerGenerated]
			private SmfSession(H3c5gcParser.SmfSession original)
			{
				this.Dnn = original.<Dnn>k__BackingField;
				this.UpCnxState = original.<UpCnxState>k__BackingField;
				this.Ip = original.<Ip>k__BackingField;
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x0003B459 File Offset: 0x00039659
			[CompilerGenerated]
			public void Deconstruct(out string Dnn, out string UpCnxState, out string Ip)
			{
				Dnn = this.Dnn;
				UpCnxState = this.UpCnxState;
				Ip = this.Ip;
			}
		}
	}
}
