using Forkly.Api.DAO;
using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Restaurantes;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Restaurantes;
using Microsoft.EntityFrameworkCore;

namespace Forkly.Api.Service.Restaurantes
{
    // Consulta del catálogo de restaurantes de FOR-61: la búsqueda (FOR-62) y la ficha (FOR-63).
    // Solo lee las tablas Restaurantes, RestauranteOpciones y OpcionesPerfil; no guarda nada.
    public class RestauranteService : IRestauranteService
    {
        private readonly ApplicationDbContext _db;

        public RestauranteService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<RestauranteResumenDTO>> Listar()
        {
            // El orden alfabético lo aplica la página (FOR-62, criterio 4)
            var restaurantes = await ConOpciones().OrderBy(r => r.Id).ToListAsync();

            return restaurantes.Select(restaurante => new RestauranteResumenDTO
            {
                Id = restaurante.Id,
                Nombre = restaurante.Nombre,
                Barrio = restaurante.Barrio,
                TiposCocina = NombresDe(restaurante, CategoriaOpcion.COCINA),
                RangoPrecio = RangoPrecioDe(restaurante)
            }).ToList();
        }

        public async Task<FichaRestauranteDTO?> ObtenerFicha(int id)
        {
            var restaurante = await ConOpciones().FirstOrDefaultAsync(r => r.Id == id);
            if (restaurante == null) return null;

            return new FichaRestauranteDTO
            {
                Nombre = restaurante.Nombre,
                RangoPrecio = RangoPrecioDe(restaurante),
                Direccion = restaurante.Direccion,
                Barrio = restaurante.Barrio,
                Telefono = restaurante.Telefono,
                TiposCocina = NombresDe(restaurante, CategoriaOpcion.COCINA),
                Ambientes = NombresDe(restaurante, CategoriaOpcion.AMBIENTE),
                FuenteUrl = restaurante.FuenteUrl
            };
        }

        // Cada restaurante con sus opciones de OpcionesPerfil (tipos de cocina, ambientes y rango de precio)
        private IQueryable<Restaurante> ConOpciones() =>
            _db.Restaurantes
                .AsNoTracking()
                .Include(r => r.Opciones)
                .ThenInclude(o => o.OpcionPerfil);

        // Nombres de las opciones de una categoría, en el orden del catálogo (columna Orden)
        private static List<string> NombresDe(Restaurante restaurante, CategoriaOpcion categoria) =>
            restaurante.Opciones
                .Select(o => o.OpcionPerfil)
                .Where(o => o.Categoria == categoria)
                .OrderBy(o => o.Orden)
                .Select(o => o.Nombre)
                .ToList();

        // Cada restaurante tiene exactamente un rango de precio (FOR-61, criterio 3)
        private static string RangoPrecioDe(Restaurante restaurante) =>
            NombresDe(restaurante, CategoriaOpcion.PRECIO).First();
    }
}
