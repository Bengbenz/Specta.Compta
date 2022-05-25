using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.Dtos.Identities;

public class ResetPasswordDto : UserInfoDto
{
    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(
        "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$",
        ErrorMessage = "The password should have one lower case character, one upper case character, one number, one special character and at least 8 characters.")]
    public string NewPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }  = null!;

    public string? Code { get; set; }
}