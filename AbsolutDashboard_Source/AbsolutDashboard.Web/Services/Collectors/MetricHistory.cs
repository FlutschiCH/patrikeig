using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AbsolutDashboard.Web.Data.Entities;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(0)]
	public static class MetricHistory
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00009124 File Offset: 0x00007324
		[NullableContext(2)]
		public static bool TryParseNumeric(string value, out double result)
		{
			result = 0.0;
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			string text = value.Trim();
			Match match = MetricHistory.LeadingNumber.Match(text);
			if (!match.Success)
			{
				return false;
			}
			for (int i = match.Length; i < text.Length; i++)
			{
				if (char.IsDigit(text[i]))
				{
					return false;
				}
			}
			return double.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000091A0 File Offset: 0x000073A0
		[return: TupleElementNames(new string[] { "Ts", "Snap" })]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		public static List<ValueTuple<DateTime, DeviceSnapshot>> Decode(IEnumerable<DevicePollResult> results)
		{
			List<ValueTuple<DateTime, DeviceSnapshot>> list = new List<ValueTuple<DateTime, DeviceSnapshot>>();
			foreach (DevicePollResult devicePollResult in results)
			{
				DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(devicePollResult.SnapshotJson);
				if (deviceSnapshot != null)
				{
					list.Add(new ValueTuple<DateTime, DeviceSnapshot>(devicePollResult.Timestamp, deviceSnapshot));
				}
			}
			list.Sort(([Nullable(new byte[] { 0, 1 })] ValueTuple<DateTime, DeviceSnapshot> a, [Nullable(new byte[] { 0, 1 })] ValueTuple<DateTime, DeviceSnapshot> b) => a.Item1.CompareTo(b.Item1));
			return list;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009230 File Offset: 0x00007430
		public static List<HistoryPoint> Series([TupleElementNames(new string[] { "Ts", "Snap" })] [Nullable(new byte[] { 1, 0, 1 })] IEnumerable<ValueTuple<DateTime, DeviceSnapshot>> decoded, string label, [Nullable(2)] string group = null)
		{
			List<HistoryPoint> list = new List<HistoryPoint>();
			Func<Metric, bool> <>9__0;
			foreach (ValueTuple<DateTime, DeviceSnapshot> valueTuple in decoded)
			{
				DateTime item = valueTuple.Item1;
				IEnumerable<Metric> metrics = valueTuple.Item2.Metrics;
				Func<Metric, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (Metric x) => string.Equals(x.Label, label, StringComparison.Ordinal) && (group == null || string.Equals(x.Group, group, StringComparison.Ordinal)));
				}
				Metric metric = metrics.FirstOrDefault<Metric>(func);
				double num;
				if (metric != null && MetricHistory.TryParseNumeric(metric.Value, out num))
				{
					list.Add(new HistoryPoint(item, num));
				}
			}
			return list;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000092E8 File Offset: 0x000074E8
		public static double?[] ValuesAt(IReadOnlyList<HistoryPoint> pts, IReadOnlyList<DateTime> stamps)
		{
			double?[] array = new double?[stamps.Count];
			int num = 0;
			double num2 = 0.0;
			bool flag = false;
			for (int i = 0; i < stamps.Count; i++)
			{
				while (num < pts.Count && pts[num].Ts <= stamps[i])
				{
					num2 = pts[num].Value;
					flag = true;
					num++;
				}
				array[i] = (flag ? new double?(num2) : null);
			}
			return array;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009384 File Offset: 0x00007584
		public static List<DateTime> Stamps(IEnumerable<List<HistoryPoint>> series)
		{
			return (from t in (from p in series.SelectMany<List<HistoryPoint>, HistoryPoint>((List<HistoryPoint> s) => s)
					select p.Ts).Distinct<DateTime>()
				orderby t
				select t).ToList<DateTime>();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009408 File Offset: 0x00007608
		public static List<HistoryPoint> Sum(IEnumerable<List<HistoryPoint>> series)
		{
			List<List<HistoryPoint>> list = series.Where<List<HistoryPoint>>((List<HistoryPoint> s) => s.Count > 0).ToList<List<HistoryPoint>>();
			if (list.Count == 0)
			{
				return new List<HistoryPoint>();
			}
			List<DateTime> stamps = MetricHistory.Stamps(list);
			List<double?[]> list2 = list.Select<List<HistoryPoint>, double?[]>((List<HistoryPoint> l) => MetricHistory.ValuesAt(l, stamps)).ToList<double?[]>();
			List<HistoryPoint> list3 = new List<HistoryPoint>(stamps.Count);
			for (int i = 0; i < stamps.Count; i++)
			{
				double num = 0.0;
				foreach (double?[] array in list2)
				{
					num += array[i].GetValueOrDefault();
				}
				list3.Add(new HistoryPoint(stamps[i], num));
			}
			return list3;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009518 File Offset: 0x00007718
		public static List<HistoryPoint> Downsample(List<HistoryPoint> pts, int maxPoints, DownsampleMode mode)
		{
			if (maxPoints < 2 || pts.Count <= maxPoints)
			{
				return pts;
			}
			long ticks = pts[0].Ts.Ticks;
			long num = pts[pts.Count - 1].Ts.Ticks - ticks;
			if (num <= 0L)
			{
				return pts;
			}
			List<HistoryPoint> list = new List<HistoryPoint>(maxPoints + 1);
			int num2 = -1;
			foreach (HistoryPoint historyPoint in pts)
			{
				int num3 = (int)Math.Min((long)(maxPoints - 1), (historyPoint.Ts.Ticks - ticks) * (long)maxPoints / num);
				if (num3 != num2)
				{
					list.Add(historyPoint);
					num2 = num3;
				}
				else
				{
					if (mode != DownsampleMode.Last)
					{
						double value = historyPoint.Value;
						List<HistoryPoint> list2 = list;
						if (value <= list2[list2.Count - 1].Value)
						{
							continue;
						}
					}
					List<HistoryPoint> list3 = list;
					list3[list3.Count - 1] = historyPoint;
				}
			}
			List<HistoryPoint> list4 = list;
			if (list4[list4.Count - 1].Ts != pts[pts.Count - 1].Ts)
			{
				list.Add(pts[pts.Count - 1]);
			}
			return list;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009674 File Offset: 0x00007874
		public static List<HistoryPoint> Reachability(IEnumerable<DevicePollResult> results)
		{
			return (from r in results
				orderby r.Timestamp
				select new HistoryPoint(r.Timestamp, r.Reachable > false)).ToList<HistoryPoint>();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000096D0 File Offset: 0x000078D0
		public static List<HistoryPoint> Latency(IEnumerable<DevicePollResult> results)
		{
			return (from r in results
				where r.LatencyMs != null
				orderby r.Timestamp
				select new HistoryPoint(r.Timestamp, (double)r.LatencyMs.Value)).ToList<HistoryPoint>();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000974F File Offset: 0x0000794F
		public static List<HistoryPoint> AlarmCount([TupleElementNames(new string[] { "Ts", "Snap" })] [Nullable(new byte[] { 1, 0, 1 })] IEnumerable<ValueTuple<DateTime, DeviceSnapshot>> decoded)
		{
			return decoded.Select<ValueTuple<DateTime, DeviceSnapshot>, HistoryPoint>(([TupleElementNames(new string[] { "Ts", "Snap" })] ValueTuple<DateTime, DeviceSnapshot> d) => new HistoryPoint(d.Item1, (double)d.Item2.Alarms.Count)).ToList<HistoryPoint>();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000977C File Offset: 0x0000797C
		public static List<HistoryPoint> UptimeSeries([TupleElementNames(new string[] { "Ts", "Snap" })] [Nullable(new byte[] { 1, 0, 1 })] IEnumerable<ValueTuple<DateTime, DeviceSnapshot>> decoded)
		{
			List<HistoryPoint> list = new List<HistoryPoint>();
			foreach (ValueTuple<DateTime, DeviceSnapshot> valueTuple in decoded)
			{
				DateTime item = valueTuple.Item1;
				Metric metric = valueTuple.Item2.Metrics.FirstOrDefault<Metric>((Metric x) => string.Equals(x.Label, "Uptime", StringComparison.Ordinal));
				if (metric != null)
				{
					double? num = UptimeFormat.ToSeconds(metric.Value);
					if (num != null)
					{
						double valueOrDefault = num.GetValueOrDefault();
						list.Add(new HistoryPoint(item, valueOrDefault / 3600.0));
					}
				}
			}
			return list;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009834 File Offset: 0x00007A34
		public static List<Trend> SelectTrends(IReadOnlyCollection<DevicePollResult> results, [Nullable(2)] string categoryKey, int max = 12)
		{
			string text = (categoryKey ?? "").ToLowerInvariant();
			bool flag = text == "cell" || text == "eu" || text == "ue" || text == "au";
			if (flag)
			{
				return new List<Trend>();
			}
			List<ValueTuple<DateTime, DeviceSnapshot>> list = MetricHistory.Decode(results);
			if (text == "bbu")
			{
				return MetricHistory.BbuTrends(list, results);
			}
			List<ValueTuple<int, double, Trend>> list2 = new List<ValueTuple<int, double, Trend>>();
			if (list.Count > 0)
			{
				List<ValueTuple<DateTime, DeviceSnapshot>> list3 = list;
				foreach (Metric metric in list3[list3.Count - 1].Item2.Metrics)
				{
					double num;
					if (!MetricHistory.NonTelemetryGroups.Contains(metric.Group ?? "") && MetricHistory.TryParseNumeric(metric.Value, out num))
					{
						List<HistoryPoint> list4 = MetricHistory.Series(list, metric.Label, metric.Group);
						if (list4.Count >= 2)
						{
							list2.Add(new ValueTuple<int, double, Trend>(MetricHistory.GroupPriority(metric.Group), MetricHistory.Variance(list4), new Trend(metric.Label, metric.Unit, metric.Group ?? "Metrics", list4)));
						}
					}
				}
			}
			Trend trend = MetricHistory.TimingTrend(text, results);
			if (trend != null)
			{
				list2.Add(new ValueTuple<int, double, Trend>(88, MetricHistory.Variance(trend.Points), trend));
			}
			List<Trend> list5 = (from t in (from t in list2
					orderby t.Item1 descending, t.Item2 descending
					select t).Take<ValueTuple<int, double, Trend>>(max)
				select t.Item3).ToList<Trend>();
			if (list5.Count == 0)
			{
				List<HistoryPoint> list6 = MetricHistory.Reachability(results);
				if (list6.Count >= 2)
				{
					list5.Add(new Trend("Reachability", null, "Health", list6));
				}
			}
			return list5;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009A84 File Offset: 0x00007C84
		private static List<Trend> BbuTrends([TupleElementNames(new string[] { "Ts", "Snap" })] [Nullable(new byte[] { 1, 0, 1 })] List<ValueTuple<DateTime, DeviceSnapshot>> decoded, IReadOnlyCollection<DevicePollResult> results)
		{
			MetricHistory.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.list = new List<Trend>();
			MetricHistory.<BbuTrends>g__Add|14_0("Connected UEs", null, "System", MetricHistory.Series(decoded, "Connected UEs", "System"), ref CS$<>8__locals1);
			MetricHistory.<BbuTrends>g__Add|14_0("CPU average", "%", "Resources", MetricHistory.Series(decoded, "CPU load", "Resources"), ref CS$<>8__locals1);
			List<HistoryPoint> list = MetricHistory.Series(decoded, "CELL.AvailRatio", "Performance").Select<HistoryPoint, HistoryPoint>(delegate(HistoryPoint p)
			{
				HistoryPoint historyPoint = p;
				historyPoint.Value = p.Value * 100.0;
				return historyPoint;
			}).ToList<HistoryPoint>();
			MetricHistory.<BbuTrends>g__Add|14_0("Cell availability", "%", "Performance", list, ref CS$<>8__locals1);
			MetricHistory.<BbuTrends>g__Add|14_0("Active alarms", null, "Health", MetricHistory.AlarmCount(decoded), ref CS$<>8__locals1);
			Trend trend = MetricHistory.TimingTrend("bbu", results);
			if (trend != null)
			{
				CS$<>8__locals1.list.Add(trend);
			}
			return CS$<>8__locals1.list;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009B70 File Offset: 0x00007D70
		[return: Nullable(2)]
		private static Trend TimingTrend(string cat, IReadOnlyCollection<DevicePollResult> results)
		{
			List<HistoryPoint> list = MetricHistory.Latency(results);
			if (list.Count < 2)
			{
				return null;
			}
			if (!(cat == "bbu") && !(cat == "mobile"))
			{
				return new Trend("Latency", "ms", "Health", list);
			}
			List<HistoryPoint> list2 = list.Select<HistoryPoint, HistoryPoint>(delegate(HistoryPoint p)
			{
				HistoryPoint historyPoint = p;
				historyPoint.Value = p.Value / 1000.0;
				return historyPoint;
			}).ToList<HistoryPoint>();
			return new Trend("Poll duration", "s", "Health", list2);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009C08 File Offset: 0x00007E08
		public static IEnumerable<Metric> InDisplayOrder(IEnumerable<Metric> metrics)
		{
			List<Metric> list = metrics.ToList<Metric>();
			if (list.Count > 0)
			{
				if (list.All<Metric>((Metric m) => MetricHistory.OpIndexed.IsMatch(m.Label)))
				{
					return list.OrderBy<Metric, int>((Metric m) => int.Parse(MetricHistory.OpIndexed.Match(m.Label).Groups["n"].Value)).ThenBy<Metric, string>((Metric m) => m.Label, StringComparer.Ordinal);
				}
			}
			return list;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009CA0 File Offset: 0x00007EA0
		[NullableContext(2)]
		private static int GroupPriority(string group)
		{
			string text = (group ?? "").ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 2:
					if (text == "ue")
					{
						return 95;
					}
					break;
				case 4:
					if (text == "cell")
					{
						return 20;
					}
					break;
				case 5:
					if (text == "power")
					{
						return 80;
					}
					break;
				case 6:
					if (text == "system")
					{
						return 40;
					}
					break;
				case 7:
				{
					char c = text[0];
					if (c != 'm')
					{
						if (c == 'o')
						{
							if (text == "optical")
							{
								return 65;
							}
						}
					}
					else if (text == "metrics")
					{
						return 55;
					}
					break;
				}
				case 9:
				{
					char c = text[0];
					if (c != 'c')
					{
						if (c == 'r')
						{
							if (text == "resources")
							{
								return 75;
							}
						}
					}
					else if (text == "cpu cores")
					{
						return 70;
					}
					break;
				}
				case 10:
					if (text == "throughput")
					{
						return 100;
					}
					break;
				case 11:
					if (text == "performance")
					{
						return 85;
					}
					break;
				}
			}
			return 50;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009E14 File Offset: 0x00008014
		private static double Variance(List<HistoryPoint> pts)
		{
			if (pts.Count < 2)
			{
				return 0.0;
			}
			double num = pts.Average<HistoryPoint>((HistoryPoint p) => p.Value);
			double num2 = 0.0;
			foreach (HistoryPoint historyPoint in pts)
			{
				double num3 = historyPoint.Value - num;
				num2 += num3 * num3;
			}
			return num2 / (double)pts.Count;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009F10 File Offset: 0x00008110
		[CompilerGenerated]
		internal static void <BbuTrends>g__Add|14_0(string title, [Nullable(2)] string unit, string group, List<HistoryPoint> pts, ref MetricHistory.<>c__DisplayClass14_0 A_4)
		{
			if (pts.Count >= 2)
			{
				A_4.list.Add(new Trend(title, unit, group, pts));
			}
		}

		// Token: 0x040000B9 RID: 185
		private static readonly Regex LeadingNumber = new Regex("^[-+]?\\d+(?:\\.\\d+)?", RegexOptions.Compiled);

		// Token: 0x040000BA RID: 186
		private static readonly HashSet<string> NonTelemetryGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Mobile network", "Radio" };

		// Token: 0x040000BB RID: 187
		private static readonly Regex OpIndexed = new Regex("^OP(?<n>\\d+)\\D", RegexOptions.Compiled);
	}
}
