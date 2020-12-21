using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DbChanges4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkedInUrl",
                table: "Speakers",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkedInUrl",
                table: "Speakers",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);
        }
    }
}
