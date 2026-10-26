using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoreLiveState
	{
		// Token: 0x06000384 RID: 900 RVA: 0x0001036B File Offset: 0x0000E56B
		public void Publish(CoreLiveSnapshot snapshot)
		{
			this._latest[snapshot.CoreDeviceId] = snapshot;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00010380 File Offset: 0x0000E580
		[NullableContext(2)]
		public CoreLiveSnapshot Latest(int coreDeviceId, TimeSpan maxAge)
		{
			CoreLiveSnapshot coreLiveSnapshot;
			if (!this._latest.TryGetValue(coreDeviceId, out coreLiveSnapshot) || !(DateTime.UtcNow - coreLiveSnapshot.AtUtc <= maxAge))
			{
				return null;
			}
			return coreLiveSnapshot;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000103B8 File Offset: 0x0000E5B8
		public void Drop(int coreDeviceId)
		{
			CoreLiveSnapshot coreLiveSnapshot;
			this._latest.TryRemove(coreDeviceId, out coreLiveSnapshot);
			IEnumerable<ValueTuple<int, string>> keys = this._rings.Keys;
			Func<ValueTuple<int, string>, bool> <>9__0;
			Func<ValueTuple<int, string>, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ([TupleElementNames(new string[] { "Core", "Imsi" })] ValueTuple<int, string> k) => k.Item1 == coreDeviceId);
			}
			foreach (ValueTuple<int, string> valueTuple in keys.Where<ValueTuple<int, string>>(func).ToList<ValueTuple<int, string>>())
			{
				List<CounterSample> list;
				this._rings.TryRemove(valueTuple, out list);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010464 File Offset: 0x0000E664
		public void RecordCounters(int coreDeviceId, string imsi, CounterSample sample)
		{
			List<CounterSample> orAdd = this._rings.GetOrAdd(new ValueTuple<int, string>(coreDeviceId, imsi), ([TupleElementNames(new string[] { "Core", "Imsi" })] [Nullable(new byte[] { 0, 1 })] ValueTuple<int, string> _) => new List<CounterSample>(25));
			List<CounterSample> list = orAdd;
			lock (list)
			{
				orAdd.Add(sample);
				if (orAdd.Count > 24)
				{
					orAdd.RemoveAt(0);
				}
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000104E4 File Offset: 0x0000E6E4
		[NullableContext(0)]
		[return: TupleElementNames(new string[] { "DlMbps", "UlMbps" })]
		public ValueTuple<double, double>? RateSince(int coreDeviceId, [Nullable(1)] string imsi, DateTime sinceUtc)
		{
			List<CounterSample> list;
			if (!this._rings.TryGetValue(new ValueTuple<int, string>(coreDeviceId, imsi), out list))
			{
				ValueTuple<double, double>? valueTuple = null;
				return valueTuple;
			}
			List<CounterSample> list2 = list;
			CounterSample counterSample;
			CounterSample counterSample2;
			lock (list2)
			{
				if (list.Count < 2)
				{
					ValueTuple<double, double>? valueTuple = null;
					return valueTuple;
				}
				List<CounterSample> list3 = list;
				counterSample = list3[list3.Count - 1];
				counterSample2 = null;
				for (int i = list.Count - 2; i >= 0; i--)
				{
					counterSample2 = list[i];
					if (list[i].AtUtc <= sinceUtc)
					{
						break;
					}
				}
			}
			if (counterSample2 != null)
			{
				return CoreLiveRates.Rate(counterSample2, counterSample);
			}
			return null;
		}

		// Token: 0x0400018B RID: 395
		private const int RingSize = 24;

		// Token: 0x0400018C RID: 396
		private readonly ConcurrentDictionary<int, CoreLiveSnapshot> _latest = new ConcurrentDictionary<int, CoreLiveSnapshot>();

		// Token: 0x0400018D RID: 397
		[TupleElementNames(new string[] { "Core", "Imsi" })]
		[Nullable(new byte[] { 1, 0, 1, 1, 1 })]
		private readonly ConcurrentDictionary<ValueTuple<int, string>, List<CounterSample>> _rings = new ConcurrentDictionary<ValueTuple<int, string>, List<CounterSample>>();
	}
}
