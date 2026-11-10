using Forkly.Api.Models.DTOs.Restaurantes;

namespace Forkly.Api.Interface
{
    public interface IRestauranteService
    {
        Task<List<RestauranteResumenDTO>> Listar();
        // null si no existe un restaurante con ese Id
        Task<FichaRestauranteDTO?> ObtenerFicha(int id);
    }
}
