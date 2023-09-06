using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixForMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId",
                unique: true);
        }
    }
}
