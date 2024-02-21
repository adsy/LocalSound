using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class splitFileContentTableToReduceCycles3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackAudioFileContent_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackImageFileContent_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackImageFileContent_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent",
                column: "ArtistTrackUploadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackAudioFileContent_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent",
                column: "ArtistTrackUploadId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent",
                column: "ArtistTrackUploadId",
                principalTable: "ArtistTrackUpload",
                principalColumn: "ArtistTrackUploadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackImageFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent",
                column: "ArtistTrackUploadId",
                principalTable: "ArtistTrackUpload",
                principalColumn: "ArtistTrackUploadId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackImageFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackImageFileContent_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackAudioFileContent_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent");

            migrationBuilder.AddColumn<Guid>(
                name: "TrackDataId",
                table: "ArtistTrackUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TrackImageId",
                table: "ArtistTrackUpload",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackDataId",
                table: "ArtistTrackUpload",
                column: "TrackDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackAudioFileContent_TrackDataId",
                table: "ArtistTrackUpload",
                column: "TrackDataId",
                principalTable: "ArtistTrackAudioFileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackImageFileContent_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId",
                principalTable: "ArtistTrackImageFileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
