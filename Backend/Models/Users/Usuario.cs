using Forkly.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Models.Users
{
    // IdentityUser ya trae los demás campos del diagrama:
    // id -> Id, correo -> Email, contrasenaHash -> PasswordHash,
    // nombreUsuario -> UserName, segundoFactor -> TwoFactorEnabled
    public abstract class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public EstadoCuenta Estado { get; set; } = EstadoCuenta.VERIFICADA;
    }
}
