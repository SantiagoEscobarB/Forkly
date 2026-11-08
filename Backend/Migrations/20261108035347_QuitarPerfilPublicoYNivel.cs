using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forkly.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarPerfilPublicoYNivel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PerfilPublico",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nivel",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PerfilPublico",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
