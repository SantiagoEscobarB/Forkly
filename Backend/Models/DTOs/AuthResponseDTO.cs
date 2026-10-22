namespace Forkly.Api.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public ComensalDTO Usuario { get; set; } = new();
    }
}
