using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Add_Product_Color_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productCategories_productCategories_ParentId",
                table: "productCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_productSelectedCategories_Products_ProductId",
                table: "productSelectedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_productSelectedCategories_productCategories_ProductCategoryId",
                table: "productSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productSelectedCategories",
                table: "productSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productCategories",
                table: "productCategories");

            migrationBuilder.RenameTable(
                name: "productSelectedCategories",
                newName: "ProductSelectedCategories");

            migrationBuilder.RenameTable(
                name: "productCategories",
                newName: "ProductCategories");

            migrationBuilder.RenameIndex(
                name: "IX_productSelectedCategories_ProductId",
                table: "ProductSelectedCategories",
                newName: "IX_ProductSelectedCategories_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_productSelectedCategories_ProductCategoryId",
                table: "ProductSelectedCategories",
                newName: "IX_ProductSelectedCategories_ProductCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_productCategories_ParentId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    editorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductColors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductCategories_ParentId",
                table: "ProductCategories",
                column: "ParentId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedCategories_ProductCategories_ProductCategoryId",
                table: "ProductSelectedCategories",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedCategories_Products_ProductId",
                table: "ProductSelectedCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategories_ParentId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedCategories_ProductCategories_ProductCategoryId",
                table: "ProductSelectedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedCategories_Products_ProductId",
                table: "ProductSelectedCategories");

            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "ProductSelectedCategories",
                newName: "productSelectedCategories");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "productCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedCategories_ProductId",
                table: "productSelectedCategories",
                newName: "IX_productSelectedCategories_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedCategories_ProductCategoryId",
                table: "productSelectedCategories",
                newName: "IX_productSelectedCategories_ProductCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ParentId",
                table: "productCategories",
                newName: "IX_productCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productSelectedCategories",
                table: "productSelectedCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productCategories",
                table: "productCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productCategories_productCategories_ParentId",
                table: "productCategories",
                column: "ParentId",
                principalTable: "productCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_productSelectedCategories_Products_ProductId",
                table: "productSelectedCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_productSelectedCategories_productCategories_ProductCategoryId",
                table: "productSelectedCategories",
                column: "ProductCategoryId",
                principalTable: "productCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
