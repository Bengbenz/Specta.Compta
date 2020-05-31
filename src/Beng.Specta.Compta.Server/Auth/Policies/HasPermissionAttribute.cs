using System;

using Microsoft.AspNetCore.Authorization;

using Beng.Specta.Compta.Core.Objects.Auth;

namespace Beng.Specta.Compta.Server.Auth.Policies
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(permission.ToString())
        { }
    }
}