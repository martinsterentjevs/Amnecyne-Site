using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnecyne.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingIconPathColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Projects",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Projects");
        }
    }
}
