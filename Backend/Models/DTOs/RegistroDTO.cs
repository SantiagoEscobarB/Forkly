using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs
{
    public class RegistroDTO : IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        // Nullable para que [Required] rechace el campo ausente en vez de recibir 0001-01-01
        [Required]
        public DateOnly? FechaNacimiento { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
                yield return new ValidationResult("La fecha de nacimiento no puede ser futura.", [nameof(FechaNacimiento)]);
        }
    }
}
