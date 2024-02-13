using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSongLike2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongLike_Account_AccountMemberId",
                table: "SongLike");

            migrationBuilder.DropIndex(
                name: "IX_SongLike_AccountMemberId",
                table: "SongLike");

            migrationBuilder.DropColumn(
                name: "AccountMemberId",
                table: "SongLike");

            migrationBuilder.AddForeignKey(
                name: "FK_SongLike_Account_MemberId",
                table: "SongLike",
                column: "MemberId",
                principalTable: "Account",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongLike_Account_MemberId",
                table: "SongLike");

            migrationBuilder.AddColumn<string>(
                name: "AccountMemberId",
                table: "SongLike",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongLike_AccountMemberId",
                table: "SongLike",
                column: "AccountMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_SongLike_Account_AccountMemberId",
                table: "SongLike",
                column: "AccountMemberId",
                principalTable: "Account",
                principalColumn: "MemberId");
        }
    }
}
