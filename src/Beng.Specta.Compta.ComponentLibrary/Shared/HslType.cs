using System;

namespace Beng.Specta.Compta.ComponentLibrary.Shared
{
    public class HslType
    {
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Lightness { get; set; }

        public HslType(double h = 0, double s = 0, double l = 0)
        {
            Hue = h;
            Saturation = s;
            Lightness = l;
        }
        public HslType(HslType offset) : this(offset.Hue, offset.Saturation, offset.Lightness)
        {
        }

        public void Round()
        {
            Hue = Math.Round(Hue);
            Saturation = Math.Round(Saturation);
            Lightness = Math.Round(Lightness);
        }

        public override string ToString() => $"hsl({Hue},{Saturation}%,{Lightness}%)";
    }
}