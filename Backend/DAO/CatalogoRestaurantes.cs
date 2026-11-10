using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Perfil;
using Forkly.Api.Models.Restaurantes;

namespace Forkly.Api.DAO
{
    // Restaurantes de Medellín de la carga inicial del catálogo (FOR-61).
    // ApplicationDbContext los carga con HasData, así que quedan en las tablas Restaurantes y
    // RestauranteOpciones al aplicar la migración Restaurantes. Para agregar un restaurante: se
    // añade aquí con un Id nuevo (nunca se reutiliza ni se cambia un Id) y se crea otra migración.
    //
    // Cómo se cargó cada restaurante (criterios 6 y 7):
    // - FuenteUrl es su página web oficial o su ficha en OpenStreetMap, y muestra el Nombre, la
    //   Direccion (escrita como aparece ahí) y el Telefono guardados. Las cadenas se cargan por
    //   sede: cada sede es un establecimiento con su propia dirección y teléfono.
    // - Los tipos de cocina y los ambientes salen de la carta, las fotos y la descripción de la fuente.
    // - El rango de precio es el gasto de una persona en un plato principal y una bebida según la
    //   carta publicada. El cálculo está al lado de cada clasificación en Opciones.
    public static class CatalogoRestaurantes
    {
        public static readonly Restaurante[] Restaurantes =
        [
            Establecimiento(1, "Mondongo's", "Calle 10 No. 38 - 38", "El Poblado", "604 604 8116",
                "https://mondongos.com.co/"),
            Establecimiento(2, "Pane e Pomodoro", "Cra. 73 # Circular 5-6", "Laureles", "604 322 9679",
                "https://paneepomodoro.com.co/"),
            Establecimiento(3, "Carmen", "Cra 36 # 10 A - 27", "El Poblado", "316 625 4896",
                "https://carmenmedellin.com/"),
            Establecimiento(4, "El Rancherito", "Calle 18 Av Las Palmas # 35-50", "El Poblado", "604 444 9576",
                "https://elrancherito.com.co/restaurantes/"),
            Establecimiento(5, "Kampanas Cocinas del Mundo", "Cr 33 # 8A - 19", "El Poblado", "300 297 8868",
                "https://www.kampanascocinasdelmundo.com/"),
            Establecimiento(6, "Casa China Junín", "Calle 54 # 47-20", "Centro", "304 476 8606",
                "https://www.casachinajunin.com/"),
            Establecimiento(7, "La Taquería", "Transversal 39 # 76-41", "Laureles", "304 352 0880",
                "https://www.lataqueriacocinamexicana.com/"),
            Establecimiento(8, "La Pampa", "Carrera 75 #40 - 10", "Laureles", "300 423 0283",
                "https://www.lapampaparrilla.com/reservas/"),
            Establecimiento(9, "Malanga del Trópico", "Calle 20 # 43 G-130", "Ciudad del Río", "302 392 3183",
                "https://malangadeltropico.com/"),
            Establecimiento(10, "Natto", "Calle 25A # 1A Sur – 45", "El Poblado", "305 289 0564",
                "https://natto.com.co/"),
            Establecimiento(11, "Semilla Café Coworking", "Cl. 39c #73-7", "Laureles", "317 515 9876",
                "https://www.semilla.com.co/cafe-laureles"),
            Establecimiento(12, "Donde Juancho", "Calle 10c # 29c - 20", "El Poblado", "350 629 5755",
                "https://dondejuancho.com/"),
            Establecimiento(13, "Hatoviejo", "Calle 16 No.28-60", "El Poblado", "312 862 6698",
                "https://hatoviejo.com/"),
            Establecimiento(14, "Kabuki", "Carrera. 33 # 8A - 11", "El Poblado", "301 470 4009",
                "https://www.kabuki.com.co/"),
            // Su web oficial (cafezorba.com.co) no publica teléfono; su ficha en OpenStreetMap sí
            Establecimiento(15, "Café Zorba", "Calle 8 # 42-33", "El Poblado", "604 580 4600",
                "https://www.openstreetmap.org/node/2089497823"),
            Establecimiento(16, "Origin", "Cra. 33 #7-69", "El Poblado", "333 717 8866",
                "https://www.origincafecol.com/"),
            Establecimiento(17, "La Miguería", "Calle 35 # 77 – 01", "Laureles", "604 448 3327",
                "https://www.lamigueria.com.co/"),
            Establecimiento(18, "Babette", "Carrera 32D # 9 – 35", "El Poblado", "304 590 3511",
                "https://bistrobabette.com/"),
            Establecimiento(19, "Herbario", "Carrera 43D No 10-30", "El Poblado", "604 311 2537",
                "https://elherbario.com/"),
            Establecimiento(20, "Mondongo's", "Carrera 70 No . C3 - 43", "Laureles", "604 604 1677",
                "https://mondongos.com.co/"),
            Establecimiento(21, "La Pampa", "Carrera. 30 # 10C - 228", "El Poblado", "304 681 4353",
                "https://www.lapampaparrilla.com/reservas/"),
            Establecimiento(22, "La Pampa", "Calle 18 #35 - 69", "El Poblado", "315 794 4033",
                "https://www.lapampaparrilla.com/reservas/"),
            Establecimiento(23, "La Pampa", "Cra. 73 Circular 4 - 13", "Laureles", "324 590 7391",
                "https://www.lapampaparrilla.com/reservas/"),
            Establecimiento(24, "Kabuki", "Transversal 39 # 76-41", "Laureles", "304 352 0880",
                "https://www.kabuki.com.co/"),
            Establecimiento(25, "Hatoviejo", "Carrera 47 No.52-17", "Centro", "311 631 1324",
                "https://hatoviejo.com/"),
            Establecimiento(26, "Hatoviejo", "Calle 6 sur No 43a 227 Piso 4", "El Poblado", "321 702 6559",
                "https://hatoviejo.com/"),
            Establecimiento(27, "Malanga del Trópico", "Calle 38 # 79 -46", "Laureles", "311 782 3084",
                "https://malangadeltropico.com/"),
            Establecimiento(28, "El Rancherito", "Calle 30 A # 82 A 26", "Belén", "604 444 9576",
                "https://elrancherito.com.co/restaurantes/"),
            Establecimiento(29, "La Miguería", "Calle 49 # 77 – 35", "Estadio", "604 448 3327",
                "https://www.lamigueria.com.co/"),
            Establecimiento(30, "Natto", "Cl. 20 #43G-144 local 116", "Ciudad del Río", "305 332 5866",
                "https://natto.com.co/"),
        ];

