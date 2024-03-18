using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketballManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExcludingStatisticAndAward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachAwards_Coaches_CoachId",
                table: "CoachAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAwards_Players_PlayerId",
                table: "PlayerAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Players_PlayerId",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Statistics",
                newName: "PlayerExperienceId");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_PlayerId",
                table: "Statistics",
                newName: "IX_Statistics_PlayerExperienceId");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PlayerAwards",
                newName: "PlayerExperienceId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerAwards_PlayerId",
                table: "PlayerAwards",
                newName: "IX_PlayerAwards_PlayerExperienceId");

            migrationBuilder.RenameColumn(
                name: "CoachId",
                table: "CoachAwards",
                newName: "CoachExperienceId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachAwards_CoachId",
                table: "CoachAwards",
                newName: "IX_CoachAwards_CoachExperienceId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CoachExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachAwards_CoachExperiences_CoachExperienceId",
                table: "CoachAwards",
                column: "CoachExperienceId",
                principalTable: "CoachExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAwards_PlayerExperiences_PlayerExperienceId",
                table: "PlayerAwards",
                column: "PlayerExperienceId",
                principalTable: "PlayerExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_PlayerExperiences_PlayerExperienceId",
                table: "Statistics",
                column: "PlayerExperienceId",
                principalTable: "PlayerExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachAwards_CoachExperiences_CoachExperienceId",
                table: "CoachAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAwards_PlayerExperiences_PlayerExperienceId",
                table: "PlayerAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_PlayerExperiences_PlayerExperienceId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CoachExperiences");

            migrationBuilder.RenameColumn(
                name: "PlayerExperienceId",
                table: "Statistics",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_PlayerExperienceId",
                table: "Statistics",
                newName: "IX_Statistics_PlayerId");

            migrationBuilder.RenameColumn(
                name: "PlayerExperienceId",
                table: "PlayerAwards",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerAwards_PlayerExperienceId",
                table: "PlayerAwards",
                newName: "IX_PlayerAwards_PlayerId");

            migrationBuilder.RenameColumn(
                name: "CoachExperienceId",
                table: "CoachAwards",
                newName: "CoachId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachAwards_CoachExperienceId",
                table: "CoachAwards",
                newName: "IX_CoachAwards_CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachAwards_Coaches_CoachId",
                table: "CoachAwards",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAwards_Players_PlayerId",
                table: "PlayerAwards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Players_PlayerId",
                table: "Statistics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
