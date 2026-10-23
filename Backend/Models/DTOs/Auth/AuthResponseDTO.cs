using Forkly.Api.Models.DTOs.Usuarios;

namespace Forkly.Api.Models.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public ComensalDTO Usuario { get; set; } = new();
    }
}
