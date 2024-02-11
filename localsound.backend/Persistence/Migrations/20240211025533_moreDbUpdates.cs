using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class moreDbUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountSetupCompleted",
                table: "AccountMessages");

            migrationBuilder.AddColumn<bool>(
                name: "OnboardingMessageClosed",
                table: "AccountMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnboardingMessageClosed",
                table: "AccountMessages");

            migrationBuilder.AddColumn<bool>(
                name: "AccountSetupCompleted",
                table: "AccountMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
