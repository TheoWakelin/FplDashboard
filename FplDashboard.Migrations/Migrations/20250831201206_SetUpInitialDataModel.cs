using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FplDashboard.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SetUpInitialDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameWeekNumber = table.Column<int>(type: "int", nullable: false),
                    YearSeasonStarted = table.Column<int>(type: "int", nullable: false),
                    DeadlineTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AverageEntryScore = table.Column<int>(type: "int", nullable: true),
                    HighestScore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameWeeks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WebName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NowCost = table.Column<double>(type: "float", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<int>(type: "int", nullable: false),
                    PointsPerGame = table.Column<double>(type: "float", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    GoalsScored = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    CleanSheets = table.Column<int>(type: "int", nullable: false),
                    GoalsConceded = table.Column<int>(type: "int", nullable: false),
                    OwnGoals = table.Column<int>(type: "int", nullable: false),
                    PenaltiesSaved = table.Column<int>(type: "int", nullable: false),
                    PenaltiesMissed = table.Column<int>(type: "int", nullable: false),
                    YellowCards = table.Column<int>(type: "int", nullable: false),
                    RedCards = table.Column<int>(type: "int", nullable: false),
                    Saves = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamGameWeekData",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    GameWeekId = table.Column<int>(type: "int", nullable: false),
                    StrengthOverallHome = table.Column<int>(type: "int", nullable: false),
                    StrengthOverallAway = table.Column<int>(type: "int", nullable: false),
                    StrengthAttackHome = table.Column<int>(type: "int", nullable: false),
                    StrengthAttackAway = table.Column<int>(type: "int", nullable: false),
                    StrengthDefenceHome = table.Column<int>(type: "int", nullable: false),
                    StrengthDefenceAway = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Form = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamGameWeekData", x => new { x.TeamId, x.GameWeekId });
                    table.ForeignKey(
                        name: "FK_TeamGameWeekData_GameWeeks_GameWeekId",
                        column: x => x.GameWeekId,
                        principalTable: "GameWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamGameWeekData_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGameWeekData",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameWeekId = table.Column<int>(type: "int", nullable: false),
                    NowCost = table.Column<double>(type: "float", nullable: false),
                    ChanceOfPlayingThisRound = table.Column<double>(type: "float", nullable: true),
                    ChanceOfPlayingNextRound = table.Column<double>(type: "float", nullable: true),
                    ExpectedAssistsPer90 = table.Column<double>(type: "float", nullable: false),
                    ExpectedGoalInvolvementsPer90 = table.Column<double>(type: "float", nullable: false),
                    ExpectedGoalsPer90 = table.Column<double>(type: "float", nullable: false),
                    ExpectedGoalsConcededPer90 = table.Column<double>(type: "float", nullable: false),
                    DefensiveContributionPer90 = table.Column<double>(type: "float", nullable: false),
                    SavesPer90 = table.Column<double>(type: "float", nullable: false),
                    EventPoints = table.Column<int>(type: "int", nullable: false),
                    Form = table.Column<double>(type: "float", nullable: false),
                    SelectedByPercent = table.Column<double>(type: "float", nullable: false),
                    ValueSeason = table.Column<double>(type: "float", nullable: false),
                    ValueForm = table.Column<double>(type: "float", nullable: false),
                    Bps = table.Column<int>(type: "int", nullable: false),
                    Influence = table.Column<double>(type: "float", nullable: false),
                    Creativity = table.Column<double>(type: "float", nullable: false),
                    Threat = table.Column<double>(type: "float", nullable: false),
                    IctIndex = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameWeekData", x => new { x.PlayerId, x.GameWeekId });
                    table.ForeignKey(
                        name: "FK_PlayerGameWeekData_GameWeeks_GameWeekId",
                        column: x => x.GameWeekId,
                        principalTable: "GameWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameWeekData_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerNews",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    NewsAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    News = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerNews", x => new { x.PlayerId, x.NewsAdded });
                    table.ForeignKey(
                        name: "FK_PlayerNews_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameWeeks_GameWeekNumber_YearSeasonStarted",
                table: "GameWeeks",
                columns: new[] { "GameWeekNumber", "YearSeasonStarted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeekData_GameWeekId",
                table: "PlayerGameWeekData",
                column: "GameWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeekData_GameWeekId",
                table: "TeamGameWeekData",
                column: "GameWeekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameWeekData");

            migrationBuilder.DropTable(
                name: "PlayerNews");

            migrationBuilder.DropTable(
                name: "TeamGameWeekData");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "GameWeeks");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
