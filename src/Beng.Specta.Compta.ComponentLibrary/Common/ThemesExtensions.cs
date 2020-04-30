using Dawn;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public static class ThemesExtensions
    {
        // Default color key
        private static string ColorThemeDefault => ThemeDictionary.PrimaryKey;

        // Default color value
        private static string ColorDefault => "#000000";

        // This allows a multitude of defaults.
        // theme color => color => theme default => hard default
        // customColor : Selected Color key or value
        public static string ComputeColor(this ThemeDictionary themeStore, string customColor = "")
        {
            Guard.Argument(themeStore, nameof(themeStore)).NotNull();

            if (!string.IsNullOrWhiteSpace(customColor)
                && themeStore.TryGetValue(customColor, out var color))
            {
                return color;
            }

            if (!string.IsNullOrWhiteSpace(customColor))
            {
                return customColor;
            }
        
            if (themeStore.TryGetValue(ColorThemeDefault, out var defaultThemeColor))
            {
                return defaultThemeColor;
            }
            return ColorDefault;
        }
    }
}