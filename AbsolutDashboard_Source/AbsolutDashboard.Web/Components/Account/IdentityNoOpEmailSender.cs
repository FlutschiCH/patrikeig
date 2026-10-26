using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AbsolutDashboard.Web.Components.Account
{
	// Token: 0x020000AF RID: 175
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0002D05F File Offset: 0x0002B25F
		public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
		{
			return this.emailSender.SendEmailAsync(email, "Confirm your email", "Please confirm your account by <a href='" + confirmationLink + "'>clicking here</a>.");
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002D082 File Offset: 0x0002B282
		public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
		{
			return this.emailSender.SendEmailAsync(email, "Reset your password", "Please reset your password by <a href='" + resetLink + "'>clicking here</a>.");
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002D0A5 File Offset: 0x0002B2A5
		public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
		{
			return this.emailSender.SendEmailAsync(email, "Reset your password", "Please reset your password using the following code: " + resetCode);
		}

		// Token: 0x0400033E RID: 830
		private readonly IEmailSender emailSender = new NoOpEmailSender();
	}
}
