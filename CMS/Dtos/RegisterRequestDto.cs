using System.ComponentModel.DataAnnotations;

namespace CMS.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
