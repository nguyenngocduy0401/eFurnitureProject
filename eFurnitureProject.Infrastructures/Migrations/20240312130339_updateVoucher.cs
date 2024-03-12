using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFurnitureProject.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class updateVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "VouchersDetails",
                newName: "Status");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "VouchersDetails",
                newName: "Count");

            migrationBuilder.AlterColumn<int>(
                name: "Time",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
