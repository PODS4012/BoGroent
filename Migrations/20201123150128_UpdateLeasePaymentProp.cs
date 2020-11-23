using Microsoft.EntityFrameworkCore.Migrations;

namespace BoGroent.Migrations
{
    public partial class UpdateLeasePaymentProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Leases");

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "Leases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Leases");

            migrationBuilder.AddColumn<string>(
                name: "Paid",
                table: "Leases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
