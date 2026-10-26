using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace AbsolutDashboard.Web.Services.Integrity
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IntegrityVerifier
	{
		// Token: 0x0600012D RID: 301 RVA: 0x000068B4 File Offset: 0x00004AB4
		public IntegrityVerifier()
		{
			this._ecdsa = ECDsa.Create();
			int num;
			this._ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String("MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE6SlGCWFXB4qmsQrpRYNFZl1FSqlEQTDyj+QMzsL5K+4fMbwU4jWfquNWAgkFccMQ2Acm0rCQeddT0RgcH4I80g=="), out num);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000068F0 File Offset: 0x00004AF0
		[NullableContext(2)]
		public bool TryRead(string token, out DateTime expiresUtc)
		{
			expiresUtc = default(DateTime);
			if (string.IsNullOrWhiteSpace(token))
			{
				return false;
			}
			string[] array = token.Trim().Split('.', StringSplitOptions.None);
			if (array.Length != 2)
			{
				return false;
			}
			byte[] array2;
			byte[] array3;
			try
			{
				array2 = IntegrityVerifier.FromB64Url(array[0]);
				array3 = IntegrityVerifier.FromB64Url(array[1]);
			}
			catch
			{
				return false;
			}
			if (!this._ecdsa.VerifyData(array2, array3, HashAlgorithmName.SHA256))
			{
				return false;
			}
			bool flag;
			try
			{
				using (JsonDocument jsonDocument = JsonDocument.Parse(array2, default(JsonDocumentOptions)))
				{
					JsonElement rootElement = jsonDocument.RootElement;
					JsonElement jsonElement;
					if (rootElement.TryGetProperty("unlimited", out jsonElement) && jsonElement.ValueKind == JsonValueKind.True)
					{
						expiresUtc = DateTime.MaxValue;
						flag = true;
					}
					else
					{
						JsonElement jsonElement2;
						if (rootElement.TryGetProperty("exp", out jsonElement2))
						{
							string @string = jsonElement2.GetString();
							if (@string != null)
							{
								expiresUtc = DateTimeOffset.Parse(@string, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal).UtcDateTime;
								return true;
							}
						}
						flag = false;
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006A20 File Offset: 0x00004C20
		private static byte[] FromB64Url(string s)
		{
			s = s.Replace('-', '+').Replace('_', '/');
			int num = s.Length % 4;
			byte[] array;
			if (num != 2)
			{
				if (num != 3)
				{
					array = Convert.FromBase64String(s);
				}
				else
				{
					array = Convert.FromBase64String(s + "=");
				}
			}
			else
			{
				array = Convert.FromBase64String(s + "==");
			}
			return array;
		}

		// Token: 0x04000090 RID: 144
		private const string PublicKeyB64 = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE6SlGCWFXB4qmsQrpRYNFZl1FSqlEQTDyj+QMzsL5K+4fMbwU4jWfquNWAgkFccMQ2Acm0rCQeddT0RgcH4I80g==";

		// Token: 0x04000091 RID: 145
		private readonly ECDsa _ecdsa;
	}
}
