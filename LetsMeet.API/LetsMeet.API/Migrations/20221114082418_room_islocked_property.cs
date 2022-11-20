using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.API.Migrations
{
    public partial class room_islocked_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLocked",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLocked",
                table: "Rooms");
        }
    }
}
