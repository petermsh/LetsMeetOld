using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.Infrastructure.EF.SqlServer.Migrations
{
    public partial class Addroomentitystatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Rooms");
        }
    }
}
