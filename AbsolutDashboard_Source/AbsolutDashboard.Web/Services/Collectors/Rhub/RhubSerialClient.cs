using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x02000061 RID: 97
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RhubSerialClient
	{
		// Token: 0x06000310 RID: 784 RVA: 0x0000EAE7 File Offset: 0x0000CCE7
		public RhubSerialClient(ILogger<RhubSerialClient> log, [Nullable(new byte[] { 2, 1 })] IOptions<PollingOptions> options = null)
		{
			this._log = log;
			this._charGap = TimeSpan.FromMilliseconds((long)Math.Max(0, (options != null) ? options.Value.SerialCharGapMs : 100));
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000EB1C File Offset: 0x0000CD1C
		private void WriteLinePaced(SerialPort port, string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				port.Write(text[i].ToString());
				if (this._charGap > TimeSpan.Zero)
				{
					Thread.Sleep(this._charGap);
				}
			}
			port.Write(port.NewLine);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000EB7A File Offset: 0x0000CD7A
		public Task<string> ReadPowerAsync(string portName, int baudRate, [Nullable(2)] string framing, [Nullable(2)] string password, TimeSpan timeout, CancellationToken ct)
		{
			return this.ExchangeAsync(portName, baudRate, framing, new RhubSerialDialog(password), timeout, ct);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000EB90 File Offset: 0x0000CD90
		[NullableContext(2)]
		[return: TupleElementNames(new string[] { "Reply", "PromptReturned", "Raw", "Reason" })]
		[return: Nullable(new byte[] { 1, 0, 1, 1, 2 })]
		public async Task<ValueTuple<string, bool, string, string>> SetChannelPowerAsync([Nullable(1)] string portName, int baudRate, string framing, string password, int channel, bool on, TimeSpan timeout, CancellationToken ct)
		{
			bool flag = channel < 1 || channel > 64;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("channel");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("pa ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(channel);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(on ? "on" : "off");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			RhubSerialDialog dialog = new RhubSerialDialog(password, text, false);
			string text2 = await this.ExchangeAsync(portName, baudRate, framing, dialog, timeout, ct);
			return new ValueTuple<string, bool, string, string>(dialog.Reply(text2), dialog.Complete(text2), text2, dialog.Reason);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000EC18 File Offset: 0x0000CE18
		private async Task<string> ExchangeAsync(string portName, int baudRate, [Nullable(2)] string framing, RhubSerialDialog dialog, TimeSpan timeout, CancellationToken ct)
		{
			SemaphoreSlim gate = RhubSerialClient.Locks.GetOrAdd(portName, (string _) => new SemaphoreSlim(1, 1));
			await gate.WaitAsync(ct);
			string text;
			try
			{
				text = await Task.Run<string>(() => this.Exchange(portName, baudRate, framing, dialog, timeout, ct), ct);
			}
			finally
			{
				gate.Release();
			}
			return text;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000EC90 File Offset: 0x0000CE90
		private string Exchange(string portName, int baudRate, [Nullable(2)] string framing, RhubSerialDialog dialog, TimeSpan timeout, CancellationToken ct)
		{
			ValueTuple<int, Parity, StopBits> valueTuple = RhubSerialClient.ParseFraming(framing);
			int item = valueTuple.Item1;
			Parity item2 = valueTuple.Item2;
			StopBits item3 = valueTuple.Item3;
			string text3;
			using (SerialPort serialPort = new SerialPort(portName, baudRate, item2, item, item3)
			{
				Handshake = Handshake.None,
				ReadTimeout = 250,
				WriteTimeout = 2000,
				NewLine = "\r",
				DtrEnable = true,
				RtsEnable = true
			})
			{
				serialPort.Open();
				Thread.Sleep(200);
				serialPort.DiscardInBuffer();
				serialPort.DiscardOutBuffer();
				StringBuilder stringBuilder = new StringBuilder();
				Stopwatch stopwatch = Stopwatch.StartNew();
				Stopwatch stopwatch2 = Stopwatch.StartNew();
				bool flag = false;
				RhubDialogStep rhubDialogStep = dialog.Advance("");
				IL_009D:
				while (rhubDialogStep.Action != RhubDialogAction.Done)
				{
					if (rhubDialogStep.Action == RhubDialogAction.Send)
					{
						this.WriteLinePaced(serialPort, rhubDialogStep.Send);
					}
					while (!(stopwatch.Elapsed >= timeout) && !ct.IsCancellationRequested)
					{
						string text;
						try
						{
							text = serialPort.ReadExisting();
						}
						catch (TimeoutException)
						{
							text = "";
						}
						if (text.Length == 0)
						{
							string text2 = stringBuilder.ToString();
							if (dialog.HasAnswer(text2) && stopwatch2.Elapsed > RhubSerialClient.IdleGap)
							{
								return text2;
							}
							bool flag2 = text2.Length > 0 && !text2.Contains('\n') && !text2.Contains('\r');
							if (!flag && flag2 && stopwatch2.Elapsed > RhubSerialClient.NudgeAfter)
							{
								this._log.LogDebug("{Port}: echoed {Echo} with no line break; re-sending Enter.", new object[] { portName, text2.Length });
								serialPort.Write("\r");
								flag = true;
								stopwatch2.Restart();
							}
							else if (flag && !dialog.HasAnswer(text2) && stopwatch2.Elapsed > RhubSerialClient.StallAfter)
							{
								return text2;
							}
							Thread.Sleep(50);
						}
						if (text.Length != 0)
						{
							stringBuilder.Append(text);
							stopwatch2.Restart();
							rhubDialogStep = dialog.Advance(stringBuilder.ToString());
							goto IL_009D;
						}
					}
					this._log.LogWarning("{Port}: gave up after {Budget:0.#}s{Detail}.", new object[]
					{
						portName,
						timeout.TotalSeconds,
						(stringBuilder.Length == 0) ? " with no response at all" : " mid-response"
					});
					return stringBuilder.ToString();
				}
				if (rhubDialogStep.Reason != null)
				{
					this._log.LogWarning("{Port}: {Reason}.", new object[] { portName, rhubDialogStep.Reason });
				}
				text3 = stringBuilder.ToString();
			}
			return text3;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000EF7C File Offset: 0x0000D17C
		[NullableContext(0)]
		[return: TupleElementNames(new string[] { "DataBits", "Parity", "StopBits" })]
		public static ValueTuple<int, Parity, StopBits> ParseFraming([Nullable(2)] string framing)
		{
			string[] array = (framing ?? "").Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			if (array.Length != 3)
			{
				return new ValueTuple<int, Parity, StopBits>(8, Parity.None, StopBits.One);
			}
			int num;
			bool flag = !int.TryParse(array[0], out num);
			if (!flag)
			{
				bool flag2 = num < 5 || num > 8;
				flag = flag2;
			}
			if (flag)
			{
				num = 8;
			}
			string text = array[1].ToUpperInvariant();
			Parity parity;
			if (!(text == "E"))
			{
				if (!(text == "O"))
				{
					if (!(text == "M"))
					{
						if (!(text == "S"))
						{
							parity = Parity.None;
						}
						else
						{
							parity = Parity.Space;
						}
					}
					else
					{
						parity = Parity.Mark;
					}
				}
				else
				{
					parity = Parity.Odd;
				}
			}
			else
			{
				parity = Parity.Even;
			}
			Parity parity2 = parity;
			text = array[2];
			StopBits stopBits;
			if (!(text == "2"))
			{
				if (!(text == "1.5"))
				{
					stopBits = StopBits.One;
				}
				else
				{
					stopBits = StopBits.OnePointFive;
				}
			}
			else
			{
				stopBits = StopBits.Two;
			}
			StopBits stopBits2 = stopBits;
			return new ValueTuple<int, Parity, StopBits>(num, parity2, stopBits2);
		}

		// Token: 0x0400015A RID: 346
		private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400015B RID: 347
		private static readonly TimeSpan IdleGap = TimeSpan.FromSeconds(3L);

		// Token: 0x0400015C RID: 348
		private static readonly TimeSpan NudgeAfter = TimeSpan.FromMilliseconds(1500L);

		// Token: 0x0400015D RID: 349
		private static readonly TimeSpan StallAfter = TimeSpan.FromSeconds(4L);

		// Token: 0x0400015E RID: 350
		private readonly TimeSpan _charGap;

		// Token: 0x0400015F RID: 351
		private readonly ILogger<RhubSerialClient> _log;
	}
}
