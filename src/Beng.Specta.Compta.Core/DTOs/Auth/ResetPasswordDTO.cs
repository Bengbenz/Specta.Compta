using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs.Auth
{
    public class ResetPasswordDTO : UserInfoDTO
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "The password should have one lower case character, one upper case character, one number, one special character and at least 8 characters.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
