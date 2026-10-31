using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnecyne.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectForRelease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChangelogEntries_ProjectId",
                table: "ChangelogEntries",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangelogEntries_Projects_ProjectId",
                table: "ChangelogEntries",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogEntries_Projects_ProjectId",
                table: "ChangelogEntries");

            migrationBuilder.DropIndex(
                name: "IX_ChangelogEntries_ProjectId",
                table: "ChangelogEntries");
        }
    }
}
