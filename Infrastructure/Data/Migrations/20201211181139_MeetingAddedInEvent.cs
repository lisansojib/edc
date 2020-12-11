using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class MeetingAddedInEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ZoomMeetings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "Events",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingPassword",
                table: "Events",
                type: "varchar(20)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "ZoomMeetings");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MeetingPassword",
                table: "Events");
        }
    }
}
