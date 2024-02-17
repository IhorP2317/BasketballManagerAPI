using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsIndividualAwardProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIndividualAward",
                table: "Awards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIndividualAward",
                table: "Awards");
        }
    }
}
