using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountOnboarding");

            migrationBuilder.CreateTable(
                name: "AccountMessages",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountSetupCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountMessages", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_AccountMessages_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountMessages");

            migrationBuilder.CreateTable(
                name: "AccountOnboarding",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountSetupCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOnboarding", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_AccountOnboarding_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
