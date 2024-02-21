using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedToBeDeletedForTrackAndTrackImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackGenre_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackImageFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_SongLike_ArtistTrackUpload_ArtistTrackId",
                table: "SongLike");

            migrationBuilder.DropTable(
                name: "ArtistTrackUpload");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent",
                newName: "ArtistTrackImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistTrackImageFileContent_ArtistTrackUploadId",
                table: "ArtistTrackImageFileContent",
                newName: "IX_ArtistTrackImageFileContent_ArtistTrackImageId");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackUploadId",
                table: "ArtistTrackGenre",
                newName: "ArtistTrackId");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent",
                newName: "ArtistTrackId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistTrackAudioFileContent_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent",
                newName: "IX_ArtistTrackAudioFileContent_ArtistTrackId");

            migrationBuilder.CreateTable(
                name: "ArtistTrack",
                columns: table => new
                {
                    ArtistTrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistMemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ToBeDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrack", x => x.ArtistTrackId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistTrack_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrackImage",
                columns: table => new
                {
                    ArtistTrackImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistTrackId = table.Column<int>(type: "int", nullable: false),
                    TrackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToBeDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackImage", x => x.ArtistTrackImageId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistTrackImage_ArtistTrack_ArtistTrackId",
                        column: x => x.ArtistTrackId,
                        principalTable: "ArtistTrack",
                        principalColumn: "ArtistTrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_AppUserId",
                table: "ArtistTrack",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_ArtistMemberId",
                table: "ArtistTrack",
                column: "ArtistMemberId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackImage_ArtistTrackId",
                table: "ArtistTrackImage",
                column: "ArtistTrackId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrack_ArtistTrackId",
                table: "ArtistTrackAudioFileContent",
                column: "ArtistTrackId",
                principalTable: "ArtistTrack",
                principalColumn: "ArtistTrackId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackGenre_ArtistTrack_ArtistTrackId",
                table: "ArtistTrackGenre",
                column: "ArtistTrackId",
                principalTable: "ArtistTrack",
                principalColumn: "ArtistTrackId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackImageFileContent_ArtistTrackImage_ArtistTrackImageId",
                table: "ArtistTrackImageFileContent",
                column: "ArtistTrackImageId",
                principalTable: "ArtistTrackImage",
                principalColumn: "ArtistTrackImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongLike_ArtistTrack_ArtistTrackId",
                table: "SongLike",
                column: "ArtistTrackId",
                principalTable: "ArtistTrack",
                principalColumn: "ArtistTrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrack_ArtistTrackId",
                table: "ArtistTrackAudioFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackGenre_ArtistTrack_ArtistTrackId",
                table: "ArtistTrackGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackImageFileContent_ArtistTrackImage_ArtistTrackImageId",
                table: "ArtistTrackImageFileContent");

            migrationBuilder.DropForeignKey(
                name: "FK_SongLike_ArtistTrack_ArtistTrackId",
                table: "SongLike");

            migrationBuilder.DropTable(
                name: "ArtistTrackImage");

            migrationBuilder.DropTable(
                name: "ArtistTrack");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackImageId",
                table: "ArtistTrackImageFileContent",
                newName: "ArtistTrackUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistTrackImageFileContent_ArtistTrackImageId",
                table: "ArtistTrackImageFileContent",
                newName: "IX_ArtistTrackImageFileContent_ArtistTrackUploadId");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackId",
                table: "ArtistTrackGenre",
                newName: "ArtistTrackUploadId");

            migrationBuilder.RenameColumn(
                name: "ArtistTrackId",
                table: "ArtistTrackAudioFileContent",
                newName: "ArtistTrackUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtistTrackAudioFileContent_ArtistTrackId",
                table: "ArtistTrackAudioFileContent",
                newName: "IX_ArtistTrackAudioFileContent_ArtistTrackUploadId");

            migrationBuilder.CreateTable(
                name: "ArtistTrackUpload",
                columns: table => new
                {
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistMemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    FileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TrackDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackUpload", x => x.ArtistTrackUploadId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_ArtistMemberId",
                table: "ArtistTrackUpload",
                column: "ArtistMemberId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackAudioFileContent_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackAudioFileContent",
                column: "ArtistTrackUploadId",
                principalTable: "ArtistTrackUpload",
                principalColumn: "ArtistTrackUploadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackGenre_ArtistTrackUpload_ArtistTrackUploadId",
                table: "ArtistTrackGenre",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SongLike_ArtistTrackUpload_ArtistTrackId",
                table: "SongLike",
                column: "ArtistTrackId",
                principalTable: "ArtistTrackUpload",
                principalColumn: "ArtistTrackUploadId");
        }
    }
}
