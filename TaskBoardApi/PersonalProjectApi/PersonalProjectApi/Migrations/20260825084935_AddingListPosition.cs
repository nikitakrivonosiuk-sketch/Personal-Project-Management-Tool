using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingListPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "BoardLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("d009636a-b61a-444d-a186-40aae85263c8"),
                column: "Position",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BoardLists",
                keyColumn: "Id",
                keyValue: new Guid("faa6be83-b7c5-4130-98a3-c6fa0982c67f"),
                column: "Position",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 25, 11, 49, 35, 403, DateTimeKind.Local).AddTicks(5354));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 25, 11, 49, 35, 403, DateTimeKind.Local).AddTicks(5313));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "BoardLists");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 22, 19, 49, 58, 498, DateTimeKind.Local).AddTicks(3737));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 22, 19, 49, 58, 498, DateTimeKind.Local).AddTicks(3691));
        }
    }
}
