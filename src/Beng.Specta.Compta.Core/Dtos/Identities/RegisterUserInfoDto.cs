using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.Dtos.Identities;

public class RegisterUserInfoDto : UserInfoDto
{
    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(
        "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$",
        ErrorMessage = "The password should have one lower case character, one upper case character, one number, one special character and at least 8 characters.")]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must read and agree with terms, or leave.")]
    public bool AgreeWithTerms { get; set; }
}