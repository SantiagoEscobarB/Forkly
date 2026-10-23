using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Usuarios;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;
using Forkly.Api.Service.Email;
using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Service.Verificacion
{
    // El código lo genera el proveedor de correo de Identity (TOTP de 6 dígitos basado en el
    // SecurityStamp del usuario), así que no se guarda en ninguna tabla y vence solo.
    public class VerificacionService : IVerificacionService
    {
        private const string Proposito = "VerificarCorreo";

        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailService _emailService;

        public VerificacionService(UserManager<Usuario> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task EnviarCodigo(Usuario usuario)
        {
            var codigo = await _userManager.GenerateUserTokenAsync(usuario, TokenOptions.DefaultEmailProvider, Proposito);
            await _emailService.Enviar(PlantillasEmail.CodigoVerificacion(usuario.Email!, usuario.NombreCompleto, codigo));
        }

        public async Task ReenviarCodigo(string usuarioId)
        {
            var usuario = await BuscarUsuario(usuarioId);
            if (usuario.Estado != EstadoCuenta.NO_VERIFICADA) throw new CorreoYaVerificadoException();

            await EnviarCodigo(usuario);
        }

        public async Task<ComensalDTO> Verificar(string usuarioId, string codigo)
        {
            var usuario = await BuscarUsuario(usuarioId);
            if (usuario.Estado != EstadoCuenta.NO_VERIFICADA) throw new CorreoYaVerificadoException();

            // El lockout de Identity limita los intentos para que no se pueda adivinar el código
            if (await _userManager.IsLockedOutAsync(usuario)) throw new DemasiadosIntentosException();

            if (!await _userManager.VerifyUserTokenAsync(usuario, TokenOptions.DefaultEmailProvider, Proposito, codigo))
            {
                await _userManager.AccessFailedAsync(usuario);
                throw new CodigoInvalidoException();
            }

            await _userManager.ResetAccessFailedCountAsync(usuario);
            usuario.EmailConfirmed = true;
            usuario.Estado = EstadoCuenta.VERIFICADA;
            await _userManager.UpdateAsync(usuario);

            return ComensalDTO.DesdeEntidad((Comensal)usuario);
        }

        // El id sale del JWT, así que el usuario existe salvo que lo hayan borrado
        private async Task<Usuario> BuscarUsuario(string usuarioId) =>
            await _userManager.FindByIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("El usuario no existe.");
    }
}
