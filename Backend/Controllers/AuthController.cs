using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs;
using Forkly.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Forkly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDTO dto)
        {
            try
            {
                return Ok(await _authService.Registro(dto));
            }
            catch (RegistroException ex)
            {
                return BadRequest(new { message = ex.Message, errores = ex.Errores });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var result = await _authService.Login(dto);
                if (result == null) return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
                return Ok(result);
            }
            catch (CuentaBloqueadaException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginDTO dto)
        {
            try
            {
                return Ok(await _authService.LoginGoogle(dto));
            }
            catch (GoogleTokenInvalidoException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (CuentaBloqueadaException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        [HttpPost("google/registro")]
        public async Task<IActionResult> RegistroGoogle([FromBody] GoogleRegistroDTO dto)
        {
            try
            {
                return Ok(await _authService.RegistroGoogle(dto));
            }
            catch (GoogleTokenInvalidoException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (RegistroException ex)
            {
                return BadRequest(new { message = ex.Message, errores = ex.Errores });
            }
        }
    }
}
