using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Data.Entities;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public static class ConnectivityTimeline
	{
		// Token: 0x0600015C RID: 348 RVA: 0x000073BC File Offset: 0x000055BC
		public static List<LinkSegment> Build(IEnumerable<DevicePollResult> rows, DateTime fromUtc, DateTime toUtc)
		{
			if (toUtc <= fromUtc)
			{
				return new List<LinkSegment>();
			}
			List<DevicePollResult> list = (from r in rows
				where r.Timestamp >= fromUtc && r.Timestamp <= toUtc
				orderby r.Timestamp
				select r).ToList<DevicePollResult>();
			if (list.Count == 0)
			{
				return new List<LinkSegment>
				{
					new LinkSegment(fromUtc, toUtc, LinkState.NoData)
				};
			}
			TimeSpan timeSpan = ConnectivityTimeline.TypicalSpacing(list);
			List<LinkSegment> list2 = new List<LinkSegment>();
			if (list[0].Timestamp > fromUtc)
			{
				list2.Add(new LinkSegment(fromUtc, list[0].Timestamp, LinkState.NoData));
			}
			for (int i = 0; i < list.Count; i++)
			{
				DateTime timestamp = list[i].Timestamp;
				DateTime dateTime = ((i + 1 < list.Count) ? list[i + 1].Timestamp : toUtc);
				LinkState linkState = ConnectivityTimeline.StateOf(list[i]);
				DateTime dateTime2 = timestamp + timeSpan * 3.0;
				if (dateTime > dateTime2)
				{
					list2.Add(new LinkSegment(timestamp, dateTime2, linkState));
					list2.Add(new LinkSegment(dateTime2, dateTime, LinkState.NoData));
				}
				else
				{
					list2.Add(new LinkSegment(timestamp, dateTime, linkState));
				}
			}
			return ConnectivityTimeline.Merge(list2);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007558 File Offset: 0x00005758
		private static LinkState StateOf(DevicePollResult poll)
		{
			if (!poll.Reachable)
			{
				return LinkState.Unreachable;
			}
			DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(poll.SnapshotJson);
			string text;
			if (deviceSnapshot == null)
			{
				text = null;
			}
			else
			{
				Metric metric = deviceSnapshot.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "State");
				if (metric == null)
				{
					text = null;
				}
				else
				{
					string value = metric.Value;
					text = ((value != null) ? value.Trim() : null);
				}
			}
			if (string.Equals(text, "Idle", StringComparison.OrdinalIgnoreCase))
			{
				return LinkState.Idle;
			}
			return LinkState.Connected;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000075D4 File Offset: 0x000057D4
		private static TimeSpan TypicalSpacing(List<DevicePollResult> polls)
		{
			if (polls.Count < 2)
			{
				return TimeSpan.FromMinutes(1L);
			}
			List<long> list = new List<long>(polls.Count - 1);
			for (int i = 1; i < polls.Count; i++)
			{
				list.Add((polls[i].Timestamp - polls[i - 1].Timestamp).Ticks);
			}
			list.Sort();
			int num = list.Count / 2;
			long num2 = ((list.Count % 2 == 1) ? list[num] : ((list[num - 1] + list[num]) / 2L));
			if (num2 <= 0L)
			{
				return TimeSpan.FromMinutes(1L);
			}
			return TimeSpan.FromTicks(num2);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000768C File Offset: 0x0000588C
		private static List<LinkSegment> Merge(List<LinkSegment> raw)
		{
			List<LinkSegment> list = new List<LinkSegment>(raw.Count);
			foreach (LinkSegment linkSegment in raw)
			{
				if (!(linkSegment.ToUtc <= linkSegment.FromUtc))
				{
					if (list.Count > 0)
					{
						List<LinkSegment> list2 = list;
						if (list2[list2.Count - 1].State == linkSegment.State)
						{
							List<LinkSegment> list3 = list;
							int num = list3.Count - 1;
							List<LinkSegment> list4 = list;
							LinkSegment linkSegment2 = list4[list4.Count - 1].<Clone>$();
							linkSegment2.ToUtc = linkSegment.ToUtc;
							list3[num] = linkSegment2;
							continue;
						}
					}
					list.Add(linkSegment);
				}
			}
			return list;
		}

		// Token: 0x040000A1 RID: 161
		private const int GapFactor = 3;
	}
}
