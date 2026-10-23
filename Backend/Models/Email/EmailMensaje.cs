namespace Forkly.Api.Models.Email
{
    // Html para los clientes de correo y Texto como alternativa (y para la consola en desarrollo)
    public record EmailMensaje(string Destinatario, string Asunto, string Html, string Texto);
}
