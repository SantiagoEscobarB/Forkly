using System.Security.Cryptography;
using System.Text;

namespace Forkly.Api.Service.Auth
{
    // El JWT lleva un resumen del SecurityStamp del usuario y Program.cs lo compara en cada
    // petición. Identity cambia el SecurityStamp al restablecer la contraseña, así que todos
    // los tokens emitidos antes dejan de valer: así se cierran las sesiones abiertas (H01.06).
    // Se guarda el hash y no el sello porque el código de verificación de correo se calcula
    // a partir del sello, y el contenido de un JWT lo puede leer cualquiera.
    public static class SelloSesion
    {
        public const string Claim = "forkly:sello";

        public static string Calcular(string? securityStamp) =>
            Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(securityStamp ?? string.Empty)));
    }
}