        // Tipos de cocina, ambientes y rango de precio de cada restaurante, con el Codigo de la
        // opción en CatalogoPerfil. Rangos: precio.1 menos de $25.000, precio.2 de $25.000 a
        // $49.999, precio.3 de $50.000 a $99.999 y precio.4 $100.000 o más.
        public static readonly RestauranteOpcion[] Opciones =
        [
            // Mondongo $58.500 (platos típicos de $42.000 a $74.000) + jugo $12.000; menú infantil
            .. Clasificar(1, ["cocina.colombiana"], ["ambiente.familiar"], "precio.3"),
            // Pasta $48.000 + jugo $16.500; trattoria familiar con terraza, cumpleaños desde 4 personas
            .. Clasificar(2, ["cocina.italiana", "cocina.pizza"],
                ["ambiente.familiar", "ambiente.amigos", "ambiente.aire-libre"], "precio.3"),
            // Platos fuertes de $93.000 a $139.000 + bebida; "casa íntima, cálida y elegante"
            .. Clasificar(3, ["cocina.colombiana"], ["ambiente.romantico"], "precio.4"),
            // Bandeja paisa $54.800 + bebida; juegos infantiles y zona pet friendly en esta sede
            .. Clasificar(4, ["cocina.colombiana", "cocina.parrilla-carnes"],
                ["ambiente.familiar", "ambiente.pet-friendly"], "precio.3"),
            // Shawarma o combinado libanés $63.500, lomo saltado $79.500 + bebida; "invitan a reunirse"
            .. Clasificar(5, ["cocina.mediterranea-arabe", "cocina.peruana"], ["ambiente.amigos"], "precio.3"),
            // Porción mini de $20.000 a $28.000 (casi todas de $24.000 o más) + bebida
            .. Clasificar(6, ["cocina.china"], ["ambiente.casual"], "precio.2"),
            // Tacos x3 $42.800 + gaseosa $8.900; "en familia o con amigos"
            .. Clasificar(7, ["cocina.mexicana"], ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            // Churrasco o bife de chorizo 300 g $75.500 + bebida; "con familiares y amigos"
            .. Clasificar(8, ["cocina.parrilla-carnes"], ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            // Arroz palenque $54.000, pesca Malanga $78.000 + bebida; cenas, cumpleaños y "celebra el amor"
            .. Clasificar(9, ["cocina.mariscos"], ["ambiente.romantico", "ambiente.amigos"], "precio.3"),
            // Bowls de $35.000 a $40.000 + jugo natural $16.000; comida saludable del día a día
            .. Clasificar(10, ["cocina.saludable"], ["ambiente.casual"], "precio.3"),
            // Desayunos y sándwiches de $20.900 a $33.900 + latte $7.900; coworking con wifi y zona de lectura
            .. Clasificar(11, ["cocina.postres-cafes"], ["ambiente.trabajar-estudiar", "ambiente.tranquilo"], "precio.2"),
            // Hamburguesa especial $24.700, chuzo de res $29.700 + bebida; comida rápida
            .. Clasificar(12, ["cocina.hamburguesas-rapida"], ["ambiente.casual"], "precio.2"),
            // Bandeja paisa desde $58.000 + bebida; restaurante típico desde 1982, con salones para eventos
            .. Clasificar(13, ["cocina.colombiana"], ["ambiente.familiar"], "precio.3"),
            // Roll completo $49.900, arroces peruanos $57.900 + limonada $12.900; show de teppanyaki
            .. Clasificar(14, ["cocina.japonesa", "cocina.peruana"], ["ambiente.amigos"], "precio.3"),
            // Pizza de $44.000 a $54.000 + gaseosa $14.000; carta sin carne con opciones veganas, reservas para grupos
            .. Clasificar(15, ["cocina.pizza", "cocina.vegetariana-vegana"], ["ambiente.amigos"], "precio.3"),
            // Desayunos de $27.000 a $30.000 y bowls de $28.000 a $43.000 + americano $7.000 o jugo $14.000;
            // café-restaurante de origen colombiano (tamales, calentado, chicharrón) con postres para mascotas
            .. Clasificar(16, ["cocina.colombiana", "cocina.postres-cafes"], ["ambiente.casual", "ambiente.pet-friendly"], "precio.2"),
            // Calzoni de $9.000 a $13.500 o pastel de $5.000 a $8.400 + bebida; panadería y repostería
            .. Clasificar(17, ["cocina.postres-cafes"], ["ambiente.casual"], "precio.1"),
            // Platos fuertes de $46.000 a $95.000 (casi todos de $69.000 a $79.000) + bebida; "lugar para una cita",
            // platos para compartir
            .. Clasificar(18, ["cocina.parrilla-carnes"], ["ambiente.romantico", "ambiente.amigos"], "precio.3"),
            // Platos fuertes de $58.800 a $68.800 + jugo natural $12.800; aniversarios y celebraciones con los tuyos
            .. Clasificar(19, ["cocina.colombiana"], ["ambiente.romantico", "ambiente.familiar"], "precio.3"),
            // Misma carta que la sede 1
            .. Clasificar(20, ["cocina.colombiana"], ["ambiente.familiar"], "precio.3"),
            // Misma carta que la sede 8
            .. Clasificar(21, ["cocina.parrilla-carnes"], ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            .. Clasificar(22, ["cocina.parrilla-carnes"], ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            .. Clasificar(23, ["cocina.parrilla-carnes"], ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            // Misma carta que la sede 14
            .. Clasificar(24, ["cocina.japonesa", "cocina.peruana"], ["ambiente.amigos"], "precio.3"),
            // Misma carta que la sede 13
            .. Clasificar(25, ["cocina.colombiana"], ["ambiente.familiar"], "precio.3"),
            .. Clasificar(26, ["cocina.colombiana"], ["ambiente.familiar"], "precio.3"),
            // Misma carta que la sede 9
            .. Clasificar(27, ["cocina.mariscos"], ["ambiente.romantico", "ambiente.amigos"], "precio.3"),
            // Misma carta que la sede 4; "una buena comida con amigos o familia"
            .. Clasificar(28, ["cocina.colombiana", "cocina.parrilla-carnes"],
                ["ambiente.familiar", "ambiente.amigos"], "precio.3"),
            // Misma carta que la sede 17
            .. Clasificar(29, ["cocina.postres-cafes"], ["ambiente.casual"], "precio.1"),
            // Misma carta que la sede 10
            .. Clasificar(30, ["cocina.saludable"], ["ambiente.casual"], "precio.3"),
        ];

        private static Restaurante Establecimiento(int id, string nombre, string direccion, string barrio,
            string telefono, string fuenteUrl) => new()
        {
            Id = id,
            Nombre = nombre,
            Direccion = direccion,
            Barrio = barrio,
            Telefono = telefono,
            FuenteUrl = fuenteUrl
        };

        // Tipos de cocina y ambientes (uno o más) y exactamente un rango de precio (criterio 3)
        private static IEnumerable<RestauranteOpcion> Clasificar(int restauranteId, string[] cocinas,
            string[] ambientes, string precio) =>
            cocinas.Select(codigo => OpcionExistente(codigo, CategoriaOpcion.COCINA))
                .Concat(ambientes.Select(codigo => OpcionExistente(codigo, CategoriaOpcion.AMBIENTE)))
                .Append(OpcionExistente(precio, CategoriaOpcion.PRECIO))
                .Select(opcion => new RestauranteOpcion { RestauranteId = restauranteId, OpcionPerfilId = opcion.Id });

        // Solo opciones que ya están en OpcionesPerfil y de la categoría esperada: la carga no crea
        // opciones nuevas ni usa DIETA o RESTRICCION. Si el código no existe, falla al crear el modelo.
        private static OpcionPerfil OpcionExistente(string codigo, CategoriaOpcion categoria) =>
            CatalogoPerfil.Opciones.Single(o => o.Codigo == codigo && o.Categoria == categoria);
    }
}
