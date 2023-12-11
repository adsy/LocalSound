using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class makeTrackImageNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonArtist_AspNetUsers_UserId",
                table: "NonArtist");

            migrationBuilder.DropIndex(
                name: "IX_NonArtist_UserId",
                table: "NonArtist");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NonArtist");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR MemberId",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR MemberId");

            migrationBuilder.AlterColumn<string>(
                name: "TrackImageUrl",
                table: "ArtistTrackUpload",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_NonArtist_AspNetUsers_AppUserId",
                table: "NonArtist",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonArtist_AspNetUsers_AppUserId",
                table: "NonArtist");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NonArtist",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR MemberId",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "NEXT VALUE FOR MemberId");

            migrationBuilder.AlterColumn<string>(
                name: "TrackImageUrl",
                table: "ArtistTrackUpload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NonArtist_UserId",
                table: "NonArtist",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonArtist_AspNetUsers_UserId",
                table: "NonArtist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
