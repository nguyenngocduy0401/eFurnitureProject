using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFurnitureProject.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Imports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Imports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
