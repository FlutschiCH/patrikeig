using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Components.Account.Shared;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QRCoder;
using __Blazor.AbsolutDashboard.Web.Components.Account.Pages.Manage.EnableAuthenticator;

namespace AbsolutDashboard.Web.Components.Account.Pages.Manage
{
	// Token: 0x020000C3 RID: 195
	[NullableContext(1)]
	[Nullable(0)]
	[ExcludeFromInteractiveRouting]
	[Authorize]
	[Layout(typeof(ManageLayout))]
	[Route("/Account/Manage/EnableAuthenticator")]
	public class EnableAuthenticator : ComponentBase
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x00030ACC File Offset: 0x0002ECCC
		[NullableContext(0)]
		protected override void BuildRenderTree(RenderTreeBuilder __builder)
		{
			__builder.OpenComponent<PageTitle>(0);
			__builder.AddAttribute(1, "ChildContent", new RenderFragment(delegate(RenderTreeBuilder __builder2)
			{
				__builder2.AddContent(2, Loc.IsGerman ? "Authenticator-App einrichten" : "Configure authenticator app");
			}));
			__builder.CloseComponent();
			if (this.recoveryCodes != null)
			{
				__builder.OpenComponent<ShowRecoveryCodes>(3);
				__builder.AddComponentParameter(4, "RecoveryCodes", RuntimeHelpers.TypeCheck<string[]>(this.recoveryCodes.ToArray<string>()));
				__builder.AddComponentParameter(5, "StatusMessage", RuntimeHelpers.TypeCheck<string>(this.message));
				__builder.CloseComponent();
				return;
			}
			__builder.OpenElement(6, "div");
			__builder.AddAttribute(7, "class", "space-y-6");
			__builder.OpenElement(8, "div");
			__builder.OpenElement(9, "h2");
			__builder.AddAttribute(10, "class", "font-display text-2xl text-cocoa-900");
			__builder.AddContent(11, Loc.IsGerman ? "Authenticator-App einrichten" : "Configure authenticator app");
			__builder.CloseElement();
			__builder.AddMarkupContent(12, "\n            ");
			__builder.OpenElement(13, "p");
			__builder.AddAttribute(14, "class", "mt-1 text-sm text-cocoa-700");
			__builder.AddContent(15, Loc.IsGerman ? "Folge den Schritten, um eine Authenticator-App zu verbinden." : "Follow the steps below to connect an authenticator app.");
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(16, "\n\n        ");
			__builder.OpenComponent<StatusMessage>(17);
			__builder.AddComponentParameter(18, "Message", RuntimeHelpers.TypeCheck<string>(this.message));
			__builder.CloseComponent();
			__builder.AddMarkupContent(19, "\n\n        ");
			__builder.OpenElement(20, "ol");
			__builder.AddAttribute(21, "class", "space-y-8");
			__builder.OpenElement(22, "li");
			__builder.AddAttribute(23, "class", "flex gap-4");
			__builder.AddMarkupContent(24, "<span class=\"step-badge\">1</span>\n                ");
			__builder.OpenElement(25, "div");
			__builder.AddAttribute(26, "class", "text-sm leading-relaxed text-cocoa-700");
			if (Loc.IsGerman)
			{
				__builder.AddMarkupContent(27, "<p>Lade eine Authenticator-App herunter, z. B. Microsoft Authenticator für\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://go.microsoft.com/fwlink/?Linkid=825072\">Android</a> und\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://go.microsoft.com/fwlink/?Linkid=825073\">iOS</a>, oder Google Authenticator für\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en\">Android</a> und\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8\">iOS</a>.</p>");
			}
			else
			{
				__builder.AddMarkupContent(28, "<p>Download a two-factor authenticator app like Microsoft Authenticator for\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://go.microsoft.com/fwlink/?Linkid=825072\">Android</a> and\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://go.microsoft.com/fwlink/?Linkid=825073\">iOS</a> or Google Authenticator for\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en\">Android</a> and\n                            <a class=\"font-medium text-blush-700 hover:text-blush-600\" href=\"https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8\">iOS</a>.</p>");
			}
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(29, "\n\n            ");
			__builder.OpenElement(30, "li");
			__builder.AddAttribute(31, "class", "flex gap-4");
			__builder.AddMarkupContent(32, "<span class=\"step-badge\">2</span>\n                ");
			__builder.OpenElement(33, "div");
			__builder.AddAttribute(34, "class", "space-y-4 text-sm text-cocoa-700");
			__builder.OpenElement(35, "p");
			__builder.AddContent(36, Loc.IsGerman ? "Scanne den QR-Code oder gib diesen Schlüssel in deiner App ein. Leerzeichen und Groß-/Kleinschreibung spielen keine Rolle." : "Scan the QR code or enter this key into your authenticator app. Spaces and casing do not matter.");
			__builder.CloseElement();
			__builder.AddMarkupContent(37, "\n\n                    ");
			__builder.OpenElement(38, "div");
			__builder.AddAttribute(39, "class", "inline-block rounded-xl bg-cream-100 px-4 py-3 font-mono text-base tracking-widest text-cocoa-900 ring-1 ring-blush-100");
			__builder.AddContent(40, this.sharedKey);
			__builder.CloseElement();
			if (this.qrCodeDataUri != null)
			{
				__builder.OpenElement(41, "div");
				__builder.OpenElement(42, "img");
				__builder.AddAttribute(43, "src", this.qrCodeDataUri);
				__builder.AddAttribute(44, "alt", Loc.IsGerman ? "QR-Code für die Authenticator-App" : "Authenticator app QR code");
				__builder.AddAttribute(45, "width", "200");
				__builder.AddAttribute(46, "height", "200");
				__builder.AddAttribute(47, "class", "rounded-xl bg-white p-3 shadow-sm ring-1 ring-blush-100");
				__builder.CloseElement();
				__builder.CloseElement();
			}
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.AddMarkupContent(48, "\n\n            ");
			__builder.OpenElement(49, "li");
			__builder.AddAttribute(50, "class", "flex gap-4");
			__builder.AddMarkupContent(51, "<span class=\"step-badge\">3</span>\n                ");
			__builder.OpenElement(52, "div");
			__builder.AddAttribute(53, "class", "w-full max-w-sm space-y-3 text-sm text-cocoa-700");
			__builder.OpenElement(54, "p");
			__builder.AddContent(55, Loc.IsGerman ? "Gib den Code aus deiner App ein, um die Einrichtung zu bestätigen." : "Enter the code your app generates to verify the setup.");
			__builder.CloseElement();
			__builder.AddMarkupContent(56, "\n\n                    ");
			__builder.OpenComponent<EditForm>(57);
			__builder.AddComponentParameter(58, "Model", RuntimeHelpers.TypeCheck<object>(this.Input));
			__builder.AddComponentParameter(59, "FormName", "send-code");
			__builder.AddComponentParameter(60, "OnValidSubmit", RuntimeHelpers.TypeCheck<EventCallback<EditContext>>(EventCallback.Factory.Create<EditContext>(this, new Func<Task>(this.OnValidSubmitAsync))));
			__builder.AddComponentParameter(61, "method", "post");
			__builder.AddComponentParameter(62, "class", "space-y-3");
			__builder.AddAttribute(63, "ChildContent", new RenderFragment<EditContext>((EditContext context) => delegate(RenderTreeBuilder __builder2)
			{
				__builder2.OpenComponent<DataAnnotationsValidator>(64);
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(65, "\n                        ");
				__builder2.OpenComponent<ValidationSummary>(66);
				__builder2.AddComponentParameter(67, "class", "rounded-xl bg-red-50 px-4 py-3 text-sm text-red-700 ring-1 ring-red-200");
				__builder2.AddComponentParameter(68, "role", "alert");
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(69, "\n                        ");
				__builder2.OpenElement(70, "div");
				__builder2.OpenElement(71, "label");
				__builder2.AddAttribute(72, "for", "Input.Code");
				__builder2.AddAttribute(73, "class", "block text-sm font-medium text-cocoa-900");
				__builder2.AddContent(74, Loc.IsGerman ? "Bestätigungscode" : "Verification code");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(75, "\n                            ");
				__builder2.OpenComponent<InputText>(76);
				__builder2.AddComponentParameter(77, "id", "Input.Code");
				__builder2.AddComponentParameter(78, "class", "input tracking-widest");
				__builder2.AddComponentParameter(79, "autocomplete", "off");
				__builder2.AddComponentParameter(80, "placeholder", Loc.IsGerman ? "Code eingeben" : "Enter the code");
				__builder2.AddComponentParameter(81, "Value", RuntimeHelpers.TypeCheck<string>(this.Input.Code));
				__builder2.AddComponentParameter(82, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create<string>(this, RuntimeHelpers.CreateInferredEventCallback<string>(this, delegate(string __value)
				{
					this.Input.Code = __value;
				}, this.Input.Code))));
				__builder2.AddComponentParameter(83, "ValueExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(EnableAuthenticator)), methodof(EnableAuthenticator.get_Input())), methodof(EnableAuthenticator.InputModel.get_Code())), Array.Empty<ParameterExpression>())));
				__builder2.CloseComponent();
				__builder2.AddMarkupContent(84, "\n                            ");
				TypeInference.CreateValidationMessage_0<string>(__builder2, 85, 86, Expression.Lambda<Func<string>>(Expression.Property(Expression.Property(Expression.Constant(this, typeof(EnableAuthenticator)), methodof(EnableAuthenticator.get_Input())), methodof(EnableAuthenticator.InputModel.get_Code())), Array.Empty<ParameterExpression>()), 87, "mt-1 block text-sm text-red-600");
				__builder2.CloseElement();
				__builder2.AddMarkupContent(88, "\n                        ");
				__builder2.OpenElement(89, "button");
				__builder2.AddAttribute(90, "type", "submit");
				__builder2.AddAttribute(91, "class", "btn-primary");
				__builder2.AddContent(92, Loc.IsGerman ? "Bestätigen" : "Verify");
				__builder2.CloseElement();
			}));
			__builder.CloseComponent();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
			__builder.CloseElement();
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00030FA9 File Offset: 0x0002F1A9
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x00030FB1 File Offset: 0x0002F1B1
		[CascadingParameter]
		private HttpContext HttpContext { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00030FBA File Offset: 0x0002F1BA
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x00030FC2 File Offset: 0x0002F1C2
		[SupplyParameterFromForm]
		private EnableAuthenticator.InputModel Input { get; set; } = new EnableAuthenticator.InputModel();

		// Token: 0x06000837 RID: 2103 RVA: 0x00030FCC File Offset: 0x0002F1CC
		protected override async Task OnInitializedAsync()
		{
			ApplicationUser applicationUser = await this.UserAccessor.GetRequiredUserAsync(this.HttpContext);
			this.user = applicationUser;
			await this.LoadSharedKeyAndQrCodeUriAsync(this.user);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00031010 File Offset: 0x0002F210
		private async Task OnValidSubmitAsync()
		{
			string text = this.Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
			TaskAwaiter<bool> taskAwaiter = this.UserManager.VerifyTwoFactorTokenAsync(this.user, this.UserManager.Options.Tokens.AuthenticatorTokenProvider, text).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this.message = "Error: Verification code is invalid.";
			}
			else
			{
				await this.UserManager.SetTwoFactorEnabledAsync(this.user, true);
				string text2 = await this.UserManager.GetUserIdAsync(this.user);
				this.Logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", new object[] { text2 });
				this.message = "Your authenticator app has been verified.";
				TaskAwaiter<int> taskAwaiter3 = this.UserManager.CountRecoveryCodesAsync(this.user).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					TaskAwaiter<int> taskAwaiter4;
					taskAwaiter3 = taskAwaiter4;
					taskAwaiter4 = default(TaskAwaiter<int>);
				}
				if (taskAwaiter3.GetResult() == 0)
				{
					this.recoveryCodes = await this.UserManager.GenerateNewTwoFactorRecoveryCodesAsync(this.user, 10);
				}
				else
				{
					this.RedirectManager.RedirectToWithStatus("Account/Manage/TwoFactorAuthentication", this.message, this.HttpContext);
				}
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00031054 File Offset: 0x0002F254
		private async ValueTask LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
		{
			string text = await this.UserManager.GetAuthenticatorKeyAsync(user);
			string unformattedKey = text;
			if (string.IsNullOrEmpty(unformattedKey))
			{
				await this.UserManager.ResetAuthenticatorKeyAsync(user);
				text = await this.UserManager.GetAuthenticatorKeyAsync(user);
				unformattedKey = text;
			}
			this.sharedKey = this.FormatKey(unformattedKey);
			string text2 = await this.UserManager.GetEmailAsync(user);
			this.authenticatorUri = this.GenerateQrCodeUri(text2, unformattedKey);
			this.qrCodeDataUri = EnableAuthenticator.GenerateQrCodeDataUri(this.authenticatorUri);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000310A0 File Offset: 0x0002F2A0
		private string FormatKey(string unformattedKey)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (num + 4 < unformattedKey.Length)
			{
				stringBuilder.Append(unformattedKey.AsSpan(num, 4)).Append(' ');
				num += 4;
			}
			if (num < unformattedKey.Length)
			{
				stringBuilder.Append(unformattedKey.AsSpan(num));
			}
			return stringBuilder.ToString().ToLowerInvariant();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000310FE File Offset: 0x0002F2FE
		private string GenerateQrCodeUri(string email, string unformattedKey)
		{
			return string.Format(CultureInfo.InvariantCulture, "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6", this.UrlEncoder.Encode("Absolut Dashboard"), this.UrlEncoder.Encode(email), unformattedKey);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0003112C File Offset: 0x0002F32C
		private static string GenerateQrCodeDataUri(string uri)
		{
			string text;
			using (QRCodeGenerator qrcodeGenerator = new QRCodeGenerator())
			{
				using (QRCodeData qrcodeData = qrcodeGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q, false, false, QRCodeGenerator.EciMode.Default, -1))
				{
					byte[] graphic = new PngByteQRCode(qrcodeData).GetGraphic(10, true);
					text = "data:image/png;base64," + Convert.ToBase64String(graphic);
				}
			}
			return text;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x000311A0 File Offset: 0x0002F3A0
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x000311A8 File Offset: 0x0002F3A8
		[Inject]
		private ILogger<EnableAuthenticator> Logger { get; set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x000311B1 File Offset: 0x0002F3B1
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x000311B9 File Offset: 0x0002F3B9
		[Inject]
		private IdentityRedirectManager RedirectManager { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000311C2 File Offset: 0x0002F3C2
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x000311CA File Offset: 0x0002F3CA
		[Inject]
		private UrlEncoder UrlEncoder { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x000311D3 File Offset: 0x0002F3D3
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x000311DB File Offset: 0x0002F3DB
		[Inject]
		private IdentityUserAccessor UserAccessor { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x000311E4 File Offset: 0x0002F3E4
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x000311EC File Offset: 0x0002F3EC
		[Inject]
		private UserManager<ApplicationUser> UserManager { get; set; }

		// Token: 0x0400038A RID: 906
		private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

		// Token: 0x0400038B RID: 907
		[Nullable(2)]
		private string message;

		// Token: 0x0400038C RID: 908
		private ApplicationUser user;

		// Token: 0x0400038D RID: 909
		[Nullable(2)]
		private string sharedKey;

		// Token: 0x0400038E RID: 910
		[Nullable(2)]
		private string authenticatorUri;

		// Token: 0x0400038F RID: 911
		[Nullable(2)]
		private string qrCodeDataUri;

		// Token: 0x04000390 RID: 912
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<string> recoveryCodes;

		// Token: 0x020001FA RID: 506
		[Nullable(0)]
		private sealed class InputModel
		{
			// Token: 0x170002F2 RID: 754
			// (get) Token: 0x06000E73 RID: 3699 RVA: 0x000607B6 File Offset: 0x0005E9B6
			// (set) Token: 0x06000E74 RID: 3700 RVA: 0x000607BE File Offset: 0x0005E9BE
			[Required]
			[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Text)]
			[Display(Name = "Verification Code")]
			public string Code { get; set; } = "";
		}
	}
}
