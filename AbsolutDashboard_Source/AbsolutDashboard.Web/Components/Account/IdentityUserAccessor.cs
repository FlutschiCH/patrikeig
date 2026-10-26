using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AbsolutDashboard.Web.Components.Account
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IdentityUserAccessor
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x0002D2AF File Offset: 0x0002B4AF
		public IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
		public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
		{
			ApplicationUser applicationUser = await this.<userManager>P.GetUserAsync(context.User);
			if (applicationUser == null)
			{
				this.<redirectManager>P.RedirectToWithStatus("Account/InvalidUser", "Error: Unable to load user with ID '" + this.<userManager>P.GetUserId(context.User) + "'.", context);
			}
			return applicationUser;
		}

		// Token: 0x04000344 RID: 836
		[CompilerGenerated]
		private UserManager<ApplicationUser> <userManager>P = userManager;

		// Token: 0x04000345 RID: 837
		[CompilerGenerated]
		private IdentityRedirectManager <redirectManager>P = redirectManager;
	}
}
