using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Perfil
{
    // Códigos de todas las opciones elegidas, de cualquier categoría (p. ej. "cocina.italiana")
    public class GuardarPerfilDTO
    {
        [Required]
        public List<string> Opciones { get; set; } = new();
    }
}
