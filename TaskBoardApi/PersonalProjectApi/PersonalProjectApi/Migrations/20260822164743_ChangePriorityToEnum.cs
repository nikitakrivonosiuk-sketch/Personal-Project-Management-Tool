using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriorityToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Priorities_PriorityId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropIndex(
                name: "IX_Cards_PriorityId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Cards");

            migrationBuilder.AddColumn<Guid>(
                name: "PriorityId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardPriority = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"),
                columns: new[] { "DueDate", "PriorityId" },
                values: new object[] { new DateTime(2026, 8, 21, 10, 58, 50, 422, DateTimeKind.Local).AddTicks(4065), new Guid("9e1572d6-328d-431d-a5c8-46d2fa119a64") });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"),
                column: "PriorityId",
                value: new Guid("c1f2fa72-41e5-4dbf-bc93-19311c40a43e"));

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"),
                columns: new[] { "DueDate", "PriorityId" },
                values: new object[] { new DateTime(2026, 8, 21, 10, 58, 50, 422, DateTimeKind.Local).AddTicks(4026), new Guid("f4000d47-117a-42cd-81de-44a64d5a4219") });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "CardPriority" },
                values: new object[,]
                {
                    { new Guid("9e1572d6-328d-431d-a5c8-46d2fa119a64"), "Low" },
                    { new Guid("c1f2fa72-41e5-4dbf-bc93-19311c40a43e"), "High" },
                    { new Guid("f4000d47-117a-42cd-81de-44a64d5a4219"), "Medium" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PriorityId",
                table: "Cards",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Priorities_PriorityId",
                table: "Cards",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
