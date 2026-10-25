using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Recuperacion
{
    public class RestablecerContrasenaDTO : EnlaceRecuperacionDTO
    {
        // Las reglas las valida Identity, igual que en el registro
        [Required]
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}
