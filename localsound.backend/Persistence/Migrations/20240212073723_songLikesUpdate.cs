using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class songLikesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SongLikeId");

            migrationBuilder.AddColumn<int>(
                name: "SongLikeId",
                table: "SongLike",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SongLikeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongLikeId",
                table: "SongLike");

            migrationBuilder.DropSequence(
                name: "SongLikeId");
        }
    }
}
