using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Perfil;
using Forkly.Api.Service.Perfil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forkly.Api.Controllers
{
    // Perfil gastronómico (H01.09) y restricciones alimentarias (H01.10).
    // Requiere el JWT; cada usuario solo ve y modifica su propio perfil.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        private string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // Catálogo de tipos de cocina, ambientes, rangos de precio y restricciones
        [HttpGet("opciones")]
        public async Task<IActionResult> Opciones() => Ok(await _perfilService.ObtenerOpciones());

        [HttpGet]
        public async Task<IActionResult> Obtener() => Ok(await _perfilService.ObtenerPerfil(UsuarioId));

        // Avance del asistente (lo elegido y el paso actual)
        [HttpPut("avance")]
        public async Task<IActionResult> GuardarAvance([FromBody] AvancePerfilDTO dto)
        {
            try
            {
                return Ok(await _perfilService.GuardarAvance(UsuarioId, dto));
            }
            catch (PerfilInvalidoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (PerfilYaConfiguradoException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Confirmar el asistente o guardar los cambios desde los ajustes
        [HttpPut]
        public async Task<IActionResult> Guardar([FromBody] GuardarPerfilDTO dto)
        {
            try
            {
                return Ok(await _perfilService.Guardar(UsuarioId, dto));
            }
            catch (PerfilInvalidoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("omitir")]
        public async Task<IActionResult> Omitir()
        {
            try
            {
                return Ok(await _perfilService.Omitir(UsuarioId));
            }
            catch (PerfilYaConfiguradoException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
