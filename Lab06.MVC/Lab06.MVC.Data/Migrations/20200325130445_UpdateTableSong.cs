using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab06.MVC.Data.Migrations
{
    public partial class UpdateTableSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                "Time",
                "Song",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                "Time",
                "Song",
                "time",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}