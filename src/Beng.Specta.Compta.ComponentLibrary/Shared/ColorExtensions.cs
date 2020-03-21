using System;
using System.Text.RegularExpressions;

namespace Beng.Specta.Compta.ComponentLibrary.Shared
{
  public static class ColorExtensions
  {
    private static RgbType HexToRgb(this string hexIn, double opacity)
    {
        string hex = hexIn.Trim();

        var rgb = new RgbType();
        var regex = new Regex("^#?(([0-9a-zA-Z]{3}){1,3})$");
        Match match = regex.Match(hex);

        if(match != null)
        {
          rgb.Opacity = opacity;
          hex = match.Value.Trim('#');
          if (hex.Length == 6)
          {
            rgb.Red = Convert.ToInt32(hex.Substring(0, 2), 16);
            rgb.Green = Convert.ToInt32(hex.Substring(2, 2), 16);
            rgb.Blue = Convert.ToInt32(hex.Substring(4, 2), 16);
          }
          else if (hex.Length == 3)
          {
            rgb.Red = Convert.ToInt32(hex.Substring(0, 1) + hex.Substring(0, 1), 16);
            rgb.Green = Convert.ToInt32(hex.Substring(1, 1) + hex.Substring(1, 1), 16);
            rgb.Blue = Convert.ToInt32(hex.Substring(2, 1) + hex.Substring(2, 1), 16);
          }
        }

        return rgb;
    }

    private static HslType HexToHsl (this string hexIn)
    {
      /*
      * the source text is taken from here (with minor modifications):
      * https://css-tricks.com/converting-color-spaces-in-javascript/
      */
      // Convert hex to RGB first. Ignore opacity
      RgbType rgb = hexIn.HexToRgb(1);
      // Then to HSL
      rgb.ToHsl();

      var cmin = rgb.Min();
      var cmax = rgb.Max();
      var delta = cmax - cmin;

      var hls = new HslType();
      hls.Hue = ComputeH(rgb, cmin, cmax, delta);
      hls.Lightness = (cmax + cmin) / 2;
      hls.Saturation = delta == 0 ? 0 : delta / (1 - Math.Abs(2 * hls.Lightness - 1));
      hls.Saturation = Convert.ToDouble(String.Format("{0:0.0}", hls.Saturation * 100));
      hls.Lightness = Convert.ToDouble(String.Format("{0:0.0}", hls.Lightness * 100));

      hls.Round();

      return hls;
    }

    /**
    public RgbType ToRgb()
    {
        
    }*/

    private static double ComputeH(RgbType rgb, double cmin, double cmax, double delta)
    {
      double hue;

      if (delta == 0)
      { 
        hue = 0;
      }
      else if (cmax == rgb.Red)
      { 
        hue = ((rgb.Green - rgb.Blue) / delta) % 6;
      }
      else if (cmax == rgb.Green)
      {
        hue = ((rgb.Blue - rgb.Red) / delta) + 2;
      }
      else
      {
        hue = ((rgb.Red - rgb.Green) / delta) + 4;
      }

      hue = Math.Round(hue * 60);
      if (hue < 0)
      {
        hue += 360;
      }

      return hue;
    }

    private static double NormalizeValue(double value, double minValue = 0, double maxValue = 100)
    {
        if (value <= minValue) {
            return minValue;
        }

        if (value >= maxValue) {
            return maxValue;
        }

        return value;
    }

    public static string GetBoxShadowColor(this string color) => color.HexToRgb(0.6).ToString();
    public static string GetHoverColor(this string color) => color.HexToRgb(0.2).ToString();
    public static string GetFocusColor(this string color) => color.HexToRgb(0.3).ToString();
    public static (string, string) GetGradientColor(this string color)
    {
      HslType first = color.HexToHsl();
      HslType second = color.HexToHsl();

      // hue circle degrees approximation
      bool isRed = (first.Hue >= 0 && first.Hue < 45) || (first.Hue >= 285);
      bool isYellow = first.Hue >= 45 && first.Hue < 85;
      bool isGreen = first.Hue >= 85 && first.Hue < 165;
      bool isBlue = first.Hue >= 165 && first.Hue < 285;
      bool isUndersaturated = first.Saturation < 30; // i.e. too pale, gray-ish, monotone

      if (isRed)
      {
        first.Hue += 11;
        first.Saturation += 27;
        first.Lightness += 8;
      }
      else if (isYellow)
      {
        first.Hue += 3;
        first.Lightness += 9;

        second.Hue -= 2;
      }
      else if (isGreen)
      {
        first.Hue += 13;
        first.Saturation -= 5;
        first.Lightness += 7;

        second.Hue -= 3;
        second.Saturation -= 1;
        second.Lightness -= 6;
      }
      else if (isBlue)
      {
        first.Hue -= 15;
        first.Saturation += 3;
        first.Lightness += 2;
      }

      if (isUndersaturated) {
        first.Lightness += 6;
        second.Lightness -= 2;
      }

      // restrictions and validations
      if (first.Hue < 0) { first.Hue += 360; }
      if (first.Hue > 0) { Math.Round(first.Hue = first.Hue % 360); }
      if (second.Hue < 0) { second.Hue += 360; }
      if (second.Hue > 0) { Math.Round(second.Hue = second.Hue % 360); }

      first.Saturation = first.Saturation > 0 ? first.Saturation : 0;
      first.Saturation = first.Saturation < 100 ? first.Saturation : 100;
      second.Saturation = second.Saturation > 0 ? second.Saturation : 0;
      second.Saturation = second.Saturation < 100 ? second.Saturation : 100;

      first.Lightness = first.Lightness > 0 ? first.Lightness : 0;
      first.Lightness = first.Lightness < 100 ? first.Lightness : 100;
      second.Lightness = second.Lightness > 0 ? second.Lightness : 0;
      second.Lightness = second.Lightness < 100 ? second.Lightness : 100;

      return (first.ToString(), second.ToString());
    }

    public static string GetGradientBackground(this string color)
    {
      var (left, right) = color.GetGradientColor();
      return $"linear-gradient(to right, {left}, {right})";
    }

    public static HslType ColorShiftHsl(this string main, HslType offset)
    {
      var offsetCopy = new HslType(offset);

      HslType color = main.HexToHsl();
      color.Hue = NormalizeValue(color.Hue + offsetCopy.Hue, minValue: 0, maxValue: 360);
      color.Saturation = NormalizeValue(color.Saturation + offsetCopy.Saturation);
      color.Lightness = NormalizeValue(color.Lightness + offsetCopy.Lightness);

      return color;
    }
  }
}
