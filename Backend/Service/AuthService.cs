using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Users;
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
