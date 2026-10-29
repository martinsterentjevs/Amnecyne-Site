using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnecyne.Migrations
{
    /// <inheritdoc />
    public partial class ProjectPostCrudMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectImage_Projects_ProjectId",
                table: "ProjectImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectImage",
                table: "ProjectImage");

            migrationBuilder.RenameTable(
                name: "ProjectImage",
                newName: "ProjectImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectImage_ProjectId",
                table: "ProjectImages",
                newName: "IX_ProjectImages_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectImages",
                table: "ProjectImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectImages_Projects_ProjectId",
                table: "ProjectImages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectImages_Projects_ProjectId",
                table: "ProjectImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectImages",
                table: "ProjectImages");

            migrationBuilder.RenameTable(
                name: "ProjectImages",
                newName: "ProjectImage");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectImages_ProjectId",
                table: "ProjectImage",
                newName: "IX_ProjectImage_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectImage",
                table: "ProjectImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectImage_Projects_ProjectId",
                table: "ProjectImage",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
