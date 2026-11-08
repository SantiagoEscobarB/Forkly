using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Auth;
using Forkly.Api.Models.DTOs.Usuarios;
using Forkly.Api.Models.Users;
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

            return CrearRespuesta(comensal);
        }

        // Al abrir la aplicación con una sesión guardada: entrega un token nuevo y los datos
        // actuales del usuario. null si la cuenta ya no existe.
        public async Task<AuthResponseDTO?> RenovarSesion(string usuarioId)
        {
            if (await _userManager.FindByIdAsync(usuarioId) is not Comensal comensal)
                return null;

            return CrearRespuesta(comensal);
        }

        private async Task<int> MinutosDeBloqueo(Usuario usuario)
        {
            var fin = await _userManager.GetLockoutEndDateAsync(usuario);
            var restante = fin.HasValue ? fin.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;
            return Math.Max(1, (int)Math.Ceiling(restante.TotalMinutes));
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
