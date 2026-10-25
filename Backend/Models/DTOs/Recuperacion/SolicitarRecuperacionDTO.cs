using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs.Recuperacion
{
    public class SolicitarRecuperacionDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
