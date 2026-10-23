using Forkly.Api.Models.Email;
using System.Net;

namespace Forkly.Api.Service.Email
{
    // Contenido de los correos que envía Forkly. Los colores son los del frontend (index.css).
    public static class PlantillasEmail
    {
        public static EmailMensaje CodigoVerificacion(string destinatario, string nombre, string codigo)
        {
            var nombreSeguro = WebUtility.HtmlEncode(nombre);

            var html = $"""
                <div style="background:#140C07;padding:32px;font-family:Arial,sans-serif;color:#ffffff">
                  <div style="max-width:480px;margin:0 auto;background:#2A1A0E;border-radius:24px;padding:32px">
                    <h1 style="margin:0 0 8px;color:#E07A1F">FORKLY</h1>
                    <p style="margin:0 0 24px;color:#A88B6E">Hola, {nombreSeguro}. Este es tu código para verificar tu correo:</p>
                    <p style="margin:0 0 24px;font-size:36px;font-weight:bold;letter-spacing:12px;text-align:center">{codigo}</p>
                    <p style="margin:0;color:#A88B6E;font-size:14px">El código vence en unos minutos. Si no creaste una cuenta en Forkly, ignora este correo.</p>
                  </div>
                </div>
                """;

            var texto = $"Hola, {nombre}. Tu código de verificación de Forkly es: {codigo}. " +
                        "Vence en unos minutos. Si no creaste una cuenta en Forkly, ignora este correo.";

            return new EmailMensaje(destinatario, "Tu código de verificación de Forkly", html, texto);
        }
    }
}
