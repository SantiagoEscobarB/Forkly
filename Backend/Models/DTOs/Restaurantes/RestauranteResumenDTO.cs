namespace Forkly.Api.Models.DTOs.Restaurantes
{
    // Lo que trae GET /api/restaurantes por cada restaurante (FOR-62, criterio 9):
    // solo los datos de su tarjeta en la página "Restaurantes"
    public class RestauranteResumenDTO
    {
        // Para abrir su ficha en /restaurantes/{id} (FOR-63)
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Barrio { get; set; } = string.Empty;
        // Nombres de sus tipos de cocina, en el orden del catálogo
        public List<string> TiposCocina { get; set; } = new();
        // Nombre de su rango de precio, p. ej. "De $25.000 a $49.999"
        public string RangoPrecio { get; set; } = string.Empty;
    }
}
