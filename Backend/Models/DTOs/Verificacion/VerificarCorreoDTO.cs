using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Verificacion
{
    public class VerificarCorreoDTO
    {
        [Required]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "El código debe tener 6 dígitos.")]
        public string Codigo { get; set; } = string.Empty;
    }
}
