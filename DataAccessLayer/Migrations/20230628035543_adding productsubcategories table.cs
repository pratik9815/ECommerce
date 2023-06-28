using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class addingproductsubcategoriestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategory_Products_ProductId",
                table: "ProductSubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategory_SubCategories_SubCategoryId",
                table: "ProductSubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubCategory",
                table: "ProductSubCategory");

            migrationBuilder.RenameTable(
                name: "ProductSubCategory",
                newName: "ProductSubCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubCategory_SubCategoryId",
                table: "ProductSubCategories",
                newName: "IX_ProductSubCategories_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubCategory_ProductId",
                table: "ProductSubCategories",
                newName: "IX_ProductSubCategories_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories",
                columns: new[] { "CategoryId", "SubCategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_Products_ProductId",
                table: "ProductSubCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_SubCategories_SubCategoryId",
                table: "ProductSubCategories",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_Products_ProductId",
                table: "ProductSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_SubCategories_SubCategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories");

            migrationBuilder.RenameTable(
                name: "ProductSubCategories",
                newName: "ProductSubCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubCategories_SubCategoryId",
                table: "ProductSubCategory",
                newName: "IX_ProductSubCategory_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubCategories_ProductId",
                table: "ProductSubCategory",
                newName: "IX_ProductSubCategory_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubCategory",
                table: "ProductSubCategory",
                columns: new[] { "CategoryId", "SubCategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategory_Products_ProductId",
                table: "ProductSubCategory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategory_SubCategories_SubCategoryId",
                table: "ProductSubCategory",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
