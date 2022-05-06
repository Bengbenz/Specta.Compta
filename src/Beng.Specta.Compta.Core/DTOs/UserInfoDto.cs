using System;
using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs
{
    [Serializable]
    public class UserInfoDto
    {
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool HasPassword { get; set; }
        public string RoleNames { get; set; } = string.Empty;
        public string PackedPermissions { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"UserName: {UserName} - Email: {Email} - IsAuthenticated: {IsAuthenticated} - IsEmailConfirmed: {IsEmailConfirmed} - HasPassword: {HasPassword} - RoleNames: {RoleNames} - PackedPermissions: {PackedPermissions}";
        }
    }
}
