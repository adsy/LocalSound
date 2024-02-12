using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class anotherUpdateForSongLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongLike",
                table: "SongLike");

            migrationBuilder.DropIndex(
                name: "IX_SongLike_ArtistTrackId",
                table: "SongLike");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongLike",
                table: "SongLike",
                column: "SongLikeId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_ArtistTrackId",
                table: "SongLike",
                column: "ArtistTrackId",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongLike",
                table: "SongLike");

            migrationBuilder.DropIndex(
                name: "IX_SongLike_ArtistTrackId",
                table: "SongLike");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongLike",
                table: "SongLike",
                column: "ArtistTrackId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_ArtistTrackId",
                table: "SongLike",
                column: "ArtistTrackId")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
