using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Userchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamUId",
                table: "Teams",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Participants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Admins",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamUId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Admins");
        }
    }
}
