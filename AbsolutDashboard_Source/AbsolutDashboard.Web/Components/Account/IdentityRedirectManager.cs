using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace AbsolutDashboard.Web.Components.Account
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IdentityRedirectManager
	{
		// Token: 0x06000771 RID: 1905 RVA: 0x0002D0D6 File Offset: 0x0002B2D6
		public IdentityRedirectManager(NavigationManager navigationManager)
		{
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002D0E5 File Offset: 0x0002B2E5
		[NullableContext(2)]
		[DoesNotReturn]
		public void RedirectTo(string uri)
		{
			if (uri == null)
			{
				uri = "";
			}
			if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
			{
				uri = this.<navigationManager>P.ToBaseRelativePath(uri);
			}
			this.<navigationManager>P.NavigateTo(uri, false, false);
			throw new InvalidOperationException("IdentityRedirectManager can only be used during static rendering.");
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002D120 File Offset: 0x0002B320
		[DoesNotReturn]
		public void RedirectTo(string uri, [Nullable(new byte[] { 1, 1, 2 })] Dictionary<string, object> queryParameters)
		{
			string leftPart = this.<navigationManager>P.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
			string uriWithQueryParameters = this.<navigationManager>P.GetUriWithQueryParameters(leftPart, queryParameters);
			this.RedirectTo(uriWithQueryParameters);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002D155 File Offset: 0x0002B355
		[DoesNotReturn]
		public void RedirectToWithStatus(string uri, string message, HttpContext context)
		{
			context.Response.Cookies.Append("Identity.StatusMessage", message, IdentityRedirectManager.StatusCookieBuilder.Build(context));
			this.RedirectTo(uri);
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002D17F File Offset: 0x0002B37F
		private string CurrentPath
		{
			get
			{
				return this.<navigationManager>P.ToAbsoluteUri(this.<navigationManager>P.Uri).GetLeftPart(UriPartial.Path);
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002D19D File Offset: 0x0002B39D
		[DoesNotReturn]
		public void RedirectToCurrentPage()
		{
			this.RedirectTo(this.CurrentPath);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002D1AB File Offset: 0x0002B3AB
		[DoesNotReturn]
		public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
		{
			this.RedirectToWithStatus(this.CurrentPath, message, context);
		}

		// Token: 0x0400033F RID: 831
		[CompilerGenerated]
		private NavigationManager <navigationManager>P = navigationManager;

		// Token: 0x04000340 RID: 832
		public const string StatusCookieName = "Identity.StatusMessage";

		// Token: 0x04000341 RID: 833
		private static readonly CookieBuilder StatusCookieBuilder = new CookieBuilder
		{
			SameSite = SameSiteMode.Strict,
			HttpOnly = true,
			IsEssential = true,
			MaxAge = new TimeSpan?(TimeSpan.FromSeconds(5L))
		};
	}
}
