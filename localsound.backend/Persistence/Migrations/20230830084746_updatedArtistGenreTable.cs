using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedArtistGenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenre_Artist_ArtistAppUserId",
                table: "ArtistGenre");

            migrationBuilder.DropIndex(
                name: "IX_ArtistGenre_ArtistAppUserId",
                table: "ArtistGenre");

            migrationBuilder.DropColumn(
                name: "ArtistAppUserId",
                table: "ArtistGenre");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenre_Artist_AppUserId",
                table: "ArtistGenre",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenre_Artist_AppUserId",
                table: "ArtistGenre");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistAppUserId",
                table: "ArtistGenre",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ArtistGenre_ArtistAppUserId",
                table: "ArtistGenre",
                column: "ArtistAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenre_Artist_ArtistAppUserId",
                table: "ArtistGenre",
                column: "ArtistAppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
