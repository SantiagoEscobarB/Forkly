using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Verificacion;
using Forkly.Api.Service.Verificacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Forkly.Api.Controllers
{
    // Requiere el JWT que se recibe al registrarse o iniciar sesión
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VerificacionController : ControllerBase
    {
        private readonly IVerificacionService _verificacionService;

        public VerificacionController(IVerificacionService verificacionService)
        {
            _verificacionService = verificacionService;
        }

        private string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost("confirmar")]
        public async Task<IActionResult> Confirmar([FromBody] VerificarCorreoDTO dto)
        {
            try
            {
                return Ok(await _verificacionService.Verificar(UsuarioId, dto.Codigo));
            }
            catch (CodigoInvalidoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (CorreoYaVerificadoException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DemasiadosIntentosException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, new { message = ex.Message });
            }
        }

        [HttpPost("reenviar")]
        [EnableRateLimiting("reenvioCodigo")]
        public async Task<IActionResult> Reenviar()
        {
            try
            {
                await _verificacionService.ReenviarCodigo(UsuarioId);
                return NoContent();
            }
            catch (CorreoYaVerificadoException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
