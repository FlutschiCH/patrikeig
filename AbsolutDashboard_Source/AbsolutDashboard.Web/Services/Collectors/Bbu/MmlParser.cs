using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x0200007D RID: 125
	[NullableContext(1)]
	[Nullable(0)]
	public static class MmlParser
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00015844 File Offset: 0x00013A44
		public static MmlResponse Parse([Nullable(2)] string content)
		{
			string[] array = (content ?? "").Replace("\r", "").Split('\n', StringSplitOptions.None);
			int num = -1;
			int num2 = -1;
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				Match match = MmlParser.RetCodeRe.Match(array[i]);
				if (match.Success)
				{
					num = int.Parse(match.Groups[1].Value);
					text = match.Groups[2].Value.Trim();
					num2 = i;
					break;
				}
			}
			int num3 = -1;
			int num4 = num2 + 1;
			while (num4 >= 1 && num4 < array.Length && !MmlParser.EndRe.IsMatch(array[num4]))
			{
				if (MmlParser.DashesRe.IsMatch(array[num4]))
				{
					num3 = num4;
					break;
				}
				num4++;
			}
			List<string> list = new List<string>();
			if (num3 >= 0)
			{
				int num5 = num3 + 1;
				while (num5 < array.Length && !MmlParser.EndRe.IsMatch(array[num5]) && !MmlParser.ResultsRe.IsMatch(array[num5]))
				{
					if (!string.IsNullOrWhiteSpace(array[num5]))
					{
						list.Add(array[num5]);
					}
					num5++;
				}
			}
			if (list.Count == 0)
			{
				return new MmlResponse
				{
					RetCode = num,
					Message = text
				};
			}
			if (list.All<string>((string l) => l.Contains('=')))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				foreach (string text2 in list)
				{
					int num6 = text2.IndexOf('=');
					string text3 = text2.Substring(0, num6).Trim();
					if (text3.Length > 0)
					{
						dictionary[text3] = MmlParser.Normalize(text2.Substring(num6 + 1).Trim());
					}
				}
				return new MmlResponse
				{
					RetCode = num,
					Message = text,
					Scalars = dictionary
				};
			}
			List<string> list2 = (from c in MmlParser.ColSplit.Split(list[0].Trim())
				where c.Length > 0
				select c).ToList<string>();
			List<Dictionary<string, string>> list3 = new List<Dictionary<string, string>>();
			for (int j = 1; j < list.Count; j++)
			{
				string[] array2 = MmlParser.ColSplit.Split(list[j].Trim());
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				for (int k = 0; k < list2.Count; k++)
				{
					dictionary2[list2[k]] = ((k < array2.Length) ? MmlParser.Normalize(array2[k].Trim()) : "");
				}
				list3.Add(dictionary2);
			}
			return new MmlResponse
			{
				RetCode = num,
				Message = text,
				Columns = list2,
				Rows = list3
			};
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00015B60 File Offset: 0x00013D60
		public static MmlResponse ParseMulti(IEnumerable<string> frames)
		{
			MmlResponse mmlResponse = null;
			foreach (string text in frames)
			{
				MmlResponse mmlResponse2 = MmlParser.Parse(text);
				if (mmlResponse == null)
				{
					mmlResponse = mmlResponse2;
				}
				else
				{
					mmlResponse.Rows.AddRange(mmlResponse2.Rows);
				}
			}
			return mmlResponse ?? new MmlResponse();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00015BCC File Offset: 0x00013DCC
		private static string Normalize(string v)
		{
			if (!string.Equals(v, "NULL", StringComparison.OrdinalIgnoreCase))
			{
				return v;
			}
			return "";
		}

		// Token: 0x040001EC RID: 492
		private static readonly Regex RetCodeRe = new Regex("^\\s*RETCODE\\s*=\\s*(\\d+)\\s*(.*)$", RegexOptions.Compiled);

		// Token: 0x040001ED RID: 493
		private static readonly Regex DashesRe = new Regex("^\\s*-{3,}\\s*$", RegexOptions.Compiled);

		// Token: 0x040001EE RID: 494
		private static readonly Regex EndRe = new Regex("^\\s*-{3,}\\s+END", RegexOptions.Compiled);

		// Token: 0x040001EF RID: 495
		private static readonly Regex ResultsRe = new Regex("^\\s*\\(Number of results", RegexOptions.Compiled);

		// Token: 0x040001F0 RID: 496
		private static readonly Regex ColSplit = new Regex("\\s{2,}", RegexOptions.Compiled);
	}
}
