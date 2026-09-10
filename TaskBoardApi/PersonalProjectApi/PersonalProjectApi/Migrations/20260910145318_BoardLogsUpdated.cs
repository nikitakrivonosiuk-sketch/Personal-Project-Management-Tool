using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class BoardLogsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "ActivityLogs",
                type: "uniqueidentifier",
                nullable: true);

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
                column: "DueDate",
                value: new DateTime(2026, 9, 10, 17, 53, 17, 824, DateTimeKind.Local).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 9, 10, 17, 53, 17, 824, DateTimeKind.Local).AddTicks(9474));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "ActivityLogs");

            migrationBuilder.UpdateData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 10, 17, 10, 31, 403, DateTimeKind.Local).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                column: "DueDate",
                value: new DateTime(2026, 9, 10, 17, 10, 31, 404, DateTimeKind.Local).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                column: "DueDate",
                value: new DateTime(2026, 9, 10, 17, 10, 31, 403, DateTimeKind.Local).AddTicks(9998));
        }
    }
}
