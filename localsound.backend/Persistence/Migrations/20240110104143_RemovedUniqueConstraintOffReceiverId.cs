using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUniqueConstraintOffReceiverId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
