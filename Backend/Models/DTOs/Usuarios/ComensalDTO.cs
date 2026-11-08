using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;

namespace Forkly.Api.Models.DTOs.Usuarios
{
    // Lo que el frontend recibe de un comensal (Frontend/src/types/usuario.ts).
    // Nunca se devuelve la entidad para no exponer PasswordHash, SecurityStamp, etc.
    public class ComensalDTO
    {
        public string Id { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public EstadoCuenta Estado { get; set; }
        // El frontend lo usa para llevar al usuario a configurar su perfil (H01.09)
        public EstadoPerfil EstadoPerfil { get; set; }

        public static ComensalDTO DesdeEntidad(Comensal comensal) => new()
        {
            Id = comensal.Id,
            NombreCompleto = comensal.NombreCompleto,
            Email = comensal.Email ?? string.Empty,
            UserName = comensal.UserName ?? string.Empty,
            FechaNacimiento = comensal.FechaNacimiento,
            Estado = comensal.Estado,
            // null = PENDIENTE, que es el primer valor del enum
            EstadoPerfil = comensal.EstadoPerfil.GetValueOrDefault()
        };
    }
}
