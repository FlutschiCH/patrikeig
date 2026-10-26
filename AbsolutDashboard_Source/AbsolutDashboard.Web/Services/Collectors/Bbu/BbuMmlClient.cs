using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuMmlClient : IAsyncDisposable
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x000145F8 File Offset: 0x000127F8
		public BbuMmlClient(string host, int port, string user, string password)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
			defaultInterpolatedStringHandler.AppendLiteral("wss://");
			defaultInterpolatedStringHandler.AppendFormatted(host);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(port);
			defaultInterpolatedStringHandler.AppendLiteral("/websocket/socket,%7Btimeout:5000%7D");
			this._url = new Uri(defaultInterpolatedStringHandler.ToStringAndClear());
			this._user = user;
			this._password = password;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000146A4 File Offset: 0x000128A4
		public async Task ConnectAsync(CancellationToken ct)
		{
			this._ws.Options.RemoteCertificateValidationCallback = ([Nullable(1)] object _, X509Certificate _, X509Chain _, SslPolicyErrors _) => true;
			await this._ws.ConnectAsync(this._url, ct);
			Task.Run(new Func<Task>(this.ReceiveLoopAsync));
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000146F0 File Offset: 0x000128F0
		private async Task ReceiveLoopAsync()
		{
			byte[] buf = new byte[65536];
			StringBuilder sb = new StringBuilder();
			try
			{
				while (this._ws.State == WebSocketState.Open && !this._cts.IsCancellationRequested)
				{
					sb.Clear();
					WebSocketReceiveResult webSocketReceiveResult;
					do
					{
						webSocketReceiveResult = await this._ws.ReceiveAsync(new ArraySegment<byte>(buf), this._cts.Token);
						if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
						{
							goto Block_3;
						}
						sb.Append(Encoding.UTF8.GetString(buf, 0, webSocketReceiveResult.Count));
					}
					while (!webSocketReceiveResult.EndOfMessage);
					string text = sb.ToString();
					if (text.StartsWith("Beat="))
					{
						await this.SendRawAsync("Beat=1");
						continue;
					}
					if (text.StartsWith("VerifyGet="))
					{
						this._verify = text.Substring("VerifyGet=".Length).Trim();
						continue;
					}
					if (text.StartsWith("Type="))
					{
						string[] array = text.Substring("Type=".Length).Split(',', StringSplitOptions.None);
						if (array.Length >= 2)
						{
							this._clientIpTcs.TrySetResult(array[1].Trim());
							continue;
						}
						continue;
					}
					else
					{
						if (text.StartsWith("Logined="))
						{
							this._degradedSession = true;
							continue;
						}
						if (text.StartsWith("Login=") && text.Contains("Token=", StringComparison.Ordinal))
						{
							this._sessionGranted = true;
							continue;
						}
						if (!text.StartsWith("Json="))
						{
							continue;
						}
						int num = text.IndexOf('{');
						if (num >= 0)
						{
							try
							{
								using (JsonDocument jsonDocument = JsonDocument.Parse(text.Substring(num), default(JsonDocumentOptions)))
								{
									JsonElement rootElement = jsonDocument.RootElement;
									JsonElement jsonElement;
									if (!rootElement.TryGetProperty("MSG_TYPE", out jsonElement) || !(jsonElement.GetString() == "ReportAlarm"))
									{
										this._incoming.Writer.TryWrite(rootElement.Clone());
									}
								}
							}
							catch (JsonException)
							{
							}
							continue;
						}
						continue;
					}
					Block_3:
					break;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (WebSocketException)
			{
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00014734 File Offset: 0x00012934
		private async Task SendRawAsync(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			await this._ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00014780 File Offset: 0x00012980
		private static string Frame(Dictionary<string, string> obj)
		{
			string text = JsonSerializer.Serialize<Dictionary<string, string>>(obj, BbuMmlClient.JsonOpts);
			int num = Encoding.UTF8.GetByteCount(text) + 9;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Json=MSG_LENGTH:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num, "D8");
			defaultInterpolatedStringHandler.AppendLiteral(",XXXXXXXX");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000147E8 File Offset: 0x000129E8
		private async Task<JsonElement> WaitForAsync(string msgType, CancellationToken ct, int timeoutMs = 15000)
		{
			JsonElement jsonElement3;
			using (CancellationTokenSource to = CancellationTokenSource.CreateLinkedTokenSource(ct))
			{
				to.CancelAfter(timeoutMs);
				JsonElement jsonElement;
				JsonElement jsonElement2;
				do
				{
					jsonElement = await this._incoming.Reader.ReadAsync(to.Token);
				}
				while (!jsonElement.TryGetProperty("MSG_TYPE", out jsonElement2) || !(jsonElement2.GetString() == msgType));
				jsonElement3 = jsonElement;
			}
			return jsonElement3;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014844 File Offset: 0x00012A44
		public async Task LoginAsync(CancellationToken ct)
		{
			await this.SendRawAsync("VerifyGet=?");
			await this.SendRawAsync("Type=?");
			using (CancellationTokenSource ipTo = CancellationTokenSource.CreateLinkedTokenSource(ct))
			{
				ipTo.CancelAfter(10000);
				this._userIp = await this._clientIpTcs.Task.WaitAsync(ipTo.Token);
			}
			CancellationTokenSource ipTo = null;
			await this.SendRawAsync("VerifyGet=?");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["MSG_TYPE"] = "getSystemInfo";
			dictionary["USER_IP"] = this._userIp;
			await this.SendRawAsync(BbuMmlClient.Frame(dictionary));
			await this.WaitForAsync("getSystemInfoResponse", ct, 15000);
			await this.SendRawAsync("Title=en");
			if (this._verify != null)
			{
				await this.SendRawAsync("VerifyCode=" + this._verify);
			}
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["MSG_TYPE"] = "login";
			dictionary2["USER_NAME"] = this._user;
			dictionary2["username"] = this._user;
			dictionary2["password"] = this._password;
			dictionary2["USER_IP"] = this._userIp;
			await this.SendRawAsync(BbuMmlClient.Frame(dictionary2));
			TaskAwaiter<JsonElement> taskAwaiter = this.WaitForAsync("loginResponse", ct, 15000).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<JsonElement> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<JsonElement>);
			}
			JsonElement jsonElement;
			if ((taskAwaiter.GetResult().TryGetProperty("result", out jsonElement) ? jsonElement.GetString() : null) != "yes")
			{
				throw new InvalidOperationException("BBU login rejected (check operator account / password).");
			}
			await this.SendRawAsync("Login=" + this._user + ",0,1");
			await this.SendRawAsync("VerifyGet=?");
			await Task.Delay(800, ct);
			if (this._degradedSession && !this._sessionGranted)
			{
				throw new InvalidOperationException("Another operator session is already active on this BBU, so MML commands would be ignored. The dashboard needs its own dedicated operator account.");
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00014890 File Offset: 0x00012A90
		public async Task<string> MmlAsync(string command, CancellationToken ct)
		{
			if (!command.TrimEnd().EndsWith(';'))
			{
				command = command.TrimEnd() + ":;";
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["MSG_TYPE"] = "MML";
			dictionary["USER_NAME"] = this._user;
			dictionary["CMD_SOURCE"] = "WebLMT";
			dictionary["MML_CONTENT"] = command;
			dictionary["USER_IP"] = this._userIp;
			await this.SendRawAsync(BbuMmlClient.Frame(dictionary));
			TaskAwaiter<JsonElement> taskAwaiter = this.WaitForAsync("MML", ct, 15000).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<JsonElement> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<JsonElement>);
			}
			JsonElement jsonElement;
			return WebUtility.HtmlDecode((taskAwaiter.GetResult().TryGetProperty("MML_CONTENT", out jsonElement) ? (jsonElement.GetString() ?? "") : "").Replace("<br/>", "\n").Replace("<br>", "\n"));
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000148E4 File Offset: 0x00012AE4
		public async Task<MmlResponse> QueryAsync(string command, CancellationToken ct)
		{
			return MmlParser.Parse(await this.MmlAsync(command, ct));
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00014938 File Offset: 0x00012B38
		public async Task<MmlResponse> QueryMultiAsync(string command, CancellationToken ct)
		{
			if (!command.TrimEnd().EndsWith(';'))
			{
				command = command.TrimEnd() + ":;";
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["MSG_TYPE"] = "MML";
			dictionary["USER_NAME"] = this._user;
			dictionary["CMD_SOURCE"] = "WebLMT";
			dictionary["MML_CONTENT"] = command;
			dictionary["USER_IP"] = this._userIp;
			await this.SendRawAsync(BbuMmlClient.Frame(dictionary));
			List<string> frames = new List<string>();
			string text;
			do
			{
				TaskAwaiter<JsonElement> taskAwaiter = this.WaitForAsync("MML", ct, 15000).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<JsonElement> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<JsonElement>);
				}
				JsonElement jsonElement;
				text = WebUtility.HtmlDecode((taskAwaiter.GetResult().TryGetProperty("MML_CONTENT", out jsonElement) ? (jsonElement.GetString() ?? "") : "").Replace("<br/>", "\n").Replace("<br>", "\n"));
				frames.Add(text);
			}
			while (text.Contains("To be continued", StringComparison.OrdinalIgnoreCase));
			return MmlParser.ParseMulti(frames);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001498C File Offset: 0x00012B8C
		public async ValueTask DisposeAsync()
		{
			this._cts.Cancel();
			try
			{
				if (this._ws.State == WebSocketState.Open)
				{
					await this._ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
				}
			}
			catch
			{
			}
			finally
			{
				this._ws.Dispose();
				this._cts.Dispose();
			}
		}

		// Token: 0x040001D7 RID: 471
		private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		// Token: 0x040001D8 RID: 472
		private readonly Uri _url;

		// Token: 0x040001D9 RID: 473
		private readonly string _user;

		// Token: 0x040001DA RID: 474
		private readonly string _password;

		// Token: 0x040001DB RID: 475
		private readonly ClientWebSocket _ws = new ClientWebSocket();

		// Token: 0x040001DC RID: 476
		private readonly Channel<JsonElement> _incoming = Channel.CreateUnbounded<JsonElement>();

		// Token: 0x040001DD RID: 477
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040001DE RID: 478
		private readonly TaskCompletionSource<string> _clientIpTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

		// Token: 0x040001DF RID: 479
		[Nullable(2)]
		private volatile string _verify;

		// Token: 0x040001E0 RID: 480
		private string _userIp = "";

		// Token: 0x040001E1 RID: 481
		private volatile bool _degradedSession;

		// Token: 0x040001E2 RID: 482
		private volatile bool _sessionGranted;
	}
}
