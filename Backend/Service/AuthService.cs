using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Forkly.Api.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<Usuario> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<AuthResponseDTO> Registro(RegistroDTO dto)
        {
            var comensal = new Comensal
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim(),
                FechaNacimiento = dto.FechaNacimiento!.Value
            };

            var result = await _userManager.CreateAsync(comensal, dto.Password);
            if (!result.Succeeded) throw new RegistroException(result.Errors);

            return CrearRespuesta(comensal);
        }

        public async Task<AuthResponseDTO?> Login(LoginDTO dto)
        {
            // Por ahora el único tipo de usuario que inicia sesión es el comensal
            if (await _userManager.FindByNameAsync(dto.UserName) is not Comensal comensal
                || !await _userManager.CheckPasswordAsync(comensal, dto.Password))
                return null;

            if (comensal.Estado == EstadoCuenta.BLOQUEADA) throw new CuentaBloqueadaException();

            return CrearRespuesta(comensal);
        }

        public async Task<GoogleAuthResponseDTO> LoginGoogle(GoogleLoginDTO dto)
        {
            var payload = await ValidarTokenGoogle(dto.IdToken);
            var comensal = await BuscarComensalGoogle(payload);

            if (comensal == null)
                return new GoogleAuthResponseDTO
                {
                    RequiereRegistro = true,
                    NombreCompleto = payload.Name,
                    Email = payload.Email
                };

            if (comensal.Estado == EstadoCuenta.BLOQUEADA) throw new CuentaBloqueadaException();

            return new GoogleAuthResponseDTO { Sesion = CrearRespuesta(comensal) };
        }

        public async Task<AuthResponseDTO> RegistroGoogle(GoogleRegistroDTO dto)
        {
            // El token se vuelve a validar: el frontend no puede elegir el correo ni la cuenta de Google
            var payload = await ValidarTokenGoogle(dto.IdToken);

            var comensal = new Comensal
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                UserName = dto.UserName.Trim(),
                Email = payload.Email,
                EmailConfirmed = payload.EmailVerified,
                FechaNacimiento = dto.FechaNacimiento!.Value
            };

            // Sin contraseña: este usuario solo entra con Google
            var result = await _userManager.CreateAsync(comensal);
            if (!result.Succeeded) throw new RegistroException(result.Errors);

            var vinculo = await _userManager.AddLoginAsync(comensal, LoginGoogleInfo(payload));
            if (!vinculo.Succeeded)
            {
                await _userManager.DeleteAsync(comensal);
                throw new RegistroException(vinculo.Errors);
            }

            return CrearRespuesta(comensal);
        }

        private async Task<GoogleJsonWebSignature.Payload> ValidarTokenGoogle(string idToken)
        {
            var clientId = _config["Google:ClientId"];
            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException("Falta la configuración Google:ClientId.");

            try
            {
                // Verifica firma, expiración, emisor y que el token sea para nuestra app
                return await GoogleJsonWebSignature.ValidateAsync(idToken,
                    new GoogleJsonWebSignature.ValidationSettings { Audience = [clientId] });
            }
            // InvalidJwtException si la firma o los datos no son válidos, y errores de lectura
            // (JSON, Base64) si el token está mal formado. Los fallos de red al bajar los
            // certificados de Google no son culpa del token, así que esos sí se propagan.
            catch (Exception ex) when (ex is not HttpRequestException and not TaskCanceledException)
            {
                throw new GoogleTokenInvalidoException();
            }
        }

        private async Task<Comensal?> BuscarComensalGoogle(GoogleJsonWebSignature.Payload payload)
        {
            if (await _userManager.FindByLoginAsync("Google", payload.Subject) is Comensal vinculado)
                return vinculado;

            // Si ya tenía cuenta con contraseña y el mismo correo verificado por Google, se vinculan
            if (payload.EmailVerified && await _userManager.FindByEmailAsync(payload.Email) is Comensal existente)
            {
                var vinculo = await _userManager.AddLoginAsync(existente, LoginGoogleInfo(payload));
                if (vinculo.Succeeded) return existente;
            }

            return null;
        }

        private static UserLoginInfo LoginGoogleInfo(GoogleJsonWebSignature.Payload payload) =>
            new("Google", payload.Subject, "Google");

        private AuthResponseDTO CrearRespuesta(Comensal comensal) => new()
        {
            Token = GenerarToken(comensal),
            Usuario = ComensalDTO.DesdeEntidad(comensal)
        };

        private string GenerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new SecurityTokenDescriptor
            {
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                    new Claim(ClaimTypes.Name, usuario.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ]),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(token);
        }
    }
}
