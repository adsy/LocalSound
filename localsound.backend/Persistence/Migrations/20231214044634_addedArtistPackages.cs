using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedArtistPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistPackage",
                columns: table => new
                {
                    ArtistPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackagePrice = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPackage", x => x.ArtistPackageId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistPackage_Artist_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Artist",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistPackageEquipment",
                columns: table => new
                {
                    ArtistPackageEquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPackageEquipment", x => x.ArtistPackageEquipmentId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistPackageEquipment_ArtistPackage_ArtistPackageId",
                        column: x => x.ArtistPackageId,
                        principalTable: "ArtistPackage",
                        principalColumn: "ArtistPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistPackagePhoto",
                columns: table => new
                {
                    ArtistPackagePhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPackagePhoto", x => x.ArtistPackagePhotoId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistPackagePhoto_ArtistPackage_ArtistPackageId",
                        column: x => x.ArtistPackageId,
                        principalTable: "ArtistPackage",
                        principalColumn: "ArtistPackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistPackagePhoto_FileContent_FileContentId",
                        column: x => x.FileContentId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackage_AppUserId",
                table: "ArtistPackage",
                column: "AppUserId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackageEquipment_ArtistPackageId",
                table: "ArtistPackageEquipment",
                column: "ArtistPackageId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackagePhoto_ArtistPackageId",
                table: "ArtistPackagePhoto",
                column: "ArtistPackageId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistPackagePhoto_FileContentId",
                table: "ArtistPackagePhoto",
                column: "FileContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistPackageEquipment");

            migrationBuilder.DropTable(
                name: "ArtistPackagePhoto");

            migrationBuilder.DropTable(
                name: "ArtistPackage");
        }
    }
}
