using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class TablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    CallAction = table.Column<string>(maxLength: 100, nullable: false),
                    LinkUrl = table.Column<string>(maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 250, nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GraphType = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Panel = table.Column<string>(maxLength: 200, nullable: false),
                    Origin = table.Column<string>(maxLength: 200, nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.UniqueConstraint("UniqueKey_TeamName", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "ValueFieldTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueFieldTypes", x => x.Id);
                    table.UniqueConstraint("UniqueKey_ValueFieldTypeName", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 500, nullable: false),
                    Email = table.Column<string>(maxLength: 500, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    ActivationCode = table.Column<string>(maxLength: 128, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    PhotoUrl = table.Column<string>(maxLength: 128, nullable: true),
                    DateSuspended = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmailCorp = table.Column<string>(maxLength: 500, nullable: true),
                    PhoneCorp = table.Column<string>(maxLength: 20, nullable: true),
                    LinkedinUrl = table.Column<string>(maxLength: 250, nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.UniqueConstraint("UniqueKey_Email", x => x.Email);
                    table.UniqueConstraint("UniqueKey_Username", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Paticipant_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                    table.UniqueConstraint("UniqueKey_SpeakerName", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Speaker_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsors", x => x.Id);
                    table.UniqueConstraint("UniqueKey_SponsorName", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Sponsor_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PollId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoints", x => x.Id);
                    table.UniqueConstraint("UniqueKey_DataPointName", x => x.Name);
                    table.ForeignKey(
                        name: "FK_DataPoint_Poll",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueField_ValueFieldType",
                        column: x => x.TypeId,
                        principalTable: "ValueFieldTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipantTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TeamMemberId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantTeam_Team",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipantTeam_Participant",
                        column: x => x.TeamMemberId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Managements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 500, nullable: false),
                    Email = table.Column<string>(maxLength: 500, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    ActivationCode = table.Column<string>(maxLength: 128, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    PhotoUrl = table.Column<string>(maxLength: 128, nullable: true),
                    DateSuspended = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    AdminLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managements", x => x.Id);
                    table.UniqueConstraint("UniqueKey_Email", x => x.Email);
                    table.UniqueConstraint("UniqueKey_Username", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Management_AdminLevel",
                        column: x => x.AdminLevelId,
                        principalTable: "ValueFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_PollId",
                table: "DataPoints",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_Managements_AdminLevelId",
                table: "Managements",
                column: "AdminLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CompanyId",
                table: "Participants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantTeams_TeamId",
                table: "ParticipantTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantTeams_TeamMemberId",
                table: "ParticipantTeams",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_EventId",
                table: "Speakers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_EventId",
                table: "Sponsors",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueFields_TypeId",
                table: "ValueFields",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins",
                column: "UserId",
                principalTable: "Managements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalLogin_Participant",
                table: "ExternalLogins",
                column: "UserId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalLogin_Participant",
                table: "ExternalLogins");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "DataPoints");

            migrationBuilder.DropTable(
                name: "Managements");

            migrationBuilder.DropTable(
                name: "ParticipantTeams");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "ValueFields");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ValueFieldTypes");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivationCode = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Active = table.Column<short>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Username = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Verified = table.Column<short>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("UniqueKey_Email", x => x.Email);
                    table.UniqueConstraint("UniqueKey_Username", x => x.Username);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_User",
                table: "ExternalLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
