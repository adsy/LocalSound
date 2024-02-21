using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class splitFileContentTableToReduceCycles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_FileContent_FileContentId",
                table: "AccountImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackagePhoto_FileContent_FileContentId",
                table: "ArtistPackagePhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_FileContent_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistPackagePhoto_FileContentId",
                table: "ArtistPackagePhoto");

            migrationBuilder.DropColumn(
                name: "FileContentId",
                table: "ArtistPackagePhoto");

            migrationBuilder.RenameColumn(
                name: "ArtistPackagePhotoId",
                table: "ArtistPackagePhoto",
                newName: "ArtistPackageImageId");

            migrationBuilder.CreateTable(
                name: "AccountImageFileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountImageId = table.Column<int>(type: "int", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountImageFileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateTable(
                name: "ArtistPackageImageFileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AristPackagePhotoId = table.Column<int>(type: "int", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPackageImageFileContent", x => x.FileContentId);
                    table.ForeignKey(
                        name: "FK_ArtistPackageImageFileContent_ArtistPackagePhoto_AristPackagePhotoId",
                        column: x => x.AristPackagePhotoId,
                        principalTable: "ArtistPackagePhoto",
                        principalColumn: "ArtistPackageImageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrackAudioFileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackAudioFileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrackImageFileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackImageFileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackageImageFileContent_AristPackagePhotoId",
                table: "ArtistPackageImageFileContent",
                column: "AristPackagePhotoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_AccountImageFileContent_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                principalTable: "AccountImageFileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_AccountImageFileContent_FileContentId",
                table: "AccountImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackAudioFileContent_TrackDataId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_ArtistTrackImageFileContent_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropTable(
                name: "AccountImageFileContent");

            migrationBuilder.DropTable(
                name: "ArtistPackageImageFileContent");

            migrationBuilder.DropTable(
                name: "ArtistTrackAudioFileContent");

            migrationBuilder.DropTable(
                name: "ArtistTrackImageFileContent");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload");

            migrationBuilder.RenameColumn(
                name: "ArtistPackageImageId",
                table: "ArtistPackagePhoto",
                newName: "ArtistPackagePhotoId");

            migrationBuilder.AddColumn<Guid>(
                name: "FileContentId",
                table: "ArtistPackagePhoto",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackagePhoto_FileContentId",
                table: "ArtistPackagePhoto",
                column: "FileContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_FileContent_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackagePhoto_FileContent_FileContentId",
                table: "ArtistPackagePhoto",
                column: "FileContentId",
                principalTable: "FileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);

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
        }
    }
}
