using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Objects.Identities;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Entities.Identities;

/// <summary>
/// This holds what modules a user or tenant has
/// </summary>
public sealed class ModulesForUser : BaseEntity
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
    [MaxLength(IdentityConstants.UserIdSize)]
    public string UserId { get; private set; } = null!;

    public PaidForModule AllowedModules { get; private set; }
}