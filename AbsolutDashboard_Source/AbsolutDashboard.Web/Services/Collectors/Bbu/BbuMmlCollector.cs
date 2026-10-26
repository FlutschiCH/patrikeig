using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuMmlCollector : IDeviceCollector
	{
		// Token: 0x0600049B RID: 1179 RVA: 0x000149E6 File Offset: 0x00012BE6
		public BbuMmlCollector(CredentialProtector creds, ILogger<BbuMmlCollector> log)
		{
			this._creds = creds;
			this._log = log;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x000149FC File Offset: 0x00012BFC
		public CollectionMethod Method
		{
			get
			{
				return CollectionMethod.BbuWebSocket;
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00014A00 File Offset: 0x00012C00
		public async Task<PollOutcome> CollectAsync(Device device, CancellationToken ct)
		{
			PollOutcome pollOutcome;
			if (string.IsNullOrWhiteSpace(device.IpAddress))
			{
				pollOutcome = PollOutcome.Unreachable("No IP address configured.");
			}
			else if (string.IsNullOrWhiteSpace(device.SshUsername))
			{
				pollOutcome = PollOutcome.Unreachable("No operator account configured.");
			}
			else
			{
				string text = this._creds.Unprotect(device.SshPasswordEnc);
				if (string.IsNullOrEmpty(text))
				{
					pollOutcome = PollOutcome.Unreachable("No operator password configured.");
				}
				else
				{
					int num = ((device.Port > 0) ? device.Port : 20000);
					Stopwatch sw = Stopwatch.StartNew();
					BbuMmlClient client = new BbuMmlClient(device.IpAddress, num, device.SshUsername, text);
					object obj = null;
					int num2 = 0;
					PollOutcome pollOutcome2;
					try
					{
						try
						{
							await client.ConnectAsync(ct);
							await client.LoginAsync(ct);
							DeviceSnapshot snapshot = new DeviceSnapshot();
							await this.WalkAsync(client, snapshot, ct);
							sw.Stop();
							pollOutcome2 = new PollOutcome
							{
								Reachable = true,
								LatencyMs = new int?((int)sw.ElapsedMilliseconds),
								Snapshot = snapshot
							};
						}
						catch (OperationCanceledException obj2) when (!ct.IsCancellationRequested)
						{
							pollOutcome2 = PollOutcome.Unreachable("Timed out talking to the BBU.");
						}
						catch (OperationCanceledException)
						{
							throw;
						}
						catch (Exception ex)
						{
							this._log.LogDebug(ex, "BBU poll failed for {Device}", new object[] { device.Name });
							pollOutcome2 = PollOutcome.Unreachable("BBU error: " + ex.Message);
						}
						num2 = 1;
					}
					catch (object obj)
					{
					}
					if (client != null)
					{
						await client.DisposeAsync();
					}
					object obj3 = obj;
					if (obj3 != null)
					{
						Exception ex2 = obj3 as Exception;
						if (ex2 == null)
						{
							throw obj3;
						}
						ExceptionDispatchInfo.Capture(ex2).Throw();
					}
					if (num2 != 1)
					{
						obj = null;
						pollOutcome2 = null;
						sw = null;
						client = null;
						throw null;
					}
					pollOutcome = pollOutcome2;
				}
			}
			return pollOutcome;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00014A54 File Offset: 0x00012C54
		private async Task WalkAsync(BbuMmlClient client, DeviceSnapshot snapshot, CancellationToken ct)
		{
			MmlResponse mmlResponse = await client.QueryAsync("LST SYSCFG", ct);
			BbuMmlCollector.AddScalar(snapshot, mmlResponse, "Manufacturer", "Manufacturer", "System");
			BbuMmlCollector.AddScalar(snapshot, mmlResponse, "ModelName", "Model", "System");
			BbuMmlCollector.AddScalar(snapshot, mmlResponse, "SerialNumber", "Serial number", "System");
			BbuMmlCollector.AddScalar(snapshot, mmlResponse, "SysName", "System name", "System");
			MmlResponse mmlResponse2 = await BbuMmlCollector.TryQueryAsync(client, "LST GNBConfig", ct);
			if (mmlResponse2 != null && mmlResponse2.Success)
			{
				BbuMmlCollector.AddScalar(snapshot, mmlResponse2, "gNBId", "gNB ID", "System");
				BbuMmlCollector.AddScalar(snapshot, mmlResponse2, "gNBName", "gNB name", "System");
			}
			List<Metric> list = snapshot.Metrics;
			MmlResponse mmlResponse3 = await BbuMmlCollector.TryQueryIndexedAsync(client, "LST SupportPLMNList", ct);
			list.AddRange(MobileNetworkFormat.Describe(mmlResponse3, await BbuMmlCollector.TryQueryIndexedAsync(client, "LST SliceList", ct)));
			list = null;
			mmlResponse3 = null;
			MmlResponse mmlResponse4 = await client.QueryAsync("DSP gNodeBBaseInfo", ct);
			foreach (KeyValuePair<string, string> keyValuePair in mmlResponse4.Scalars)
			{
				string text;
				string text2;
				keyValuePair.Deconstruct(out text, out text2);
				string text3 = text;
				string text4 = text2;
				if (!string.IsNullOrWhiteSpace(text4))
				{
					if (BbuMmlCollector.CpuUsageKey.IsMatch(text3))
					{
						string text5 = text3.Replace(" Usage", "", StringComparison.OrdinalIgnoreCase).Trim();
						snapshot.Metrics.Add(new Metric(text5, text4.Trim().TrimEnd('%').Trim(), "%", "CPU cores"));
					}
					else if (!text3.Equals("UpTime(second)", StringComparison.OrdinalIgnoreCase))
					{
						if (text3.Equals("CPUModelName", StringComparison.OrdinalIgnoreCase))
						{
							snapshot.Metrics.Add(new Metric("CPU model", text4, null, "System"));
						}
						else
						{
							snapshot.Metrics.Add(new Metric(BbuMmlCollector.Humanize(text3), text4, null, "System"));
						}
					}
				}
			}
			string text6;
			long num;
			if (mmlResponse4.Scalars.TryGetValue("UpTime(second)", out text6) && long.TryParse((text6 != null) ? text6.Trim() : null, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) && num > 0L)
			{
				snapshot.Metrics.Add(new Metric("Uptime", UptimeFormat.FromSeconds(num), null, "System"));
			}
			string text7;
			if (mmlResponse4.Scalars.TryGetValue("AMF1 State", out text7) && !string.IsNullOrWhiteSpace(text7))
			{
				snapshot.Metrics.Add(new Metric("5G core link", text7, null, "System"));
			}
			string text8;
			if (mmlResponse4.Scalars.TryGetValue("Online UE", out text8))
			{
				int? num2 = BbuMmlCollector.ParseOnlineUeTotal(text8);
				if (num2 != null)
				{
					int valueOrDefault = num2.GetValueOrDefault();
					snapshot.Metrics.Add(new Metric("Connected UEs", valueOrDefault.ToString(), null, "System"));
				}
			}
			MmlResponse mmlResponse5 = await BbuMmlCollector.TryQueryAsync(client, "LST FGAFINFO", ct);
			string clockMode;
			if (mmlResponse5 != null && mmlResponse5.Scalars.TryGetValue("CurrentClockSynMode", out clockMode) && !string.IsNullOrWhiteSpace(clockMode))
			{
				snapshot.Metrics.Add(new Metric("Clock sync", clockMode, null, "System"));
			}
			string clockLock;
			if (mmlResponse5 != null && mmlResponse5.Scalars.TryGetValue("ClockSyncStatus", out clockLock) && !string.IsNullOrWhiteSpace(clockLock))
			{
				snapshot.Metrics.Add(new Metric("Clock lock", clockLock, null, "System"));
			}
			List<string> list2 = (from s in (await client.QueryAsync("LST OptPortMapping", ct)).Scalars
				where s.Key.StartsWith("OPT", StringComparison.OrdinalIgnoreCase)
				select s.Value).ToList<string>();
			bool flag = list2.Any<string>((string t) => t.Equals("AU", StringComparison.OrdinalIgnoreCase));
			bool flag2 = list2.Any<string>((string t) => t.Equals("EU", StringComparison.OrdinalIgnoreCase));
			if (flag)
			{
				foreach (Dictionary<string, string> dictionary in (await client.QueryAsync("DSP AU300TOPO", ct)).Rows)
				{
					ChildStatus childStatus = BbuMmlCollector.ToNode(dictionary, "au");
					snapshot.Children.Add(childStatus);
					string auIndex = BbuMmlCollector.Cell(dictionary, "DeviceIndex");
					if (!string.IsNullOrEmpty(auIndex))
					{
						await BbuMmlCollector.AddDetailAsync(client, childStatus, "DSP AU300OptModuleInfo:indexid_AU300=" + auIndex + ";", "Optical", ct, null);
						foreach (Dictionary<string, string> dictionary2 in (await client.QueryAsync("DSP EU300TOPO:indexid_AU300=" + auIndex + ";", ct)).Rows)
						{
							ChildStatus euNode = BbuMmlCollector.ToNode(dictionary2, "eu");
							snapshot.Children.Add(euNode);
							string euIndex = BbuMmlCollector.Cell(dictionary2, "DeviceIndex");
							if (!string.IsNullOrEmpty(euIndex))
							{
								ChildStatus childStatus2 = euNode;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
								defaultInterpolatedStringHandler.AppendLiteral("DSP EU300OptModuleInfo:indexid_AU300=");
								defaultInterpolatedStringHandler.AppendFormatted(auIndex);
								defaultInterpolatedStringHandler.AppendLiteral(",indexid_EU300=");
								defaultInterpolatedStringHandler.AppendFormatted(euIndex);
								defaultInterpolatedStringHandler.AppendLiteral(";");
								await BbuMmlCollector.AddDetailAsync(client, childStatus2, defaultInterpolatedStringHandler.ToStringAndClear(), "Optical", ct, null);
								ChildStatus childStatus3 = euNode;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(49, 2);
								defaultInterpolatedStringHandler2.AppendLiteral("DSP EU300AlarmInfo:indexid_AU300=");
								defaultInterpolatedStringHandler2.AppendFormatted(auIndex);
								defaultInterpolatedStringHandler2.AppendLiteral(",indexid_EU300=");
								defaultInterpolatedStringHandler2.AppendFormatted(euIndex);
								defaultInterpolatedStringHandler2.AppendLiteral(";");
								await BbuMmlCollector.AddDetailAsync(client, childStatus3, defaultInterpolatedStringHandler2.ToStringAndClear(), "Port alarms", ct, null);
							}
							euNode = null;
							euIndex = null;
						}
						List<Dictionary<string, string>>.Enumerator enumerator3 = default(List<Dictionary<string, string>>.Enumerator);
						foreach (Dictionary<string, string> dictionary3 in (await client.QueryAsync("DSP RU8250TOPO:indexid_AU300=" + auIndex + ";", ct)).Rows)
						{
							ChildStatus euNode = BbuMmlCollector.ToNode(dictionary3, "ru");
							snapshot.Children.Add(euNode);
							string euIndex = BbuMmlCollector.Cell(dictionary3, "DeviceIndex");
							if (!string.IsNullOrEmpty(euIndex))
							{
								ChildStatus childStatus4 = euNode;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(49, 2);
								defaultInterpolatedStringHandler3.AppendLiteral("DSP RU8250SysInfo:indexid_AU300=");
								defaultInterpolatedStringHandler3.AppendFormatted(auIndex);
								defaultInterpolatedStringHandler3.AppendLiteral(",indexid_RU8250=");
								defaultInterpolatedStringHandler3.AppendFormatted(euIndex);
								defaultInterpolatedStringHandler3.AppendLiteral(";");
								await BbuMmlCollector.AddDetailAsync(client, childStatus4, defaultInterpolatedStringHandler3.ToStringAndClear(), "Power", ct, null);
								ChildStatus childStatus5 = euNode;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(53, 2);
								defaultInterpolatedStringHandler4.AppendLiteral("DSP RU8250OpticalInfo:indexid_AU300=");
								defaultInterpolatedStringHandler4.AppendFormatted(auIndex);
								defaultInterpolatedStringHandler4.AppendLiteral(",indexid_RU8250=");
								defaultInterpolatedStringHandler4.AppendFormatted(euIndex);
								defaultInterpolatedStringHandler4.AppendLiteral(";");
								await BbuMmlCollector.AddDetailAsync(client, childStatus5, defaultInterpolatedStringHandler4.ToStringAndClear(), "Optical", ct, null);
							}
							euNode = null;
							euIndex = null;
						}
						enumerator3 = default(List<Dictionary<string, string>>.Enumerator);
						auIndex = null;
					}
				}
				List<Dictionary<string, string>>.Enumerator enumerator2 = default(List<Dictionary<string, string>>.Enumerator);
			}
			else
			{
				if (flag2)
				{
					using (List<Dictionary<string, string>>.Enumerator enumerator4 = (await client.QueryAsync("DSP EUTOPO", ct)).Rows.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							Dictionary<string, string> dictionary4 = enumerator4.Current;
							snapshot.Children.Add(BbuMmlCollector.ToNode(dictionary4, "eu"));
						}
						goto IL_10AF;
					}
				}
				foreach (Dictionary<string, string> dictionary5 in (await client.QueryAsync("DSP RRUTOPO", ct)).Rows)
				{
					snapshot.Children.Add(BbuMmlCollector.ToNode(dictionary5, "ru"));
				}
			}
			IL_10AF:
			MmlResponse cells = await client.QueryAsync("LST CellConfig", ct);
			MmlResponse opState = await BbuMmlCollector.TryQueryAsync(client, "LST OpState", ct);
			int? num3 = BbuMmlCollector.ActiveCellCount(await BbuMmlCollector.TryQueryAsync(client, "LST ActiveCell", ct));
			if (num3 != null)
			{
				int n = num3.GetValueOrDefault();
				snapshot.Metrics.Add(new Metric("Active cells", n.ToString(CultureInfo.InvariantCulture), null, "System"));
				cells.Rows.RemoveAll(delegate(Dictionary<string, string> r)
				{
					int num5;
					return int.TryParse(BbuMmlCollector.Cell(r, "CellIndex"), out num5) && num5 > n;
				});
			}
			List<string> cellIndices = (from r in cells.Rows
				select BbuMmlCollector.Cell(r, "CellIndex") into i
				where i.Length > 0
				select i).ToList<string>();
			Dictionary<string, Dictionary<string, string>> tdd = await this.ReadPerCellAsync(client, "LST PHYTddULDL", cellIndices, ct);
			Dictionary<string, Dictionary<string, string>> dictionary6 = await this.ReadPerCellAsync(client, "LST MaxTxPower", cellIndices, ct);
			foreach (Dictionary<string, string> dictionary7 in cells.Rows)
			{
				snapshot.Children.Add(BbuMmlCollector.ToCell(dictionary7, opState, tdd, dictionary6));
			}
			foreach (Dictionary<string, string> dictionary8 in (await client.QueryAsync("LST ActAlarm", ct)).Rows)
			{
				snapshot.Alarms.Add(new AlarmStatus
				{
					Name = BbuMmlCollector.Cell(dictionary8, "AlarmName"),
					Severity = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(dictionary8, "AlarmSeverity")),
					Description = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(dictionary8, "AlarmDesc")),
					RaisedAt = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(dictionary8, "AlarmRaisedTime")),
					Id = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(dictionary8, "AlarmID"))
				});
			}
			MmlResponse mmlResponse6 = await BbuMmlCollector.TryQueryMultiAsync(client, "DSP PmInstaneousResult", ct);
			if (mmlResponse6 != null && mmlResponse6.Success)
			{
				foreach (Dictionary<string, string> dictionary9 in mmlResponse6.Rows)
				{
					string text9 = BbuMmlCollector.Cell(dictionary9, "CountName");
					string text10 = BbuMmlCollector.Cell(dictionary9, "Value");
					if (text9.Length != 0 && text10.Length != 0 && !text9.StartsWith("PHY.ULInterference.PRB", StringComparison.OrdinalIgnoreCase))
					{
						snapshot.Metrics.Add(new Metric(text9, text10, null, "Performance"));
						double num4;
						if (text9.Equals("gNB.CPUAvg", StringComparison.OrdinalIgnoreCase) && double.TryParse(text10, NumberStyles.Any, CultureInfo.InvariantCulture, out num4))
						{
							snapshot.Metrics.Add(new Metric("CPU load", num4.ToString("0.#", CultureInfo.InvariantCulture), "%", "Resources"));
						}
					}
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00014AB0 File Offset: 0x00012CB0
		private static ChildStatus ToNode(Dictionary<string, string> row, string kind)
		{
			string text = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(row, "SerialNumber"));
			string text2 = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(row, "DevType"));
			string text3 = BbuMmlCollector.Cell(row, "Status");
			ChildStatus childStatus = new ChildStatus
			{
				Identifier = (text ?? (kind + "-" + BbuMmlCollector.Cell(row, "DeviceIndex"))),
				Kind = kind,
				SerialNumber = text,
				Model = text2,
				Version = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(row, "SoftwareVersion")),
				RouteAddr = BbuMmlCollector.NullIfEmpty(BbuMmlCollector.Cell(row, "DevRouteAddr")),
				Up = BbuMmlCollector.NodeUp(text3),
				State = BbuMmlCollector.NullIfEmpty(text3)
			};
			BbuMmlCollector.AddMetric(childStatus, "Index", BbuMmlCollector.Cell(row, "DeviceIndex"), "System", null);
			BbuMmlCollector.AddMetric(childStatus, "Sub-id", BbuMmlCollector.Cell(row, "DeviceSubId"), "System", null);
			BbuMmlCollector.AddMetric(childStatus, "Model name", BbuMmlCollector.Cell(row, "ModelName"), "System", null);
			BbuMmlCollector.AddMetric(childStatus, "Software", childStatus.Version, "System", null);
			return childStatus;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00014BD8 File Offset: 0x00012DD8
		private static ChildStatus ToCell(Dictionary<string, string> row, [Nullable(2)] MmlResponse opState, [Nullable(new byte[] { 2, 1, 1, 1, 1 })] IReadOnlyDictionary<string, Dictionary<string, string>> tdd = null, [Nullable(new byte[] { 2, 1, 1, 1, 1 })] IReadOnlyDictionary<string, Dictionary<string, string>> txPower = null)
		{
			string idx = BbuMmlCollector.Cell(row, "CellIndex");
			ChildStatus childStatus = new ChildStatus
			{
				Identifier = "Cell " + idx,
				Kind = "cell",
				Model = "NR Cell"
			};
			BbuMmlCollector.AddMetric(childStatus, "PCI", BbuMmlCollector.Cell(row, "PhyCellID"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "Band", BbuMmlCollector.Cell(row, "FreqBandIndicator"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "DL bandwidth", BbuMmlCollector.BandwidthMhz(BbuMmlCollector.Cell(row, "DLBandwidth(MHz)")), null, "MHz");
			BbuMmlCollector.AddMetric(childStatus, "UL bandwidth", BbuMmlCollector.BandwidthMhz(BbuMmlCollector.Cell(row, "ULBandwidth(MHz)")), null, "MHz");
			BbuMmlCollector.AddMetric(childStatus, "DL QAM", BbuMmlCollector.Cell(row, "PdschMcsTable"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "UL QAM", BbuMmlCollector.Cell(row, "PuschMcsTable"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "DL ARFCN", BbuMmlCollector.Cell(row, "NRARFCNDL"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "UL ARFCN", BbuMmlCollector.Cell(row, "NRARFCNUL"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "SSB ARFCN", BbuMmlCollector.Cell(row, "AbsoluteFrequencySSB"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "DL frequency", BbuMmlCollector.ArfcnMhz(BbuMmlCollector.Cell(row, "NRARFCNDL")), null, "MHz");
			BbuMmlCollector.AddMetric(childStatus, "UL frequency", BbuMmlCollector.ArfcnMhz(BbuMmlCollector.Cell(row, "NRARFCNUL")), null, "MHz");
			BbuMmlCollector.AddMetric(childStatus, "SSB frequency", BbuMmlCollector.ArfcnMhz(BbuMmlCollector.Cell(row, "AbsoluteFrequencySSB")), null, "MHz");
			BbuMmlCollector.AddMetric(childStatus, "Protocol", BbuMmlCollector.Cell(row, "ProtocolType"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "DL layers", BbuMmlCollector.Cell(row, "DlLayers"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "DL antennas", BbuMmlCollector.Cell(row, "DlAntennas"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "UL layers", BbuMmlCollector.Cell(row, "UlLayers"), null, null);
			BbuMmlCollector.AddMetric(childStatus, "UL antennas", BbuMmlCollector.Cell(row, "UlAntennas"), null, null);
			Dictionary<string, string> dictionary = ((opState != null) ? opState.Rows.FirstOrDefault<Dictionary<string, string>>((Dictionary<string, string> r) => string.Equals(BbuMmlCollector.Cell(r, "CellIndex"), idx, StringComparison.OrdinalIgnoreCase)) : null);
			if (dictionary != null)
			{
				string text = BbuMmlCollector.Cell(dictionary, "OpState");
				string text2 = BbuMmlCollector.Cell(dictionary, "CellSubState");
				BbuMmlCollector.AddMetric(childStatus, "Op state", text, null, null);
				BbuMmlCollector.AddMetric(childStatus, "Sub state", text2, null, null);
				BbuMmlCollector.AddMetric(childStatus, "Admin state", BbuMmlCollector.Cell(dictionary, "AdminState"), null, null);
				BbuMmlCollector.AddMetric(childStatus, "Cell barred", BbuMmlCollector.Cell(dictionary, "CellBarred"), null, null);
				if (text.Length > 0)
				{
					bool flag = BbuMmlCollector.CellOnAir(text);
					childStatus.Up = new bool?(flag);
					childStatus.State = (flag ? "On air" : "Off air");
				}
			}
			Dictionary<string, string> dictionary2;
			if (tdd != null && tdd.TryGetValue(idx, out dictionary2))
			{
				string text3 = BbuMmlCollector.Cell(dictionary2, "SubframeAssignmentType");
				BbuMmlCollector.AddMetric(childStatus, "TDD pattern", BbuCellSettings.ParseTddPattern(text3) ?? text3, null, null);
				BbuMmlCollector.AddMetric(childStatus, "Special slot", BbuMmlCollector.Cell(dictionary2, "SpecialSubframePatterns"), null, null);
			}
			Dictionary<string, string> dictionary3;
			if (txPower != null && txPower.TryGetValue(idx, out dictionary3))
			{
				BbuMmlCollector.AddMetric(childStatus, "Max TX power", BbuMmlCollector.Cell(dictionary3, "MaxTxPower"), null, "dBm");
			}
			return childStatus;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00014F38 File Offset: 0x00013138
		[NullableContext(2)]
		public static bool CellOnAir(string opState)
		{
			return string.Equals((opState != null) ? opState.Trim() : null, "normal", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00014F54 File Offset: 0x00013154
		[NullableContext(2)]
		public static bool? NodeUp(string status)
		{
			string text = (status ?? "").Trim().ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 0:
					return null;
				case 1:
				case 3:
				case 9:
				case 11:
					goto IL_0259;
				case 2:
				{
					char c = text[0];
					if (c != 'o')
					{
						if (c != 'u')
						{
							goto IL_0259;
						}
						if (!(text == "up"))
						{
							goto IL_0259;
						}
					}
					else if (!(text == "ok"))
					{
						goto IL_0259;
					}
					break;
				}
				case 4:
				{
					char c = text[0];
					if (c != 'd')
					{
						if (c != 'f')
						{
							goto IL_0259;
						}
						if (!(text == "fail"))
						{
							goto IL_0259;
						}
						goto IL_0250;
					}
					else
					{
						if (!(text == "down"))
						{
							goto IL_0259;
						}
						goto IL_0250;
					}
					break;
				}
				case 5:
				{
					char c = text[0];
					if (c != 'a')
					{
						if (c != 'f')
						{
							goto IL_0259;
						}
						if (!(text == "fault"))
						{
							goto IL_0259;
						}
					}
					else if (!(text == "alarm"))
					{
						goto IL_0259;
					}
					break;
				}
				case 6:
				{
					char c = text[0];
					if (c <= 'f')
					{
						if (c != 'a')
						{
							if (c != 'f')
							{
								goto IL_0259;
							}
							if (!(text == "failed"))
							{
								goto IL_0259;
							}
							goto IL_0250;
						}
						else if (!(text == "active"))
						{
							goto IL_0259;
						}
					}
					else if (c != 'n')
					{
						if (c != 'o')
						{
							goto IL_0259;
						}
						if (!(text == "online"))
						{
							goto IL_0259;
						}
					}
					else if (!(text == "normal"))
					{
						goto IL_0259;
					}
					break;
				}
				case 7:
				{
					char c = text[0];
					if (c != 'o')
					{
						if (c != 'r')
						{
							if (c != 'w')
							{
								goto IL_0259;
							}
							if (!(text == "warning"))
							{
								goto IL_0259;
							}
						}
						else if (!(text == "running"))
						{
							goto IL_0259;
						}
					}
					else
					{
						if (!(text == "offline"))
						{
							goto IL_0259;
						}
						goto IL_0250;
					}
					break;
				}
				case 8:
					if (!(text == "degraded"))
					{
						goto IL_0259;
					}
					break;
				case 10:
					if (!(text == "disconnect"))
					{
						goto IL_0259;
					}
					goto IL_0250;
				case 12:
					if (!(text == "disconnected"))
					{
						goto IL_0259;
					}
					goto IL_0250;
				default:
					goto IL_0259;
				}
				return new bool?(true);
				IL_0250:
				return new bool?(false);
			}
			IL_0259:
			return null;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000151C8 File Offset: 0x000133C8
		[NullableContext(2)]
		public static int? ParseOnlineUeTotal(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			int num = 0;
			bool flag = false;
			foreach (object obj in Regex.Matches(value, ":\\s*(\\d+)"))
			{
				Match match = (Match)obj;
				num += int.Parse(match.Groups[1].Value);
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00015268 File Offset: 0x00013468
		[return: Nullable(new byte[] { 1, 2 })]
		private static async Task<MmlResponse> TryQueryAsync(BbuMmlClient client, string cmd, CancellationToken ct)
		{
			MmlResponse mmlResponse;
			try
			{
				mmlResponse = await client.QueryAsync(cmd, ct);
			}
			catch (OperationCanceledException obj) when (ct.IsCancellationRequested)
			{
				throw;
			}
			catch
			{
				mmlResponse = null;
			}
			return mmlResponse;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000152BC File Offset: 0x000134BC
		private async Task<Dictionary<string, Dictionary<string, string>>> ReadPerCellAsync(BbuMmlClient client, string command, IReadOnlyList<string> cellIndices, CancellationToken ct)
		{
			Dictionary<string, Dictionary<string, string>> byCell = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
			MmlResponse mmlResponse = await BbuMmlCollector.TryQueryAsync(client, command, ct);
			MmlResponse bare = mmlResponse;
			if (bare != null && bare.Success)
			{
				foreach (Dictionary<string, string> dictionary in bare.Rows)
				{
					string text = BbuMmlCollector.Cell(dictionary, "CellIndex");
					if (text.Length > 0)
					{
						byCell[text] = dictionary;
					}
				}
			}
			Dictionary<string, Dictionary<string, string>> dictionary2;
			if (byCell.Count > 0)
			{
				dictionary2 = byCell;
			}
			else
			{
				foreach (string idx in cellIndices)
				{
					MmlResponse mmlResponse2 = await BbuMmlCollector.TryQueryAsync(client, command + ":indexid_CellConfig=" + idx + ";", ct);
					if (mmlResponse2 != null && mmlResponse2.Success)
					{
						if (mmlResponse2.Rows.Count > 0)
						{
							byCell[idx] = mmlResponse2.Rows[0];
						}
						else if (mmlResponse2.Scalars.Count > 0)
						{
							byCell[idx] = new Dictionary<string, string>(mmlResponse2.Scalars, StringComparer.OrdinalIgnoreCase);
						}
						idx = null;
					}
				}
				IEnumerator<string> enumerator2 = null;
				if (byCell.Count == 0)
				{
					ILogger log = this._log;
					string text2 = "{Command} returned nothing usable (bare RETCODE {Code} {Message}); the settings form will show these fields as not read rather than guessing.";
					object[] array = new object[3];
					array[0] = command;
					int num = 1;
					MmlResponse mmlResponse3 = bare;
					array[num] = ((mmlResponse3 != null) ? new int?(mmlResponse3.RetCode) : null);
					int num2 = 2;
					MmlResponse mmlResponse4 = bare;
					array[num2] = ((mmlResponse4 != null) ? mmlResponse4.Message : null);
					log.LogWarning(text2, array);
				}
				dictionary2 = byCell;
			}
			return dictionary2;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00015320 File Offset: 0x00013520
		[return: Nullable(new byte[] { 1, 2 })]
		private static async Task<MmlResponse> TryQueryIndexedAsync(BbuMmlClient client, string cmd, CancellationToken ct)
		{
			MmlResponse mmlResponse = await BbuMmlCollector.TryQueryAsync(client, cmd, ct);
			MmlResponse mmlResponse2;
			if (mmlResponse != null && mmlResponse.Success)
			{
				mmlResponse2 = mmlResponse;
			}
			else
			{
				mmlResponse2 = await BbuMmlCollector.TryQueryAsync(client, cmd + ":indexid_AMFPoolConfigParam=1;", ct);
			}
			return mmlResponse2;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00015374 File Offset: 0x00013574
		[return: Nullable(new byte[] { 1, 2 })]
		private static async Task<MmlResponse> TryQueryMultiAsync(BbuMmlClient client, string cmd, CancellationToken ct)
		{
			MmlResponse mmlResponse;
			try
			{
				mmlResponse = await client.QueryMultiAsync(cmd, ct);
			}
			catch (OperationCanceledException obj) when (ct.IsCancellationRequested)
			{
				throw;
			}
			catch
			{
				mmlResponse = null;
			}
			return mmlResponse;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000153C8 File Offset: 0x000135C8
		[NullableContext(2)]
		public static int? ActiveCellCount(MmlResponse r)
		{
			if (r == null || !r.Success)
			{
				return null;
			}
			foreach (KeyValuePair<string, string> keyValuePair in r.Scalars)
			{
				string text;
				string text2;
				keyValuePair.Deconstruct(out text, out text2);
				string text3 = text;
				string text4 = text2;
				int num;
				if (text3.Contains("Active", StringComparison.OrdinalIgnoreCase) && int.TryParse(text4.Trim(), out num) && num >= 0)
				{
					return new int?(num);
				}
			}
			if (r.Rows.Count == 1)
			{
				foreach (string text5 in r.Columns)
				{
					string text6;
					int num2;
					if (text5.Contains("Active", StringComparison.OrdinalIgnoreCase) && r.Rows[0].TryGetValue(text5, out text6) && int.TryParse(text6.Trim(), out num2) && num2 >= 0)
					{
						return new int?(num2);
					}
				}
			}
			return null;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00015504 File Offset: 0x00013704
		private static async Task AddDetailAsync(BbuMmlClient client, ChildStatus node, string cmd, string group, CancellationToken ct, [Nullable(new byte[] { 2, 1 })] Func<string, bool> keep = null)
		{
			MmlResponse mmlResponse = await BbuMmlCollector.TryQueryAsync(client, cmd, ct);
			if (mmlResponse != null && mmlResponse.Success)
			{
				foreach (KeyValuePair<string, string> keyValuePair in mmlResponse.Scalars)
				{
					string text;
					string text2;
					keyValuePair.Deconstruct(out text, out text2);
					string text3 = text;
					string text4 = text2;
					if (!string.IsNullOrWhiteSpace(text4) && (keep == null || keep(text3)))
					{
						node.Metrics.Add(new Metric(text3, text4, null, group));
					}
				}
				for (int i = 0; i < mmlResponse.Rows.Count; i++)
				{
					foreach (string text5 in mmlResponse.Columns)
					{
						string text6;
						if (mmlResponse.Rows[i].TryGetValue(text5, out text6) && !string.IsNullOrWhiteSpace(text6))
						{
							List<Metric> metrics = node.Metrics;
							string text7;
							if (mmlResponse.Rows.Count <= 1)
							{
								text7 = text5;
							}
							else
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
								defaultInterpolatedStringHandler.AppendFormatted(text5);
								defaultInterpolatedStringHandler.AppendLiteral(" ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
								text7 = defaultInterpolatedStringHandler.ToStringAndClear();
							}
							metrics.Add(new Metric(text7, text6, null, group));
						}
					}
				}
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00015574 File Offset: 0x00013774
		public static string Cell(Dictionary<string, string> row, string col)
		{
			string text;
			if (row.TryGetValue(col, out text))
			{
				return text;
			}
			foreach (KeyValuePair<string, string> keyValuePair in row)
			{
				if (BbuMmlCollector.ColumnBaseName(keyValuePair.Key).Equals(col, StringComparison.OrdinalIgnoreCase))
				{
					return keyValuePair.Value;
				}
			}
			return "";
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000155F0 File Offset: 0x000137F0
		public static string ColumnBaseName(string column)
		{
			int num = column.IndexOf('(');
			return ((num < 0) ? column : column.Substring(0, num)).Trim();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001561A File Offset: 0x0001381A
		[NullableContext(2)]
		private static string NullIfEmpty(string v)
		{
			if (!string.IsNullOrWhiteSpace(v))
			{
				return v;
			}
			return null;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00015628 File Offset: 0x00013828
		private static void AddScalar(DeviceSnapshot s, MmlResponse r, string key, string label, string group)
		{
			string text;
			if (r.Scalars.TryGetValue(key, out text) && !string.IsNullOrWhiteSpace(text))
			{
				s.Metrics.Add(new Metric(label, text, null, group));
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00015662 File Offset: 0x00013862
		[NullableContext(2)]
		private static void AddMetric([Nullable(1)] ChildStatus node, [Nullable(1)] string label, string value, string group = null, string unit = null)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				node.Metrics.Add(new Metric(label, value, unit, group));
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00015684 File Offset: 0x00013884
		[NullableContext(2)]
		public static string BandwidthMhz(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}
			string text = value.Trim();
			if (!text.EndsWith("MHz", StringComparison.OrdinalIgnoreCase))
			{
				return text;
			}
			string text2 = text;
			return text2.Substring(0, text2.Length - 3).Trim();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000156C8 File Offset: 0x000138C8
		[NullableContext(2)]
		public static string ArfcnMhz(string arfcn)
		{
			int num;
			if (!int.TryParse((arfcn != null) ? arfcn.Trim() : null, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) || num <= 0)
			{
				return null;
			}
			return BbuCellSettings.ArfcnToMhz(num).ToString("0.###", CultureInfo.InvariantCulture);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001570E File Offset: 0x0001390E
		private static string Humanize(string key)
		{
			return key;
		}

		// Token: 0x040001E3 RID: 483
		private readonly CredentialProtector _creds;

		// Token: 0x040001E4 RID: 484
		private readonly ILogger<BbuMmlCollector> _log;

		// Token: 0x040001E5 RID: 485
		private static readonly Regex CpuUsageKey = new Regex("^CPU\\d+\\s*Usage$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
