using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedNotificationColumnsSpelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreaterId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "NotificationCreaterId",
                table: "Notification",
                newName: "NotificationCreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationCreaterId",
                table: "Notification",
                newName: "IX_Notification_NotificationCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreatorId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "NotificationCreatorId",
                table: "Notification",
                newName: "NotificationCreaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification",
                newName: "IX_Notification_NotificationCreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreaterId",
                table: "Notification",
                column: "NotificationCreaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
