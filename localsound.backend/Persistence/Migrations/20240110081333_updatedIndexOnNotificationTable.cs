using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedIndexOnNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "NotificationId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                unique: true);
        }
    }
}
