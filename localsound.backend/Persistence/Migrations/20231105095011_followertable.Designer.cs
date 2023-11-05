﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using localsound.backend.Persistence.DbContext;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    [DbContext(typeof(LocalSoundDbContext))]
    [Migration("20231105095011_followertable")]
    partial class followertable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("MemberId")
                .StartsAt(100000L);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AccountImage", b =>
                {
                    b.Property<int>("AccountImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountImageId"));

                    b.Property<int>("AccountImageTypeId")
                        .HasColumnType("int");

                    b.Property<string>("AccountImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FileContentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AccountImageId");

                    b.HasIndex("AccountImageTypeId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("FileContentId")
                        .IsUnique();

                    b.ToTable("AccountImage");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AccountImageType", b =>
                {
                    b.Property<int>("AccountImageTypeId")
                        .HasColumnType("int");

                    b.Property<string>("AccountImageTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountImageTypeId");

                    b.ToTable("AccountImageType");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerType")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValueSql("NEXT VALUE FOR MemberId");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AppUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpirationDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("DateAdd(week,1,getDate())");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.Artist", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AboutSection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SoundcloudUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpotifyUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YoutubeUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AppUserId");

                    b.HasIndex("ProfileUrl")
                        .IsUnique();

                    b.ToTable("Artist");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistEquipment", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EquipmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AppUserId", "EquipmentId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("AppUserId", "EquipmentId"), false);

                    b.HasIndex("AppUserId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AppUserId"));

                    b.ToTable("ArtistEquipment");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistEventType", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppUserId", "EventTypeId");

                    b.HasIndex("EventTypeId");

                    b.ToTable("ArtistEventType");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistFollower", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArtistId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("ArtistFollower");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistGenre", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppUserId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("ArtistGenre");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistTrackGenre", b =>
                {
                    b.Property<Guid>("ArtistTrackUploadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArtistTrackUploadId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("ArtistTrackGenre");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistTrackUpload", b =>
                {
                    b.Property<Guid>("ArtistTrackUploadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<int>("FileSizeInBytes")
                        .HasColumnType("int");

                    b.Property<Guid>("TrackDataId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TrackDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TrackImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TrackImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrackName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrackUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WaveformUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistTrackUploadId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("ArtistTrackUploadId"), false);

                    b.HasIndex("AppUserId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AppUserId"));

                    b.HasIndex("TrackDataId")
                        .IsUnique();

                    b.HasIndex("TrackImageId");

                    b.ToTable("ArtistTrackUpload");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.EventType", b =>
                {
                    b.Property<Guid>("EventTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventTypeId");

                    b.ToTable("EventType");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.FileContent", b =>
                {
                    b.Property<Guid>("FileContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileExtensionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FileContentId");

                    b.ToTable("FileContent");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.Genre", b =>
                {
                    b.Property<Guid>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.NonArtist", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppUserId");

                    b.HasIndex("ProfileUrl")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("NonArtist");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AccountImage", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AccountImageType", "AccountImageType")
                        .WithMany()
                        .HasForeignKey("AccountImageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", "AppUser")
                        .WithMany("Images")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.FileContent", "FileContent")
                        .WithOne("Image")
                        .HasForeignKey("localsound.backend.Domain.Model.Entity.AccountImage", "FileContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountImageType");

                    b.Navigation("AppUser");

                    b.Navigation("FileContent");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AppUserToken", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.Artist", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistEquipment", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.Artist", "Artist")
                        .WithMany("Equipment")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistEventType", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.Artist", "Artist")
                        .WithMany("EventTypes")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistFollower", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.Artist", "Artist")
                        .WithMany("Followers")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistGenre", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.Artist", "Artist")
                        .WithMany("Genres")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistTrackGenre", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.ArtistTrackUpload", "ArtistTrackUpload")
                        .WithMany("Genres")
                        .HasForeignKey("ArtistTrackUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArtistTrackUpload");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistTrackUpload", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.FileContent", "TrackData")
                        .WithOne("ArtistTrackUpload")
                        .HasForeignKey("localsound.backend.Domain.Model.Entity.ArtistTrackUpload", "TrackDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localsound.backend.Domain.Model.Entity.FileContent", "TrackImage")
                        .WithMany()
                        .HasForeignKey("TrackImageId");

                    b.Navigation("Artist");

                    b.Navigation("TrackData");

                    b.Navigation("TrackImage");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.NonArtist", b =>
                {
                    b.HasOne("localsound.backend.Domain.Model.Entity.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.AppUser", b =>
                {
                    b.Navigation("Following");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.Artist", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("EventTypes");

                    b.Navigation("Followers");

                    b.Navigation("Genres");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.ArtistTrackUpload", b =>
                {
                    b.Navigation("Genres");
                });

            modelBuilder.Entity("localsound.backend.Domain.Model.Entity.FileContent", b =>
                {
                    b.Navigation("ArtistTrackUpload")
                        .IsRequired();

                    b.Navigation("Image")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
