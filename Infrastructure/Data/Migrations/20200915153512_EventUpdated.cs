using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class EventUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UniqueKey_SponsorName",
                table: "Sponsors");

            migrationBuilder.DropUniqueConstraint(
                name: "UniqueKey_SpeakerName",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Speakers");

            migrationBuilder.AddColumn<int>(
                name: "SponsorId",
                table: "Sponsors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpeakerId",
                table: "Speakers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_SponsorId",
                table: "Sponsors",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_SpeakerId",
                table: "Speakers",
                column: "SpeakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speaker_ValueField",
                table: "Speakers",
                column: "SpeakerId",
                principalTable: "ValueFields",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_ValueField",
                table: "Speakers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsor_ValueField",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_SponsorId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_SpeakerId",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "SpeakerId",
                table: "Speakers");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sponsors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Speakers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "UniqueKey_SponsorName",
                table: "Sponsors",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "UniqueKey_SpeakerName",
                table: "Speakers",
                column: "Name");
        }
    }
}
