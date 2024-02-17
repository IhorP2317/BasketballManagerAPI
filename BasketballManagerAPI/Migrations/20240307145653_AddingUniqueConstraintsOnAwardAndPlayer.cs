using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingUniqueConstraintsOnAwardAndPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Awards",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId_JerseyNumber",
                table: "Players",
                columns: new[] { "TeamId", "JerseyNumber" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_Name_Date",
                table: "Awards",
                columns: new[] { "Name", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId_JerseyNumber",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Awards_Name_Date",
                table: "Awards");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Awards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");
        }
    }
}
