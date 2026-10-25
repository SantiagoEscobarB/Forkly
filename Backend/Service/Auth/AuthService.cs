using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Auth;
using Forkly.Api.Models.DTOs.Usuarios;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Forkly.Api.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IVerificacionService _verificacionService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
            IVerificacionService verificacionService, IConfiguration config, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _verificacionService = verificacionService;
            _config = config;
            _logger = logger;
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

            await EnviarCodigoVerificacion(comensal);
            return CrearRespuesta(comensal);
        }

        // null = credenciales incorrectas. El controlador responde el mismo mensaje sin importar
        // si falló el usuario, el correo o la contraseña (H01.04).
        public async Task<AuthResponseDTO?> Login(LoginDTO dto)
        {
            var identificador = dto.UserName.Trim();
            // Los nombres de usuario no admiten '@' (Program.cs), así que si lo trae es un correo
            var usuario = identificador.Contains('@')
                ? await _userManager.FindByEmailAsync(identificador)
                : await _userManager.FindByNameAsync(identificador);

            // Por ahora el único tipo de usuario que inicia sesión es el comensal
            if (usuario is not Comensal comensal) return null;

            // Cuenta los intentos fallidos y bloquea al llegar al límite de PoliticaAcceso.
            // Mientras dure el bloqueo se rechaza incluso la contraseña correcta.
            var resultado = await _signInManager.CheckPasswordSignInAsync(comensal, dto.Password, lockoutOnFailure: true);
            if (resultado.IsLockedOut) throw new AccesoBloqueadoException(await MinutosDeBloqueo(comensal));
            if (!resultado.Succeeded) return null;

            if (comensal.Estado == EstadoCuenta.BLOQUEADA) throw new CuentaBloqueadaException();

            return CrearRespuesta(comensal);
        }

        // Al abrir la aplicación con una sesión guardada: entrega un token nuevo y los datos
        // actuales del usuario. null si la cuenta ya no existe o está bloqueada.
        public async Task<AuthResponseDTO?> RenovarSesion(string usuarioId)
        {
            if (await _userManager.FindByIdAsync(usuarioId) is not Comensal comensal
                || comensal.Estado == EstadoCuenta.BLOQUEADA)
                return null;

            return CrearRespuesta(comensal);
        }

        private async Task<int> MinutosDeBloqueo(Usuario usuario)
        {
            var fin = await _userManager.GetLockoutEndDateAsync(usuario);
            var restante = fin.HasValue ? fin.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;
            return Math.Max(1, (int)Math.Ceiling(restante.TotalMinutes));
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
                // Si Google ya verificó el correo no hace falta pedir código
                Estado = payload.EmailVerified ? EstadoCuenta.VERIFICADA : EstadoCuenta.NO_VERIFICADA,
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

            if (comensal.Estado == EstadoCuenta.NO_VERIFICADA) await EnviarCodigoVerificacion(comensal);
            return CrearRespuesta(comensal);
        }

        // Si el correo falla, la cuenta ya quedó creada: el usuario puede pedir otro código
        // desde la página de verificación, así que no se interrumpe el registro.
        private async Task EnviarCodigoVerificacion(Comensal comensal)
        {
            try
            {
                await _verificacionService.EnviarCodigo(comensal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo enviar el código de verificación a {Email}", comensal.Email);
            }
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
                if (vinculo.Succeeded)
                {
                    // Google confirma que el correo es suyo, así que ya no necesita el código
                    if (existente.Estado == EstadoCuenta.NO_VERIFICADA)
                    {
                        existente.EmailConfirmed = true;
                        existente.Estado = EstadoCuenta.VERIFICADA;
                        await _userManager.UpdateAsync(existente);
                    }
                    return existente;
                }
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    // Program.cs lo compara en cada petición para invalidar sesiones (SelloSesion)
                    new Claim(SelloSesion.Claim, SelloSesion.Calcular(usuario.SecurityStamp))
                ]),
                Expires = DateTime.UtcNow.Add(PoliticaAcceso.DuracionSesion),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(token);
        }
    }
}
