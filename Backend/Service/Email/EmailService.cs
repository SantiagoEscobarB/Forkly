using Forkly.Api.Interface;
using Forkly.Api.Models.Configuracion;
using Forkly.Api.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Forkly.Api.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _opciones;
        private readonly IHostEnvironment _entorno;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailOptions> opciones, IHostEnvironment entorno, ILogger<EmailService> logger)
        {
            _opciones = opciones.Value;
            _entorno = entorno;
            _logger = logger;
        }

        public async Task Enviar(EmailMensaje mensaje)
        {
            if (string.IsNullOrWhiteSpace(_opciones.Host))
            {
                // Solo en desarrollo: permite probar el flujo sin un servidor SMTP
                if (!_entorno.IsDevelopment())
                    throw new InvalidOperationException("Falta la configuración Email:Host.");

                _logger.LogWarning("SMTP sin configurar. Correo para {Destinatario} | {Asunto}\n{Texto}",
                    mensaje.Destinatario, mensaje.Asunto, mensaje.Texto);
                return;
            }

            var correo = new MimeMessage();
            correo.From.Add(MailboxAddress.Parse(_opciones.Remitente));
            correo.To.Add(MailboxAddress.Parse(mensaje.Destinatario));
            correo.Subject = mensaje.Asunto;
            correo.Body = new BodyBuilder { HtmlBody = mensaje.Html, TextBody = mensaje.Texto }.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_opciones.Host, _opciones.Puerto, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_opciones.Usuario, _opciones.Contrasena);
            await smtp.SendAsync(correo);
            await smtp.DisconnectAsync(true);
        }
    }
}
