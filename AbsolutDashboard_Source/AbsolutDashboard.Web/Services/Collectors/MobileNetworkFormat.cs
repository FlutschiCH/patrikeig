using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Services.Collectors.Bbu;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200003F RID: 63
	[NullableContext(2)]
	[Nullable(0)]
	public static class MobileNetworkFormat
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x00009F30 File Offset: 0x00008130
		[return: Nullable(1)]
		public static List<Metric> Describe(MmlResponse plmns, MmlResponse slices)
		{
			List<Metric> list = new List<Metric>();
			List<string> list2 = MobileNetworkFormat.DistinctColumn(plmns, "PLMNID");
			IEnumerable<string> enumerable = list2;
			Func<string, ValueTuple<string, string>?> func;
			if ((func = MobileNetworkFormat.<>O.<0>__DecodePlmnId) == null)
			{
				func = (MobileNetworkFormat.<>O.<0>__DecodePlmnId = new Func<string, ValueTuple<string, string>?>(MobileNetworkFormat.DecodePlmnId));
			}
			List<ValueTuple<string, string>> list3 = (from p in enumerable.Select<string, ValueTuple<string, string>?>(func)
				where p != null
				select p.Value).ToList<ValueTuple<string, string>>();
			MobileNetworkFormat.AddJoined(list, "MCC", list3.Select<ValueTuple<string, string>, string>(([TupleElementNames(new string[] { "Mcc", "Mnc" })] ValueTuple<string, string> p) => p.Item1));
			MobileNetworkFormat.AddJoined(list, "MNC", list3.Select<ValueTuple<string, string>, string>(([TupleElementNames(new string[] { "Mcc", "Mnc" })] ValueTuple<string, string> p) => p.Item2));
			MobileNetworkFormat.AddJoined(list, "PLMN ID", list2);
			List<string> list4 = MobileNetworkFormat.DistinctColumn(slices, "SNSSAI");
			string text = "Network slices";
			IEnumerable<string> enumerable2 = list4;
			Func<string, string> func2;
			if ((func2 = MobileNetworkFormat.<>O.<1>__DecodeSnssai) == null)
			{
				func2 = (MobileNetworkFormat.<>O.<1>__DecodeSnssai = new Func<string, string>(MobileNetworkFormat.DecodeSnssai));
			}
			MobileNetworkFormat.AddJoined(list, text, enumerable2.Select<string, string>(func2).OfType<string>());
			MobileNetworkFormat.AddJoined(list, "SNSSAI", list4);
			return list;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A074 File Offset: 0x00008274
		[NullableContext(1)]
		private static List<string> DistinctColumn([Nullable(2)] MmlResponse r, string column)
		{
			if (r == null || !r.Success)
			{
				return new List<string>();
			}
			return (from v in r.Rows.Select<Dictionary<string, string>, string>(delegate(Dictionary<string, string> row)
				{
					string text;
					if (!row.TryGetValue(column, out text))
					{
						return "";
					}
					return text;
				})
				where v.Length > 0
				select v).Distinct<string>(StringComparer.OrdinalIgnoreCase).ToList<string>();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A0EC File Offset: 0x000082EC
		[NullableContext(1)]
		private static void AddJoined(List<Metric> metrics, string label, IEnumerable<string> values)
		{
			string text = string.Join(", ", values.Distinct<string>(StringComparer.OrdinalIgnoreCase));
			if (text.Length > 0)
			{
				metrics.Add(new Metric(label, text, null, "Mobile network"));
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A12C File Offset: 0x0000832C
		[return: TupleElementNames(new string[] { "Mcc", "Mnc" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<string, string>? DecodePlmnId(string plmnId)
		{
			string text = ((plmnId != null) ? plmnId.Trim() : null);
			if (text == null || text.Length != 6)
			{
				return null;
			}
			string text2 = text.Substring(0, 3);
			IEnumerable<char> enumerable = text2;
			Func<char, bool> func;
			if ((func = MobileNetworkFormat.<>O.<2>__IsAsciiDigit) == null)
			{
				func = (MobileNetworkFormat.<>O.<2>__IsAsciiDigit = new Func<char, bool>(char.IsAsciiDigit));
			}
			if (!enumerable.All<char>(func))
			{
				return null;
			}
			string text3 = text.Substring(3).TrimEnd(new char[] { 'f', 'F' });
			if (text3.Length != 0)
			{
				IEnumerable<char> enumerable2 = text3;
				Func<char, bool> func2;
				if ((func2 = MobileNetworkFormat.<>O.<2>__IsAsciiDigit) == null)
				{
					func2 = (MobileNetworkFormat.<>O.<2>__IsAsciiDigit = new Func<char, bool>(char.IsAsciiDigit));
				}
				if (enumerable2.All<char>(func2))
				{
					return new ValueTuple<string, string>?(new ValueTuple<string, string>(text2, text3));
				}
			}
			return null;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A1F0 File Offset: 0x000083F0
		public static Snssai? ParseSnssai(string snssai)
		{
			string text = ((snssai != null) ? snssai.Trim() : null);
			if (text == null || text.Length != 8)
			{
				return null;
			}
			int num;
			if (!int.TryParse(text.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				return null;
			}
			string text2 = text.Substring(2);
			IEnumerable<char> enumerable = text2;
			Func<char, bool> func;
			if ((func = MobileNetworkFormat.<>O.<3>__IsAsciiHexDigit) == null)
			{
				func = (MobileNetworkFormat.<>O.<3>__IsAsciiHexDigit = new Func<char, bool>(char.IsAsciiHexDigit));
			}
			if (!enumerable.All<char>(func))
			{
				return null;
			}
			return new Snssai?(new Snssai(num, text2.All<char>((char c) => c == 'F' || c == 'f') ? null : text2));
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public static string DecodeSnssai(string snssai)
		{
			Snssai? snssai2 = MobileNetworkFormat.ParseSnssai(snssai);
			if (snssai2 == null)
			{
				return null;
			}
			Snssai valueOrDefault = snssai2.GetValueOrDefault();
			if (valueOrDefault.Sd != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendLiteral("SST ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault.Sst);
				defaultInterpolatedStringHandler.AppendLiteral(" · SD ");
				defaultInterpolatedStringHandler.AppendFormatted(valueOrDefault.Sd);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("SST ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(valueOrDefault.Sst);
			defaultInterpolatedStringHandler2.AppendLiteral(" (no SD)");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A360 File Offset: 0x00008560
		public static string SstName(int sst)
		{
			string text;
			switch (sst)
			{
			case 1:
				text = "eMBB";
				break;
			case 2:
				text = "URLLC";
				break;
			case 3:
				text = "MIoT";
				break;
			case 4:
				text = "V2X";
				break;
			case 5:
				text = "HMTC";
				break;
			default:
				text = null;
				break;
			}
			return text;
		}

		// Token: 0x040000BC RID: 188
		[Nullable(1)]
		public const string Group = "Mobile network";

		// Token: 0x02000103 RID: 259
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000510 RID: 1296
			[TupleElementNames(new string[] { "Mcc", "Mnc" })]
			[Nullable(0)]
			public static Func<string, ValueTuple<string, string>?> <0>__DecodePlmnId;

			// Token: 0x04000511 RID: 1297
			[Nullable(0)]
			public static Func<string, string> <1>__DecodeSnssai;

			// Token: 0x04000512 RID: 1298
			[Nullable(0)]
			public static Func<char, bool> <2>__IsAsciiDigit;

			// Token: 0x04000513 RID: 1299
			[Nullable(0)]
			public static Func<char, bool> <3>__IsAsciiHexDigit;
		}
	}
}
