using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationCreaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationViewed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_NotificationCreaterId",
                        column: x => x.NotificationCreaterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_NotificationReceiverId",
                        column: x => x.NotificationReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationCreaterId",
                table: "Notification",
                column: "NotificationCreaterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}
