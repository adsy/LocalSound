using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TryingToGetImageDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_FileContent_FileContentId",
                table: "AccountImage");

            migrationBuilder.DropIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage");

            migrationBuilder.AddColumn<int>(
                name: "ImageAccountImageId",
                table: "FileContent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileContent_ImageAccountImageId",
                table: "FileContent",
                column: "ImageAccountImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileContent_AccountImage_ImageAccountImageId",
                table: "FileContent",
                column: "ImageAccountImageId",
                principalTable: "AccountImage",
                principalColumn: "AccountImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileContent_AccountImage_ImageAccountImageId",
                table: "FileContent");

            migrationBuilder.DropIndex(
                name: "IX_FileContent_ImageAccountImageId",
                table: "FileContent");

            migrationBuilder.DropColumn(
                name: "ImageAccountImageId",
                table: "FileContent");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_FileContent_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
