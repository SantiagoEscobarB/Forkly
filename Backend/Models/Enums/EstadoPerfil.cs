namespace Forkly.Api.Models.Enums
{
    // Estado de la configuración del perfil gastronómico (H01.09).
    // Con PENDIENTE o EN_PROGRESO el frontend lleva al usuario a configurarlo;
    // con COMPLETO u OMITIDO no se lo vuelve a pedir.
    public enum EstadoPerfil
    {
        PENDIENTE,
        EN_PROGRESO,
        COMPLETO,
        OMITIDO
    }
}
