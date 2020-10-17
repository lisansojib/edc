using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class PendingSpeakerUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PendingSepakers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PendingSepakers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsReferrer",
                table: "PendingSepakers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PendingSepakers",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PendingSepakers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PendingSepakers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PendingSepakers");

            migrationBuilder.DropColumn(
                name: "IsReferrer",
                table: "PendingSepakers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PendingSepakers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PendingSepakers");
        }
    }
}
