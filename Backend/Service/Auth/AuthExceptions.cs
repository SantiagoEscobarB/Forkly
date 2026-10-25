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

    public class CuentaBloqueadaException : Exception
    {
        public CuentaBloqueadaException() : base("Tu cuenta está bloqueada.") { }
    }

    // Bloqueo temporal por intentos fallidos (H01.04). Es distinto de CuentaBloqueadaException,
    // que corresponde al estado BLOQUEADA de la cuenta.
    public class AccesoBloqueadoException : Exception
    {
        public AccesoBloqueadoException(int minutosRestantes) : base(
            $"Por seguridad bloqueamos el acceso durante {(int)PoliticaAcceso.DuracionBloqueo.TotalMinutes} minutos " +
            $"tras {PoliticaAcceso.IntentosPermitidos} intentos fallidos. Podrás intentarlo de nuevo en " +
            $"{minutosRestantes} {(minutosRestantes == 1 ? "minuto" : "minutos")}.") { }
    }

    public class GoogleTokenInvalidoException : Exception
    {
        public GoogleTokenInvalidoException() : base("No se pudo verificar tu cuenta de Google. Intenta de nuevo.") { }
    }
}
