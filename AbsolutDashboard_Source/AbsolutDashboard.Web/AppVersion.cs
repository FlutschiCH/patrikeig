using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	public static class AppVersion
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000058E0 File Offset: 0x00003AE0
		public static string Full { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000058E8 File Offset: 0x00003AE8
		public static string Display
		{
			get
			{
				int num = AppVersion.Full.IndexOf('+');
				if (num >= 0)
				{
					return "v" + AppVersion.Full.Substring(0, num) + " · " + AppVersion.Full.Substring(num + 1);
				}
				return "v" + AppVersion.Full;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005940 File Offset: 0x00003B40
		// Note: this type is marked as 'beforefieldinit'.
		static AppVersion()
		{
			AssemblyInformationalVersionAttribute customAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
			string text;
			if ((text = ((customAttribute != null) ? customAttribute.InformationalVersion : null)) == null)
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				text = ((version != null) ? version.ToString(3) : null) ?? "unknown";
			}
			AppVersion.Full = text;
		}
	}
}
