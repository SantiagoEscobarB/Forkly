using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forkly.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorregirOrigin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RestauranteOpciones",
                columns: new[] { "OpcionPerfilId", "RestauranteId" },
                values: new object[,]
                {
                    { 1, 16 },
                    { 108, 16 }
                });

            migrationBuilder.UpdateData(
                table: "Restaurantes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Direccion", "FuenteUrl", "Nombre" },
                values: new object[] { "Cra. 33 #7-69", "https://www.origincafecol.com/", "Origin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestauranteOpciones",
                keyColumns: new[] { "OpcionPerfilId", "RestauranteId" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "RestauranteOpciones",
                keyColumns: new[] { "OpcionPerfilId", "RestauranteId" },
                keyValues: new object[] { 108, 16 });

            migrationBuilder.UpdateData(
                table: "Restaurantes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Direccion", "FuenteUrl", "Nombre" },
                values: new object[] { "Cl 26, Cr. 38-21, Int. 101", "https://origincafemenu.netlify.app/", "Origin Café" });
        }
    }
}
