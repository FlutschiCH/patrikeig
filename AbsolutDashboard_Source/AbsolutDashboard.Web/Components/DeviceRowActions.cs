using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AbsolutDashboard.Web.Components
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	public class DeviceRowActions : ComponentBase
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x0001CC60 File Offset: 0x0001AE60
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "div");
			__builder.AddAttribute(1, "class", "flex items-center justify-end gap-1");
			__builder.OpenElement(2, "a");
			__builder.AddAttribute(3, "class", "inline-flex h-8 w-8 items-center justify-center rounded-lg text-cocoa-700 transition hover:bg-cream-100 hover:text-blush-700 disabled:opacity-50");
			__builder.AddAttribute(4, "title", "Details");
			__builder.AddAttribute(5, "aria-label", "Details");
			int num = 6;
			string text = "href";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Device.Id);
			__builder.AddAttribute(num, text, defaultInterpolatedStringHandler.ToStringAndClear());
			__builder.AddMarkupContent(7, "<svg class=\"h-4 w-4\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"><path d=\"M3 12h4l2 5 4-14 3 9h5\"></path></svg>");
			__builder.CloseElement();
			__builder.AddMarkupContent(8, "\n\n    ");
			__builder.OpenComponent<AuthorizeView>(9);
			__builder.AddComponentParameter(10, "Roles", "Admin");
			__builder.AddAttribute(11, "ChildContent", new RenderFragment<AuthenticationState>((AuthenticationState context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenElement(12, "a");
				__builder2.AddAttribute(13, "class", "inline-flex h-8 w-8 items-center justify-center rounded-lg text-cocoa-700 transition hover:bg-cream-100 hover:text-blush-700 disabled:opacity-50");
				__builder2.AddAttribute(14, "title", "Edit");
				__builder2.AddAttribute(15, "aria-label", "Edit");
				int num2 = 16;
				string text2 = "href";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("devices/detail/");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.Device.Id);
				defaultInterpolatedStringHandler2.AppendLiteral("/edit");
				__builder2.AddAttribute(num2, text2, defaultInterpolatedStringHandler2.ToStringAndClear());
				__builder2.AddMarkupContent(17, "<svg class=\"h-4 w-4\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"><circle cx=\"12\" cy=\"12\" r=\"3\"></circle>\n                <path d=\"M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z\"></path></svg>");
				__builder2.CloseElement();
				if (this.Device.CollectionMethod != CollectionMethod.ViaParent)
				{
					__builder2.OpenElement(18, "button");
					__builder2.AddAttribute(19, "class", "inline-flex h-8 w-8 items-center justify-center rounded-lg text-cocoa-700 transition hover:bg-cream-100 hover:text-blush-700 disabled:opacity-50");
					__builder2.AddAttribute<MouseEventArgs>(20, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, new Func<Task>(this.ToggleEnabled)));
					__builder2.AddAttribute(21, "disabled", this._busy);
					__builder2.AddAttribute(22, "title", this.Device.Enabled ? "Pause polling" : "Resume polling");
					__builder2.AddAttribute(23, "aria-label", this.Device.Enabled ? "Pause polling" : "Resume polling");
					if (this.Device.Enabled)
					{
						__builder2.AddMarkupContent(24, "<svg class=\"h-4 w-4\" viewBox=\"0 0 24 24\" fill=\"currentColor\"><rect x=\"6\" y=\"5\" width=\"4\" height=\"14\" rx=\"1\"></rect>\n                        <rect x=\"14\" y=\"5\" width=\"4\" height=\"14\" rx=\"1\"></rect></svg>");
					}
					else
					{
						__builder2.AddMarkupContent(25, "<svg class=\"h-4 w-4\" viewBox=\"0 0 24 24\" fill=\"currentColor\"><path d=\"M8 5v14l11-7z\"></path></svg>");
					}
					__builder2.CloseElement();
				}
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001CD61 File Offset: 0x0001AF61
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001CD69 File Offset: 0x0001AF69
		[Parameter]
		[EditorRequired]
		public Device Device { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001CD72 File Offset: 0x0001AF72
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001CD7A File Offset: 0x0001AF7A
		[Parameter]
		public EventCallback OnChanged { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001CD83 File Offset: 0x0001AF83
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0001CD8B File Offset: 0x0001AF8B
		[Nullable(new byte[] { 2, 1 })]
		[CascadingParameter]
		private Task<AuthenticationState> AuthState
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001CD94 File Offset: 0x0001AF94
		private async Task ToggleEnabled()
		{
			TaskAwaiter<bool> taskAwaiter = this.EnsureAdminAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._busy = true;
				try
				{
					DeviceRowActions.<>c__DisplayClass15_0 CS$<>8__locals1 = new DeviceRowActions.<>c__DisplayClass15_0();
					ApplicationDbContext db = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
					object obj = null;
					try
					{
						CS$<>8__locals1.newValue = !this.Device.Enabled;
						IQueryable<Device> devices = db.Devices;
						ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
						await devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceRowActions)), methodof(DeviceRowActions.get_Device())), methodof(Device.get_Id()))), new ParameterExpression[] { parameterExpression })).ExecuteUpdateAsync<Device>(delegate([Nullable(new byte[] { 1, 0 })] UpdateSettersBuilder<Device> s)
						{
							s.SetProperty<bool>((Device d) => d.Enabled, CS$<>8__locals1.newValue);
						}, default(CancellationToken));
						await this.OnChanged.InvokeAsync();
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
					obj = null;
					CS$<>8__locals1 = null;
					db = null;
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		private async Task<bool> EnsureAdminAsync()
		{
			bool flag;
			if (this.AuthState == null)
			{
				flag = false;
			}
			else
			{
				flag = (await this.AuthState).User.IsInRole("Admin");
			}
			return flag;
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001CE1B File Offset: 0x0001B01B
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001CE23 File Offset: 0x0001B023
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x04000255 RID: 597
		private const string IconBtn = "inline-flex h-8 w-8 items-center justify-center rounded-lg text-cocoa-700 transition hover:bg-cream-100 hover:text-blush-700 disabled:opacity-50";

		// Token: 0x04000259 RID: 601
		private bool _busy;
	}
}
