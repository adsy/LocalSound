using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removedIntColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "ArtistTrackUploadId",
                table: "ArtistTrackUpload");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload",
                column: "UploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload");

            migrationBuilder.AddColumn<int>(
                name: "ArtistTrackUploadId",
                table: "ArtistTrackUpload",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload",
                column: "ArtistTrackUploadId");
        }
    }
}
