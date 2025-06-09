using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronoLogical.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ProjectCode = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    TaskId = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_DateRange",
                table: "TimeEntries",
                columns: new[] { "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ProjectCode",
                table: "TimeEntries",
                column: "ProjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_StartTime",
                table: "TimeEntries",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_TaskId",
                table: "TimeEntries",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries");
        }
    }
}
