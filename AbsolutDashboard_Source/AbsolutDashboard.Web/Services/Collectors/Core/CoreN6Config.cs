using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	public static class CoreN6Config
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x00011108 File Offset: 0x0000F308
		[NullableContext(2)]
		public static N6Interface ParseInterface(string configInterfaceDump)
		{
			if (string.IsNullOrWhiteSpace(configInterfaceDump))
			{
				return null;
			}
			CoreN6Config.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.found = new List<N6Interface>();
			CS$<>8__locals1.name = null;
			CS$<>8__locals1.primary = null;
			CS$<>8__locals1.subs = new List<IpWithMask>();
			CS$<>8__locals1.acl = null;
			CS$<>8__locals1.group = null;
			foreach (string text in configInterfaceDump.Replace("\r", "").Split('\n', StringSplitOptions.None))
			{
				Match match = CoreN6Config.InterfaceHeader.Match(text);
				if (match.Success)
				{
					CoreN6Config.<ParseInterface>g__Flush|7_0(ref CS$<>8__locals1);
					CS$<>8__locals1.name = match.Groups[1].Value;
				}
				else if (CS$<>8__locals1.name != null)
				{
					Match match2 = CoreN6Config.IpAddressLine.Match(text);
					if (match2.Success)
					{
						IpWithMask ipWithMask = new IpWithMask(match2.Groups[1].Value, CoreN6Config.NormalizeMask(match2.Groups[2].Value));
						if (match2.Groups[3].Success)
						{
							CS$<>8__locals1.subs.Add(ipWithMask);
						}
						else if (CS$<>8__locals1.primary == null)
						{
							CS$<>8__locals1.primary = ipWithMask;
						}
					}
					else
					{
						Match match3 = CoreN6Config.NatOutboundLine.Match(text);
						if (match3.Success)
						{
							CS$<>8__locals1.acl = new int?(int.Parse(match3.Groups[1].Value, CultureInfo.InvariantCulture));
							CS$<>8__locals1.group = new int?(int.Parse(match3.Groups[2].Value, CultureInfo.InvariantCulture));
						}
					}
				}
			}
			CoreN6Config.<ParseInterface>g__Flush|7_0(ref CS$<>8__locals1);
			N6Interface n6Interface;
			if ((n6Interface = CS$<>8__locals1.found.FirstOrDefault<N6Interface>((N6Interface i) => i.NatAddressGroup != null)) == null)
			{
				n6Interface = CS$<>8__locals1.found.FirstOrDefault<N6Interface>((N6Interface i) => i.Secondaries.Count > 0) ?? CS$<>8__locals1.found.FirstOrDefault<N6Interface>();
			}
			return n6Interface;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00011328 File Offset: 0x0000F528
		[NullableContext(2)]
		[return: TupleElementNames(new string[] { "DefaultGateways", "Others" })]
		[return: Nullable(new byte[] { 0, 1, 1, 1, 1 })]
		public static ValueTuple<IReadOnlyList<string>, IReadOnlyList<IpWithMask>> ParseStaticRoutes(string routeLines)
		{
			List<string> list = new List<string>();
			List<IpWithMask> list2 = new List<IpWithMask>();
			if (string.IsNullOrWhiteSpace(routeLines))
			{
				return new ValueTuple<IReadOnlyList<string>, IReadOnlyList<IpWithMask>>(list, list2);
			}
			foreach (string text in routeLines.Replace("\r", "").Split('\n', StringSplitOptions.None))
			{
				Match match = CoreN6Config.RouteStaticLine.Match(text);
				if (match.Success)
				{
					string value = match.Groups[1].Value;
					string text2 = CoreN6Config.NormalizeMask(match.Groups[2].Value);
					string value2 = match.Groups[3].Value;
					if (value == "0.0.0.0" && text2 == "0.0.0.0")
					{
						string text3 = value2.Trim();
						if (text3.Split(' ', StringSplitOptions.RemoveEmptyEntries).Any<string>(delegate(string t)
						{
							IPAddress ipaddress;
							if (IPAddress.TryParse(t, out ipaddress))
							{
								return t.Count<char>((char c) => c == '.') == 3;
							}
							return false;
						}) && !list.Contains(text3))
						{
							list.Add(text3);
						}
					}
					else
					{
						list2.Add(new IpWithMask(value, text2));
					}
				}
			}
			return new ValueTuple<IReadOnlyList<string>, IReadOnlyList<IpWithMask>>(list, list2);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00011460 File Offset: 0x0000F660
		public static IReadOnlyDictionary<int, IReadOnlyList<string>> ParseNatGroups([Nullable(2)] string natDump)
		{
			CoreN6Config.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.result = new Dictionary<int, IReadOnlyList<string>>();
			if (string.IsNullOrWhiteSpace(natDump))
			{
				return CS$<>8__locals1.result;
			}
			CS$<>8__locals1.current = null;
			CS$<>8__locals1.addrs = new List<string>();
			foreach (string text in natDump.Replace("\r", "").Split('\n', StringSplitOptions.None))
			{
				Match match = CoreN6Config.NatGroupHeader.Match(text);
				if (match.Success)
				{
					CoreN6Config.<ParseNatGroups>g__Flush|9_0(ref CS$<>8__locals1);
					CS$<>8__locals1.current = new int?(int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture));
				}
				else
				{
					Match match2 = CoreN6Config.NatGroupRow.Match(text);
					if (match2.Success && CS$<>8__locals1.current != null)
					{
						CS$<>8__locals1.addrs.Add(match2.Groups[1].Value);
					}
				}
			}
			CoreN6Config.<ParseNatGroups>g__Flush|9_0(ref CS$<>8__locals1);
			return CS$<>8__locals1.result;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00011568 File Offset: 0x0000F768
		[NullableContext(2)]
		[return: Nullable(1)]
		public static List<string> Validate([Nullable(1)] N6State state, string newAddress, string newMask, string newGateway)
		{
			List<string> list = new List<string>();
			IPAddress ipaddress;
			if (!CoreN6Config.TryParseIPv4(newAddress, out ipaddress))
			{
				list.Add("Address must be an IPv4 address like 192.0.2.30.");
			}
			IPAddress ipaddress2;
			if (!CoreN6Config.TryParseMask(newMask, out ipaddress2))
			{
				list.Add("Mask must be dotted (255.255.255.0) or a prefix length (24).");
			}
			IPAddress ipaddress3;
			if (!CoreN6Config.TryParseIPv4(newGateway, out ipaddress3))
			{
				list.Add("Gateway must be an IPv4 address.");
			}
			if (list.Count > 0)
			{
				return list;
			}
			if (ipaddress.Equals(ipaddress3))
			{
				list.Add("Address and gateway must differ.");
			}
			if (!CoreN6Config.SameSubnet(ipaddress, ipaddress3, ipaddress2))
			{
				List<string> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Gateway ");
				defaultInterpolatedStringHandler.AppendFormatted<IPAddress>(ipaddress3);
				defaultInterpolatedStringHandler.AppendLiteral(" is not inside ");
				defaultInterpolatedStringHandler.AppendFormatted<IPAddress>(ipaddress);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CoreN6Config.PrefixLength(ipaddress2));
				defaultInterpolatedStringHandler.AppendLiteral(".");
				list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (CoreN6Config.IsNetworkOrBroadcast(ipaddress, ipaddress2))
			{
				List<string> list3 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<IPAddress>(ipaddress);
				defaultInterpolatedStringHandler2.AppendLiteral(" is the network or broadcast address of that subnet.");
				list3.Add(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			IpWithMask primary = state.Interface.Primary;
			IPAddress ipaddress4;
			IPAddress ipaddress5;
			if (primary != null && CoreN6Config.TryParseIPv4(primary.Address, out ipaddress4) && CoreN6Config.TryParseMask(primary.Mask, out ipaddress5))
			{
				if (ipaddress4.Equals(ipaddress))
				{
					List<string> list4 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler3.AppendFormatted<IPAddress>(ipaddress);
					defaultInterpolatedStringHandler3.AppendLiteral(" is the port's primary address (the base station side).");
					list4.Add(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				else if (CoreN6Config.SameSubnet(ipaddress, ipaddress4, ipaddress5) || CoreN6Config.SameSubnet(ipaddress, ipaddress4, ipaddress2))
				{
					List<string> list5 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(77, 3);
					defaultInterpolatedStringHandler4.AppendFormatted<IPAddress>(ipaddress);
					defaultInterpolatedStringHandler4.AppendLiteral("/");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(CoreN6Config.PrefixLength(ipaddress2));
					defaultInterpolatedStringHandler4.AppendLiteral(" overlaps the primary ");
					defaultInterpolatedStringHandler4.AppendFormatted<IpWithMask>(primary);
					defaultInterpolatedStringHandler4.AppendLiteral(" — the core refuses overlapping addresses on one port.");
					list5.Add(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
			}
			return list;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00011760 File Offset: 0x0000F960
		public static bool ManagementAtRisk(N6State state, [Nullable(2)] string dashboardAddress, out string reason)
		{
			reason = "";
			IPAddress ipaddress;
			if (CoreN6Config.TryParseIPv4(dashboardAddress, out ipaddress))
			{
				IpWithMask currentN = state.CurrentN6;
				if (currentN != null)
				{
					IPAddress ipaddress2;
					IPAddress ipaddress3;
					if (!CoreN6Config.TryParseIPv4(currentN.Address, out ipaddress2) || !CoreN6Config.TryParseMask(currentN.Mask, out ipaddress3))
					{
						return false;
					}
					if (!CoreN6Config.SameSubnet(ipaddress, ipaddress2, ipaddress3))
					{
						return false;
					}
					IpWithMask primary = state.Interface.Primary;
					IPAddress ipaddress4;
					IPAddress ipaddress5;
					if (primary != null && CoreN6Config.TryParseIPv4(primary.Address, out ipaddress4) && CoreN6Config.TryParseMask(primary.Mask, out ipaddress5) && CoreN6Config.SameSubnet(ipaddress, ipaddress4, ipaddress5))
					{
						return false;
					}
					foreach (IpWithMask ipWithMask in state.OtherStaticRoutes)
					{
						IPAddress ipaddress6;
						IPAddress ipaddress7;
						if (CoreN6Config.TryParseIPv4(ipWithMask.Address, out ipaddress6) && CoreN6Config.TryParseMask(ipWithMask.Mask, out ipaddress7) && CoreN6Config.SameSubnet(ipaddress, ipaddress6, ipaddress7))
						{
							return false;
						}
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 3);
					defaultInterpolatedStringHandler.AppendLiteral("The dashboard (");
					defaultInterpolatedStringHandler.AppendFormatted<IPAddress>(ipaddress);
					defaultInterpolatedStringHandler.AppendLiteral(") reaches this core through the secondary ");
					defaultInterpolatedStringHandler.AppendFormatted(currentN.Address);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(CoreN6Config.PrefixLength(ipaddress3));
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					reason = defaultInterpolatedStringHandler.ToStringAndClear() + "that this change removes, and the core has no other route back to it. Add a static route on the core for the dashboard's subnet first, or the core goes silent halfway through the change.";
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000118DC File Offset: 0x0000FADC
		public static List<CliStep> BuildSteps(N6State state, string newAddress, string newMask, string newGateway)
		{
			CoreN6Config.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.state = state;
			CS$<>8__locals1.newAddress = newAddress;
			CS$<>8__locals1.mask = CoreN6Config.NormalizeMask(newMask.Trim());
			string name = CS$<>8__locals1.state.Interface.Name;
			CS$<>8__locals1.steps = new List<CliStep>
			{
				new CliStep("Enter system view", "system-view"),
				new CliStep("Select the N6 port", "interface " + name)
			};
			IpWithMask currentN = CS$<>8__locals1.state.CurrentN6;
			IPAddress ipaddress;
			IPAddress ipaddress2;
			IPAddress ipaddress3;
			bool flag = currentN != null && CoreN6Config.TryParseIPv4(currentN.Address, out ipaddress) && CoreN6Config.TryParseMask(currentN.Mask, out ipaddress2) && CoreN6Config.TryParseIPv4(CS$<>8__locals1.newAddress, out ipaddress3) && (CoreN6Config.SameSubnet(ipaddress3, ipaddress, ipaddress2) || CoreN6Config.SameSubnet(ipaddress3, ipaddress, CoreN6Config.ParseMaskOrThrow(CS$<>8__locals1.mask)));
			CS$<>8__locals1.sameAddress = currentN != null && currentN.Address == CS$<>8__locals1.newAddress.Trim() && currentN.Mask == CS$<>8__locals1.mask;
			if (flag && !CS$<>8__locals1.sameAddress)
			{
				CoreN6Config.<BuildSteps>g__RemoveOld|12_0(ref CS$<>8__locals1);
				CoreN6Config.<BuildSteps>g__AddNew|12_1(ref CS$<>8__locals1);
			}
			else
			{
				CoreN6Config.<BuildSteps>g__AddNew|12_1(ref CS$<>8__locals1);
				CoreN6Config.<BuildSteps>g__RemoveOld|12_0(ref CS$<>8__locals1);
			}
			CS$<>8__locals1.steps.Add(new CliStep("Leave the port", "quit"));
			string text = newGateway.Trim();
			if (!CS$<>8__locals1.state.DefaultGateways.Contains(text))
			{
				CS$<>8__locals1.steps.Add(new CliStep("Default route via " + text, "ip route-static 0.0.0.0 0 " + text));
			}
			foreach (string text2 in CS$<>8__locals1.state.DefaultGateways)
			{
				if (text2 != text)
				{
					CS$<>8__locals1.steps.Add(new CliStep("Remove old default route via " + text2, "undo ip route-static 0.0.0.0 0 " + text2));
				}
			}
			int? natAddressGroup = CS$<>8__locals1.state.Interface.NatAddressGroup;
			if (natAddressGroup != null)
			{
				int valueOrDefault = natAddressGroup.GetValueOrDefault();
				string text3 = CS$<>8__locals1.newAddress.Trim();
				List<CliStep> steps = CS$<>8__locals1.steps;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
				defaultInterpolatedStringHandler.AppendLiteral("NAT group ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault);
				string text4 = defaultInterpolatedStringHandler.ToStringAndClear();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("nat address-group ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(valueOrDefault);
				steps.Add(new CliStep(text4, defaultInterpolatedStringHandler2.ToStringAndClear()));
				if (!CS$<>8__locals1.state.NatSourceAddresses.Contains(text3))
				{
					CS$<>8__locals1.steps.Add(new CliStep("NAT source " + text3, "address " + text3 + " " + text3));
				}
				foreach (string text5 in CS$<>8__locals1.state.NatSourceAddresses)
				{
					if (text5 != text3)
					{
						CS$<>8__locals1.steps.Add(new CliStep("Drop NAT source " + text5, "undo address " + text5 + " " + text5));
					}
				}
				CS$<>8__locals1.steps.Add(new CliStep("Leave the NAT group", "quit"));
			}
			CS$<>8__locals1.steps.Add(new CliStep("Leave system view", "quit"));
			CS$<>8__locals1.steps.Add(new CliStep("Save configuration", "save force"));
			return CS$<>8__locals1.steps;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00011C90 File Offset: 0x0000FE90
		[NullableContext(2)]
		public static bool IsError(string reply)
		{
			return !string.IsNullOrWhiteSpace(reply) && CoreN6Config.ErrorLine.IsMatch(reply);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00011CA8 File Offset: 0x0000FEA8
		public static string ErrorText([Nullable(2)] string reply)
		{
			if (string.IsNullOrWhiteSpace(reply))
			{
				return "";
			}
			Match match = CoreN6Config.ErrorLine.Match(reply);
			if (!match.Success)
			{
				return reply.Trim();
			}
			return reply.Substring(match.Index).Split('\n', StringSplitOptions.None)[0].Trim().TrimStart('%').Trim();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00011D08 File Offset: 0x0000FF08
		[NullableContext(2)]
		public static bool TryParseIPv4(string s, [NotNullWhen(true)] out IPAddress ip)
		{
			ip = null;
			if (string.IsNullOrWhiteSpace(s))
			{
				return false;
			}
			if (IPAddress.TryParse(s.Trim(), out ip) && ip.AddressFamily == AddressFamily.InterNetwork)
			{
				return s.Trim().Count<char>((char c) => c == '.') == 3;
			}
			return false;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00011D6C File Offset: 0x0000FF6C
		[NullableContext(2)]
		public static bool TryParseMask(string s, [NotNullWhen(true)] out IPAddress mask)
		{
			mask = null;
			if (string.IsNullOrWhiteSpace(s))
			{
				return false;
			}
			string text = s.Trim();
			int num;
			if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				bool flag = num < 0 || num > 32;
				if (flag)
				{
					return false;
				}
				uint num2 = ((num == 0) ? 0U : (uint.MaxValue << 32 - num));
				mask = new IPAddress(new byte[]
				{
					(byte)(num2 >> 24),
					(byte)(num2 >> 16),
					(byte)(num2 >> 8),
					(byte)num2
				});
				return true;
			}
			else
			{
				if (!CoreN6Config.TryParseIPv4(text, out mask))
				{
					return false;
				}
				uint num3 = ~CoreN6Config.ToUInt(mask);
				return (num3 & (num3 + 1U)) == 0U;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00011E08 File Offset: 0x00010008
		public static string NormalizeMask(string s)
		{
			IPAddress ipaddress;
			if (!CoreN6Config.TryParseMask(s, out ipaddress))
			{
				return s;
			}
			return ipaddress.ToString();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00011E28 File Offset: 0x00010028
		public static int PrefixLength(IPAddress mask)
		{
			uint num = CoreN6Config.ToUInt(mask);
			int num2 = 0;
			while (num != 0U)
			{
				num2 += (int)(num & 1U);
				num >>= 1;
			}
			return num2;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00011E50 File Offset: 0x00010050
		public static bool SameSubnet(IPAddress a, IPAddress b, IPAddress mask)
		{
			uint num = CoreN6Config.ToUInt(mask);
			return (CoreN6Config.ToUInt(a) & num) == (CoreN6Config.ToUInt(b) & num);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00011E78 File Offset: 0x00010078
		private static bool IsNetworkOrBroadcast(IPAddress ip, IPAddress mask)
		{
			uint num = CoreN6Config.ToUInt(mask);
			if (num == 4294967295U)
			{
				return false;
			}
			uint num2 = CoreN6Config.ToUInt(ip);
			return (num2 & ~(num != 0U)) == 0U || (num2 | num) == uint.MaxValue;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00011EA8 File Offset: 0x000100A8
		private static IPAddress ParseMaskOrThrow(string mask)
		{
			IPAddress ipaddress;
			if (!CoreN6Config.TryParseMask(mask, out ipaddress))
			{
				throw new ArgumentException("Bad mask '" + mask + "'.");
			}
			return ipaddress;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00011ED8 File Offset: 0x000100D8
		private static uint ToUInt(IPAddress ip)
		{
			byte[] addressBytes = ip.GetAddressBytes();
			return (uint)(((int)addressBytes[0] << 24) | ((int)addressBytes[1] << 16) | ((int)addressBytes[2] << 8) | (int)addressBytes[3]);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00011F88 File Offset: 0x00010188
		[CompilerGenerated]
		internal static void <ParseInterface>g__Flush|7_0(ref CoreN6Config.<>c__DisplayClass7_0 A_0)
		{
			if (A_0.name != null && !A_0.name.Equals("NULL0", StringComparison.OrdinalIgnoreCase) && (A_0.primary != null || A_0.subs.Count > 0))
			{
				A_0.found.Add(new N6Interface(A_0.name, A_0.primary, A_0.subs.ToArray(), A_0.acl, A_0.group));
			}
			A_0.name = null;
			A_0.primary = null;
			A_0.subs = new List<IpWithMask>();
			A_0.acl = null;
			A_0.group = null;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001202C File Offset: 0x0001022C
		[CompilerGenerated]
		internal static void <ParseNatGroups>g__Flush|9_0(ref CoreN6Config.<>c__DisplayClass9_0 A_0)
		{
			if (A_0.current != null)
			{
				int valueOrDefault = A_0.current.GetValueOrDefault();
				A_0.result[valueOrDefault] = A_0.addrs.ToArray();
			}
			A_0.addrs = new List<string>();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00012074 File Offset: 0x00010274
		[CompilerGenerated]
		internal static void <BuildSteps>g__RemoveOld|12_0(ref CoreN6Config.<>c__DisplayClass12_0 A_0)
		{
			foreach (IpWithMask ipWithMask in A_0.state.Interface.Secondaries)
			{
				if (!(ipWithMask.Address == A_0.newAddress.Trim()) || !(ipWithMask.Mask == A_0.mask))
				{
					List<CliStep> steps = A_0.steps;
					string text = "Remove old secondary " + ipWithMask.Address;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
					defaultInterpolatedStringHandler.AppendLiteral("undo ip address ");
					defaultInterpolatedStringHandler.AppendFormatted(ipWithMask.Address);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(ipWithMask.Mask);
					defaultInterpolatedStringHandler.AppendLiteral(" sub");
					steps.Add(new CliStep(text, defaultInterpolatedStringHandler.ToStringAndClear()));
				}
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00012164 File Offset: 0x00010364
		[CompilerGenerated]
		internal static void <BuildSteps>g__AddNew|12_1(ref CoreN6Config.<>c__DisplayClass12_0 A_0)
		{
			if (!A_0.sameAddress)
			{
				List<CliStep> steps = A_0.steps;
				string text = "Add new secondary " + A_0.newAddress.Trim();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
				defaultInterpolatedStringHandler.AppendLiteral("ip address ");
				defaultInterpolatedStringHandler.AppendFormatted(A_0.newAddress.Trim());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(A_0.mask);
				defaultInterpolatedStringHandler.AppendLiteral(" sub");
				steps.Add(new CliStep(text, defaultInterpolatedStringHandler.ToStringAndClear()));
			}
		}

		// Token: 0x0400019B RID: 411
		private static readonly Regex InterfaceHeader = new Regex("^\\s*interface\\s+(\\S+)\\s*$", RegexOptions.Compiled);

		// Token: 0x0400019C RID: 412
		private static readonly Regex IpAddressLine = new Regex("^\\s*ip address\\s+(\\d{1,3}(?:\\.\\d{1,3}){3})\\s+(\\d{1,3}(?:\\.\\d{1,3}){3}|\\d{1,2})(\\s+sub)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400019D RID: 413
		private static readonly Regex NatOutboundLine = new Regex("^\\s*nat outbound\\s+(\\d+)\\s+address-group\\s+(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400019E RID: 414
		private static readonly Regex RouteStaticLine = new Regex("^\\s*ip route-static\\s+(\\d{1,3}(?:\\.\\d{1,3}){3})\\s+(\\d{1,3}(?:\\.\\d{1,3}){3}|\\d{1,2})\\s+(.+?)\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400019F RID: 415
		private static readonly Regex NatGroupHeader = new Regex("Address group ID:\\s*(\\d+)", RegexOptions.Compiled);

		// Token: 0x040001A0 RID: 416
		private static readonly Regex NatGroupRow = new Regex("^\\s*(\\d{1,3}(?:\\.\\d{1,3}){3})\\s+(\\d{1,3}(?:\\.\\d{1,3}){3})\\s*$", RegexOptions.Compiled);

		// Token: 0x040001A1 RID: 417
		private static readonly Regex ErrorLine = new Regex("^\\s*(%(?!\\s?[A-Z][a-z]{2}\\s+\\d)|Error:|Invalid |Incomplete |Unrecognized |Ambiguous |Too many |The IP address .* overlaps)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
	}
}
