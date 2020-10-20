using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class PendingSpeakerUpdate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers");

            migrationBuilder.DropTable(
                name: "PendingSepakers");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Speakers",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Speakers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Speakers",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Speakers",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Speakers",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PendingSpeakers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(100)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(500)", nullable: false),
                    InterestInTopic = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    ReferredBy = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "varchar(500)", nullable: false),
                    IsReferrer = table.Column<bool>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false),
                    AcceptedBy = table.Column<int>(nullable: false),
                    AcceptDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRejected = table.Column<bool>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    SpeakerId = table.Column<int>(nullable: false),
                    RejectDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingSpeakers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Speaker_Company",
                table: "Speakers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_Company",
                table: "Speakers");

            migrationBuilder.DropTable(
                name: "PendingSpeakers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Speakers");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Speakers",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Speakers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PendingSepakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(500)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    InterestInTopic = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    IsReferrer = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "varchar(500)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    ReferredBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingSepakers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
