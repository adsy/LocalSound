using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedBookingIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistBooking",
                table: "ArtistBooking");

            migrationBuilder.DropIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistBooking",
                table: "ArtistBooking",
                column: "BookingId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistBooking",
                table: "ArtistBooking");

            migrationBuilder.DropIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistBooking",
                table: "ArtistBooking",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId");
        }
    }
}
