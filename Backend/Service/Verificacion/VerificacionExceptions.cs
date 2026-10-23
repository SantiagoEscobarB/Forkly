namespace Forkly.Api.Service.Verificacion
{
    public class CodigoInvalidoException : Exception
    {
        public CodigoInvalidoException() : base("El código es incorrecto o ya venció.") { }
    }

    public class DemasiadosIntentosException : Exception
    {
        public DemasiadosIntentosException() : base("Demasiados intentos fallidos. Espera unos minutos e intenta de nuevo.") { }
    }

    public class CorreoYaVerificadoException : Exception
    {
        public CorreoYaVerificadoException() : base("Tu correo ya está verificado.") { }
    }
}
