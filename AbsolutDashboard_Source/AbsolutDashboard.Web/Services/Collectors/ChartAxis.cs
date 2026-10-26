using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	public static class ChartAxis
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00006D34 File Offset: 0x00004F34
		public static ValueAxis Values(double dataMax)
		{
			if (!double.IsFinite(dataMax) || dataMax <= 0.0)
			{
				dataMax = 1.0;
			}
			ValueAxis valueAxis = default(ValueAxis);
			double num = double.MaxValue;
			int num2 = int.MaxValue;
			foreach (int num3 in new int[] { 3, 4, 5, 6 })
			{
				ValueAxis valueAxis2 = ChartAxis.Fit(dataMax, num3);
				double num4 = valueAxis2.Top - dataMax;
				int num5 = Math.Abs(num3 - 4);
				if (num4 < num - 1E-09 || (num4 < num + 1E-09 && num5 < num2))
				{
					valueAxis = valueAxis2;
					num = num4;
					num2 = num5;
				}
			}
			return valueAxis;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006DF0 File Offset: 0x00004FF0
		private static ValueAxis Fit(double dataMax, int intervals)
		{
			double num = ChartAxis.NiceStep(dataMax / (double)intervals);
			double num2 = num * Math.Ceiling(dataMax / num - 1E-09);
			return new ValueAxis(num2, num, ChartAxis.Ladder(num, (int)Math.Round(num2 / num)));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006E34 File Offset: 0x00005034
		private static IReadOnlyList<double> Ladder(double step, int count)
		{
			List<double> list = new List<double>(count + 1);
			for (int i = 0; i <= count; i++)
			{
				list.Add(step * (double)i);
			}
			return list;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006E64 File Offset: 0x00005064
		private static double NiceStep(double raw)
		{
			double num = Math.Pow(10.0, Math.Floor(Math.Log10(raw)));
			double num2 = raw / num;
			foreach (double num3 in ChartAxis.NiceMultipliers)
			{
				if (num2 <= num3 + 1E-09)
				{
					return num3 * num;
				}
			}
			return 10.0 * num;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006EC7 File Offset: 0x000050C7
		public static string FormatValue(double value)
		{
			return value.ToString("0.####", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006EDC File Offset: 0x000050DC
		public static List<DateTime> TimeTicks(DateTime fromUtc, DateTime toUtc, int targetIntervals = 6)
		{
			DateTime dateTime = ChartAxis.ToLocal(fromUtc);
			DateTime dateTime2 = ChartAxis.ToLocal(toUtc);
			if (dateTime2 <= dateTime)
			{
				return new List<DateTime> { dateTime };
			}
			TimeSpan wanted = TimeSpan.FromTicks((dateTime2 - dateTime).Ticks / (long)Math.Max(1, targetIntervals));
			TimeSpan timeSpan = ChartAxis.TimeSteps.FirstOrDefault<TimeSpan>((TimeSpan s) => s >= wanted);
			if (timeSpan == default(TimeSpan))
			{
				timeSpan = TimeSpan.FromDays(1);
			}
			DateTime date = dateTime.Date;
			long ticks = (dateTime - date).Ticks;
			DateTime dateTime3 = date + TimeSpan.FromTicks((long)Math.Ceiling((double)ticks / (double)timeSpan.Ticks) * timeSpan.Ticks);
			List<DateTime> list = new List<DateTime>();
			DateTime dateTime4 = dateTime3;
			while (dateTime4 <= dateTime2)
			{
				list.Add(dateTime4);
				dateTime4 += timeSpan;
			}
			if (list.Count == 0)
			{
				list.Add(dateTime);
				list.Add(dateTime2);
			}
			return list;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006FF0 File Offset: 0x000051F0
		public static DateTime ToLocal(DateTime utc)
		{
			ChartAxis.RefreshLocalZone();
			return DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007014 File Offset: 0x00005214
		public static void RefreshLocalZone()
		{
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow - ChartAxis._zoneCheckedUtc < TimeSpan.FromMinutes(1L))
			{
				return;
			}
			ChartAxis._zoneCheckedUtc = utcNow;
			TimeZoneInfo.ClearCachedData();
		}

		// Token: 0x04000096 RID: 150
		private static readonly double[] NiceMultipliers = new double[] { 1.0, 2.0, 2.5, 5.0, 10.0 };

		// Token: 0x04000097 RID: 151
		private static readonly TimeSpan[] TimeSteps = new TimeSpan[]
		{
			TimeSpan.FromMinutes(1L),
			TimeSpan.FromMinutes(2L),
			TimeSpan.FromMinutes(5L),
			TimeSpan.FromMinutes(10L),
			TimeSpan.FromMinutes(15L),
			TimeSpan.FromMinutes(30L),
			TimeSpan.FromHours(1),
			TimeSpan.FromHours(2),
			TimeSpan.FromHours(3),
			TimeSpan.FromHours(4),
			TimeSpan.FromHours(6),
			TimeSpan.FromHours(12),
			TimeSpan.FromDays(1)
		};

		// Token: 0x04000098 RID: 152
		private static DateTime _zoneCheckedUtc;
	}
}
