using Forkly.Api.Models.DTOs.Perfil;

namespace Forkly.Api.Interface
{
    public interface IPerfilService
    {
        Task<List<OpcionPerfilDTO>> ObtenerOpciones();
        Task<PerfilDTO> ObtenerPerfil(string usuarioId);
        Task<PerfilDTO> GuardarAvance(string usuarioId, AvancePerfilDTO dto);
        Task<PerfilDTO> Guardar(string usuarioId, GuardarPerfilDTO dto);
        Task<PerfilDTO> Omitir(string usuarioId);
    }
}
