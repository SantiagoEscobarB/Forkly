namespace Forkly.Api.Models.Restaurantes
{
    // Establecimiento del catálogo de Medellín (FOR-61). Los datos iniciales están en
    // CatalogoRestaurantes y se cargan con la migración Restaurantes.
    public class Restaurante
    {
        // Id fijo del catálogo (del 1 al 30 en la carga inicial)
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        // Tal como aparece en la fuente, p. ej. "Calle 10 # 40-20"
        public string Direccion { get; set; } = string.Empty;
        // P. ej. "El Poblado" o "Laureles"
        public string Barrio { get; set; } = string.Empty;
        // 10 dígitos en grupos de 3, 3 y 4, p. ej. "604 444 1234" o "300 123 4567"
        public string Telefono { get; set; } = string.Empty;
        // Página web oficial, perfil oficial en una red social o ficha en OpenStreetMap
        public string FuenteUrl { get; set; } = string.Empty;
        // Tipos de cocina, ambientes y rango de precio (opciones de OpcionesPerfil)
        public ICollection<RestauranteOpcion> Opciones { get; set; } = new List<RestauranteOpcion>();
    }
}
