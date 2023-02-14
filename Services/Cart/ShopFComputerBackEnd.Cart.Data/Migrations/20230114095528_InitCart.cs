using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

#nullable disable

namespace ShopFComputerBackEnd.Cart.Data.Migrations
{
    public partial class InitCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CART",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Items = table.Column<ICollection<CartItemValueObject>>(type: "jsonb", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CART", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CART_Id",
                table: "CART",
                column: "Id")
                .Annotation("Npgsql:IndexInclude", new[] { "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "IsDeleted", "ModifiedBy", "ModifiedTime", "Items" });

            migrationBuilder.CreateIndex(
                name: "IX_CART_ProfileId",
                table: "CART",
                column: "ProfileId")
                .Annotation("Npgsql:IndexInclude", new[] { "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "IsDeleted", "ModifiedBy", "ModifiedTime", "Items" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CART");
        }
    }
}
