using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Perfil;

namespace Forkly.Api.DAO
{
    // Opciones del perfil gastronómico que ve el usuario (H01.09 y H01.10).
    // ApplicationDbContext las carga con HasData, así que quedan en la tabla OpcionesPerfil
    // al aplicar la migración. Para agregar una opción: se añade aquí con un Id nuevo
    // (nunca se reutiliza ni se cambia un Id) y se crea otra migración.
    public static class CatalogoPerfil
    {
        public static readonly OpcionPerfil[] Opciones =
        [
            // Tipos de cocina
            Opcion(1, CategoriaOpcion.COCINA, "cocina.colombiana", "Colombiana", 1),
            Opcion(2, CategoriaOpcion.COCINA, "cocina.mexicana", "Mexicana", 2),
            Opcion(3, CategoriaOpcion.COCINA, "cocina.italiana", "Italiana", 3),
            Opcion(4, CategoriaOpcion.COCINA, "cocina.japonesa", "Japonesa", 4),
            Opcion(5, CategoriaOpcion.COCINA, "cocina.china", "China", 5),
            Opcion(6, CategoriaOpcion.COCINA, "cocina.peruana", "Peruana", 6),
            Opcion(7, CategoriaOpcion.COCINA, "cocina.mediterranea-arabe", "Mediterránea y árabe", 7),
            Opcion(8, CategoriaOpcion.COCINA, "cocina.hamburguesas-rapida", "Hamburguesas y comida rápida", 8),
            Opcion(9, CategoriaOpcion.COCINA, "cocina.pizza", "Pizza", 9),
            Opcion(10, CategoriaOpcion.COCINA, "cocina.mariscos", "Mariscos", 10),
            Opcion(11, CategoriaOpcion.COCINA, "cocina.parrilla-carnes", "Parrilla y carnes", 11),
            Opcion(12, CategoriaOpcion.COCINA, "cocina.vegetariana-vegana", "Vegetariana y vegana", 12),
            Opcion(13, CategoriaOpcion.COCINA, "cocina.saludable", "Saludable", 13),
            Opcion(14, CategoriaOpcion.COCINA, "cocina.postres-cafes", "Postres y cafés", 14),

            // Ambientes
            Opcion(101, CategoriaOpcion.AMBIENTE, "ambiente.familiar", "Familiar", 1),
            Opcion(102, CategoriaOpcion.AMBIENTE, "ambiente.romantico", "Romántico", 2),
            Opcion(103, CategoriaOpcion.AMBIENTE, "ambiente.casual", "Casual", 3),
            Opcion(104, CategoriaOpcion.AMBIENTE, "ambiente.amigos", "Con amigos", 4),
            Opcion(105, CategoriaOpcion.AMBIENTE, "ambiente.trabajar-estudiar", "Para trabajar o estudiar", 5),
            Opcion(106, CategoriaOpcion.AMBIENTE, "ambiente.aire-libre", "Al aire libre", 6),
            Opcion(107, CategoriaOpcion.AMBIENTE, "ambiente.tranquilo", "Tranquilo", 7),
            Opcion(108, CategoriaOpcion.AMBIENTE, "ambiente.pet-friendly", "Pet friendly", 8),

            // Rangos de precio por persona
            Opcion(201, CategoriaOpcion.PRECIO, "precio.1", "$", 1, "Menos de $25.000"),
            Opcion(202, CategoriaOpcion.PRECIO, "precio.2", "$$", 2, "De $25.000 a $50.000"),
            Opcion(203, CategoriaOpcion.PRECIO, "precio.3", "$$$", 3, "De $50.000 a $100.000"),
            Opcion(204, CategoriaOpcion.PRECIO, "precio.4", "$$$$", 4, "Más de $100.000"),

            // Tipos de dieta: solo se puede elegir uno (H01.10, criterio 2)
            Opcion(301, CategoriaOpcion.DIETA, "dieta.vegetariana", "Vegetariana", 1),
            Opcion(302, CategoriaOpcion.DIETA, "dieta.vegana", "Vegana", 2),
            Opcion(303, CategoriaOpcion.DIETA, "dieta.pescetariana", "Pescetariana", 3),

            // Alergias, intolerancias y otras restricciones: se pueden combinar
            Opcion(401, CategoriaOpcion.RESTRICCION, "restriccion.sin-gluten", "Sin gluten", 1),
            Opcion(402, CategoriaOpcion.RESTRICCION, "restriccion.sin-lactosa", "Sin lactosa", 2),
            Opcion(403, CategoriaOpcion.RESTRICCION, "restriccion.alergia-frutos-secos", "Alergia a frutos secos", 3),
            Opcion(404, CategoriaOpcion.RESTRICCION, "restriccion.alergia-mariscos", "Alergia a mariscos", 4),
            Opcion(405, CategoriaOpcion.RESTRICCION, "restriccion.halal", "Halal", 5),
            Opcion(406, CategoriaOpcion.RESTRICCION, "restriccion.kosher", "Kosher", 6),
        ];

        private static OpcionPerfil Opcion(int id, CategoriaOpcion categoria, string codigo, string nombre,
            int orden, string? detalle = null) => new()
        {
            Id = id,
            Categoria = categoria,
            Codigo = codigo,
            Nombre = nombre,
            Detalle = detalle,
            Orden = orden
        };
    }
}
