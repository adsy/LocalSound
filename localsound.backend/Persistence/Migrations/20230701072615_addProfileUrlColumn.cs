using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localsound.backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addProfileUrlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileUrl",
                table: "Artist",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileUrl",
                table: "Artist");
        }
    }
}
