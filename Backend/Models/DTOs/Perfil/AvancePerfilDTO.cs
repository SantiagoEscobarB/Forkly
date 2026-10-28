using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Perfil
{
    // Avance del asistente: lo elegido hasta ahora y el paso en el que va el usuario
    public class AvancePerfilDTO : GuardarPerfilDTO
    {
        [Range(1, 4)]
        public int Paso { get; set; } = 1;
    }
}
