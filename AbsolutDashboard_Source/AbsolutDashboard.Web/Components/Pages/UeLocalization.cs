using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Route("/apps/ue-localization")]
	[UeLocalization.__PrivateComponentRenderModeAttribute]
	public class UeLocalization : ComponentBase, IDisposable
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "UE Localization — Absolut");
			}));
			__builder.CloseComponent();
			__builder.AddMarkupContent(3, "\n\n");
			__builder.AddMarkupContent(4, "<header class=\"mb-6\"><h1 class=\"text-2xl font-semibold text-cocoa-900\">UE Localization</h1>\n    <p class=\"mt-1 text-sm text-cocoa-700\">Where the connected handsets are on site.</p></header>\n\n");
			__builder.OpenComponent<PreviewBanner>(5);
			__builder.AddComponentParameter(6, "Note", "handset positions are not calculated yet; the devices below are live.");
			__builder.CloseComponent();
			__builder.AddMarkupContent(7, "\n\n");
			__builder.OpenElement(8, "section");
			__builder.AddAttribute(9, "class", "mb-8");
			__builder.AddMarkupContent(10, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">UE positions</h2>");
			if (this._ues == null)
			{
				__builder.AddMarkupContent(11, "<p class=\"text-cocoa-700\">Loading…</p>");
			}
			else if (this._ues.Count == 0)
			{
				__builder.AddMarkupContent(12, "<div class=\"card p-8 text-center text-cocoa-700\">\n            No UE devices yet. Handsets appear automatically once the 5G core reports them.\n        </div>");
			}
			else
			{
				__builder.OpenElement(13, "div");
				__builder.AddAttribute(14, "class", "card overflow-x-auto");
				__builder.OpenElement(15, "table");
				__builder.AddAttribute(16, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(17, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"px-4 py-3\">MSIN</th>\n                        <th class=\"hidden px-4 py-3 lg:table-cell\">Description</th>\n                        <th class=\"px-4 py-3\">X / Y / Z</th>\n                        <th class=\"hidden px-4 py-3 md:table-cell\">Base station</th>\n                        <th class=\"px-4 py-3\">Status</th>\n                        <th class=\"px-4 py-3\">IP</th></tr></thead>\n                ");
				__builder.OpenElement(18, "tbody");
				foreach (Device device in this._ues)
				{
					ValueTuple<string, string> valueTuple = DeviceStatusView.StateBadge(device.LastReachable, this.Metric(device, "State"));
					string text = this.Metric(device, "Base station");
					__builder.OpenElement(19, "tr");
					__builder.AddAttribute(20, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(21, "td");
					__builder.AddAttribute(22, "class", "px-4 py-3 font-medium text-cocoa-900");
					__builder.AddContent(23, this.Metric(device, "MSIN") ?? device.Name);
					__builder.CloseElement();
					__builder.AddMarkupContent(24, "\n                            ");
					__builder.OpenElement(25, "td");
					__builder.AddAttribute(26, "class", "hidden px-4 py-3 text-cocoa-700 lg:table-cell");
					__builder.AddContent(27, string.IsNullOrWhiteSpace(device.Notes) ? "—" : device.Notes);
					__builder.CloseElement();
					__builder.AddMarkupContent(28, "\n                            ");
					__builder.OpenElement(29, "td");
					__builder.AddAttribute(30, "class", "px-4 py-3 text-cocoa-700");
					__builder.AddContent(31, UeLocalization.Xyz(device));
					__builder.CloseElement();
					__builder.AddMarkupContent(32, "\n                            ");
					__builder.OpenElement(33, "td");
					__builder.AddAttribute(34, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					if (text == null)
					{
						__builder.AddMarkupContent(35, "<span>—</span>");
					}
					else
					{
						int? num = this.BaseStationId(device);
						if (num != null)
						{
							int valueOrDefault = num.GetValueOrDefault();
							__builder.OpenElement(36, "a");
							__builder.AddAttribute(37, "class", "text-blush-700 hover:underline");
							int num2 = 38;
							string text2 = "href";
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
							defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
							defaultInterpolatedStringHandler.AppendFormatted<int>(valueOrDefault);
							__builder.AddAttribute(num2, text2, defaultInterpolatedStringHandler.ToStringAndClear());
							__builder.AddAttribute(39, "title", "Open this base station");
							__builder.AddContent(40, text);
							__builder.CloseElement();
						}
						else
						{
							__builder.OpenElement(41, "span");
							__builder.AddContent(42, text);
							__builder.CloseElement();
						}
					}
					__builder.CloseElement();
					__builder.AddMarkupContent(43, "\n                            ");
					__builder.OpenElement(44, "td");
					__builder.AddAttribute(45, "class", "px-4 py-3");
					__builder.OpenElement(46, "span");
					__builder.AddAttribute(47, "class", valueTuple.Item1);
					__builder.AddAttribute(48, "title", device.LastError);
					__builder.AddContent(49, valueTuple.Item2);
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(50, "\n                            ");
					__builder.OpenElement(51, "td");
					__builder.AddAttribute(52, "class", "px-4 py-3 text-cocoa-700");
					__builder.AddContent(53, this.Metric(device, "IP") ?? "—");
					__builder.CloseElement();
					__builder.CloseElement();
				}
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.CloseElement();
				__builder.AddMarkupContent(54, "\n        ");
				__builder.AddMarkupContent(55, "<p class=\"mt-2 text-xs text-cocoa-700\">\n            Positions stay blank until triangulation is implemented — it will compute them from the\n            PICO sender coordinates below and write them back to each handset.\n        </p>");
			}
			__builder.CloseElement();
			__builder.AddMarkupContent(56, "\n\n");
			__builder.OpenElement(57, "section");
			__builder.AddMarkupContent(58, "<h2 class=\"mb-3 text-lg font-semibold text-cocoa-900\">PICO senders</h2>\n    ");
			__builder.AddMarkupContent(59, "<p class=\"mb-3 text-sm text-cocoa-700\">\n        The reference points triangulation will measure against. Coordinates are set by hand on each\n        unit's edit form.\n    </p>");
			if (this._rus == null)
			{
				__builder.AddMarkupContent(60, "<p class=\"text-cocoa-700\">Loading…</p>");
			}
			else if (this._rus.Count == 0)
			{
				__builder.AddMarkupContent(61, "<div class=\"card p-8 text-center text-cocoa-700\">\n            No PICO senders yet. They appear automatically once a base station reports its topology.\n        </div>");
			}
			else
			{
				__builder.OpenElement(62, "div");
				__builder.AddAttribute(63, "class", "card overflow-x-auto");
				__builder.OpenElement(64, "table");
				__builder.AddAttribute(65, "class", "w-full text-left text-sm");
				__builder.AddMarkupContent(66, "<thead class=\"border-b border-cream-200 text-cocoa-700\"><tr><th class=\"px-4 py-3\">PICO sender</th>\n                        <th class=\"hidden px-4 py-3 md:table-cell\">Serial</th>\n                        <th class=\"px-4 py-3\">X / Y / Z</th>\n                        <th class=\"px-4 py-3\">Status</th></tr></thead>\n                ");
				__builder.OpenElement(67, "tbody");
				foreach (Device device2 in this._rus)
				{
					ValueTuple<string, string> valueTuple2 = DeviceStatusView.Pill(device2.LastReachable);
					__builder.OpenElement(68, "tr");
					__builder.AddAttribute(69, "class", "border-b border-cream-100 last:border-0");
					__builder.OpenElement(70, "td");
					__builder.AddAttribute(71, "class", "px-4 py-3 font-medium text-cocoa-900");
					__builder.OpenElement(72, "a");
					__builder.AddAttribute(73, "class", "text-blush-700 hover:underline");
					int num3 = 74;
					string text3 = "href";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("devices/detail/");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(device2.Id);
					__builder.AddAttribute(num3, text3, defaultInterpolatedStringHandler2.ToStringAndClear());
					__builder.AddContent(75, device2.Name);
					__builder.CloseElement();
					__builder.CloseElement();
					__builder.AddMarkupContent(76, "\n                            ");
					__builder.OpenElement(77, "td");
					__builder.AddAttribute(78, "class", "hidden px-4 py-3 text-cocoa-700 md:table-cell");
					__builder.AddContent(79, string.IsNullOrWhiteSpace(device2.SerialNumber) ? "—" : device2.SerialNumber);
					__builder.CloseElement();
					__builder.AddMarkupContent(80, "\n                            ");
					__builder.OpenElement(81, "td");
					__builder.AddAttribute(82, "class", "px-4 py-3 text-cocoa-700");
					__builder.AddContent(83, UeLocalization.Xyz(device2));
					__builder.CloseElement();
					__builder.AddMarkupContent(84, "\n                            ");
					__builder.OpenElement(85, "td");
					__builder.AddAttribute(86, "class", "px-4 py-3");
					__builder.OpenElement(87, "span");
					__builder.AddAttribute(88, "class", valueTuple2.Item1);
					__builder.AddAttribute(89, "title", device2.LastError);
					__builder.AddContent(90, valueTuple2.Item2);
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

		// Token: 0x0600075F RID: 1887 RVA: 0x0002CD8C File Offset: 0x0002AF8C
		protected override async Task OnInitializedAsync()
		{
			await this.LoadAsync();
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0002CDCF File Offset: 0x0002AFCF
		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender && base.RendererInfo.IsInteractive)
			{
				this.RefreshLoopAsync();
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
		private async Task LoadAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(this._cts.Token);
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.OrElse(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("ue", typeof(string))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("ru", typeof(string)))), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				List<Device> list = await queryable2.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(this._cts.Token);
				this._ues = list.Where<Device>((Device d) => d.Category.Key == "ue").ToList<Device>();
				this._rus = list.Where<Device>((Device d) => d.Category.Key == "ru").ToList<Device>();
				this._snapshots = list.ToDictionary<Device, int, DeviceSnapshot>((Device d) => d.Id, (Device d) => SnapshotSerializer.Deserialize(d.LastSnapshotJson));
				this._bbuByGnb = await BaseStationLookup.ByGnbIdAsync(db, this._cts.Token);
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

		// Token: 0x06000762 RID: 1890 RVA: 0x0002CE2C File Offset: 0x0002B02C
		private int? BaseStationId(Device d)
		{
			DeviceSnapshot deviceSnapshot;
			if (!this._snapshots.TryGetValue(d.Id, out deviceSnapshot))
			{
				return null;
			}
			return BaseStationLookup.Resolve(this._bbuByGnb, deviceSnapshot);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002CE64 File Offset: 0x0002B064
		[return: Nullable(2)]
		private string Metric(Device d, string label)
		{
			DeviceSnapshot deviceSnapshot;
			if (!this._snapshots.TryGetValue(d.Id, out deviceSnapshot) || deviceSnapshot == null)
			{
				return null;
			}
			Metric metric = deviceSnapshot.Metrics.FirstOrDefault<Metric>((Metric x) => x.Label == label);
			if (metric == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(metric.Unit))
			{
				return metric.Value + " " + metric.Unit;
			}
			return metric.Value;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		private static string Xyz(Device d)
		{
			if (d.CoordinateX != null || d.CoordinateY != null || d.CoordinateZ != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendFormatted(UeLocalization.N(d.CoordinateX));
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted(UeLocalization.N(d.CoordinateY));
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted(UeLocalization.N(d.CoordinateZ));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return "—";
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002CF84 File Offset: 0x0002B184
		private static string N(double? v)
		{
			return ((v != null) ? v.GetValueOrDefault().ToString("0.##", CultureInfo.InvariantCulture) : null) ?? "—";
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002CFC0 File Offset: 0x0002B1C0
		private async Task RefreshLoopAsync()
		{
			try
			{
				using (PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10L)))
				{
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
							break;
						}
						await this.LoadAsync();
						await base.InvokeAsync(new Action(base.StateHasChanged));
					}
				}
				PeriodicTimer timer = null;
			}
			catch (OperationCanceledException)
			{
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002D003 File Offset: 0x0002B203
		public void Dispose()
		{
			this._cts.Cancel();
			this._cts.Dispose();
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002D01B File Offset: 0x0002B21B
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0002D023 File Offset: 0x0002B223
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x04000338 RID: 824
		[Nullable(new byte[] { 2, 1 })]
		private List<Device> _ues;

		// Token: 0x04000339 RID: 825
		[Nullable(new byte[] { 2, 1 })]
		private List<Device> _rus;

		// Token: 0x0400033A RID: 826
		[Nullable(new byte[] { 1, 2 })]
		private Dictionary<int, DeviceSnapshot> _snapshots = new Dictionary<int, DeviceSnapshot>();

		// Token: 0x0400033B RID: 827
		private Dictionary<string, int> _bbuByGnb = new Dictionary<string, int>();

		// Token: 0x0400033C RID: 828
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x020001D3 RID: 467
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000DFB RID: 3579 RVA: 0x0005E0DE File Offset: 0x0005C2DE
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return RenderMode.InteractiveServer;
				}
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0005E0E5 File Offset: 0x0005C2E5
			public override IComponentRenderMode Mode
			{
				get
				{
					return UeLocalization.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
