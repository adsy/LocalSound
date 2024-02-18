using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class restartDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "MemberId",
                startValue: 100000L);

            migrationBuilder.CreateSequence(
                name: "SongLikeId");

            migrationBuilder.CreateTable(
                name: "AccountImageType",
                columns: table => new
                {
                    AccountImageTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountImageTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountImageType", x => x.AccountImageTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    EventTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.EventTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtensionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.FileContentId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoundcloudUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpotifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEXT VALUE FOR MemberId")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AppUserId);
                    table.UniqueConstraint("AK_Account_MemberId", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "DateAdd(week,1,getDate())"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountGenre",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGenre", x => new { x.AppUserId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_AccountGenre_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountImage",
                columns: table => new
                {
                    AccountImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountImageTypeId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToBeDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountImage", x => x.AccountImageId);
                    table.ForeignKey(
                        name: "FK_AccountImage_AccountImageType_AccountImageTypeId",
                        column: x => x.AccountImageTypeId,
                        principalTable: "AccountImageType",
                        principalColumn: "AccountImageTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountImage_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountImage_FileContent_FileContentId",
                        column: x => x.FileContentId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountMessages",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnboardingMessageClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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

            migrationBuilder.CreateTable(
                name: "ArtistEquipment",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistEquipment", x => new { x.AppUserId, x.EquipmentId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistEquipment_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistEventType",
                columns: table => new
                {
                    EventTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistEventType", x => new { x.AppUserId, x.EventTypeId });
                    table.ForeignKey(
                        name: "FK_ArtistEventType_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistEventType_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventType",
                        principalColumn: "EventTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistFollower",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistFollower", x => new { x.ArtistId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_ArtistFollower_Account_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                    table.ForeignKey(
                        name: "FK_ArtistFollower_Account_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                });

            migrationBuilder.CreateTable(
                name: "ArtistPackage",
                columns: table => new
                {
                    ArtistPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackagePrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistPackage", x => x.ArtistPackageId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistPackage_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrackUpload",
                columns: table => new
                {
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistMemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackUpload", x => x.ArtistTrackUploadId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_Account_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Account",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_FileContent_TrackDataId",
                        column: x => x.TrackDataId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrackUpload_FileContent_TrackImageId",
                        column: x => x.TrackImageId,
                        principalTable: "FileContent",
                        principalColumn: "FileContentId");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationCreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationViewed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Notification_Account_NotificationCreatorId",
                        column: x => x.NotificationCreatorId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                    table.ForeignKey(
                        name: "FK_Notification_Account_NotificationReceiverId",
                        column: x => x.NotificationReceiverId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                });

            migrationBuilder.CreateTable(
                name: "ArtistBooking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    BookingCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistBooking", x => x.BookingId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ArtistBooking_Account_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_Account_BookerId",
                        column: x => x.BookerId,
                        principalTable: "Account",
                        principalColumn: "AppUserId");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_ArtistPackage_PackageId",
                        column: x => x.PackageId,
                        principalTable: "ArtistPackage",
                        principalColumn: "ArtistPackageId");
                    table.ForeignKey(
                        name: "FK_ArtistBooking_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventType",
                        principalColumn: "EventTypeId");
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
                    table.PrimaryKey("PK_ArtistPackageEquipment", x => new { x.ArtistPackageId, x.ArtistPackageEquipmentId })
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
                    ArtistPackagePhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToBeDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ArtistTrackGenre",
                columns: table => new
                {
                    ArtistTrackUploadId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrackGenre", x => new { x.ArtistTrackUploadId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ArtistTrackGenre_ArtistTrackUpload_ArtistTrackUploadId",
                        column: x => x.ArtistTrackUploadId,
                        principalTable: "ArtistTrackUpload",
                        principalColumn: "ArtistTrackUploadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrackGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SongLike",
                columns: table => new
                {
                    SongLikeId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SongLikeId"),
                    ArtistTrackId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongLike", x => x.SongLikeId)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_SongLike_ArtistTrackId_MemberId", x => new { x.ArtistTrackId, x.MemberId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_SongLike_Account_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Account",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongLike_ArtistTrackUpload_ArtistTrackId",
                        column: x => x.ArtistTrackId,
                        principalTable: "ArtistTrackUpload",
                        principalColumn: "ArtistTrackUploadId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_MemberId",
                table: "Account",
                column: "MemberId",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Account_ProfileUrl",
                table: "Account",
                column: "ProfileUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGenre_GenreId",
                table: "AccountGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_AccountImageTypeId",
                table: "AccountImage",
                column: "AccountImageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_AppUserId",
                table: "AccountImage",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_ArtistId",
                table: "ArtistBooking",
                column: "ArtistId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_BookerId",
                table: "ArtistBooking",
                column: "BookerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_EventTypeId",
                table: "ArtistBooking",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistBooking_PackageId",
                table: "ArtistBooking",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistEquipment_AppUserId",
                table: "ArtistEquipment",
                column: "AppUserId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistEventType_EventTypeId",
                table: "ArtistEventType",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistFollower_FollowerId",
                table: "ArtistFollower",
                column: "FollowerId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackGenre_GenreId",
                table: "ArtistTrackGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_AppUserId",
                table: "ArtistTrackUpload",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_ArtistMemberId",
                table: "ArtistTrackUpload",
                column: "ArtistMemberId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackDataId",
                table: "ArtistTrackUpload",
                column: "TrackDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrackUpload_TrackImageId",
                table: "ArtistTrackUpload",
                column: "TrackImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationCreatorId",
                table: "Notification",
                column: "NotificationCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationReceiverId",
                table: "Notification",
                column: "NotificationReceiverId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_ArtistTrackId",
                table: "SongLike",
                column: "ArtistTrackId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_MemberId",
                table: "SongLike",
                column: "MemberId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGenre");

            migrationBuilder.DropTable(
                name: "AccountImage");

            migrationBuilder.DropTable(
                name: "AccountMessages");

            migrationBuilder.DropTable(
                name: "ArtistBooking");

            migrationBuilder.DropTable(
                name: "ArtistEquipment");

            migrationBuilder.DropTable(
                name: "ArtistEventType");

            migrationBuilder.DropTable(
                name: "ArtistFollower");

            migrationBuilder.DropTable(
                name: "ArtistPackageEquipment");

            migrationBuilder.DropTable(
                name: "ArtistPackagePhoto");

            migrationBuilder.DropTable(
                name: "ArtistTrackGenre");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "SongLike");

            migrationBuilder.DropTable(
                name: "AccountImageType");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "ArtistPackage");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ArtistTrackUpload");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropSequence(
                name: "MemberId");

            migrationBuilder.DropSequence(
                name: "SongLikeId");
        }
    }
}
