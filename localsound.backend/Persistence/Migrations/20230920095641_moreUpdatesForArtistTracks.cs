using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class moreUpdatesForArtistTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload");

            migrationBuilder.RenameColumn(
                name: "FileContentId",
                table: "ArtistTrackUpload",
                newName: "TrackDataId");

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId",
                table: "ArtistTrackUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TrackDescription",
                table: "ArtistTrackUpload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TrackImageId",
                table: "ArtistTrackUpload",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload",
                column: "ArtistTrackUploadId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_GenreId",
                table: "ArtistTrackUpload",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackDataId",
                table: "ArtistTrackUpload",
                column: "TrackDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackDataId",
                table: "ArtistTrackUpload",
                column: "TrackDataId",
                principalTable: "FileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId",
                principalTable: "FileContent",
                principalColumn: "FileContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_Genres_GenreId",
                table: "ArtistTrackUpload",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_Genres_GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "TrackDescription",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.RenameColumn(
                name: "TrackDataId",
                table: "ArtistTrackUpload",
                newName: "FileContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistTrackUpload",
                table: "ArtistTrackUpload",
                column: "ArtistTrackUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
