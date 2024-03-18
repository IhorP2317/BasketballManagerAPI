using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingAwardPhotoPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Awards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Awards");
        }
    }
}
