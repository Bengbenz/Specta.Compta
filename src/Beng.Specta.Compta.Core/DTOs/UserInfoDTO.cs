using System;
using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs
{
    public class UserInfoDTO : BaseDTO
    {
        public string UserId { get; private set; }

        [Required]
        public string UserName { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }

        public string RoleNames { get; set; }

        public string PackedPermissions { get; set; }

        public UserInfoDTO ()
        {
        }

        public UserInfoDTO(string userName, bool isAuthenticated)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            IsAuthenticated = isAuthenticated;
        }
    }
}
