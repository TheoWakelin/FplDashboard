using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FplDashboard.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddFixtureData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TeamAId = table.Column<int>(type: "int", nullable: false),
                    TeamHId = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    KickoffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeamAScore = table.Column<int>(type: "int", nullable: true),
                    TeamHScore = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    TeamId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fixtures_GameWeeks_EventId",
                        column: x => x.EventId,
                        principalTable: "GameWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_TeamHId",
                        column: x => x.TeamHId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_TeamId1",
                        column: x => x.TeamId1,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_EventId",
                table: "Fixtures",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_TeamAId",
                table: "Fixtures",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_TeamHId",
                table: "Fixtures",
                column: "TeamHId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_TeamId",
                table: "Fixtures",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_TeamId1",
                table: "Fixtures",
                column: "TeamId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
