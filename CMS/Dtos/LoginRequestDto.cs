using System.ComponentModel.DataAnnotations;

namespace CMS.Dtos
{
    public class LoginRequestDto
    {
        [MinLength(Consts.UsernameMinLength, ErrorMessage = Consts.UsernameLengthValidationError)]
        public string? Username { get; set; }

        [RegularExpression(Consts.PasswordRegex, ErrorMessage = Consts.PasswordValidationError)]
        public string? Password { get; set; }
    }
}
