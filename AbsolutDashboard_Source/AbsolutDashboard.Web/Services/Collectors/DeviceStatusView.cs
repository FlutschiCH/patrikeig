using System;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Data.Entities;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000038 RID: 56
	[NullableContext(1)]
	[Nullable(0)]
	public static class DeviceStatusView
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00008674 File Offset: 0x00006874
		[return: TupleElementNames(new string[] { "Css", "Text" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<string, string> Pill(bool? reachable)
		{
			ValueTuple<string, string> valueTuple;
			if (reachable != null)
			{
				if (reachable.GetValueOrDefault())
				{
					valueTuple = new ValueTuple<string, string>("pill pill-up", "Up");
				}
				else
				{
					valueTuple = new ValueTuple<string, string>("pill pill-down", "Down");
				}
			}
			else
			{
				valueTuple = new ValueTuple<string, string>("pill pill-unknown", "Unknown");
			}
			return valueTuple;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000086C8 File Offset: 0x000068C8
		[NullableContext(2)]
		public static bool IsAlarmState(string state)
		{
			return state != null && DeviceStatusView.AlarmStates.Contains(state.Trim().ToLowerInvariant());
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000086EC File Offset: 0x000068EC
		[NullableContext(2)]
		[return: TupleElementNames(new string[] { "Css", "Text" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<string, string> StateBadge(bool? reachable, string state)
		{
			string text = ((state != null) ? state.Trim().ToLowerInvariant() : null);
			string text2;
			if (!(text == "connected"))
			{
				if (!(text == "idle"))
				{
					if (DeviceStatusView.IsAlarmState(state))
					{
						text2 = "pill pill-warn";
					}
					else
					{
						text2 = DeviceStatusView.Pill(reachable).Item1;
					}
				}
				else
				{
					text2 = "pill pill-unknown";
				}
			}
			else
			{
				text2 = "pill pill-up";
			}
			return new ValueTuple<string, string>(text2, string.IsNullOrWhiteSpace(state) ? DeviceStatusView.Pill(reachable).Item2 : state);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008770 File Offset: 0x00006970
		[NullableContext(2)]
		[return: TupleElementNames(new string[] { "Css", "Text" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<string, string> UeBadge(bool? reachable, string state)
		{
			string text = ((state != null) ? state.Trim().ToLowerInvariant() : null);
			if (text == "connected")
			{
				return new ValueTuple<string, string>("pill pill-up", "Connected");
			}
			if (text == "idle")
			{
				return new ValueTuple<string, string>("pill pill-idle", "Idle");
			}
			if (reachable.GetValueOrDefault())
			{
				return new ValueTuple<string, string>("pill pill-up", string.IsNullOrWhiteSpace(state) ? "Connected" : state.Trim());
			}
			return new ValueTuple<string, string>("pill pill-off", "Disconnected");
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008804 File Offset: 0x00006A04
		[return: TupleElementNames(new string[] { "Css", "Label" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<string, string> PortCell(PortStatus p)
		{
			if (string.Equals(p.AdminStatus, "down", StringComparison.OrdinalIgnoreCase))
			{
				return new ValueTuple<string, string>("port-cell port-disabled", "disabled");
			}
			if (p.Up)
			{
				return new ValueTuple<string, string>("port-cell port-up", "up");
			}
			return new ValueTuple<string, string>("port-cell port-idle", "idle");
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000885C File Offset: 0x00006A5C
		public static string MethodLabel(CollectionMethod m)
		{
			string text;
			switch (m)
			{
			case CollectionMethod.Snmp:
				text = "SNMP";
				break;
			case CollectionMethod.SshScript:
				text = "SSH";
				break;
			case CollectionMethod.ViaParent:
				text = "via BBU";
				break;
			case CollectionMethod.BbuWebSocket:
				text = "WebSocket/MML";
				break;
			default:
				text = m.ToString();
				break;
			}
			return text;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000088B0 File Offset: 0x00006AB0
		public static string Ago(DateTime? utc)
		{
			if (utc == null)
			{
				return "never";
			}
			TimeSpan timeSpan = DateTime.UtcNow - utc.Value;
			if (timeSpan.TotalSeconds < 60.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)Math.Max(0.0, timeSpan.TotalSeconds));
				defaultInterpolatedStringHandler.AppendLiteral("s ago");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (timeSpan.TotalMinutes < 60.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<int>((int)timeSpan.TotalMinutes);
				defaultInterpolatedStringHandler2.AppendLiteral("m ago");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (timeSpan.TotalHours < 24.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler3.AppendFormatted<int>((int)timeSpan.TotalHours);
				defaultInterpolatedStringHandler3.AppendLiteral("h ago");
				return defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(5, 1);
			defaultInterpolatedStringHandler4.AppendFormatted<int>((int)timeSpan.TotalDays);
			defaultInterpolatedStringHandler4.AppendLiteral("d ago");
			return defaultInterpolatedStringHandler4.ToStringAndClear();
		}

		// Token: 0x040000AB RID: 171
		private static readonly string[] AlarmStates = new string[] { "alarm", "fault", "degraded", "warning" };
	}
}
