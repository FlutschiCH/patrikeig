using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.DataProtection;

namespace AbsolutDashboard.Web.Services.Integrity
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IntegrityStore
	{
		// Token: 0x06000127 RID: 295 RVA: 0x000065F4 File Offset: 0x000047F4
		public IntegrityStore(IDataProtectionProvider dataProtection, string contentRoot)
		{
			this._protector = dataProtection.CreateProtector("AbsolutDashboard.Diagnostics.v1");
			this._dirs = (from d in IntegrityStore.BuildDirs(contentRoot)
				where !string.IsNullOrWhiteSpace(d)
				select (d)).Distinct<string>(StringComparer.OrdinalIgnoreCase).ToArray<string>();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000667B File Offset: 0x0000487B
		[return: Nullable(new byte[] { 1, 2 })]
		private static IEnumerable<string> BuildDirs(string contentRoot)
		{
			IntegrityStore.<BuildDirs>d__5 <BuildDirs>d__ = new IntegrityStore.<BuildDirs>d__5(-2);
			<BuildDirs>d__.<>3__contentRoot = contentRoot;
			return <BuildDirs>d__;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000668C File Offset: 0x0000488C
		[NullableContext(2)]
		public string ReadToken()
		{
			foreach (string text in this._dirs)
			{
				try
				{
					string text2 = Path.Combine(text, ".dp-cache");
					if (File.Exists(text2))
					{
						string text3 = File.ReadAllText(text2).Trim();
						if (!string.IsNullOrWhiteSpace(text3))
						{
							return text3;
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006700 File Offset: 0x00004900
		public void HealToken(string token)
		{
			foreach (string text in this._dirs)
			{
				try
				{
					string text2 = Path.Combine(text, ".dp-cache");
					if (!File.Exists(text2) || !(File.ReadAllText(text2).Trim() == token))
					{
						Directory.CreateDirectory(text);
						File.WriteAllText(text2, token);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006774 File Offset: 0x00004974
		public DateTime ReadHighWater()
		{
			DateTime dateTime = default(DateTime);
			foreach (string text in this._dirs)
			{
				try
				{
					string text2 = Path.Combine(text, ".dp-cache.st");
					if (File.Exists(text2))
					{
						string text3 = File.ReadAllText(text2).Trim();
						if (text3.Length != 0)
						{
							long num;
							if (long.TryParse(this._protector.Unprotect(text3), out num) && num >= 0L && num <= DateTime.MaxValue.Ticks)
							{
								DateTime dateTime2 = new DateTime(num, DateTimeKind.Utc);
								if (dateTime2 > dateTime)
								{
									dateTime = dateTime2;
								}
							}
						}
					}
				}
				catch
				{
				}
			}
			return dateTime;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006830 File Offset: 0x00004A30
		public void WriteHighWater(DateTime highWaterUtc)
		{
			string text;
			try
			{
				text = this._protector.Protect(highWaterUtc.Ticks.ToString());
			}
			catch
			{
				return;
			}
			foreach (string text2 in this._dirs)
			{
				try
				{
					Directory.CreateDirectory(text2);
					File.WriteAllText(Path.Combine(text2, ".dp-cache.st"), text);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0400008C RID: 140
		public const string TokenFileName = ".dp-cache";

		// Token: 0x0400008D RID: 141
		public const string StateFileName = ".dp-cache.st";

		// Token: 0x0400008E RID: 142
		private readonly IDataProtector _protector;

		// Token: 0x0400008F RID: 143
		private readonly string[] _dirs;
	}
}
