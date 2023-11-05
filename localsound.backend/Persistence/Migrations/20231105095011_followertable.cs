using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class followertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistFollower",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistFollower", x => new { x.ArtistId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_ArtistFollower_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArtistFollower_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistFollower_FollowerId",
                table: "ArtistFollower",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistFollower");
        }
    }
}
