using Forkly.Api.Models.DTOs.Auth;

namespace Forkly.Api.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Registro(RegistroDTO dto);
        Task<AuthResponseDTO?> Login(LoginDTO dto);
        Task<AuthResponseDTO?> RenovarSesion(string usuarioId);
        Task<GoogleAuthResponseDTO> LoginGoogle(GoogleLoginDTO dto);
        Task<AuthResponseDTO> RegistroGoogle(GoogleRegistroDTO dto);
    }
}
