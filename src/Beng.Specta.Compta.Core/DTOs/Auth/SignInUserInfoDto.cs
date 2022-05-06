using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs.Auth
{
    public class SignInUserInfoDto : UserInfoDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
