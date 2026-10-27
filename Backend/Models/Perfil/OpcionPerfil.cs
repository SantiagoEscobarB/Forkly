using Forkly.Api.Models.Enums;

namespace Forkly.Api.Models.Perfil
{
    // Catálogo de opciones que el usuario puede elegir (tipos de cocina, ambientes,
    // rangos de precio, tipos de dieta y restricciones). Los datos iniciales están en
    // CatalogoPerfil y se cargan con la migración.
    public class OpcionPerfil
    {
        public int Id { get; set; }
        public CategoriaOpcion Categoria { get; set; }
        // Identificador estable que usa el frontend, p. ej. "cocina.italiana"
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        // Texto de apoyo, p. ej. el rango en pesos de "$$"
        public string? Detalle { get; set; }
        public int Orden { get; set; }
    }
}
