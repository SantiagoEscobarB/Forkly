using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Recuperacion
{
    // Los dos parámetros del enlace que llega al correo:
    // /restablecer-contrasena?id={UsuarioId}&token={Token}
    public class EnlaceRecuperacionDTO
    {
        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        // Token de Identity codificado en Base64Url para que viaje bien en la URL
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
