namespace Forkly.Api.Models.DTOs.Restaurantes
{
    // Lo que trae GET /api/restaurantes/{id} (FOR-63, criterio 8): solo los datos que
    // muestra la ficha del restaurante (criterios 2 a 5)
    public class FichaRestauranteDTO
    {
        public string Nombre { get; set; } = string.Empty;
        // Nombre de su rango de precio, p. ej. "De $25.000 a $49.999"
        public string RangoPrecio { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Barrio { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        // Nombres de sus tipos de cocina y de sus ambientes, en el orden del catálogo
        public List<string> TiposCocina { get; set; } = new();
        public List<string> Ambientes { get; set; } = new();
        public string FuenteUrl { get; set; } = string.Empty;
    }
}
