using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CliParse
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0000B400 File Offset: 0x00009600
		[return: TupleElementNames(new string[] { "Key", "Value" })]
		[return: Nullable(new byte[] { 1, 0, 1, 1 })]
		public static IEnumerable<ValueTuple<string, string>> KeyValues(string text)
		{
			CliParse.<KeyValues>d__1 <KeyValues>d__ = new CliParse.<KeyValues>d__1(-2);
			<KeyValues>d__.<>3__text = text;
			return <KeyValues>d__;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B410 File Offset: 0x00009610
		public static IEnumerable<string> Lines(string text)
		{
			return from l in (text ?? "").Replace("\r", "").Split('\n', StringSplitOptions.None)
				where l.Trim().Length > 0
				select l;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B464 File Offset: 0x00009664
		public static bool? IsUp(string token)
		{
			string text = token.Trim().ToLowerInvariant();
			if (CliParse.UpTokens.Any<string>(new Func<string, bool>(text.Contains)))
			{
				return new bool?(true);
			}
			if (CliParse.DownTokens.Any<string>(new Func<string, bool>(text.Contains)))
			{
				return new bool?(false);
			}
			return null;
		}

		// Token: 0x04000100 RID: 256
		private static readonly Regex KeyValue = new Regex("^\\s*(?<k>[A-Za-z][\\w .\\-/()%]*?)\\s*[:=]\\s*(?<v>.+?)\\s*$", RegexOptions.Compiled);

		// Token: 0x04000101 RID: 257
		private static readonly string[] UpTokens = new string[] { "up", "online", "active", "connected", "running", "normal", "ok", "enabled" };

		// Token: 0x04000102 RID: 258
		private static readonly string[] DownTokens = new string[] { "down", "offline", "inactive", "disconnected", "fault", "alarm", "fail", "disabled" };
	}
}
