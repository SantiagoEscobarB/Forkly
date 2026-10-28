namespace Forkly.Api.Service.Perfil
{
    // Opciones que no existen o combinación no permitida (más de un tipo de dieta)
    public class PerfilInvalidoException : Exception
    {
        public PerfilInvalidoException(string mensaje) : base(mensaje) { }
    }

    // El asistente solo guarda avances mientras el perfil no está configurado;
    // después se edita desde los ajustes
    public class PerfilYaConfiguradoException : Exception
    {
        public PerfilYaConfiguradoException() : base("Tu perfil ya está configurado. Puedes editarlo desde los ajustes.") { }
    }
}
