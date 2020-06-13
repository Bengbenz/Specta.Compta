using System;
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Objects.Auth;

namespace Beng.Specta.Compta.Core.Entities.Auth
{
    /// <summary>
    /// This holds what modules a user or tenant has
    /// </summary>
    public class ModulesForUser
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

        [Key]
        [MaxLength(AuthConstants.UserIdSize)]
        public string UserId { get; private set; }

        public PaidForModule AllowedModules { get; private set; }
    }
}