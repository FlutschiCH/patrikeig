using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000048 RID: 72
	[NullableContext(2)]
	[Nullable(0)]
	public static class RouteAddr
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000AC34 File Offset: 0x00008E34
		public static string Port(string route)
		{
			if (string.IsNullOrWhiteSpace(route))
			{
				return null;
			}
			string text = route.Split('.', 2, StringSplitOptions.None)[0].Trim();
			if (text.Length <= 0)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000AC69 File Offset: 0x00008E69
		public static string Norm(string route)
		{
			if (!string.IsNullOrWhiteSpace(route))
			{
				return string.Join('.', route.Split('.', StringSplitOptions.TrimEntries));
			}
			return null;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000AC88 File Offset: 0x00008E88
		public static int? Leaf(string route)
		{
			if (string.IsNullOrWhiteSpace(route))
			{
				return null;
			}
			string[] array = route.Split('.', StringSplitOptions.TrimEntries);
			int? num = null;
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num3;
				if (int.TryParse(array[i], out num3) && num3 != 0)
				{
					num = new int?(num3);
					num2 = i;
				}
			}
			if (num2 <= 0)
			{
				return null;
			}
			return num;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000ACFC File Offset: 0x00008EFC
		public static string Parent(string route)
		{
			if (string.IsNullOrWhiteSpace(route))
			{
				return null;
			}
			string[] array = route.Split('.', StringSplitOptions.TrimEntries);
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				int num2;
				if (int.TryParse(array[i], out num2) && num2 != 0)
				{
					num = i;
				}
			}
			if (num <= 0)
			{
				return null;
			}
			array[num] = "0";
			return string.Join('.', array);
		}
	}
}
