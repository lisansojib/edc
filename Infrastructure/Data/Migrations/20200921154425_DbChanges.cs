using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class DbChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_Event",
                table: "Speakers");

            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_ValueField",
                table: "Speakers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsor_Event",
                table: "Sponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsor_ValueField",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_EventId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_SponsorId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_EventId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_SpeakerId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "SpeakerId",
                table: "Speakers");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Sponsors",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Sponsors",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonEmail",
                table: "Sponsors",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonPhone",
                table: "Sponsors",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sponsors",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Sponsors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sponsors",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Sponsors",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sponsors",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Sponsors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Sponsors",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Speakers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Speakers",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Speakers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Speakers",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Speakers",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Speakers",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Speakers",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Speakers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Companies",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Companies",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventSpeakers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    SpeakerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSpeakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSpeaker_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSpeaker_ValueField",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSponsors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    SponsorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSponsor_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSponsor_Sponsor",
                        column: x => x.SponsorId,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeakers_EventId",
                table: "EventSpeakers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeakers_SpeakerId",
                table: "EventSpeakers",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsors_EventId",
                table: "EventSponsors",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsors_SponsorId",
                table: "EventSponsors",
                column: "SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers");

            migrationBuilder.DropTable(
                name: "EventSpeakers");

            migrationBuilder.DropTable(
                name: "EventSponsors");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ContactPersonEmail",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ContactPersonPhone",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Sponsors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SponsorId",
                table: "Sponsors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Speakers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpeakerId",
                table: "Speakers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_EventId",
                table: "Sponsors",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_SponsorId",
                table: "Sponsors",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_EventId",
                table: "Speakers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_SpeakerId",
                table: "Speakers",
                column: "SpeakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speaker_Event",
                table: "Speakers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Speaker_ValueField",
                table: "Speakers",
                column: "SpeakerId",
                principalTable: "ValueFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsor_Event",
                table: "Sponsors",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsor_ValueField",
                table: "Sponsors",
                column: "SponsorId",
                principalTable: "ValueFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
