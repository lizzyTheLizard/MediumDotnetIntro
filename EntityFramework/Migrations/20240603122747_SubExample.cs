using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations;

/// <inheritdoc />
public partial class SubExample : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SubExample",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Note = table.Column<string>(type: "TEXT", nullable: false),
                Value = table.Column<int>(type: "INTEGER", nullable: false),
                ExampleId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubExample", x => x.Id);
                table.ForeignKey(
                    name: "FK_SubExample_Examples_ExampleId",
                    column: x => x.ExampleId,
                    principalTable: "Examples",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SubExample_ExampleId",
            table: "SubExample",
            column: "ExampleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SubExample");
    }
}