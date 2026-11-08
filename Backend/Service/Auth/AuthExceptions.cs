using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Service.Auth
{
    public class RegistroException : Exception
    {
        public IEnumerable<string> Errores { get; }

        public RegistroException(IEnumerable<IdentityError> errores) : base("No se pudo crear la cuenta.")
        {
            Errores = errores.Select(e => e.Description).ToList();
        }
    }

    // Bloqueo temporal por intentos fallidos (H01.04).
    public class AccesoBloqueadoException : Exception
    {
        public AccesoBloqueadoException(int minutosRestantes) : base(
            $"Por seguridad bloqueamos el acceso durante {(int)PoliticaAcceso.DuracionBloqueo.TotalMinutes} minutos " +
            $"tras {PoliticaAcceso.IntentosPermitidos} intentos fallidos. Podrás intentarlo de nuevo en " +
            $"{minutosRestantes} {(minutosRestantes == 1 ? "minuto" : "minutos")}.") { }
    }
}
