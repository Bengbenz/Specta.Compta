using Beng.Specta.Compta.ComponentLibrary.Common;

namespace Beng.Specta.Compta.Client.State;

public class ThemeState
{
    private string[] BackgroundImages { get; } =
    {
        "pexels-photo.jpg",
        "pexels-photo-39811.jpeg",
        "pexels-photo-235615.jpeg",
        "pexels-photo-247431.jpeg",
        //"pexels-photo-247478.jpeg",
        "pexels-photo-257840.jpeg",
        "pexels-photo-459059.jpeg",
        //"fall-autumn-red-season.jpg",
        "profile-bg.jpg",
        "woodland-road-falling-leaf-natural-38537.jpeg"
    };

    public const string AuthBackgroundImage = "imgs/profile-bg.jpg";

    public bool IsInvertedColor { get; set; } = true;

    public event Action? OnChange;

    public ThemeDictionary SelectedTheme =>
        IsInvertedColor ? ThemeDictionary.Corporate : ThemeDictionary.Default;

    public void ToggleInverted()
    {
        IsInvertedColor = !IsInvertedColor;
        NotifyStateChanged();
    }

    public string GetRandomBackgroundImage() => $"imgs/{BackgroundImages[new Random().Next(BackgroundImages.Length)]}";

    private void NotifyStateChanged() => OnChange?.Invoke();
}