using System.Globalization;

namespace Beng.Specta.Compta.ComponentLibrary.Common;

public static class StyleExtensions
{
    public static string ToPixelValue(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return double.TryParse(value, out var pixelValue) ? $"{pixelValue.ToString("0.#", CultureInfo.InvariantCulture)}px" : value;
    }
}