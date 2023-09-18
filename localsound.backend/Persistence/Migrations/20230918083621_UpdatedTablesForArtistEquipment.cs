using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTablesForArtistEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEquipment_Equipment_EquipmentId",
                table: "ArtistEquipment");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_ArtistEquipment_EquipmentId",
                table: "ArtistEquipment");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentName",
                table: "ArtistEquipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentName",
                table: "ArtistEquipment");

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistEquipment_EquipmentId",
                table: "ArtistEquipment",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEquipment_Equipment_EquipmentId",
                table: "ArtistEquipment",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "EquipmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
