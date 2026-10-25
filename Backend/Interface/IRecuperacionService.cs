namespace Forkly.Api.Interface
{
    public interface IRecuperacionService
    {
        Task SolicitarRecuperacion(string email);
        Task ValidarEnlace(string usuarioId, string token);
        Task RestablecerContrasena(string usuarioId, string token, string nuevaContrasena);
    }
}
