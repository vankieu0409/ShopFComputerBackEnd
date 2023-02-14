using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFComputerBackEnd.Product.Data.Migrations
{
    public partial class UpdateStratSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Sale",
                table: "ProductVariants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Start",
                table: "ProductVariants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sale",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "ProductVariants");
        }
    }
}
