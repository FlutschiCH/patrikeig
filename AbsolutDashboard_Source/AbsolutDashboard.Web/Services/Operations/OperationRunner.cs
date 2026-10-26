using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Bbu;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Security;

namespace AbsolutDashboard.Web.Services.Operations
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OperationRunner
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00005C70 File Offset: 0x00003E70
		public OperationRunner(IDbContextFactory<ApplicationDbContext> db, BbuConfigWriter bbu, CredentialProtector creds, RhubSerialClient serial, IOptions<OperationsOptions> opts, IOptions<PollingOptions> polling, ILogger<OperationRunner> log)
		{
			this._db = db;
			this._bbu = bbu;
			this._creds = creds;
			this._serial = serial;
			this._opts = opts.Value;
			this._polling = polling.Value;
			this._log = log;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005CC2 File Offset: 0x00003EC2
		public ShutdownAllOptions ShutdownAll
		{
			get
			{
				return this._opts.ShutdownAll;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005CCF File Offset: 0x00003ECF
		public PrruPowerOptions PrruPower
		{
			get
			{
				return this._opts.PrruPower;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005CDC File Offset: 0x00003EDC
		public async Task<List<OperationResult>> RunShutdownAllAsync(CancellationToken ct)
		{
			List<OperationResult> results = new List<OperationResult>();
			List<OperationResult> list;
			if (!this.ShutdownAll.Configured)
			{
				results.Add(new OperationResult("Shutdown", "", false, "Not configured."));
				list = results;
			}
			else
			{
				foreach (OperationStep step in this.ShutdownAll.Steps)
				{
					List<OperationResult> list2 = await this.RunStepAsync(step, ct);
					results.AddRange(list2);
					foreach (OperationResult operationResult in list2)
					{
						this._log.Log(operationResult.Ok ? LogLevel.Information : LogLevel.Warning, "Shutdown step {Label}: {Ok} {Detail}", new object[]
						{
							operationResult.Label,
							operationResult.Ok ? "ok" : "FAILED",
							operationResult.Detail
						});
					}
					if (list2.Any<OperationResult>((OperationResult r) => !r.Ok) && !step.ContinueOnError)
					{
						break;
					}
					if (step.WaitSeconds > 0)
					{
						await Task.Delay(TimeSpan.FromSeconds((long)step.WaitSeconds), ct);
					}
					step = null;
				}
				List<OperationStep>.Enumerator enumerator = default(List<OperationStep>.Enumerator);
				list = results;
			}
			return list;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005D28 File Offset: 0x00003F28
		private static string Printable([Nullable(2)] string raw, int max = 600)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return "(empty)";
			}
			return ((raw.Length > max) ? (raw.Substring(0, max) + "…") : raw).Replace("\r", "\\r").Replace("\n", "\\n");
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005D80 File Offset: 0x00003F80
		public async Task<OperationRunner.PrruPowerTarget> DescribePrruPowerAsync(int radioUnitId, CancellationToken ct)
		{
			OperationRunner.<>c__DisplayClass15_1 CS$<>8__locals2 = new OperationRunner.<>c__DisplayClass15_1();
			ApplicationDbContext applicationDbContext = await this._db.CreateDbContextAsync(ct);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			OperationRunner.PrruPowerTarget prruPowerTarget;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				List<Device> list = await devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(ct);
				CS$<>8__locals2.ru = list.FirstOrDefault<Device>((Device d) => d.Id == radioUnitId);
				if (CS$<>8__locals2.ru == null)
				{
					prruPowerTarget = new OperationRunner.PrruPowerTarget(null, null, null, "Radio unit not found.");
				}
				else
				{
					int? num2 = CS$<>8__locals2.ru.ParentDeviceId;
					Device device;
					if (num2 != null)
					{
						CS$<>8__locals2.pid = num2.GetValueOrDefault();
						device = list.FirstOrDefault<Device>(delegate(Device d)
						{
							if (d.Id == CS$<>8__locals2.pid)
							{
								DeviceCategory category2 = d.Category;
								return ((category2 != null) ? category2.Key : null) == "eu";
							}
							return false;
						});
					}
					else
					{
						device = null;
					}
					Device device2 = device;
					if (device2 == null)
					{
						prruPowerTarget = new OperationRunner.PrruPowerTarget(null, null, null, "No extension unit above this PICO sender in the topology, so there is no power board to switch.");
					}
					else if (string.IsNullOrWhiteSpace(device2.SerialPort))
					{
						prruPowerTarget = new OperationRunner.PrruPowerTarget(device2, null, null, "'" + device2.Name + "' has no serial console configured. Its power board is switched over that console (Edit device → serial port).");
					}
					else
					{
						string text = null;
						Device device3 = device2;
						HashSet<int> hashSet = new HashSet<int>
						{
							CS$<>8__locals2.ru.Id,
							device2.Id
						};
						Device device4;
						for (;;)
						{
							num2 = device3.ParentDeviceId;
							if (num2 == null)
							{
								goto IL_03AD;
							}
							int p2 = num2.GetValueOrDefault();
							device4 = list.FirstOrDefault<Device>((Device d) => d.Id == p2);
							if (device4 == null || !hashSet.Add(device4.Id))
							{
								goto IL_03AD;
							}
							DeviceCategory category = device4.Category;
							if (((category != null) ? category.Key : null) == "bbu")
							{
								break;
							}
							device3 = device4;
						}
						DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(device4.LastSnapshotJson);
						string text2;
						if (deviceSnapshot == null)
						{
							text2 = null;
						}
						else
						{
							IEnumerable<ChildStatus> children = deviceSnapshot.Children;
							Func<ChildStatus, bool> func;
							if ((func = CS$<>8__locals2.<>9__5) == null)
							{
								OperationRunner.<>c__DisplayClass15_1 CS$<>8__locals4 = CS$<>8__locals2;
								Func<ChildStatus, bool> func2 = (ChildStatus c) => string.Equals(c.Kind, "ru", StringComparison.OrdinalIgnoreCase) && ((!string.IsNullOrWhiteSpace(CS$<>8__locals2.ru.SerialNumber) && string.Equals(c.SerialNumber, CS$<>8__locals2.ru.SerialNumber, StringComparison.OrdinalIgnoreCase)) || string.Equals(c.Identifier, CS$<>8__locals2.ru.ChildIdentifier, StringComparison.OrdinalIgnoreCase));
								CS$<>8__locals4.<>9__5 = func2;
								func = func2;
							}
							ChildStatus childStatus = children.FirstOrDefault<ChildStatus>(func);
							text2 = ((childStatus != null) ? childStatus.RouteAddr : null);
						}
						text = text2;
						IL_03AD:
						int? num3 = RouteAddr.Leaf(text);
						DeviceSnapshot deviceSnapshot2 = SnapshotSerializer.Deserialize(device2.LastSnapshotJson);
						int? num4;
						if (deviceSnapshot2 == null)
						{
							num4 = null;
						}
						else
						{
							num4 = new int?(deviceSnapshot2.HubPorts.Count<RhubPort>((RhubPort p) => p.Kind == PortKind.Power));
						}
						int? num5 = num4;
						prruPowerTarget = new OperationRunner.PrruPowerTarget(device2, num3, (num5 > 0) ? num5 : null, null);
					}
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			if (db != null)
			{
				await db.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return prruPowerTarget;
			}
			obj = null;
			prruPowerTarget = null;
			CS$<>8__locals2 = null;
			db = null;
			throw null;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005DD4 File Offset: 0x00003FD4
		public async Task<OperationResult> RunPrruPowerAsync(int radioUnitId, bool on, int channel, CancellationToken ct)
		{
			OperationRunner.<>c__DisplayClass16_0 CS$<>8__locals1 = new OperationRunner.<>c__DisplayClass16_0();
			CS$<>8__locals1.channel = channel;
			string label = "PICO sender power " + (on ? "on" : "off");
			OperationResult operationResult;
			if (!this.PrruPower.Enabled)
			{
				operationResult = new OperationResult(label, "", false, "Disabled in the site configuration.");
			}
			else
			{
				OperationRunner.PrruPowerTarget prruPowerTarget = await this.DescribePrruPowerAsync(radioUnitId, ct);
				if (prruPowerTarget.Blocker != null || prruPowerTarget.ExtensionUnit == null)
				{
					operationResult = new OperationResult(label, "", false, prruPowerTarget.Blocker ?? "No extension unit.");
				}
				else
				{
					if (CS$<>8__locals1.channel >= 1)
					{
						int? channelCount = prruPowerTarget.ChannelCount;
						if (channelCount != null)
						{
							int valueOrDefault = channelCount.GetValueOrDefault();
							if (CS$<>8__locals1.channel > valueOrDefault)
							{
								goto IL_0161;
							}
						}
						Device eu = prruPowerTarget.ExtensionUnit;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
						defaultInterpolatedStringHandler.AppendLiteral("pa ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.channel);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted(on ? "on" : "off");
						string command = defaultInterpolatedStringHandler.ToStringAndClear();
						TimeSpan budget = TimeSpan.FromSeconds((long)Math.Max(5, this._polling.SerialTimeoutSeconds));
						string password = this._creds.Unprotect(eu.SshPasswordEnc);
						try
						{
							ValueTuple<string, bool, string, string> valueTuple = await this._serial.SetChannelPowerAsync(eu.SerialPort, eu.SerialBaudRate, eu.SerialFraming, password, CS$<>8__locals1.channel, on, budget, ct);
							string reply = valueTuple.Item1;
							bool item = valueTuple.Item2;
							string item2 = valueTuple.Item3;
							string item3 = valueTuple.Item4;
							this._log.LogWarning("PICO power: '{Command}' on {Eu} ({Port}) -> reply {Reply}; raw {Raw}", new object[]
							{
								command,
								eu.Name,
								eu.SerialPort,
								(reply.Length == 0) ? "(prompt only)" : reply,
								OperationRunner.Printable(item2, 600)
							});
							if (item3 != null)
							{
								operationResult = new OperationResult(label, command, false, "Not sent: " + item3 + ".");
							}
							else if (!item)
							{
								operationResult = new OperationResult(label, command, false, (item2.Length == 0) ? "No answer from the power board." : ("The power board did not return to its prompt: " + reply).TrimEnd(new char[] { ':', ' ' }));
							}
							else
							{
								bool refused = reply.Length > 0 && Regex.IsMatch(reply, "error|invalid|unknown|fail|denied|bad", RegexOptions.IgnoreCase);
								RhubPort rhubPort = null;
								for (int attempt = 0; attempt < 3; attempt++)
								{
									await Task.Delay(TimeSpan.FromSeconds((attempt == 0) ? 2L : 3L), ct);
									IEnumerable<RhubPort> ports = RhubPowerResponseParser.Parse(await this._serial.ReadPowerAsync(eu.SerialPort, eu.SerialBaudRate, eu.SerialFraming, password, budget, ct), new DateTimeOffset?(DateTimeOffset.Now)).Ports;
									Func<RhubPort, bool> func;
									if ((func = CS$<>8__locals1.<>9__0) == null)
									{
										OperationRunner.<>c__DisplayClass16_0 CS$<>8__locals2 = CS$<>8__locals1;
										Func<RhubPort, bool> func2 = (RhubPort p) => p.Index == CS$<>8__locals1.channel;
										CS$<>8__locals2.<>9__0 = func2;
										func = func2;
									}
									rhubPort = ports.FirstOrDefault<RhubPort>(func);
									if (rhubPort == null || (on ? (rhubPort.State == PortState.Ok) : (rhubPort.State > PortState.Ok)))
									{
										break;
									}
								}
								string text = ((reply.Length == 0) ? "the board answered with its prompt" : ("the board answered \"" + reply + "\""));
								if (refused)
								{
									string text2 = label;
									string text3 = command;
									bool flag = false;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(19, 3);
									defaultInterpolatedStringHandler2.AppendFormatted(text);
									defaultInterpolatedStringHandler2.AppendLiteral("; channel ");
									defaultInterpolatedStringHandler2.AppendFormatted<int>(CS$<>8__locals1.channel);
									defaultInterpolatedStringHandler2.AppendLiteral(" reads \"");
									defaultInterpolatedStringHandler2.AppendFormatted(((rhubPort != null) ? rhubPort.RawStatus : null) ?? "not listed");
									defaultInterpolatedStringHandler2.AppendLiteral("\"");
									operationResult = new OperationResult(text2, text3, flag, defaultInterpolatedStringHandler2.ToStringAndClear());
								}
								else
								{
									string text4;
									if (rhubPort != null)
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(21, 2);
										defaultInterpolatedStringHandler3.AppendLiteral("channel ");
										defaultInterpolatedStringHandler3.AppendFormatted<int>(CS$<>8__locals1.channel);
										defaultInterpolatedStringHandler3.AppendLiteral(" now reads \"");
										defaultInterpolatedStringHandler3.AppendFormatted(rhubPort.RawStatus);
										defaultInterpolatedStringHandler3.AppendLiteral("\"");
										text4 = defaultInterpolatedStringHandler3.ToStringAndClear();
									}
									else
									{
										text4 = "the read-back did not list this channel";
									}
									string text5 = text4;
									if (!on && rhubPort != null && rhubPort.State == PortState.Ok)
									{
										operationResult = new OperationResult(label, command, false, text + ", but " + text5 + " — the channel is still powered");
									}
									else
									{
										operationResult = new OperationResult(label, command, true, text + "; " + text5);
									}
								}
							}
						}
						catch (OperationCanceledException)
						{
							throw;
						}
						catch (Exception ex)
						{
							operationResult = new OperationResult(label, command, false, ex.Message);
						}
						return operationResult;
					}
					IL_0161:
					string text6 = label;
					string text7 = "";
					bool flag2 = false;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(56, 2);
					defaultInterpolatedStringHandler4.AppendLiteral("Channel ");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(CS$<>8__locals1.channel);
					defaultInterpolatedStringHandler4.AppendLiteral(" is not one of this power board's channels (1–");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(prruPowerTarget.ChannelCount.GetValueOrDefault(16));
					defaultInterpolatedStringHandler4.AppendLiteral(").");
					operationResult = new OperationResult(text6, text7, flag2, defaultInterpolatedStringHandler4.ToStringAndClear());
				}
			}
			return operationResult;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005E38 File Offset: 0x00004038
		private async Task<List<OperationResult>> RunStepAsync(OperationStep step, CancellationToken ct)
		{
			OperationRunner.<>c__DisplayClass17_0 CS$<>8__locals1 = new OperationRunner.<>c__DisplayClass17_0();
			CS$<>8__locals1.step = step;
			List<OperationResult> list3;
			try
			{
				if (CS$<>8__locals1.step.IsMml)
				{
					List<Device> targets;
					if (string.IsNullOrWhiteSpace(CS$<>8__locals1.step.Device))
					{
						List<Device> list = await this.AllBaseStationsAsync(ct);
						targets = list;
						if (targets.Count == 0)
						{
							return new List<OperationResult>
							{
								new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, "No base station in the dashboard.")
							};
						}
					}
					else
					{
						Device device = await this.FindDeviceAsync(CS$<>8__locals1.step.Device, "bbu", ct);
						if (device == null)
						{
							return new List<OperationResult>
							{
								new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, "No base station named '" + CS$<>8__locals1.step.Device + "'.")
							};
						}
						targets = new List<Device> { device };
					}
					List<OperationResult> results = new List<OperationResult>();
					foreach (Device device2 in targets)
					{
						string label = ((targets.Count > 1) ? (CS$<>8__locals1.step.Label + " (" + device2.Name + ")") : CS$<>8__locals1.step.Label);
						BbuWriteOutcome bbuWriteOutcome = await this._bbu.ApplyAsync(device2, new MmlWrite[]
						{
							new MmlWrite(label, CS$<>8__locals1.step.Command, false)
						}, ct);
						MmlStep mmlStep = bbuWriteOutcome.Steps.FirstOrDefault<MmlStep>();
						List<OperationResult> list2 = results;
						string text = label;
						string command = CS$<>8__locals1.step.Command;
						bool ok = bbuWriteOutcome.Ok;
						string text2;
						if (!bbuWriteOutcome.Ok)
						{
							if ((text2 = bbuWriteOutcome.Error) == null)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
								defaultInterpolatedStringHandler.AppendLiteral("RETCODE ");
								defaultInterpolatedStringHandler.AppendFormatted<int?>((mmlStep != null) ? new int?(mmlStep.RetCode) : null);
								defaultInterpolatedStringHandler.AppendLiteral(" ");
								defaultInterpolatedStringHandler.AppendFormatted((mmlStep != null) ? mmlStep.Message : null);
								text2 = defaultInterpolatedStringHandler.ToStringAndClear();
							}
						}
						else
						{
							text2 = "accepted";
						}
						list2.Add(new OperationResult(text, command, ok, text2));
						label = null;
					}
					List<Device>.Enumerator enumerator = default(List<Device>.Enumerator);
					list3 = results;
				}
				else if (CS$<>8__locals1.step.IsSsh)
				{
					CS$<>8__locals1.def = new SshDefinition();
					if (!string.IsNullOrWhiteSpace(CS$<>8__locals1.step.Device))
					{
						Device device3 = await this.FindDeviceAsync(CS$<>8__locals1.step.Device, null, ct);
						if (device3 == null)
						{
							return new List<OperationResult>
							{
								new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, "No device named '" + CS$<>8__locals1.step.Device + "'.")
							};
						}
						if (string.IsNullOrWhiteSpace(device3.IpAddress) || string.IsNullOrWhiteSpace(device3.SshUsername))
						{
							return new List<OperationResult>
							{
								new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, "'" + device3.Name + "' has no SSH address or user.")
							};
						}
						CS$<>8__locals1.host = device3.IpAddress;
						CS$<>8__locals1.user = device3.SshUsername;
						CS$<>8__locals1.password = this._creds.Unprotect(device3.SshPasswordEnc) ?? "";
						CS$<>8__locals1.port = ((device3.Port > 0) ? device3.Port : 22);
						OperationRunner.<>c__DisplayClass17_0 CS$<>8__locals2 = CS$<>8__locals1;
						DeviceTemplate template = device3.Template;
						CS$<>8__locals2.def = TemplateDefinition.Parse((template != null) ? template.DefinitionJson : null).Ssh ?? new SshDefinition();
					}
					else
					{
						CS$<>8__locals1.host = CS$<>8__locals1.step.Host;
						CS$<>8__locals1.user = CS$<>8__locals1.step.User;
						CS$<>8__locals1.password = CS$<>8__locals1.step.Password ?? "";
						CS$<>8__locals1.port = ((CS$<>8__locals1.step.Port > 0) ? CS$<>8__locals1.step.Port : 22);
					}
					List<OperationResult> list4 = new List<OperationResult>();
					List<OperationResult> results = list4;
					results.Add(await Task.Run<OperationResult>(() => OperationRunner.RunSsh(CS$<>8__locals1.step, CS$<>8__locals1.host, CS$<>8__locals1.port, CS$<>8__locals1.user, CS$<>8__locals1.password, CS$<>8__locals1.def), ct));
					list3 = list4;
				}
				else
				{
					list3 = new List<OperationResult>
					{
						new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, "Unknown step kind '" + CS$<>8__locals1.step.Kind + "'.")
					};
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				list3 = new List<OperationResult>
				{
					new OperationResult(CS$<>8__locals1.step.Label, CS$<>8__locals1.step.Command, false, ex.Message)
				};
			}
			return list3;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005E8C File Offset: 0x0000408C
		private async Task<List<Device>> AllBaseStationsAsync(CancellationToken ct)
		{
			ApplicationDbContext applicationDbContext = await this._db.CreateDbContextAsync(ct);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			List<Device> list;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable3 = queryable2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.Property(parameterExpression, methodof(Device.get_Enabled())), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string)))), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				list = await queryable3.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(ct);
				num = 1;
			}
			catch (object obj)
			{
			}
			if (db != null)
			{
				await db.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return list;
			}
			obj = null;
			list = null;
			db = null;
			throw null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005ED8 File Offset: 0x000040D8
		private static OperationResult RunSsh(OperationStep step, string host, int port, string user, string password, SshDefinition def)
		{
			ConnectionInfo connectionInfo = new ConnectionInfo(host, port, user, new AuthenticationMethod[]
			{
				new PasswordAuthenticationMethod(user, password)
			});
			connectionInfo.Timeout = TimeSpan.FromSeconds(8L);
			SshScriptCollector.RestrictAlgorithms<Func<byte[], KeyHostAlgorithm>>(connectionInfo.HostKeyAlgorithms, def.HostKeyAlgorithms);
			SshScriptCollector.RestrictAlgorithms<Func<IKeyExchange>>(connectionInfo.KeyExchangeAlgorithms, def.KeyExchangeAlgorithms);
			SshScriptCollector.RestrictAlgorithms<CipherInfo>(connectionInfo.Encryptions, def.Encryptions);
			OperationResult operationResult;
			using (SshClient sshClient = new SshClient(connectionInfo))
			{
				sshClient.Connect();
				try
				{
					using (SshCommand sshCommand = sshClient.CreateCommand(step.Command))
					{
						sshCommand.CommandTimeout = TimeSpan.FromSeconds(60L);
						string text = sshCommand.Execute();
						string text2 = (string.IsNullOrWhiteSpace(text) ? sshCommand.Error : text);
						bool flag = (sshCommand.ExitStatus ?? 0) == 0;
						bool flag2 = flag;
						string label = step.Label;
						string command = step.Command;
						bool flag3 = flag2;
						string text3 = (text2 ?? "").Trim();
						string text4;
						if (text3 == null || text3.Length <= 0)
						{
							if (!flag2)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
								defaultInterpolatedStringHandler.AppendLiteral("exit ");
								defaultInterpolatedStringHandler.AppendFormatted<int?>(sshCommand.ExitStatus);
								text4 = defaultInterpolatedStringHandler.ToStringAndClear();
							}
							else
							{
								text4 = "done";
							}
						}
						else
						{
							text4 = text3;
						}
						operationResult = new OperationResult(label, command, flag3, text4);
					}
				}
				catch (SshConnectionException)
				{
					operationResult = new OperationResult(step.Label, step.Command, true, "connection closed by the host (shut down)");
				}
				finally
				{
					try
					{
						sshClient.Disconnect();
					}
					catch
					{
					}
				}
			}
			return operationResult;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000060D0 File Offset: 0x000042D0
		[return: Nullable(new byte[] { 1, 2 })]
		private async Task<Device> FindDeviceAsync(string name, [Nullable(2)] string categoryKey, CancellationToken ct)
		{
			OperationRunner.<>c__DisplayClass20_0 CS$<>8__locals1 = new OperationRunner.<>c__DisplayClass20_0();
			CS$<>8__locals1.name = name;
			CS$<>8__locals1.categoryKey = categoryKey;
			ApplicationDbContext applicationDbContext = await this._db.CreateDbContextAsync(ct);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			Device device;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable3 = queryable2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Name())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(OperationRunner.<>c__DisplayClass20_0)), fieldof(OperationRunner.<>c__DisplayClass20_0.name))), new ParameterExpression[] { parameterExpression }));
				if (CS$<>8__locals1.categoryKey != null)
				{
					IQueryable<Device> queryable4 = queryable3;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					queryable3 = queryable4.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(OperationRunner.<>c__DisplayClass20_0)), fieldof(OperationRunner.<>c__DisplayClass20_0.categoryKey))), new ParameterExpression[] { parameterExpression }));
				}
				device = await queryable3.FirstOrDefaultAsync<Device>(ct);
				num = 1;
			}
			catch (object obj)
			{
			}
			if (db != null)
			{
				await db.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num == 1)
			{
				return device;
			}
			obj = null;
			device = null;
			db = null;
			throw null;
		}

		// Token: 0x0400006E RID: 110
		private readonly IDbContextFactory<ApplicationDbContext> _db;

		// Token: 0x0400006F RID: 111
		private readonly BbuConfigWriter _bbu;

		// Token: 0x04000070 RID: 112
		private readonly CredentialProtector _creds;

		// Token: 0x04000071 RID: 113
		private readonly RhubSerialClient _serial;

		// Token: 0x04000072 RID: 114
		private readonly OperationsOptions _opts;

		// Token: 0x04000073 RID: 115
		private readonly PollingOptions _polling;

		// Token: 0x04000074 RID: 116
		private readonly ILogger<OperationRunner> _log;

		// Token: 0x020000D1 RID: 209
		[NullableContext(2)]
		[Nullable(0)]
		public sealed class PrruPowerTarget : IEquatable<OperationRunner.PrruPowerTarget>
		{
			// Token: 0x060008AD RID: 2221 RVA: 0x00033BB7 File Offset: 0x00031DB7
			public PrruPowerTarget(Device ExtensionUnit, int? DefaultChannel, int? ChannelCount, string Blocker)
			{
				this.ExtensionUnit = ExtensionUnit;
				this.DefaultChannel = DefaultChannel;
				this.ChannelCount = ChannelCount;
				this.Blocker = Blocker;
				base..ctor();
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x060008AE RID: 2222 RVA: 0x00033BDC File Offset: 0x00031DDC
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(OperationRunner.PrruPowerTarget);
				}
			}

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x060008AF RID: 2223 RVA: 0x00033BE8 File Offset: 0x00031DE8
			// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00033BF0 File Offset: 0x00031DF0
			public Device ExtensionUnit { get; set; }

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00033BF9 File Offset: 0x00031DF9
			// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00033C01 File Offset: 0x00031E01
			public int? DefaultChannel { get; set; }

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00033C0A File Offset: 0x00031E0A
			// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00033C12 File Offset: 0x00031E12
			public int? ChannelCount { get; set; }

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00033C1B File Offset: 0x00031E1B
			// (set) Token: 0x060008B6 RID: 2230 RVA: 0x00033C23 File Offset: 0x00031E23
			public string Blocker { get; set; }

			// Token: 0x060008B7 RID: 2231 RVA: 0x00033C2C File Offset: 0x00031E2C
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PrruPowerTarget");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x00033C78 File Offset: 0x00031E78
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("ExtensionUnit = ");
				builder.Append(this.ExtensionUnit);
				builder.Append(", DefaultChannel = ");
				builder.Append(this.DefaultChannel.ToString());
				builder.Append(", ChannelCount = ");
				builder.Append(this.ChannelCount.ToString());
				builder.Append(", Blocker = ");
				builder.Append(this.Blocker);
				return true;
			}

			// Token: 0x060008B9 RID: 2233 RVA: 0x00033D0B File Offset: 0x00031F0B
			[CompilerGenerated]
			public static bool operator !=(OperationRunner.PrruPowerTarget left, OperationRunner.PrruPowerTarget right)
			{
				return !(left == right);
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x00033D17 File Offset: 0x00031F17
			[CompilerGenerated]
			public static bool operator ==(OperationRunner.PrruPowerTarget left, OperationRunner.PrruPowerTarget right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x00033D2C File Offset: 0x00031F2C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Device>.Default.GetHashCode(this.<ExtensionUnit>k__BackingField)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.<DefaultChannel>k__BackingField)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.<ChannelCount>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Blocker>k__BackingField);
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x00033DA5 File Offset: 0x00031FA5
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as OperationRunner.PrruPowerTarget);
			}

			// Token: 0x060008BD RID: 2237 RVA: 0x00033DB4 File Offset: 0x00031FB4
			[CompilerGenerated]
			public bool Equals(OperationRunner.PrruPowerTarget other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Device>.Default.Equals(this.<ExtensionUnit>k__BackingField, other.<ExtensionUnit>k__BackingField) && EqualityComparer<int?>.Default.Equals(this.<DefaultChannel>k__BackingField, other.<DefaultChannel>k__BackingField) && EqualityComparer<int?>.Default.Equals(this.<ChannelCount>k__BackingField, other.<ChannelCount>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Blocker>k__BackingField, other.<Blocker>k__BackingField));
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x00033E45 File Offset: 0x00032045
			[CompilerGenerated]
			private PrruPowerTarget([Nullable(1)] OperationRunner.PrruPowerTarget original)
			{
				this.ExtensionUnit = original.<ExtensionUnit>k__BackingField;
				this.DefaultChannel = original.<DefaultChannel>k__BackingField;
				this.ChannelCount = original.<ChannelCount>k__BackingField;
				this.Blocker = original.<Blocker>k__BackingField;
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x00033E7D File Offset: 0x0003207D
			[CompilerGenerated]
			public void Deconstruct(out Device ExtensionUnit, out int? DefaultChannel, out int? ChannelCount, out string Blocker)
			{
				ExtensionUnit = this.ExtensionUnit;
				DefaultChannel = this.DefaultChannel;
				ChannelCount = this.ChannelCount;
				Blocker = this.Blocker;
			}
		}
	}
}
