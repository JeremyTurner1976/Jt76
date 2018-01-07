namespace Jt76.WebApi.Migrations.ApplicationDb
{
	using System;
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class InitialApplicationMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Errors",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					AdditionalInformation = table.Column<string>(nullable: true),
					CreatedBy = table.Column<string>(nullable: true),
					CreatedDate = table.Column<DateTime>(nullable: false),
					ErrorLevel = table.Column<string>(nullable: true),
					Message = table.Column<string>(nullable: true),
					Source = table.Column<string>(nullable: true),
					StackTrace = table.Column<string>(nullable: true),
					UpdatedBy = table.Column<string>(nullable: true),
					UpdatedDate = table.Column<DateTime>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_Errors", x => x.Id); });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"Errors");
		}
	}
}