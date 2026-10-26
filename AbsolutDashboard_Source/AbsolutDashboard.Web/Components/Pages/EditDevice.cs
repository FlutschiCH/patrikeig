using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000AA RID: 170
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Authorize(Roles = "Admin")]
	[Route("/devices/detail/{Id:int}/edit")]
	[EditDevice.__PrivateComponentRenderModeAttribute]
	public class EditDevice : ComponentBase
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x0002A7F0 File Offset: 0x000289F0
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Edit device — Absolut");
			}));
			__builder.CloseComponent();
			if (this._device == null)
			{
				__builder.OpenElement(3, "p");
				__builder.AddAttribute(4, "class", "text-cocoa-700");
				__builder.AddContent(5, this._notFound ? "Device not found." : "Loading…");
				__builder.CloseElement();
				return;
			}
			__builder.OpenElement(6, "header");
			__builder.AddAttribute(7, "class", "mb-6");
			__builder.OpenElement(8, "a");
			__builder.AddAttribute(9, "class", "btn-back mb-2");
			int num = 10;
			string text = "href";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Id);
			__builder.AddAttribute(num, text, defaultInterpolatedStringHandler.ToStringAndClear());
			__builder.AddMarkupContent(11, "← ");
			__builder.AddContent(12, this._device.Name);
			__builder.CloseElement();
			__builder.AddMarkupContent(13, "\n        ");
			__builder.AddMarkupContent(14, "<h1 class=\"text-2xl font-semibold text-cocoa-900\">Edit device</h1>");
			__builder.CloseElement();
			__builder.OpenElement(15, "div");
			__builder.AddAttribute(16, "class", "grid items-start gap-6 lg:grid-cols-2");
			__builder.OpenElement(17, "div");
			__builder.AddAttribute(18, "class", "space-y-6");
			__builder.OpenComponent<DeviceForm>(19);
			__builder.AddComponentParameter(20, "Model", RuntimeHelpers.TypeCheck<DeviceFormModel>(this._model));
			__builder.AddComponentParameter(21, "Template", RuntimeHelpers.TypeCheck<DeviceTemplate>(this._template));
			__builder.AddComponentParameter(22, "Parents", RuntimeHelpers.TypeCheck<List<Device>>(this._parents));
			__builder.AddComponentParameter(23, "IsEdit", RuntimeHelpers.TypeCheck<bool>(true));
			__builder.AddComponentParameter(24, "ShowCoordinates", RuntimeHelpers.TypeCheck<bool>(this._device.Category.Key == "ru"));
			__builder.AddComponentParameter(25, "ShowSerialPort", RuntimeHelpers.TypeCheck<bool>(this._device.Category.Key == "eu"));
			__builder.AddComponentParameter(26, "Busy", RuntimeHelpers.TypeCheck<bool>(this._busy));
			__builder.AddComponentParameter(27, "Error", RuntimeHelpers.TypeCheck<string>(this._error));
			__builder.AddComponentParameter(28, "SubmitLabel", "Save changes");
			int num2 = 29;
			string text2 = "CancelHref";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("devices/detail/");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.Id);
			__builder.AddComponentParameter(num2, text2, RuntimeHelpers.TypeCheck<string>(defaultInterpolatedStringHandler2.ToStringAndClear()));
			__builder.AddComponentParameter(30, "OnSubmit", RuntimeHelpers.TypeCheck<EventCallback>(EventCallback.Factory.Create(this, new Func<Task>(this.Save))));
			__builder.CloseComponent();
			if (this._device.Category.Key == "mobile")
			{
				__builder.OpenComponent<N6Uplink>(31);
				__builder.AddComponentParameter(32, "CoreId", RuntimeHelpers.TypeCheck<int>(this.Id));
				__builder.CloseComponent();
			}
			__builder.CloseElement();
			if (this._device.Category.Key == "bbu")
			{
				__builder.OpenComponent<BbuSettings>(33);
				__builder.AddComponentParameter(34, "BbuId", RuntimeHelpers.TypeCheck<int?>(new int?(this.Id)));
				__builder.CloseComponent();
			}
			__builder.CloseElement();
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0002AB92 File Offset: 0x00028D92
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0002AB9A File Offset: 0x00028D9A
		[Parameter]
		public int Id { get; set; }

		// Token: 0x06000737 RID: 1847 RVA: 0x0002ABA4 File Offset: 0x00028DA4
		protected override async Task OnParametersSetAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<Device> devices = db.Devices;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable = devices.Include<Device, DeviceTemplate>(Expression.Lambda<Func<Device, DeviceTemplate>>(Expression.Property(parameterExpression, methodof(Device.get_Template())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				IQueryable<Device> queryable2 = queryable.Include<Device, DeviceCategory>(Expression.Lambda<Func<Device, DeviceCategory>>(Expression.Property(parameterExpression, methodof(Device.get_Category())), new ParameterExpression[] { parameterExpression }));
				parameterExpression = Expression.Parameter(typeof(Device), "d");
				this._device = await queryable2.FirstOrDefaultAsync<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(EditDevice)), methodof(EditDevice.get_Id()))), new ParameterExpression[] { parameterExpression }), default(CancellationToken));
				if (this._device == null)
				{
					this._notFound = true;
				}
				else
				{
					this._template = this._device.Template;
					IQueryable<Device> devices2 = db.Devices;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable3 = devices2.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.OrElse(Expression.OrElse(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("au", typeof(string)))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("eu", typeof(string)))), Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(EditDevice)), methodof(EditDevice.get_Id())))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					this._parents = await queryable3.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(default(CancellationToken));
					this._model = new DeviceFormModel
					{
						Name = this._device.Name,
						IpAddress = this._device.IpAddress,
						Port = this._device.Port,
						SnmpVersion = this._device.SnmpVersion,
						SnmpCommunity = null,
						SshUsername = this._device.SshUsername,
						SshPassword = null,
						ParentDeviceId = this._device.ParentDeviceId,
						SerialNumber = this._device.SerialNumber,
						ChildIdentifier = this._device.ChildIdentifier,
						SerialPort = this._device.SerialPort,
						SerialBaudRate = ((this._device.SerialBaudRate > 0) ? this._device.SerialBaudRate : 115200),
						SerialFraming = (string.IsNullOrWhiteSpace(this._device.SerialFraming) ? "8-N-1" : this._device.SerialFraming),
						CoordinateX = this._device.CoordinateX,
						CoordinateY = this._device.CoordinateY,
						CoordinateZ = this._device.CoordinateZ,
						PollIntervalSeconds = this._device.PollIntervalSeconds,
						Enabled = this._device.Enabled,
						Notes = this._device.Notes
					};
				}
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

		// Token: 0x06000738 RID: 1848 RVA: 0x0002ABE8 File Offset: 0x00028DE8
		private async Task Save()
		{
			EditDevice.<>c__DisplayClass14_0 CS$<>8__locals1 = new EditDevice.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			if (this._device != null && this._template != null)
			{
				this._serialPortTaken = null;
				EditDevice.<>c__DisplayClass14_0 CS$<>8__locals2 = CS$<>8__locals1;
				string serialPort = this._model.SerialPort;
				CS$<>8__locals2.port = ((serialPort != null) ? serialPort.Trim() : null);
				if (!string.IsNullOrEmpty(CS$<>8__locals1.port))
				{
					ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
					ApplicationDbContext check = applicationDbContext;
					object obj = null;
					int num = 0;
					try
					{
						IQueryable<Device> devices = check.Devices;
						ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
						IQueryable<Device> queryable = devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.AndAlso(Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(EditDevice)), methodof(EditDevice.get_Id()))), Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_SerialPort())), Expression.Constant(null, typeof(string)))), Expression.Equal(Expression.Call(Expression.Property(parameterExpression, methodof(Device.get_SerialPort())), methodof(string.ToLower()), Array.Empty<Expression>()), Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(EditDevice.<>c__DisplayClass14_0)), fieldof(EditDevice.<>c__DisplayClass14_0.port)), methodof(string.ToLower()), Array.Empty<Expression>()))), new ParameterExpression[] { parameterExpression }));
						parameterExpression = Expression.Parameter(typeof(Device), "d");
						this._serialPortTaken = await queryable.Select<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).FirstOrDefaultAsync<string>(default(CancellationToken));
					}
					catch (object obj)
					{
					}
					if (check != null)
					{
						await check.DisposeAsync();
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
					check = null;
				}
				this._error = this.Validate();
				if (this._error == null)
				{
					this._busy = true;
					try
					{
						ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
						ApplicationDbContext check = applicationDbContext;
						object obj = null;
						int num = 0;
						try
						{
							IQueryable<Device> devices2 = check.Devices;
							ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
							Device device = await devices2.FirstOrDefaultAsync<Device>(Expression.Lambda<Func<Device, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(Device.get_Id())), Expression.Property(Expression.Constant(this, typeof(EditDevice)), methodof(EditDevice.get_Id()))), new ParameterExpression[] { parameterExpression }), default(CancellationToken));
							if (device == null)
							{
								this._notFound = true;
								this._busy = false;
								num = 1;
							}
							else
							{
								device.Name = this._model.Name.Trim();
								device.IpAddress = (string.IsNullOrWhiteSpace(this._model.IpAddress) ? null : this._model.IpAddress.Trim());
								device.Port = this._model.Port;
								device.SnmpVersion = this._model.SnmpVersion;
								device.SshUsername = (string.IsNullOrWhiteSpace(this._model.SshUsername) ? null : this._model.SshUsername.Trim());
								device.ParentDeviceId = ((this._template.CollectionMethod == CollectionMethod.ViaParent) ? this._model.ParentDeviceId : null);
								device.SerialNumber = (string.IsNullOrWhiteSpace(this._model.SerialNumber) ? null : this._model.SerialNumber.Trim());
								device.ChildIdentifier = (string.IsNullOrWhiteSpace(this._model.ChildIdentifier) ? null : this._model.ChildIdentifier.Trim());
								device.SerialPort = (string.IsNullOrWhiteSpace(this._model.SerialPort) ? null : this._model.SerialPort.Trim());
								device.SerialBaudRate = this._model.SerialBaudRate;
								device.SerialFraming = (string.IsNullOrWhiteSpace(this._model.SerialFraming) ? null : this._model.SerialFraming.Trim());
								device.CoordinateX = this._model.CoordinateX;
								device.CoordinateY = this._model.CoordinateY;
								device.CoordinateZ = this._model.CoordinateZ;
								device.PollIntervalSeconds = this._model.PollIntervalSeconds;
								device.Enabled = this._model.Enabled;
								device.Notes = this._model.Notes;
								if (!string.IsNullOrWhiteSpace(this._model.SnmpCommunity))
								{
									device.SnmpCommunityEnc = this.Protector.Protect(this._model.SnmpCommunity);
								}
								if (!string.IsNullOrWhiteSpace(this._model.SshPassword))
								{
									device.SshPasswordEnc = this.Protector.Protect(this._model.SshPassword);
								}
								await check.SaveChangesAsync(default(CancellationToken));
								NavigationManager nav = this.Nav;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
								defaultInterpolatedStringHandler.AppendLiteral("devices/detail/");
								defaultInterpolatedStringHandler.AppendFormatted<int>(this.Id);
								nav.NavigateTo(defaultInterpolatedStringHandler.ToStringAndClear(), false, false);
							}
						}
						catch (object obj)
						{
						}
						if (check != null)
						{
							await check.DisposeAsync();
						}
						object obj2 = obj;
						if (obj2 != null)
						{
							Exception ex2 = obj2 as Exception;
							if (ex2 == null)
							{
								throw obj2;
							}
							ExceptionDispatchInfo.Capture(ex2).Throw();
						}
						if (num != 1)
						{
							obj = null;
							check = null;
						}
					}
					catch (Exception ex3)
					{
						this._error = ex3.Message;
						this._busy = false;
					}
				}
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002AC2C File Offset: 0x00028E2C
		[NullableContext(2)]
		private string Validate()
		{
			if (string.IsNullOrWhiteSpace(this._model.Name))
			{
				return "Name is required.";
			}
			string serialPortTaken = this._serialPortTaken;
			if (serialPortTaken != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Serial port '");
				defaultInterpolatedStringHandler.AppendFormatted(this._model.SerialPort.Trim());
				defaultInterpolatedStringHandler.AppendLiteral("' is already assigned to '");
				defaultInterpolatedStringHandler.AppendFormatted(serialPortTaken);
				defaultInterpolatedStringHandler.AppendLiteral("'. ");
				return defaultInterpolatedStringHandler.ToStringAndClear() + "A serial device can only be opened by one unit.";
			}
			switch (this._template.CollectionMethod)
			{
			case CollectionMethod.Snmp:
				if (string.IsNullOrWhiteSpace(this._model.IpAddress))
				{
					return "IP address is required.";
				}
				if (string.IsNullOrWhiteSpace(this._device.SnmpCommunityEnc) && string.IsNullOrWhiteSpace(this._model.SnmpCommunity))
				{
					return "SNMP community is required.";
				}
				break;
			case CollectionMethod.SshScript:
				if (string.IsNullOrWhiteSpace(this._model.IpAddress))
				{
					return "IP address is required.";
				}
				if (string.IsNullOrWhiteSpace(this._model.SshUsername))
				{
					return "SSH username is required.";
				}
				if (string.IsNullOrWhiteSpace(this._device.SshPasswordEnc) && string.IsNullOrWhiteSpace(this._model.SshPassword))
				{
					return "SSH password is required.";
				}
				break;
			case CollectionMethod.BbuWebSocket:
				if (string.IsNullOrWhiteSpace(this._model.IpAddress))
				{
					return "IP address is required.";
				}
				if (string.IsNullOrWhiteSpace(this._model.SshUsername))
				{
					return "Operator account is required.";
				}
				if (string.IsNullOrWhiteSpace(this._device.SshPasswordEnc) && string.IsNullOrWhiteSpace(this._model.SshPassword))
				{
					return "Operator password is required.";
				}
				break;
			}
			return null;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0002ADDE File Offset: 0x00028FDE
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0002ADE6 File Offset: 0x00028FE6
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002ADEF File Offset: 0x00028FEF
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0002ADF7 File Offset: 0x00028FF7
		[Inject]
		private CredentialProtector Protector { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002AE00 File Offset: 0x00029000
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0002AE08 File Offset: 0x00029008
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x0400030F RID: 783
		[Nullable(2)]
		private Device _device;

		// Token: 0x04000310 RID: 784
		[Nullable(2)]
		private DeviceTemplate _template;

		// Token: 0x04000311 RID: 785
		private List<Device> _parents = new List<Device>();

		// Token: 0x04000312 RID: 786
		private DeviceFormModel _model = new DeviceFormModel();

		// Token: 0x04000313 RID: 787
		private bool _notFound;

		// Token: 0x04000314 RID: 788
		private bool _busy;

		// Token: 0x04000315 RID: 789
		[Nullable(2)]
		private string _error;

		// Token: 0x04000316 RID: 790
		[Nullable(2)]
		private string _serialPortTaken;

		// Token: 0x020001C0 RID: 448
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0005AB19 File Offset: 0x00058D19
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return new InteractiveServerRenderMode(false);
				}
			}

			// Token: 0x170002D7 RID: 727
			// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0005AB21 File Offset: 0x00058D21
			public override IComponentRenderMode Mode
			{
				get
				{
					return EditDevice.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
