using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Integrity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Security;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoreLivePollService : BackgroundService
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000F673 File Offset: 0x0000D873
		public CoreLivePollService(IServiceScopeFactory scopeFactory, IOptions<PollingOptions> options, CredentialProtector creds, CoreLiveState state, ILogger<CoreLivePollService> log)
		{
			this._scopeFactory = scopeFactory;
			this._options = options.Value;
			this._creds = creds;
			this._state = state;
			this._log = log;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (!this._options.Enabled || !this._options.CoreLiveEnabled)
			{
				this._log.LogInformation("Live core polling is disabled by configuration.", Array.Empty<object>());
			}
			else
			{
				TimeSpan interval = TimeSpan.FromSeconds((long)Math.Max(2, this._options.CoreLiveIntervalSeconds));
				this._log.LogInformation("Live core polling started (every {Interval}s, budget {Budget}s).", new object[]
				{
					interval.TotalSeconds,
					this._options.CoreLiveBudgetSeconds
				});
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(10L), stoppingToken);
					goto IL_027D;
				}
				catch (OperationCanceledException)
				{
					return;
				}
				IL_0124:
				Stopwatch started = Stopwatch.StartNew();
				try
				{
					await this.TickAsync(stoppingToken);
				}
				catch (OperationCanceledException obj) when (stoppingToken.IsCancellationRequested)
				{
					goto IL_028D;
				}
				catch (Exception ex)
				{
					this._log.LogError(ex, "Live core poll tick failed.", Array.Empty<object>());
				}
				TimeSpan timeSpan = interval - started.Elapsed;
				if (timeSpan < TimeSpan.FromMilliseconds(500L))
				{
					timeSpan = TimeSpan.FromMilliseconds(500L);
				}
				try
				{
					await Task.Delay(timeSpan, stoppingToken);
				}
				catch (OperationCanceledException)
				{
					goto IL_028D;
				}
				started = null;
				IL_027D:
				if (!stoppingToken.IsCancellationRequested)
				{
					goto IL_0124;
				}
				IL_028D:
				foreach (CoreLivePollService.CoreSession coreSession in this._sessions.Values)
				{
					coreSession.Dispose();
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		private async Task TickAsync(CancellationToken ct)
		{
			CoreLivePollService.<>c__DisplayClass8_0 CS$<>8__locals1 = new CoreLivePollService.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ct = ct;
			using (IServiceScope scope = this._scopeFactory.CreateScope())
			{
				ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				if (!scope.ServiceProvider.GetRequiredService<IntegrityGate>().IsBlocked)
				{
					IQueryable<Device> devices = db.Devices;
					ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable = devices.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable2 = queryable.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					List<Device> list = await queryable2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.AndAlso(Expression.Property(parameterExpression, methodof(Device.get_Enabled())), Expression.Equal(Expression.Convert(Expression.Property(parameterExpression, methodof(Device.get_CollectionMethod())), typeof(int)), Expression.Constant(1, typeof(int)))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("mobile", typeof(string)))), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(CS$<>8__locals1.ct);
					CS$<>8__locals1.cores = list;
					IEnumerable<int> keys = this._sessions.Keys;
					Func<int, bool> func;
					if ((func = CS$<>8__locals1.<>9__3) == null)
					{
						CoreLivePollService.<>c__DisplayClass8_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<int, bool> func2 = (int id) => CS$<>8__locals1.cores.All<Device>((Device c) => c.Id != id);
						CS$<>8__locals2.<>9__3 = func2;
						func = func2;
					}
					foreach (int num in keys.Where<int>(func).ToList<int>())
					{
						this._sessions[num].Dispose();
						this._sessions.Remove(num);
						this._state.Drop(num);
					}
					CS$<>8__locals1.budget = TimeSpan.FromSeconds((long)Math.Max(1, this._options.CoreLiveBudgetSeconds));
					CS$<>8__locals1.reconnect = TimeSpan.FromSeconds((long)Math.Max(5, this._options.CoreLiveReconnectSeconds));
					using (List<Device>.Enumerator enumerator2 = CS$<>8__locals1.cores.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CoreLivePollService.<>c__DisplayClass8_2 CS$<>8__locals3 = new CoreLivePollService.<>c__DisplayClass8_2();
							CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals3.core = enumerator2.Current;
							CoreLivePollService.<>c__DisplayClass8_2 CS$<>8__locals4 = CS$<>8__locals3;
							DeviceTemplate template = CS$<>8__locals3.core.Template;
							CS$<>8__locals4.def = TemplateDefinition.Parse((template != null) ? template.DefinitionJson : null).Ssh;
							if (CS$<>8__locals3.def != null && CS$<>8__locals3.def.Interactive)
							{
								if (!this._sessions.TryGetValue(CS$<>8__locals3.core.Id, out CS$<>8__locals3.session))
								{
									Dictionary<int, CoreLivePollService.CoreSession> sessions = this._sessions;
									int id2 = CS$<>8__locals3.core.Id;
									CoreLivePollService.<>c__DisplayClass8_2 CS$<>8__locals5 = CS$<>8__locals3;
									CoreLivePollService.CoreSession coreSession = new CoreLivePollService.CoreSession(CS$<>8__locals3.core.Id, this._log);
									CS$<>8__locals5.session = coreSession;
									sessions[id2] = coreSession;
								}
								CS$<>8__locals3.password = this._creds.Unprotect(CS$<>8__locals3.core.SshPasswordEnc);
								if (!string.IsNullOrWhiteSpace(CS$<>8__locals3.core.IpAddress) && !string.IsNullOrWhiteSpace(CS$<>8__locals3.core.SshUsername) && !string.IsNullOrEmpty(CS$<>8__locals3.password))
								{
									CS$<>8__locals3.pingEvery = TimeSpan.FromSeconds((long)Math.Max(0, this._options.CoreLivePingSeconds));
									CoreLiveSnapshot coreLiveSnapshot = await Task.Run<CoreLiveSnapshot>(() => CS$<>8__locals3.session.Sample(CS$<>8__locals3.core, CS$<>8__locals3.def, CS$<>8__locals3.password, CS$<>8__locals3.CS$<>8__locals1.budget, CS$<>8__locals3.CS$<>8__locals1.reconnect, CS$<>8__locals3.pingEvery, CS$<>8__locals3.CS$<>8__locals1.<>4__this._state, CS$<>8__locals3.CS$<>8__locals1.ct), CS$<>8__locals3.CS$<>8__locals1.ct);
									if (coreLiveSnapshot != null)
									{
										this._state.Publish(coreLiveSnapshot);
										await CoreLivePollService.PersistAsync(db, CS$<>8__locals3.core, coreLiveSnapshot, CS$<>8__locals3.CS$<>8__locals1.ct);
										CS$<>8__locals3 = null;
									}
								}
							}
						}
					}
					List<Device>.Enumerator enumerator2 = default(List<Device>.Enumerator);
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000F748 File Offset: 0x0000D948
		private static async Task PersistAsync(ApplicationDbContext db, Device core, CoreLiveSnapshot sample, CancellationToken ct)
		{
			CoreLivePollService.<>c__DisplayClass9_0 CS$<>8__locals1 = new CoreLivePollService.<>c__DisplayClass9_0();
			CS$<>8__locals1.core = core;
			DateTime now = sample.AtUtc;
			await db.Entry<Device>(CS$<>8__locals1.core).ReloadAsync(ct);
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			foreach (Device device in await queryable.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.AndAlso(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_ParentDeviceId())), Expression.Convert(Expression.Property(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(CoreLivePollService.<>c__DisplayClass9_0)), fieldof(CoreLivePollService.<>c__DisplayClass9_0.core)), methodof(Device.get_Id())), typeof(int?))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("ue", typeof(string)))), Expression.Property(parameterExpression, methodof(Device.get_Enabled()))), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(ct))
			{
				string childIdentifier = device.ChildIdentifier;
				string text = ((childIdentifier != null) ? childIdentifier.Trim() : null);
				if (!string.IsNullOrEmpty(text))
				{
					UeLive ueLive;
					if (sample.Ues.TryGetValue(text, out ueLive) && ueLive.State != "Disconnected")
					{
						DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(device.LastSnapshotJson) ?? new DeviceSnapshot();
						Metric metric = deviceSnapshot.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "State");
						string text2 = ((metric != null) ? metric.Value : null);
						bool? lastReachable = device.LastReachable;
						if (ueLive.State != null)
						{
							CoreLivePollService.SetMetric(deviceSnapshot, "State", ueLive.State, null, "Status", true);
							CoreLivePollService.SetMetric(deviceSnapshot, "Connection", ueLive.State, null, "UE", false);
						}
						CoreLivePollService.SetMetric(deviceSnapshot, "DL", CoreLivePollService.Fmt(ueLive.DlMbps), "Mbps", "UE", false);
						CoreLivePollService.SetMetric(deviceSnapshot, "UL", CoreLivePollService.Fmt(ueLive.UlMbps), "Mbps", "UE", false);
						if (!string.IsNullOrEmpty(ueLive.Ip))
						{
							CoreLivePollService.SetMetric(deviceSnapshot, "IP", ueLive.Ip, null, "UE", false);
						}
						device.LastReachable = new bool?(true);
						device.LastError = null;
						device.LastSnapshotJson = SnapshotSerializer.Serialize(deviceSnapshot);
						bool flag = ueLive.State != null && !string.Equals(text2, ueLive.State, StringComparison.OrdinalIgnoreCase);
						if (!lastReachable.GetValueOrDefault() || flag)
						{
							db.DevicePollResults.Add(new DevicePollResult
							{
								DeviceId = device.Id,
								Timestamp = now,
								Reachable = true,
								SnapshotJson = device.LastSnapshotJson
							});
						}
					}
					else
					{
						bool? lastReachable2 = device.LastReachable;
						bool flag2 = false;
						if (!((lastReachable2.GetValueOrDefault() == flag2) & (lastReachable2 != null)))
						{
							DeviceSnapshot deviceSnapshot2 = SnapshotSerializer.Deserialize(device.LastSnapshotJson) ?? new DeviceSnapshot();
							CoreLivePollService.SetMetric(deviceSnapshot2, "State", "Disconnected", null, "Status", true);
							CoreLivePollService.SetMetric(deviceSnapshot2, "Connection", "Disconnected", null, "UE", false);
							CoreLivePollService.SetMetric(deviceSnapshot2, "DL", "0", "Mbps", "UE", false);
							CoreLivePollService.SetMetric(deviceSnapshot2, "UL", "0", "Mbps", "UE", false);
							device.LastReachable = new bool?(false);
							device.LastError = "Not found in parent '" + CS$<>8__locals1.core.Name + "' (e.g. UE disconnected).";
							device.LastSnapshotJson = SnapshotSerializer.Serialize(deviceSnapshot2);
							db.DevicePollResults.Add(new DevicePollResult
							{
								DeviceId = device.Id,
								Timestamp = now,
								Reachable = false,
								Error = device.LastError,
								SnapshotJson = device.LastSnapshotJson
							});
						}
					}
				}
			}
			DeviceSnapshot deviceSnapshot3 = SnapshotSerializer.Deserialize(CS$<>8__locals1.core.LastSnapshotJson);
			if (deviceSnapshot3 != null)
			{
				CoreLivePollService.SetMetric(deviceSnapshot3, "Total DL", CoreLivePollService.Fmt(sample.TotalDlMbps), "Mbps", "Throughput", false);
				CoreLivePollService.SetMetric(deviceSnapshot3, "Total UL", CoreLivePollService.Fmt(sample.TotalUlMbps), "Mbps", "Throughput", false);
				CoreLivePollService.SetMetric(deviceSnapshot3, "Registered UEs", sample.Ues.Values.Count<UeLive>((UeLive u) => u.State != "Disconnected").ToString(), null, "System", false);
				DeviceSnapshot deviceSnapshot4 = deviceSnapshot3;
				string text3 = "Live poll";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
				defaultInterpolatedStringHandler.AppendFormatted<DateTime>(now.ToLocalTime(), "HH:mm:ss");
				defaultInterpolatedStringHandler.AppendLiteral(", counters ");
				defaultInterpolatedStringHandler.AppendFormatted(sample.CountersAvailable ? "yes" : "no");
				CoreLivePollService.SetMetric(deviceSnapshot4, text3, defaultInterpolatedStringHandler.ToStringAndClear(), null, "System", false);
				List<UeLive> list = sample.Ues.Values.Where<UeLive>((UeLive u) => u.State != null).ToList<UeLive>();
				if (list.Count > 0)
				{
					CoreLivePollService.SetMetric(deviceSnapshot3, "Active UEs", list.Count<UeLive>((UeLive u) => string.Equals(u.State, "Connected", StringComparison.OrdinalIgnoreCase)).ToString(), null, "System", false);
				}
				foreach (ChildStatus childStatus in deviceSnapshot3.Children.Where<ChildStatus>((ChildStatus c) => string.Equals(c.Kind, "ue", StringComparison.OrdinalIgnoreCase)))
				{
					UeLive ueLive2;
					if (sample.Ues.TryGetValue(childStatus.Identifier, out ueLive2))
					{
						if (ueLive2.State != null)
						{
							childStatus.State = ueLive2.State;
						}
						int num = childStatus.Metrics.FindIndex((Metric m) => m.Label == "DL");
						if (num >= 0)
						{
							childStatus.Metrics[num] = new Metric("DL", CoreLivePollService.Fmt(ueLive2.DlMbps), "Mbps", "UE");
						}
						num = childStatus.Metrics.FindIndex((Metric m) => m.Label == "UL");
						if (num >= 0)
						{
							childStatus.Metrics[num] = new Metric("UL", CoreLivePollService.Fmt(ueLive2.UlMbps), "Mbps", "UE");
						}
					}
				}
				CS$<>8__locals1.core.LastSnapshotJson = SnapshotSerializer.Serialize(deviceSnapshot3);
			}
			db.ThroughputSamples.Add(new ThroughputSample
			{
				Timestamp = now,
				DlMbps = sample.TotalDlMbps,
				UlMbps = sample.TotalUlMbps
			});
			await db.SaveChangesAsync(ct);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000F7A3 File Offset: 0x0000D9A3
		private static string Fmt(double mbps)
		{
			return mbps.ToString("0.##", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		private static void SetMetric(DeviceSnapshot snap, string label, string value, [Nullable(2)] string unit, [Nullable(2)] string group, bool first = false)
		{
			int num = snap.Metrics.FindIndex((Metric m) => m.Label == label);
			Metric metric = new Metric(label, value, unit, group);
			if (num >= 0)
			{
				snap.Metrics[num] = metric;
				return;
			}
			if (first)
			{
				snap.Metrics.Insert(0, metric);
				return;
			}
			snap.Metrics.Add(metric);
		}

		// Token: 0x04000172 RID: 370
		private readonly IServiceScopeFactory _scopeFactory;

		// Token: 0x04000173 RID: 371
		private readonly PollingOptions _options;

		// Token: 0x04000174 RID: 372
		private readonly CredentialProtector _creds;

		// Token: 0x04000175 RID: 373
		private readonly CoreLiveState _state;

		// Token: 0x04000176 RID: 374
		private readonly ILogger<CoreLivePollService> _log;

		// Token: 0x04000177 RID: 375
		private readonly Dictionary<int, CoreLivePollService.CoreSession> _sessions = new Dictionary<int, CoreLivePollService.CoreSession>();

		// Token: 0x04000178 RID: 376
		private const double DataFlowMbps = 0.05;

		// Token: 0x04000179 RID: 377
		private const int SilentRoundsToDisconnect = 2;

		// Token: 0x0400017A RID: 378
		private static readonly TimeSpan PingBudget = TimeSpan.FromSeconds(4L);

		// Token: 0x02000120 RID: 288
		[Nullable(0)]
		private sealed class CoreSession : IDisposable
		{
			// Token: 0x06000A1F RID: 2591 RVA: 0x0003C0A4 File Offset: 0x0003A2A4
			public CoreSession(int coreId, ILogger log)
			{
				this._coreId = coreId;
				this._log = log;
			}

			// Token: 0x06000A20 RID: 2592 RVA: 0x0003C12C File Offset: 0x0003A32C
			[return: Nullable(2)]
			public CoreLiveSnapshot Sample(Device core, SshDefinition def, string password, TimeSpan budget, TimeSpan reconnect, TimeSpan pingEvery, CoreLiveState state, CancellationToken ct)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 5);
				defaultInterpolatedStringHandler.AppendFormatted(core.IpAddress);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted<int>(core.Port);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(core.SshUsername);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(core.SshPasswordEnc);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				DeviceTemplate template = core.Template;
				int? num;
				if (template == null)
				{
					num = null;
				}
				else
				{
					string definitionJson = template.DefinitionJson;
					num = ((definitionJson != null) ? new int?(definitionJson.Length) : null);
				}
				defaultInterpolatedStringHandler.AppendFormatted<int?>(num);
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (this._client != null && (this._connectionKey != text || !this._client.IsConnected))
				{
					this.Close();
				}
				if (this._client == null && DateTime.UtcNow < this._nextConnectUtc)
				{
					return null;
				}
				Stopwatch stopwatch = Stopwatch.StartNew();
				CoreLiveSnapshot coreLiveSnapshot;
				try
				{
					if (this._client == null)
					{
						this.Connect(core, def, password, text);
					}
					DateTime utcNow = DateTime.UtcNow;
					IEnumerable<H3c5gcParser.UpfSession> enumerable = H3c5gcParser.ParseUpfSessions(this.Run("display system internal upf session all", ct));
					bool flag = true;
					Dictionary<string, ValueTuple<double, double, long?, long?, string>> byImsi = new Dictionary<string, ValueTuple<double, double, long?, long?, string>>(StringComparer.OrdinalIgnoreCase);
					foreach (IGrouping<string, H3c5gcParser.UpfSession> grouping in enumerable.GroupBy<H3c5gcParser.UpfSession, string>((H3c5gcParser.UpfSession s) => s.Imsi, StringComparer.OrdinalIgnoreCase))
					{
						List<H3c5gcParser.UpfSession> list = grouping.ToList<H3c5gcParser.UpfSession>();
						bool flag2 = list.All<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession s) => s.DlBytes != null && s.UlBytes != null);
						flag = flag && flag2;
						Dictionary<string, ValueTuple<double, double, long?, long?, string>> byImsi2 = byImsi;
						string key = grouping.Key;
						double num2 = list.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession s) => s.DlMbps);
						double num3 = list.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession s) => s.UlMbps);
						long? num4;
						if (!flag2)
						{
							num4 = null;
						}
						else
						{
							num4 = new long?(list.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession s) => s.DlBytes.Value));
						}
						long? num5;
						if (!flag2)
						{
							num5 = null;
						}
						else
						{
							num5 = new long?(list.Sum<H3c5gcParser.UpfSession>((H3c5gcParser.UpfSession s) => s.UlBytes.Value));
						}
						byImsi2[key] = new ValueTuple<double, double, long?, long?, string>(num2, num3, num4, num5, list.Select<H3c5gcParser.UpfSession, string>((H3c5gcParser.UpfSession s) => s.UeIp).FirstOrDefault<string>((string ip) => !string.IsNullOrEmpty(ip)));
					}
					foreach (string text2 in byImsi.Keys.OrderBy<string, DateTime>(delegate(string i)
					{
						ValueTuple<string, string, DateTime> valueTuple7;
						if (!this._smf.TryGetValue(i, out valueTuple7))
						{
							return DateTime.MinValue;
						}
						return valueTuple7.Item3;
					}).ToList<string>())
					{
						if (stopwatch.Elapsed >= budget)
						{
							break;
						}
						List<H3c5gcParser.SmfSession> valueOrDefault = H3c5gcParser.ParseSmf(this.Run("display system internal smf ue-context supi " + text2 + " brief", ct)).GetValueOrDefault(text2);
						if (valueOrDefault != null && valueOrDefault.Count > 0)
						{
							this._smf[text2] = new ValueTuple<string, string, DateTime>(H3c5gcParser.SmfState(valueOrDefault), H3c5gcParser.SmfIp(valueOrDefault), DateTime.UtcNow);
						}
					}
					IEnumerable<string> keys = this._smf.Keys;
					Func<string, bool> func;
					Func<string, bool> <>9__10;
					if ((func = <>9__10) == null)
					{
						func = (<>9__10 = (string k) => !byImsi.ContainsKey(k));
					}
					foreach (string text3 in keys.Where<string>(func).ToList<string>())
					{
						this._smf.Remove(text3);
					}
					if (pingEvery > TimeSpan.Zero && utcNow - this._lastPingUtc >= pingEvery)
					{
						this._lastPingUtc = utcNow;
						foreach (string text4 in byImsi.Keys.OrderBy<string, DateTime>((string i) => this._lastPinged.GetValueOrDefault(i, DateTime.MinValue)))
						{
							if (!(stopwatch.Elapsed >= budget + CoreLivePollService.PingBudget))
							{
								CounterSample counterSample;
								if (!this._prev.TryGetValue(text4, out counterSample))
								{
									goto IL_0516;
								}
								long? num6 = byImsi[text4].Item3;
								if (num6 == null)
								{
									goto IL_0516;
								}
								long valueOrDefault2 = num6.GetValueOrDefault();
								num6 = byImsi[text4].Item4;
								if (num6 == null)
								{
									goto IL_0516;
								}
								long valueOrDefault3 = num6.GetValueOrDefault();
								bool flag3 = valueOrDefault2 > counterSample.DlBytes || valueOrDefault3 > counterSample.UlBytes;
								IL_0517:
								if (flag3 || byImsi[text4].Item1 > 0.05 || byImsi[text4].Item2 > 0.05)
								{
									this._silentRounds[text4] = 0;
									continue;
								}
								ValueTuple<string, string, DateTime> valueTuple;
								string text5 = (this._smf.TryGetValue(text4, out valueTuple) ? valueTuple.Item2 : byImsi[text4].Item5);
								if (!string.IsNullOrEmpty(text5))
								{
									this._lastPinged[text4] = utcNow;
									string text6 = this.Run("ping -c 2 -t 1000 " + text5, ct);
									Match match = CoreLivePollService.CoreSession.PingLoss.Match(text6);
									double num7;
									bool flag4 = match.Success && double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num7) && num7 >= 100.0;
									this._silentRounds[text4] = (flag4 ? (this._silentRounds.GetValueOrDefault(text4) + 1) : 0);
									continue;
								}
								continue;
								IL_0516:
								flag3 = false;
								goto IL_0517;
							}
							break;
						}
					}
					IEnumerable<string> keys2 = this._silentRounds.Keys;
					Func<string, bool> func2;
					Func<string, bool> <>9__12;
					if ((func2 = <>9__12) == null)
					{
						func2 = (<>9__12 = (string k) => !byImsi.ContainsKey(k));
					}
					foreach (string text7 in keys2.Where<string>(func2).ToList<string>())
					{
						this._silentRounds.Remove(text7);
					}
					IEnumerable<string> keys3 = this._lastPinged.Keys;
					Func<string, bool> func3;
					Func<string, bool> <>9__13;
					if ((func3 = <>9__13) == null)
					{
						func3 = (<>9__13 = (string k) => !byImsi.ContainsKey(k));
					}
					foreach (string text8 in keys3.Where<string>(func3).ToList<string>())
					{
						this._lastPinged.Remove(text8);
					}
					this._countersMove.RemoveWhere((string k) => !byImsi.ContainsKey(k));
					Dictionary<string, UeLive> dictionary = new Dictionary<string, UeLive>(StringComparer.OrdinalIgnoreCase);
					double num8 = 0.0;
					double num9 = 0.0;
					foreach (KeyValuePair<string, ValueTuple<double, double, long?, long?, string>> keyValuePair in byImsi)
					{
						string text9;
						ValueTuple<double, double, long?, long?, string> valueTuple2;
						keyValuePair.Deconstruct(out text9, out valueTuple2);
						string text10 = text9;
						ValueTuple<double, double, long?, long?, string> valueTuple3 = valueTuple2;
						double num10 = valueTuple3.Item1;
						double num11 = valueTuple3.Item2;
						long? num6 = valueTuple3.Item3;
						if (num6 != null)
						{
							long valueOrDefault4 = num6.GetValueOrDefault();
							num6 = valueTuple3.Item4;
							if (num6 != null)
							{
								long valueOrDefault5 = num6.GetValueOrDefault();
								CounterSample counterSample2 = new CounterSample(utcNow, valueOrDefault4, valueOrDefault5);
								CounterSample counterSample3;
								if (this._prev.TryGetValue(text10, out counterSample3))
								{
									ValueTuple<double, double>? valueTuple4 = CoreLiveRates.Rate(counterSample3, counterSample2);
									if (valueTuple4 != null)
									{
										ValueTuple<double, double> valueOrDefault6 = valueTuple4.GetValueOrDefault();
										if (valueOrDefault6.Item1 > 0.0 || valueOrDefault6.Item2 > 0.0)
										{
											this._countersMove.Add(text10);
										}
										if (this._countersMove.Contains(text10))
										{
											num10 = valueOrDefault6.Item1;
											num11 = valueOrDefault6.Item2;
										}
										else
										{
											if (valueOrDefault6.Item1 > 0.0 || valueTuple3.Item1 <= 0.0)
											{
												num10 = valueOrDefault6.Item1;
											}
											if (valueOrDefault6.Item2 > 0.0 || valueTuple3.Item2 <= 0.0)
											{
												num11 = valueOrDefault6.Item2;
											}
										}
									}
								}
								this._prev[text10] = counterSample2;
								state.RecordCounters(this._coreId, text10, counterSample2);
							}
						}
						ValueTuple<string, string, DateTime> valueTuple6;
						ValueTuple<string, string, DateTime?> valueTuple5 = (this._smf.TryGetValue(text10, out valueTuple6) ? new ValueTuple<string, string, DateTime?>(valueTuple6.Item1, valueTuple6.Item2, new DateTime?(valueTuple6.Item3)) : new ValueTuple<string, string, DateTime?>(null, null, null));
						string text11 = valueTuple5.Item1;
						DateTime? item = valueTuple5.Item3;
						if (num10 > 0.05 || num11 > 0.05)
						{
							text11 = "Connected";
							item = new DateTime?(utcNow);
							this._silentRounds[text10] = 0;
						}
						else if (this._silentRounds.GetValueOrDefault(text10) >= 2)
						{
							text11 = "Disconnected";
							item = new DateTime?(utcNow);
						}
						dictionary[text10] = new UeLive(text10, text11, valueTuple5.Item2 ?? valueTuple3.Item5, num10, num11, utcNow, item);
						num8 += num10;
						num9 += num11;
					}
					IEnumerable<string> keys4 = this._prev.Keys;
					Func<string, bool> func4;
					Func<string, bool> <>9__14;
					if ((func4 = <>9__14) == null)
					{
						func4 = (<>9__14 = (string k) => !byImsi.ContainsKey(k));
					}
					foreach (string text12 in keys4.Where<string>(func4).ToList<string>())
					{
						this._prev.Remove(text12);
					}
					if (this._reportedDown)
					{
						this._log.LogInformation("Live core poll of {Core} is back.", new object[] { core.Name });
						this._reportedDown = false;
					}
					coreLiveSnapshot = new CoreLiveSnapshot(this._coreId, utcNow, dictionary, num8, num9, flag && byImsi.Count > 0);
				}
				catch (OperationCanceledException obj) when (ct.IsCancellationRequested)
				{
					throw;
				}
				catch (Exception ex)
				{
					if (!this._reportedDown)
					{
						this._log.LogWarning("Live core poll of {Core} failed ({Error}); retrying every {Retry}s.", new object[] { core.Name, ex.Message, reconnect.TotalSeconds });
						this._reportedDown = true;
					}
					this.Close();
					this._nextConnectUtc = DateTime.UtcNow + reconnect;
					coreLiveSnapshot = null;
				}
				return coreLiveSnapshot;
			}

			// Token: 0x06000A21 RID: 2593 RVA: 0x0003CD8C File Offset: 0x0003AF8C
			private void Connect(Device core, SshDefinition def, string password, string key)
			{
				ConnectionInfo connectionInfo = new ConnectionInfo(core.IpAddress, (core.Port > 0) ? core.Port : 22, core.SshUsername, new AuthenticationMethod[]
				{
					new PasswordAuthenticationMethod(core.SshUsername, password)
				});
				connectionInfo.Timeout = TimeSpan.FromSeconds(8L);
				SshScriptCollector.RestrictAlgorithms<Func<byte[], KeyHostAlgorithm>>(connectionInfo.HostKeyAlgorithms, def.HostKeyAlgorithms);
				SshScriptCollector.RestrictAlgorithms<Func<IKeyExchange>>(connectionInfo.KeyExchangeAlgorithms, def.KeyExchangeAlgorithms);
				SshScriptCollector.RestrictAlgorithms<CipherInfo>(connectionInfo.Encryptions, def.Encryptions);
				SshClient sshClient = new SshClient(connectionInfo)
				{
					KeepAliveInterval = TimeSpan.FromSeconds(15L)
				};
				sshClient.Connect();
				this._client = sshClient;
				this._shell = sshClient.CreateShellStream("vt100", 200U, 50U, 800U, 600U, 16384);
				if (this._shell.Expect(SshScriptCollector.PromptRegex, TimeSpan.FromSeconds(8L), -1) == null)
				{
					throw new TimeoutException("no prompt after login");
				}
				this.Run("screen-length disable", CancellationToken.None);
				this.Run("sys", CancellationToken.None);
				this.Run("upf session statistics enable", CancellationToken.None);
				this.Run("probe", CancellationToken.None);
				this._connectionKey = key;
				this._smf.Clear();
				this._prev.Clear();
				this._silentRounds.Clear();
				this._lastPinged.Clear();
				this._countersMove.Clear();
				this._lastPingUtc = DateTime.MinValue;
			}

			// Token: 0x06000A22 RID: 2594 RVA: 0x0003CF10 File Offset: 0x0003B110
			private string Run(string command, CancellationToken ct)
			{
				ct.ThrowIfCancellationRequested();
				if (this._shell == null)
				{
					throw new InvalidOperationException("no shell");
				}
				this._shell.WriteLine(command);
				string text = this._shell.Expect(SshScriptCollector.PromptRegex, TimeSpan.FromSeconds(10L), -1);
				if (text == null)
				{
					throw new TimeoutException("no prompt after '" + command + "'");
				}
				return text;
			}

			// Token: 0x06000A23 RID: 2595 RVA: 0x0003CF78 File Offset: 0x0003B178
			private void Close()
			{
				try
				{
					ShellStream shell = this._shell;
					if (shell != null)
					{
						shell.Dispose();
					}
				}
				catch
				{
				}
				try
				{
					SshClient client = this._client;
					if (client != null)
					{
						client.Dispose();
					}
				}
				catch
				{
				}
				this._shell = null;
				this._client = null;
				this._connectionKey = null;
			}

			// Token: 0x06000A24 RID: 2596 RVA: 0x0003CFE4 File Offset: 0x0003B1E4
			public void Dispose()
			{
				this.Close();
			}

			// Token: 0x040005C7 RID: 1479
			private readonly int _coreId;

			// Token: 0x040005C8 RID: 1480
			private readonly ILogger _log;

			// Token: 0x040005C9 RID: 1481
			[Nullable(2)]
			private SshClient _client;

			// Token: 0x040005CA RID: 1482
			[Nullable(2)]
			private ShellStream _shell;

			// Token: 0x040005CB RID: 1483
			[Nullable(2)]
			private string _connectionKey;

			// Token: 0x040005CC RID: 1484
			private DateTime _nextConnectUtc = DateTime.MinValue;

			// Token: 0x040005CD RID: 1485
			private bool _reportedDown;

			// Token: 0x040005CE RID: 1486
			[TupleElementNames(new string[] { "State", "Ip", "ReadUtc" })]
			[Nullable(new byte[] { 1, 1, 0, 2, 2 })]
			private readonly Dictionary<string, ValueTuple<string, string, DateTime>> _smf = new Dictionary<string, ValueTuple<string, string, DateTime>>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x040005CF RID: 1487
			private readonly Dictionary<string, CounterSample> _prev = new Dictionary<string, CounterSample>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x040005D0 RID: 1488
			private readonly Dictionary<string, int> _silentRounds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x040005D1 RID: 1489
			private readonly Dictionary<string, DateTime> _lastPinged = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x040005D2 RID: 1490
			private readonly HashSet<string> _countersMove = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x040005D3 RID: 1491
			private DateTime _lastPingUtc = DateTime.MinValue;

			// Token: 0x040005D4 RID: 1492
			private static readonly Regex PingLoss = new Regex("(\\d+(?:\\.\\d+)?)%\\s*packet loss", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
	}
}
