using Beng.Specta.Compta.ComponentLibrary.Configs.Theme;

namespace Beng.Specta.Compta.Client.Configs
{
    public class AppSettings
    {
        public bool IsInvertedColor { get; set; }
        public ThemeStore ThemeStore => IsInvertedColor ? ThemeStore.CORPORATE : ThemeStore.DEFAULT;

        public void ToggleInverted()
        {
            IsInvertedColor = !IsInvertedColor;
        }
    }
}