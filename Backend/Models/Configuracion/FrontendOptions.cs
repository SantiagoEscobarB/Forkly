namespace Forkly.Api.Models.Configuracion
{
    // Sección "Frontend" de appsettings.json. Se usa para armar los enlaces que van en los
    // correos (p. ej. el de recuperar la contraseña). No se toma del encabezado de la
    // petición para que nadie pueda hacer que el correo apunte a otro sitio.
    public class FrontendOptions
    {
        public const string Seccion = "Frontend";

        public string Url { get; set; } = "http://localhost:5173";
    }
}
