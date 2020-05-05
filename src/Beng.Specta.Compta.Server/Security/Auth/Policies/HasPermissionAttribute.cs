// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;

using Microsoft.AspNetCore.Authorization;

using Beng.Specta.Compta.Core.Entities.Security.Permissions;

namespace Beng.Specta.Compta.Server.Security.Auth.Policies
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(permission.ToString())
        { }
    }
}