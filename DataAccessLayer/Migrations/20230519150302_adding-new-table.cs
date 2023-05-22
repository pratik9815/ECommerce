using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class addingnewtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Img",
                table: "Products",
                newName: "ImgThumbnail");

            migrationBuilder.CreateTable(
                name: "SystemAccessLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLoggedIn = table.Column<bool>(type: "bit", nullable: false),
                    LoggedInDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAccessLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemAccessLogs");

            migrationBuilder.RenameColumn(
                name: "ImgThumbnail",
                table: "Products",
                newName: "Img");
        }
    }
}
