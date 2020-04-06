using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab06.MVC.Data.Migrations
{
    public partial class EditNameCatalogSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogSongs");

            migrationBuilder.CreateTable(
                name: "CatalogSong",
                columns: table => new
                {
                    SongId = table.Column<int>(nullable: false),
                    CatalogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogSong", x => new { x.CatalogId, x.SongId });
                    table.ForeignKey(
                        name: "FK_CatalogSong_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogSong_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogSong_SongId",
                table: "CatalogSong",
                column: "SongId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogSong");

            migrationBuilder.CreateTable(
                name: "CatalogSongs",
                columns: table => new
                {
                    CatalogId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogSongs", x => new { x.CatalogId, x.SongId });
                    table.ForeignKey(
                        name: "FK_CatalogSongs_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogSongs_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogSongs_SongId",
                table: "CatalogSongs",
                column: "SongId");
        }
    }
}
