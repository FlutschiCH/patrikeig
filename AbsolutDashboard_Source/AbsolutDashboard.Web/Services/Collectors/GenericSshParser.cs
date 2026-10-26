using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GenericSshParser : ISshOutputParser
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000B577 File Offset: 0x00009777
		public string Key
		{
			get
			{
				return "generic";
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B580 File Offset: 0x00009780
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
				string text5 = ((text3.Length > 40) ? text3.Substring(0, 40) : text3);
				bool flag = false;
				foreach (ValueTuple<string, string> valueTuple in CliParse.KeyValues(text4))
				{
					string item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					deviceSnapshot.Metrics.Add(new Metric(item, item2, null, text5));
					flag = true;
				}
				if (!flag)
				{
					string text6 = string.Join(" ", CliParse.Lines(text4).Take<string>(3));
					if (text6.Length > 0)
					{
						deviceSnapshot.Metrics.Add(new Metric(text5, (text6.Length > 200) ? (text6.Substring(0, 200) + "…") : text6, null, "Output"));
					}
				}
			}
			return deviceSnapshot;
		}
	}
}
