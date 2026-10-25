using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Recuperacion;
using Forkly.Api.Service.Recuperacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Forkly.Api.Controllers
{
    // Recuperar la contraseña (H01.06). No requiere sesión: el usuario la olvidó.
    // Un enlace inválido, usado o vencido responde 410 para que el frontend
    // ofrezca solicitar uno nuevo.
    [ApiController]
    [Route("api/[controller]")]
    public class RecuperacionController : ControllerBase
    {
        private readonly IRecuperacionService _recuperacionService;

        public RecuperacionController(IRecuperacionService recuperacionService)
        {
            _recuperacionService = recuperacionService;
        }

        // Siempre 204, exista o no el correo
        [HttpPost("solicitar")]
        [EnableRateLimiting("solicitudRecuperacion")]
        public async Task<IActionResult> Solicitar([FromBody] SolicitarRecuperacionDTO dto)
        {
            await _recuperacionService.SolicitarRecuperacion(dto.Email);
            return NoContent();
        }

        [HttpPost("validar")]
        public async Task<IActionResult> Validar([FromBody] EnlaceRecuperacionDTO dto)
        {
            try
            {
                await _recuperacionService.ValidarEnlace(dto.UsuarioId, dto.Token);
                return NoContent();
            }
            catch (EnlaceInvalidoException ex)
            {
                return StatusCode(StatusCodes.Status410Gone, new { message = ex.Message });
            }
        }

        [HttpPost("restablecer")]
        public async Task<IActionResult> Restablecer([FromBody] RestablecerContrasenaDTO dto)
        {
            try
            {
                await _recuperacionService.RestablecerContrasena(dto.UsuarioId, dto.Token, dto.NuevaContrasena);
                return NoContent();
            }
            catch (EnlaceInvalidoException ex)
            {
                return StatusCode(StatusCodes.Status410Gone, new { message = ex.Message });
            }
            catch (ContrasenaInvalidaException ex)
            {
                return BadRequest(new { message = ex.Message, errores = ex.Errores });
            }
        }
    }
}
