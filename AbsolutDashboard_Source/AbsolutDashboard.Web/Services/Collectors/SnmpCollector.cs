using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x0200004A RID: 74
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SnmpCollector : IDeviceCollector
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000ADAD File Offset: 0x00008FAD
		public SnmpCollector(CredentialProtector creds, ILogger<SnmpCollector> log)
		{
			this._creds = creds;
			this._log = log;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000ADC3 File Offset: 0x00008FC3
		public CollectionMethod Method
		{
			get
			{
				return CollectionMethod.Snmp;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		public async Task<PollOutcome> CollectAsync(Device device, CancellationToken ct)
		{
			SnmpCollector.<>c__DisplayClass16_0 CS$<>8__locals1 = new SnmpCollector.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.device = device;
			PollOutcome pollOutcome;
			if (string.IsNullOrWhiteSpace(CS$<>8__locals1.device.IpAddress))
			{
				pollOutcome = PollOutcome.Unreachable("No IP address configured.");
			}
			else
			{
				CS$<>8__locals1.community = this._creds.Unprotect(CS$<>8__locals1.device.SnmpCommunityEnc);
				IPAddress ipaddress;
				if (string.IsNullOrEmpty(CS$<>8__locals1.community))
				{
					pollOutcome = PollOutcome.Unreachable("No SNMP community configured.");
				}
				else if (!IPAddress.TryParse(CS$<>8__locals1.device.IpAddress, out ipaddress))
				{
					pollOutcome = PollOutcome.Unreachable("Invalid IP address '" + CS$<>8__locals1.device.IpAddress + "'.");
				}
				else
				{
					int num = ((CS$<>8__locals1.device.Port > 0) ? CS$<>8__locals1.device.Port : 161);
					CS$<>8__locals1.version = (CS$<>8__locals1.device.SnmpVersion.Equals("v1", StringComparison.OrdinalIgnoreCase) ? VersionCode.V1 : VersionCode.V2);
					CS$<>8__locals1.endpoint = new IPEndPoint(ipaddress, num);
					SnmpCollector.<>c__DisplayClass16_0 CS$<>8__locals2 = CS$<>8__locals1;
					DeviceTemplate template = CS$<>8__locals1.device.Template;
					CS$<>8__locals2.def = TemplateDefinition.Parse((template != null) ? template.DefinitionJson : null).Snmp ?? new SnmpDefinition();
					CS$<>8__locals1.sw = Stopwatch.StartNew();
					try
					{
						pollOutcome = await Task.Run<PollOutcome>(delegate
						{
							DeviceSnapshot deviceSnapshot = new DeviceSnapshot();
							List<Variable> list = new List<Variable>
							{
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"), null),
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"), null),
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), null),
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"), null),
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0"), null),
								new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.4.0"), null)
							};
							Dictionary<string, string> dictionary = Messenger.Get(CS$<>8__locals1.version, CS$<>8__locals1.endpoint, new OctetString(CS$<>8__locals1.community), list, 4000).ToDictionary<Variable, string, string>((Variable v) => v.Id.ToString(), (Variable v) => v.Data.ToString());
							SnmpCollector.AddIfPresent(deviceSnapshot, "System name", dictionary, "1.3.6.1.2.1.1.5.0", "System");
							string text;
							if (dictionary.TryGetValue("1.3.6.1.2.1.1.3.0", out text) && !string.IsNullOrWhiteSpace(text))
							{
								deviceSnapshot.Metrics.Add(new Metric("Uptime", UptimeFormat.FromTimeTicks(text), null, "System"));
							}
							int? vendorEnterprise = CS$<>8__locals1.def.VendorEnterprise;
							if (vendorEnterprise != null)
							{
								int valueOrDefault = vendorEnterprise.GetValueOrDefault();
								string text2 = SnmpCollector.VendorMismatch(dictionary, valueOrDefault, CS$<>8__locals1.def.VendorName);
								if (text2 != null)
								{
									return new PollOutcome
									{
										Reachable = false,
										Error = text2,
										Snapshot = deviceSnapshot
									};
								}
							}
							foreach (SnmpMetric snmpMetric in CS$<>8__locals1.def.Metrics.Where<SnmpMetric>((SnmpMetric m) => !string.IsNullOrWhiteSpace(m.Oid)))
							{
								try
								{
									string text3;
									if (snmpMetric.Walk)
									{
										text3 = SnmpCollector.AggregateColumn(CS$<>8__locals1.version, CS$<>8__locals1.endpoint, CS$<>8__locals1.community, snmpMetric.Oid, snmpMetric.Aggregate, 4000);
									}
									else
									{
										Variable variable = Messenger.Get(CS$<>8__locals1.version, CS$<>8__locals1.endpoint, new OctetString(CS$<>8__locals1.community), new List<Variable>
										{
											new Variable(new ObjectIdentifier(snmpMetric.Oid), null)
										}, 4000).FirstOrDefault<Variable>();
										text3 = ((variable != null) ? variable.Data.ToString() : null) ?? "";
									}
									if (!string.IsNullOrWhiteSpace(text3))
									{
										deviceSnapshot.Metrics.Add(new Metric(snmpMetric.Label, text3, snmpMetric.Unit, snmpMetric.Group ?? "Metrics"));
									}
								}
								catch (Exception ex2)
								{
									CS$<>8__locals1.<>4__this._log.LogDebug(ex2, "OID {Oid} failed on {Device}", new object[]
									{
										snmpMetric.Oid,
										CS$<>8__locals1.device.Name
									});
								}
							}
							if (CS$<>8__locals1.def.WalkInterfaces)
							{
								try
								{
									deviceSnapshot.Ports.AddRange(SnmpCollector.WalkInterfaces(CS$<>8__locals1.version, CS$<>8__locals1.endpoint, CS$<>8__locals1.community, 4000));
								}
								catch (Exception ex3)
								{
									CS$<>8__locals1.<>4__this._log.LogDebug(ex3, "Interface walk failed on {Device}", new object[] { CS$<>8__locals1.device.Name });
								}
							}
							CS$<>8__locals1.sw.Stop();
							return new PollOutcome
							{
								Reachable = true,
								LatencyMs = new int?((int)CS$<>8__locals1.sw.ElapsedMilliseconds),
								Snapshot = deviceSnapshot
							};
						}, ct);
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (TimeoutException)
					{
						pollOutcome = PollOutcome.Unreachable("SNMP timeout — no response (device down, wrong community, or SNMP disabled).");
					}
					catch (Exception ex)
					{
						pollOutcome = PollOutcome.Unreachable("SNMP error: " + ex.Message);
					}
				}
			}
			return pollOutcome;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AE1C File Offset: 0x0000901C
		private static void AddIfPresent(DeviceSnapshot s, string label, Dictionary<string, string> sys, string oid, string group)
		{
			string text;
			if (sys.TryGetValue(oid, out text) && !string.IsNullOrWhiteSpace(text))
			{
				s.Metrics.Add(new Metric(label, text, null, group));
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000AE54 File Offset: 0x00009054
		[NullableContext(2)]
		private static string VendorMismatch([Nullable(1)] Dictionary<string, string> sys, int expectedPen, string vendorName)
		{
			string text = vendorName;
			if (vendorName == null && (text = KnownVendors.NameForPen(expectedPen)) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendLiteral("enterprise ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(expectedPen);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string text2 = text;
			string text3;
			sys.TryGetValue("1.3.6.1.2.1.1.2.0", out text3);
			int? num = SnmpCollector.EnterpriseOf(text3);
			int? num2 = num;
			if ((num2.GetValueOrDefault() == expectedPen) & (num2 != null))
			{
				return null;
			}
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				string text4 = KnownVendors.NameForPen(valueOrDefault);
				string text5;
				if (text4 == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(11, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("enterprise ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(valueOrDefault);
					text5 = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(13, 2);
					defaultInterpolatedStringHandler3.AppendFormatted(text4);
					defaultInterpolatedStringHandler3.AppendLiteral(", enterprise ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(valueOrDefault);
					text5 = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				string text6 = text5;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(51, 3);
				defaultInterpolatedStringHandler4.AppendLiteral("Not a ");
				defaultInterpolatedStringHandler4.AppendFormatted(text2);
				defaultInterpolatedStringHandler4.AppendLiteral(" device (sysObjectID ");
				defaultInterpolatedStringHandler4.AppendFormatted(text6);
				defaultInterpolatedStringHandler4.AppendLiteral("; expected enterprise ");
				defaultInterpolatedStringHandler4.AppendFormatted<int>(expectedPen);
				defaultInterpolatedStringHandler4.AppendLiteral(").");
				return defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			string text7;
			if (!string.IsNullOrEmpty(vendorName) && sys.TryGetValue("1.3.6.1.2.1.1.1.0", out text7) && text7.Contains(vendorName, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return "Could not verify this is a " + text2 + " device (sysObjectID unavailable).";
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AFD0 File Offset: 0x000091D0
		[NullableContext(2)]
		private static int? EnterpriseOf(string sysObjectId)
		{
			if (string.IsNullOrEmpty(sysObjectId) || !sysObjectId.StartsWith("1.3.6.1.4.1."))
			{
				return null;
			}
			string text = sysObjectId.Substring("1.3.6.1.4.1.".Length);
			int num = text.IndexOf('.');
			int num2;
			if (!int.TryParse((num < 0) ? text : text.Substring(0, num), out num2))
			{
				return null;
			}
			return new int?(num2);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000B03E File Offset: 0x0000923E
		private static IEnumerable<PortStatus> WalkInterfaces(VersionCode version, IPEndPoint endpoint, string community, int timeoutMs)
		{
			SnmpCollector.<WalkInterfaces>d__20 <WalkInterfaces>d__ = new SnmpCollector.<WalkInterfaces>d__20(-2);
			<WalkInterfaces>d__.<>3__version = version;
			<WalkInterfaces>d__.<>3__endpoint = endpoint;
			<WalkInterfaces>d__.<>3__community = community;
			<WalkInterfaces>d__.<>3__timeoutMs = timeoutMs;
			return <WalkInterfaces>d__;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B064 File Offset: 0x00009264
		private static string AggregateColumn(VersionCode version, IPEndPoint endpoint, string community, string columnOid, [Nullable(2)] string aggregate, int timeoutMs)
		{
			List<string> list = (from kv in SnmpCollector.WalkColumn(version, endpoint, community, columnOid, timeoutMs)
				orderby SnmpCollector.IndexSort(kv.Key)
				select kv.Value into v
				where !string.IsNullOrWhiteSpace(v)
				select v).ToList<string>();
			if (list.Count == 0)
			{
				return "";
			}
			string text = (aggregate ?? "max").Trim().ToLowerInvariant();
			if (text == "list")
			{
				return string.Join(", ", list);
			}
			if (text == "first")
			{
				return list[0];
			}
			List<double> list2 = (from d in list.Select<string, double?>(delegate(string v)
				{
					double num3;
					if (!double.TryParse(v.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out num3))
					{
						return null;
					}
					return new double?(num3);
				})
				where d != null
				select d.Value).ToList<double>();
			if (list2.Count == 0)
			{
				return list[0];
			}
			double num;
			if (!(text == "min"))
			{
				if (!(text == "avg"))
				{
					if (!(text == "sum"))
					{
						num = list2.Max();
					}
					else
					{
						num = list2.Sum();
					}
				}
				else
				{
					num = list2.Average();
				}
			}
			else
			{
				num = list2.Min();
			}
			double num2 = num;
			return num2.ToString("0.##", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000B228 File Offset: 0x00009428
		private static Dictionary<string, string> WalkColumn(VersionCode version, IPEndPoint endpoint, string community, string columnOid, int timeoutMs)
		{
			List<Variable> list = new List<Variable>();
			Messenger.Walk(version, endpoint, new OctetString(community), new ObjectIdentifier(columnOid), list, timeoutMs, WalkMode.WithinSubtree);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (Variable variable in list)
			{
				string text = variable.Id.ToString();
				string text2 = text.Substring(text.LastIndexOf('.') + 1);
				dictionary[text2] = variable.Data.ToString();
			}
			return dictionary;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000B2C8 File Offset: 0x000094C8
		private static int IndexSort(string idx)
		{
			int num;
			if (!int.TryParse(idx, out num))
			{
				return int.MaxValue;
			}
			return num;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B2E8 File Offset: 0x000094E8
		private static string StatusText(string raw)
		{
			string text = raw.Trim();
			if (text != null)
			{
				int length = text.Length;
				if (length == 1)
				{
					switch (text[0])
					{
					case '1':
						return "up";
					case '2':
						return "down";
					case '3':
						return "testing";
					case '4':
						return "unknown";
					case '5':
						return "dormant";
					case '6':
						return "notPresent";
					case '7':
						return "lowerLayerDown";
					}
				}
			}
			return raw;
		}

		// Token: 0x040000F1 RID: 241
		private readonly CredentialProtector _creds;

		// Token: 0x040000F2 RID: 242
		private readonly ILogger<SnmpCollector> _log;

		// Token: 0x040000F3 RID: 243
		private const string SysDescr = "1.3.6.1.2.1.1.1.0";

		// Token: 0x040000F4 RID: 244
		private const string SysObjectId = "1.3.6.1.2.1.1.2.0";

		// Token: 0x040000F5 RID: 245
		private const string SysUpTime = "1.3.6.1.2.1.1.3.0";

		// Token: 0x040000F6 RID: 246
		private const string SysContact = "1.3.6.1.2.1.1.4.0";

		// Token: 0x040000F7 RID: 247
		private const string SysName = "1.3.6.1.2.1.1.5.0";

		// Token: 0x040000F8 RID: 248
		private const string SysLocation = "1.3.6.1.2.1.1.6.0";

		// Token: 0x040000F9 RID: 249
		private const string IfDescr = "1.3.6.1.2.1.2.2.1.2";

		// Token: 0x040000FA RID: 250
		private const string IfType = "1.3.6.1.2.1.2.2.1.3";

		// Token: 0x040000FB RID: 251
		private const string IfAdminStatus = "1.3.6.1.2.1.2.2.1.7";

		// Token: 0x040000FC RID: 252
		private const string IfOperStatus = "1.3.6.1.2.1.2.2.1.8";

		// Token: 0x040000FD RID: 253
		private const string IfAlias = "1.3.6.1.2.1.31.1.1.1.18";
	}
}
