using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.Dtos.Identities
{
    public class SignInUserInfoDto : UserInfoDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
