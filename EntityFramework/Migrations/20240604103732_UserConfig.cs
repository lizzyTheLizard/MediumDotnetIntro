using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UserConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConfiguration",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ExampleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfiguration", x => x.Name);
                    table.ForeignKey(
                        name: "FK_UserConfiguration_Examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguration_ExampleId",
                table: "UserConfiguration",
                column: "ExampleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConfiguration");
        }
    }
}
