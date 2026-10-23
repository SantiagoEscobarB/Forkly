using Forkly.Api.Models.Email;

namespace Forkly.Api.Interface
{
    public interface IEmailService
    {
        Task Enviar(EmailMensaje mensaje);
    }
}
