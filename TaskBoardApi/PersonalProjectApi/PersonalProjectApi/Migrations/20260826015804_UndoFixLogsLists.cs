using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class UndoFixLogsLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 4, 58, 4, 2, DateTimeKind.Local).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 4, 58, 4, 1, DateTimeKind.Local).AddTicks(9979));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 4, 47, 22, 306, DateTimeKind.Local).AddTicks(9388));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 8, 26, 4, 47, 22, 306, DateTimeKind.Local).AddTicks(9350));
        }
    }
}
