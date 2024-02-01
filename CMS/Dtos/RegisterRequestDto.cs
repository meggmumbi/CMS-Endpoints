using System.ComponentModel.DataAnnotations;

namespace CMS.Dtos
{
    public class RegisterRequestDto
    {
        [MinLength(Consts.UsernameMinLength, ErrorMessage = Consts.UsernameLengthValidationError)]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = Consts.EmailValidationError)]
        public string? Email { get; set; }

        [RegularExpression(Consts.PasswordRegex, ErrorMessage = Consts.PasswordValidationError)]
        public string? Password { get; set; }
    }
}

