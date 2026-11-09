using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forkly.Api.Migrations
{
    /// <inheritdoc />
    public partial class Restaurantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Barrio = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    FuenteUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurantes", x => x.Id);
                    table.CheckConstraint("CK_Restaurantes_Barrio", "[Barrio] <> ''");
                    table.CheckConstraint("CK_Restaurantes_Direccion", "[Direccion] <> ''");
                    table.CheckConstraint("CK_Restaurantes_FuenteUrl", "[FuenteUrl] LIKE 'https://_%'");
                    table.CheckConstraint("CK_Restaurantes_Nombre", "[Nombre] <> ''");
                    table.CheckConstraint("CK_Restaurantes_Telefono", "[Telefono] LIKE '[0-9][0-9][0-9] [0-9][0-9][0-9] [0-9][0-9][0-9][0-9]'");
                });

            migrationBuilder.CreateTable(
                name: "RestauranteOpciones",
                columns: table => new
                {
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    OpcionPerfilId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestauranteOpciones", x => new { x.RestauranteId, x.OpcionPerfilId });
                    table.ForeignKey(
                        name: "FK_RestauranteOpciones_OpcionesPerfil_OpcionPerfilId",
                        column: x => x.OpcionPerfilId,
                        principalTable: "OpcionesPerfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestauranteOpciones_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Restaurantes",
                columns: new[] { "Id", "Barrio", "Direccion", "FuenteUrl", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "El Poblado", "Calle 10 No. 38 - 38", "https://mondongos.com.co/", "Mondongo's", "604 604 8116" },
                    { 2, "Laureles", "Cra. 73 # Circular 5-6", "https://paneepomodoro.com.co/", "Pane e Pomodoro", "604 322 9679" },
                    { 3, "El Poblado", "Cra 36 # 10 A - 27", "https://carmenmedellin.com/", "Carmen", "316 625 4896" },
                    { 4, "El Poblado", "Calle 18 Av Las Palmas # 35-50", "https://elrancherito.com.co/restaurantes/", "El Rancherito", "604 444 9576" },
                    { 5, "El Poblado", "Cr 33 # 8A - 19", "https://www.kampanascocinasdelmundo.com/", "Kampanas Cocinas del Mundo", "300 297 8868" },
                    { 6, "Centro", "Calle 54 # 47-20", "https://www.casachinajunin.com/", "Casa China Junín", "304 476 8606" },
                    { 7, "Laureles", "Transversal 39 # 76-41", "https://www.lataqueriacocinamexicana.com/", "La Taquería", "304 352 0880" },
                    { 8, "Laureles", "Carrera 75 #40 - 10", "https://www.lapampaparrilla.com/reservas/", "La Pampa", "300 423 0283" },
                    { 9, "Ciudad del Río", "Calle 20 # 43 G-130", "https://malangadeltropico.com/", "Malanga del Trópico", "302 392 3183" },
                    { 10, "El Poblado", "Calle 25A # 1A Sur – 45", "https://natto.com.co/", "Natto", "305 289 0564" },
                    { 11, "Laureles", "Cl. 39c #73-7", "https://www.semilla.com.co/cafe-laureles", "Semilla Café Coworking", "317 515 9876" },
                    { 12, "El Poblado", "Calle 10c # 29c - 20", "https://dondejuancho.com/", "Donde Juancho", "350 629 5755" },
                    { 13, "El Poblado", "Calle 16 No.28-60", "https://hatoviejo.com/", "Hatoviejo", "312 862 6698" },
                    { 14, "El Poblado", "Carrera. 33 # 8A - 11", "https://www.kabuki.com.co/", "Kabuki", "301 470 4009" },
                    { 15, "El Poblado", "Calle 8 # 42-33", "https://www.openstreetmap.org/node/2089497823", "Café Zorba", "604 580 4600" },
                    { 16, "El Poblado", "Cl 26, Cr. 38-21, Int. 101", "https://origincafemenu.netlify.app/", "Origin Café", "333 717 8866" },
                    { 17, "Laureles", "Calle 35 # 77 – 01", "https://www.lamigueria.com.co/", "La Miguería", "604 448 3327" },
                    { 18, "El Poblado", "Carrera 32D # 9 – 35", "https://bistrobabette.com/", "Babette", "304 590 3511" },
                    { 19, "El Poblado", "Carrera 43D No 10-30", "https://elherbario.com/", "Herbario", "604 311 2537" },
                    { 20, "Laureles", "Carrera 70 No . C3 - 43", "https://mondongos.com.co/", "Mondongo's", "604 604 1677" },
                    { 21, "El Poblado", "Carrera. 30 # 10C - 228", "https://www.lapampaparrilla.com/reservas/", "La Pampa", "304 681 4353" },
                    { 22, "El Poblado", "Calle 18 #35 - 69", "https://www.lapampaparrilla.com/reservas/", "La Pampa", "315 794 4033" },
                    { 23, "Laureles", "Cra. 73 Circular 4 - 13", "https://www.lapampaparrilla.com/reservas/", "La Pampa", "324 590 7391" },
                    { 24, "Laureles", "Transversal 39 # 76-41", "https://www.kabuki.com.co/", "Kabuki", "304 352 0880" },
                    { 25, "Centro", "Carrera 47 No.52-17", "https://hatoviejo.com/", "Hatoviejo", "311 631 1324" },
                    { 26, "El Poblado", "Calle 6 sur No 43a 227 Piso 4", "https://hatoviejo.com/", "Hatoviejo", "321 702 6559" },
                    { 27, "Laureles", "Calle 38 # 79 -46", "https://malangadeltropico.com/", "Malanga del Trópico", "311 782 3084" },
                    { 28, "Belén", "Calle 30 A # 82 A 26", "https://elrancherito.com.co/restaurantes/", "El Rancherito", "604 444 9576" },
                    { 29, "Estadio", "Calle 49 # 77 – 35", "https://www.lamigueria.com.co/", "La Miguería", "604 448 3327" },
                    { 30, "Ciudad del Río", "Cl. 20 #43G-144 local 116", "https://natto.com.co/", "Natto", "305 332 5866" }
                });

            migrationBuilder.InsertData(
                table: "RestauranteOpciones",
                columns: new[] { "OpcionPerfilId", "RestauranteId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 101, 1 },
                    { 203, 1 },
                    { 3, 2 },
                    { 9, 2 },
                    { 101, 2 },
                    { 104, 2 },
                    { 106, 2 },
                    { 203, 2 },
                    { 1, 3 },
                    { 102, 3 },
                    { 204, 3 },
                    { 1, 4 },
                    { 11, 4 },
                    { 101, 4 },
                    { 108, 4 },
                    { 203, 4 },
                    { 6, 5 },
                    { 7, 5 },
                    { 104, 5 },
                    { 203, 5 },
                    { 5, 6 },
                    { 103, 6 },
                    { 202, 6 },
                    { 2, 7 },
                    { 101, 7 },
                    { 104, 7 },
                    { 203, 7 },
                    { 11, 8 },
                    { 101, 8 },
                    { 104, 8 },
                    { 203, 8 },
                    { 10, 9 },
                    { 102, 9 },
                    { 104, 9 },
                    { 203, 9 },
                    { 13, 10 },
                    { 103, 10 },
                    { 203, 10 },
                    { 14, 11 },
                    { 105, 11 },
                    { 107, 11 },
                    { 202, 11 },
                    { 8, 12 },
                    { 103, 12 },
                    { 202, 12 },
                    { 1, 13 },
                    { 101, 13 },
                    { 203, 13 },
                    { 4, 14 },
                    { 6, 14 },
                    { 104, 14 },
                    { 203, 14 },
                    { 9, 15 },
                    { 12, 15 },
                    { 104, 15 },
                    { 203, 15 },
                    { 14, 16 },
                    { 103, 16 },
                    { 202, 16 },
                    { 14, 17 },
                    { 103, 17 },
                    { 201, 17 },
                    { 11, 18 },
                    { 102, 18 },
                    { 104, 18 },
                    { 203, 18 },
                    { 1, 19 },
                    { 101, 19 },
                    { 102, 19 },
                    { 203, 19 },
                    { 1, 20 },
                    { 101, 20 },
                    { 203, 20 },
                    { 11, 21 },
                    { 101, 21 },
                    { 104, 21 },
                    { 203, 21 },
                    { 11, 22 },
                    { 101, 22 },
                    { 104, 22 },
                    { 203, 22 },
                    { 11, 23 },
                    { 101, 23 },
                    { 104, 23 },
                    { 203, 23 },
                    { 4, 24 },
                    { 6, 24 },
                    { 104, 24 },
                    { 203, 24 },
                    { 1, 25 },
                    { 101, 25 },
                    { 203, 25 },
                    { 1, 26 },
                    { 101, 26 },
                    { 203, 26 },
                    { 10, 27 },
                    { 102, 27 },
                    { 104, 27 },
                    { 203, 27 },
                    { 1, 28 },
                    { 11, 28 },
                    { 101, 28 },
                    { 104, 28 },
                    { 203, 28 },
                    { 14, 29 },
                    { 103, 29 },
                    { 201, 29 },
                    { 13, 30 },
                    { 103, 30 },
                    { 203, 30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestauranteOpciones_OpcionPerfilId",
                table: "RestauranteOpciones",
                column: "OpcionPerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_Nombre_Direccion",
                table: "Restaurantes",
                columns: new[] { "Nombre", "Direccion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestauranteOpciones");

            migrationBuilder.DropTable(
                name: "Restaurantes");
        }
    }
}
