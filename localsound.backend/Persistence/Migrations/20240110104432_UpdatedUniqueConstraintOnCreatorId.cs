using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUniqueConstraintOnCreatorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId",
                unique: true);
        }
    }
}
