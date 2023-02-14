using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;

#nullable disable

namespace ShopFComputerBackEnd.Product.Data.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Categoty = table.Column<string>(type: "text", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyTimestamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkuId = table.Column<string>(type: "text", nullable: true),
                    ImportPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Images = table.Column<ICollection<ImageValueObject>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true, defaultValue: "Updating"),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionValues_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionValues_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_Id",
                table: "Options",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Name", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Options_Name",
                table: "Options",
                column: "Name")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Options_ProductId",
                table: "Options",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValues_Id",
                table: "OptionValues",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "OptionId", "ProductVariantId", "Value", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OptionValues_OptionId_ProductVariantId",
                table: "OptionValues",
                columns: new[] { "OptionId", "ProductVariantId" },
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Value", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OptionValues_ProductVariantId",
                table: "OptionValues",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValues_Value",
                table: "OptionValues",
                column: "Value")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "OptionId", "ProductVariantId", "IsDeleted" });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_Id",
                table: "ProductVariants",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "SkuId", "ProductId", "ImportPrice", "Price", "Quantity", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_Price",
                table: "ProductVariants",
                column: "Price")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "ProductId", "ImportPrice", "SkuId", "Quantity", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SkuId",
                table: "ProductVariants",
                column: "SkuId",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "ProductId", "ImportPrice", "Price", "Quantity", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionValues");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
