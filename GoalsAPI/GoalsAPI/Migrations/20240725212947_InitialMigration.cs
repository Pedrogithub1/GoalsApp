using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoalsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    GoalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalTasks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.GoalId);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    TaskItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    GoalId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.TaskItemId);
                    table.ForeignKey(
                        name: "FK_TaskItems_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "GoalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItems_Goals_GoalId1",
                        column: x => x.GoalId1,
                        principalTable: "Goals",
                        principalColumn: "GoalId");
                });

            migrationBuilder.InsertData(
                table: "Goals",
                columns: new[] { "GoalId", "Date", "Name", "TotalTasks" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), "Configurar plan de compensación", 2 },
                    { 2, new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), "Meta 2", 0 }
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "TaskItemId", "Date", "GoalId", "GoalId1", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, null, "Tarea 1", "Completada" },
                    { 2, new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, null, "Tarea 2", "Abierta" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_GoalId",
                table: "TaskItems",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_GoalId1",
                table: "TaskItems",
                column: "GoalId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Goals");
        }
    }
}
