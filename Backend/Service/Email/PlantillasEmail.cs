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

        public static EmailMensaje RecuperarContrasena(string destinatario, string nombre, string enlace, int minutosVigencia)
        {
            var nombreSeguro = WebUtility.HtmlEncode(nombre);
            var enlaceSeguro = WebUtility.HtmlEncode(enlace);

            var html = $"""
                <div style="background:#140C07;padding:32px;font-family:Arial,sans-serif;color:#ffffff">
                  <div style="max-width:480px;margin:0 auto;background:#2A1A0E;border-radius:24px;padding:32px">
                    <h1 style="margin:0 0 8px;color:#E07A1F">FORKLY</h1>
                    <p style="margin:0 0 24px;color:#A88B6E">Hola, {nombreSeguro}. Recibimos una solicitud para restablecer la contraseña de tu cuenta.</p>
                    <p style="margin:0 0 24px;text-align:center">
                      <a href="{enlaceSeguro}" style="display:inline-block;background:#E07A1F;color:#140C07;font-weight:bold;text-decoration:none;padding:14px 28px;border-radius:999px">Crear contraseña nueva</a>
                    </p>
                    <p style="margin:0;color:#A88B6E;font-size:14px">El enlace sirve una sola vez y vence en {minutosVigencia} minutos. Si no pediste este cambio, ignora este correo: tu contraseña seguirá igual.</p>
                  </div>
                </div>
                """;

            var texto = $"Hola, {nombre}. Para crear una contraseña nueva en Forkly abre este enlace: {enlace} . " +
                        $"Sirve una sola vez y vence en {minutosVigencia} minutos. Si no pediste este cambio, ignora este correo.";

            return new EmailMensaje(destinatario, "Restablece tu contraseña de Forkly", html, texto);
        }
    }
}
