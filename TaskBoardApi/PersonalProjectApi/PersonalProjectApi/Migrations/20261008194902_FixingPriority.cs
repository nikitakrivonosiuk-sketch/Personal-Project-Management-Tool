using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class FixingPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                column: "CreatedAt",
                value: new DateTime(2026, 10, 8, 22, 49, 2, 417, DateTimeKind.Local).AddTicks(874));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 10, 8, 22, 49, 2, 417, DateTimeKind.Local).AddTicks(1058), 2 });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 10, 8, 22, 49, 2, 417, DateTimeKind.Local).AddTicks(1053));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 10, 17, 53, 17, 824, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                columns: new[] { "DueDate", "Priority" },
                values: new object[] { new DateTime(2026, 9, 10, 17, 53, 17, 824, DateTimeKind.Local).AddTicks(9479), 1 });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 9, 10, 17, 53, 17, 824, DateTimeKind.Local).AddTicks(9474));
        }
    }
}
