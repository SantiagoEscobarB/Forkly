using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forkly.Api.Migrations
{
    /// <inheritdoc />
    public partial class PerfilGastronomico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoPerfil",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OpcionesPerfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcionesPerfil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilesGastronomicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComensalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasoActual = table.Column<int>(type: "int", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesGastronomicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilesGastronomicos_AspNetUsers_ComensalId",
                        column: x => x.ComensalId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeleccionesPerfil",
                columns: table => new
                {
                    PerfilGastronomicoId = table.Column<int>(type: "int", nullable: false),
                    OpcionPerfilId = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeleccionesPerfil", x => new { x.PerfilGastronomicoId, x.OpcionPerfilId });
                    table.ForeignKey(
                        name: "FK_SeleccionesPerfil_OpcionesPerfil_OpcionPerfilId",
                        column: x => x.OpcionPerfilId,
                        principalTable: "OpcionesPerfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeleccionesPerfil_PerfilesGastronomicos_PerfilGastronomicoId",
                        column: x => x.PerfilGastronomicoId,
                        principalTable: "PerfilesGastronomicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OpcionesPerfil",
                columns: new[] { "Id", "Categoria", "Codigo", "Detalle", "Nombre", "Orden" },
                values: new object[,]
                {
                    { 1, "COCINA", "cocina.colombiana", null, "Colombiana", 1 },
                    { 2, "COCINA", "cocina.mexicana", null, "Mexicana", 2 },
                    { 3, "COCINA", "cocina.italiana", null, "Italiana", 3 },
                    { 4, "COCINA", "cocina.japonesa", null, "Japonesa", 4 },
                    { 5, "COCINA", "cocina.china", null, "China", 5 },
                    { 6, "COCINA", "cocina.peruana", null, "Peruana", 6 },
                    { 7, "COCINA", "cocina.mediterranea-arabe", null, "Mediterránea y árabe", 7 },
                    { 8, "COCINA", "cocina.hamburguesas-rapida", null, "Hamburguesas y comida rápida", 8 },
                    { 9, "COCINA", "cocina.pizza", null, "Pizza", 9 },
                    { 10, "COCINA", "cocina.mariscos", null, "Mariscos", 10 },
                    { 11, "COCINA", "cocina.parrilla-carnes", null, "Parrilla y carnes", 11 },
                    { 12, "COCINA", "cocina.vegetariana-vegana", null, "Vegetariana y vegana", 12 },
                    { 13, "COCINA", "cocina.saludable", null, "Saludable", 13 },
                    { 14, "COCINA", "cocina.postres-cafes", null, "Postres y cafés", 14 },
                    { 101, "AMBIENTE", "ambiente.familiar", null, "Familiar", 1 },
                    { 102, "AMBIENTE", "ambiente.romantico", null, "Romántico", 2 },
                    { 103, "AMBIENTE", "ambiente.casual", null, "Casual", 3 },
                    { 104, "AMBIENTE", "ambiente.amigos", null, "Con amigos", 4 },
                    { 105, "AMBIENTE", "ambiente.trabajar-estudiar", null, "Para trabajar o estudiar", 5 },
                    { 106, "AMBIENTE", "ambiente.aire-libre", null, "Al aire libre", 6 },
                    { 107, "AMBIENTE", "ambiente.tranquilo", null, "Tranquilo", 7 },
                    { 108, "AMBIENTE", "ambiente.pet-friendly", null, "Pet friendly", 8 },
                    { 201, "PRECIO", "precio.1", "Menos de $25.000", "$", 1 },
                    { 202, "PRECIO", "precio.2", "De $25.000 a $50.000", "$$", 2 },
                    { 203, "PRECIO", "precio.3", "De $50.000 a $100.000", "$$$", 3 },
                    { 204, "PRECIO", "precio.4", "Más de $100.000", "$$$$", 4 },
                    { 301, "DIETA", "dieta.vegetariana", null, "Vegetariana", 1 },
                    { 302, "DIETA", "dieta.vegana", null, "Vegana", 2 },
                    { 303, "DIETA", "dieta.pescetariana", null, "Pescetariana", 3 },
                    { 401, "RESTRICCION", "restriccion.sin-gluten", null, "Sin gluten", 1 },
                    { 402, "RESTRICCION", "restriccion.sin-lactosa", null, "Sin lactosa", 2 },
                    { 403, "RESTRICCION", "restriccion.alergia-frutos-secos", null, "Alergia a frutos secos", 3 },
                    { 404, "RESTRICCION", "restriccion.alergia-mariscos", null, "Alergia a mariscos", 4 },
                    { 405, "RESTRICCION", "restriccion.halal", null, "Halal", 5 },
                    { 406, "RESTRICCION", "restriccion.kosher", null, "Kosher", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpcionesPerfil_Codigo",
                table: "OpcionesPerfil",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesGastronomicos_ComensalId",
                table: "PerfilesGastronomicos",
                column: "ComensalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeleccionesPerfil_OpcionPerfilId",
                table: "SeleccionesPerfil",
                column: "OpcionPerfilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeleccionesPerfil");

            migrationBuilder.DropTable(
                name: "OpcionesPerfil");

            migrationBuilder.DropTable(
                name: "PerfilesGastronomicos");

            migrationBuilder.DropColumn(
                name: "EstadoPerfil",
                table: "AspNetUsers");
        }
    }
}
