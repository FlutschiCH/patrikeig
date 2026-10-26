using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	public static class BbuCellSettings
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x00012A57 File Offset: 0x00010C57
		public static string AdminStateForWrite(string adminState)
		{
			if (!adminState.Equals("on", StringComparison.OrdinalIgnoreCase))
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00012A74 File Offset: 0x00010C74
		[NullableContext(2)]
		public static string ValidateSysName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "Base station name must not be empty.";
			}
			string text = name.Trim();
			if (text.Length > 32)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Base station name must be at most ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(32);
				defaultInterpolatedStringHandler.AppendLiteral(" characters.");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (!text.All<char>(delegate(char c)
			{
				bool flag = char.IsLetterOrDigit(c);
				if (!flag)
				{
					bool flag2 = c == '-' || c == '_';
					flag = flag2;
				}
				return flag;
			}))
			{
				return "Base station name may contain letters, digits, '-' and '_' only.";
			}
			return null;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00012B01 File Offset: 0x00010D01
		public static string SetSysName(string name)
		{
			return "SET SYSCFG:SysName=" + name.Trim() + ";";
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00012B18 File Offset: 0x00010D18
		public static double DbmToMilliwatts(int dBm)
		{
			return Math.Pow(10.0, (double)dBm / 10.0);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00012B34 File Offset: 0x00010D34
		public static string FormatMilliwatts(double mW)
		{
			if (mW >= 1000.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendFormatted<double>(mW / 1000.0, "0.#");
				defaultInterpolatedStringHandler.AppendLiteral(" W");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (mW >= 100.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<double>(mW, "0");
				defaultInterpolatedStringHandler2.AppendLiteral(" mW");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (mW < 10.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler3.AppendFormatted<double>(mW, "0.##");
				defaultInterpolatedStringHandler3.AppendLiteral(" mW");
				return defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(3, 1);
			defaultInterpolatedStringHandler4.AppendFormatted<double>(mW, "0.#");
			defaultInterpolatedStringHandler4.AppendLiteral(" mW");
			return defaultInterpolatedStringHandler4.ToStringAndClear();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00012C1C File Offset: 0x00010E1C
		[NullableContext(2)]
		[return: TupleElementNames(new string[] { "Note", "Warning" })]
		[return: Nullable(new byte[] { 0, 1 })]
		public static ValueTuple<string, bool> TddPatternNote(string pattern, int? band)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				return new ValueTuple<string, bool>("", false);
			}
			if (pattern.StartsWith("DDDSU", StringComparison.Ordinal))
			{
				return new ValueTuple<string, bool>("DDDSU: required on band n78.", false);
			}
			if (!pattern.StartsWith("DSUUU", StringComparison.Ordinal))
			{
				return new ValueTuple<string, bool>("", false);
			}
			if (band.GetValueOrDefault() != 78)
			{
				return new ValueTuple<string, bool>("DSUUU: best upload speed; not allowed on band n78.", false);
			}
			return new ValueTuple<string, bool>("DSUUU: best upload speed, but not allowed on band n78 — this cell is on n78.", true);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00012C94 File Offset: 0x00010E94
		[NullableContext(2)]
		public static BbuCellSettings.PowerRange PowerRangeFor(string radioUnitModel)
		{
			if (string.IsNullOrWhiteSpace(radioUnitModel))
			{
				return null;
			}
			foreach (KeyValuePair<string, BbuCellSettings.PowerRange> keyValuePair in BbuCellSettings.RadioUnitPower)
			{
				string text;
				BbuCellSettings.PowerRange powerRange;
				keyValuePair.Deconstruct(out text, out powerRange);
				string text2 = text;
				BbuCellSettings.PowerRange powerRange2 = powerRange;
				if (radioUnitModel.Trim().StartsWith(text2, StringComparison.OrdinalIgnoreCase))
				{
					return powerRange2;
				}
			}
			return null;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00012D0C File Offset: 0x00010F0C
		public static double ArfcnToMhz(int arfcn)
		{
			if (arfcn >= 600000)
			{
				return 3000.0 + (double)(arfcn - 600000) * 0.015;
			}
			return (double)arfcn * 0.005;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00012D40 File Offset: 0x00010F40
		public static int MhzToArfcn(double mhz)
		{
			if (mhz >= 3000.0)
			{
				return 600000 + (int)Math.Round((mhz - 3000.0) / 0.015, MidpointRounding.AwayFromZero);
			}
			return (int)Math.Round(mhz / 0.005, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00012D90 File Offset: 0x00010F90
		[NullableContext(2)]
		public static BbuCellSettings.BandWindow Band(int band)
		{
			return BbuCellSettings.Bands.FirstOrDefault<BbuCellSettings.BandWindow>((BbuCellSettings.BandWindow b) => b.Band == band);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00012DC0 File Offset: 0x00010FC0
		public static List<string> Validate(CellConfigEdit e)
		{
			List<string> list = new List<string>();
			int? num = e.Antennas;
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				if (!BbuCellSettings.AntennaCounts.Contains(valueOrDefault))
				{
					List<string> list2 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault);
					defaultInterpolatedStringHandler.AppendLiteral(" antennas is not a supported MIMO setting (allowed: ");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join<int>(", ", BbuCellSettings.AntennaCounts));
					defaultInterpolatedStringHandler.AppendLiteral(").");
					list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			num = e.Layers;
			if (num != null)
			{
				int valueOrDefault2 = num.GetValueOrDefault();
				if (!BbuCellSettings.LayerCounts.Contains(valueOrDefault2))
				{
					List<string> list3 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(52, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<int>(valueOrDefault2);
					defaultInterpolatedStringHandler2.AppendLiteral(" layers is not a supported MIMO setting (allowed: ");
					defaultInterpolatedStringHandler2.AppendFormatted(string.Join<int>(", ", BbuCellSettings.LayerCounts));
					defaultInterpolatedStringHandler2.AppendLiteral(").");
					list3.Add(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			num = e.Antennas;
			if (num != null)
			{
				int valueOrDefault3 = num.GetValueOrDefault();
				num = e.Layers;
				if (num != null)
				{
					int valueOrDefault4 = num.GetValueOrDefault();
					if (valueOrDefault4 > valueOrDefault3)
					{
						List<string> list4 = list;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(39, 3);
						defaultInterpolatedStringHandler3.AppendFormatted<int>(valueOrDefault4);
						defaultInterpolatedStringHandler3.AppendLiteral(" layers need at least ");
						defaultInterpolatedStringHandler3.AppendFormatted<int>(valueOrDefault4);
						defaultInterpolatedStringHandler3.AppendLiteral(" antennas (set ");
						defaultInterpolatedStringHandler3.AppendFormatted<int>(valueOrDefault3);
						defaultInterpolatedStringHandler3.AppendLiteral(").");
						list4.Add(defaultInterpolatedStringHandler3.ToStringAndClear());
					}
				}
			}
			int cellIndex = e.CellIndex;
			bool flag = cellIndex < 1 || cellIndex > 4;
			if (flag)
			{
				List<string> list5 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Cell index must be 1-4 (got ");
				defaultInterpolatedStringHandler4.AppendFormatted<int>(e.CellIndex);
				defaultInterpolatedStringHandler4.AppendLiteral(").");
				list5.Add(defaultInterpolatedStringHandler4.ToStringAndClear());
			}
			num = e.Band;
			if (num != null)
			{
				int valueOrDefault5 = num.GetValueOrDefault();
				if (BbuCellSettings.Band(valueOrDefault5) == null)
				{
					List<string> list6 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(45, 1);
					defaultInterpolatedStringHandler5.AppendLiteral("Band n");
					defaultInterpolatedStringHandler5.AppendFormatted<int>(valueOrDefault5);
					defaultInterpolatedStringHandler5.AppendLiteral(" is not supported by this base station.");
					list6.Add(defaultInterpolatedStringHandler5.ToStringAndClear());
				}
			}
			double? centreMhz = e.CentreMhz;
			if (centreMhz != null)
			{
				double valueOrDefault6 = centreMhz.GetValueOrDefault();
				int num2 = BbuCellSettings.MhzToArfcn(valueOrDefault6);
				flag = num2 < 0 || num2 > 3279165;
				if (flag)
				{
					List<string> list7 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(41, 2);
					defaultInterpolatedStringHandler6.AppendLiteral("NR-ARFCN ");
					defaultInterpolatedStringHandler6.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler6.AppendLiteral(" is outside the device range 0-");
					defaultInterpolatedStringHandler6.AppendFormatted<int>(3279165);
					defaultInterpolatedStringHandler6.AppendLiteral(".");
					list7.Add(defaultInterpolatedStringHandler6.ToStringAndClear());
				}
				num = e.Band;
				if (num != null)
				{
					int valueOrDefault7 = num.GetValueOrDefault();
					BbuCellSettings.BandWindow bandWindow = BbuCellSettings.Band(valueOrDefault7);
					if (bandWindow != null && (num2 < bandWindow.Low || num2 > bandWindow.High))
					{
						List<string> list8 = list;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(50, 7);
						defaultInterpolatedStringHandler7.AppendFormatted(BbuCellSettings.Mhz(valueOrDefault6));
						defaultInterpolatedStringHandler7.AppendLiteral(" MHz is NR-ARFCN ");
						defaultInterpolatedStringHandler7.AppendFormatted<int>(num2);
						defaultInterpolatedStringHandler7.AppendLiteral(", outside band n");
						defaultInterpolatedStringHandler7.AppendFormatted<int>(bandWindow.Band);
						defaultInterpolatedStringHandler7.AppendLiteral(" ");
						defaultInterpolatedStringHandler7.AppendLiteral("(");
						defaultInterpolatedStringHandler7.AppendFormatted<int>(bandWindow.Low);
						defaultInterpolatedStringHandler7.AppendLiteral("-");
						defaultInterpolatedStringHandler7.AppendFormatted<int>(bandWindow.High);
						defaultInterpolatedStringHandler7.AppendLiteral(", i.e. ");
						defaultInterpolatedStringHandler7.AppendFormatted(BbuCellSettings.Mhz(BbuCellSettings.ArfcnToMhz(bandWindow.Low)));
						defaultInterpolatedStringHandler7.AppendLiteral("-");
						defaultInterpolatedStringHandler7.AppendFormatted(BbuCellSettings.Mhz(BbuCellSettings.ArfcnToMhz(bandWindow.High)));
						defaultInterpolatedStringHandler7.AppendLiteral(" MHz).");
						list8.Add(defaultInterpolatedStringHandler7.ToStringAndClear());
					}
				}
			}
			num = e.Bandwidth;
			if (num != null)
			{
				int valueOrDefault8 = num.GetValueOrDefault();
				if (!BbuCellSettings.Bandwidths.Contains(valueOrDefault8))
				{
					List<string> list9 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler8.AppendLiteral("Bandwidth ");
					defaultInterpolatedStringHandler8.AppendFormatted<int>(valueOrDefault8);
					defaultInterpolatedStringHandler8.AppendLiteral(" MHz is not supported (allowed: ");
					defaultInterpolatedStringHandler8.AppendFormatted(string.Join<int>(", ", BbuCellSettings.Bandwidths));
					defaultInterpolatedStringHandler8.AppendLiteral(").");
					list9.Add(defaultInterpolatedStringHandler8.ToStringAndClear());
				}
			}
			string tddPattern = e.TddPattern;
			if (tddPattern != null)
			{
				string[] array;
				if (!BbuCellSettings.TddPatterns.TryGetValue(tddPattern, out array))
				{
					list.Add("TDD pattern '" + tddPattern + "' is not one of the six the device supports.");
				}
				else
				{
					string specialSlot = e.SpecialSlot;
					if (specialSlot != null && !array.Contains(specialSlot, StringComparer.Ordinal))
					{
						List<string> list10 = list;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(52, 3);
						defaultInterpolatedStringHandler9.AppendLiteral("Special slot ");
						defaultInterpolatedStringHandler9.AppendFormatted(specialSlot);
						defaultInterpolatedStringHandler9.AppendLiteral(" is not valid for pattern ");
						defaultInterpolatedStringHandler9.AppendFormatted(tddPattern);
						defaultInterpolatedStringHandler9.AppendLiteral(" (allowed: ");
						defaultInterpolatedStringHandler9.AppendFormatted(string.Join(", ", array));
						defaultInterpolatedStringHandler9.AppendLiteral(").");
						list10.Add(defaultInterpolatedStringHandler9.ToStringAndClear());
					}
				}
			}
			num = e.MaxTxPower;
			int valueOrDefault9;
			bool flag2;
			if (num != null)
			{
				valueOrDefault9 = num.GetValueOrDefault();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			flag = flag2;
			if (flag)
			{
				bool flag3 = valueOrDefault9 < 0 || valueOrDefault9 > 50;
				flag = flag3;
			}
			if (flag)
			{
				List<string> list11 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler10 = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler10.AppendLiteral("Max TX power must be ");
				defaultInterpolatedStringHandler10.AppendFormatted<int>(0);
				defaultInterpolatedStringHandler10.AppendLiteral("-");
				defaultInterpolatedStringHandler10.AppendFormatted<int>(50);
				defaultInterpolatedStringHandler10.AppendLiteral(" dBm (got ");
				defaultInterpolatedStringHandler10.AppendFormatted<int>(valueOrDefault9);
				defaultInterpolatedStringHandler10.AppendLiteral(").");
				list11.Add(defaultInterpolatedStringHandler10.ToStringAndClear());
			}
			string adminState = e.AdminState;
			if (adminState != null && !BbuCellSettings.AdminStates.Contains(adminState, StringComparer.Ordinal))
			{
				list.Add("Admin state must be 'on' or 'off' (got '" + adminState + "').");
			}
			return list;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000133E7 File Offset: 0x000115E7
		public static string Mhz(double v)
		{
			return v.ToString("0.###", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000133FC File Offset: 0x000115FC
		public static string ModCellAntennas(int cellIndex, int antennas)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 3);
			defaultInterpolatedStringHandler.AppendLiteral("MOD CellConfig:indexid_CellConfig=");
			defaultInterpolatedStringHandler.AppendFormatted(cellIndex.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",UlAntennas=");
			defaultInterpolatedStringHandler.AppendFormatted(antennas.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",DlAntennas=");
			defaultInterpolatedStringHandler.AppendFormatted(antennas.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(";");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00013484 File Offset: 0x00011684
		public static string ModCellLayers(int cellIndex, int layers)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
			defaultInterpolatedStringHandler.AppendLiteral("MOD CellConfig:indexid_CellConfig=");
			defaultInterpolatedStringHandler.AppendFormatted(cellIndex.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",UlLayers=");
			defaultInterpolatedStringHandler.AppendFormatted(layers.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",DlLayers=");
			defaultInterpolatedStringHandler.AppendFormatted(layers.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(";");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001350C File Offset: 0x0001170C
		[NullableContext(2)]
		public static string ModCellConfig(int cellIndex, int? band, int? bandwidth, int? arfcn)
		{
			if (band == null && bandwidth == null && arfcn == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("MOD CellConfig:indexid_CellConfig=").Append(cellIndex.ToString(CultureInfo.InvariantCulture));
			if (arfcn != null)
			{
				int valueOrDefault = arfcn.GetValueOrDefault();
				stringBuilder.Append(",NRARFCNUL=").Append(valueOrDefault.ToString(CultureInfo.InvariantCulture)).Append(",NRARFCNDL=")
					.Append(valueOrDefault.ToString(CultureInfo.InvariantCulture));
			}
			if (band != null)
			{
				int valueOrDefault2 = band.GetValueOrDefault();
				stringBuilder.Append(",FreqBandIndicator=").Append(valueOrDefault2.ToString(CultureInfo.InvariantCulture));
			}
			if (bandwidth != null)
			{
				int valueOrDefault3 = bandwidth.GetValueOrDefault();
				stringBuilder.Append(",ULBandwidth=").Append(valueOrDefault3.ToString(CultureInfo.InvariantCulture)).Append(",DLBandwidth=")
					.Append(valueOrDefault3.ToString(CultureInfo.InvariantCulture));
			}
			return stringBuilder.Append(';').ToString();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00013620 File Offset: 0x00011820
		[return: Nullable(2)]
		public static string CarrierCommand(CellConfigEdit edit, CellConfigEdit baseline, [Nullable(2)] out string error)
		{
			error = null;
			if (!BbuCellSettings.<CarrierCommand>g__Differs|31_1(edit.Band, baseline.Band) && !BbuCellSettings.<CarrierCommand>g__Differs|31_1(edit.Bandwidth, baseline.Bandwidth) && !BbuCellSettings.<CarrierCommand>g__Differs|31_1(BbuCellSettings.<CarrierCommand>g__Arfcn|31_0(edit.CentreMhz), BbuCellSettings.<CarrierCommand>g__Arfcn|31_0(baseline.CentreMhz)))
			{
				return null;
			}
			List<string> list = new List<string>();
			if (edit.Band == null)
			{
				list.Add("band");
			}
			if (edit.Bandwidth == null)
			{
				list.Add("bandwidth");
			}
			if (edit.CentreMhz == null)
			{
				list.Add("centre frequency");
			}
			if (list.Count > 0)
			{
				error = "Band, bandwidth and centre frequency are written together (the base station rejects a frequency without its band), but " + string.Join(" and ", list) + " was not read from it — the carrier is left unchanged.";
				return null;
			}
			return BbuCellSettings.ModCellConfig(edit.CellIndex, edit.Band, edit.Bandwidth, BbuCellSettings.<CarrierCommand>g__Arfcn|31_0(edit.CentreMhz));
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00013718 File Offset: 0x00011918
		public static string ModPhyTddUlDl(int cellIndex, string pattern, string specialSlot)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 3);
			defaultInterpolatedStringHandler.AppendLiteral("MOD PHYTddULDL:indexid_CellConfig=");
			defaultInterpolatedStringHandler.AppendFormatted(cellIndex.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendLiteral("SubframeAssignmentType=");
			defaultInterpolatedStringHandler.AppendFormatted(pattern);
			defaultInterpolatedStringHandler.AppendLiteral(",SpecialSubframePatterns=");
			defaultInterpolatedStringHandler.AppendFormatted(specialSlot);
			defaultInterpolatedStringHandler.AppendLiteral(";");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00013798 File Offset: 0x00011998
		public static string ModMaxTxPower(int cellIndex, int dBm)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
			defaultInterpolatedStringHandler.AppendLiteral("MOD MaxTxPower:indexid_CellConfig=");
			defaultInterpolatedStringHandler.AppendFormatted(cellIndex.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendLiteral("MaxTxPower=");
			defaultInterpolatedStringHandler.AppendFormatted(dBm.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(";");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001380C File Offset: 0x00011A0C
		public static string ModOpState(int cellIndex, string adminState)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("MOD OpState:indexid_CellConfig=");
			defaultInterpolatedStringHandler.AppendFormatted(cellIndex.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendLiteral("AdminState=");
			defaultInterpolatedStringHandler.AppendFormatted(BbuCellSettings.AdminStateForWrite(adminState));
			defaultInterpolatedStringHandler.AppendLiteral(";");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001387A File Offset: 0x00011A7A
		public static string SetClockSync(string mode)
		{
			return "SET FGAFINFO:ClockSynMode=" + mode + ";";
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001388C File Offset: 0x00011A8C
		[NullableContext(2)]
		public static int? ParseBand(string reported)
		{
			if (string.IsNullOrWhiteSpace(reported))
			{
				return null;
			}
			string text = reported.Trim().TrimStart(new char[] { 'n', 'N' });
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			int num2;
			if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
			{
				return null;
			}
			return new int?(num2);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00013900 File Offset: 0x00011B00
		[NullableContext(2)]
		public static int? ParseBandwidth(string reported)
		{
			if (string.IsNullOrWhiteSpace(reported))
			{
				return null;
			}
			IEnumerable<char> enumerable = reported.TrimStart();
			Func<char, bool> func;
			if ((func = BbuCellSettings.<>O.<0>__IsDigit) == null)
			{
				func = (BbuCellSettings.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
			}
			int num;
			if (!int.TryParse(new string(enumerable.TakeWhile<char>(func).ToArray<char>()), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00013970 File Offset: 0x00011B70
		[NullableContext(2)]
		public static string ParseTddPattern(string reported)
		{
			if (string.IsNullOrWhiteSpace(reported))
			{
				return null;
			}
			string text = reported.Trim();
			int num = 0;
			for (;;)
			{
				bool flag = num < text.Length;
				if (flag)
				{
					char c = text[num];
					bool flag2 = c == 'D' || c == 'S' || c == 'U';
					flag = flag2;
				}
				if (!flag)
				{
					break;
				}
				num++;
			}
			if (num != 0)
			{
				return text.Substring(0, num);
			}
			return null;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000139D8 File Offset: 0x00011BD8
		[NullableContext(2)]
		public static int? ParseInt(string reported)
		{
			int num;
			if (!int.TryParse((reported ?? "").Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00013A14 File Offset: 0x00011C14
		// Note: this type is marked as 'beforefieldinit'.
		static BbuCellSettings()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);
			dictionary["DDDDDDDSUU"] = new string[] { "10:2", "6:4" };
			dictionary["DDDSUDDDSU"] = new string[] { "10:2" };
			dictionary["DDDSUDDSUU"] = new string[] { "10:2", "8:2" };
			dictionary["DDSUUDDSUU"] = new string[] { "10:2" };
			dictionary["DSUUUDSUUU"] = new string[] { "10:2" };
			dictionary["DDDSUUDDDD"] = new string[] { "10:2", "6:4" };
			BbuCellSettings.TddPatterns = dictionary;
			BbuCellSettings.AdminStates = new string[] { "on", "off" };
			BbuCellSettings.ClockSyncModes = new string[] { "GNSS_mode", "1588_mode", "free_run_mode" };
			BbuCellSettings.PowerTableDbm = new int[]
			{
				0, 10, 20, 23, 25, 27, 30, 33, 37, 40,
				43, 46, 50
			};
			Dictionary<string, BbuCellSettings.PowerRange> dictionary2 = new Dictionary<string, BbuCellSettings.PowerRange>(StringComparer.OrdinalIgnoreCase);
			dictionary2["RU8250"] = new BbuCellSettings.PowerRange(0, 25, "reported by the base station, 2026-09-03");
			dictionary2["RU4370"] = new BbuCellSettings.PowerRange(24, 37, "vendor notes, not yet confirmed on hardware");
			dictionary2["RU4460"] = new BbuCellSettings.PowerRange(33, 46, "vendor notes, not yet confirmed on hardware");
			BbuCellSettings.RadioUnitPower = dictionary2;
			BbuCellSettings.AntennaCounts = new int[] { 1, 2, 4 };
			BbuCellSettings.LayerCounts = new int[] { 0, 1, 2, 4 };
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00013C68 File Offset: 0x00011E68
		[CompilerGenerated]
		internal static int? <CarrierCommand>g__Arfcn|31_0(double? mhz)
		{
			if (mhz != null)
			{
				double valueOrDefault = mhz.GetValueOrDefault();
				return new int?(BbuCellSettings.MhzToArfcn(valueOrDefault));
			}
			return null;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00013CA0 File Offset: 0x00011EA0
		[CompilerGenerated]
		internal static bool <CarrierCommand>g__Differs|31_1(int? now, int? was)
		{
			if (now != null)
			{
				int? num = now;
				int? num2 = was;
				return !((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)));
			}
			return false;
		}

		// Token: 0x040001B9 RID: 441
		public static readonly IReadOnlyList<BbuCellSettings.BandWindow> Bands = new BbuCellSettings.BandWindow[]
		{
			new BbuCellSettings.BandWindow(38, 514000, 524000),
			new BbuCellSettings.BandWindow(40, 460000, 480000),
			new BbuCellSettings.BandWindow(41, 499200, 537999),
			new BbuCellSettings.BandWindow(48, 636667, 646666),
			new BbuCellSettings.BandWindow(77, 620000, 680000),
			new BbuCellSettings.BandWindow(78, 620000, 653333),
			new BbuCellSettings.BandWindow(79, 693334, 733332)
		};

		// Token: 0x040001BA RID: 442
		public static readonly IReadOnlyList<int> Bandwidths = new int[] { 10, 20, 40, 50, 60, 80, 100 };

		// Token: 0x040001BB RID: 443
		public static readonly IReadOnlyDictionary<string, string[]> TddPatterns;

		// Token: 0x040001BC RID: 444
		public static readonly IReadOnlyList<string> AdminStates;

		// Token: 0x040001BD RID: 445
		public static readonly IReadOnlyList<string> ClockSyncModes;

		// Token: 0x040001BE RID: 446
		public const int SysNameMaxLength = 32;

		// Token: 0x040001BF RID: 447
		public const int MaxTxPowerMin = 0;

		// Token: 0x040001C0 RID: 448
		public const int MaxTxPowerMax = 50;

		// Token: 0x040001C1 RID: 449
		public static readonly IReadOnlyList<int> PowerTableDbm;

		// Token: 0x040001C2 RID: 450
		public static readonly IReadOnlyDictionary<string, BbuCellSettings.PowerRange> RadioUnitPower;

		// Token: 0x040001C3 RID: 451
		public const int ArfcnMax = 3279165;

		// Token: 0x040001C4 RID: 452
		private const int MidOffset = 600000;

		// Token: 0x040001C5 RID: 453
		public static readonly int[] AntennaCounts;

		// Token: 0x040001C6 RID: 454
		public static readonly int[] LayerCounts;

		// Token: 0x040001C7 RID: 455
		public const string RestartBts = "RST BTS:TYPE=PROCESS;";

		// Token: 0x02000133 RID: 307
		[Nullable(0)]
		public sealed class BandWindow : IEquatable<BbuCellSettings.BandWindow>
		{
			// Token: 0x06000A52 RID: 2642 RVA: 0x0003EF4E File Offset: 0x0003D14E
			public BandWindow(int Band, int Low, int High)
			{
				this.Band = Band;
				this.Low = Low;
				this.High = High;
				base..ctor();
			}

			// Token: 0x1700028E RID: 654
			// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0003EF6B File Offset: 0x0003D16B
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(BbuCellSettings.BandWindow);
				}
			}

			// Token: 0x1700028F RID: 655
			// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0003EF77 File Offset: 0x0003D177
			// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0003EF7F File Offset: 0x0003D17F
			public int Band { get; set; }

			// Token: 0x17000290 RID: 656
			// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0003EF88 File Offset: 0x0003D188
			// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0003EF90 File Offset: 0x0003D190
			public int Low { get; set; }

			// Token: 0x17000291 RID: 657
			// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0003EF99 File Offset: 0x0003D199
			// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0003EFA1 File Offset: 0x0003D1A1
			public int High { get; set; }

			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0003EFAC File Offset: 0x0003D1AC
			public string Label
			{
				get
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler.AppendLiteral("n");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Band);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}

			// Token: 0x06000A5B RID: 2651 RVA: 0x0003EFE4 File Offset: 0x0003D1E4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("BandWindow");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000A5C RID: 2652 RVA: 0x0003F030 File Offset: 0x0003D230
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Band = ");
				builder.Append(this.Band.ToString());
				builder.Append(", Low = ");
				builder.Append(this.Low.ToString());
				builder.Append(", High = ");
				builder.Append(this.High.ToString());
				builder.Append(", Label = ");
				builder.Append(this.Label);
				return true;
			}

			// Token: 0x06000A5D RID: 2653 RVA: 0x0003F0D1 File Offset: 0x0003D2D1
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(BbuCellSettings.BandWindow left, BbuCellSettings.BandWindow right)
			{
				return !(left == right);
			}

			// Token: 0x06000A5E RID: 2654 RVA: 0x0003F0DD File Offset: 0x0003D2DD
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(BbuCellSettings.BandWindow left, BbuCellSettings.BandWindow right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000A5F RID: 2655 RVA: 0x0003F0F4 File Offset: 0x0003D2F4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Band>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Low>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<High>k__BackingField);
			}

			// Token: 0x06000A60 RID: 2656 RVA: 0x0003F156 File Offset: 0x0003D356
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as BbuCellSettings.BandWindow);
			}

			// Token: 0x06000A61 RID: 2657 RVA: 0x0003F164 File Offset: 0x0003D364
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(BbuCellSettings.BandWindow other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<Band>k__BackingField, other.<Band>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Low>k__BackingField, other.<Low>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<High>k__BackingField, other.<High>k__BackingField));
			}

			// Token: 0x06000A63 RID: 2659 RVA: 0x0003F1DD File Offset: 0x0003D3DD
			[CompilerGenerated]
			private BandWindow(BbuCellSettings.BandWindow original)
			{
				this.Band = original.<Band>k__BackingField;
				this.Low = original.<Low>k__BackingField;
				this.High = original.<High>k__BackingField;
			}

			// Token: 0x06000A64 RID: 2660 RVA: 0x0003F209 File Offset: 0x0003D409
			[CompilerGenerated]
			public void Deconstruct(out int Band, out int Low, out int High)
			{
				Band = this.Band;
				Low = this.Low;
				High = this.High;
			}
		}

		// Token: 0x02000134 RID: 308
		[Nullable(0)]
		public sealed class PowerRange : IEquatable<BbuCellSettings.PowerRange>
		{
			// Token: 0x06000A65 RID: 2661 RVA: 0x0003F223 File Offset: 0x0003D423
			public PowerRange(int Min, int Max, string Source)
			{
				this.Min = Min;
				this.Max = Max;
				this.Source = Source;
				base..ctor();
			}

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0003F240 File Offset: 0x0003D440
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(BbuCellSettings.PowerRange);
				}
			}

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0003F24C File Offset: 0x0003D44C
			// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0003F254 File Offset: 0x0003D454
			public int Min { get; set; }

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0003F25D File Offset: 0x0003D45D
			// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0003F265 File Offset: 0x0003D465
			public int Max { get; set; }

			// Token: 0x17000296 RID: 662
			// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0003F26E File Offset: 0x0003D46E
			// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0003F276 File Offset: 0x0003D476
			public string Source { get; set; }

			// Token: 0x06000A6D RID: 2669 RVA: 0x0003F280 File Offset: 0x0003D480
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PowerRange");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000A6E RID: 2670 RVA: 0x0003F2CC File Offset: 0x0003D4CC
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Min = ");
				builder.Append(this.Min.ToString());
				builder.Append(", Max = ");
				builder.Append(this.Max.ToString());
				builder.Append(", Source = ");
				builder.Append(this.Source);
				return true;
			}

			// Token: 0x06000A6F RID: 2671 RVA: 0x0003F346 File Offset: 0x0003D546
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(BbuCellSettings.PowerRange left, BbuCellSettings.PowerRange right)
			{
				return !(left == right);
			}

			// Token: 0x06000A70 RID: 2672 RVA: 0x0003F352 File Offset: 0x0003D552
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(BbuCellSettings.PowerRange left, BbuCellSettings.PowerRange right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000A71 RID: 2673 RVA: 0x0003F368 File Offset: 0x0003D568
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Min>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Max>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Source>k__BackingField);
			}

			// Token: 0x06000A72 RID: 2674 RVA: 0x0003F3CA File Offset: 0x0003D5CA
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as BbuCellSettings.PowerRange);
			}

			// Token: 0x06000A73 RID: 2675 RVA: 0x0003F3D8 File Offset: 0x0003D5D8
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(BbuCellSettings.PowerRange other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.<Min>k__BackingField, other.<Min>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Max>k__BackingField, other.<Max>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Source>k__BackingField, other.<Source>k__BackingField));
			}

			// Token: 0x06000A75 RID: 2677 RVA: 0x0003F451 File Offset: 0x0003D651
			[CompilerGenerated]
			private PowerRange(BbuCellSettings.PowerRange original)
			{
				this.Min = original.<Min>k__BackingField;
				this.Max = original.<Max>k__BackingField;
				this.Source = original.<Source>k__BackingField;
			}

			// Token: 0x06000A76 RID: 2678 RVA: 0x0003F47D File Offset: 0x0003D67D
			[CompilerGenerated]
			public void Deconstruct(out int Min, out int Max, out string Source)
			{
				Min = this.Min;
				Max = this.Max;
				Source = this.Source;
			}
		}

		// Token: 0x02000135 RID: 309
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000639 RID: 1593
			[Nullable(0)]
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
