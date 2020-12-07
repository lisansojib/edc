using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class ZoomMeetingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZoomMeetings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUId = table.Column<string>(type: "varchar(128)", nullable: false),
                    HostId = table.Column<string>(type: "varchar(128)", nullable: false),
                    Topic = table.Column<string>(nullable: true),
                    Agenda = table.Column<string>(maxLength: 2000, nullable: false),
                    Type = table.Column<int>(nullable: false, comment: "1-Instant meeting, 2-Schedule meeting, 3-Recurring meeting with no fixed time, 4-Recurring meeting with fixed time"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    Timezone = table.Column<string>(type: "varchar(50)", nullable: false),
                    JoinUrl = table.Column<string>(maxLength: 200, nullable: false),
                    PMI = table.Column<string>(type: "varchar(128)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoomMeetings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZoomMeetings");
        }
    }
}
