using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedTrackTableIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrackLikeCount");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.AddColumn<string>(
                name: "ArtistMemberId",
                table: "ArtistTrackUpload",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ArtistTrackUpload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_ArtistMemberId",
                table: "ArtistTrackUpload",
                column: "ArtistMemberId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrackUpload_ArtistMemberId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "ArtistMemberId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ArtistTrackUpload");

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

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId")
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
