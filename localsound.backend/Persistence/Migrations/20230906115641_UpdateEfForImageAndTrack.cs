using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEfForImageAndTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId");
        }
    }
}
