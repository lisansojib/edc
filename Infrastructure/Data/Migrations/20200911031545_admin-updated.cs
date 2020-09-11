using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class adminupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins");

            migrationBuilder.DropTable(
                name: "Managements");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Participants",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 500, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.UniqueConstraint("UniqueKey_Email", x => x.Email);
                    table.UniqueConstraint("UniqueKey_Username", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Management_AdminLevel",
                        column: x => x.AdminLevelId,
                        principalTable: "ValueFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminLevelId",
                table: "Admins",
                column: "AdminLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins",
                column: "UserId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Participants");

            migrationBuilder.CreateTable(
                name: "Managements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivationCode = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Active = table.Column<short>(type: "bit", nullable: false),
                    AdminLevelId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateSuspended = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Mobile = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Username = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Verified = table.Column<short>(type: "bit", nullable: false)
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
                name: "IX_Managements_AdminLevelId",
                table: "Managements",
                column: "AdminLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalLogin_Management",
                table: "ExternalLogins",
                column: "UserId",
                principalTable: "Managements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
