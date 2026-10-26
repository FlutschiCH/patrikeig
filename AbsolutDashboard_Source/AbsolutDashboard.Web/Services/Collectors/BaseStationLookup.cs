using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbsolutDashboard.Web.Services.Collectors
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	public static class BaseStationLookup
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00006A84 File Offset: 0x00004C84
		public static async Task<Dictionary<string, int>> ByGnbIdAsync(ApplicationDbContext db, CancellationToken ct = default(CancellationToken))
		{
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			IQueryable<Device> queryable = devices.Where<Device>(Expression.Lambda<Func<Device, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(Expression.Property(parameterExpression, methodof(Device.get_Category())), methodof(DeviceCategory.get_Key())), Expression.Constant("bbu", typeof(string))), Expression.NotEqual(Expression.Property(parameterExpression, methodof(Device.get_LastSnapshotJson())), Expression.Constant(null, typeof(string)))), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(Device), "d");
			var list = await queryable.Select(Expression.Lambda(Expression.New(methodof(<>f__AnonymousType16<int, string>..ctor(<Id>j__TPar, <LastSnapshotJson>j__TPar)), new Expression[]
			{
				Expression.Property(parameterExpression, methodof(Device.get_Id())),
				Expression.Property(parameterExpression, methodof(Device.get_LastSnapshotJson()))
			}, new MemberInfo[]
			{
				methodof(<>f__AnonymousType16<int, string>.get_Id()),
				methodof(<>f__AnonymousType16<int, string>.get_LastSnapshotJson())
			}), new ParameterExpression[] { parameterExpression })).ToListAsync(ct);
			Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
			foreach (var anon in list)
			{
				DeviceSnapshot deviceSnapshot = SnapshotSerializer.Deserialize(anon.LastSnapshotJson);
				string text;
				if (deviceSnapshot == null)
				{
					text = null;
				}
				else
				{
					Metric metric = deviceSnapshot.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "gNB ID");
					if (metric == null)
					{
						text = null;
					}
					else
					{
						string value = metric.Value;
						text = ((value != null) ? value.Trim() : null);
					}
				}
				string text2 = text;
				if (!string.IsNullOrEmpty(text2))
				{
					List<int> list2;
					if (!dictionary.TryGetValue(text2, out list2))
					{
						list2 = (dictionary[text2] = new List<int>());
					}
					if (!list2.Contains(anon.Id))
					{
						list2.Add(anon.Id);
					}
				}
			}
			return dictionary.Where<KeyValuePair<string, List<int>>>((KeyValuePair<string, List<int>> kv) => kv.Value.Count == 1).ToDictionary<KeyValuePair<string, List<int>>, string, int>((KeyValuePair<string, List<int>> kv) => kv.Key, (KeyValuePair<string, List<int>> kv) => kv.Value[0], StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006AD0 File Offset: 0x00004CD0
		public static int? Resolve(IReadOnlyDictionary<string, int> byGnbId, [Nullable(2)] DeviceSnapshot ue)
		{
			string text;
			if (ue == null)
			{
				text = null;
			}
			else
			{
				Metric metric = ue.Metrics.FirstOrDefault<Metric>((Metric m) => m.Label == "gNB ID");
				if (metric == null)
				{
					text = null;
				}
				else
				{
					string value = metric.Value;
					text = ((value != null) ? value.Trim() : null);
				}
			}
			string text2 = text;
			int num;
			if (string.IsNullOrEmpty(text2) || !byGnbId.TryGetValue(text2, out num))
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x04000092 RID: 146
		public const string GnbIdLabel = "gNB ID";
	}
}
