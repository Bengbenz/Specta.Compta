using System;

namespace Beng.Specta.Compta.ComponentLibrary.Shared
{
    public class RgbType
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Opacity { get; set; }

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
			var r = String.Format("{0:0}", Red);
			var g = String.Format("{0:0}", Green);
			var b = String.Format("{0:0}", Blue);
			var o = String.Format("{0:0.0}", Opacity);
            css += (Opacity > 0 ? "a" : "") + "(";
            css += $"{r},{g},{b}";
            css += (Opacity != 1 ? $",{o}" : "") + ")";
            return css;
        }
    }
}