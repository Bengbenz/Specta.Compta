using Beng.Specta.Compta.ComponentLibrary.Common;

namespace Beng.Specta.Compta.Client.Configs
{
    public class AppSettings
    {
        public bool IsInvertedColor { get; set; } = true;

        public ThemeDictionary SelectedTheme =>
            IsInvertedColor ? ThemeDictionary.CORPORATE : ThemeDictionary.DEFAULT;

        public void ToggleInverted() => IsInvertedColor = !IsInvertedColor;
    }
}