using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentShedulerAPI.Migrations
{
    /// <inheritdoc />
    public partial class DoradaAppointmenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cancelled",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Appointments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Appointments");
        }
    }
}
