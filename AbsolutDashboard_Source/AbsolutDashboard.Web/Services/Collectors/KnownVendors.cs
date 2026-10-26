using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	public static class KnownVendors
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000DE0F File Offset: 0x0000C00F
		[NullableContext(2)]
		public static string NameForPen(int pen)
		{
			return KnownVendors.ByPen.GetValueOrDefault(pen);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000DE1C File Offset: 0x0000C01C
		public static int? PenForName(string name)
		{
			foreach (KeyValuePair<int, string> keyValuePair in KnownVendors.ByPen)
			{
				int num;
				string text;
				keyValuePair.Deconstruct(out num, out text);
				int num2 = num;
				if (text.Contains(name, StringComparison.OrdinalIgnoreCase))
				{
					return new int?(num2);
				}
			}
			return null;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000DE90 File Offset: 0x0000C090
		// Note: this type is marked as 'beforefieldinit'.
		static KnownVendors()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary[2011] = "Huawei";
			dictionary[25506] = "H3C";
			dictionary[9] = "Cisco";
			dictionary[2636] = "Juniper";
			dictionary[6527] = "Nokia (Alcatel-Lucent)";
			dictionary[193] = "Ericsson";
			dictionary[3902] = "ZTE";
			dictionary[14988] = "MikroTik";
			dictionary[12356] = "Fortinet";
			dictionary[30065] = "Arista";
			dictionary[14823] = "Aruba / HPE";
			dictionary[8072] = "Net-SNMP (Linux)";
			KnownVendors.ByPen = dictionary;
		}

		// Token: 0x04000142 RID: 322
		public static readonly IReadOnlyDictionary<int, string> ByPen;
	}
}
