using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Examples",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                Value = table.Column<int>(type: "INTEGER", nullable: false),
                Note2 = table.Column<string>(type: "varchar256", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Examples", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Examples");
    }
}