using Forkly.Api.Models.Enums;

namespace Forkly.Api.Models.Users
{
    public class Comensal : Usuario
    {
        public bool PerfilPublico { get; set; } = true;
        public NivelComensal Nivel { get; set; } = NivelComensal.NORMAL;
    }
}
