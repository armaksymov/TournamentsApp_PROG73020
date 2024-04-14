using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MTournamentsApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRoles",
                columns: table => new
                {
                    PlayerRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlayerRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRoles", x => x.PlayerRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainTeamGameId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlayerIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Games_MainTeamGameId",
                        column: x => x.MainTeamGameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    TournamentGameId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeamIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tournaments_Games_TournamentGameId",
                        column: x => x.TournamentGameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeamId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_PlayerRoles_PlayerRoleId",
                        column: x => x.PlayerRoleId,
                        principalTable: "PlayerRoles",
                        principalColumn: "PlayerRoleId");
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "TeamTournament",
                columns: table => new
                {
                    TournamentTeamsTeamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TournamentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTournament", x => new { x.TournamentTeamsTeamId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_TeamTournament_Teams_TournamentTeamsTeamId",
                        column: x => x.TournamentTeamsTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTournament_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invations",
                columns: table => new
                {
                    InvitationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_Invations_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "StreetAddress", "TournamentCity", "TournamentCountry", "TournamentPostalCode" },
                values: new object[] { 1, "123 Random St S", "Toronto", "Canada", "H0H 0H0" });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "GameName" },
                values: new object[,]
                {
                    { "halInf", "Halo Infinite" },
                    { "val", "Valorant" }
                });

            migrationBuilder.InsertData(
                table: "PlayerRoles",
                columns: new[] { "PlayerRoleId", "PlayerRoleName" },
                values: new object[,]
                {
                    { "C", "Coach" },
                    { "L", "Leader" },
                    { "P", "Player" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamId", "MainTeamGameId", "PlayerIds", "TeamDescription", "TeamName", "TournamentIds" },
                values: new object[] { "ConCE", "val", "[1,2]", "Conestoga's Esports Team", "Conestoga Condors E-Sports", "[1]" });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "AddressID", "TeamIds", "TournamentDate", "TournamentGameId", "TournamentName" },
                values: new object[] { 1, 1, "[\"ConCE\"]", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "val", "Conestoga College Home Tournament" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "PlayerRoleId", "TeamId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Test", "User #1", "C", "ConCE" },
                    { 2, new DateTime(2002, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Test", "User #2", "P", "ConCE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invations_PlayerId",
                table: "Invations",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerRoleId",
                table: "Players",
                column: "PlayerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_MainTeamGameId",
                table: "Teams",
                column: "MainTeamGameId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournament_TournamentsId",
                table: "TeamTournament",
                column: "TournamentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_AddressID",
                table: "Tournaments",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentGameId",
                table: "Tournaments",
                column: "TournamentGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invations");

            migrationBuilder.DropTable(
                name: "TeamTournament");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "PlayerRoles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
