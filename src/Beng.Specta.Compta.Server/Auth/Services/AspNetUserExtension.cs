using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

[assembly: InternalsVisibleTo("Beng.Specta.Compta.FunctionalTests")]

namespace Beng.Specta.Compta.Server.Auth.Services
{
    internal static class AspNetUserExtension
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
            IdentityUser user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return user;
            }

            user = new IdentityUser { UserName = email, Email = email };
            IdentityResult result = await userManager.CreateAsync(user, password); 
            if (!result.Succeeded)
            {
                var errorDescriptions = string.Join("\n", result.Errors.Select(x => x.Description));
                throw new InvalidOperationException(
                    $"Tried to add user {email}, but failed. Errors:\n {errorDescriptions}");
            }

            return user;
        }

        public static IDictionary<string, List<string>> GetFormatedErrors(this IdentityResult identityResult)
        {
            // see https://github.com/aspnet/AspNetCore/blob/bfec2c14be1e65f7dd361a43950d4c848ad0cd35/src/Identity/Extensions.Core/src/IdentityErrorDescriber.cs
            // for diffrent error codes
            var keyMapping = new Dictionary<string, string>()
            {
                {"PasswordMismatch","Password" },
                {"InvalidUserName","Name" },
                {"InvalidEmail","Email" },
                {"DuplicateUserName","UserName" },
                {"DuplicateEmail","Email" },
                {"PasswordTooShort","Password" },
                {"PasswordRequiresUniqueChars","Password" },
                {"PasswordRequiresNonAlphanumeric","Password" },
                {"PasswordRequiresDigit","Password" },
                {"PasswordRequiresLower","Password" },
                {"PasswordRequiresUpper","Password" },

            };
            var formatedErrors = identityResult.Errors
                .Select(e =>
                {
                    var key = e.Code;
                    keyMapping.TryGetValue(key, out key);
                    return new { Key = key, e.Description };
                }
                ).ToLookup(e => e.Key, e => e.Description)
                .ToDictionary(l => l.Key, l => l.ToList());

            return formatedErrors;
        }
    }
}