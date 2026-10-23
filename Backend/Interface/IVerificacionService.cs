using Forkly.Api.Models.DTOs.Usuarios;
using Forkly.Api.Models.Users;

namespace Forkly.Api.Interface
{
    public interface IVerificacionService
    {
        Task EnviarCodigo(Usuario usuario);
        Task ReenviarCodigo(string usuarioId);
        Task<ComensalDTO> Verificar(string usuarioId, string codigo);
    }
}
