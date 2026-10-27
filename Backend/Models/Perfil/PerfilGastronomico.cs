using Forkly.Api.Models.Users;

namespace Forkly.Api.Models.Perfil
{
    // Perfil gastronómico de un comensal (H01.09 y H01.10). Se crea la primera vez que
    // guarda un paso de la configuración. El estado de la configuración está en
    // Comensal.EstadoPerfil para que viaje con la sesión.
    public class PerfilGastronomico
    {
        public int Id { get; set; }
        public string ComensalId { get; set; } = string.Empty;
        public Comensal Comensal { get; set; } = null!;
        // Paso del asistente en el que quedó el usuario (1 a 4), para retomarlo (criterio 4)
        public int PasoActual { get; set; } = 1;
        public DateTime ActualizadoEn { get; set; }
        public ICollection<SeleccionPerfil> Selecciones { get; set; } = new List<SeleccionPerfil>();
    }
}
