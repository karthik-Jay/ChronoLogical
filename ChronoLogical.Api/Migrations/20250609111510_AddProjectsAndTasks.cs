using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronoLogical.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectsAndTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_ProjectCode",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "TimeEntries");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "TimeEntries",
                type: "INTEGER",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProjectCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DevopsId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ProjectId",
                table: "TimeEntries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Projects_ProjectId",
                table: "TimeEntries",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Tasks_TaskId",
                table: "TimeEntries",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Projects_ProjectId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Tasks_TaskId",
                table: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_ProjectId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TimeEntries");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "TimeEntries",
                type: "TEXT",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "TimeEntries",
                type: "TEXT",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ProjectCode",
                table: "TimeEntries",
                column: "ProjectCode");
        }
    }
}
