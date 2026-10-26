using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors.Bbu;
using AbsolutDashboard.Web.Services.Collectors.Core;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using AbsolutDashboard.Web.Services.Integrity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000037 RID: 55
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DevicePollingService : BackgroundService
	{
		// Token: 0x06000163 RID: 355 RVA: 0x000077C4 File Offset: 0x000059C4
		public DevicePollingService(IServiceScopeFactory scopeFactory, IEnumerable<IDeviceCollector> collectors, IOptions<PollingOptions> options, RhubSerialClient serial, CredentialProtector creds, BbuSessionLock bbuSessions, CoreLiveState live, ILogger<DevicePollingService> log)
		{
			this._scopeFactory = scopeFactory;
			this._collectors = collectors.ToDictionary<IDeviceCollector, CollectionMethod>((IDeviceCollector c) => c.Method);
			this._options = options.Value;
			this._serial = serial;
			this._creds = creds;
			this._bbuSessions = bbuSessions;
			this._live = live;
			this._log = log;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007840 File Offset: 0x00005A40
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (!this._options.Enabled)
			{
				this._log.LogInformation("Device polling is disabled by configuration.", Array.Empty<object>());
			}
			else
			{
				this._log.LogInformation("Device polling service started (tick {Tick}s).", new object[] { this._options.TickSeconds });
				TimeSpan tick = TimeSpan.FromSeconds((long)Math.Max(1, this._options.TickSeconds));
				while (!stoppingToken.IsCancellationRequested)
				{
					try
					{
						await this.PollDueDevicesAsync(stoppingToken);
					}
					catch (OperationCanceledException obj) when (stoppingToken.IsCancellationRequested)
					{
						break;
					}
					catch (Exception ex)
					{
						this._log.LogError(ex, "Polling cycle failed.", Array.Empty<object>());
					}
					try
					{
						await Task.Delay(tick, stoppingToken);
					}
					catch (OperationCanceledException)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000788C File Offset: 0x00005A8C
		private async Task PollDueDevicesAsync(CancellationToken ct)
		{
			DevicePollingService.<>c__DisplayClass10_0 CS$<>8__locals1 = new DevicePollingService.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ct = ct;
			using (IServiceScope scope = this._scopeFactory.CreateScope())
			{
				ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				IntegrityGate integrityGate = scope.ServiceProvider.GetRequiredService<IntegrityGate>();
				IQueryable<DevicePollResult> devicePollResults = db.DevicePollResults;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
				DateTime? dateTime = await devicePollResults.MaxAsync<DevicePollResult, DateTime?>(Expression.Lambda<Func<DevicePollResult, DateTime?>>(Expression.Convert(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), typeof(DateTime?)), new ParameterExpression[] { parameterExpression }), CS$<>8__locals1.ct);
				integrityGate.Evaluate(DateTime.UtcNow, dateTime);
				if (!integrityGate.IsBlocked)
				{
					IQueryable<Device> devices2 = db.Devices;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable = devices2.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable2 = queryable.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					List<Device> devices = await queryable2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Property(parameterExpression, methodof(Device.get_Enabled())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(CS$<>8__locals1.ct);
					Dictionary<int, Device> byId = devices.ToDictionary<Device, int>((Device d) => d.Id);
					CS$<>8__locals1.now = DateTime.UtcNow;
					List<Device> direct = devices.Where<Device>((Device d) => d.CollectionMethod != CollectionMethod.ViaParent && base.<PollDueDevicesAsync>g__Due|1(d)).ToList<Device>();
					CS$<>8__locals1.outcomes = new Dictionary<int, PollOutcome>();
					if (direct.Count > 0)
					{
						DevicePollingService.<>c__DisplayClass10_1 CS$<>8__locals2 = new DevicePollingService.<>c__DisplayClass10_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.gate = new SemaphoreSlim(Math.Max(1, this._options.MaxConcurrency));
						try
						{
							foreach (ValueTuple<int, PollOutcome> valueTuple in await Task.WhenAll<ValueTuple<int, PollOutcome>>(direct.Select<Device, Task<ValueTuple<int, PollOutcome>>>(delegate(Device d)
							{
								DevicePollingService.<>c__DisplayClass10_1.<<PollDueDevicesAsync>b__9>d <<PollDueDevicesAsync>b__9>d;
								<<PollDueDevicesAsync>b__9>d.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<int, PollOutcome>>.Create();
								<<PollDueDevicesAsync>b__9>d.<>4__this = CS$<>8__locals2;
								<<PollDueDevicesAsync>b__9>d.d = d;
								<<PollDueDevicesAsync>b__9>d.<>1__state = -1;
								<<PollDueDevicesAsync>b__9>d.<>t__builder.Start<DevicePollingService.<>c__DisplayClass10_1.<<PollDueDevicesAsync>b__9>d>(ref <<PollDueDevicesAsync>b__9>d);
								return <<PollDueDevicesAsync>b__9>d.<>t__builder.Task;
							})))
							{
								int item = valueTuple.Item1;
								PollOutcome item2 = valueTuple.Item2;
								if (item2 != null)
								{
									CS$<>8__locals2.CS$<>8__locals1.outcomes[item] = item2;
								}
							}
							foreach (Device device in direct.Where<Device>(delegate(Device d)
							{
								DeviceCategory category = d.Category;
								return ((category != null) ? category.Key : null) == "mobile";
							}))
							{
								PollOutcome pollOutcome;
								if (CS$<>8__locals2.CS$<>8__locals1.outcomes.TryGetValue(device.Id, out pollOutcome) && pollOutcome.Reachable)
								{
									this.OverlayLiveRates(device, pollOutcome.Snapshot, CS$<>8__locals2.CS$<>8__locals1.now);
								}
							}
						}
						finally
						{
							if (CS$<>8__locals2.gate != null)
							{
								((IDisposable)CS$<>8__locals2.gate).Dispose();
							}
						}
						CS$<>8__locals2 = null;
					}
					List<Device> list = direct.Where<Device>(delegate(Device d)
					{
						PollOutcome pollOutcome2;
						return CS$<>8__locals1.outcomes.TryGetValue(d.Id, out pollOutcome2) && pollOutcome2.Reachable && pollOutcome2.Snapshot.Children.Count > 0;
					}).ToList<Device>();
					if (list.Count > 0)
					{
						try
						{
							foreach (Device device2 in await this.ReconcileDiscoveredChildrenAsync(db, list, CS$<>8__locals1.outcomes, CS$<>8__locals1.ct))
							{
								devices.Add(device2);
								byId[device2.Id] = device2;
							}
						}
						catch (OperationCanceledException obj) when (CS$<>8__locals1.ct.IsCancellationRequested)
						{
							throw;
						}
						catch (Exception ex)
						{
							this._log.LogError(ex, "Discovered-child reconcile failed; device status still saved.", Array.Empty<object>());
							List<EntityEntry>.Enumerator enumerator3 = (from e in db.ChangeTracker.Entries()
								where e.State == EntityState.Added
								select e).ToList<EntityEntry>().GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									EntityEntry entityEntry = enumerator3.Current;
									entityEntry.State = EntityState.Detached;
								}
							}
							finally
							{
								int num;
								if (num < 0)
								{
									((IDisposable)enumerator3).Dispose();
								}
							}
						}
					}
					IEnumerable<Device> enumerable = devices;
					Func<Device, bool> func;
					if ((func = CS$<>8__locals1.<>9__12) == null)
					{
						DevicePollingService.<>c__DisplayClass10_0 CS$<>8__locals3 = CS$<>8__locals1;
						Func<Device, bool> func2 = (Device d) => d.CollectionMethod == CollectionMethod.ViaParent && base.<PollDueDevicesAsync>g__Due|1(d);
						CS$<>8__locals3.<>9__12 = func2;
						func = func2;
					}
					foreach (Device device3 in enumerable.Where<Device>(func))
					{
						CS$<>8__locals1.outcomes[device3.Id] = DevicePollingService.ResolveViaParent(device3, byId, CS$<>8__locals1.outcomes);
					}
					IEnumerable<Device> enumerable2 = devices;
					Func<Device, bool> func3;
					if ((func3 = CS$<>8__locals1.<>9__13) == null)
					{
						DevicePollingService.<>c__DisplayClass10_0 CS$<>8__locals4 = CS$<>8__locals1;
						Func<Device, bool> func4 = delegate(Device d)
						{
							DeviceCategory category2 = d.Category;
							return ((category2 != null) ? category2.Key : null) == "eu" && !string.IsNullOrWhiteSpace(d.SerialPort) && CS$<>8__locals1.outcomes.ContainsKey(d.Id);
						};
						CS$<>8__locals4.<>9__13 = func4;
						func3 = func4;
					}
					foreach (Device device4 in enumerable2.Where<Device>(func3))
					{
						await this.AddSerialPowerAsync(device4, CS$<>8__locals1.outcomes[device4.Id], CS$<>8__locals1.ct);
					}
					IEnumerator<Device> enumerator4 = null;
					List<int> polledIds = new List<int>();
					IEnumerable<Device> enumerable3 = devices;
					Func<Device, bool> func5;
					if ((func5 = CS$<>8__locals1.<>9__14) == null)
					{
						DevicePollingService.<>c__DisplayClass10_0 CS$<>8__locals5 = CS$<>8__locals1;
						Func<Device, bool> func6 = (Device d) => CS$<>8__locals1.outcomes.ContainsKey(d.Id);
						CS$<>8__locals5.<>9__14 = func6;
						func5 = func6;
					}
					foreach (Device device5 in enumerable3.Where<Device>(func5))
					{
						DevicePollingService.Apply(db, device5, CS$<>8__locals1.outcomes[device5.Id], CS$<>8__locals1.now);
						polledIds.Add(device5.Id);
					}
					if (polledIds.Count > 0)
					{
						await db.SaveChangesAsync(CS$<>8__locals1.ct);
						if (!devices.Any<Device>(delegate(Device d)
						{
							DeviceCategory category3 = d.Category;
							return ((category3 != null) ? category3.Key : null) == "mobile" && CS$<>8__locals1.<>4__this._live.Latest(d.Id, CS$<>8__locals1.<>4__this.LiveFreshness) != null;
						}))
						{
							await DevicePollingService.RecordThroughputAsync(db, devices, polledIds, CS$<>8__locals1.now, CS$<>8__locals1.ct);
						}
						await this.TrimHistoryAsync(db, polledIds, CS$<>8__locals1.ct);
					}
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000078D7 File Offset: 0x00005AD7
		private TimeSpan LiveFreshness
		{
			get
			{
				return TimeSpan.FromSeconds((long)(Math.Max(2, this._options.CoreLiveIntervalSeconds) * 3));
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000078F4 File Offset: 0x00005AF4
		private void OverlayLiveRates(Device core, DeviceSnapshot snapshot, DateTime now)
		{
			int num = ((core.PollIntervalSeconds > 0) ? core.PollIntervalSeconds : this._options.DefaultIntervalSeconds);
			DateTime dateTime = core.LastPolledAt ?? now.AddSeconds((double)(-(double)num));
			CoreLiveSnapshot coreLiveSnapshot = this._live.Latest(core.Id, this.LiveFreshness);
			bool flag = false;
			foreach (ChildStatus childStatus in snapshot.Children.Where<ChildStatus>((ChildStatus c) => string.Equals(c.Kind, "ue", StringComparison.OrdinalIgnoreCase)))
			{
				UeLive ueLive;
				if (coreLiveSnapshot != null && coreLiveSnapshot.Ues.TryGetValue(childStatus.Identifier, out ueLive))
				{
					if (ueLive.State != null)
					{
						DateTime? stateReadUtc = ueLive.StateReadUtc;
						if (stateReadUtc != null)
						{
							DateTime valueOrDefault = stateReadUtc.GetValueOrDefault();
							if (valueOrDefault > dateTime)
							{
								childStatus.State = ueLive.State;
								if (ueLive.State == "Disconnected")
								{
									childStatus.Up = new bool?(false);
								}
								int num2 = childStatus.Metrics.FindIndex((Metric m) => m.Label == "Connection");
								if (num2 >= 0)
								{
									List<Metric> metrics = childStatus.Metrics;
									int num3 = num2;
									Metric metric = childStatus.Metrics[num2].<Clone>$();
									metric.Value = ueLive.State;
									metrics[num3] = metric;
								}
							}
						}
					}
					if (!string.IsNullOrEmpty(ueLive.Ip))
					{
						int num4 = childStatus.Metrics.FindIndex((Metric m) => m.Label == "IP");
						if (num4 >= 0)
						{
							List<Metric> metrics2 = childStatus.Metrics;
							int num5 = num4;
							Metric metric2 = childStatus.Metrics[num4].<Clone>$();
							metric2.Value = ueLive.Ip;
							metrics2[num5] = metric2;
						}
					}
				}
				ValueTuple<double, double>? valueTuple = this._live.RateSince(core.Id, childStatus.Identifier, dateTime);
				if (valueTuple != null)
				{
					ValueTuple<double, double> valueOrDefault2 = valueTuple.GetValueOrDefault();
					DevicePollingService.<OverlayLiveRates>g__Replace|13_0(childStatus.Metrics, "DL", valueOrDefault2.Item1);
					DevicePollingService.<OverlayLiveRates>g__Replace|13_0(childStatus.Metrics, "UL", valueOrDefault2.Item2);
					flag = true;
				}
			}
			if (coreLiveSnapshot != null)
			{
				List<ChildStatus> list = snapshot.Children.Where<ChildStatus>((ChildStatus c) => string.Equals(c.Kind, "ue", StringComparison.OrdinalIgnoreCase)).ToList<ChildStatus>();
				int num6 = snapshot.Metrics.FindIndex((Metric m) => m.Label == "Active UEs");
				if (num6 >= 0)
				{
					List<Metric> metrics3 = snapshot.Metrics;
					int num7 = num6;
					Metric metric3 = snapshot.Metrics[num6].<Clone>$();
					metric3.Value = list.Count<ChildStatus>((ChildStatus c) => string.Equals(c.State, "Connected", StringComparison.OrdinalIgnoreCase)).ToString();
					metrics3[num7] = metric3;
				}
				int num8 = snapshot.Metrics.FindIndex((Metric m) => m.Label == "Registered UEs");
				if (num8 >= 0)
				{
					List<Metric> metrics4 = snapshot.Metrics;
					int num9 = num8;
					Metric metric4 = snapshot.Metrics[num8].<Clone>$();
					metric4.Value = list.Count<ChildStatus>((ChildStatus c) => !string.Equals(c.State, "Disconnected", StringComparison.OrdinalIgnoreCase)).ToString();
					metrics4[num9] = metric4;
				}
			}
			if (!flag)
			{
				return;
			}
			double num10 = 0.0;
			double num11 = 0.0;
			foreach (ChildStatus childStatus2 in snapshot.Children.Where<ChildStatus>((ChildStatus c) => string.Equals(c.Kind, "ue", StringComparison.OrdinalIgnoreCase)))
			{
				double num12 = num10;
				Metric metric5 = childStatus2.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "DL");
				double num13;
				num10 = num12 + (MetricHistory.TryParseNumeric((metric5 != null) ? metric5.Value : null, out num13) ? num13 : 0.0);
				double num14 = num11;
				Metric metric6 = childStatus2.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "UL");
				double num15;
				num11 = num14 + (MetricHistory.TryParseNumeric((metric6 != null) ? metric6.Value : null, out num15) ? num15 : 0.0);
			}
			DevicePollingService.<OverlayLiveRates>g__Replace|13_0(snapshot.Metrics, "Total DL", num10);
			DevicePollingService.<OverlayLiveRates>g__Replace|13_0(snapshot.Metrics, "Total UL", num11);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007E18 File Offset: 0x00006018
		private async Task AddSerialPowerAsync(Device eu, PollOutcome outcome, CancellationToken ct)
		{
			try
			{
				TimeSpan budget = TimeSpan.FromSeconds((long)Math.Max(1, this._options.SerialTimeoutSeconds));
				Task<string> read = this._serial.ReadPowerAsync(eu.SerialPort, eu.SerialBaudRate, eu.SerialFraming, this._creds.Unprotect(eu.SshPasswordEnc), budget, ct);
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(read, Task.Delay(budget + TimeSpan.FromSeconds(2L), ct)).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() != read)
				{
					this._log.LogWarning("Serial power read for {Device} on {Port} did not finish within {Budget}s; leaving its power ports unknown.", new object[] { eu.Name, eu.SerialPort, budget.TotalSeconds });
				}
				else
				{
					string text = await read;
					RhubPowerParseResult rhubPowerParseResult = RhubPowerResponseParser.Parse(text, new DateTimeOffset?(DateTimeOffset.Now));
					outcome.Snapshot.HubPorts.AddRange(rhubPowerParseResult.Ports);
					string text2;
					if (text.Length != 0)
					{
						if (!rhubPowerParseResult.TimedOut)
						{
							if (!rhubPowerParseResult.PasswordPromptMidStream)
							{
								if (rhubPowerParseResult.Ports.Count != 0)
								{
									if (!rhubPowerParseResult.Truncated)
									{
										text2 = null;
									}
									else
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
										defaultInterpolatedStringHandler.AppendLiteral("ended after ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(rhubPowerParseResult.Ports.Count);
										defaultInterpolatedStringHandler.AppendLiteral(" channel(s) without the unit returning ");
										text2 = defaultInterpolatedStringHandler.ToStringAndClear() + "to its prompt, so the list may be incomplete";
									}
								}
								else
								{
									text2 = "a response arrived but contained no channel lines";
								}
							}
							else
							{
								text2 = "the session dropped mid-read (it re-prompted for a password)";
							}
						}
						else
						{
							text2 = "the unit reported a timeout";
						}
					}
					else
					{
						text2 = "no response at all — is anything attached to this port?";
					}
					string text3 = text2;
					if (text3 != null)
					{
						this._log.LogWarning("Serial power read for {Device} on {Port}: {Problem}. Raw response: {Raw}", new object[]
						{
							eu.Name,
							eu.SerialPort,
							text3,
							DevicePollingService.Printable(text, 2000)
						});
					}
					read = null;
				}
			}
			catch (OperationCanceledException obj) when (ct.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex)
			{
				this._log.LogWarning(ex, "Serial power read failed for {Device} on {Port}.", new object[] { eu.Name, eu.SerialPort });
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007E74 File Offset: 0x00006074
		private static string Printable([Nullable(2)] string raw, int max = 2000)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return "(empty)";
			}
			string text = ((raw.Length > max) ? (raw.Substring(0, max) + " ...(truncated in log)") : raw);
			StringBuilder stringBuilder = new StringBuilder(text.Length + 16);
			foreach (char c in text)
			{
				if (c == '\r')
				{
					stringBuilder.Append("\\r");
				}
				else if (c == '\n')
				{
					stringBuilder.Append("\\n");
				}
				else if (c == '\t')
				{
					stringBuilder.Append("\\t");
				}
				else if (char.IsControl(c))
				{
					StringBuilder stringBuilder2 = stringBuilder.Append("\\x");
					int num = (int)c;
					stringBuilder2.Append(num.ToString("X2"));
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007F48 File Offset: 0x00006148
		[return: Nullable(new byte[] { 1, 2 })]
		private async Task<PollOutcome> PollOneAsync(Device device, CancellationToken ct)
		{
			IDeviceCollector deviceCollector;
			PollOutcome pollOutcome;
			if (!this._collectors.TryGetValue(device.CollectionMethod, out deviceCollector))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No collector for ");
				defaultInterpolatedStringHandler.AppendFormatted<CollectionMethod>(device.CollectionMethod);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				pollOutcome = PollOutcome.Unreachable(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				bool holdsSession = device.CollectionMethod == CollectionMethod.BbuWebSocket;
				if (holdsSession && !this._bbuSessions.TryEnter(device.Id))
				{
					this._log.LogDebug("Skipping {Device}: its operator session is in use by a configuration write.", new object[] { device.Name });
					pollOutcome = null;
				}
				else
				{
					try
					{
						CollectionMethod collectionMethod = device.CollectionMethod;
						int num;
						if (collectionMethod != CollectionMethod.SshScript)
						{
							if (collectionMethod == CollectionMethod.BbuWebSocket)
							{
								num = this._options.BbuTimeoutSeconds;
							}
							else
							{
								num = this._options.TimeoutSeconds;
							}
						}
						else
						{
							num = this._options.SshTimeoutSeconds;
						}
						int timeoutSec = num;
						using (CancellationTokenSource timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
						{
							timeoutCts.CancelAfter(TimeSpan.FromSeconds((long)Math.Max(1, timeoutSec)));
							try
							{
								pollOutcome = await deviceCollector.CollectAsync(device, timeoutCts.Token);
							}
							catch (OperationCanceledException obj) when (!ct.IsCancellationRequested)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(18, 1);
								defaultInterpolatedStringHandler2.AppendLiteral("Timed out after ");
								defaultInterpolatedStringHandler2.AppendFormatted<int>(timeoutSec);
								defaultInterpolatedStringHandler2.AppendLiteral("s.");
								pollOutcome = PollOutcome.Unreachable(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
							catch (Exception ex)
							{
								pollOutcome = PollOutcome.Unreachable(ex.Message);
							}
						}
					}
					finally
					{
						if (holdsSession)
						{
							this._bbuSessions.Release(device.Id);
						}
					}
				}
			}
			return pollOutcome;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007F9C File Offset: 0x0000619C
		private static PollOutcome ResolveViaParent(Device child, Dictionary<int, Device> byId, Dictionary<int, PollOutcome> outcomes)
		{
			DevicePollingService.<>c__DisplayClass17_0 CS$<>8__locals1 = new DevicePollingService.<>c__DisplayClass17_0();
			Device device = DevicePollingService.FindDiscoveryAncestor(child, byId);
			if (device == null)
			{
				return PollOutcome.Unreachable("No polled parent linked.");
			}
			PollOutcome pollOutcome;
			DeviceSnapshot deviceSnapshot;
			bool? flag;
			bool flag2;
			if (outcomes.TryGetValue(device.Id, out pollOutcome))
			{
				if (!pollOutcome.Reachable)
				{
					return PollOutcome.Unreachable("Parent '" + device.Name + "' is unreachable.");
				}
				deviceSnapshot = pollOutcome.Snapshot;
			}
			else
			{
				flag = device.LastReachable;
				flag2 = false;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					return PollOutcome.Unreachable("Parent '" + device.Name + "' is unreachable.");
				}
				deviceSnapshot = SnapshotSerializer.Deserialize(device.LastSnapshotJson);
			}
			if (deviceSnapshot == null)
			{
				return PollOutcome.Unreachable("No data from parent '" + device.Name + "' yet.");
			}
			DevicePollingService.<>c__DisplayClass17_0 CS$<>8__locals2 = CS$<>8__locals1;
			string serialNumber = child.SerialNumber;
			CS$<>8__locals2.serial = ((serialNumber != null) ? serialNumber.Trim() : null);
			DevicePollingService.<>c__DisplayClass17_0 CS$<>8__locals3 = CS$<>8__locals1;
			string childIdentifier = child.ChildIdentifier;
			CS$<>8__locals3.ident = ((childIdentifier != null) ? childIdentifier.Trim() : null);
			ChildStatus childStatus = deviceSnapshot.Children.FirstOrDefault<ChildStatus>((ChildStatus c) => !string.IsNullOrEmpty(CS$<>8__locals1.serial) && string.Equals(c.SerialNumber, CS$<>8__locals1.serial, StringComparison.OrdinalIgnoreCase)) ?? deviceSnapshot.Children.FirstOrDefault<ChildStatus>((ChildStatus c) => !string.IsNullOrEmpty(CS$<>8__locals1.ident) && string.Equals(c.Identifier, CS$<>8__locals1.ident, StringComparison.OrdinalIgnoreCase));
			if (childStatus == null)
			{
				return PollOutcome.Unreachable("Not found in parent '" + device.Name + "' (e.g. UE disconnected).");
			}
			DeviceSnapshot deviceSnapshot2 = new DeviceSnapshot
			{
				Metrics = new List<Metric>(childStatus.Metrics)
			};
			if (!string.IsNullOrWhiteSpace(childStatus.State))
			{
				deviceSnapshot2.Metrics.Insert(0, new Metric("State", childStatus.State, null, "Status"));
			}
			if (string.Equals(childStatus.Kind, "eu", StringComparison.OrdinalIgnoreCase))
			{
				deviceSnapshot2.HubPorts.AddRange(RhubDataPorts.From(childStatus.Metrics, new DateTimeOffset?(DateTimeOffset.Now)));
			}
			PollOutcome pollOutcome2 = new PollOutcome();
			pollOutcome2.Reachable = childStatus.Up.GetValueOrDefault(true);
			flag = childStatus.Up;
			flag2 = false;
			string text;
			if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
			{
				text = null;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Reported ");
				defaultInterpolatedStringHandler.AppendFormatted(string.IsNullOrWhiteSpace(childStatus.State) ? "offline" : childStatus.State);
				defaultInterpolatedStringHandler.AppendLiteral(" by '");
				defaultInterpolatedStringHandler.AppendFormatted(device.Name);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			pollOutcome2.Error = text;
			pollOutcome2.Snapshot = deviceSnapshot2;
			return pollOutcome2;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008220 File Offset: 0x00006420
		[return: Nullable(2)]
		private static Device FindDiscoveryAncestor(Device d, Dictionary<int, Device> byId)
		{
			HashSet<int> hashSet = new HashSet<int> { d.Id };
			Device device = d;
			Device device2;
			for (;;)
			{
				int? parentDeviceId = device.ParentDeviceId;
				if (parentDeviceId == null)
				{
					goto IL_0056;
				}
				int valueOrDefault = parentDeviceId.GetValueOrDefault();
				if (!byId.TryGetValue(valueOrDefault, out device2))
				{
					goto IL_0056;
				}
				if (device2.CollectionMethod != CollectionMethod.ViaParent)
				{
					break;
				}
				if (!hashSet.Add(device2.Id))
				{
					goto IL_0056;
				}
				device = device2;
			}
			return device2;
			IL_0056:
			return null;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008284 File Offset: 0x00006484
		private async Task<List<Device>> ReconcileDiscoveredChildrenAsync(ApplicationDbContext db, List<Device> parents, Dictionary<int, PollOutcome> outcomes, CancellationToken ct)
		{
			List<Device> created = new List<Device>();
			List<DeviceCategory> list = await db.DeviceCategories.ToListAsync<DeviceCategory>(ct);
			Dictionary<string, DeviceCategory> catByKey = list.ToDictionary<DeviceCategory, string>((DeviceCategory c) => c.Key, StringComparer.OrdinalIgnoreCase);
			List<DeviceTemplate> templates = await db.DeviceTemplates.ToListAsync<DeviceTemplate>(ct);
			List<Device> list2 = await db.Devices.ToListAsync<Device>(ct);
			Dictionary<string, Device> dictionary = list2.Where<Device>((Device d) => !string.IsNullOrWhiteSpace(d.SerialNumber)).GroupBy<Device, string>((Device d) => d.SerialNumber, StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, Device>, string, Device>((IGrouping<string, Device> g) => g.Key, (IGrouping<string, Device> g) => g.First<Device>(), StringComparer.OrdinalIgnoreCase);
			using (List<Device>.Enumerator enumerator = parents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DevicePollingService.<>c__DisplayClass19_0 CS$<>8__locals1 = new DevicePollingService.<>c__DisplayClass19_0();
					CS$<>8__locals1.parent = enumerator.Current;
					DeviceSnapshot snapshot = outcomes[CS$<>8__locals1.parent.Id].Snapshot;
					Dictionary<string, Device> dictionary2 = new Dictionary<string, Device>(StringComparer.OrdinalIgnoreCase);
					Dictionary<string, Device> dictionary3 = new Dictionary<string, Device>(StringComparer.OrdinalIgnoreCase);
					string[] array = new string[] { "au", "eu", "ru", "cell", "ue" };
					for (int i = 0; i < array.Length; i++)
					{
						DevicePollingService.<>c__DisplayClass19_1 CS$<>8__locals2 = new DevicePollingService.<>c__DisplayClass19_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.kind = array[i];
						DeviceCategory cat;
						if (catByKey.TryGetValue(CS$<>8__locals2.kind, out cat))
						{
							IEnumerable<ChildStatus> children = snapshot.Children;
							Func<ChildStatus, bool> func;
							if ((func = CS$<>8__locals2.<>9__5) == null)
							{
								DevicePollingService.<>c__DisplayClass19_1 CS$<>8__locals4 = CS$<>8__locals2;
								Func<ChildStatus, bool> func2 = (ChildStatus c) => string.Equals(c.Kind, CS$<>8__locals2.kind, StringComparison.OrdinalIgnoreCase);
								CS$<>8__locals4.<>9__5 = func2;
								func = func2;
							}
							using (IEnumerator<ChildStatus> enumerator2 = children.Where<ChildStatus>(func).GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									ChildStatus child = enumerator2.Current;
									DeviceTemplate deviceTemplate = DevicePollingService.PickTemplate(templates, cat.Id, child.Model);
									if (deviceTemplate != null)
									{
										Device device = CS$<>8__locals2.CS$<>8__locals1.parent;
										string kind = CS$<>8__locals2.kind;
										if (kind == "ru" || kind == "eu")
										{
											string text = RouteAddr.Parent(child.RouteAddr);
											Device device2;
											if (text != null && dictionary3.TryGetValue(text, out device2))
											{
												device = device2;
											}
											else
											{
												string text2 = RouteAddr.Port(child.RouteAddr);
												Device device3;
												if (text2 != null && dictionary2.TryGetValue(text2, out device3))
												{
													device = device3;
												}
											}
										}
										Device device4 = null;
										if (!string.IsNullOrWhiteSpace(child.SerialNumber))
										{
											dictionary.TryGetValue(child.SerialNumber, out device4);
										}
										else
										{
											device4 = list2.FirstOrDefault<Device>(delegate(Device d)
											{
												if (d.CategoryId == cat.Id)
												{
													int? parentDeviceId = d.ParentDeviceId;
													int id = CS$<>8__locals2.CS$<>8__locals1.parent.Id;
													if ((parentDeviceId.GetValueOrDefault() == id) & (parentDeviceId != null))
													{
														return string.Equals(d.ChildIdentifier, child.Identifier, StringComparison.OrdinalIgnoreCase);
													}
												}
												return false;
											});
										}
										if (device4 == null)
										{
											device4 = new Device
											{
												Name = DevicePollingService.MakeName(CS$<>8__locals2.kind, child),
												CategoryId = cat.Id,
												TemplateId = deviceTemplate.Id,
												CollectionMethod = CollectionMethod.ViaParent,
												SerialNumber = child.SerialNumber,
												ChildIdentifier = (child.SerialNumber ?? child.Identifier),
												ParentDevice = device,
												Enabled = true,
												PollIntervalSeconds = CS$<>8__locals2.CS$<>8__locals1.parent.PollIntervalSeconds,
												CreatedAt = DateTime.UtcNow,
												Notes = ((CS$<>8__locals2.kind == "ue") ? "Auto-discovered from the 5G core." : "Auto-discovered from BBU topology.")
											};
											db.Devices.Add(device4);
											created.Add(device4);
											list2.Add(device4);
											if (!string.IsNullOrWhiteSpace(device4.SerialNumber))
											{
												dictionary[device4.SerialNumber] = device4;
											}
										}
										else
										{
											if (string.IsNullOrWhiteSpace(device4.SerialNumber))
											{
												device4.SerialNumber = child.SerialNumber;
											}
											if (device4.ParentDeviceId == null && device4.ParentDevice == null)
											{
												device4.ParentDevice = device;
											}
											if (string.IsNullOrWhiteSpace(device4.ChildIdentifier))
											{
												device4.ChildIdentifier = child.SerialNumber ?? child.Identifier;
											}
										}
										string text3 = RouteAddr.Norm(child.RouteAddr);
										if (text3 != null)
										{
											dictionary3[text3] = device4;
										}
										if (CS$<>8__locals2.kind == "au")
										{
											string text4 = RouteAddr.Port(child.RouteAddr);
											if (text4 != null)
											{
												dictionary2[text4] = device4;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (db.ChangeTracker.HasChanges())
			{
				await db.SaveChangesAsync(ct);
			}
			return created;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000082E0 File Offset: 0x000064E0
		[NullableContext(2)]
		private static DeviceTemplate PickTemplate([Nullable(1)] List<DeviceTemplate> templates, int categoryId, string model)
		{
			List<DeviceTemplate> list = templates.Where<DeviceTemplate>((DeviceTemplate t) => t.CategoryId == categoryId).ToList<DeviceTemplate>();
			if (!string.IsNullOrWhiteSpace(model))
			{
				DeviceTemplate deviceTemplate = list.FirstOrDefault<DeviceTemplate>((DeviceTemplate t) => string.Equals(t.Model, model, StringComparison.OrdinalIgnoreCase));
				if (deviceTemplate != null)
				{
					return deviceTemplate;
				}
			}
			DeviceTemplate deviceTemplate2;
			if ((deviceTemplate2 = list.FirstOrDefault<DeviceTemplate>((DeviceTemplate t) => !t.IsGeneric && t.CollectionMethod == CollectionMethod.ViaParent)) == null)
			{
				deviceTemplate2 = list.FirstOrDefault<DeviceTemplate>((DeviceTemplate t) => t.CollectionMethod == CollectionMethod.ViaParent) ?? list.FirstOrDefault<DeviceTemplate>();
			}
			return deviceTemplate2;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008398 File Offset: 0x00006598
		private static string MakeName(string kind, ChildStatus child)
		{
			if (kind == "cell")
			{
				return child.Identifier;
			}
			if (kind == "ue")
			{
				Metric metric = child.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "MSIN");
				string text = ((metric != null) ? metric.Value : null);
				return ("UE " + (text ?? child.Identifier)).Trim();
			}
			string text2 = child.Model ?? kind.ToUpperInvariant();
			string serialNumber = child.SerialNumber;
			string text3;
			if (serialNumber == null || serialNumber.Length <= 5)
			{
				text3 = child.SerialNumber ?? child.Identifier;
			}
			else
			{
				string text4 = serialNumber;
				text3 = text4.Substring(text4.Length - 5);
			}
			string text5 = text3;
			return (text2 + " " + text5).Trim();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000846C File Offset: 0x0000666C
		private static void Apply(ApplicationDbContext db, Device device, PollOutcome outcome, DateTime when)
		{
			string text = SnapshotSerializer.Serialize(outcome.Snapshot);
			device.LastPolledAt = new DateTime?(when);
			device.LastReachable = new bool?(outcome.Reachable);
			device.LastError = outcome.Error;
			device.LastSnapshotJson = text;
			db.DevicePollResults.Add(new DevicePollResult
			{
				DeviceId = device.Id,
				Timestamp = when,
				Reachable = outcome.Reachable,
				LatencyMs = outcome.LatencyMs,
				Error = outcome.Error,
				SnapshotJson = text
			});
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008504 File Offset: 0x00006704
		private static async Task RecordThroughputAsync(ApplicationDbContext db, List<Device> devices, List<int> polledIds, DateTime now, CancellationToken ct)
		{
			HashSet<int> polled = polledIds.ToHashSet<int>();
			if (devices.Any<Device>(delegate(Device d)
			{
				DeviceCategory category = d.Category;
				return ((category != null) ? category.Key : null) == "mobile" && polled.Contains(d.Id);
			}))
			{
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				foreach (Device device in devices.Where<Device>(delegate(Device d)
				{
					DeviceCategory category2 = d.Category;
					return ((category2 != null) ? category2.Key : null) == "mobile" && d.LastReachable.GetValueOrDefault();
				}))
				{
					DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(device.LastSnapshotJson);
					if (deviceSnapshot != null)
					{
						num += DevicePollingService.SnapshotMbps(deviceSnapshot, "Total DL");
						num2 += DevicePollingService.SnapshotMbps(deviceSnapshot, "Total UL");
						flag = true;
					}
				}
				if (flag)
				{
					db.ThroughputSamples.Add(new ThroughputSample
					{
						Timestamp = now,
						DlMbps = num,
						UlMbps = num2
					});
					await db.SaveChangesAsync(ct);
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008568 File Offset: 0x00006768
		private static double SnapshotMbps(DeviceSnapshot snap, string label)
		{
			Metric metric = snap.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == label);
			double num;
			if (!MetricHistory.TryParseNumeric((metric != null) ? metric.Value : null, out num))
			{
				return 0.0;
			}
			return num;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000085BC File Offset: 0x000067BC
		private async Task TrimHistoryAsync(ApplicationDbContext db, List<int> deviceIds, CancellationToken ct)
		{
			DevicePollingService.<>c__DisplayClass25_0 CS$<>8__locals1 = new DevicePollingService.<>c__DisplayClass25_0();
			CS$<>8__locals1.deviceIds = deviceIds;
			CS$<>8__locals1.horizon = DateTime.UtcNow - TimeSpan.FromHours(Math.Max(1, this._options.HistoryHours));
			IQueryable<DevicePollResult> devicePollResults = db.DevicePollResults;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
			await devicePollResults.Where<DevicePollResult>(Expression.Lambda<Func<DevicePollResult, bool>>(Expression.AndAlso(Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DevicePollingService.<>c__DisplayClass25_0)), fieldof(DevicePollingService.<>c__DisplayClass25_0.deviceIds)), methodof(List<int>.Contains(T)), new Expression[] { Expression.Property(parameterExpression, methodof(DevicePollResult.get_DeviceId())) }), Expression.LessThan(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DevicePollingService.<>c__DisplayClass25_0)), fieldof(DevicePollingService.<>c__DisplayClass25_0.horizon)), false, methodof(DateTime.op_LessThan(DateTime, DateTime)))), new ParameterExpression[] { parameterExpression })).ExecuteDeleteAsync<DevicePollResult>(ct);
			IQueryable<ThroughputSample> throughputSamples = db.ThroughputSamples;
			parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
			await throughputSamples.Where<ThroughputSample>(Expression.Lambda<Func<ThroughputSample, bool>>(Expression.LessThan(Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(DevicePollingService.<>c__DisplayClass25_0)), fieldof(DevicePollingService.<>c__DisplayClass25_0.horizon)), false, methodof(DateTime.op_LessThan(DateTime, DateTime))), new ParameterExpression[] { parameterExpression })).ExecuteDeleteAsync<ThroughputSample>(ct);
			int keep = Math.Max(10, this._options.HistoryPerDevice);
			using (List<int>.Enumerator enumerator = CS$<>8__locals1.deviceIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DevicePollingService.<>c__DisplayClass25_1 CS$<>8__locals2 = new DevicePollingService.<>c__DisplayClass25_1();
					CS$<>8__locals2.id = enumerator.Current;
					DevicePollingService.<>c__DisplayClass25_2 CS$<>8__locals3 = new DevicePollingService.<>c__DisplayClass25_2();
					CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
					IQueryable<DevicePollResult> devicePollResults2 = db.DevicePollResults;
					parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
					IQueryable<DevicePollResult> queryable = devicePollResults2.Where<DevicePollResult>(Expression.Lambda<Func<DevicePollResult, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(DevicePollResult.get_DeviceId())), Expression.Field(Expression.Field(Expression.Constant(CS$<>8__locals3, typeof(DevicePollingService.<>c__DisplayClass25_2)), fieldof(DevicePollingService.<>c__DisplayClass25_2.CS$<>8__locals1)), fieldof(DevicePollingService.<>c__DisplayClass25_1.id))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
					IQueryable<DevicePollResult> queryable2 = queryable.OrderByDescending<DevicePollResult, DateTime>(Expression.Lambda<Func<DevicePollResult, DateTime>>(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), new ParameterExpression[] { parameterExpression })).Skip<DevicePollResult>(keep);
					parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
					DateTime? dateTime = await queryable2.Select<DevicePollResult, DateTime?>(Expression.Lambda<Func<DevicePollResult, DateTime?>>(Expression.Convert(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), typeof(DateTime?)), new ParameterExpression[] { parameterExpression })).FirstOrDefaultAsync<DateTime?>(ct);
					if (dateTime != null)
					{
						CS$<>8__locals3.c = dateTime.GetValueOrDefault();
						IQueryable<DevicePollResult> devicePollResults3 = db.DevicePollResults;
						parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
						await devicePollResults3.Where<DevicePollResult>(Expression.Lambda<Func<DevicePollResult, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(parameterExpression, methodof(DevicePollResult.get_DeviceId())), Expression.Field(Expression.Field(Expression.Constant(CS$<>8__locals3, typeof(DevicePollingService.<>c__DisplayClass25_2)), fieldof(DevicePollingService.<>c__DisplayClass25_2.CS$<>8__locals1)), fieldof(DevicePollingService.<>c__DisplayClass25_1.id))), Expression.LessThanOrEqual(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals3, typeof(DevicePollingService.<>c__DisplayClass25_2)), fieldof(DevicePollingService.<>c__DisplayClass25_2.c)), false, methodof(DateTime.op_LessThanOrEqual(DateTime, DateTime)))), new ParameterExpression[] { parameterExpression })).ExecuteDeleteAsync<DevicePollResult>(ct);
					}
					CS$<>8__locals3 = null;
				}
			}
			List<int>.Enumerator enumerator = default(List<int>.Enumerator);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008618 File Offset: 0x00006818
		[CompilerGenerated]
		internal static void <OverlayLiveRates>g__Replace|13_0(List<Metric> metrics, string label, double mbps)
		{
			int num = metrics.FindIndex((Metric m) => m.Label == label);
			if (num < 0)
			{
				return;
			}
			int num2 = num;
			Metric metric = metrics[num].<Clone>$();
			metric.Value = mbps.ToString("0.##", CultureInfo.InvariantCulture);
			metrics[num2] = metric;
		}

		// Token: 0x040000A3 RID: 163
		private readonly IServiceScopeFactory _scopeFactory;

		// Token: 0x040000A4 RID: 164
		private readonly IReadOnlyDictionary<CollectionMethod, IDeviceCollector> _collectors;

		// Token: 0x040000A5 RID: 165
		private readonly PollingOptions _options;

		// Token: 0x040000A6 RID: 166
		private readonly RhubSerialClient _serial;

		// Token: 0x040000A7 RID: 167
		private readonly CredentialProtector _creds;

		// Token: 0x040000A8 RID: 168
		private readonly BbuSessionLock _bbuSessions;

		// Token: 0x040000A9 RID: 169
		private readonly CoreLiveState _live;

		// Token: 0x040000AA RID: 170
		private readonly ILogger<DevicePollingService> _log;
	}
}
