using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovingNormalizedEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_CoachesStatuses_CoachStatusId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchHistories_MatchEvents_MatchEventId",
                table: "MatchHistories");

            migrationBuilder.DropTable(
                name: "CoachesStatuses");

            migrationBuilder.DropTable(
                name: "MatchEvents");

            migrationBuilder.DropIndex(
                name: "IX_MatchHistories_MatchEventId",
                table: "MatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_CoachStatusId",
                table: "Coaches");

            migrationBuilder.RenameColumn(
                name: "MatchEventId",
                table: "MatchHistories",
                newName: "MatchEvent");

            migrationBuilder.RenameColumn(
                name: "CoachStatusId",
                table: "Coaches",
                newName: "CoachStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchEvent",
                table: "MatchHistories",
                newName: "MatchEventId");

            migrationBuilder.RenameColumn(
                name: "CoachStatus",
                table: "Coaches",
                newName: "CoachStatusId");

            migrationBuilder.CreateTable(
                name: "CoachesStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachesStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchEvents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CoachesStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Head" },
                    { 2, "Assistant" },
                    { 3, "Personal" }
                });

            migrationBuilder.InsertData(
                table: "MatchEvents",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "OnePointHit" },
                    { 2, "OnePointMiss" },
                    { 3, "TwoPointHit" },
                    { 4, "TwoPointMiss" },
                    { 5, "ThreePointHit" },
                    { 6, "ThreePointMiss" },
                    { 7, "Assist" },
                    { 8, "OffensiveRebound" },
                    { 9, "DefensiveRebound" },
                    { 10, "Steal" },
                    { 11, "Block" },
                    { 12, "Turnover" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistories_MatchEventId",
                table: "MatchHistories",
                column: "MatchEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_CoachStatusId",
                table: "Coaches",
                column: "CoachStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_CoachesStatuses_CoachStatusId",
                table: "Coaches",
                column: "CoachStatusId",
                principalTable: "CoachesStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchHistories_MatchEvents_MatchEventId",
                table: "MatchHistories",
                column: "MatchEventId",
                principalTable: "MatchEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
