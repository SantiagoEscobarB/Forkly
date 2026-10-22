using System.ComponentModel.DataAnnotations;

namespace Forkly.Api.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
