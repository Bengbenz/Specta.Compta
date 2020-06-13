using System;

using Microsoft.AspNetCore.Authorization;

namespace Beng.Specta.Compta.Server.Auth.Policies
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
        }

        public string PermissionName { get; }
    }
}