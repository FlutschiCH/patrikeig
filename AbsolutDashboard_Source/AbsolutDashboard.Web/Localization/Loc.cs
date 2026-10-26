using System;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Localization
{
	// Token: 0x0200007E RID: 126
	public static class Loc
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00015C41 File Offset: 0x00013E41
		public static bool IsGerman
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00015C44 File Offset: 0x00013E44
		[NullableContext(2)]
		[return: Nullable(1)]
		public static string Pick(string de, string en)
		{
			return en ?? de ?? "";
		}
	}
}
