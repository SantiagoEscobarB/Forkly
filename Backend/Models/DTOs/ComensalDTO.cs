using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;

namespace Forkly.Api.Models.DTOs
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
        public bool TwoFactorEnabled { get; set; }
        public bool PerfilPublico { get; set; }
        public NivelComensal Nivel { get; set; }

        public static ComensalDTO DesdeEntidad(Comensal comensal) => new()
        {
            Id = comensal.Id,
            NombreCompleto = comensal.NombreCompleto,
            Email = comensal.Email ?? string.Empty,
            UserName = comensal.UserName ?? string.Empty,
            FechaNacimiento = comensal.FechaNacimiento,
            Estado = comensal.Estado,
            TwoFactorEnabled = comensal.TwoFactorEnabled,
            PerfilPublico = comensal.PerfilPublico,
            Nivel = comensal.Nivel
        };
    }
}
