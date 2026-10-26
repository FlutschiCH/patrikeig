using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Route("/")]
	[Home.__PrivateComponentRenderModeAttribute]
	public class Home : ComponentBase, IDisposable
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x0002AFE4 File Offset: 0x000291E4
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Dashboard — Absolut");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<header class=\"mb-6\"><h1 class=\"text-2xl font-semibold text-cocoa-900\">Network Dashboard</h1>\n    <p class=\"mt-1 text-sm text-cocoa-700\">Live status across all monitored 5G devices.</p></header>\n\n");
			__builder.OpenComponent<AuthorizeView>(5);
			__builder.AddComponentParameter(6, "Roles", "Admin");
			__builder.AddAttribute(7, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<ShutdownAll>(8);
				__builder2.CloseComponent();
			}));
			__builder.CloseComponent();
			if (this._rows == null)
			{
				__builder.AddMarkupContent(9, "<p class=\"text-cocoa-700\">Loading…</p>");
				return;
			}
			__builder.OpenElement(10, "div");
			__builder.AddAttribute(11, "class", "grid gap-4 lg:grid-cols-2");
			__builder.OpenElement(12, "div");
			__builder.AddAttribute(13, "class", "card p-5");
			__builder.AddMarkupContent(14, "<h2 class=\"mb-3 text-sm font-semibold text-cocoa-900\">Performance metrics</h2>\n            ");
			__builder.OpenElement(15, "div");
			__builder.AddAttribute(16, "class", "grid grid-cols-2 gap-4 sm:grid-cols-4");
			__builder.OpenElement(17, "div");
			__builder.AddMarkupContent(18, "<p class=\"text-sm text-cocoa-700\">Peak download</p>\n                    ");
			__builder.OpenElement(19, "p");
			__builder.AddAttribute(20, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(21, Home.FormatPeak(this._peakDl));
			__builder.CloseElement();
			__builder.AddMarkupContent(22, "\n                    ");
			__builder.OpenElement(23, "p");
			__builder.AddAttribute(24, "class", "text-xs text-cocoa-700");
			__builder.AddContent(25, "last ");
			__builder.AddContent(26, this.WindowLabel);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(27, "\n                ");
			__builder.OpenElement(28, "div");
			__builder.AddMarkupContent(29, "<p class=\"text-sm text-cocoa-700\">Peak upload</p>\n                    ");
			__builder.OpenElement(30, "p");
			__builder.AddAttribute(31, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(32, Home.FormatPeak(this._peakUl));
			__builder.CloseElement();
			__builder.AddMarkupContent(33, "\n                    ");
			__builder.OpenElement(34, "p");
			__builder.AddAttribute(35, "class", "text-xs text-cocoa-700");
			__builder.AddContent(36, "last ");
			__builder.AddContent(37, this.WindowLabel);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(38, "\n                ");
			__builder.OpenElement(39, "div");
			__builder.AddMarkupContent(40, "<p class=\"text-sm text-cocoa-700\">BBU uptime</p>\n                    ");
			__builder.OpenElement(41, "p");
			__builder.AddAttribute(42, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(43, this._bbuUptime ?? "—");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(44, "\n                ");
			__builder.OpenElement(45, "div");
			__builder.AddMarkupContent(46, "<p class=\"text-sm text-cocoa-700\">5G Core uptime</p>\n                    ");
			__builder.OpenElement(47, "p");
			__builder.AddAttribute(48, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
			__builder.AddAttribute(49, "title", this._coreUptimeRaw);
			__builder.AddContent(50, this._coreUptime ?? "—");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(51, "\n        ");
			__builder.OpenElement(52, "div");
			__builder.AddAttribute(53, "class", "card p-5");
			__builder.AddMarkupContent(54, "<h2 class=\"mb-3 text-sm font-semibold text-cocoa-900\">Devices</h2>\n            ");
			__builder.OpenElement(55, "div");
			__builder.AddAttribute(56, "class", "grid grid-cols-2 gap-4 sm:grid-cols-4");
			__builder.OpenElement(57, "div");
			__builder.AddMarkupContent(58, "<p class=\"text-sm text-cocoa-700\">Total</p>");
			__builder.OpenElement(59, "p");
			__builder.AddAttribute(60, "class", "mt-1 text-2xl font-semibold text-cocoa-900");
			__builder.AddContent(61, this._total);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(62, "\n                ");
			__builder.OpenElement(63, "div");
			__builder.AddMarkupContent(64, "<p class=\"text-sm text-cocoa-700\">Up</p>");
			__builder.OpenElement(65, "p");
			__builder.AddAttribute(66, "class", "mt-1 text-2xl font-semibold text-moss-600");
			__builder.AddContent(67, this._up);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(68, "\n                ");
			__builder.OpenElement(69, "div");
			__builder.AddMarkupContent(70, "<p class=\"text-sm text-cocoa-700\">Down</p>");
			__builder.OpenElement(71, "p");
			__builder.AddAttribute(72, "class", "mt-1 text-2xl font-semibold text-red-600");
			__builder.AddContent(73, this._down);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(74, "\n                ");
			__builder.OpenElement(75, "div");
			__builder.AddMarkupContent(76, "<p class=\"text-sm text-cocoa-700\">Unknown</p>");
			__builder.OpenElement(77, "p");
			__builder.AddAttribute(78, "class", "mt-1 text-2xl font-semibold text-cocoa-700");
			__builder.AddContent(79, this._unknown);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.OpenElement(80, "section");
			__builder.AddAttribute(81, "class", "mt-8");
			__builder.OpenElement(82, "div");
			__builder.AddAttribute(83, "class", "mb-3 flex flex-wrap items-center justify-between gap-3");
			__builder.OpenElement(84, "h2");
			__builder.AddAttribute(85, "class", "text-lg font-semibold text-cocoa-900");
			__builder.AddMarkupContent(86, "\n                Overview ");
			__builder.OpenElement(87, "span");
			__builder.AddAttribute(88, "class", "text-sm font-normal text-cocoa-700");
			__builder.AddMarkupContent(89, "· last ");
			__builder.AddContent(90, this.WindowLabel);
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(91, "\n            ");
			__builder.OpenElement(92, "div");
			__builder.AddAttribute(93, "class", "inline-flex overflow-hidden rounded-full border border-blush-300 bg-white");
			int[] windowMinutes = Home.WindowMinutes;
			for (int i = 0; i < windowMinutes.Length; i++)
			{
				int m = windowMinutes[i];
				__builder.OpenElement(94, "button");
				__builder.AddAttribute(95, "type", "button");
				__builder.AddAttribute(96, "class", this.WindowCss(m));
				__builder.AddAttribute<MouseEventArgs>(97, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => this.SetWindowAsync(m)));
				__builder.AddContent(98, Home.WindowButton(m));
				__builder.CloseElement();
			}
			__builder.CloseElement();
			__builder.CloseElement();
			if (this._dlChart.Count > 0 || this._ulChart.Count > 0 || this._latencyChart.Count > 0 || this._strip.Count > 0)
			{
				__builder.OpenElement(99, "div");
				__builder.OpenElement(100, "div");
				__builder.AddAttribute(101, "class", "grid gap-4 lg:grid-cols-2");
				__builder.OpenComponent<LineChart>(102);
				__builder.AddComponentParameter(103, "Title", "Downlink");
				__builder.AddComponentParameter(104, "Unit", "Mbps");
				__builder.AddComponentParameter(105, "Series", RuntimeHelpers.TypeCheck<List<ChartSeries>>(this._dlChart));
				__builder.AddComponentParameter(106, "From", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowFrom)));
				__builder.AddComponentParameter(107, "To", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowTo)));
				__builder.CloseComponent();
				__builder.AddMarkupContent(108, "\n                ");
				__builder.OpenComponent<LineChart>(109);
				__builder.AddComponentParameter(110, "Title", "Uplink");
				__builder.AddComponentParameter(111, "Unit", "Mbps");
				__builder.AddComponentParameter(112, "Series", RuntimeHelpers.TypeCheck<List<ChartSeries>>(this._ulChart));
				__builder.AddComponentParameter(113, "From", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowFrom)));
				__builder.AddComponentParameter(114, "To", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowTo)));
				__builder.CloseComponent();
				__builder.CloseElement();
				__builder.AddMarkupContent(115, "\n            ");
				__builder.OpenElement(116, "div");
				__builder.AddAttribute(117, "class", "mt-4 grid gap-4 lg:grid-cols-2");
				__builder.OpenComponent<StatusStrip>(118);
				__builder.AddComponentParameter(119, "Title", "UE connectivity");
				__builder.AddComponentParameter(120, "Rows", RuntimeHelpers.TypeCheck<List<StatusStrip.Row>>(this._strip));
				__builder.AddComponentParameter(121, "From", RuntimeHelpers.TypeCheck<DateTime>(this._windowFrom));
				__builder.AddComponentParameter(122, "To", RuntimeHelpers.TypeCheck<DateTime>(this._windowTo));
				int num = 123;
				string text = "Subtitle";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._strip.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._ues.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" SIM");
				defaultInterpolatedStringHandler.AppendFormatted((this._ues.Count == 1) ? "" : "s");
				defaultInterpolatedStringHandler.AppendLiteral(" on air in this window");
				__builder.AddComponentParameter(num, text, RuntimeHelpers.TypeCheck<string>(defaultInterpolatedStringHandler.ToStringAndClear()));
				__builder.AddComponentParameter(124, "EmptyText", "no handset was on air in this window");
				__builder.CloseComponent();
				__builder.AddMarkupContent(125, "\n                ");
				__builder.OpenComponent<LineChart>(126);
				__builder.AddComponentParameter(127, "Title", "UE latency");
				__builder.AddComponentParameter(128, "Unit", "ms");
				__builder.AddComponentParameter(129, "Series", RuntimeHelpers.TypeCheck<List<ChartSeries>>(this._latencyChart));
				__builder.AddComponentParameter(130, "From", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowFrom)));
				__builder.AddComponentParameter(131, "To", RuntimeHelpers.TypeCheck<DateTime?>(new DateTime?(this._windowTo)));
				__builder.CloseComponent();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			else
			{
				__builder.AddMarkupContent(132, "<div class=\"card p-8 text-center text-cocoa-700\">\n            No history for this window yet. Graphs appear once the poller has collected a few samples.\n        </div>");
			}
			__builder.CloseElement();
			__builder.OpenElement(133, "section");
			__builder.AddAttribute(134, "class", "mt-8");
			__builder.AddMarkupContent(135, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">All devices</h2>");
			if (this._devices.Count == 0)
			{
				__builder.AddMarkupContent(136, "<div class=\"card p-8 text-center text-cocoa-700\">No devices yet.</div>");
			}
			else
			{
				__builder.OpenElement(137, "div");
				__builder.AddAttribute(138, "class", "card overflow-x-auto");
				__builder.OpenElement(139, "table");
				__builder.AddAttribute(140, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(141, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"px-4 py-3\">Name</th>\n                            <th class=\"px-4 py-3\">Type</th>\n                            <th class=\"hidden px-4 py-3 sm:table-cell\">Model</th>\n                            <th class=\"hidden px-4 py-3 md:table-cell\">Address</th>\n                            <th class=\"px-4 py-3\">Status</th>\n                            <th class=\"hidden px-4 py-3 md:table-cell\">Last polled</th>\n                            <th class=\"px-4 py-3\"></th></tr></thead>\n                    ");
				__builder.OpenElement(142, "tbody");
				foreach (Home.DeviceNode deviceNode in this._devices)
				{
					Device device = deviceNode.Device;
					ValueTuple<string, string> valueTuple = ((device.Category.Key == "ue") ? DeviceStatusView.UeBadge(device.LastReachable, this._ueState.GetValueOrDefault(device.Id)) : DeviceStatusView.Pill(device.LastReachable));
					__builder.OpenElement(143, "tr");
					__builder.AddAttribute(144, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(145, "td");
					__builder.AddAttribute(146, "class", "px-4 py-3 font-medium text-cocoa-900");
					__builder.OpenElement(147, "span");
					__builder.AddAttribute(148, "class", "inline-block");
					__builder.AddAttribute(149, "style", "width:" + (Math.Max(0, deviceNode.Depth - 1) * 20).ToString() + "px");
					__builder.CloseElement();
					if (deviceNode.Depth > 0)
					{
						__builder.AddMarkupContent(150, "<span class=\"mr-1 text-cocoa-700\">└</span>");
					}
					__builder.AddContent(151, device.Name);
					if (!device.Enabled)
					{
						__builder.AddMarkupContent(152, "<span class=\"ml-2 pill pill-unknown\">paused</span>");
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(153, "\n                                ");
					__builder.OpenElement(154, "td");
					__builder.AddAttribute(155, "class", "px-4 py-3");
					__builder.OpenElement(156, "span");
					__builder.AddAttribute(157, "class", "nav-chip");
					__builder.AddContent(158, device.Category.ShortName);
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(159, "\n                                ");
					__builder.OpenElement(160, "td");
					__builder.AddAttribute(161, "class", "hidden px-4 py-3 text-cocoa-700 sm:table-cell");
					int num2 = 162;
					DeviceTemplate template = device.Template;
					__builder.AddContent(num2, (template != null) ? template.Name : null);
					__builder.CloseElement();
					__builder.AddMarkupContent(163, "\n                                ");
					__builder.OpenElement(164, "td");
					__builder.AddAttribute(165, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					if (device.CollectionMethod == CollectionMethod.ViaParent)
					{
						__builder.OpenElement(166, "span");
						__builder.AddContent(167, "via ");
						int num3 = 168;
						Device parentDevice = device.ParentDevice;
						__builder.AddContent(num3, ((parentDevice != null) ? parentDevice.Name : null) ?? "—");
						__builder.CloseElement();
					}
					else
					{
						__builder.AddContent(169, device.IpAddress ?? "—");
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(170, "\n                                ");
					__builder.OpenElement(171, "td");
					__builder.AddAttribute(172, "class", "px-4 py-3");
					__builder.OpenElement(173, "span");
					__builder.AddAttribute(174, "class", valueTuple.Item1);
					__builder.AddAttribute(175, "title", device.LastError);
					__builder.AddContent(176, valueTuple.Item2);
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(177, "\n                                ");
					__builder.OpenElement(178, "td");
					__builder.AddAttribute(179, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					__builder.AddContent(180, DeviceStatusView.Ago(device.LastPolledAt));
					__builder.CloseElement();
					__builder.AddMarkupContent(181, "\n                                ");
					__builder.OpenElement(182, "td");
					__builder.AddAttribute(183, "class", "px-4 py-3 text-right");
					__builder.OpenElement(184, "a");
					__builder.AddAttribute(185, "class", "text-blush-700 hover:underline");
					int num4 = 186;
					string text2 = "href";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("devices/detail/");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(device.Id);
					__builder.AddAttribute(num4, text2, defaultInterpolatedStringHandler2.ToStringAndClear());
					__builder.AddContent(187, "Details");
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002C000 File Offset: 0x0002A200
		[NullableContext(2)]
		private static string SnapMetricStr(DeviceSnapshot snap, [Nullable(1)] string label)
		{
			if (snap == null)
			{
				return null;
			}
			Metric metric = snap.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == label);
			if (metric == null)
			{
				return null;
			}
			return metric.Value;
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0002C044 File Offset: 0x0002A244
		private string WindowLabel
		{
			get
			{
				if (this._windowMinutes < 60)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._windowMinutes);
					defaultInterpolatedStringHandler.AppendLiteral(" min");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				if (this._windowMinutes != 60)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 1);
					defaultInterpolatedStringHandler2.AppendFormatted<int>(this._windowMinutes / 60);
					defaultInterpolatedStringHandler2.AppendLiteral(" hours");
					return defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				return "hour";
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
		private static string WindowButton(int minutes)
		{
			if (minutes >= 60)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(minutes / 60);
				defaultInterpolatedStringHandler.AppendLiteral("h");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<int>(minutes);
			defaultInterpolatedStringHandler2.AppendLiteral("m");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002C122 File Offset: 0x0002A322
		private string WindowCss(int minutes)
		{
			return "px-3 py-1.5 text-sm font-medium transition " + ((minutes == this._windowMinutes) ? "bg-blush-500 text-white" : "text-cocoa-700 hover:bg-cream-100");
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002C143 File Offset: 0x0002A343
		private static string FormatPeak(double? mbps)
		{
			if (mbps != null)
			{
				return Home.FormatMbps(mbps.Value);
			}
			return "—";
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002C160 File Offset: 0x0002A360
		private static string FormatMbps(double mbps)
		{
			if (mbps < 1000.0)
			{
				return mbps.ToString("0.##", CultureInfo.InvariantCulture) + " Mbps";
			}
			return (mbps / 1000.0).ToString("0.##", CultureInfo.InvariantCulture) + " Gbps";
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002C1BC File Offset: 0x0002A3BC
		protected override async Task OnInitializedAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				await this.LoadStatusAsync(db);
				await this.LoadHistoryAsync(db);
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
				return;
			}
			obj = null;
			db = null;
			throw null;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002C1FF File Offset: 0x0002A3FF
		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender && base.RendererInfo.IsInteractive)
			{
				this.RefreshLoopAsync();
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002C218 File Offset: 0x0002A418
		private async Task SetWindowAsync(int minutes)
		{
			if (minutes != this._windowMinutes)
			{
				this._windowMinutes = minutes;
				ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
				ApplicationDbContext db = applicationDbContext;
				object obj = null;
				int num = 0;
				try
				{
					await this.LoadHistoryAsync(db);
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
				if (num != 1)
				{
					obj = null;
					db = null;
					throw null;
				}
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002C264 File Offset: 0x0002A464
		private async Task LoadStatusAsync(ApplicationDbContext db)
		{
			IQueryable<DeviceCategory> deviceCategories = db.DeviceCategories;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(DeviceCategory), "c");
			IQueryable<DeviceCategory> queryable = deviceCategories.OrderBy<DeviceCategory, int>(Expression.Lambda<Func<DeviceCategory, int>>(Expression.Property(parameterExpression, methodof(DeviceCategory.get_DisplayOrder())), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(DeviceCategory), "c");
			ConstructorInfo constructorInfo = methodof(Home.Row..ctor(string, int, int, int, int));
			Expression[] array = new Expression[5];
			array[0] = Expression.Property(parameterExpression, methodof(DeviceCategory.get_Key()));
			array[1] = Expression.Property(Expression.Property(parameterExpression, methodof(DeviceCategory.get_Devices())), methodof(ICollection<Device>.get_Count()));
			int num = 2;
			Expression expression = null;
			MethodInfo methodInfo = methodof(IEnumerable<!!0>.Count(Func<!!0, bool>));
			Expression[] array2 = new Expression[2];
			array2[0] = Expression.Property(parameterExpression, methodof(DeviceCategory.get_Devices()));
			int num2 = 1;
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Device), "d");
			array2[num2] = Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression2, methodof(Device.get_LastReachable())), Expression.Convert(Expression.Constant(true, typeof(bool)), typeof(bool?))), new ParameterExpression[] { parameterExpression2 });
			array[num] = Expression.Call(expression, methodInfo, array2);
			int num3 = 3;
			Expression expression2 = null;
			MethodInfo methodInfo2 = methodof(IEnumerable<!!0>.Count(Func<!!0, bool>));
			Expression[] array3 = new Expression[2];
			array3[0] = Expression.Property(parameterExpression, methodof(DeviceCategory.get_Devices()));
			int num4 = 1;
			parameterExpression2 = Expression.Parameter(typeof(Device), "d");
			array3[num4] = Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression2, methodof(Device.get_LastReachable())), Expression.Convert(Expression.Constant(false, typeof(bool)), typeof(bool?))), new ParameterExpression[] { parameterExpression2 });
			array[num3] = Expression.Call(expression2, methodInfo2, array3);
			int num5 = 4;
			Expression expression3 = null;
			MethodInfo methodInfo3 = methodof(IEnumerable<!!0>.Count(Func<!!0, bool>));
			Expression[] array4 = new Expression[2];
			array4[0] = Expression.Property(parameterExpression, methodof(DeviceCategory.get_Devices()));
			int num6 = 1;
			parameterExpression2 = Expression.Parameter(typeof(Device), "d");
			array4[num6] = Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression2, methodof(Device.get_LastReachable())), Expression.Constant(null, typeof(bool?))), new ParameterExpression[] { parameterExpression2 });
			array[num5] = Expression.Call(expression3, methodInfo3, array4);
			List<Home.Row> list = await queryable.Select<DeviceCategory, Home.Row>(Expression.Lambda<Func<DeviceCategory, Home.Row>>(Expression.New(constructorInfo, array), new ParameterExpression[] { parameterExpression })).ToListAsync<Home.Row>(this._cts.Token);
			this._rows = list;
			this._rows = this._rows.Where<Home.Row>((Home.Row r) => !DeviceCategory.IsHiddenKey(r.Key)).ToList<Home.Row>();
			List<Home.Row> list2 = this._rows.Where<Home.Row>((Home.Row r) => r.Key != "ue").ToList<Home.Row>();
			this._total = list2.Sum<Home.Row>((Home.Row r) => r.Total);
			this._up = list2.Sum<Home.Row>((Home.Row r) => r.Up);
			this._down = list2.Sum<Home.Row>((Home.Row r) => r.Down);
			this._unknown = list2.Sum<Home.Row>((Home.Row r) => r.Unknown);
			IQueryable<Device> devices = db.Devices;
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			IQueryable<Device> queryable2 = devices.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			IQueryable<Device> queryable3 = queryable2.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			List<Device> list3 = await queryable3.Include<Device, Device>(Expression.Lambda<Func<Device, Device>>(Expression.Property(parameterExpression, methodof(Device.get_ParentDevice())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(this._cts.Token);
			list3 = list3.Where<Device>((Device d) => !DeviceCategory.IsHiddenKey(d.Category.Key)).ToList<Device>();
			this._ueState = new Dictionary<int, string>();
			foreach (Device device in list3.Where<Device>((Device x) => x.Category.Key == "ue"))
			{
				this._ueState[device.Id] = Home.SnapMetricStr(SnapshotSerializer.Deserialize(device.LastSnapshotJson), "State");
			}
			this._bbuUptime = null;
			foreach (Device device2 in list3.Where<Device>((Device x) => x.Category.Key == "bbu"))
			{
				this._bbuUptime = Home.SnapMetricStr(SnapshotSerializer.Deserialize(device2.LastSnapshotJson), "Uptime");
				if (this._bbuUptime != null)
				{
					break;
				}
			}
			object obj = null;
			this._coreUptimeRaw = obj;
			this._coreUptime = obj;
			foreach (Device device3 in list3.Where<Device>((Device x) => x.Category.Key == "mobile"))
			{
				string text = Home.SnapMetricStr(SnapshotSerializer.Deserialize(device3.LastSnapshotJson), "Uptime");
				if (text != null)
				{
					this._coreUptimeRaw = text;
					double? num7 = UptimeFormat.ToSeconds(text);
					this._coreUptime = ((num7 == null) ? text : UptimeFormat.FromSeconds((long)num7.GetValueOrDefault()));
					break;
				}
			}
			Home.<>c__DisplayClass40_0 CS$<>8__locals1;
			CS$<>8__locals1.byParent = list3.Where<Device>((Device d) => d.ParentDeviceId != null).ToLookup<Device, int>((Device d) => d.ParentDeviceId.Value);
			CS$<>8__locals1.flat = new List<Home.DeviceNode>();
			foreach (Device device4 in from d in list3
				where d.ParentDeviceId == null
				orderby d.Category.DisplayOrder, d.Name
				select d)
			{
				Home.<LoadStatusAsync>g__Walk|40_9(device4, 0, ref CS$<>8__locals1);
			}
			this._devices = CS$<>8__locals1.flat;
			this._ues = (from x in list3
				where x.Category.Key == "ue"
				orderby x.Name
				select x).ToList<Device>();
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
		private async Task LoadHistoryAsync(ApplicationDbContext db)
		{
			Home.<>c__DisplayClass41_0 CS$<>8__locals1 = new Home.<>c__DisplayClass41_0();
			this._historyLoadedAt = DateTime.UtcNow;
			CS$<>8__locals1.cutoff = this._historyLoadedAt - TimeSpan.FromMinutes((long)this._windowMinutes);
			DateTime cutoff = CS$<>8__locals1.cutoff;
			DateTime historyLoadedAt = this._historyLoadedAt;
			this._windowFrom = cutoff;
			this._windowTo = historyLoadedAt;
			IQueryable<ThroughputSample> throughputSamples = db.ThroughputSamples;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
			IQueryable<ThroughputSample> queryable = throughputSamples.Where<ThroughputSample>(Expression.Lambda<Func<ThroughputSample, bool>>(Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(Home.<>c__DisplayClass41_0)), fieldof(Home.<>c__DisplayClass41_0.cutoff)), false, methodof(DateTime.op_GreaterThanOrEqual(DateTime, DateTime))), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
			double? num = await queryable.MaxAsync<ThroughputSample, double?>(Expression.Lambda<Func<ThroughputSample, double?>>(Expression.Convert(Expression.Property(parameterExpression, methodof(ThroughputSample.get_DlMbps())), typeof(double?)), new ParameterExpression[] { parameterExpression }), this._cts.Token);
			this._peakDl = num;
			IQueryable<ThroughputSample> throughputSamples2 = db.ThroughputSamples;
			parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
			IQueryable<ThroughputSample> queryable2 = throughputSamples2.Where<ThroughputSample>(Expression.Lambda<Func<ThroughputSample, bool>>(Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(Home.<>c__DisplayClass41_0)), fieldof(Home.<>c__DisplayClass41_0.cutoff)), false, methodof(DateTime.op_GreaterThanOrEqual(DateTime, DateTime))), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
			num = await queryable2.MaxAsync<ThroughputSample, double?>(Expression.Lambda<Func<ThroughputSample, double?>>(Expression.Convert(Expression.Property(parameterExpression, methodof(ThroughputSample.get_UlMbps())), typeof(double?)), new ParameterExpression[] { parameterExpression }), this._cts.Token);
			this._peakUl = num;
			this._dlChart = new List<ChartSeries>();
			this._ulChart = new List<ChartSeries>();
			this._latencyChart = new List<ChartSeries>();
			this._strip = new List<StatusStrip.Row>();
			if (this._ues.Count != 0)
			{
				CS$<>8__locals1.ueIds = this._ues.Select<Device, int>((Device x) => x.Id).ToList<int>();
				IQueryable<DevicePollResult> devicePollResults = db.DevicePollResults;
				parameterExpression = Expression.Parameter(typeof(DevicePollResult), "r");
				Dictionary<int, IEnumerable<DevicePollResult>> dictionary = (from r in await devicePollResults.Where<DevicePollResult>(Expression.Lambda<Func<DevicePollResult, bool>>(Expression.AndAlso(Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(Home.<>c__DisplayClass41_0)), fieldof(Home.<>c__DisplayClass41_0.ueIds)), methodof(List<int>.Contains(T)), new Expression[] { Expression.Property(parameterExpression, methodof(DevicePollResult.get_DeviceId())) }), Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, methodof(DevicePollResult.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(Home.<>c__DisplayClass41_0)), fieldof(Home.<>c__DisplayClass41_0.cutoff)), false, methodof(DateTime.op_GreaterThanOrEqual(DateTime, DateTime)))), new ParameterExpression[] { parameterExpression })).ToListAsync<DevicePollResult>(this._cts.Token)
					group r by r.DeviceId).ToDictionary<IGrouping<int, DevicePollResult>, int, IEnumerable<DevicePollResult>>((IGrouping<int, DevicePollResult> g) => g.Key, (IGrouping<int, DevicePollResult> g) => g);
				for (int i = 0; i < this._ues.Count; i++)
				{
					IEnumerable<DevicePollResult> enumerable;
					if (dictionary.TryGetValue(this._ues[i].Id, out enumerable))
					{
						List<ValueTuple<DateTime, DeviceSnapshot>> list = MetricHistory.Decode(enumerable);
						string text = Home.Palette[i % Home.Palette.Length];
						string name = this._ues[i].Name;
						List<HistoryPoint> list2 = MetricHistory.Series(list, "DL", "UE");
						List<HistoryPoint> list3 = MetricHistory.Series(list, "UL", "UE");
						List<HistoryPoint> list4 = MetricHistory.Series(list, "Latency", "UE");
						if (list2.Count > 0)
						{
							this._dlChart.Add(new ChartSeries(name, text, list2, false));
						}
						if (list3.Count > 0)
						{
							this._ulChart.Add(new ChartSeries(name, text, list3, false));
						}
						if (list4.Count > 0)
						{
							this._latencyChart.Add(new ChartSeries(name, text, list4, false));
						}
						List<LinkSegment> list5 = ConnectivityTimeline.Build(enumerable, CS$<>8__locals1.cutoff, this._historyLoadedAt);
						if (list5.Any<LinkSegment>(delegate(LinkSegment seg)
						{
							LinkState state = seg.State;
							return state <= LinkState.Idle;
						}))
						{
							this._strip.Add(new StatusStrip.Row(name, list5));
						}
					}
				}
				IQueryable<ThroughputSample> throughputSamples3 = db.ThroughputSamples;
				parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
				IQueryable<ThroughputSample> queryable3 = throughputSamples3.Where<ThroughputSample>(Expression.Lambda<Func<ThroughputSample, bool>>(Expression.GreaterThanOrEqual(Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())), Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(Home.<>c__DisplayClass41_0)), fieldof(Home.<>c__DisplayClass41_0.cutoff)), false, methodof(DateTime.op_GreaterThanOrEqual(DateTime, DateTime))), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
				IQueryable<ThroughputSample> queryable4 = queryable3.OrderBy<ThroughputSample, DateTime>(Expression.Lambda<Func<ThroughputSample, DateTime>>(Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(ThroughputSample), "s");
				var list6 = await queryable4.Select(Expression.Lambda(Expression.New(methodof(<>f__AnonymousType19<DateTime, double, double>..ctor(<Timestamp>j__TPar, <DlMbps>j__TPar, <UlMbps>j__TPar)), new Expression[]
				{
					Expression.Property(parameterExpression, methodof(ThroughputSample.get_Timestamp())),
					Expression.Property(parameterExpression, methodof(ThroughputSample.get_DlMbps())),
					Expression.Property(parameterExpression, methodof(ThroughputSample.get_UlMbps()))
				}, new MemberInfo[]
				{
					methodof(<>f__AnonymousType19<DateTime, double, double>.get_Timestamp()),
					methodof(<>f__AnonymousType19<DateTime, double, double>.get_DlMbps()),
					methodof(<>f__AnonymousType19<DateTime, double, double>.get_UlMbps())
				}), new ParameterExpression[] { parameterExpression })).ToListAsync(this._cts.Token);
				if (list6.Count > 1)
				{
					this._dlChart.Insert(0, new ChartSeries("Total", "#0f172a", list6.Select(s => new HistoryPoint(s.Timestamp, s.DlMbps)).ToList<HistoryPoint>(), true));
					this._ulChart.Insert(0, new ChartSeries("Total", "#0f172a", list6.Select(s => new HistoryPoint(s.Timestamp, s.UlMbps)).ToList<HistoryPoint>(), true));
				}
				else
				{
					Home.AddTotal(this._dlChart);
					Home.AddTotal(this._ulChart);
				}
				Home.Resample(this._dlChart, DownsampleMode.Max);
				Home.Resample(this._ulChart, DownsampleMode.Max);
				Home.Resample(this._latencyChart, DownsampleMode.Max);
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002C2FC File Offset: 0x0002A4FC
		private static void Resample(List<ChartSeries> chart, DownsampleMode mode)
		{
			for (int i = 0; i < chart.Count; i++)
			{
				int num = i;
				ChartSeries chartSeries = chart[i].<Clone>$();
				chartSeries.Points = MetricHistory.Downsample(chart[i].Points, 200, mode);
				chart[num] = chartSeries;
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002C34C File Offset: 0x0002A54C
		private static void AddTotal(List<ChartSeries> chart)
		{
			if (chart.Count == 0)
			{
				return;
			}
			List<HistoryPoint> list = MetricHistory.Sum(chart.Select<ChartSeries, List<HistoryPoint>>((ChartSeries s) => s.Points));
			if (list.Count > 0)
			{
				chart.Insert(0, new ChartSeries("Total", "#0f172a", list, true));
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002C3B0 File Offset: 0x0002A5B0
		private async Task RefreshLoopAsync()
		{
			try
			{
				using (PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5L)))
				{
					object obj2;
					for (;;)
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter = timer.WaitForNextTickAsync(this._cts.Token).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							await valueTaskAwaiter;
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						}
						if (!valueTaskAwaiter.GetResult())
						{
							goto Block_9;
						}
						ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
						ApplicationDbContext db = applicationDbContext;
						object obj = null;
						try
						{
							await this.LoadStatusAsync(db);
							if (DateTime.UtcNow - this._historyLoadedAt >= Home.HistoryRefresh)
							{
								await this.LoadHistoryAsync(db);
							}
							await base.InvokeAsync(new Action(base.StateHasChanged));
						}
						catch (object obj)
						{
						}
						if (db != null)
						{
							await db.DisposeAsync();
						}
						obj2 = obj;
						if (obj2 != null)
						{
							Exception ex = obj2 as Exception;
							if (ex == null)
							{
								break;
							}
							ExceptionDispatchInfo.Capture(ex).Throw();
						}
						obj = null;
						db = null;
					}
					throw obj2;
					Block_9:;
				}
				PeriodicTimer timer = null;
			}
			catch (OperationCanceledException)
			{
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002C3F3 File Offset: 0x0002A5F3
		public void Dispose()
		{
			this._cts.Cancel();
			this._cts.Dispose();
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0002C40B File Offset: 0x0002A60B
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0002C413 File Offset: 0x0002A613
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x0600075D RID: 1885 RVA: 0x0002C50C File Offset: 0x0002A70C
		[CompilerGenerated]
		internal static void <LoadStatusAsync>g__Walk|40_9(Device d, int depth, ref Home.<>c__DisplayClass40_0 A_2)
		{
			if (d.Category.Key == "ue" && !d.LastReachable.GetValueOrDefault())
			{
				return;
			}
			A_2.flat.Add(new Home.DeviceNode(d, depth));
			foreach (Device device in from c in A_2.byParent[d.Id]
				orderby c.Category.DisplayOrder, c.Name
				select c)
			{
				Home.<LoadStatusAsync>g__Walk|40_9(device, depth + 1, ref A_2);
			}
		}

		// Token: 0x0400031C RID: 796
		[Nullable(new byte[] { 2, 1 })]
		private List<Home.Row> _rows;

		// Token: 0x0400031D RID: 797
		[Nullable(new byte[] { 2, 1 })]
		private List<Home.DeviceNode> _devices;

		// Token: 0x0400031E RID: 798
		private int _total;

		// Token: 0x0400031F RID: 799
		private int _up;

		// Token: 0x04000320 RID: 800
		private int _down;

		// Token: 0x04000321 RID: 801
		private int _unknown;

		// Token: 0x04000322 RID: 802
		[Nullable(2)]
		private string _bbuUptime;

		// Token: 0x04000323 RID: 803
		[Nullable(2)]
		private string _coreUptime;

		// Token: 0x04000324 RID: 804
		[Nullable(2)]
		private string _coreUptimeRaw;

		// Token: 0x04000325 RID: 805
		private static readonly int[] WindowMinutes = new int[] { 15, 30, 60, 120, 360, 720, 1440 };

		// Token: 0x04000326 RID: 806
		private int _windowMinutes = 60;

		// Token: 0x04000327 RID: 807
		private double? _peakDl;

		// Token: 0x04000328 RID: 808
		private double? _peakUl;

		// Token: 0x04000329 RID: 809
		private DateTime _windowFrom;

		// Token: 0x0400032A RID: 810
		private DateTime _windowTo;

		// Token: 0x0400032B RID: 811
		private List<Device> _ues = new List<Device>();

		// Token: 0x0400032C RID: 812
		private static readonly TimeSpan HistoryRefresh = TimeSpan.FromSeconds(30L);

		// Token: 0x0400032D RID: 813
		private DateTime _historyLoadedAt = DateTime.MinValue;

		// Token: 0x0400032E RID: 814
		private const int MaxChartPoints = 200;

		// Token: 0x0400032F RID: 815
		[Nullable(new byte[] { 1, 2 })]
		private Dictionary<int, string> _ueState = new Dictionary<int, string>();

		// Token: 0x04000330 RID: 816
		private List<ChartSeries> _dlChart = new List<ChartSeries>();

		// Token: 0x04000331 RID: 817
		private List<ChartSeries> _ulChart = new List<ChartSeries>();

		// Token: 0x04000332 RID: 818
		private List<ChartSeries> _latencyChart = new List<ChartSeries>();

		// Token: 0x04000333 RID: 819
		private List<StatusStrip.Row> _strip = new List<StatusStrip.Row>();

		// Token: 0x04000334 RID: 820
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000335 RID: 821
		private static readonly string[] Palette = new string[] { "#2563eb", "#059669", "#d97706", "#7c3aed", "#0f766e", "#db2777", "#dc2626", "#64748b" };

		// Token: 0x04000336 RID: 822
		private const string TotalColor = "#0f172a";

		// Token: 0x020001C6 RID: 454
		[Nullable(0)]
		private class Row : IEquatable<Home.Row>
		{
			// Token: 0x06000DA3 RID: 3491 RVA: 0x0005BC46 File Offset: 0x00059E46
			public Row(string Key, int Total, int Up, int Down, int Unknown)
			{
				this.Key = Key;
				this.Total = Total;
				this.Up = Up;
				this.Down = Down;
				this.Unknown = Unknown;
				base..ctor();
			}

			// Token: 0x170002D8 RID: 728
			// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0005BC73 File Offset: 0x00059E73
			[CompilerGenerated]
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Home.Row);
				}
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0005BC7F File Offset: 0x00059E7F
			// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x0005BC87 File Offset: 0x00059E87
			public string Key { get; set; }

			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0005BC90 File Offset: 0x00059E90
			// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x0005BC98 File Offset: 0x00059E98
			public int Total { get; set; }

			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0005BCA1 File Offset: 0x00059EA1
			// (set) Token: 0x06000DAA RID: 3498 RVA: 0x0005BCA9 File Offset: 0x00059EA9
			public int Up { get; set; }

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0005BCB2 File Offset: 0x00059EB2
			// (set) Token: 0x06000DAC RID: 3500 RVA: 0x0005BCBA File Offset: 0x00059EBA
			public int Down { get; set; }

			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0005BCC3 File Offset: 0x00059EC3
			// (set) Token: 0x06000DAE RID: 3502 RVA: 0x0005BCCB File Offset: 0x00059ECB
			public int Unknown { get; set; }

			// Token: 0x06000DAF RID: 3503 RVA: 0x0005BCD4 File Offset: 0x00059ED4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Row");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000DB0 RID: 3504 RVA: 0x0005BD20 File Offset: 0x00059F20
			[CompilerGenerated]
			protected virtual bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Key = ");
				builder.Append(this.Key);
				builder.Append(", Total = ");
				builder.Append(this.Total.ToString());
				builder.Append(", Up = ");
				builder.Append(this.Up.ToString());
				builder.Append(", Down = ");
				builder.Append(this.Down.ToString());
				builder.Append(", Unknown = ");
				builder.Append(this.Unknown.ToString());
				return true;
			}

			// Token: 0x06000DB1 RID: 3505 RVA: 0x0005BDE8 File Offset: 0x00059FE8
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(Home.Row left, Home.Row right)
			{
				return !(left == right);
			}

			// Token: 0x06000DB2 RID: 3506 RVA: 0x0005BDF4 File Offset: 0x00059FF4
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(Home.Row left, Home.Row right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000DB3 RID: 3507 RVA: 0x0005BE08 File Offset: 0x0005A008
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Key>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Total>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Up>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Down>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Unknown>k__BackingField);
			}

			// Token: 0x06000DB4 RID: 3508 RVA: 0x0005BE98 File Offset: 0x0005A098
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as Home.Row);
			}

			// Token: 0x06000DB5 RID: 3509 RVA: 0x0005BEA8 File Offset: 0x0005A0A8
			[NullableContext(2)]
			[CompilerGenerated]
			public virtual bool Equals(Home.Row other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Key>k__BackingField, other.<Key>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Total>k__BackingField, other.<Total>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Up>k__BackingField, other.<Up>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Down>k__BackingField, other.<Down>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Unknown>k__BackingField, other.<Unknown>k__BackingField));
			}

			// Token: 0x06000DB7 RID: 3511 RVA: 0x0005BF58 File Offset: 0x0005A158
			[CompilerGenerated]
			protected Row(Home.Row original)
			{
				this.Key = original.<Key>k__BackingField;
				this.Total = original.<Total>k__BackingField;
				this.Up = original.<Up>k__BackingField;
				this.Down = original.<Down>k__BackingField;
				this.Unknown = original.<Unknown>k__BackingField;
			}

			// Token: 0x06000DB8 RID: 3512 RVA: 0x0005BFA7 File Offset: 0x0005A1A7
			[CompilerGenerated]
			public void Deconstruct(out string Key, out int Total, out int Up, out int Down, out int Unknown)
			{
				Key = this.Key;
				Total = this.Total;
				Up = this.Up;
				Down = this.Down;
				Unknown = this.Unknown;
			}
		}

		// Token: 0x020001C7 RID: 455
		[Nullable(0)]
		private sealed class DeviceNode : IEquatable<Home.DeviceNode>
		{
			// Token: 0x06000DB9 RID: 3513 RVA: 0x0005BFD3 File Offset: 0x0005A1D3
			public DeviceNode(Device Device, int Depth)
			{
				this.Device = Device;
				this.Depth = Depth;
				base..ctor();
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0005BFE9 File Offset: 0x0005A1E9
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Home.DeviceNode);
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0005BFF5 File Offset: 0x0005A1F5
			// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0005BFFD File Offset: 0x0005A1FD
			public Device Device { get; set; }

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0005C006 File Offset: 0x0005A206
			// (set) Token: 0x06000DBE RID: 3518 RVA: 0x0005C00E File Offset: 0x0005A20E
			public int Depth { get; set; }

			// Token: 0x06000DBF RID: 3519 RVA: 0x0005C018 File Offset: 0x0005A218
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DeviceNode");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000DC0 RID: 3520 RVA: 0x0005C064 File Offset: 0x0005A264
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Device = ");
				builder.Append(this.Device);
				builder.Append(", Depth = ");
				builder.Append(this.Depth.ToString());
				return true;
			}

			// Token: 0x06000DC1 RID: 3521 RVA: 0x0005C0B7 File Offset: 0x0005A2B7
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(Home.DeviceNode left, Home.DeviceNode right)
			{
				return !(left == right);
			}

			// Token: 0x06000DC2 RID: 3522 RVA: 0x0005C0C3 File Offset: 0x0005A2C3
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(Home.DeviceNode left, Home.DeviceNode right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06000DC3 RID: 3523 RVA: 0x0005C0D7 File Offset: 0x0005A2D7
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Device>.Default.GetHashCode(this.<Device>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Depth>k__BackingField);
			}

			// Token: 0x06000DC4 RID: 3524 RVA: 0x0005C117 File Offset: 0x0005A317
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as Home.DeviceNode);
			}

			// Token: 0x06000DC5 RID: 3525 RVA: 0x0005C128 File Offset: 0x0005A328
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(Home.DeviceNode other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Device>.Default.Equals(this.<Device>k__BackingField, other.<Device>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Depth>k__BackingField, other.<Depth>k__BackingField));
			}

			// Token: 0x06000DC7 RID: 3527 RVA: 0x0005C189 File Offset: 0x0005A389
			[CompilerGenerated]
			private DeviceNode(Home.DeviceNode original)
			{
				this.Device = original.<Device>k__BackingField;
				this.Depth = original.<Depth>k__BackingField;
			}

			// Token: 0x06000DC8 RID: 3528 RVA: 0x0005C1A9 File Offset: 0x0005A3A9
			[CompilerGenerated]
			public void Deconstruct(out Device Device, out int Depth)
			{
				Device = this.Device;
				Depth = this.Depth;
			}
		}

		// Token: 0x020001C8 RID: 456
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0005C1BB File Offset: 0x0005A3BB
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return RenderMode.InteractiveServer;
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0005C1C2 File Offset: 0x0005A3C2
			public override IComponentRenderMode Mode
			{
				get
				{
					return Home.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
