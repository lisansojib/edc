using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class UpdatedTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UUId",
                table: "Participants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UUId",
                table: "Admins",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UUId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UUId",
                table: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "TeamUId",
                table: "Teams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Participants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
