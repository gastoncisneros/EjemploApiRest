using Microsoft.EntityFrameworkCore.Migrations;

namespace EjemploApiRest.WebApi.Migrations
{
    public partial class FootballTeamAddManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Teams");
        }
    }
}
