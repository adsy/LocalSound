using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFlagToCheckIfTrackReady : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrackReady",
                table: "ArtistTrackUpload",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackReady",
                table: "ArtistTrackUpload");
        }
    }
}
