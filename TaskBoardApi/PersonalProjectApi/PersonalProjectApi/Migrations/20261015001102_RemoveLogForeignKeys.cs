using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLogForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_BoardLists_BoardListId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Boards_BoardId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Cards_CardId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_BoardId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_BoardListId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_CardId",
                table: "ActivityLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BoardId",
                table: "ActivityLogs",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BoardListId",
                table: "ActivityLogs",
                column: "BoardListId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CardId",
                table: "ActivityLogs",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_BoardLists_BoardListId",
                table: "ActivityLogs",
                column: "BoardListId",
                principalTable: "BoardLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Boards_BoardId",
                table: "ActivityLogs",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Cards_CardId",
                table: "ActivityLogs",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
