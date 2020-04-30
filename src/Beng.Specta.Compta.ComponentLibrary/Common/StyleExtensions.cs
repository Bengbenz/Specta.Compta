using System.Globalization;

using Dawn;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public static class StyleExtensions
    {
        public static string ToPixelValue(this string value)
        {
            Guard.Argument(value, nameof(value)).NotNull();

            return (double.TryParse(value, out var pixelValue)) ? $"{pixelValue.ToString("0.#", CultureInfo.InvariantCulture)}px" : value;
        }
    }
}