using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardLists", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_BoardLists_BoardListId",
                        column: x => x.BoardListId,
                        principalTable: "BoardLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BoardLists",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), "To Do" },
                    { new Guid("d009636a-b61a-444d-a186-40aae85263c8"), "Finished" },
                    { new Guid("faa6be83-b7c5-4130-98a3-c6fa0982c67f"), "In Progress" }
                });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "CardPriority" },
                values: new object[,]
                {
                    { new Guid("9e1572d6-328d-431d-a5c8-46d2fa119a64"), "Low" },
                    { new Guid("c1f2fa72-41e5-4dbf-bc93-19311c40a43e"), "High" },
                    { new Guid("f4000d47-117a-42cd-81de-44a64d5a4219"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "BoardListId", "Description", "DueDate", "PriorityId", "Title" },
                values: new object[,]
                {
                    { new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"), new Guid("d009636a-b61a-444d-a186-40aae85263c8"), "Clean my wardrobe...", new DateTime(2026, 8, 21, 10, 58, 50, 422, DateTimeKind.Local).AddTicks(4065), new Guid("9e1572d6-328d-431d-a5c8-46d2fa119a64"), "Do household chores" },
                    { new Guid("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"), new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), "Make an DB with all necesary data, connect it with API and front. Test it.", new DateTime(2026, 8, 30, 18, 0, 30, 0, DateTimeKind.Unspecified), new Guid("c1f2fa72-41e5-4dbf-bc93-19311c40a43e"), "Make back for personal project" },
                    { new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"), new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), "Go to the gym and eat well after that.", new DateTime(2026, 8, 21, 10, 58, 50, 422, DateTimeKind.Local).AddTicks(4026), new Guid("f4000d47-117a-42cd-81de-44a64d5a4219"), "Workout" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardListId",
                table: "Cards",
                column: "BoardListId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PriorityId",
                table: "Cards",
                column: "PriorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "BoardLists");

            migrationBuilder.DropTable(
                name: "Priorities");
        }
    }
}
