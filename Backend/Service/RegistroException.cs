using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Service
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
}
