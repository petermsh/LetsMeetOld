using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.API.Migrations
{
    public partial class user_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FieldOfStudies",
                table: "Users",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Users",
                newName: "Bio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Major",
                table: "Users",
                newName: "FieldOfStudies");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Users",
                newName: "Description");
        }
    }
}
