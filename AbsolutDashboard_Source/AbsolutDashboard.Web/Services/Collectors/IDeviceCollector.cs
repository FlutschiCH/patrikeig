using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	public interface IDeviceCollector
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600017D RID: 381
		CollectionMethod Method { get; }

		// Token: 0x0600017E RID: 382
		Task<PollOutcome> CollectAsync(Device device, CancellationToken ct);
	}
}
