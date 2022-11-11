using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.API.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Connections",
                newName: "ConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConnectionId",
                table: "Connections",
                newName: "Id");
        }
    }
}
