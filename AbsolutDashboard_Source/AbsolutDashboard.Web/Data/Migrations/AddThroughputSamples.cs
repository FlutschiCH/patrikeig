using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AbsolutDashboard.Web.Data.Migrations
{
	// Token: 0x02000088 RID: 136
	[DbContext(typeof(ApplicationDbContext))]
	[Migration("20260822212828_AddThroughputSamples")]
	public class AddThroughputSamples : Migration
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00017FEC File Offset: 0x000161EC
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable("ThroughputSamples", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				Timestamp = table.Column<DateTime>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				DlMbps = table.Column<double>("REAL", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				UlMbps = table.Column<double>("REAL", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_ThroughputSamples", (<>f__AnonymousType15<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
			}, null);
			migrationBuilder.CreateIndex("IX_ThroughputSamples_Timestamp", "ThroughputSamples", "Timestamp", null, false, null, null);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001805F File Offset: 0x0001625F
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable("ThroughputSamples", null);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018070 File Offset: 0x00016270
		protected override void BuildTargetModel(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("ProductVersion", "10.0.9");
			modelBuilder.Entity("AbsolutDashboard.Web.Data.ApplicationUser", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("Id").HasColumnType<string>("TEXT");
				b.Property<int>("AccessFailedCount").HasColumnType<int>("INTEGER");
				b.Property<string>("ConcurrencyStamp").IsConcurrencyToken(true).HasColumnType<string>("TEXT");
				b.Property<string>("Email").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.Property<bool>("EmailConfirmed").HasColumnType<bool>("INTEGER");
				b.Property<bool>("IsOwner").HasColumnType<bool>("INTEGER");
				b.Property<bool>("LockoutEnabled").HasColumnType<bool>("INTEGER");
				b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType<DateTimeOffset?>("TEXT");
				b.Property<string>("NormalizedEmail").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.Property<string>("NormalizedUserName").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.Property<string>("PasswordHash").HasColumnType<string>("TEXT");
				b.Property<string>("PhoneNumber").HasColumnType<string>("TEXT");
				b.Property<bool>("PhoneNumberConfirmed").HasColumnType<bool>("INTEGER");
				b.Property<string>("SecurityStamp").HasColumnType<string>("TEXT");
				b.Property<bool>("TwoFactorEnabled").HasColumnType<bool>("INTEGER");
				b.Property<string>("UserName").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "NormalizedEmail" }).HasDatabaseName("EmailIndex");
				b.HasIndex(new string[] { "NormalizedUserName" }).IsUnique(true).HasDatabaseName("UserNameIndex");
				b.ToTable("AspNetUsers", null);
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.Device", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<int>("CategoryId").HasColumnType<int>("INTEGER");
				b.Property<string>("ChildIdentifier").HasColumnType<string>("TEXT");
				b.Property<int>("CollectionMethod").HasColumnType<int>("INTEGER");
				b.Property<double?>("CoordinateX").HasColumnType<double?>("REAL");
				b.Property<double?>("CoordinateY").HasColumnType<double?>("REAL");
				b.Property<double?>("CoordinateZ").HasColumnType<double?>("REAL");
				b.Property<DateTime>("CreatedAt").HasColumnType<DateTime>("TEXT");
				b.Property<bool>("Enabled").HasColumnType<bool>("INTEGER");
				b.Property<string>("IpAddress").HasColumnType<string>("TEXT");
				b.Property<string>("LastError").HasColumnType<string>("TEXT");
				b.Property<DateTime?>("LastPolledAt").HasColumnType<DateTime?>("TEXT");
				b.Property<bool?>("LastReachable").HasColumnType<bool?>("INTEGER");
				b.Property<string>("LastSnapshotJson").HasColumnType<string>("TEXT");
				b.Property<string>("Name").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("Notes").HasColumnType<string>("TEXT");
				b.Property<int?>("ParentDeviceId").HasColumnType<int?>("INTEGER");
				b.Property<int>("PollIntervalSeconds").HasColumnType<int>("INTEGER");
				b.Property<int>("Port").HasColumnType<int>("INTEGER");
				b.Property<int>("SerialBaudRate").HasColumnType<int>("INTEGER");
				b.Property<string>("SerialFraming").HasColumnType<string>("TEXT");
				b.Property<string>("SerialNumber").HasColumnType<string>("TEXT");
				b.Property<string>("SerialPort").HasColumnType<string>("TEXT");
				b.Property<string>("SnmpCommunityEnc").HasColumnType<string>("TEXT");
				b.Property<string>("SnmpVersion").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("SshPasswordEnc").HasColumnType<string>("TEXT");
				b.Property<string>("SshUsername").HasColumnType<string>("TEXT");
				b.Property<int>("TemplateId").HasColumnType<int>("INTEGER");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "CategoryId" });
				b.HasIndex(new string[] { "ParentDeviceId" });
				b.HasIndex(new string[] { "SerialNumber" });
				b.HasIndex(new string[] { "TemplateId" });
				b.ToTable("Devices");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DeviceCategory", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<string>("Description").HasColumnType<string>("TEXT");
				b.Property<int>("DisplayOrder").HasColumnType<int>("INTEGER");
				b.Property<string>("Key").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("Name").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("ShortName").IsRequired(true).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "Key" }).IsUnique(true);
				b.ToTable("DeviceCategories");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DevicePollResult", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<int>("DeviceId").HasColumnType<int>("INTEGER");
				b.Property<string>("Error").HasColumnType<string>("TEXT");
				b.Property<int?>("LatencyMs").HasColumnType<int?>("INTEGER");
				b.Property<bool>("Reachable").HasColumnType<bool>("INTEGER");
				b.Property<string>("SnapshotJson").HasColumnType<string>("TEXT");
				b.Property<DateTime>("Timestamp").HasColumnType<DateTime>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "DeviceId", "Timestamp" });
				b.ToTable("DevicePollResults");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DeviceTemplate", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<int>("CategoryId").HasColumnType<int>("INTEGER");
				b.Property<int>("CollectionMethod").HasColumnType<int>("INTEGER");
				b.Property<int>("DefaultPollIntervalSeconds").HasColumnType<int>("INTEGER");
				b.Property<int>("DefaultPort").HasColumnType<int>("INTEGER");
				b.Property<string>("DefinitionJson").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("Description").HasColumnType<string>("TEXT");
				b.Property<bool>("IsGeneric").HasColumnType<bool>("INTEGER");
				b.Property<string>("Model").HasColumnType<string>("TEXT");
				b.Property<string>("Name").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("Vendor").HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "CategoryId", "Name" }).IsUnique(true);
				b.ToTable("DeviceTemplates");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.ThroughputSample", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<double>("DlMbps").HasColumnType<double>("REAL");
				b.Property<DateTime>("Timestamp").HasColumnType<DateTime>("TEXT");
				b.Property<double>("UlMbps").HasColumnType<double>("REAL");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "Timestamp" });
				b.ToTable("ThroughputSamples");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("Id").HasColumnType<string>("TEXT");
				b.Property<string>("ConcurrencyStamp").IsConcurrencyToken(true).HasColumnType<string>("TEXT");
				b.Property<string>("Name").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.Property<string>("NormalizedName").HasMaxLength(256).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "NormalizedName" }).IsUnique(true).HasDatabaseName("RoleNameIndex");
				b.ToTable("AspNetRoles", null);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<string>("ClaimType").HasColumnType<string>("TEXT");
				b.Property<string>("ClaimValue").HasColumnType<string>("TEXT");
				b.Property<string>("RoleId").IsRequired(true).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "RoleId" });
				b.ToTable("AspNetRoleClaims", null);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType<int>("INTEGER");
				b.Property<string>("ClaimType").HasColumnType<string>("TEXT");
				b.Property<string>("ClaimValue").HasColumnType<string>("TEXT");
				b.Property<string>("UserId").IsRequired(true).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "UserId" });
				b.ToTable("AspNetUserClaims", null);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("LoginProvider").HasColumnType<string>("TEXT");
				b.Property<string>("ProviderKey").HasColumnType<string>("TEXT");
				b.Property<string>("ProviderDisplayName").HasColumnType<string>("TEXT");
				b.Property<string>("UserId").IsRequired(true).HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "LoginProvider", "ProviderKey" });
				b.HasIndex(new string[] { "UserId" });
				b.ToTable("AspNetUserLogins", null);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("UserId").HasColumnType<string>("TEXT");
				b.Property<string>("RoleId").HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "UserId", "RoleId" });
				b.HasIndex(new string[] { "RoleId" });
				b.ToTable("AspNetUserRoles", null);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
			{
				b.Property<string>("UserId").HasColumnType<string>("TEXT");
				b.Property<string>("LoginProvider").HasColumnType<string>("TEXT");
				b.Property<string>("Name").HasColumnType<string>("TEXT");
				b.Property<string>("Value").HasColumnType<string>("TEXT");
				b.HasKey(new string[] { "UserId", "LoginProvider", "Name" });
				b.ToTable("AspNetUserTokens", null);
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.Device", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.Entities.DeviceCategory", "Category").WithMany("Devices").HasForeignKey(new string[] { "CategoryId" })
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired(true);
				b.HasOne("AbsolutDashboard.Web.Data.Entities.Device", "ParentDevice").WithMany("Children").HasForeignKey(new string[] { "ParentDeviceId" })
					.OnDelete(DeleteBehavior.Cascade);
				b.HasOne("AbsolutDashboard.Web.Data.Entities.DeviceTemplate", "Template").WithMany("Devices").HasForeignKey(new string[] { "TemplateId" })
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired(true);
				b.Navigation("Category");
				b.Navigation("ParentDevice");
				b.Navigation("Template");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DevicePollResult", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.Entities.Device", "Device").WithMany("PollResults").HasForeignKey(new string[] { "DeviceId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
				b.Navigation("Device");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DeviceTemplate", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.Entities.DeviceCategory", "Category").WithMany("Templates").HasForeignKey(new string[] { "CategoryId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
				b.Navigation("Category");
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany(null).HasForeignKey(new string[] { "RoleId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.ApplicationUser", null).WithMany(null).HasForeignKey(new string[] { "UserId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.ApplicationUser", null).WithMany(null).HasForeignKey(new string[] { "UserId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany(null).HasForeignKey(new string[] { "RoleId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
				b.HasOne("AbsolutDashboard.Web.Data.ApplicationUser", null).WithMany(null).HasForeignKey(new string[] { "UserId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
			});
			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
			{
				b.HasOne("AbsolutDashboard.Web.Data.ApplicationUser", null).WithMany(null).HasForeignKey(new string[] { "UserId" })
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(true);
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.Device", delegate(EntityTypeBuilder b)
			{
				b.Navigation("Children");
				b.Navigation("PollResults");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DeviceCategory", delegate(EntityTypeBuilder b)
			{
				b.Navigation("Devices");
				b.Navigation("Templates");
			});
			modelBuilder.Entity("AbsolutDashboard.Web.Data.Entities.DeviceTemplate", delegate(EntityTypeBuilder b)
			{
				b.Navigation("Devices");
			});
		}
	}
}
