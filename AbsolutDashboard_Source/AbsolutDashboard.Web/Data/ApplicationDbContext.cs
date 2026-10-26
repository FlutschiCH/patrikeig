using System;
using System.Runtime.CompilerServices;
using AbsolutDashboard.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsolutDashboard.Web.Data
{
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x00015C55 File Offset: 0x00013E55
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00015C5E File Offset: 0x00013E5E
		public DbSet<DeviceCategory> DeviceCategories
		{
			get
			{
				return this.Set<DeviceCategory>();
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00015C66 File Offset: 0x00013E66
		public DbSet<DeviceTemplate> DeviceTemplates
		{
			get
			{
				return this.Set<DeviceTemplate>();
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00015C6E File Offset: 0x00013E6E
		public DbSet<Device> Devices
		{
			get
			{
				return this.Set<Device>();
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00015C76 File Offset: 0x00013E76
		public DbSet<DevicePollResult> DevicePollResults
		{
			get
			{
				return this.Set<DevicePollResult>();
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00015C7E File Offset: 0x00013E7E
		public DbSet<ThroughputSample> ThroughputSamples
		{
			get
			{
				return this.Set<ThroughputSample>();
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00015C88 File Offset: 0x00013E88
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<DeviceCategory>(delegate(EntityTypeBuilder<DeviceCategory> e)
			{
				e.HasIndex((DeviceCategory c) => c.Key).IsUnique(true);
				e.HasMany<DeviceTemplate>((DeviceCategory c) => c.Templates).WithOne((DeviceTemplate t) => t.Category).HasForeignKey((DeviceTemplate t) => (object)t.CategoryId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			builder.Entity<DeviceTemplate>(delegate(EntityTypeBuilder<DeviceTemplate> e)
			{
				e.HasIndex((DeviceTemplate t) => new { t.CategoryId, t.Name }).IsUnique(true);
			});
			builder.Entity<Device>(delegate(EntityTypeBuilder<Device> e)
			{
				e.HasIndex((Device d) => (object)d.CategoryId);
				e.HasIndex((Device d) => d.SerialNumber);
				e.HasOne<DeviceCategory>((Device d) => d.Category).WithMany((DeviceCategory c) => c.Devices).HasForeignKey((Device d) => (object)d.CategoryId)
					.OnDelete(DeleteBehavior.Restrict);
				e.HasOne<DeviceTemplate>((Device d) => d.Template).WithMany((DeviceTemplate t) => t.Devices).HasForeignKey((Device d) => (object)d.TemplateId)
					.OnDelete(DeleteBehavior.Restrict);
				e.HasOne<Device>((Device d) => d.ParentDevice).WithMany((Device d) => d.Children).HasForeignKey((Device d) => (object)d.ParentDeviceId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			builder.Entity<DevicePollResult>(delegate(EntityTypeBuilder<DevicePollResult> e)
			{
				e.HasIndex((DevicePollResult r) => new { r.DeviceId, r.Timestamp });
				e.HasOne<Device>((DevicePollResult r) => r.Device).WithMany((Device d) => d.PollResults).HasForeignKey((DevicePollResult r) => (object)r.DeviceId)
					.OnDelete(DeleteBehavior.Cascade);
			});
			builder.Entity<ThroughputSample>(delegate(EntityTypeBuilder<ThroughputSample> e)
			{
				e.HasIndex((ThroughputSample s) => (object)s.Timestamp);
			});
		}
	}
}
