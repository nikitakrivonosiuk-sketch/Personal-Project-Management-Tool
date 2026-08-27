using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class AddListLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoardListId",
                table: "ActivityLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 3, 23, 25, 813, DateTimeKind.Local).AddTicks(4491));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 3, 23, 25, 813, DateTimeKind.Local).AddTicks(4450));

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BoardListId",
                table: "ActivityLogs",
                column: "BoardListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_BoardLists_BoardListId",
                table: "ActivityLogs",
                column: "BoardListId",
                principalTable: "BoardLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_BoardLists_BoardListId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_BoardListId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "BoardListId",
                table: "ActivityLogs");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 25, 22, 13, 5, 644, DateTimeKind.Local).AddTicks(857));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 25, 22, 13, 5, 644, DateTimeKind.Local).AddTicks(816));
        }
    }
}
