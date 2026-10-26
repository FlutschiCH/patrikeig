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
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	[Authorize(Roles = "Admin")]
	[Route("/devices/{Key}/add")]
	[AddDevice.__PrivateComponentRenderModeAttribute]
	public class AddDevice : ComponentBase
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x00022F58 File Offset: 0x00021158
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddMarkupContent(2, "Add device — Absolut");
			}));
			__builder.CloseComponent();
			if (this._category == null)
			{
				__builder.OpenElement(3, "p");
				__builder.AddAttribute(4, "class", "text-cocoa-700");
				__builder.AddContent(5, this._notFound ? "Unknown category." : "Loading…");
				__builder.CloseElement();
				return;
			}
			if (this._notAllowed)
			{
				__builder.OpenElement(6, "header");
				__builder.AddAttribute(7, "class", "mb-6");
				__builder.OpenElement(8, "a");
				__builder.AddAttribute(9, "class", "btn-back mb-2");
				__builder.AddAttribute(10, "href", "devices/" + this.Key);
				__builder.AddMarkupContent(11, "← ");
				__builder.AddContent(12, this._category.Name);
				__builder.CloseElement();
				__builder.AddMarkupContent(13, "\n        ");
				__builder.AddMarkupContent(14, "<h1 class=\"text-2xl font-semibold text-cocoa-900\">Add a device</h1>");
				__builder.CloseElement();
				__builder.AddMarkupContent(15, "\n    ");
				__builder.OpenElement(16, "div");
				__builder.AddAttribute(17, "class", "card p-8 text-center text-cocoa-700");
				__builder.AddContent(18, this._category.Name);
				__builder.AddMarkupContent(19, " are discovered automatically by the poller and can't be added manually.\n    ");
				__builder.CloseElement();
				return;
			}
			__builder.OpenElement(20, "header");
			__builder.AddAttribute(21, "class", "mb-6");
			__builder.OpenElement(22, "a");
			__builder.AddAttribute(23, "class", "btn-back mb-2");
			__builder.AddAttribute(24, "href", "devices/" + this.Key);
			__builder.AddMarkupContent(25, "← ");
			__builder.AddContent(26, this._category.Name);
			__builder.CloseElement();
			__builder.AddMarkupContent(27, "\n        ");
			__builder.AddMarkupContent(28, "<h1 class=\"text-2xl font-semibold text-cocoa-900\">Add a device</h1>");
			__builder.CloseElement();
			if (this._template == null)
			{
				__builder.AddMarkupContent(29, "<p class=\"mb-3 text-sm text-cocoa-700\">Choose a device type from the catalog:</p>\n        ");
				__builder.OpenElement(30, "div");
				__builder.AddAttribute(31, "class", "grid gap-3 sm:grid-cols-2");
				using (List<DeviceTemplate>.Enumerator enumerator = this._templates.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DeviceTemplate t = enumerator.Current;
						__builder.OpenElement(32, "button");
						__builder.AddAttribute(33, "class", "card p-4 text-left transition hover:ring-blush-300");
						__builder.AddAttribute<MouseEventArgs>(34, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
						{
							this.SelectTemplate(t);
						}));
						__builder.OpenElement(35, "div");
						__builder.AddAttribute(36, "class", "flex items-center justify-between");
						__builder.OpenElement(37, "span");
						__builder.AddAttribute(38, "class", "font-semibold text-cocoa-900");
						__builder.AddContent(39, t.Name);
						__builder.CloseElement();
						__builder.AddMarkupContent(40, "\n                        ");
						__builder.OpenElement(41, "span");
						__builder.AddAttribute(42, "class", "pill pill-unknown");
						__builder.AddContent(43, DeviceStatusView.MethodLabel(t.CollectionMethod));
						__builder.CloseElement();
						__builder.CloseElement();
						if (t.Description != null)
						{
							__builder.OpenElement(44, "p");
							__builder.AddAttribute(45, "class", "mt-1 text-xs text-cocoa-700");
							__builder.AddContent(46, t.Description);
							__builder.CloseElement();
						}
						__builder.CloseElement();
					}
				}
				__builder.CloseElement();
				return;
			}
			__builder.OpenComponent<DeviceForm>(47);
			__builder.AddComponentParameter(48, "Model", RuntimeHelpers.TypeCheck<DeviceFormModel>(this._model));
			__builder.AddComponentParameter(49, "Template", RuntimeHelpers.TypeCheck<DeviceTemplate>(this._template));
			__builder.AddComponentParameter(50, "Parents", RuntimeHelpers.TypeCheck<List<Device>>(this._parents));
			__builder.AddComponentParameter(51, "IsEdit", RuntimeHelpers.TypeCheck<bool>(false));
			__builder.AddComponentParameter(52, "ShowCoordinates", RuntimeHelpers.TypeCheck<bool>(this._category.Key == "ru"));
			__builder.AddComponentParameter(53, "Busy", RuntimeHelpers.TypeCheck<bool>(this._busy));
			__builder.AddComponentParameter(54, "Error", RuntimeHelpers.TypeCheck<string>(this._error));
			__builder.AddComponentParameter(55, "SubmitLabel", "Create device");
			__builder.AddComponentParameter(56, "CancelHref", RuntimeHelpers.TypeCheck<string>("devices/" + this.Key));
			__builder.AddComponentParameter(57, "OnSubmit", RuntimeHelpers.TypeCheck<EventCallback>(EventCallback.Factory.Create(this, new Func<Task>(this.Save))));
			__builder.CloseComponent();
			__builder.AddMarkupContent(58, "\n        ");
			__builder.OpenElement(59, "button");
			__builder.AddAttribute(60, "class", "btn-back mt-3");
			__builder.AddAttribute<MouseEventArgs>(61, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, delegate
			{
				this._template = null;
			}));
			__builder.AddMarkupContent(62, "← Choose a different type");
			__builder.CloseElement();
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x000234EC File Offset: 0x000216EC
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x000234F4 File Offset: 0x000216F4
		[Parameter]
		public string Key { get; set; } = "";

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x000234FD File Offset: 0x000216FD
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x00023505 File Offset: 0x00021705
		[SupplyParameterFromQuery]
		public int? Parent { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0002350E File Offset: 0x0002170E
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00023516 File Offset: 0x00021716
		[Nullable(2)]
		[SupplyParameterFromQuery]
		public string Child
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00023520 File Offset: 0x00021720
		protected override async Task OnParametersSetAsync()
		{
			ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
			ApplicationDbContext db = applicationDbContext;
			object obj = null;
			int num = 0;
			try
			{
				IQueryable<DeviceCategory> deviceCategories = db.DeviceCategories;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(DeviceCategory), "c");
				this._category = await deviceCategories.FirstOrDefaultAsync<DeviceCategory>(Expression.Lambda<Func<DeviceCategory, bool>>(Expression.Equal(Expression.Property(parameterExpression, methodof(DeviceCategory.get_Key())), Expression.Property(Expression.Constant(this, typeof(AddDevice)), methodof(AddDevice.get_Key()))), new ParameterExpression[] { parameterExpression }), default(CancellationToken));
				if (this._category == null || this._category.IsHidden)
				{
					this._category = null;
					this._notFound = true;
				}
				else if (!this._category.AllowsManualAdd)
				{
					this._notAllowed = true;
				}
				else
				{
					IQueryable<DeviceTemplate> deviceTemplates = db.DeviceTemplates;
					parameterExpression = Expression.Parameter(typeof(DeviceTemplate), "t");
					IQueryable<DeviceTemplate> queryable = deviceTemplates.Where<DeviceTemplate>(Expression.Lambda<Func<DeviceTemplate, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(parameterExpression, methodof(DeviceTemplate.get_CategoryId())), Expression.Property(Expression.Field(Expression.Constant(this, typeof(AddDevice)), fieldof(AddDevice._category)), methodof(DeviceCategory.get_Id()))), Expression.Not(Expression.Property(parameterExpression, methodof(DeviceTemplate.get_IsGeneric())))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(DeviceTemplate), "t");
					this._templates = await queryable.OrderBy<DeviceTemplate, string>(Expression.Lambda<Func<DeviceTemplate, string>>(Expression.Property(parameterExpression, methodof(DeviceTemplate.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<DeviceTemplate>(default(CancellationToken));
					IQueryable<Device> devices = db.Devices;
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					IQueryable<Device> queryable2 = devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.OrElse(Expression.OrElse(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("au", typeof(string)))), Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("eu", typeof(string)))), new ParameterExpression[] { parameterExpression }));
					parameterExpression = Expression.Parameter(typeof(Device), "d");
					this._parents = await queryable2.OrderBy<Device, string>(Expression.Lambda<Func<Device, string>>(Expression.Property(parameterExpression, methodof(Device.get_Name())), new ParameterExpression[] { parameterExpression })).ToListAsync<Device>(default(CancellationToken));
					if (!string.IsNullOrEmpty(this.Child) && this._template == null)
					{
						DeviceTemplate deviceTemplate;
						if ((deviceTemplate = this._templates.FirstOrDefault<DeviceTemplate>((DeviceTemplate t) => t.CollectionMethod == CollectionMethod.ViaParent && !t.IsGeneric)) == null)
						{
							deviceTemplate = this._templates.FirstOrDefault<DeviceTemplate>((DeviceTemplate t) => t.CollectionMethod == CollectionMethod.ViaParent);
						}
						DeviceTemplate deviceTemplate2 = deviceTemplate;
						if (deviceTemplate2 != null)
						{
							this.SelectTemplate(deviceTemplate2);
							this._model.Name = this.Child;
							this._model.ParentDeviceId = this.Parent;
							this._model.SerialNumber = this.Child;
							this._model.ChildIdentifier = this.Child;
						}
					}
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

		// Token: 0x060006B3 RID: 1715 RVA: 0x00023564 File Offset: 0x00021764
		private void SelectTemplate(DeviceTemplate t)
		{
			this._template = t;
			this._error = null;
			this._model = new DeviceFormModel
			{
				Port = t.DefaultPort,
				PollIntervalSeconds = t.DefaultPollIntervalSeconds,
				SnmpVersion = "v2c",
				Enabled = true
			};
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000235B4 File Offset: 0x000217B4
		private async Task Save()
		{
			if (this._template != null && this._category != null)
			{
				this._error = this.Validate();
				if (this._error == null)
				{
					this._busy = true;
					try
					{
						ApplicationDbContext applicationDbContext = await this.DbFactory.CreateDbContextAsync(default(CancellationToken));
						ApplicationDbContext db = applicationDbContext;
						object obj = null;
						try
						{
							Device device = new Device
							{
								Name = this._model.Name.Trim(),
								CategoryId = this._category.Id,
								TemplateId = this._template.Id,
								CollectionMethod = this._template.CollectionMethod,
								IpAddress = (string.IsNullOrWhiteSpace(this._model.IpAddress) ? null : this._model.IpAddress.Trim()),
								Port = this._model.Port,
								SnmpVersion = this._model.SnmpVersion,
								SnmpCommunityEnc = this.Protector.Protect(this._model.SnmpCommunity),
								SshUsername = (string.IsNullOrWhiteSpace(this._model.SshUsername) ? null : this._model.SshUsername.Trim()),
								SshPasswordEnc = this.Protector.Protect(this._model.SshPassword),
								ParentDeviceId = ((this._template.CollectionMethod == CollectionMethod.ViaParent) ? this._model.ParentDeviceId : null),
								SerialNumber = (string.IsNullOrWhiteSpace(this._model.SerialNumber) ? null : this._model.SerialNumber.Trim()),
								ChildIdentifier = (string.IsNullOrWhiteSpace(this._model.ChildIdentifier) ? (string.IsNullOrWhiteSpace(this._model.SerialNumber) ? null : this._model.SerialNumber.Trim()) : this._model.ChildIdentifier.Trim()),
								CoordinateX = this._model.CoordinateX,
								CoordinateY = this._model.CoordinateY,
								CoordinateZ = this._model.CoordinateZ,
								PollIntervalSeconds = this._model.PollIntervalSeconds,
								Enabled = this._model.Enabled,
								Notes = this._model.Notes,
								CreatedAt = DateTime.UtcNow
							};
							db.Devices.Add(device);
							await db.SaveChangesAsync(default(CancellationToken));
							this.Nav.NavigateTo("devices/" + this.Key, false, false);
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
						db = null;
					}
					catch (Exception ex2)
					{
						this._error = ex2.Message;
						this._busy = false;
					}
				}
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000235F8 File Offset: 0x000217F8
		[NullableContext(2)]
		private string Validate()
		{
			if (string.IsNullOrWhiteSpace(this._model.Name))
			{
				return "Name is required.";
			}
			switch (this._template.CollectionMethod)
			{
			case CollectionMethod.Snmp:
				if (string.IsNullOrWhiteSpace(this._model.IpAddress))
				{
					return "IP address is required.";
				}
				if (string.IsNullOrWhiteSpace(this._model.SnmpCommunity))
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
				if (string.IsNullOrWhiteSpace(this._model.SshPassword))
				{
					return "SSH password is required.";
				}
				break;
			case CollectionMethod.ViaParent:
				if (this._model.ParentDeviceId == null)
				{
					return "Select the parent device.";
				}
				if (string.IsNullOrWhiteSpace(this._model.SerialNumber) && string.IsNullOrWhiteSpace(this._model.ChildIdentifier))
				{
					return "Serial number is required.";
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
				if (string.IsNullOrWhiteSpace(this._model.SshPassword))
				{
					return "Operator password is required.";
				}
				break;
			}
			return null;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00023750 File Offset: 0x00021950
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00023758 File Offset: 0x00021958
		[Inject]
		private NavigationManager Nav { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00023761 File Offset: 0x00021961
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00023769 File Offset: 0x00021969
		[Inject]
		private CredentialProtector Protector { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00023772 File Offset: 0x00021972
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x0002377A File Offset: 0x0002197A
		[Inject]
		private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; }

		// Token: 0x040002D0 RID: 720
		[Nullable(2)]
		private DeviceCategory _category;

		// Token: 0x040002D1 RID: 721
		[Nullable(new byte[] { 2, 1 })]
		private List<DeviceTemplate> _templates;

		// Token: 0x040002D2 RID: 722
		[Nullable(2)]
		private DeviceTemplate _template;

		// Token: 0x040002D3 RID: 723
		private List<Device> _parents = new List<Device>();

		// Token: 0x040002D4 RID: 724
		private DeviceFormModel _model = new DeviceFormModel();

		// Token: 0x040002D5 RID: 725
		private bool _notFound;

		// Token: 0x040002D6 RID: 726
		private bool _busy;

		// Token: 0x040002D7 RID: 727
		private bool _notAllowed;

		// Token: 0x040002D8 RID: 728
		[Nullable(2)]
		private string _error;

		// Token: 0x0200019E RID: 414
		[NullableContext(0)]
		private sealed class __PrivateComponentRenderModeAttribute : RenderModeAttribute
		{
			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00056106 File Offset: 0x00054306
			private static IComponentRenderMode ModeImpl
			{
				get
				{
					return new InteractiveServerRenderMode(false);
				}
			}

			// Token: 0x170002CB RID: 715
			// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0005610E File Offset: 0x0005430E
			public override IComponentRenderMode Mode
			{
				get
				{
					return AddDevice.__PrivateComponentRenderModeAttribute.ModeImpl;
				}
			}
		}
	}
}
