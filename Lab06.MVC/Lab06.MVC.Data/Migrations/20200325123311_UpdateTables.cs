using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab06.MVC.Data.Migrations
{
    public partial class UpdateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Song_Album_AlbumId",
                "Song");

            migrationBuilder.DropColumn(
                "Size",
                "Song");

            migrationBuilder.DropColumn(
                "UserId",
                "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                "AlbumId",
                "Song",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                "Time",
                "Song",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddForeignKey(
                "FK_Song_Album_AlbumId",
                "Song",
                "AlbumId",
                "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Song_Album_AlbumId",
                "Song");

            migrationBuilder.DropColumn(
                "Time",
                "Song");

            migrationBuilder.AlterColumn<int>(
                "AlbumId",
                "Song",
                "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                "Size",
                "Song",
                "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                "UserId",
                "AspNetUsers",
                "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                "FK_Song_Album_AlbumId",
                "Song",
                "AlbumId",
                "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}