﻿using Microsoft.AspNetCore.Identity;

namespace Beng.Specta.Compta.Server.Identities.Services;

public static class AspNetUserExtension
{
    /// <summary>
    /// This will add a user with the given email if they don't all ready exist
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public static async Task<IdentityUser> AddNewUserAsync(this UserManager<IdentityUser> userManager,
        string email,
        string password)
    {
        ArgumentNullException.ThrowIfNull(userManager);
            
        IdentityUser user = await userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            return user;
        }

        user = new IdentityUser { UserName = email, Email = email };
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return user;
        }
        var errorDescriptions = string.Join("\n", result.Errors.Select(x => x.Description));
        throw new InvalidOperationException(
            $"Tried to add user {email}, but failed. Errors:\n {errorDescriptions}");

    }

    public static IDictionary<string, List<string>> GetFormattedErrors(this IdentityResult identityResult)
    {
        // see https://github.com/aspnet/AspNetCore/blob/bfec2c14be1e65f7dd361a43950d4c848ad0cd35/src/Identity/Extensions.Core/src/IdentityErrorDescriber.cs
        // for different error codes
        var keyMapping = new Dictionary<string, string>
        {
            {"DefaultError","Name" },
            {"ConcurrencyFailure","Concurrency" },
                
            {"InvalidEmail","Email" },
            {"DuplicateEmail","Email" },

            {"InvalidUserName","UserName" },
            {"DuplicateUserName","UserName" },
            {"LoginAlreadyAssociated","UserName" },
            {"UserLockoutNotEnabled","UserName" },

            {"InvalidToken","Password" },
            {"RecoveryCodeRedemptionFailed","Password" },
            {"PasswordMismatch","Password" },
            {"PasswordTooShort","Password" },
            {"PasswordRequiresUniqueChars","Password" },
            {"PasswordRequiresNonAlphanumeric","Password" },
            {"PasswordRequiresDigit","Password" },
            {"PasswordRequiresLower","Password" },
            {"PasswordRequiresUpper","Password" },
            {"UserAlreadyHasPassword","Password" },
                
            {"InvalidRoleName","Role" },
            {"DuplicateRoleName","Role" },
            {"UserAlreadyInRole","Role" },
            {"UserNotInRole","Role" }
        };
        var formattedErrors = identityResult.Errors
            .Select(e =>
                {
                    var key = keyMapping.TryGetValue(e.Code, out var value) ? value : "Name";
                    return new { Key = key, e.Description };
                }
            ).ToLookup(e => e.Key, e => e.Description)
            .ToDictionary(l => l.Key, l => l.ToList());

        return formattedErrors;
    }
}