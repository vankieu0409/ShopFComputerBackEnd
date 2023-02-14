using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopFComputerBackEnd.Product.Data.Migrations
{
    public partial class UpdateFieldNameCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Id",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Categoty",
                table: "Products",
                newName: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Name", "Description", "Category", "Brand", "IsDeleted", "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime", "DeletedBy", "DeletedTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Description", "Category", "Brand", "IsDeleted", "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime", "DeletedBy", "DeletedTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Id",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Categoty");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Name", "Description", "Categoty", "Brand", "IsDeleted", "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime", "DeletedBy", "DeletedTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Description", "Categoty", "Brand", "IsDeleted", "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime", "DeletedBy", "DeletedTime" });
        }
    }
}
