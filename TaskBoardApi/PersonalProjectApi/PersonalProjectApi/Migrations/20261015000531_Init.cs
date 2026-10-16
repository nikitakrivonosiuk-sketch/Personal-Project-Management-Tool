using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalProjectApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoardLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardLists_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BoardListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_BoardLists_BoardListId",
                        column: x => x.BoardListId,
                        principalTable: "BoardLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "CreatedAt", "Title", "UpdatedAt" },
                values: new object[] { new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "My Tasks", null });

            migrationBuilder.InsertData(
                table: "BoardLists",
                columns: new[] { "Id", "BoardId", "Position", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"), 0, "To Do", null },
                    { new Guid("d009636a-b61a-444d-a186-40aae85263c8"), new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"), 2, "Finished", null },
                    { new Guid("faa6be83-b7c5-4130-98a3-c6fa0982c67f"), new Guid("4637be30-a4e0-4ed0-99a6-83988fd44e00"), 1, "In Progress", null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "BoardListId", "Description", "DueDate", "Priority", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30e15610-ba6a-41b9-ac8d-060fd0e6fbea"), new Guid("d009636a-b61a-444d-a186-40aae85263c8"), "Clean my wardrobe...", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Do household chores", null },
                    { new Guid("4cf6d1e5-bd29-4978-9b38-e4459c0bfa71"), new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), "Make an DB with all necesary data, connect it with API and front. Test it.", new DateTime(2026, 8, 30, 18, 0, 30, 0, DateTimeKind.Unspecified), 3, "Make back for personal project", null },
                    { new Guid("aa94bb74-7c70-4fa8-89ab-de6148c34f98"), new Guid("cffb00c7-25bb-4ee2-929a-f5b4bd9a1bfb"), "Go to the gym and eat well after that.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Workout", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BoardId",
                table: "ActivityLogs",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BoardListId",
                table: "ActivityLogs",
                column: "BoardListId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CardId",
                table: "ActivityLogs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardLists_BoardId",
                table: "BoardLists",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardListId",
                table: "Cards",
                column: "BoardListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "BoardLists");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
