namespace Forkly.Api.Models.Enums
{
    // Categorías de las opciones del perfil gastronómico.
    // COCINA, AMBIENTE y PRECIO son preferencias y forman el vector de preferencias (H01.09).
    // DIETA y RESTRICCION son restricciones alimentarias (H01.10): no tienen peso.
    public enum CategoriaOpcion
    {
        COCINA,
        AMBIENTE,
        PRECIO,
        DIETA,
        RESTRICCION
    }
}
