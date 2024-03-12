using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFurnitureProject.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class fixDoubleWalettInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Wallet",
                table: "AspNetUsers",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Wallet",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
