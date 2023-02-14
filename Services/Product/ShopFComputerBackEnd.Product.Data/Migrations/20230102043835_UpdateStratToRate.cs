using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFComputerBackEnd.Product.Data.Migrations
{
    public partial class UpdateStratToRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "ProductVariants",
                newName: "Rate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "ProductVariants",
                newName: "Start");
        }
    }
}
