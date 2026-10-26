using System;
using Microsoft.AspNetCore.Identity;

namespace AbsolutDashboard.Web.Data
{
	// Token: 0x02000080 RID: 128
	public class ApplicationUser : IdentityUser
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00015D5A File Offset: 0x00013F5A
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x00015D62 File Offset: 0x00013F62
		public bool IsOwner { get; set; }
	}
}
