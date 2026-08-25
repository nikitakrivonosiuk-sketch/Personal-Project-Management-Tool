using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriorityInCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 8, 22, 19, 49, 58, 498, DateTimeKind.Local).AddTicks(3737), 1 });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"),
                column: "Priority",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 8, 22, 19, 49, 58, 498, DateTimeKind.Local).AddTicks(3691), 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 8, 22, 19, 47, 43, 293, DateTimeKind.Local).AddTicks(9676), 0 });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"),
                column: "Priority",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 8, 22, 19, 47, 43, 293, DateTimeKind.Local).AddTicks(9630), 0 });
        }
    }
}
