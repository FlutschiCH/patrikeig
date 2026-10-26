using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Collectors.Rhub
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RhubSerialDialog
	{
		// Token: 0x06000327 RID: 807 RVA: 0x0000F287 File Offset: 0x0000D487
		[NullableContext(2)]
		public RhubSerialDialog(string password)
			: this(password, "noloadget", true)
		{
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000F296 File Offset: 0x0000D496
		public RhubSerialDialog([Nullable(2)] string password, string command, bool expectsChannels)
		{
			this._password = password;
			this._command = command;
			this._expectsChannels = expectsChannels;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000F2C1 File Offset: 0x0000D4C1
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000F2C9 File Offset: 0x0000D4C9
		public bool Authenticated { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000F2D2 File Offset: 0x0000D4D2
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000F2DA File Offset: 0x0000D4DA
		[Nullable(2)]
		public string Reason
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000F2E4 File Offset: 0x0000D4E4
		private int AnswerStart(string transcript)
		{
			if (this._answerPromptAt <= 0)
			{
				return 0;
			}
			int num = transcript.IndexOf('\n', Math.Min(transcript.Length, this._answerPromptAt));
			if (num < 0)
			{
				return transcript.Length;
			}
			return num + 1;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000F324 File Offset: 0x0000D524
		public bool Complete(string transcript)
		{
			if (!this._expectsChannels)
			{
				return RhubSerialDialog.PromptIndexAfter(transcript, this.AnswerStart(transcript)) >= 0;
			}
			return RhubSerialDialog.IsComplete(transcript);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000F348 File Offset: 0x0000D548
		public bool HasAnswer(string transcript)
		{
			if (this._expectsChannels)
			{
				return RhubSerialDialog.HasChannelData(transcript);
			}
			RhubSerialDialog.Phase phase = this._phase;
			bool flag = phase == RhubSerialDialog.Phase.Opening || phase == RhubSerialDialog.Phase.AwaitingPasswordAck;
			return !flag && (this.Complete(transcript) || this.Reply(transcript).Length > 0);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000F398 File Offset: 0x0000D598
		public string Reply(string transcript)
		{
			List<string> list = new List<string>();
			foreach (ValueTuple<string, int> valueTuple in RhubSerialDialog.Lines(transcript, this.AnswerStart(transcript)))
			{
				string text = valueTuple.Item1.Trim();
				if (text.Length != 0)
				{
					if (!text.All<char>((char c) => c == '*'))
					{
						if (text.StartsWith("->", StringComparison.Ordinal))
						{
							break;
						}
						if (!text.StartsWith("Enter Password->", StringComparison.OrdinalIgnoreCase) && !text.Equals(this._command, StringComparison.OrdinalIgnoreCase))
						{
							list.Add(text);
						}
					}
				}
			}
			return string.Join<string>('\n', list);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000F460 File Offset: 0x0000D660
		public RhubDialogStep Advance(string transcript)
		{
			if (transcript == null)
			{
				transcript = "";
			}
			switch (this._phase)
			{
			case RhubSerialDialog.Phase.Opening:
				this._phase = RhubSerialDialog.Phase.AwaitingReply;
				this._answerPromptAt = 0;
				return new RhubDialogStep(RhubDialogAction.Send, this._command, null);
			case RhubSerialDialog.Phase.AwaitingReply:
			{
				if (this.Complete(transcript))
				{
					this._phase = RhubSerialDialog.Phase.Finished;
					return RhubSerialDialog.Done();
				}
				int num = transcript.IndexOf("Enter Password->", StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					return RhubSerialDialog.Waiting();
				}
				if (string.IsNullOrEmpty(this._password))
				{
					return this.Fail("the unit asked for a password but none is configured");
				}
				this._passwordPromptAt = num + "Enter Password->".Length;
				this._phase = RhubSerialDialog.Phase.AwaitingPasswordAck;
				return new RhubDialogStep(RhubDialogAction.Send, this._password, null);
			}
			case RhubSerialDialog.Phase.AwaitingPasswordAck:
			{
				int num2 = RhubSerialDialog.PromptIndexAfter(transcript, this._passwordPromptAt);
				if (num2 < 0)
				{
					return RhubSerialDialog.Waiting();
				}
				this.Authenticated = true;
				this._phase = RhubSerialDialog.Phase.AwaitingData;
				this._answerPromptAt = num2;
				return new RhubDialogStep(RhubDialogAction.Send, this._command, null);
			}
			case RhubSerialDialog.Phase.AwaitingData:
				if (this.Complete(transcript))
				{
					this._phase = RhubSerialDialog.Phase.Finished;
					return RhubSerialDialog.Done();
				}
				return RhubSerialDialog.Waiting();
			default:
				return RhubSerialDialog.Done();
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000F580 File Offset: 0x0000D780
		public static bool IsComplete(string transcript)
		{
			int num = transcript.LastIndexOf("Channel[", StringComparison.Ordinal);
			return num >= 0 && RhubSerialDialog.PromptIndexAfter(transcript, num) >= 0;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000F5AD File Offset: 0x0000D7AD
		public static bool HasChannelData(string transcript)
		{
			return transcript.Contains("Channel[", StringComparison.Ordinal);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		private static int PromptIndexAfter(string transcript, int from)
		{
			if (from < 0)
			{
				from = 0;
			}
			foreach (ValueTuple<string, int> valueTuple in RhubSerialDialog.Lines(transcript, from))
			{
				string item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item.TrimStart().StartsWith("->", StringComparison.Ordinal))
				{
					return item2;
				}
			}
			return -1;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000F630 File Offset: 0x0000D830
		[return: TupleElementNames(new string[] { "Line", "Start" })]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		private static IEnumerable<ValueTuple<string, int>> Lines(string text, int from)
		{
			RhubSerialDialog.<Lines>d__28 <Lines>d__ = new RhubSerialDialog.<Lines>d__28(-2);
			<Lines>d__.<>3__text = text;
			<Lines>d__.<>3__from = from;
			return <Lines>d__;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000F647 File Offset: 0x0000D847
		private static RhubDialogStep Waiting()
		{
			return new RhubDialogStep(RhubDialogAction.Wait, null, null);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000F651 File Offset: 0x0000D851
		private static RhubDialogStep Done()
		{
			return new RhubDialogStep(RhubDialogAction.Done, null, null);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000F65B File Offset: 0x0000D85B
		private RhubDialogStep Fail(string reason)
		{
			this._phase = RhubSerialDialog.Phase.Finished;
			this.Reason = reason;
			return new RhubDialogStep(RhubDialogAction.Done, null, reason);
		}

		// Token: 0x04000167 RID: 359
		private const string Prompt = "->";

		// Token: 0x04000168 RID: 360
		private const string PasswordPrompt = "Enter Password->";

		// Token: 0x04000169 RID: 361
		public const string NoLoadGet = "noloadget";

		// Token: 0x0400016A RID: 362
		[Nullable(2)]
		private readonly string _password;

		// Token: 0x0400016B RID: 363
		private readonly string _command;

		// Token: 0x0400016C RID: 364
		private readonly bool _expectsChannels;

		// Token: 0x0400016D RID: 365
		private RhubSerialDialog.Phase _phase;

		// Token: 0x0400016E RID: 366
		private int _passwordPromptAt = -1;

		// Token: 0x0400016F RID: 367
		private int _answerPromptAt = -1;

		// Token: 0x0200011D RID: 285
		[NullableContext(0)]
		private enum Phase
		{
			// Token: 0x040005B8 RID: 1464
			Opening,
			// Token: 0x040005B9 RID: 1465
			AwaitingReply,
			// Token: 0x040005BA RID: 1466
			AwaitingPasswordAck,
			// Token: 0x040005BB RID: 1467
			AwaitingData,
			// Token: 0x040005BC RID: 1468
			Finished
		}
	}
}
