using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class splitFileContentTableToReduceCycles4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackageImageFileContent_ArtistPackagePhoto_AristPackagePhotoId",
                table: "ArtistPackageImageFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackagePhoto_ArtistPackage_ArtistPackageId",
                table: "ArtistPackagePhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistPackagePhoto",
                table: "ArtistPackagePhoto");

            migrationBuilder.RenameTable(
                name: "ArtistPackagePhoto",
                newName: "ArtistPackageImage");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistPackagePhoto_ArtistPackageId",
                table: "ArtistPackageImage",
                newName: "IX_ArtistPackageImage_ArtistPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistPackageImage",
                table: "ArtistPackageImage",
                column: "ArtistPackageImageId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackageImage_ArtistPackage_ArtistPackageId",
                table: "ArtistPackageImage",
                column: "ArtistPackageId",
                principalTable: "ArtistPackage",
                principalColumn: "ArtistPackageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackageImageFileContent_ArtistPackageImage_AristPackagePhotoId",
                table: "ArtistPackageImageFileContent",
                column: "AristPackagePhotoId",
                principalTable: "ArtistPackageImage",
                principalColumn: "ArtistPackageImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackageImage_ArtistPackage_ArtistPackageId",
                table: "ArtistPackageImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackageImageFileContent_ArtistPackageImage_AristPackagePhotoId",
                table: "ArtistPackageImageFileContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistPackageImage",
                table: "ArtistPackageImage");

            migrationBuilder.RenameTable(
                name: "ArtistPackageImage",
                newName: "ArtistPackagePhoto");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistPackageImage_ArtistPackageId",
                table: "ArtistPackagePhoto",
                newName: "IX_ArtistPackagePhoto_ArtistPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistPackagePhoto",
                table: "ArtistPackagePhoto",
                column: "ArtistPackageImageId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackageImageFileContent_ArtistPackagePhoto_AristPackagePhotoId",
                table: "ArtistPackageImageFileContent",
                column: "AristPackagePhotoId",
                principalTable: "ArtistPackagePhoto",
                principalColumn: "ArtistPackageImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackagePhoto_ArtistPackage_ArtistPackageId",
                table: "ArtistPackagePhoto",
                column: "ArtistPackageId",
                principalTable: "ArtistPackage",
                principalColumn: "ArtistPackageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
