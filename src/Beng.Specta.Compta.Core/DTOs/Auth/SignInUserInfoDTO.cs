using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs.Auth
{
    public class SignInUserInfoDTO : UserInfoDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
