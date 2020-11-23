using Microsoft.EntityFrameworkCore.Migrations;

namespace BoGroent.Migrations
{
    public partial class UpdateLeaseCarIdPorperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarPlateId",
                table: "Leases");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Leases",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Leases");

            migrationBuilder.AddColumn<int>(
                name: "CarPlateId",
                table: "Leases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
