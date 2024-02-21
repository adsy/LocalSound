using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class splitFileContentTableToReduceCycles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImage_AccountImageFileContent_FileContentId",
                table: "AccountImage");

            migrationBuilder.DropIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage");

            migrationBuilder.DropColumn(
                name: "FileContentId",
                table: "AccountImage");

            migrationBuilder.CreateIndex(
                name: "IX_AccountImageFileContent_AccountImageId",
                table: "AccountImageFileContent",
                column: "AccountImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImageFileContent_AccountImage_AccountImageId",
                table: "AccountImageFileContent",
                column: "AccountImageId",
                principalTable: "AccountImage",
                principalColumn: "AccountImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountImageFileContent_AccountImage_AccountImageId",
                table: "AccountImageFileContent");

            migrationBuilder.DropIndex(
                name: "IX_AccountImageFileContent_AccountImageId",
                table: "AccountImageFileContent");

            migrationBuilder.AddColumn<Guid>(
                name: "FileContentId",
                table: "AccountImage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AccountImage_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountImage_AccountImageFileContent_FileContentId",
                table: "AccountImage",
                column: "FileContentId",
                principalTable: "AccountImageFileContent",
                principalColumn: "FileContentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
