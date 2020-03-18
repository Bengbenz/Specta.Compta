using System.Text;

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
        public string BoxShadow { get; set; }
        public string FontSize { get; set; }
        public string Filter { get; set; }
        public double Rotation { get; set; }

        public override string ToString()
        {
            var style = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Background))
            {
                style.Append($"background:{Background}; ");
            }
            if (!string.IsNullOrWhiteSpace(BackgroundImage))
            {
                style.Append($"background-image:{BackgroundImage}; ");
            }
            if (!string.IsNullOrWhiteSpace(BackgroundColor))
            {
                style.Append($"background-color:{BackgroundColor}; ");
            }
            if (!string.IsNullOrWhiteSpace(Color))
            {
                style.Append($"color:{Color}; ");
            }
            if (!string.IsNullOrWhiteSpace(BorderColor))
            {
                style.Append($"border-color:{BorderColor}; ");
            }
            if (!string.IsNullOrWhiteSpace(BorderTopColor))
            {
                style.Append($"border-top-color:{BorderTopColor}; ");
            }
            if (!string.IsNullOrWhiteSpace(BoxShadow))
            {
                style.Append($"box-shadow:{BoxShadow}; ");
            }
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                style.Append($"filter:{Filter}; ");
            }
            if (!string.IsNullOrWhiteSpace(FontSize))
            {
                var value = (int.TryParse(FontSize, out var fontSize)) ? $"{fontSize}px; " : $"{FontSize}; ";
                style.Append($"fontSize:{value}; ");
            }
            if (Rotation != 0)
            {
                style.Append($"transform: rotate({Rotation}deg); ");
            }

            return style.ToString();
        }
    }
}