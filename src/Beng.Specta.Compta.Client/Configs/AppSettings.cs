using System;
using Beng.Specta.Compta.ComponentLibrary.Configs.Theme;

namespace Beng.Specta.Compta.Client.Configs
{
    public class AppSettings
    {
        public bool IsInvertedColor { get; set; }

        public ThemeStore ThemeStore
        {
            get => IsInvertedColor ? ThemeStore.CORPORATE : ThemeStore.DEFAULT;
            set => ThemeStore = value;
        }

        public void ToggleInverted()
        {
            IsInvertedColor = !IsInvertedColor;
        }
    }
}