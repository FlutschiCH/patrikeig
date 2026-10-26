using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Security;

namespace AbsolutDashboard.Web.Services.Collectors.Core
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoreN6Writer
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x000127BC File Offset: 0x000109BC
		public CoreN6Writer(CredentialProtector creds, ILogger<CoreN6Writer> log)
		{
			this._creds = creds;
			this._log = log;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000127D4 File Offset: 0x000109D4
		public Task<CoreN6Outcome> ReadAsync(Device core, CancellationToken ct)
		{
			return this.RunAsync(core, null, ct);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000127F2 File Offset: 0x000109F2
		public Task<CoreN6Outcome> ApplyAsync(Device core, string address, string mask, string gateway, CancellationToken ct)
		{
			return this.RunAsync(core, new ValueTuple<string, string, string>?(new ValueTuple<string, string, string>(address, mask, gateway)), ct);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001280C File Offset: 0x00010A0C
		[NullableContext(2)]
		public static string LocalAddressToward(string coreAddress)
		{
			string text;
			try
			{
				IPAddress ipaddress;
				if (!IPAddress.TryParse(coreAddress, out ipaddress))
				{
					text = null;
				}
				else
				{
					using (UdpClient udpClient = new UdpClient(ipaddress.AddressFamily))
					{
						udpClient.Connect(ipaddress, 22);
						IPEndPoint ipendPoint = udpClient.Client.LocalEndPoint as IPEndPoint;
						text = ((ipendPoint != null) ? ipendPoint.Address.ToString() : null);
					}
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001288C File Offset: 0x00010A8C
		private async Task<CoreN6Outcome> RunAsync(Device core, [TupleElementNames(new string[] { "Address", "Mask", "Gateway" })] [Nullable(new byte[] { 0, 1, 1, 1 })] ValueTuple<string, string, string>? change, CancellationToken ct)
		{
			CoreN6Writer.<>c__DisplayClass9_0 CS$<>8__locals1 = new CoreN6Writer.<>c__DisplayClass9_0();
			CS$<>8__locals1.core = core;
			CS$<>8__locals1.ct = ct;
			CS$<>8__locals1.change = change;
			CS$<>8__locals1.<>4__this = this;
			CoreN6Outcome coreN6Outcome;
			if (string.IsNullOrWhiteSpace(CS$<>8__locals1.core.IpAddress))
			{
				coreN6Outcome = CoreN6Outcome.Failed("No IP address configured.");
			}
			else if (string.IsNullOrWhiteSpace(CS$<>8__locals1.core.SshUsername))
			{
				coreN6Outcome = CoreN6Outcome.Failed("No SSH username configured.");
			}
			else
			{
				CS$<>8__locals1.password = this._creds.Unprotect(CS$<>8__locals1.core.SshPasswordEnc);
				if (string.IsNullOrEmpty(CS$<>8__locals1.password))
				{
					coreN6Outcome = CoreN6Outcome.Failed("No SSH password configured.");
				}
				else
				{
					CoreN6Writer.<>c__DisplayClass9_0 CS$<>8__locals2 = CS$<>8__locals1;
					DeviceTemplate template = CS$<>8__locals1.core.Template;
					CS$<>8__locals2.def = TemplateDefinition.Parse((template != null) ? template.DefinitionJson : null).Ssh ?? new SshDefinition();
					CS$<>8__locals1.port = ((CS$<>8__locals1.core.Port > 0) ? CS$<>8__locals1.core.Port : 22);
					CS$<>8__locals1.steps = new List<CoreStep>();
					try
					{
						coreN6Outcome = await Task.Run<CoreN6Outcome>(delegate
						{
							ConnectionInfo connectionInfo = new ConnectionInfo(CS$<>8__locals1.core.IpAddress, CS$<>8__locals1.port, CS$<>8__locals1.core.SshUsername, new AuthenticationMethod[]
							{
								new PasswordAuthenticationMethod(CS$<>8__locals1.core.SshUsername, CS$<>8__locals1.password)
							});
							connectionInfo.Timeout = TimeSpan.FromSeconds(8L);
							SshScriptCollector.RestrictAlgorithms<Func<byte[], KeyHostAlgorithm>>(connectionInfo.HostKeyAlgorithms, CS$<>8__locals1.def.HostKeyAlgorithms);
							SshScriptCollector.RestrictAlgorithms<Func<IKeyExchange>>(connectionInfo.KeyExchangeAlgorithms, CS$<>8__locals1.def.KeyExchangeAlgorithms);
							SshScriptCollector.RestrictAlgorithms<CipherInfo>(connectionInfo.Encryptions, CS$<>8__locals1.def.Encryptions);
							CoreN6Outcome coreN6Outcome2;
							using (SshClient sshClient = new SshClient(connectionInfo))
							{
								sshClient.Connect();
								CoreN6Writer.<>c__DisplayClass9_1 CS$<>8__locals3;
								CS$<>8__locals3.shell = sshClient.CreateShellStream("vt100", 200U, 50U, 800U, 600U, 16384);
								try
								{
									CS$<>8__locals3.shell.Expect(CoreN6Writer.Prompt, TimeSpan.FromSeconds(8L), -1);
									base.<RunAsync>g__Send|2("screen-length disable", ref CS$<>8__locals3);
									base.<RunAsync>g__SendOrNull|1("undo terminal monitor", ref CS$<>8__locals3);
									string text = base.<RunAsync>g__Send|2("display current-configuration interface", ref CS$<>8__locals3);
									string text2 = base.<RunAsync>g__Send|2("display current-configuration | include route-static", ref CS$<>8__locals3);
									string text3 = base.<RunAsync>g__Send|2("display nat address-group", ref CS$<>8__locals3);
									N6Interface n6Interface = CoreN6Config.ParseInterface(text);
									if (n6Interface == null)
									{
										coreN6Outcome2 = new CoreN6Outcome(false, "Could not find a routed port with an address in the core's configuration.", null, CS$<>8__locals1.steps);
									}
									else
									{
										ValueTuple<IReadOnlyList<string>, IReadOnlyList<IpWithMask>> valueTuple = CoreN6Config.ParseStaticRoutes(text2);
										IReadOnlyList<string> item = valueTuple.Item1;
										IReadOnlyList<IpWithMask> item2 = valueTuple.Item2;
										N6State n6State = new N6State(n6Interface, item, item2, CoreN6Config.ParseNatGroups(text3));
										if (CS$<>8__locals1.change != null)
										{
											ValueTuple<string, string, string> valueOrDefault = CS$<>8__locals1.change.GetValueOrDefault();
											List<string> list = CoreN6Config.Validate(n6State, valueOrDefault.Item1, valueOrDefault.Item2, valueOrDefault.Item3);
											string text4;
											if (list.Count > 0)
											{
												coreN6Outcome2 = new CoreN6Outcome(false, string.Join(" ", list), n6State, CS$<>8__locals1.steps);
											}
											else if (CoreN6Config.ManagementAtRisk(n6State, CoreN6Writer.LocalAddressToward(CS$<>8__locals1.core.IpAddress), out text4))
											{
												coreN6Outcome2 = new CoreN6Outcome(false, text4, n6State, CS$<>8__locals1.steps);
											}
											else
											{
												foreach (CliStep cliStep in CoreN6Config.BuildSteps(n6State, valueOrDefault.Item1, valueOrDefault.Item2, valueOrDefault.Item3))
												{
													string text5 = base.<RunAsync>g__SendOrNull|1(cliStep.Command, ref CS$<>8__locals3);
													bool flag = text5 == null;
													bool flag2 = !flag && !CoreN6Config.IsError(text5);
													string text6 = (flag ? "no reply from the core (timed out)" : (flag2 ? text5.Trim() : CoreN6Config.ErrorText(text5)));
													CS$<>8__locals1.steps.Add(new CoreStep(cliStep.Label, cliStep.Command, flag2, text6));
													if (!flag2)
													{
														CS$<>8__locals1.<>4__this._log.LogWarning("5G core {Device}: {Command} failed: {Reply}", new object[]
														{
															CS$<>8__locals1.core.Name,
															cliStep.Command,
															text6
														});
														sshClient.Disconnect();
														return new CoreN6Outcome(false, "Stopped at \"" + cliStep.Label + "\": " + text6, n6State, CS$<>8__locals1.steps);
													}
													CS$<>8__locals1.<>4__this._log.LogInformation("5G core {Device}: {Command} accepted.", new object[]
													{
														CS$<>8__locals1.core.Name,
														cliStep.Command
													});
												}
												N6Interface n6Interface2 = CoreN6Config.ParseInterface(base.<RunAsync>g__Send|2("display current-configuration interface", ref CS$<>8__locals3));
												ValueTuple<IReadOnlyList<string>, IReadOnlyList<IpWithMask>> valueTuple2 = CoreN6Config.ParseStaticRoutes(base.<RunAsync>g__Send|2("display current-configuration | include route-static", ref CS$<>8__locals3));
												IReadOnlyList<string> item3 = valueTuple2.Item1;
												IReadOnlyList<IpWithMask> item4 = valueTuple2.Item2;
												IReadOnlyDictionary<int, IReadOnlyList<string>> readOnlyDictionary = CoreN6Config.ParseNatGroups(base.<RunAsync>g__Send|2("display nat address-group", ref CS$<>8__locals3));
												sshClient.Disconnect();
												N6State n6State2 = ((n6Interface2 == null) ? n6State : new N6State(n6Interface2, item3, item4, readOnlyDictionary));
												coreN6Outcome2 = new CoreN6Outcome(true, null, n6State2, CS$<>8__locals1.steps);
											}
										}
										else
										{
											sshClient.Disconnect();
											coreN6Outcome2 = new CoreN6Outcome(true, null, n6State, CS$<>8__locals1.steps);
										}
									}
								}
								finally
								{
									if (CS$<>8__locals3.shell != null)
									{
										((IDisposable)CS$<>8__locals3.shell).Dispose();
									}
								}
							}
							return coreN6Outcome2;
						}, CS$<>8__locals1.ct);
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex)
					{
						this._log.LogWarning(ex, "5G core {Device}: N6 session failed.", new object[] { CS$<>8__locals1.core.Name });
						coreN6Outcome = new CoreN6Outcome(false, "SSH error: " + ex.Message, null, CS$<>8__locals1.steps);
					}
				}
			}
			return coreN6Outcome;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000128E8 File Offset: 0x00010AE8
		public static string StripEcho(string reply, string command)
		{
			List<string> list = reply.Replace("\r", "").Split('\n', StringSplitOptions.None).ToList<string>();
			if (list.Count > 0 && list[0].Trim().EndsWith(command, StringComparison.Ordinal))
			{
				list.RemoveAt(0);
			}
			if (list.Count > 0)
			{
				Regex prompt = CoreN6Writer.Prompt;
				List<string> list2 = list;
				if (prompt.IsMatch(list2[list2.Count - 1]))
				{
					list.RemoveAt(list.Count - 1);
				}
			}
			return string.Join<string>('\n', list);
		}

		// Token: 0x040001AA RID: 426
		private readonly CredentialProtector _creds;

		// Token: 0x040001AB RID: 427
		private readonly ILogger<CoreN6Writer> _log;

		// Token: 0x040001AC RID: 428
		private static readonly Regex Prompt = new Regex("^\\s*[<\\[][^\\s<>\\[\\]]+[>\\]]\\s*$", RegexOptions.Multiline | RegexOptions.Compiled);

		// Token: 0x040001AD RID: 429
		private static readonly TimeSpan CommandWait = TimeSpan.FromSeconds(15L);

		// Token: 0x040001AE RID: 430
		private static readonly TimeSpan SaveWait = TimeSpan.FromSeconds(60L);
	}
}
