using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Security;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SshScriptCollector : IDeviceCollector
	{
		// Token: 0x06000288 RID: 648 RVA: 0x0000D225 File Offset: 0x0000B425
		public SshScriptCollector(CredentialProtector creds, SshParserRegistry parsers, ILogger<SshScriptCollector> log)
		{
			this._creds = creds;
			this._parsers = parsers;
			this._log = log;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D242 File Offset: 0x0000B442
		public CollectionMethod Method
		{
			get
			{
				return CollectionMethod.SshScript;
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000D248 File Offset: 0x0000B448
		public async Task<PollOutcome> CollectAsync(Device device, CancellationToken ct)
		{
			SshScriptCollector.<>c__DisplayClass7_0 CS$<>8__locals1 = new SshScriptCollector.<>c__DisplayClass7_0();
			CS$<>8__locals1.device = device;
			CS$<>8__locals1.ct = ct;
			CS$<>8__locals1.<>4__this = this;
			PollOutcome pollOutcome;
			if (string.IsNullOrWhiteSpace(CS$<>8__locals1.device.IpAddress))
			{
				pollOutcome = PollOutcome.Unreachable("No IP address configured.");
			}
			else if (string.IsNullOrWhiteSpace(CS$<>8__locals1.device.SshUsername))
			{
				pollOutcome = PollOutcome.Unreachable("No SSH username configured.");
			}
			else
			{
				CS$<>8__locals1.password = this._creds.Unprotect(CS$<>8__locals1.device.SshPasswordEnc);
				if (string.IsNullOrEmpty(CS$<>8__locals1.password))
				{
					pollOutcome = PollOutcome.Unreachable("No SSH password configured.");
				}
				else
				{
					SshScriptCollector.<>c__DisplayClass7_0 CS$<>8__locals2 = CS$<>8__locals1;
					DeviceTemplate template = CS$<>8__locals1.device.Template;
					CS$<>8__locals2.def = TemplateDefinition.Parse((template != null) ? template.DefinitionJson : null).Ssh ?? new SshDefinition();
					if (CS$<>8__locals1.def.Commands.Count == 0)
					{
						pollOutcome = PollOutcome.Unreachable("No SSH commands defined for this device template.");
					}
					else
					{
						CS$<>8__locals1.port = ((CS$<>8__locals1.device.Port > 0) ? CS$<>8__locals1.device.Port : 22);
						CS$<>8__locals1.sw = Stopwatch.StartNew();
						try
						{
							pollOutcome = await Task.Run<PollOutcome>(delegate
							{
								ConnectionInfo connectionInfo = new ConnectionInfo(CS$<>8__locals1.device.IpAddress, CS$<>8__locals1.port, CS$<>8__locals1.device.SshUsername, new AuthenticationMethod[]
								{
									new PasswordAuthenticationMethod(CS$<>8__locals1.device.SshUsername, CS$<>8__locals1.password)
								});
								connectionInfo.Timeout = TimeSpan.FromSeconds(8L);
								SshScriptCollector.RestrictAlgorithms<Func<byte[], KeyHostAlgorithm>>(connectionInfo.HostKeyAlgorithms, CS$<>8__locals1.def.HostKeyAlgorithms);
								SshScriptCollector.RestrictAlgorithms<Func<IKeyExchange>>(connectionInfo.KeyExchangeAlgorithms, CS$<>8__locals1.def.KeyExchangeAlgorithms);
								SshScriptCollector.RestrictAlgorithms<CipherInfo>(connectionInfo.Encryptions, CS$<>8__locals1.def.Encryptions);
								PollOutcome pollOutcome2;
								using (SshClient sshClient = new SshClient(connectionInfo))
								{
									sshClient.Connect();
									Dictionary<string, string> dictionary = (CS$<>8__locals1.def.Interactive ? SshScriptCollector.RunInteractive(sshClient, CS$<>8__locals1.def, CS$<>8__locals1.ct) : SshScriptCollector.RunExec(sshClient, CS$<>8__locals1.def.Commands, CS$<>8__locals1.ct));
									sshClient.Disconnect();
									DeviceSnapshot deviceSnapshot = CS$<>8__locals1.<>4__this._parsers.Resolve(CS$<>8__locals1.def.Parser).Parse(dictionary);
									CS$<>8__locals1.sw.Stop();
									pollOutcome2 = new PollOutcome
									{
										Reachable = true,
										LatencyMs = new int?((int)CS$<>8__locals1.sw.ElapsedMilliseconds),
										Snapshot = deviceSnapshot
									};
								}
								return pollOutcome2;
							}, CS$<>8__locals1.ct);
						}
						catch (OperationCanceledException)
						{
							throw;
						}
						catch (Exception ex)
						{
							this._log.LogDebug(ex, "SSH poll failed for {Device}", new object[] { CS$<>8__locals1.device.Name });
							pollOutcome = PollOutcome.Unreachable("SSH error: " + ex.Message);
						}
					}
				}
			}
			return pollOutcome;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000D29C File Offset: 0x0000B49C
		private static Dictionary<string, string> RunExec(SshClient client, List<string> commands, CancellationToken ct)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in commands)
			{
				ct.ThrowIfCancellationRequested();
				SshCommand sshCommand = client.RunCommand(text);
				dictionary[text] = (string.IsNullOrEmpty(sshCommand.Result) ? sshCommand.Error : sshCommand.Result);
			}
			return dictionary;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000D31C File Offset: 0x0000B51C
		private static Dictionary<string, string> RunInteractive(SshClient client, SshDefinition def, CancellationToken ct)
		{
			SshScriptCollector.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.ct = ct;
			CS$<>8__locals1.outputs = new Dictionary<string, string>();
			CS$<>8__locals1.shell = client.CreateShellStream("vt100", 200U, 50U, 800U, 600U, 16384);
			Dictionary<string, string> outputs;
			try
			{
				CS$<>8__locals1.shell.Expect(SshScriptCollector.PromptRegex, TimeSpan.FromSeconds(8L), -1);
				CS$<>8__locals1.i = 0;
				foreach (string text in def.Commands)
				{
					SshScriptCollector.<RunInteractive>g__Run|9_0(text, ref CS$<>8__locals1);
				}
				foreach (SshFollowUp sshFollowUp in def.EffectiveFollowUps())
				{
					if (!string.IsNullOrWhiteSpace(sshFollowUp.TokenPattern) && !string.IsNullOrWhiteSpace(sshFollowUp.CommandTemplate))
					{
						foreach (string text2 in SshScriptCollector.ExtractTokens(CS$<>8__locals1.outputs.Values.ToList<string>(), sshFollowUp.TokenPattern, sshFollowUp.Max))
						{
							SshScriptCollector.<RunInteractive>g__Run|9_0(string.Format(sshFollowUp.CommandTemplate, text2), ref CS$<>8__locals1);
						}
					}
				}
				outputs = CS$<>8__locals1.outputs;
			}
			finally
			{
				if (CS$<>8__locals1.shell != null)
				{
					((IDisposable)CS$<>8__locals1.shell).Dispose();
				}
			}
			return outputs;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		private static List<string> ExtractTokens(IEnumerable<string> transcript, string pattern, int max)
		{
			Regex regex = new Regex(pattern, RegexOptions.Compiled);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			foreach (string text in transcript)
			{
				foreach (object obj in regex.Matches(text))
				{
					Match match = (Match)obj;
					string text2 = ((match.Groups.Count > 1) ? match.Groups[1].Value : match.Value).Trim();
					if (text2.Length > 0 && hashSet.Add(text2))
					{
						list.Add(text2);
						if (list.Count >= max)
						{
							return list;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		internal static void RestrictAlgorithms<[Nullable(2)] TValue>(IDictionary<string, TValue> available, List<string> allowed)
		{
			if (allowed == null || allowed.Count == 0)
			{
				return;
			}
			HashSet<string> keep = new HashSet<string>(allowed, StringComparer.OrdinalIgnoreCase);
			List<string> list = available.Keys.Where<string>((string k) => !keep.Contains(k)).ToList<string>();
			if (list.Count == available.Count)
			{
				return;
			}
			foreach (string text in list)
			{
				available.Remove(text);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		[CompilerGenerated]
		internal static void <RunInteractive>g__Run|9_0(string command, ref SshScriptCollector.<>c__DisplayClass9_0 A_1)
		{
			A_1.ct.ThrowIfCancellationRequested();
			A_1.shell.WriteLine(command);
			string text = A_1.shell.Expect(SshScriptCollector.PromptRegex, TimeSpan.FromSeconds(10L), -1) ?? "";
			Dictionary<string, string> outputs = A_1.outputs;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(A_1.i, "D2");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(command);
			outputs[defaultInterpolatedStringHandler.ToStringAndClear()] = text;
			int i = A_1.i;
			A_1.i = i + 1;
		}

		// Token: 0x0400011F RID: 287
		private readonly CredentialProtector _creds;

		// Token: 0x04000120 RID: 288
		private readonly SshParserRegistry _parsers;

		// Token: 0x04000121 RID: 289
		private readonly ILogger<SshScriptCollector> _log;

		// Token: 0x04000122 RID: 290
		internal static readonly Regex PromptRegex = new Regex("[>\\]]\\s*$", RegexOptions.Compiled);
	}
}
