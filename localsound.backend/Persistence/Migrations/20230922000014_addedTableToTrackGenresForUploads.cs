using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedTableToTrackGenresForUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_Genres_GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.AddColumn<string>(
                name: "TrackImageUrl",
                table: "ArtistTrackUpload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ArtistTrackGenre",
                columns: table => new
                {
                    ArtistTrackUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackGenre", x => new { x.ArtistTrackUploadId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ArtistTrackGenre_ArtistTrackUpload_ArtistTrackUploadId",
                        column: x => x.ArtistTrackUploadId,
                        principalTable: "ArtistTrackUpload",
                        principalColumn: "ArtistTrackUploadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrackGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackGenre_GenreId",
                table: "ArtistTrackGenre",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrackGenre");

            migrationBuilder.DropColumn(
                name: "TrackImageUrl",
                table: "ArtistTrackUpload");

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId",
                table: "ArtistTrackUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_GenreId",
                table: "ArtistTrackUpload",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_Genres_GenreId",
                table: "ArtistTrackUpload",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
