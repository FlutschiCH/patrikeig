using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.DataProtection;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000036 RID: 54
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CredentialProtector
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00007750 File Offset: 0x00005950
		public CredentialProtector(IDataProtectionProvider provider)
		{
			this._protector = provider.CreateProtector("AbsolutDashboard.DeviceCredentials.v1");
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007769 File Offset: 0x00005969
		[NullableContext(2)]
		public string Protect(string plaintext)
		{
			if (!string.IsNullOrEmpty(plaintext))
			{
				return this._protector.Protect(plaintext);
			}
			return plaintext;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007784 File Offset: 0x00005984
		[NullableContext(2)]
		public string Unprotect(string ciphertext)
		{
			if (string.IsNullOrEmpty(ciphertext))
			{
				return null;
			}
			string text;
			try
			{
				text = this._protector.Unprotect(ciphertext);
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x040000A2 RID: 162
		private readonly IDataProtector _protector;
	}
}
