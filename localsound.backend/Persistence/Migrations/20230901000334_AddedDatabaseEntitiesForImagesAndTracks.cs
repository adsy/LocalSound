using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDatabaseEntitiesForImagesAndTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountImageType",
                columns: table => new
                {
                    AccountImageTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountImageTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountImageType", x => x.AccountImageTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateTable(
                name: "AccountImage",
                columns: table => new
                {
                    AccountImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountImageTypeId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountImage", x => x.AccountImageId);
                    table.ForeignKey(
                        name: "FK_AccountImage_AccountImageType_AccountImageTypeId",
                        column: x => x.AccountImageTypeId,
                        principalTable: "AccountImageType",
                        principalColumn: "AccountImageTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountImage_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountImage_FileContent_FileContentId",
                        column: x => x.FileContentId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrackUpload",
                columns: table => new
                {
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackUpload", x => x.ArtistTrackUploadId);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_Artist_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Artist",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_FileContent_FileContentId",
                        column: x => x.FileContentId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_AccountImageTypeId",
                table: "AccountImage",
                column: "AccountImageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_AppUserId",
                table: "AccountImage",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_FileContentId",
                table: "ArtistTrackUpload",
                column: "FileContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountImage");

            migrationBuilder.DropTable(
                name: "ArtistTrackUpload");

            migrationBuilder.DropTable(
                name: "AccountImageType");

            migrationBuilder.DropTable(
                name: "FileContent");
        }
    }
}
