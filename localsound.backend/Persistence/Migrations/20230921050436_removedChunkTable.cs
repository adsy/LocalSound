using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removedChunkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistTrackChunk");

            migrationBuilder.DropColumn(
                name: "TrackReady",
                table: "ArtistTrackUpload");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrackReady",
                table: "ArtistTrackUpload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ArtistTrackChunk",
                columns: table => new
                {
                    ChunkId = table.Column<int>(type: "int", nullable: false),
                    PartialTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackChunk", x => new { x.ChunkId, x.PartialTrackId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistTrackChunk_FileContent_FileContentId",
                        column: x => x.FileContentId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackChunk_FileContentId",
                table: "ArtistTrackChunk",
                column: "FileContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackChunk_PartialTrackId",
                table: "ArtistTrackChunk",
                column: "PartialTrackId")
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
