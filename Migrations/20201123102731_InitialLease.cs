using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoGroent.Migrations
{
    public partial class InitialLease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarPlateId = table.Column<int>(nullable: false),
                    CarBrand = table.Column<string>(nullable: true),
                    CarColor = table.Column<string>(nullable: true),
                    CarRentPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    UserUserName = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Paid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leases");
        }
    }
}
