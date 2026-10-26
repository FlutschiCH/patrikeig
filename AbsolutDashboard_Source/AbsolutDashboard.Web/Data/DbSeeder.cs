using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AbsolutDashboard.Web.Data
{
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	[Nullable(0)]
	public static class DbSeeder
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00015D74 File Offset: 0x00013F74
		public static async Task SeedAsync(IServiceProvider services, ILogger logger)
		{
			using (IServiceScope scope = services.CreateScope())
			{
				IServiceProvider sp = scope.ServiceProvider;
				await DbSeeder.SeedRolesAsync(sp.GetRequiredService<RoleManager<IdentityRole>>());
				await DbSeeder.BackfillOwnerAsync(sp.GetRequiredService<UserManager<ApplicationUser>>(), logger);
				await DbSeeder.SeedCatalogAsync(sp.GetRequiredService<ApplicationDbContext>(), logger);
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00015DC0 File Offset: 0x00013FC0
		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roles)
		{
			foreach (string role in new string[] { "Admin", "User" })
			{
				TaskAwaiter<bool> taskAwaiter = roles.RoleExistsAsync(role).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					await roles.CreateAsync(new IdentityRole(role));
				}
				role = null;
			}
			string[] array = null;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00015E04 File Offset: 0x00014004
		private static async Task BackfillOwnerAsync(UserManager<ApplicationUser> users, ILogger logger)
		{
			if (users.Users.Any<ApplicationUser>())
			{
				if (!users.Users.Any<ApplicationUser>((ApplicationUser u) => u.IsOwner))
				{
					IList<ApplicationUser> list = await users.GetUsersInRoleAsync("Admin");
					if (list.Count == 1)
					{
						ApplicationUser owner = list[0];
						owner.IsOwner = true;
						await users.UpdateAsync(owner);
						logger.LogInformation("Backfilled owner flag onto sole admin {Email}.", new object[] { owner.Email });
						owner = null;
					}
					else
					{
						logger.LogWarning("No owner account is set and {Count} admins exist — set IsOwner manually.", new object[] { list.Count });
					}
				}
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00015E50 File Offset: 0x00014050
		private static List<SnmpMetric> H3cResourceMetrics()
		{
			return new List<SnmpMetric>
			{
				new SnmpMetric
				{
					Label = "CPU load",
					Oid = "1.3.6.1.4.1.25506.2.6.1.1.1.1.6",
					Unit = "%",
					Group = "Resources",
					Walk = true,
					Aggregate = "max"
				},
				new SnmpMetric
				{
					Label = "Memory usage",
					Oid = "1.3.6.1.4.1.25506.2.6.1.1.1.1.8",
					Unit = "%",
					Group = "Resources",
					Walk = true,
					Aggregate = "max"
				}
			};
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00015EF4 File Offset: 0x000140F4
		private static async Task SeedCatalogAsync(ApplicationDbContext db, ILogger logger)
		{
			ValueTuple<string, string, string, string, int>[] categories = new ValueTuple<string, string, string, string, int>[]
			{
				new ValueTuple<string, string, string, string, int>("mobile", "5G Core", "Core", "5G mobile core network functions.", 0),
				new ValueTuple<string, string, string, string, int>("ims", "Voice server (IMS)", "IMS", "IMS core: voice and video calls over the 5G network.", 1),
				new ValueTuple<string, string, string, string, int>("bbu", "Base Stations", "BBU", "5G base stations / baseband controllers (gNodeB).", 2),
				new ValueTuple<string, string, string, string, int>("au", "Access Units", "AU", "Aggregation units in the radio chain.", 3),
				new ValueTuple<string, string, string, string, int>("eu", "Hub & Power", "EU", "Remote hub & power units in the radio chain.", 4),
				new ValueTuple<string, string, string, string, int>("ru", "PICO Sender", "RU", "PICO senders (radio units) serving the cells.", 5),
				new ValueTuple<string, string, string, string, int>("cell", "Cells", "Cell", "NR/LTE cells served by the base stations.", 6),
				new ValueTuple<string, string, string, string, int>("ue", "User Equipment", "UE", "Connected handsets (UEs) seen by the 5G core.", 7),
				new ValueTuple<string, string, string, string, int>("switch", "Switches", "SW", "Ethernet switches with per-port status.", 8)
			};
			Dictionary<string, DeviceCategory> dictionary = await db.DeviceCategories.ToDictionaryAsync<DeviceCategory, string>((DeviceCategory c) => c.Key, default(CancellationToken));
			Dictionary<string, DeviceCategory> byKey = dictionary;
			foreach (ValueTuple<string, string, string, string, int> valueTuple in categories)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string item3 = valueTuple.Item3;
				string item4 = valueTuple.Item4;
				int item5 = valueTuple.Item5;
				DeviceCategory deviceCategory;
				if (byKey.TryGetValue(item, out deviceCategory))
				{
					deviceCategory.Name = item2;
					deviceCategory.ShortName = item3;
					deviceCategory.Description = item4;
					deviceCategory.DisplayOrder = item5;
				}
				else
				{
					DeviceCategory deviceCategory2 = new DeviceCategory();
					deviceCategory2.Key = item;
					deviceCategory2.Name = item2;
					deviceCategory2.ShortName = item3;
					deviceCategory2.Description = item4;
					deviceCategory2.DisplayOrder = item5;
					db.DeviceCategories.Add(deviceCategory2);
					byKey[item] = deviceCategory2;
				}
			}
			await db.SaveChangesAsync(default(CancellationToken));
			List<ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>> templates = new List<ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>>
			{
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("bbu", "BTI nCELL-T5100", "BTI Wireless", "nCELL-T5100", CollectionMethod.BbuWebSocket, 20000, new TemplateDefinition
				{
					Bbu = new BbuDefinition()
				}, new ValueTuple<bool, string>(false, "BTI nCELL-T5100 base station (gNodeB). Add one per site — its access units, hub & power units, PICO senders and cells are then discovered automatically and appear in their own categories.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("au", "BTI AU300", "BTI Wireless", "AU300", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "au"
					}
				}, new ValueTuple<bool, string>(false, "BTI AU300 access/aggregation unit. Status read from its parent BBU's topology.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("eu", "BTI EU300", "BTI Wireless", "EU300", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "eu"
					}
				}, new ValueTuple<bool, string>(false, "BTI EU300 extension unit. Status read from its parent BBU's topology.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("ru", "BTI RU8250", "BTI Wireless", "RU8250", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "ru"
					}
				}, new ValueTuple<bool, string>(false, "BTI RU8250 radio unit. Status read from its parent BBU's topology.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("ru", "BTI RU4370", "BTI Wireless", "RU4370", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "ru"
					}
				}, new ValueTuple<bool, string>(false, "BTI RU4370 radio unit. Status read from its parent BBU's topology.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("ru", "BTI RU4460", "BTI Wireless", "RU4460", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "ru"
					}
				}, new ValueTuple<bool, string>(false, "BTI RU4460 radio unit. Status read from its parent BBU's topology.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("cell", "NR/LTE Cell", "BTI Wireless", "NR Cell", CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "cell"
					}
				}, new ValueTuple<bool, string>(false, "An NR/LTE cell served by a base unit. Config and op-state read from its BBU.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("switch", "Huawei Switch", "Huawei", "S-series", CollectionMethod.Snmp, 161, new TemplateDefinition
				{
					Snmp = new SnmpDefinition
					{
						WalkInterfaces = true,
						VendorEnterprise = new int?(2011),
						VendorName = "Huawei"
					}
				}, new ValueTuple<bool, string>(false, "Huawei switch. Reads per-port operational status from the IF-MIB. Locked to Huawei devices (sysObjectID enterprise 2011).")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("ims", "H3C 5gVIMS (IMS)", "H3C", "5gVIMS", CollectionMethod.Snmp, 161, new TemplateDefinition
				{
					Snmp = new SnmpDefinition
					{
						Metrics = DbSeeder.H3cResourceMetrics()
					}
				}, new ValueTuple<bool, string>(false, "H3C 5gVIMS IMS core. Reports reachability, identity and CPU/memory load over SNMP.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("mobile", "H3C 5gVSCN (5GC)", "H3C", "vSCN5100", CollectionMethod.SshScript, 22, new TemplateDefinition
				{
					Ssh = new SshDefinition
					{
						Interactive = true,
						Parser = "h3c-5gc",
						HostKeyAlgorithms = new List<string> { "ssh-rsa" },
						Commands = new List<string>
						{
							"screen-length disable", "display version", "display cpu-usage", "display memory", "display current-configuration", "display amf gnb", "display amf ue-context gnb globalgnbid mcc 999 mnc 99 gnb-id 7 bit-len 22", "sys", "upf session statistics enable", "probe",
							"display system internal upf session all"
						},
						FollowUps = new List<SshFollowUp>
						{
							new SshFollowUp
							{
								TokenPattern = "IMSI\\s*:\\s*(\\d{6,15})",
								CommandTemplate = "display system internal smf ue-context supi {0} brief"
							},
							new SshFollowUp
							{
								TokenPattern = "IMSI\\s*:\\s*(\\d{6,15})",
								CommandTemplate = "display amf ue-context supi {0}"
							},
							new SshFollowUp
							{
								TokenPattern = "UpCnxState\\s*:\\s*ACTIVATED(?:(?!----)[\\s\\S])*?IPv4\\s*:\\s*(\\d{1,3}(?:\\.\\d{1,3}){3})",
								CommandTemplate = "ping -c 2 -t 1000 {0}",
								Max = 8
							}
						}
					}
				}, new ValueTuple<bool, string>(false, "H3C 5gVSCN 5G core (AMF/SMF/UPF). Reports its own health and network configuration, and discovers every connected handset — throughput, connection state, serving base station and latency — into the User Equipment category.")),
				new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>("ue", "5G UE", null, null, CollectionMethod.ViaParent, 0, new TemplateDefinition
				{
					ViaParent = new ViaParentDefinition
					{
						Kind = "ue"
					}
				}, new ValueTuple<bool, string>(false, "A user-equipment PDU session reported by the 5G core. Status, speeds and identity read from its parent 5GC."))
			};
			string[] array2 = new string[] { "switch", "ims", "mobile" };
			for (int i = 0; i < array2.Length; i++)
			{
				templates.Add(new ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>(array2[i], "Generic SNMP Device", null, null, CollectionMethod.Snmp, 161, new TemplateDefinition
				{
					Snmp = new SnmpDefinition()
				}, new ValueTuple<bool, string>(true, "Any SNMP-capable device. Reads the MIB-II system group for reachability and identity.")));
			}
			List<DeviceTemplate> existing = await db.DeviceTemplates.ToListAsync<DeviceTemplate>(default(CancellationToken));
			Dictionary<ValueTuple<int, string>, DeviceTemplate> dictionary2 = existing.ToDictionary<DeviceTemplate, ValueTuple<int, string>>((DeviceTemplate t) => new ValueTuple<int, string>(t.CategoryId, t.Name));
			int added = 0;
			int updated = 0;
			foreach (ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>> valueTuple2 in templates)
			{
				DeviceCategory deviceCategory3 = byKey[valueTuple2.Item1];
				string text = TemplateDefinition.Serialize(valueTuple2.Item7);
				DeviceTemplate deviceTemplate;
				if (dictionary2.TryGetValue(new ValueTuple<int, string>(deviceCategory3.Id, valueTuple2.Item2), out deviceTemplate))
				{
					if (deviceTemplate.DefinitionJson != text || deviceTemplate.CollectionMethod != valueTuple2.Item5 || deviceTemplate.DefaultPort != valueTuple2.Item6 || deviceTemplate.Description != valueTuple2.Rest.Item2 || deviceTemplate.Vendor != valueTuple2.Item3 || deviceTemplate.Model != valueTuple2.Item4 || deviceTemplate.IsGeneric != valueTuple2.Rest.Item1)
					{
						deviceTemplate.DefinitionJson = text;
						deviceTemplate.CollectionMethod = valueTuple2.Item5;
						deviceTemplate.DefaultPort = valueTuple2.Item6;
						deviceTemplate.Description = valueTuple2.Rest.Item2;
						deviceTemplate.Vendor = valueTuple2.Item3;
						deviceTemplate.Model = valueTuple2.Item4;
						deviceTemplate.IsGeneric = valueTuple2.Rest.Item1;
						updated++;
					}
				}
				else
				{
					db.DeviceTemplates.Add(new DeviceTemplate
					{
						CategoryId = deviceCategory3.Id,
						Name = valueTuple2.Item2,
						Vendor = valueTuple2.Item3,
						Model = valueTuple2.Item4,
						Description = valueTuple2.Rest.Item2,
						CollectionMethod = valueTuple2.Item5,
						DefaultPort = valueTuple2.Item6,
						IsGeneric = valueTuple2.Rest.Item1,
						DefinitionJson = text
					});
					added++;
				}
			}
			HashSet<ValueTuple<int, string>> desired = templates.Select<ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>>, ValueTuple<int, string>>(([TupleElementNames(new string[]
			{
				"CatKey", "Name", "Vendor", "Model", "Method", "Port", "Def", "Generic", "Desc", null,
				null
			})] ValueTuple<string, string, string, string, CollectionMethod, int, TemplateDefinition, ValueTuple<bool, string>> t) => new ValueTuple<int, string>(byKey[t.Item1].Id, t.Item2)).ToHashSet<ValueTuple<int, string>>();
			IQueryable<Device> devices = db.Devices;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Device), "d");
			HashSet<int> usedTemplateIds = (await devices.Select<Device, int>(Expression.Lambda<Func<Device, int>>(Expression.Property(parameterExpression, methodof(Device.get_TemplateId())), new ParameterExpression[] { parameterExpression })).Distinct<int>().ToListAsync<int>(default(CancellationToken))).ToHashSet<int>();
			List<DeviceTemplate> list = existing.Where<DeviceTemplate>((DeviceTemplate t) => !desired.Contains(new ValueTuple<int, string>(t.CategoryId, t.Name)) && !usedTemplateIds.Contains(t.Id)).ToList<DeviceTemplate>();
			int removed = 0;
			if (list.Count > 0)
			{
				db.DeviceTemplates.RemoveRange(list);
				removed = list.Count;
			}
			if (added > 0 || updated > 0 || removed > 0)
			{
				await db.SaveChangesAsync(default(CancellationToken));
				logger.LogInformation("Device templates: {Added} added, {Updated} updated, {Removed} removed.", new object[] { added, updated, removed });
			}
		}

		// Token: 0x040001F2 RID: 498
		public const string AdminRole = "Admin";

		// Token: 0x040001F3 RID: 499
		public const string UserRole = "User";
	}
}
