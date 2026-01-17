using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace HeroesApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    HeroName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Superpowers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Superpower = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superpowers", x => x.Id);
                });

            var now = DateTime.Now;

            migrationBuilder.InsertData(
                table: "Superpowers",
                columns: ["Superpower", "Description", "CreatedAt", "UpdatedAt"],
                values: new object[,]
                {
                    { "Super Força", "Força sobre-humana", now, now },
                    { "Voo", "Capacidade de voar", now, now },
                    { "Visão de Raio-X", "Ver através de objetos", now, now },
                    { "Invisibilidade", "Tornar-se invisível", now, now },
                    { "Telepatia", "Ler mentes", now, now },
                    { "Velocidade", "Velocidade sobre-humana", now, now },
                    { "Regeneração", "Cura acelerada", now, now },
                    { "Controle Mental", "Controlar outras mentes", now, now }
                });

            migrationBuilder.CreateTable(
                name: "HeroSuperpowers",
                columns: table => new
                {
                    HeroId = table.Column<long>(type: "bigint", nullable: false),
                    SuperpowerId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroSuperpowers", x => new { x.HeroId, x.SuperpowerId });
                    table.ForeignKey(
                        name: "FK_HeroSuperpowers_Heroes_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Heroes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroSuperpowers_Superpowers_SuperpowerId",
                        column: x => x.SuperpowerId,
                        principalTable: "Superpowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_HeroName",
                table: "Heroes",
                column: "HeroName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeroSuperpowers_SuperpowerId",
                table: "HeroSuperpowers",
                column: "SuperpowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Superpowers_Superpower",
                table: "Superpowers",
                column: "Superpower",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroSuperpowers");

            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "Superpowers");
        }
    }
}
