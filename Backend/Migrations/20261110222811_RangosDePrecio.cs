using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forkly.Api.Migrations
{
    /// <inheritdoc />
    public partial class RangosDePrecio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detalle",
                table: "OpcionesPerfil");

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 201,
                column: "Nombre",
                value: "Menos de $25.000");

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 202,
                column: "Nombre",
                value: "De $25.000 a $49.999");

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 203,
                column: "Nombre",
                value: "De $50.000 a $99.999");

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 204,
                column: "Nombre",
                value: "$100.000 o más");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Detalle",
                table: "OpcionesPerfil",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 1,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 2,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 3,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 4,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 5,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 6,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 7,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 8,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 9,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 10,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 11,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 12,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 13,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 14,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 101,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 102,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 103,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 104,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 105,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 106,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 107,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 108,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "Detalle", "Nombre" },
                values: new object[] { "Menos de $25.000", "$" });

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "Detalle", "Nombre" },
                values: new object[] { "De $25.000 a $50.000", "$$" });

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "Detalle", "Nombre" },
                values: new object[] { "De $50.000 a $100.000", "$$$" });

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "Detalle", "Nombre" },
                values: new object[] { "Más de $100.000", "$$$$" });

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 301,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 302,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 303,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 401,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 402,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 403,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 404,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 405,
                column: "Detalle",
                value: null);

            migrationBuilder.UpdateData(
                table: "OpcionesPerfil",
                keyColumn: "Id",
                keyValue: 406,
                column: "Detalle",
                value: null);
        }
    }
}
