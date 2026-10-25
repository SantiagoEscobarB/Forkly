using Forkly.Api.Interface;
using Forkly.Api.Models.Configuracion;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;
using Forkly.Api.Service.Auth;
using Forkly.Api.Service.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;

namespace Forkly.Api.Service.Recuperacion
{
    // El token lo genera el proveedor por defecto de Identity (DataProtectorTokenProvider):
    // no se guarda en ninguna tabla, vence a los 30 minutos (Program.cs) y lleva el
    // SecurityStamp del usuario. Como restablecer la contraseña cambia el SecurityStamp,
    // el enlace deja de servir apenas se usa (un solo uso).
    public class RecuperacionService : IRecuperacionService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailService _emailService;
        private readonly FrontendOptions _frontend;
        private readonly ILogger<RecuperacionService> _logger;

        public RecuperacionService(UserManager<Usuario> userManager, IEmailService emailService,
            IOptions<FrontendOptions> frontend, ILogger<RecuperacionService> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _frontend = frontend.Value;
            _logger = logger;
        }

        public async Task SolicitarRecuperacion(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email.Trim());
            // Si el correo no está registrado no se hace nada: el controlador responde lo mismo
            // en ambos casos para no revelar qué correos tienen cuenta
            if (usuario == null || usuario.Estado == EstadoCuenta.BLOQUEADA) return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            var enlace = $"{_frontend.Url.TrimEnd('/')}/restablecer-contrasena" +
                         $"?id={Uri.EscapeDataString(usuario.Id)}" +
                         $"&token={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))}";
            var minutos = (int)PoliticaAcceso.VigenciaEnlaceRecuperacion.TotalMinutes;

            // Si el correo falla tampoco se avisa al usuario, por la misma razón de arriba
            try
            {
                await _emailService.Enviar(
                    PlantillasEmail.RecuperarContrasena(usuario.Email!, usuario.NombreCompleto, enlace, minutos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo enviar el enlace de recuperación a {Email}", usuario.Email);
            }
        }

        // Permite que el frontend avise que el enlace venció antes de que el usuario escriba la contraseña
        public async Task ValidarEnlace(string usuarioId, string token)
        {
            var (usuario, tokenIdentity) = await LeerEnlace(usuarioId, token);

            var valido = await _userManager.VerifyUserTokenAsync(usuario,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                UserManager<Usuario>.ResetPasswordTokenPurpose, tokenIdentity);
            if (!valido) throw new EnlaceInvalidoException();
        }

        public async Task RestablecerContrasena(string usuarioId, string token, string nuevaContrasena)
        {
            var (usuario, tokenIdentity) = await LeerEnlace(usuarioId, token);

            // Identity revisa primero el token y después las reglas de la contraseña.
            // Al terminar cambia el SecurityStamp: el enlace ya no sirve y los JWT emitidos
            // antes dejan de ser válidos (SelloSesion), es decir, se cierran todas las sesiones.
            var result = await _userManager.ResetPasswordAsync(usuario, tokenIdentity, nuevaContrasena);
            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.InvalidToken)))
                    throw new EnlaceInvalidoException();
                throw new ContrasenaInvalidaException(result.Errors);
            }

            // Si estaba bloqueado por intentos fallidos (H01.04), recuperar la contraseña le devuelve el acceso
            await _userManager.ResetAccessFailedCountAsync(usuario);
            await _userManager.SetLockoutEndDateAsync(usuario, null);
        }

        // Devuelve el usuario del enlace y el token tal como lo generó Identity
        private async Task<(Usuario, string)> LeerEnlace(string usuarioId, string token)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null || usuario.Estado == EstadoCuenta.BLOQUEADA) throw new EnlaceInvalidoException();

            try
            {
                return (usuario, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)));
            }
            catch (FormatException)
            {
                // Enlace recortado o modificado a mano
                throw new EnlaceInvalidoException();
            }
        }
    }
}
