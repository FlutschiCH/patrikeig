using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using __Blazor.AbsolutDashboard.Web.Components.Pages.DeviceForm;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(1)]
	[Nullable(0)]
	[Authorize]
	public class DeviceForm : ComponentBase
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x000289A8 File Offset: 0x00026BA8
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<EditForm>(0);
			__builder.AddComponentParameter(1, "Model", RuntimeHelpers.TypeCheck<object>(this.Model));
			__builder.AddComponentParameter(2, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, this.OnSubmit)));
			__builder.AddComponentParameter(3, "class", "card max-w-2xl space-y-5 p-6");
			__builder.AddAttribute(4, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(5);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(6, "\n    ");
				__builder2.OpenComponent<ValidationSummary>(7);
				__builder2.AddComponentParameter(8, "class", "text-sm text-red-700");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(9, "\n\n    ");
				__builder2.OpenElement(10, "div");
				__builder2.AddAttribute(11, "class", "rounded-lg bg-cream-100 px-3 py-2 text-sm text-cocoa-700");
				__builder2.AddMarkupContent(12, "\n        Template: ");
				__builder2.OpenElement(13, "b");
				__builder2.AddAttribute(14, "class", "text-cocoa-900");
				__builder2.AddContent(15, this.Template.Name);
				__builder2.CloseElement();
				__builder2.AddMarkupContent(16, "\n        · ");
				__builder2.AddContent(17, DeviceStatusView.MethodLabel(this.Template.CollectionMethod));
				__builder2.CloseElement();
				__builder2.AddMarkupContent(18, "\n\n    ");
				__builder2.OpenElement(19, "label");
				__builder2.AddAttribute(20, "class", "block");
				__builder2.AddMarkupContent(21, "<span class=\"text-sm font-medium\">Name</span>\n        ");
				__builder2.OpenComponent<InputText>(22);
				__builder2.AddComponentParameter(23, "class", "input");
				__builder2.AddComponentParameter(24, "placeholder", "e.g. AU-Site-1");
				__builder2.AddComponentParameter(25, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.Name));
				__builder2.AddComponentParameter(26, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Model.Name = __value;
				}, this.Model.Name))));
				__builder2.AddComponentParameter(27, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_Name())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.CloseElement();
				if (this.Template.CollectionMethod == CollectionMethod.ViaParent)
				{
					if (!this.IsEdit)
					{
						__builder2.OpenElement(28, "label");
						__builder2.AddAttribute(29, "class", "block");
						__builder2.AddMarkupContent(30, "<span class=\"text-sm font-medium\">Parent device</span>\n                ");
						TypeInference.CreateInputSelect_0<int?>(__builder2, 31, 32, "input", 33, this.Model.ParentDeviceId, 34, EventCallback.Factory.Create<int?>(this, RuntimeHelpers.CreateInferredEventCallback<int?>(this, delegate(int? __value)
						{
							this.Model.ParentDeviceId = __value;
						}, this.Model.ParentDeviceId)), 35, Expression.Lambda<Func<int?>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_ParentDeviceId())), Array.Empty<ParameterExpression>()), 36, delegate(RenderTreeBuilder __builder3)
						{
							__builder3.AddMarkupContent(37, "<option value>— select the BBU/AU/EU this unit connects to —</option>");
							foreach (Device device in this.Parents)
							{
								__builder3.OpenElement(38, "option");
								__builder3.AddAttribute(39, "value", device.Id);
								__builder3.AddContent(40, device.Name);
								__builder3.CloseElement();
							}
						});
						__builder2.AddMarkupContent(41, "\n                ");
						__builder2.AddMarkupContent(42, "<span class=\"mt-1 block text-xs text-cocoa-700\">Usually auto-discovered from the BBU. Set this only to pre-create a node before its first poll.</span>");
						__builder2.CloseElement();
						__builder2.AddMarkupContent(43, "\n            ");
						__builder2.OpenElement(44, "label");
						__builder2.AddAttribute(45, "class", "block");
						__builder2.AddMarkupContent(46, "<span class=\"text-sm font-medium\">Serial number</span>\n                ");
						__builder2.OpenComponent<InputText>(47);
						__builder2.AddComponentParameter(48, "class", "input");
						__builder2.AddComponentParameter(49, "placeholder", "e.g. C0465926000015T");
						__builder2.AddComponentParameter(50, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SerialNumber));
						__builder2.AddComponentParameter(51, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SerialNumber = __value;
						}, this.Model.SerialNumber))));
						__builder2.AddComponentParameter(52, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SerialNumber())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.AddMarkupContent(53, "\n                ");
						__builder2.AddMarkupContent(54, "<span class=\"mt-1 block text-xs text-cocoa-700\">The node's hardware serial — used to link this entry to the matching node in the BBU topology.</span>");
						__builder2.CloseElement();
					}
				}
				else
				{
					__builder2.OpenElement(55, "div");
					__builder2.AddAttribute(56, "class", "grid gap-4 sm:grid-cols-3");
					__builder2.OpenElement(57, "label");
					__builder2.AddAttribute(58, "class", "block sm:col-span-2");
					__builder2.AddMarkupContent(59, "<span class=\"text-sm font-medium\">IP address</span>\n                ");
					__builder2.OpenComponent<InputText>(60);
					__builder2.AddComponentParameter(61, "class", "input");
					__builder2.AddComponentParameter(62, "placeholder", "e.g. 10.20.110.6");
					__builder2.AddComponentParameter(63, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.IpAddress));
					__builder2.AddComponentParameter(64, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
					{
						this.Model.IpAddress = __value;
					}, this.Model.IpAddress))));
					__builder2.AddComponentParameter(65, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_IpAddress())), Array.Empty<ParameterExpression>())));
					__builder2.CloseComponent();
					__builder2.CloseElement();
					__builder2.AddMarkupContent(66, "\n            ");
					__builder2.OpenElement(67, "label");
					__builder2.AddAttribute(68, "class", "block");
					__builder2.AddMarkupContent(69, "<span class=\"text-sm font-medium\">Port</span>\n                ");
					TypeInference.CreateInputNumber_1<int>(__builder2, 70, 71, "input", 72, this.Model.Port, 73, EventCallback.Factory.Create<int>(this, RuntimeHelpers.CreateInferredEventCallback<int>(this, delegate(int __value)
					{
						this.Model.Port = __value;
					}, this.Model.Port)), 74, Expression.Lambda<Func<int>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_Port())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.CloseElement();
					if (this.Template.CollectionMethod == CollectionMethod.Snmp)
					{
						__builder2.OpenElement(75, "div");
						__builder2.AddAttribute(76, "class", "grid gap-4 sm:grid-cols-3");
						__builder2.OpenElement(77, "label");
						__builder2.AddAttribute(78, "class", "block");
						__builder2.AddMarkupContent(79, "<span class=\"text-sm font-medium\">SNMP version</span>\n                    ");
						TypeInference.CreateInputSelect_2<string>(__builder2, 80, 81, "input", 82, this.Model.SnmpVersion, 83, EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SnmpVersion = __value;
						}, this.Model.SnmpVersion)), 84, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SnmpVersion())), Array.Empty<ParameterExpression>()), 85, delegate(RenderTreeBuilder __builder3)
						{
							__builder3.AddMarkupContent(86, "<option value=\"v2c\">v2c</option>\n                        ");
							__builder3.AddMarkupContent(87, "<option value=\"v1\">v1</option>");
						});
						__builder2.CloseElement();
						__builder2.AddMarkupContent(88, "\n                ");
						__builder2.OpenElement(89, "label");
						__builder2.AddAttribute(90, "class", "block sm:col-span-2");
						__builder2.AddMarkupContent(91, "<span class=\"text-sm font-medium\">Community</span>\n                    ");
						__builder2.OpenComponent<InputText>(92);
						__builder2.AddComponentParameter(93, "type", "password");
						__builder2.AddComponentParameter(94, "class", "input");
						__builder2.AddComponentParameter(95, "placeholder", this.IsEdit ? "leave blank to keep" : "e.g. public");
						__builder2.AddComponentParameter(96, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SnmpCommunity));
						__builder2.AddComponentParameter(97, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SnmpCommunity = __value;
						}, this.Model.SnmpCommunity))));
						__builder2.AddComponentParameter(98, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SnmpCommunity())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.CloseElement();
						__builder2.CloseElement();
					}
					else if (this.Template.CollectionMethod == CollectionMethod.SshScript)
					{
						__builder2.OpenElement(99, "div");
						__builder2.AddAttribute(100, "class", "grid gap-4 sm:grid-cols-2");
						__builder2.OpenElement(101, "label");
						__builder2.AddAttribute(102, "class", "block");
						__builder2.AddMarkupContent(103, "<span class=\"text-sm font-medium\">SSH username</span>\n                    ");
						__builder2.OpenComponent<InputText>(104);
						__builder2.AddComponentParameter(105, "class", "input");
						__builder2.AddComponentParameter(106, "autocomplete", "off");
						__builder2.AddComponentParameter(107, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SshUsername));
						__builder2.AddComponentParameter(108, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SshUsername = __value;
						}, this.Model.SshUsername))));
						__builder2.AddComponentParameter(109, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SshUsername())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.CloseElement();
						__builder2.AddMarkupContent(110, "\n                ");
						__builder2.OpenElement(111, "label");
						__builder2.AddAttribute(112, "class", "block");
						__builder2.AddMarkupContent(113, "<span class=\"text-sm font-medium\">SSH password</span>\n                    ");
						__builder2.OpenComponent<InputText>(114);
						__builder2.AddComponentParameter(115, "type", "password");
						__builder2.AddComponentParameter(116, "class", "input");
						__builder2.AddComponentParameter(117, "autocomplete", "new-password");
						__builder2.AddComponentParameter(118, "placeholder", this.IsEdit ? "leave blank to keep" : "");
						__builder2.AddComponentParameter(119, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SshPassword));
						__builder2.AddComponentParameter(120, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SshPassword = __value;
						}, this.Model.SshPassword))));
						__builder2.AddComponentParameter(121, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SshPassword())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.CloseElement();
						__builder2.CloseElement();
					}
					else if (this.Template.CollectionMethod == CollectionMethod.BbuWebSocket)
					{
						__builder2.AddMarkupContent(122, "<p class=\"text-xs text-cocoa-700\">Use a <b>dedicated operator account</b> (the BBU allows only one session per operator — it must not collide with a human's WebLMT login).</p>\n            ");
						__builder2.OpenElement(123, "div");
						__builder2.AddAttribute(124, "class", "grid gap-4 sm:grid-cols-2");
						__builder2.OpenElement(125, "label");
						__builder2.AddAttribute(126, "class", "block");
						__builder2.AddMarkupContent(127, "<span class=\"text-sm font-medium\">Operator account</span>\n                    ");
						__builder2.OpenComponent<InputText>(128);
						__builder2.AddComponentParameter(129, "class", "input");
						__builder2.AddComponentParameter(130, "autocomplete", "off");
						__builder2.AddComponentParameter(131, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SshUsername));
						__builder2.AddComponentParameter(132, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SshUsername = __value;
						}, this.Model.SshUsername))));
						__builder2.AddComponentParameter(133, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SshUsername())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.CloseElement();
						__builder2.AddMarkupContent(134, "\n                ");
						__builder2.OpenElement(135, "label");
						__builder2.AddAttribute(136, "class", "block");
						__builder2.AddMarkupContent(137, "<span class=\"text-sm font-medium\">Operator password</span>\n                    ");
						__builder2.OpenComponent<InputText>(138);
						__builder2.AddComponentParameter(139, "type", "password");
						__builder2.AddComponentParameter(140, "class", "input");
						__builder2.AddComponentParameter(141, "autocomplete", "new-password");
						__builder2.AddComponentParameter(142, "placeholder", this.IsEdit ? "leave blank to keep" : "");
						__builder2.AddComponentParameter(143, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SshPassword));
						__builder2.AddComponentParameter(144, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
						{
							this.Model.SshPassword = __value;
						}, this.Model.SshPassword))));
						__builder2.AddComponentParameter(145, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SshPassword())), Array.Empty<ParameterExpression>())));
						__builder2.CloseComponent();
						__builder2.CloseElement();
						__builder2.CloseElement();
					}
				}
				if (!this.AutoManaged)
				{
					__builder2.OpenElement(146, "div");
					__builder2.AddAttribute(147, "class", "grid gap-4 sm:grid-cols-2");
					__builder2.OpenElement(148, "label");
					__builder2.AddAttribute(149, "class", "block");
					__builder2.AddMarkupContent(150, "<span class=\"text-sm font-medium\">Poll interval (seconds)</span>\n                ");
					TypeInference.CreateInputNumber_3<int>(__builder2, 151, 152, "input", 153, this.Model.PollIntervalSeconds, 154, EventCallback.Factory.Create<int>(this, RuntimeHelpers.CreateInferredEventCallback<int>(this, delegate(int __value)
					{
						this.Model.PollIntervalSeconds = __value;
					}, this.Model.PollIntervalSeconds)), 155, Expression.Lambda<Func<int>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_PollIntervalSeconds())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.AddMarkupContent(156, "\n            ");
					__builder2.OpenElement(157, "label");
					__builder2.AddAttribute(158, "class", "mt-7 flex items-center gap-2 text-sm");
					__builder2.OpenComponent<InputCheckbox>(159);
					__builder2.AddComponentParameter(160, "Value", RuntimeHelpers.TypeCheck<bool>(this.Model.Enabled));
					__builder2.AddComponentParameter(161, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<bool>>(EventCallback.Factory.Create<bool>(this, RuntimeHelpers.CreateInferredEventCallback<bool>(this, delegate(bool __value)
					{
						this.Model.Enabled = __value;
					}, this.Model.Enabled))));
					__builder2.AddComponentParameter(162, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<bool>>>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_Enabled())), Array.Empty<ParameterExpression>())));
					__builder2.CloseComponent();
					__builder2.AddMarkupContent(163, " Enabled (actively polled)\n            ");
					__builder2.CloseElement();
					__builder2.CloseElement();
				}
				if (this.ShowSerialPort)
				{
					__builder2.OpenElement(164, "fieldset");
					__builder2.AddAttribute(165, "class", "rounded-lg border border-cream-200 p-4");
					__builder2.AddMarkupContent(166, "<legend class=\"px-1 text-sm font-medium\">Serial console (power ports)</legend>\n            ");
					__builder2.OpenElement(167, "div");
					__builder2.AddAttribute(168, "class", "grid gap-4 sm:grid-cols-3");
					__builder2.OpenElement(169, "label");
					__builder2.AddAttribute(170, "class", "block");
					__builder2.AddMarkupContent(171, "<span class=\"text-xs text-cocoa-700\">Serial port</span>\n                    ");
					__builder2.OpenComponent<InputText>(172);
					__builder2.AddComponentParameter(173, "class", "input");
					__builder2.AddComponentParameter(174, "placeholder", "/dev/ttyUSB0");
					__builder2.AddComponentParameter(175, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SerialPort));
					__builder2.AddComponentParameter(176, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
					{
						this.Model.SerialPort = __value;
					}, this.Model.SerialPort))));
					__builder2.AddComponentParameter(177, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SerialPort())), Array.Empty<ParameterExpression>())));
					__builder2.CloseComponent();
					__builder2.CloseElement();
					__builder2.AddMarkupContent(178, "\n                ");
					__builder2.OpenElement(179, "label");
					__builder2.AddAttribute(180, "class", "block");
					__builder2.AddMarkupContent(181, "<span class=\"text-xs text-cocoa-700\">Baud rate</span>\n                    ");
					TypeInference.CreateInputNumber_4<int>(__builder2, 182, 183, "input", 184, this.Model.SerialBaudRate, 185, EventCallback.Factory.Create<int>(this, RuntimeHelpers.CreateInferredEventCallback<int>(this, delegate(int __value)
					{
						this.Model.SerialBaudRate = __value;
					}, this.Model.SerialBaudRate)), 186, Expression.Lambda<Func<int>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SerialBaudRate())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.AddMarkupContent(187, "\n                ");
					__builder2.OpenElement(188, "label");
					__builder2.AddAttribute(189, "class", "block");
					__builder2.AddMarkupContent(190, "<span class=\"text-xs text-cocoa-700\">Framing</span>\n                    ");
					TypeInference.CreateInputSelect_5<string>(__builder2, 191, 192, "input", 193, this.Model.SerialFraming, 194, EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
					{
						this.Model.SerialFraming = __value;
					}, this.Model.SerialFraming)), 195, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SerialFraming())), Array.Empty<ParameterExpression>()), 196, delegate(RenderTreeBuilder __builder3)
					{
						__builder3.AddMarkupContent(197, "<option value=\"8-N-1\">8-N-1</option>\n                        ");
						__builder3.AddMarkupContent(198, "<option value=\"8-E-1\">8-E-1</option>\n                        ");
						__builder3.AddMarkupContent(199, "<option value=\"8-O-1\">8-O-1</option>\n                        ");
						__builder3.AddMarkupContent(200, "<option value=\"8-N-2\">8-N-2</option>\n                        ");
						__builder3.AddMarkupContent(201, "<option value=\"7-E-1\">7-E-1</option>");
					});
					__builder2.CloseElement();
					__builder2.CloseElement();
					__builder2.AddMarkupContent(202, "\n            ");
					__builder2.OpenElement(203, "label");
					__builder2.AddAttribute(204, "class", "mt-4 block");
					__builder2.AddMarkupContent(205, "<span class=\"text-xs text-cocoa-700\">Password (optional)</span>\n                ");
					__builder2.OpenComponent<InputText>(206);
					__builder2.AddComponentParameter(207, "type", "password");
					__builder2.AddComponentParameter(208, "class", "input");
					__builder2.AddComponentParameter(209, "autocomplete", "new-password");
					__builder2.AddComponentParameter(210, "placeholder", this.IsEdit ? "leave blank to keep" : "");
					__builder2.AddComponentParameter(211, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.SshPassword));
					__builder2.AddComponentParameter(212, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
					{
						this.Model.SshPassword = __value;
					}, this.Model.SshPassword))));
					__builder2.AddComponentParameter(213, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_SshPassword())), Array.Empty<ParameterExpression>())));
					__builder2.CloseComponent();
					__builder2.CloseElement();
					__builder2.AddMarkupContent(214, "\n            ");
					__builder2.AddMarkupContent(215, "<span class=\"mt-2 block text-xs text-cocoa-700\">\n                Leave the serial port blank if this unit has no console wired up — its power ports then read\n                &ldquo;unknown&rdquo; rather than showing a status we have not measured. The password is only sent\n                if the unit actually asks for one; nothing waits for a prompt. On Linux the service account needs\n                the <b>dialout</b> group to open <code>/dev/ttyUSB*</code>.\n            </span>");
					__builder2.CloseElement();
				}
				if (this.ShowCoordinates)
				{
					__builder2.OpenElement(216, "fieldset");
					__builder2.AddAttribute(217, "class", "rounded-lg border border-cream-200 p-4");
					__builder2.AddMarkupContent(218, "<legend class=\"px-1 text-sm font-medium\">Position (X / Y / Z)</legend>\n            ");
					__builder2.OpenElement(219, "div");
					__builder2.AddAttribute(220, "class", "grid gap-4 sm:grid-cols-3");
					__builder2.OpenElement(221, "label");
					__builder2.AddAttribute(222, "class", "block");
					__builder2.AddMarkupContent(223, "<span class=\"text-xs text-cocoa-700\">X</span>\n                    ");
					TypeInference.CreateInputNumber_6<double?>(__builder2, 224, 225, "input", 226, this.Model.CoordinateX, 227, EventCallback.Factory.Create<double?>(this, RuntimeHelpers.CreateInferredEventCallback<double?>(this, delegate(double? __value)
					{
						this.Model.CoordinateX = __value;
					}, this.Model.CoordinateX)), 228, Expression.Lambda<Func<double?>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_CoordinateX())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.AddMarkupContent(229, "\n                ");
					__builder2.OpenElement(230, "label");
					__builder2.AddAttribute(231, "class", "block");
					__builder2.AddMarkupContent(232, "<span class=\"text-xs text-cocoa-700\">Y</span>\n                    ");
					TypeInference.CreateInputNumber_7<double?>(__builder2, 233, 234, "input", 235, this.Model.CoordinateY, 236, EventCallback.Factory.Create<double?>(this, RuntimeHelpers.CreateInferredEventCallback<double?>(this, delegate(double? __value)
					{
						this.Model.CoordinateY = __value;
					}, this.Model.CoordinateY)), 237, Expression.Lambda<Func<double?>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_CoordinateY())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.AddMarkupContent(238, "\n                ");
					__builder2.OpenElement(239, "label");
					__builder2.AddAttribute(240, "class", "block");
					__builder2.AddMarkupContent(241, "<span class=\"text-xs text-cocoa-700\">Z</span>\n                    ");
					TypeInference.CreateInputNumber_8<double?>(__builder2, 242, 243, "input", 244, this.Model.CoordinateZ, 245, EventCallback.Factory.Create<double?>(this, RuntimeHelpers.CreateInferredEventCallback<double?>(this, delegate(double? __value)
					{
						this.Model.CoordinateZ = __value;
					}, this.Model.CoordinateZ)), 246, Expression.Lambda<Func<double?>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_CoordinateZ())), Array.Empty<ParameterExpression>()));
					__builder2.CloseElement();
					__builder2.CloseElement();
					__builder2.AddMarkupContent(247, "\n            ");
					__builder2.AddMarkupContent(248, "<span class=\"mt-2 block text-xs text-cocoa-700\">Physical coordinates set here only. Unrelated to BBU data; used later for UE triangulation.</span>");
					__builder2.CloseElement();
				}
				__builder2.OpenElement(249, "label");
				__builder2.AddAttribute(250, "class", "block");
				__builder2.AddMarkupContent(251, "<span class=\"text-sm font-medium\">Notes</span>\n        ");
				__builder2.OpenComponent<InputTextArea>(252);
				__builder2.AddComponentParameter(253, "class", "input");
				__builder2.AddComponentParameter(254, "rows", "2");
				__builder2.AddComponentParameter(255, "Value", RuntimeHelpers.TypeCheck<string>(this.Model.Notes));
				__builder2.AddComponentParameter(256, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Model.Notes = __value;
				}, this.Model.Notes))));
				__builder2.AddComponentParameter(257, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(DeviceForm)), methodof(DeviceForm.get_Model())), methodof(DeviceFormModel.get_Notes())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.CloseElement();
				if (this.Error != null)
				{
					__builder2.OpenElement(258, "div");
					__builder2.AddAttribute(259, "class", "rounded-lg border-l-4 border-red-400 bg-red-50 p-3 text-sm text-red-700");
					__builder2.AddContent(260, this.Error);
					__builder2.CloseElement();
				}
				__builder2.OpenElement(261, "div");
				__builder2.AddAttribute(262, "class", "flex gap-3");
				__builder2.OpenElement(263, "button");
				__builder2.AddAttribute(264, "type", "submit");
				__builder2.AddAttribute(265, "class", "btn-primary !px-5 !py-2.5 text-sm");
				__builder2.AddAttribute(266, "disabled", this.Busy);
				__builder2.AddContent(267, this.SubmitLabel);
				__builder2.CloseElement();
				__builder2.AddMarkupContent(268, "\n        ");
				__builder2.OpenElement(269, "a");
				__builder2.AddAttribute(270, "class", "btn-secondary !px-5 !py-2.5 text-sm");
				__builder2.AddAttribute(271, "href", this.CancelHref);
				__builder2.AddContent(272, "Cancel");
				__builder2.CloseElement();
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00028A29 File Offset: 0x00026C29
		private bool AutoManaged
		{
			get
			{
				return this.IsEdit && this.Template.CollectionMethod == CollectionMethod.ViaParent;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00028A43 File Offset: 0x00026C43
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00028A4B File Offset: 0x00026C4B
		[Parameter]
		[EditorRequired]
		public DeviceFormModel Model { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00028A54 File Offset: 0x00026C54
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x00028A5C File Offset: 0x00026C5C
		[Parameter]
		[EditorRequired]
		public DeviceTemplate Template { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00028A65 File Offset: 0x00026C65
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x00028A6D File Offset: 0x00026C6D
		[Parameter]
		public List<Device> Parents { get; set; } = new List<Device>();

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00028A76 File Offset: 0x00026C76
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00028A7E File Offset: 0x00026C7E
		[Parameter]
		public bool IsEdit { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00028A87 File Offset: 0x00026C87
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x00028A8F File Offset: 0x00026C8F
		[Parameter]
		public bool ShowCoordinates { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00028A98 File Offset: 0x00026C98
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x00028AA0 File Offset: 0x00026CA0
		[Parameter]
		public bool ShowSerialPort { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00028AA9 File Offset: 0x00026CA9
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00028AB1 File Offset: 0x00026CB1
		[Parameter]
		public bool Busy { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00028ABA File Offset: 0x00026CBA
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00028AC2 File Offset: 0x00026CC2
		[Nullable(2)]
		[Parameter]
		public string Error
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00028ACB File Offset: 0x00026CCB
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00028AD3 File Offset: 0x00026CD3
		[Parameter]
		public string SubmitLabel { get; set; } = "Save";

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00028ADC File Offset: 0x00026CDC
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00028AE4 File Offset: 0x00026CE4
		[Parameter]
		public string CancelHref { get; set; } = "";

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00028AED File Offset: 0x00026CED
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00028AF5 File Offset: 0x00026CF5
		[Parameter]
		public EventCallback OnSubmit { get; set; }
	}
}
