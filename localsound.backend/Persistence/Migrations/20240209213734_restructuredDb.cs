using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class restructuredDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_AspNetUsers_AppUserId",
                table: "AccountImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountOnboarding_AspNetUsers_AppUserId",
                table: "AccountOnboarding");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistBooking_Artist_ArtistId",
                table: "ArtistBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistBooking_AspNetUsers_BookerId",
                table: "ArtistBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEquipment_Artist_AppUserId",
                table: "ArtistEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEventType_Artist_AppUserId",
                table: "ArtistEventType");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistFollower_Artist_ArtistId",
                table: "ArtistFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistFollower_AspNetUsers_FollowerId",
                table: "ArtistFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenre_Artist_AppUserId",
                table: "ArtistGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackage_Artist_AppUserId",
                table: "ArtistPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_Artist_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreatorId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "NonArtist");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoundcloudUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpotifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "NEXT VALUE FOR MemberId")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ProfileUrl",
                table: "Account",
                column: "ProfileUrl",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_Account_AppUserId",
                table: "AccountImage",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountOnboarding_Account_AppUserId",
                table: "AccountOnboarding",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistBooking_Account_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId",
                principalTable: "Account",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistBooking_Account_BookerId",
                table: "ArtistBooking",
                column: "BookerId",
                principalTable: "Account",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEquipment_Account_AppUserId",
                table: "ArtistEquipment",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEventType_Account_AppUserId",
                table: "ArtistEventType",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistFollower_Account_ArtistId",
                table: "ArtistFollower",
                column: "ArtistId",
                principalTable: "Account",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistFollower_Account_FollowerId",
                table: "ArtistFollower",
                column: "FollowerId",
                principalTable: "Account",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenre_Account_AppUserId",
                table: "ArtistGenre",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackage_Account_AppUserId",
                table: "ArtistPackage",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_Account_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId",
                principalTable: "Account",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Account_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId",
                principalTable: "Account",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Account_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                principalTable: "Account",
                principalColumn: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_Account_AppUserId",
                table: "AccountImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountOnboarding_Account_AppUserId",
                table: "AccountOnboarding");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistBooking_Account_ArtistId",
                table: "ArtistBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistBooking_Account_BookerId",
                table: "ArtistBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEquipment_Account_AppUserId",
                table: "ArtistEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEventType_Account_AppUserId",
                table: "ArtistEventType");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistFollower_Account_ArtistId",
                table: "ArtistFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistFollower_Account_FollowerId",
                table: "ArtistFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenre_Account_AppUserId",
                table: "ArtistGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistPackage_Account_AppUserId",
                table: "ArtistPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistTrackUpload_Account_AppUserId",
                table: "ArtistTrackUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Account_NotificationCreatorId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Account_NotificationReceiverId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR MemberId");

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoundcloudUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpotifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_Artist_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonArtist",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileUrl = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonArtist", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_NonArtist_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artist_ProfileUrl",
                table: "Artist",
                column: "ProfileUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NonArtist_ProfileUrl",
                table: "NonArtist",
                column: "ProfileUrl",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_AspNetUsers_AppUserId",
                table: "AccountImage",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountOnboarding_AspNetUsers_AppUserId",
                table: "AccountOnboarding",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistBooking_Artist_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistBooking_AspNetUsers_BookerId",
                table: "ArtistBooking",
                column: "BookerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEquipment_Artist_AppUserId",
                table: "ArtistEquipment",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEventType_Artist_AppUserId",
                table: "ArtistEventType",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistFollower_Artist_ArtistId",
                table: "ArtistFollower",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistFollower_AspNetUsers_FollowerId",
                table: "ArtistFollower",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenre_Artist_AppUserId",
                table: "ArtistGenre",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistPackage_Artist_AppUserId",
                table: "ArtistPackage",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistTrackUpload_Artist_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId",
                principalTable: "Artist",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
