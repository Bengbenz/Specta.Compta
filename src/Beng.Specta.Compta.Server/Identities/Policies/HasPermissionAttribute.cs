using System;

using Microsoft.AspNetCore.Authorization;

using Beng.Specta.Compta.Core.Objects.Identities;

namespace Beng.Specta.Compta.Server.Identities.Policies
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(permission.ToString())
        { }
    }
}