using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Auth
{
    public class GoogleLoginDTO
    {
        // ID token (credential) que entrega Google Identity Services en el frontend
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}
