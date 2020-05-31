using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Beng.Specta.Compta.Server.Objects
{
    public static class ModelStateDictionaryExtension
    {
        public static void AddModelErrors(
            this ModelStateDictionary modelState,
            IDictionary<string, List<string>> errors)
        {
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));
            if (errors == null) throw new ArgumentNullException(nameof(errors));

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, string.Join("\n", error.Value));
            }
        }
    }
}