using Forkly.Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Forkly.Api.Controllers
{
    // Catálogo de restaurantes: búsqueda (FOR-62) y ficha (FOR-63).
    // Requiere el JWT: sin sesión iniciada responde 401 y no entrega datos.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RestaurantesController : ControllerBase
    {
        // Un Id del catálogo: entero positivo sin ceros a la izquierda ni otros caracteres
        private static readonly Regex FormatoId = new(@"^[1-9][0-9]{0,8}\z");

        private readonly IRestauranteService _restauranteService;

        public RestaurantesController(IRestauranteService restauranteService)
        {
            _restauranteService = restauranteService;
        }

        // FOR-62, criterio 9: Id, nombre, barrio, tipos de cocina y rango de precio de cada restaurante
        [HttpGet]
        public async Task<IActionResult> Listar() => Ok(await _restauranteService.Listar());

        // FOR-63, criterio 8: la ficha, o 404 si el restaurante no existe
        // (también cuando {id} no es un Id, por ejemplo "abc")
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(string id)
        {
            if (!FormatoId.IsMatch(id)) return NotFound();

            var ficha = await _restauranteService.ObtenerFicha(int.Parse(id));
            return ficha == null ? NotFound() : Ok(ficha);
        }
    }
}
