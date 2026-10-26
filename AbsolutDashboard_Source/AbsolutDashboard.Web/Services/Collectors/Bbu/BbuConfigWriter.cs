using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuConfigWriter
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x0001452B File Offset: 0x0001272B
		public BbuConfigWriter(CredentialProtector creds, BbuSessionLock sessions, ILogger<BbuConfigWriter> log)
		{
			this._creds = creds;
			this._sessions = sessions;
			this._log = log;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00014548 File Offset: 0x00012748
		public bool IsBusy(Device bbu)
		{
			return this._sessions.IsBusy(bbu.Id);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001455B File Offset: 0x0001275B
		public Task<BbuWriteOutcome> ApplyAsync(Device bbu, IReadOnlyList<MmlWrite> writes, CancellationToken ct)
		{
			return this.RunAsync(bbu, writes, ct);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00014566 File Offset: 0x00012766
		public Task<BbuWriteOutcome> RestartAsync(Device bbu, CancellationToken ct)
		{
			return this.RunAsync(bbu, new MmlWrite[]
			{
				new MmlWrite("Restart base station", "RST BTS:TYPE=PROCESS;", true)
			}, ct);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001458C File Offset: 0x0001278C
		private async Task<BbuWriteOutcome> RunAsync(Device bbu, IReadOnlyList<MmlWrite> writes, CancellationToken ct)
		{
			BbuWriteOutcome bbuWriteOutcome;
			if (writes.Count == 0)
			{
				bbuWriteOutcome = new BbuWriteOutcome(true, null, new List<MmlStep>());
			}
			else if (string.IsNullOrWhiteSpace(bbu.IpAddress))
			{
				bbuWriteOutcome = BbuWriteOutcome.Failed("No IP address configured.");
			}
			else if (string.IsNullOrWhiteSpace(bbu.SshUsername))
			{
				bbuWriteOutcome = BbuWriteOutcome.Failed("No operator account configured.");
			}
			else
			{
				string password = this._creds.Unprotect(bbu.SshPasswordEnc);
				if (string.IsNullOrEmpty(password))
				{
					bbuWriteOutcome = BbuWriteOutcome.Failed("No operator password configured.");
				}
				else
				{
					TaskAwaiter<bool> taskAwaiter = this._sessions.WaitAsync(bbu.Id, BbuConfigWriter.SessionWait, ct).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						List<MmlStep> steps = new List<MmlStep>();
						try
						{
							int num = ((bbu.Port > 0) ? bbu.Port : 20000);
							BbuMmlClient client = new BbuMmlClient(bbu.IpAddress, num, bbu.SshUsername, password);
							object obj = null;
							int num2 = 0;
							BbuWriteOutcome bbuWriteOutcome2;
							try
							{
								await client.ConnectAsync(ct);
								await client.LoginAsync(ct);
								foreach (MmlWrite w in writes)
								{
									MmlResponse mmlResponse = await client.QueryAsync(w.Command, ct);
									MmlStep mmlStep = new MmlStep(w.Label, w.Command, mmlResponse.Success, mmlResponse.RetCode, mmlResponse.Message);
									steps.Add(mmlStep);
									if (!mmlStep.Ok)
									{
										this._log.LogWarning("BBU {Device}: {Command} rejected, RETCODE {Code} {Message}", new object[] { bbu.Name, w.Command, mmlResponse.RetCode, mmlResponse.Message });
										bool flag = false;
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
										defaultInterpolatedStringHandler.AppendFormatted(w.Label);
										defaultInterpolatedStringHandler.AppendLiteral(" was rejected (RETCODE ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(mmlResponse.RetCode);
										defaultInterpolatedStringHandler.AppendLiteral("). ");
										defaultInterpolatedStringHandler.AppendFormatted(mmlResponse.Message);
										bbuWriteOutcome2 = new BbuWriteOutcome(flag, defaultInterpolatedStringHandler.ToStringAndClear().TrimEnd(), steps);
										goto IL_04F9;
									}
									this._log.LogInformation("BBU {Device}: {Command} accepted.", new object[] { bbu.Name, w.Command });
									w = null;
								}
								IEnumerator<MmlWrite> enumerator = null;
								bbuWriteOutcome2 = new BbuWriteOutcome(true, null, steps);
								IL_04F9:
								num2 = 1;
							}
							catch (object obj)
							{
							}
							if (client != null)
							{
								await client.DisposeAsync();
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
							if (num2 == 1)
							{
								return bbuWriteOutcome2;
							}
							obj = null;
							bbuWriteOutcome2 = null;
							client = null;
						}
						catch (OperationCanceledException obj3) when (!ct.IsCancellationRequested)
						{
							return new BbuWriteOutcome(false, "Timed out talking to the base station.", steps);
						}
						catch (OperationCanceledException)
						{
							throw;
						}
						catch (Exception ex2)
						{
							this._log.LogWarning(ex2, "BBU write failed for {Device}", new object[] { bbu.Name });
							return new BbuWriteOutcome(false, ex2.Message, steps);
						}
						finally
						{
							this._sessions.Release(bbu.Id);
						}
						password = null;
						steps = null;
						throw null;
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(61, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("The base station was busy for ");
					defaultInterpolatedStringHandler2.AppendFormatted<double>(BbuConfigWriter.SessionWait.TotalSeconds, "0");
					defaultInterpolatedStringHandler2.AppendLiteral(" s — it is mid-poll. Try again.");
					bbuWriteOutcome = BbuWriteOutcome.Failed(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			return bbuWriteOutcome;
		}

		// Token: 0x040001D3 RID: 467
		private readonly CredentialProtector _creds;

		// Token: 0x040001D4 RID: 468
		private readonly BbuSessionLock _sessions;

		// Token: 0x040001D5 RID: 469
		private readonly ILogger<BbuConfigWriter> _log;

		// Token: 0x040001D6 RID: 470
		public static readonly TimeSpan SessionWait = TimeSpan.FromSeconds(90L);
	}
}
