using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004C RID: 76
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SshParserRegistry
	{
		// Token: 0x06000264 RID: 612 RVA: 0x0000B374 File Offset: 0x00009574
		public SshParserRegistry(IEnumerable<ISshOutputParser> parsers)
		{
			this._byKey = parsers.ToDictionary<ISshOutputParser, string>((ISshOutputParser p) => p.Key, StringComparer.OrdinalIgnoreCase);
			this._fallback = this._byKey.GetValueOrDefault("generic") ?? new GenericSshParser();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B3D8 File Offset: 0x000095D8
		public ISshOutputParser Resolve([Nullable(2)] string key)
		{
			ISshOutputParser sshOutputParser;
			if (key == null || !this._byKey.TryGetValue(key, out sshOutputParser))
			{
				return this._fallback;
			}
			return sshOutputParser;
		}

		// Token: 0x040000FE RID: 254
		private readonly Dictionary<string, ISshOutputParser> _byKey;

		// Token: 0x040000FF RID: 255
		private readonly ISshOutputParser _fallback;
	}
}
