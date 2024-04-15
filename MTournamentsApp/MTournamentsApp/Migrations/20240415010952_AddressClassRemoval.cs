using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTournamentsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddressClassRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Games_MainTeamGameId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Addresses_AddressID",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_AddressID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Tournaments");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MainTeamGameId",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Address",
                value: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Games_MainTeamGameId",
                table: "Teams",
                column: "MainTeamGameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Games_MainTeamGameId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MainTeamGameId",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "StreetAddress", "TournamentCity", "TournamentCountry", "TournamentPostalCode" },
                values: new object[] { 1, "123 Random St S", "Toronto", "Canada", "H0H 0H0" });

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddressID",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_AddressID",
                table: "Tournaments",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Games_MainTeamGameId",
                table: "Teams",
                column: "MainTeamGameId",
                principalTable: "Games",
                principalColumn: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Addresses_AddressID",
                table: "Tournaments",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
