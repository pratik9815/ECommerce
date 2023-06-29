using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class updatingfewthings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories",
                columns: new[] { "CategoryId", "SubCategoryId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubCategories",
                table: "ProductSubCategories",
                columns: new[] { "CategoryId", "SubCategoryId" });
        }
    }
}
