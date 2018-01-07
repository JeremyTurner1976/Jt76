namespace Jt76.WebApi.Migrations.IdentityDb
{
	using System;
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class InitialIdentityMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"AspNetRoles",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					ConcurrencyStamp = table.Column<string>(nullable: true),
					CreatedBy = table.Column<string>(nullable: true),
					CreatedDate = table.Column<DateTime>(nullable: false),
					Description = table.Column<string>(nullable: true),
					Name = table.Column<string>(maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
					UpdatedBy = table.Column<string>(nullable: true),
					UpdatedDate = table.Column<DateTime>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

			migrationBuilder.CreateTable(
				"AspNetUsers",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					AccessFailedCount = table.Column<int>(nullable: false),
					ConcurrencyStamp = table.Column<string>(nullable: true),
					Configuration = table.Column<string>(nullable: true),
					CreatedBy = table.Column<string>(nullable: true),
					CreatedDate = table.Column<DateTime>(nullable: false),
					Email = table.Column<string>(maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(nullable: false),
					FullName = table.Column<string>(nullable: true),
					IsEnabled = table.Column<bool>(nullable: false),
					JobTitle = table.Column<string>(nullable: true),
					LockoutEnabled = table.Column<bool>(nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
					NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
					PasswordHash = table.Column<string>(nullable: true),
					PhoneNumber = table.Column<string>(nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(nullable: false),
					SecurityStamp = table.Column<string>(nullable: true),
					TwoFactorEnabled = table.Column<bool>(nullable: false),
					UpdatedBy = table.Column<string>(nullable: true),
					UpdatedDate = table.Column<DateTime>(nullable: false),
					UserName = table.Column<string>(maxLength: 256, nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

			migrationBuilder.CreateTable(
				"OpenIddictApplications",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					ClientId = table.Column<string>(nullable: false),
					ClientSecret = table.Column<string>(nullable: true),
					ConcurrencyToken = table.Column<string>(nullable: true),
					DisplayName = table.Column<string>(nullable: true),
					PostLogoutRedirectUris = table.Column<string>(nullable: true),
					RedirectUris = table.Column<string>(nullable: true),
					Type = table.Column<string>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_OpenIddictApplications", x => x.Id); });

			migrationBuilder.CreateTable(
				"OpenIddictScopes",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					ConcurrencyToken = table.Column<string>(nullable: true),
					Description = table.Column<string>(nullable: true),
					Name = table.Column<string>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_OpenIddictScopes", x => x.Id); });

			migrationBuilder.CreateTable(
				"AspNetRoleClaims",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					ClaimType = table.Column<string>(nullable: true),
					ClaimValue = table.Column<string>(nullable: true),
					RoleId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						"FK_AspNetRoleClaims_AspNetRoles_RoleId",
						x => x.RoleId,
						"AspNetRoles",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"AspNetUserClaims",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					ClaimType = table.Column<string>(nullable: true),
					ClaimValue = table.Column<string>(nullable: true),
					UserId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						"FK_AspNetUserClaims_AspNetUsers_UserId",
						x => x.UserId,
						"AspNetUsers",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"AspNetUserLogins",
				table => new
				{
					LoginProvider = table.Column<string>(nullable: false),
					ProviderKey = table.Column<string>(nullable: false),
					ProviderDisplayName = table.Column<string>(nullable: true),
					UserId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
					table.ForeignKey(
						"FK_AspNetUserLogins_AspNetUsers_UserId",
						x => x.UserId,
						"AspNetUsers",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"AspNetUserRoles",
				table => new
				{
					UserId = table.Column<string>(nullable: false),
					RoleId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
					table.ForeignKey(
						"FK_AspNetUserRoles_AspNetRoles_RoleId",
						x => x.RoleId,
						"AspNetRoles",
						"Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						"FK_AspNetUserRoles_AspNetUsers_UserId",
						x => x.UserId,
						"AspNetUsers",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"AspNetUserTokens",
				table => new
				{
					UserId = table.Column<string>(nullable: false),
					LoginProvider = table.Column<string>(nullable: false),
					Name = table.Column<string>(nullable: false),
					Value = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
					table.ForeignKey(
						"FK_AspNetUserTokens_AspNetUsers_UserId",
						x => x.UserId,
						"AspNetUsers",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"OpenIddictAuthorizations",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					ApplicationId = table.Column<string>(nullable: true),
					ConcurrencyToken = table.Column<string>(nullable: true),
					Scopes = table.Column<string>(nullable: true),
					Status = table.Column<string>(nullable: false),
					Subject = table.Column<string>(nullable: false),
					Type = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
					table.ForeignKey(
						"FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
						x => x.ApplicationId,
						"OpenIddictApplications",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				"OpenIddictTokens",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					ApplicationId = table.Column<string>(nullable: true),
					AuthorizationId = table.Column<string>(nullable: true),
					ConcurrencyToken = table.Column<string>(nullable: true),
					CreationDate = table.Column<DateTimeOffset>(nullable: true),
					ExpirationDate = table.Column<DateTimeOffset>(nullable: true),
					Payload = table.Column<string>(nullable: true),
					ReferenceId = table.Column<string>(nullable: true),
					Status = table.Column<string>(nullable: true),
					Subject = table.Column<string>(nullable: false),
					Type = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
					table.ForeignKey(
						"FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
						x => x.ApplicationId,
						"OpenIddictApplications",
						"Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						"FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
						x => x.AuthorizationId,
						"OpenIddictAuthorizations",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				"IX_AspNetRoleClaims_RoleId",
				"AspNetRoleClaims",
				"RoleId");

			migrationBuilder.CreateIndex(
				"RoleNameIndex",
				"AspNetRoles",
				"NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				"IX_AspNetUserClaims_UserId",
				"AspNetUserClaims",
				"UserId");

			migrationBuilder.CreateIndex(
				"IX_AspNetUserLogins_UserId",
				"AspNetUserLogins",
				"UserId");

			migrationBuilder.CreateIndex(
				"IX_AspNetUserRoles_RoleId",
				"AspNetUserRoles",
				"RoleId");

			migrationBuilder.CreateIndex(
				"EmailIndex",
				"AspNetUsers",
				"NormalizedEmail");

			migrationBuilder.CreateIndex(
				"UserNameIndex",
				"AspNetUsers",
				"NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				"IX_OpenIddictApplications_ClientId",
				"OpenIddictApplications",
				"ClientId",
				unique: true);

			migrationBuilder.CreateIndex(
				"IX_OpenIddictAuthorizations_ApplicationId",
				"OpenIddictAuthorizations",
				"ApplicationId");

			migrationBuilder.CreateIndex(
				"IX_OpenIddictTokens_ApplicationId",
				"OpenIddictTokens",
				"ApplicationId");

			migrationBuilder.CreateIndex(
				"IX_OpenIddictTokens_AuthorizationId",
				"OpenIddictTokens",
				"AuthorizationId");

			migrationBuilder.CreateIndex(
				"IX_OpenIddictTokens_ReferenceId",
				"OpenIddictTokens",
				"ReferenceId",
				unique: true,
				filter: "[ReferenceId] IS NOT NULL");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"AspNetRoleClaims");

			migrationBuilder.DropTable(
				"AspNetUserClaims");

			migrationBuilder.DropTable(
				"AspNetUserLogins");

			migrationBuilder.DropTable(
				"AspNetUserRoles");

			migrationBuilder.DropTable(
				"AspNetUserTokens");

			migrationBuilder.DropTable(
				"OpenIddictScopes");

			migrationBuilder.DropTable(
				"OpenIddictTokens");

			migrationBuilder.DropTable(
				"AspNetRoles");

			migrationBuilder.DropTable(
				"AspNetUsers");

			migrationBuilder.DropTable(
				"OpenIddictAuthorizations");

			migrationBuilder.DropTable(
				"OpenIddictApplications");
		}
	}
}