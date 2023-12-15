using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistPackageEquipment",
                table: "ArtistPackageEquipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistPackageEquipment",
                table: "ArtistPackageEquipment",
                columns: new[] { "ArtistPackageId", "ArtistPackageEquipmentId" })
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistPackageEquipment",
                table: "ArtistPackageEquipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistPackageEquipment",
                table: "ArtistPackageEquipment",
                column: "ArtistPackageEquipmentId")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
