using System.Text;
using System.Globalization;

namespace Beng.Specta.Compta.ComponentLibrary.Shared
{
    public class StyleType 
    {
        public string Background { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundColor { get; set; }
        public string Color { get; set; } 
        public string BorderColor { get; set; } 
        public string BorderTopColor { get; set; } 
        public string BorderLeftColor { get; set; } 
        public string BorderWidth { get; set; } 
        public string BorderTopWidth { get; set; } 
        public string BorderLeftWidth { get; set; } 
        public string BoxShadow { get; set; }
        public string FontSize { get; set; }
        public string Filter { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Padding { get; set; }
        public double Rotation { get; set; }
        public double? AnimationDuration { get; set; }
        public double? AnimationDelay { get; set; }
        public bool IsDisabled { get; set; }

        public StyleType()
        {
        }

        public StyleType(StyleType style)
        {
            Background = style.Background;
            BackgroundImage = style.BackgroundImage;
            BackgroundColor = style.BackgroundColor;
            Color = style.Color;
            BorderColor = style.BorderColor;
            BorderTopColor = style.BorderTopColor;
            BorderLeftColor = style.BorderLeftColor;
            BorderWidth = style.BorderWidth;
            BorderTopWidth = style.BorderTopWidth;
            BorderLeftWidth = style.BorderTopWidth;
            BoxShadow = style.BoxShadow;
            FontSize = style.FontSize;
            Filter = style.Filter;
            Rotation = style.Rotation;
            IsDisabled = style.IsDisabled;
            AnimationDuration = style.AnimationDuration;
            AnimationDelay = style.AnimationDelay;
            Height = style.Height;
            Width = style.Width;
            Padding = style.Padding;
            Left = style.Left;
            Top = style.Top;
        }

        private string ParsePixelValue(string value) => (double.TryParse(value, out var pixelValue)) ? $"{pixelValue.ToString("0.#", CultureInfo.InvariantCulture)}px" : value;

        public override string ToString()
        {
            var style = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Background))
            {
                style.Append($" background: {Background};");
            }
            if (!string.IsNullOrWhiteSpace(BackgroundImage))
            {
                style.Append($" background-image: {BackgroundImage};");
            }
            if (!string.IsNullOrWhiteSpace(BackgroundColor))
            {
                style.Append($" background-color: {BackgroundColor};");
            }
            if (!string.IsNullOrWhiteSpace(Color))
            {
                style.Append($" color: {Color};");
            }
            if (!string.IsNullOrWhiteSpace(BorderColor))
            {
                style.Append($" border-color: {BorderColor};");
            }
            if (!string.IsNullOrWhiteSpace(BorderTopColor))
            {
                style.Append($" border-top-color: {BorderTopColor};");
            }
            if (!string.IsNullOrWhiteSpace(BorderLeftColor))
            {
                style.Append($" border-left-color: {BorderLeftColor};");
            }
            if (!string.IsNullOrWhiteSpace(BorderWidth))
            {
                style.Append($" border-width: {ParsePixelValue(BorderWidth)};");
            }
            if (!string.IsNullOrWhiteSpace(BorderTopWidth))
            {
                style.Append($" border-top-width: {ParsePixelValue(BorderTopWidth)};");
            }
            if (!string.IsNullOrWhiteSpace(BorderLeftWidth))
            {
                style.Append($" border-left-width: {ParsePixelValue(BorderLeftWidth)};");
            }
            if (!string.IsNullOrWhiteSpace(BoxShadow))
            {
                style.Append($" box-shadow: {BoxShadow};");
            }
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                style.Append($" filter: {Filter};");
            }
            if (!string.IsNullOrWhiteSpace(FontSize))
            {
                style.Append($" font-size: {ParsePixelValue(FontSize)};");
            }
            if (Rotation != 0)
            {
                style.Append($" transform: rotate({Rotation}deg);");
            }
            if (AnimationDuration != null)
            {
                style.Append($" animation-duration: {AnimationDuration}ms;");
            }
            if (AnimationDelay != null)
            {
                style.Append($" animation-delay: {AnimationDelay}ms;");
            }
            if (IsDisabled)
            {
                style.Append($" display: none;");
            }
            if (!string.IsNullOrWhiteSpace(Height))
            {
                style.Append($" height: {ParsePixelValue(Height)};");
            }
            if (!string.IsNullOrWhiteSpace(Width))
            {
                style.Append($" width: {ParsePixelValue(Width)};");
            }
            if (!string.IsNullOrWhiteSpace(Padding))
            {
                style.Append($" padding: {ParsePixelValue(Padding)};");
            }
            if (!string.IsNullOrWhiteSpace(Top))
            {
                style.Append($" top: {ParsePixelValue(Top)};");
            }
            if (!string.IsNullOrWhiteSpace(Left))
            {
                style.Append($" left: {ParsePixelValue(Left)};");
            }

            return style.ToString().Trim();
        }
    }
}