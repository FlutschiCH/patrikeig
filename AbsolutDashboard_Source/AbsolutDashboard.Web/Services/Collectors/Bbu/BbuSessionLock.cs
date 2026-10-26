using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AbsolutDashboard.Web.Services.Collectors.Bbu
{
	// Token: 0x0200007B RID: 123
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BbuSessionLock
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x00015724 File Offset: 0x00013924
		private SemaphoreSlim Gate(int deviceId)
		{
			return this._gates.GetOrAdd(deviceId, (int _) => new SemaphoreSlim(1, 1));
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00015751 File Offset: 0x00013951
		public bool TryEnter(int deviceId)
		{
			return this.Gate(deviceId).Wait(0);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00015760 File Offset: 0x00013960
		public Task<bool> WaitAsync(int deviceId, TimeSpan timeout, CancellationToken ct)
		{
			return this.Gate(deviceId).WaitAsync(timeout, ct);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00015770 File Offset: 0x00013970
		public void Release(int deviceId)
		{
			this.Gate(deviceId).Release();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001577F File Offset: 0x0001397F
		public bool IsBusy(int deviceId)
		{
			return this.Gate(deviceId).CurrentCount == 0;
		}

		// Token: 0x040001E6 RID: 486
		private readonly ConcurrentDictionary<int, SemaphoreSlim> _gates = new ConcurrentDictionary<int, SemaphoreSlim>();
	}
}
