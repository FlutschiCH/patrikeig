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
	// Token: 0x02000082 RID: 130
	[DbContext(typeof(ApplicationDbContext))]
	[Migration("20260612172135_InitialCreate")]
	public class InitialCreate : Migration
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x00015F40 File Offset: 0x00014140
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable("AspNetRoles", delegate([Nullable(1)] ColumnsBuilder table)
			{
				string text = "TEXT";
				bool? flag = null;
				int? num = null;
				bool flag2 = false;
				string text2 = null;
				bool flag3 = false;
				object obj = null;
				string text3 = null;
				string text4 = null;
				bool? flag4 = null;
				string text5 = null;
				string text6 = null;
				int? num2 = null;
				int? num3 = null;
				OperationBuilder<AddColumnOperation> operationBuilder = table.Column<string>(text, flag, num, flag2, text2, flag3, obj, text3, text4, flag4, text5, text6, num2, num3, null);
				string text7 = "TEXT";
				num3 = new int?(256);
				OperationBuilder<AddColumnOperation> operationBuilder2 = table.Column<string>(text7, null, num3, false, null, true, null, null, null, null, null, null, null, null, null);
				string text8 = "TEXT";
				num3 = new int?(256);
				return new
				{
					Id = operationBuilder,
					Name = operationBuilder2,
					NormalizedName = table.Column<string>(text8, null, num3, false, null, true, null, null, null, null, null, null, null, null, null),
					ConcurrencyStamp = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
				};
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetRoles", (<>f__AnonymousType2<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
			}, null);
			migrationBuilder.CreateTable("AspNetUsers", delegate([Nullable(1)] ColumnsBuilder table)
			{
				string text9 = "TEXT";
				bool? flag5 = null;
				int? num4 = null;
				bool flag6 = false;
				string text10 = null;
				bool flag7 = false;
				object obj2 = null;
				string text11 = null;
				string text12 = null;
				bool? flag8 = null;
				string text13 = null;
				string text14 = null;
				int? num5 = null;
				int? num6 = null;
				OperationBuilder<AddColumnOperation> operationBuilder3 = table.Column<string>(text9, flag5, num4, flag6, text10, flag7, obj2, text11, text12, flag8, text13, text14, num5, num6, null);
				string text15 = "TEXT";
				num6 = new int?(256);
				OperationBuilder<AddColumnOperation> operationBuilder4 = table.Column<string>(text15, null, num6, false, null, true, null, null, null, null, null, null, null, null, null);
				string text16 = "TEXT";
				num6 = new int?(256);
				OperationBuilder<AddColumnOperation> operationBuilder5 = table.Column<string>(text16, null, num6, false, null, true, null, null, null, null, null, null, null, null, null);
				string text17 = "TEXT";
				num6 = new int?(256);
				OperationBuilder<AddColumnOperation> operationBuilder6 = table.Column<string>(text17, null, num6, false, null, true, null, null, null, null, null, null, null, null, null);
				string text18 = "TEXT";
				num6 = new int?(256);
				return new
				{
					Id = operationBuilder3,
					UserName = operationBuilder4,
					NormalizedUserName = operationBuilder5,
					Email = operationBuilder6,
					NormalizedEmail = table.Column<string>(text18, null, num6, false, null, true, null, null, null, null, null, null, null, null, null),
					EmailConfirmed = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
					PasswordHash = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
					SecurityStamp = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
					ConcurrencyStamp = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
					PhoneNumber = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
					PhoneNumberConfirmed = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
					TwoFactorEnabled = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
					LockoutEnd = table.Column<DateTimeOffset>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
					LockoutEnabled = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
					AccessFailedCount = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
				};
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetUsers", (<>f__AnonymousType3<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
			}, null);
			migrationBuilder.CreateTable("DeviceCategories", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				Key = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Name = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				ShortName = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Description = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				DisplayOrder = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_DeviceCategories", (<>f__AnonymousType4<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
			}, null);
			migrationBuilder.CreateTable("AspNetRoleClaims", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				RoleId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				ClaimType = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				ClaimValue = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetRoleClaims", (<>f__AnonymousType5<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
				table.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId", (<>f__AnonymousType5<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("AspNetUserClaims", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				UserId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				ClaimType = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				ClaimValue = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetUserClaims", (<>f__AnonymousType6<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
				table.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId", (<>f__AnonymousType6<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("AspNetUserLogins", ([Nullable(1)] ColumnsBuilder table) => new
			{
				LoginProvider = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				ProviderKey = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				ProviderDisplayName = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				UserId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetUserLogins", (<>f__AnonymousType7<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => new { x.LoginProvider, x.ProviderKey });
				table.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId", (<>f__AnonymousType7<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("AspNetUserRoles", ([Nullable(1)] ColumnsBuilder table) => new
			{
				UserId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				RoleId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetUserRoles", (<>f__AnonymousType9<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => new { x.UserId, x.RoleId });
				table.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", (<>f__AnonymousType9<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
				table.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", (<>f__AnonymousType9<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("AspNetUserTokens", ([Nullable(1)] ColumnsBuilder table) => new
			{
				UserId = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				LoginProvider = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Name = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Value = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_AspNetUserTokens", (<>f__AnonymousType10<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => new { x.UserId, x.LoginProvider, x.Name });
				table.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId", (<>f__AnonymousType10<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("DeviceTemplates", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				CategoryId = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Name = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Vendor = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				Model = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				Description = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				CollectionMethod = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				DefaultPort = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				DefaultPollIntervalSeconds = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				IsGeneric = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				DefinitionJson = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_DeviceTemplates", (<>f__AnonymousType12<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
				table.ForeignKey("FK_DeviceTemplates_DeviceCategories_CategoryId", (<>f__AnonymousType12<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.CategoryId, "DeviceCategories", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateTable("Devices", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				Name = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				CategoryId = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				TemplateId = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				CollectionMethod = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				IpAddress = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				Port = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				SnmpVersion = table.Column<string>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				SnmpCommunityEnc = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				SshUsername = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				SshPasswordEnc = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				ParentDeviceId = table.Column<int>("INTEGER", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				ChildIdentifier = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				Enabled = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				PollIntervalSeconds = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Notes = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				CreatedAt = table.Column<DateTime>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				LastPolledAt = table.Column<DateTime>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				LastReachable = table.Column<bool>("INTEGER", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				LastError = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				LastSnapshotJson = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_Devices", (<>f__AnonymousType13<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
				table.ForeignKey("FK_Devices_DeviceCategories_CategoryId", (<>f__AnonymousType13<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.CategoryId, "DeviceCategories", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
				table.ForeignKey("FK_Devices_DeviceTemplates_TemplateId", (<>f__AnonymousType13<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.TemplateId, "DeviceTemplates", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
				table.ForeignKey("FK_Devices_Devices_ParentDeviceId", (<>f__AnonymousType13<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.ParentDeviceId, "Devices", "Id", null, ReferentialAction.NoAction, ReferentialAction.Restrict);
			}, null);
			migrationBuilder.CreateTable("DevicePollResults", ([Nullable(1)] ColumnsBuilder table) => new
			{
				Id = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null).Annotation("Sqlite:Autoincrement", true),
				DeviceId = table.Column<int>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Timestamp = table.Column<DateTime>("TEXT", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				Reachable = table.Column<bool>("INTEGER", null, null, false, null, false, null, null, null, null, null, null, null, null, null),
				LatencyMs = table.Column<int>("INTEGER", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				SnapshotJson = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null),
				Error = table.Column<string>("TEXT", null, null, false, null, true, null, null, null, null, null, null, null, null, null)
			}, null, delegate([Nullable(new byte[] { 1, 0 })] table)
			{
				table.PrimaryKey("PK_DevicePollResults", (<>f__AnonymousType14<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.Id);
				table.ForeignKey("FK_DevicePollResults_Devices_DeviceId", (<>f__AnonymousType14<OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>, OperationBuilder<AddColumnOperation>> x) => x.DeviceId, "Devices", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			}, null);
			migrationBuilder.CreateIndex("IX_AspNetRoleClaims_RoleId", "AspNetRoleClaims", "RoleId", null, false, null, null);
			migrationBuilder.CreateIndex("RoleNameIndex", "AspNetRoles", "NormalizedName", null, true, null, null);
			migrationBuilder.CreateIndex("IX_AspNetUserClaims_UserId", "AspNetUserClaims", "UserId", null, false, null, null);
			migrationBuilder.CreateIndex("IX_AspNetUserLogins_UserId", "AspNetUserLogins", "UserId", null, false, null, null);
			migrationBuilder.CreateIndex("IX_AspNetUserRoles_RoleId", "AspNetUserRoles", "RoleId", null, false, null, null);
			migrationBuilder.CreateIndex("EmailIndex", "AspNetUsers", "NormalizedEmail", null, false, null, null);
			migrationBuilder.CreateIndex("UserNameIndex", "AspNetUsers", "NormalizedUserName", null, true, null, null);
			migrationBuilder.CreateIndex("IX_DeviceCategories_Key", "DeviceCategories", "Key", null, true, null, null);
			migrationBuilder.CreateIndex("IX_DevicePollResults_DeviceId_Timestamp", "DevicePollResults", new string[] { "DeviceId", "Timestamp" }, null, false, null, null);
			migrationBuilder.CreateIndex("IX_Devices_CategoryId", "Devices", "CategoryId", null, false, null, null);
			migrationBuilder.CreateIndex("IX_Devices_ParentDeviceId", "Devices", "ParentDeviceId", null, false, null, null);
			migrationBuilder.CreateIndex("IX_Devices_TemplateId", "Devices", "TemplateId", null, false, null, null);
			migrationBuilder.CreateIndex("IX_DeviceTemplates_CategoryId_Name", "DeviceTemplates", new string[] { "CategoryId", "Name" }, null, true, null, null);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00016408 File Offset: 0x00014608
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable("AspNetRoleClaims", null);
			migrationBuilder.DropTable("AspNetUserClaims", null);
			migrationBuilder.DropTable("AspNetUserLogins", null);
			migrationBuilder.DropTable("AspNetUserRoles", null);
			migrationBuilder.DropTable("AspNetUserTokens", null);
			migrationBuilder.DropTable("DevicePollResults", null);
			migrationBuilder.DropTable("AspNetRoles", null);
			migrationBuilder.DropTable("AspNetUsers", null);
			migrationBuilder.DropTable("Devices", null);
			migrationBuilder.DropTable("DeviceTemplates", null);
			migrationBuilder.DropTable("DeviceCategories", null);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000164A4 File Offset: 0x000146A4
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
				b.Property<string>("SnmpCommunityEnc").HasColumnType<string>("TEXT");
				b.Property<string>("SnmpVersion").IsRequired(true).HasColumnType<string>("TEXT");
				b.Property<string>("SshPasswordEnc").HasColumnType<string>("TEXT");
				b.Property<string>("SshUsername").HasColumnType<string>("TEXT");
				b.Property<int>("TemplateId").HasColumnType<int>("INTEGER");
				b.HasKey(new string[] { "Id" });
				b.HasIndex(new string[] { "CategoryId" });
				b.HasIndex(new string[] { "ParentDeviceId" });
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
					.OnDelete(DeleteBehavior.Restrict);
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
