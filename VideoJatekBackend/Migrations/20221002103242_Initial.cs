using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoJatekBackend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kiado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alapitas_datuma = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kiado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videojatek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kategoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fejleszto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KiadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videojatek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videojatek_Kiado_KiadoId",
                        column: x => x.KiadoId,
                        principalTable: "Kiado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videojatek_KiadoId",
                table: "Videojatek",
                column: "KiadoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videojatek");

            migrationBuilder.DropTable(
                name: "Kiado");
        }
    }
}
