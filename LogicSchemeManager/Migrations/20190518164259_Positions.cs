using Microsoft.EntityFrameworkCore.Migrations;

namespace LogicSchemeManager.Migrations
{
    public partial class Positions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "x",
                table: "Elements",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "y",
                table: "Elements",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "x",
                table: "ElementPorts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "y",
                table: "ElementPorts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "x",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "y",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "x",
                table: "ElementPorts");

            migrationBuilder.DropColumn(
                name: "y",
                table: "ElementPorts");
        }
    }
}
