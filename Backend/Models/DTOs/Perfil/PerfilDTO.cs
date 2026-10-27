using Forkly.Api.Models.Enums;

namespace Forkly.Api.Models.DTOs.Perfil
{
    public class PerfilDTO
    {
        public EstadoPerfil Estado { get; set; }
        // Paso del asistente en el que quedó (1 a 4)
        public int PasoActual { get; set; } = 1;
        // Preferencias y restricciones elegidas, en el orden del catálogo
        public List<OpcionPerfilDTO> Seleccionadas { get; set; } = new();
        // Vector de preferencias: un peso por cada tipo de cocina, ambiente y rango de precio
        // del catálogo (0 si no se eligió). Vacío mientras el perfil no se ha confirmado.
        public Dictionary<string, double> Vector { get; set; } = new();
        public DateTime? ActualizadoEn { get; set; }
    }
}
