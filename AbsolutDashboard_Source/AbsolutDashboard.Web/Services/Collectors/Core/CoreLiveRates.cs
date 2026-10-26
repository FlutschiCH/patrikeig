using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006A RID: 106
	public static class CoreLiveRates
	{
		// Token: 0x0600038A RID: 906 RVA: 0x000105D4 File Offset: 0x0000E7D4
		[NullableContext(1)]
		[return: TupleElementNames(new string[] { "DlMbps", "UlMbps" })]
		[return: Nullable(0)]
		public static ValueTuple<double, double>? Rate(CounterSample from, CounterSample to)
		{
			double totalSeconds = (to.AtUtc - from.AtUtc).TotalSeconds;
			if (totalSeconds <= 0.0)
			{
				return null;
			}
			if (to.DlBytes < from.DlBytes || to.UlBytes < from.UlBytes)
			{
				return null;
			}
			return new ValueTuple<double, double>?(new ValueTuple<double, double>(CoreLiveRates.Mbps(to.DlBytes - from.DlBytes, totalSeconds), CoreLiveRates.Mbps(to.UlBytes - from.UlBytes, totalSeconds)));
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010667 File Offset: 0x0000E867
		public static double Mbps(long bytes, double seconds)
		{
			return (double)bytes * 8.0 / seconds / 1000000.0;
		}
	}
}
