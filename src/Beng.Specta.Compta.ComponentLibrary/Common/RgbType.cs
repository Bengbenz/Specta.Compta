using System;
using System.Globalization;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public class RgbType
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Opacity { get; set; } = 1;

        public double Min() => Math.Min(Math.Min(Red, Green), Blue);

        public double Max() => Math.Max(Math.Max(Red, Green), Blue);

        public void ToHsl()
        {
            Red /= 255;
            Green /= 255;
            Blue /= 255;
        }

        public override string ToString()
        {
            var css = $"rgb";
			var r = Red.ToString("F0", CultureInfo.InvariantCulture);
			var g = Green.ToString("F0", CultureInfo.InvariantCulture);
			var b = Blue.ToString("F0", CultureInfo.InvariantCulture);
			var o = Opacity.ToString("F1", CultureInfo.InvariantCulture);
            css += (Opacity != 1 ? "a" : "") + "(";
            css += $"{r},{g},{b}";
            css += (Opacity != 1 ? $",{o}" : "") + ")";
            return css;
        }
    }
}