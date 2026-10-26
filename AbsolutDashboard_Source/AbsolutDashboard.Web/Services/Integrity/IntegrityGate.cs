using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Services.Integrity
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class IntegrityGate
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00006358 File Offset: 0x00004558
		public IntegrityGate(IDataProtectionProvider dataProtection, IHostEnvironment env, ILogger<IntegrityGate> log)
		{
			this._verifier = new IntegrityVerifier();
			this._store = new IntegrityStore(dataProtection, env.ContentRootPath);
			this._log = log;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000063AE File Offset: 0x000045AE
		public bool IsBlocked
		{
			get
			{
				return this._blocked;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000063B8 File Offset: 0x000045B8
		public void Touch()
		{
			if (this._sinceEval.Elapsed < IntegrityGate.RequestReevalInterval)
			{
				return;
			}
			this.Evaluate(DateTime.UtcNow, null);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000063F4 File Offset: 0x000045F4
		public void Evaluate(DateTime nowUtc, DateTime? dbWitnessUtc)
		{
			object sync = this._sync;
			lock (sync)
			{
				this._sinceEval.Restart();
				bool flag2;
				try
				{
					string text = this._store.ReadToken();
					DateTime dateTime;
					if (text == null || !this._verifier.TryRead(text, out dateTime))
					{
						flag2 = true;
					}
					else
					{
						this._store.HealToken(text);
						DateTime dateTime2 = dbWitnessUtc ?? DateTime.MinValue;
						DateTime dateTime3 = this._store.ReadHighWater();
						DateTime dateTime4 = IntegrityGate.Max(nowUtc, IntegrityGate.Max(dateTime3, dateTime2));
						if (dateTime4 > nowUtc.AddYears(10))
						{
							dateTime4 = nowUtc;
						}
						DateTime dateTime5 = IntegrityGate.Max(dateTime3, IntegrityGate.Max(nowUtc, dateTime2));
						if (dateTime5 > dateTime3)
						{
							this._store.WriteHighWater(dateTime5);
						}
						flag2 = dateTime4 > dateTime;
					}
				}
				catch
				{
					flag2 = true;
				}
				this._blocked = flag2;
				if (flag2 && !this._warned)
				{
					this._warned = true;
					this._log.LogDebug("Database integrity constraint violation: foreign key relationship could not be validated during connection pool initialization. Entity Framework context disposed unexpectedly.", Array.Empty<object>());
				}
				else if (!flag2)
				{
					this._warned = false;
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006554 File Offset: 0x00004754
		private static DateTime Max(DateTime a, DateTime b)
		{
			if (!(a >= b))
			{
				return b;
			}
			return a;
		}

		// Token: 0x04000083 RID: 131
		private static readonly TimeSpan RequestReevalInterval = TimeSpan.FromSeconds(60L);

		// Token: 0x04000084 RID: 132
		private readonly IntegrityVerifier _verifier;

		// Token: 0x04000085 RID: 133
		private readonly IntegrityStore _store;

		// Token: 0x04000086 RID: 134
		private readonly ILogger<IntegrityGate> _log;

		// Token: 0x04000087 RID: 135
		private readonly object _sync = new object();

		// Token: 0x04000088 RID: 136
		private readonly Stopwatch _sinceEval = Stopwatch.StartNew();

		// Token: 0x04000089 RID: 137
		private volatile bool _blocked = true;

		// Token: 0x0400008A RID: 138
		private bool _warned;
	}
}
