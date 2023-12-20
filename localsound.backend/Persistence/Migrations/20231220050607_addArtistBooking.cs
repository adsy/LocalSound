using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addArtistBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistBooking",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingConfirmed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistBooking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_ArtistBooking_ArtistPackage_PackageId",
                        column: x => x.PackageId,
                        principalTable: "ArtistPackage",
                        principalColumn: "ArtistPackageId");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "AppUserId");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_AspNetUsers_BookerId",
                        column: x => x.BookerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventType",
                        principalColumn: "EventTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_BookerId",
                table: "ArtistBooking",
                column: "BookerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_EventTypeId",
                table: "ArtistBooking",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_PackageId",
                table: "ArtistBooking",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistBooking");
        }
    }
}
