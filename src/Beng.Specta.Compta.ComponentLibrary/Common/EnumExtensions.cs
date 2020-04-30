using System.ComponentModel;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString<TEnum>(this TEnum val) where TEnum : struct
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes?.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}