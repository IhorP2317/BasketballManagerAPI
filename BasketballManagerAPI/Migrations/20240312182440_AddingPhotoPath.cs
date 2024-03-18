using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingPhotoPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Coaches");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
