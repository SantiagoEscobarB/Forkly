namespace Forkly.Api.Models.DTOs.Auth
{
    // Si la cuenta de Google ya está en Forkly se devuelve la sesión.
    // Si no, RequiereRegistro = true y se devuelven los datos de Google
    // para precargar el formulario de completar perfil.
    public class GoogleAuthResponseDTO
    {
        public bool RequiereRegistro { get; set; }
        public AuthResponseDTO? Sesion { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
    }
}
