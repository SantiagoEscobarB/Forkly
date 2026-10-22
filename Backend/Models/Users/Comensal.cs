namespace Forkly.Api.Models.Users
{
    public class Comensal : Usuario
    {
        public bool PerfilPublico { get; set; } = true;
        //Vamos a usar 1 para un usuario normal y 2 para un usuario premium
        public int Nivel { get; set; } = 1;
    }
}
