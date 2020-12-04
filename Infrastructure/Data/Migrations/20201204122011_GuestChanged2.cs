using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class GuestChanged2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guest_GuestType",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Paticipant_Company",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_Company",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Participants_CompanyId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Guests_GuestTypeId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "GuestTypeId",
                table: "Guests");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Speakers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Participants",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Guests",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                comment: "Guest,Member,Speaker,Sponsor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Guests");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Speakers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Participants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuestTypeId",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CompanyId",
                table: "Participants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_GuestTypeId",
                table: "Guests",
                column: "GuestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_GuestType",
                table: "Guests",
                column: "GuestTypeId",
                principalTable: "ValueFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paticipant_Company",
                table: "Participants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Speaker_Company",
                table: "Speakers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
