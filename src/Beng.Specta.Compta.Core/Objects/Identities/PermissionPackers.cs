// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Beng.Specta.Compta.Core.Objects.Identities
{
    public static class PermissionPackers
    {
        public static string PackPermissionsIntoString(this IEnumerable<Permission> permissions)
        {
            return string.Join(string.Empty, permissions.Select(x => (char)x));
        }

        public static IEnumerable<Permission> UnpackPermissionsFromString(this string packedPermissions)
        {
            if (packedPermissions == null) throw new ArgumentNullException(nameof(packedPermissions));
            
            foreach (var character in packedPermissions)
            {
                yield return (Permission)character;
            }
        }

        public static Permission? FindPermissionViaName(this string permissionName)
        {
            return Enum.TryParse(permissionName, out Permission permission)
                ? permission
                : null;
        }

    }
}