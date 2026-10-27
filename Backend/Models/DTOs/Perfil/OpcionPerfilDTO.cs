using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Perfil;

namespace Forkly.Api.Models.DTOs.Perfil
{
    // Lo que el frontend recibe de cada opción del catálogo (Frontend/src/types/perfil.ts)
    public class OpcionPerfilDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Detalle { get; set; }
        public CategoriaOpcion Categoria { get; set; }

        public static OpcionPerfilDTO DesdeEntidad(OpcionPerfil opcion) => new()
        {
            Codigo = opcion.Codigo,
            Nombre = opcion.Nombre,
            Detalle = opcion.Detalle,
            Categoria = opcion.Categoria
        };
    }
}
