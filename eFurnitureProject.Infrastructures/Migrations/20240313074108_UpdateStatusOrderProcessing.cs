using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFurnitureProject.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusOrderProcessing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "StatusOrderProcessings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "StatusOrderProcessings");
        }
    }
}
