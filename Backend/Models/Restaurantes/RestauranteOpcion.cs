using Forkly.Api.Models.Perfil;

namespace Forkly.Api.Models.Restaurantes
{
    // Clasificación de un restaurante con las mismas opciones del perfil gastronómico:
    // tipos de cocina (COCINA), ambientes (AMBIENTE) y su rango de precio (PRECIO).
    public class RestauranteOpcion
    {
        public int RestauranteId { get; set; }
        public Restaurante Restaurante { get; set; } = null!;
        public int OpcionPerfilId { get; set; }
        public OpcionPerfil OpcionPerfil { get; set; } = null!;
    }
}
