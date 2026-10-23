namespace Forkly.Api.Models.Configuracion
{
    // Sección "Email" de appsettings.json. La contraseña va en user-secrets:
    // dotnet user-secrets set "Email:Contrasena" "..."
    public class EmailOptions
    {
        public const string Seccion = "Email";

        // Si Host está vacío, en desarrollo los correos se escriben en la consola
        public string Host { get; set; } = string.Empty;
        public int Puerto { get; set; } = 587;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Remitente { get; set; } = "Forkly <no-reply@forkly.com>";
    }
}
