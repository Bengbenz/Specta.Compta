namespace Beng.Specta.Compta.ComponentLibrary.Common;

public class HslType
{
    public double Hue { get; set; }
    public double Saturation { get; set; }
    public double Lightness { get; set; }

    public HslType()
    {
    }

    public HslType(HslType offset)
    {
        if (offset == null) throw new ArgumentNullException(nameof(offset));

        Hue = offset.Hue;
        Saturation = offset.Saturation;
        Lightness = offset.Lightness;
    }

    public void Round()
    {
        Hue = Math.Round(Hue);
        Saturation = Math.Round(Saturation);
        Lightness = Math.Round(Lightness);
    }

    public override string ToString() => $"hsl({Hue},{Saturation}%,{Lightness}%)";
}