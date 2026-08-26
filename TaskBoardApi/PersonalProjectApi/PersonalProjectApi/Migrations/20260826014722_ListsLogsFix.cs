using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class ListsLogsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
