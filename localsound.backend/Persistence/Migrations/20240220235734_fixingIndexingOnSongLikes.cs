using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixingIndexingOnSongLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SongLike_MemberId",
                table: "SongLike");

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_MemberId_SongLikeId",
                table: "SongLike",
                columns: new[] { "MemberId", "SongLikeId" })
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SongLike_MemberId_SongLikeId",
                table: "SongLike");

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_MemberId",
                table: "SongLike",
                column: "MemberId")
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
