namespace Forkly.Api.Models.Perfil
{
    // Una opción elegida por el usuario. Las preferencias (COCINA, AMBIENTE, PRECIO) guardan
    // su peso en el vector de preferencias; las opciones no elegidas pesan 0 y no se guardan.
    public class SeleccionPerfil
    {
        public int PerfilGastronomicoId { get; set; }
        public PerfilGastronomico PerfilGastronomico { get; set; } = null!;
        public int OpcionPerfilId { get; set; }
        public OpcionPerfil OpcionPerfil { get; set; } = null!;
        // null en las restricciones y mientras el perfil no se ha confirmado
        public double? Peso { get; set; }
    }
}
