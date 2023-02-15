using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkysFormsDemo.Migrations
{
    public partial class AddedPlayerPos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Person");
        }
    }
}
