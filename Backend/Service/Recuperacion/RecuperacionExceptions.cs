using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Service.Recuperacion
{
    // El enlace no existe, ya se usó o venció. No se distingue entre los casos.
    public class EnlaceInvalidoException : Exception
    {
        public EnlaceInvalidoException() : base("Este enlace ya se usó o venció. Solicita uno nuevo.") { }
    }

    public class ContrasenaInvalidaException : Exception
    {
        public IEnumerable<string> Errores { get; }

        public ContrasenaInvalidaException(IEnumerable<IdentityError> errores) : base("La contraseña no cumple los requisitos.")
        {
            Errores = errores.Select(e => e.Description).ToList();
        }
    }
}
