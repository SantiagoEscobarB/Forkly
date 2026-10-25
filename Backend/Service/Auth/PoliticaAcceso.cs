namespace Forkly.Api.Service.Auth
{
    // Valores que fijan los criterios de aceptación de H01.04 (Iniciar sesión)
    // y H01.06 (Recuperar la contraseña). Program.cs los usa para configurar Identity.
    public static class PoliticaAcceso
    {
        // H01.04: tras tres intentos fallidos consecutivos se bloquea el acceso durante quince minutos
        public const int IntentosPermitidos = 3;
        public static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(15);

        // H01.04: la sesión se mantiene al cerrar y volver a abrir la aplicación.
        // El frontend renueva el token cada vez que se abre (GET /api/auth/sesion).
        public static readonly TimeSpan DuracionSesion = TimeSpan.FromDays(7);

        // H01.06: el enlace de recuperación es de un solo uso y vence en treinta minutos
        public static readonly TimeSpan VigenciaEnlaceRecuperacion = TimeSpan.FromMinutes(30);
    }
}
