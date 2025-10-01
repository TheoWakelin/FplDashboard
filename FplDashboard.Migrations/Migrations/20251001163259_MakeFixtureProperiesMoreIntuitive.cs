using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FplDashboard.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class MakeFixtureProperiesMoreIntuitive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_GameWeeks_EventId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_TeamAId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_TeamHId",
                table: "Fixtures");

            migrationBuilder.RenameColumn(
                name: "TeamHScore",
                table: "Fixtures",
                newName: "HomeTeamScore");

            migrationBuilder.RenameColumn(
                name: "TeamHId",
                table: "Fixtures",
                newName: "HomeTeamId");

            migrationBuilder.RenameColumn(
                name: "TeamAScore",
                table: "Fixtures",
                newName: "AwayTeamScore");

            migrationBuilder.RenameColumn(
                name: "TeamAId",
                table: "Fixtures",
                newName: "AwayTeamId");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Fixtures",
                newName: "GameweekId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_TeamHId",
                table: "Fixtures",
                newName: "IX_Fixtures_HomeTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_TeamAId",
                table: "Fixtures",
                newName: "IX_Fixtures_GameweekId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_EventId",
                table: "Fixtures",
                newName: "IX_Fixtures_AwayTeamId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Players",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            
            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_GameWeeks_GameweekId",
                table: "Fixtures",
                column: "GameweekId",
                principalTable: "GameWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_GameWeeks_GameweekId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_AwayTeamId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Teams_HomeTeamId",
                table: "Fixtures");

            migrationBuilder.RenameColumn(
                name: "HomeTeamScore",
                table: "Fixtures",
                newName: "TeamHScore");

            migrationBuilder.RenameColumn(
                name: "HomeTeamId",
                table: "Fixtures",
                newName: "TeamHId");

            migrationBuilder.RenameColumn(
                name: "GameweekId",
                table: "Fixtures",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "AwayTeamScore",
                table: "Fixtures",
                newName: "TeamAScore");

            migrationBuilder.RenameColumn(
                name: "AwayTeamId",
                table: "Fixtures",
                newName: "TeamAId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures",
                newName: "IX_Fixtures_TeamHId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_GameweekId",
                table: "Fixtures",
                newName: "IX_Fixtures_TeamAId");

            migrationBuilder.RenameIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures",
                newName: "IX_Fixtures_EventId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_GameWeeks_EventId",
                table: "Fixtures",
                column: "EventId",
                principalTable: "GameWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_TeamAId",
                table: "Fixtures",
                column: "TeamAId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Teams_TeamHId",
                table: "Fixtures",
                column: "TeamHId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
