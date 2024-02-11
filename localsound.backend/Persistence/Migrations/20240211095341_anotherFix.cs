using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class anotherFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistTrackLikeCount",
                columns: table => new
                {
                    ArtistTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackLikeCount", x => x.ArtistTrackId);
                    table.ForeignKey(
                        name: "FK_ArtistTrackLikeCount_ArtistTrackUpload_ArtistTrackId",
                        column: x => x.ArtistTrackId,
                        principalTable: "ArtistTrackUpload",
                        principalColumn: "ArtistTrackUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SongLike",
                columns: table => new
                {
                    ArtistTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongLike", x => x.ArtistTrackId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_SongLike_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongLike_ArtistTrackUpload_ArtistTrackId",
                        column: x => x.ArtistTrackId,
                        principalTable: "ArtistTrackUpload",
                        principalColumn: "ArtistTrackUploadId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_AppUserId",
                table: "SongLike",
                column: "AppUserId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrackLikeCount");

            migrationBuilder.DropTable(
                name: "SongLike");
        }
    }
}
