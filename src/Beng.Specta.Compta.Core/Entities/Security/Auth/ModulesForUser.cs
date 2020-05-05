// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Entities.Security.Permissions;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Entities.Security.Auth
{
    /// <summary>
    /// This holds what modules a user or tenant has
    /// </summary>
    public class ModulesForUser : BaseEntity
    {
        private ModulesForUser() { } //needed by EF Core

        /// <summary>
        /// This links modules to a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allowedPaidForModules"></param>
        public ModulesForUser(string userId, PaidForModule allowedPaidForModules)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            AllowedModules = allowedPaidForModules;
        }

        [Key] // TODO: Unique instead of Key
        [MaxLength(AuthConstants.UserIdSize)]
        public string UserId { get; private set; }

        public PaidForModule AllowedModules { get; private set; }
    }
}