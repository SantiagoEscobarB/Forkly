using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Perfil;

namespace Forkly.Api.Models.Users
{
    public class Comensal : Usuario
    {
        public bool PerfilPublico { get; set; } = true;
        public NivelComensal Nivel { get; set; } = NivelComensal.NORMAL;
        // null = todavía no ha empezado a configurar el perfil (se trata como PENDIENTE).
        // Es nullable porque la columna se agregó con usuarios ya creados.
        public EstadoPerfil? EstadoPerfil { get; set; }
        public PerfilGastronomico? PerfilGastronomico { get; set; }
    }
}
