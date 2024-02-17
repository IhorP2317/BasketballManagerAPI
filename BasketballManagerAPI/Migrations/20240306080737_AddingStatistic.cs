using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchHistories");

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeUnit = table.Column<int>(type: "int", nullable: false),
                    OnePointShotHitCount = table.Column<int>(type: "int", nullable: false),
                    OnePointShotMissCount = table.Column<int>(type: "int", nullable: false),
                    TwoPointShotHitCount = table.Column<int>(type: "int", nullable: false),
                    TwoPointShotMissCount = table.Column<int>(type: "int", nullable: false),
                    ThreePointShotHitCount = table.Column<int>(type: "int", nullable: false),
                    ThreePointShotMissCount = table.Column<int>(type: "int", nullable: false),
                    AssistCount = table.Column<int>(type: "int", nullable: false),
                    OffensiveReboundCount = table.Column<int>(type: "int", nullable: false),
                    DefensiveReboundCount = table.Column<int>(type: "int", nullable: false),
                    StealCount = table.Column<int>(type: "int", nullable: false),
                    BlockCount = table.Column<int>(type: "int", nullable: false),
                    TurnoverCount = table.Column<int>(type: "int", nullable: false),
                    CourtTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => new { x.MatchId, x.PlayerId, x.TimeUnit });
                    table.ForeignKey(
                        name: "FK_Statistics_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_PlayerId",
                table: "Statistics",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.CreateTable(
                name: "MatchHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MatchEvent = table.Column<int>(type: "int", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchHistories_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchHistories_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistories_MatchId",
                table: "MatchHistories",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistories_PlayerId",
                table: "MatchHistories",
                column: "PlayerId");
        }
    }
}
