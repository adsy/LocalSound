using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedUniqueIndexOnArtistAndNonArtistTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfileUrl",
                table: "NonArtist",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileUrl",
                table: "Artist",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_NonArtist_ProfileUrl",
                table: "NonArtist",
                column: "ProfileUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artist_ProfileUrl",
                table: "Artist",
                column: "ProfileUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NonArtist_ProfileUrl",
                table: "NonArtist");

            migrationBuilder.DropIndex(
                name: "IX_Artist_ProfileUrl",
                table: "Artist");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileUrl",
                table: "NonArtist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileUrl",
                table: "Artist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
