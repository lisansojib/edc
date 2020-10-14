using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class CohortTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cohort",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "CohortId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cohorts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cohorts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CohortId",
                table: "Events",
                column: "CohortId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Cohort",
                table: "Events",
                column: "CohortId",
                principalTable: "Cohorts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Cohort",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Cohorts");

            migrationBuilder.DropIndex(
                name: "IX_Events_CohortId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CohortId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Cohort",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "");
        }
    }
}
