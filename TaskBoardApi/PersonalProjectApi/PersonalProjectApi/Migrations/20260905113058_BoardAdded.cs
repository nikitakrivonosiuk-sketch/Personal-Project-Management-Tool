using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class BoardAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "BoardLists",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                column: "BoardId",
                value: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"));

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("d009636a-b61a-444d-a186-40aae85263c8"),
                column: "BoardId",
                value: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"));

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("faa6be83-b7c5-4130-98a3-c6fa0982c67f"),
                column: "BoardId",
                value: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"));

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "CreatedAt", "Title" },
                values: new object[] { new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"), new DateTime(2026, 9, 5, 14, 30, 57, 758, DateTimeKind.Local).AddTicks(4745), "My Tasks" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 9, 5, 14, 30, 57, 758, DateTimeKind.Local).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 9, 5, 14, 30, 57, 758, DateTimeKind.Local).AddTicks(4903));

            migrationBuilder.CreateIndex(
                name: "IX_BoardLists_BoardId",
                table: "BoardLists",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardLists_Boards_BoardId",
                table: "BoardLists",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardLists_Boards_BoardId",
                table: "BoardLists");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_BoardLists_BoardId",
                table: "BoardLists");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "BoardLists");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 5, 2, 22, 591, DateTimeKind.Local).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 5, 2, 22, 591, DateTimeKind.Local).AddTicks(2420));
        }
    }
}
