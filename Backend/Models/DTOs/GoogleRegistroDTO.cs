using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs
{
    // Datos que Google no entrega y que Usuario necesita
    public class GoogleRegistroDTO : IValidatableObject
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public DateOnly? FechaNacimiento { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
                yield return new ValidationResult("La fecha de nacimiento no puede ser futura.", [nameof(FechaNacimiento)]);
        }
    }
}
