using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab06.MVC.Data.Migrations
{
    public partial class UpdateCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Catalogs_AspNetUsers_UserId",
                "Catalogs");

            migrationBuilder.AddForeignKey(
                "FK_Catalogs_AspNetUsers_UserId",
                "Catalogs",
                "UserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Catalogs_AspNetUsers_UserId",
                "Catalogs");

            migrationBuilder.AddForeignKey(
                "FK_Catalogs_AspNetUsers_UserId",
                "Catalogs",
                "UserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}