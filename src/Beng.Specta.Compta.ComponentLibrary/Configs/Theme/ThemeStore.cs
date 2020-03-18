using System.Collections.Generic;

namespace Beng.Specta.Compta.ComponentLibrary.Configs.Theme
{
    public class ThemeStore : Dictionary<string, string>
    {
        public static readonly string PrimaryKey = "primary";
        public static readonly string SecondaryKey = "secondary";
        public static readonly string SuccessKey = "success";
        public static readonly string InfoKey = "info";
        public static readonly string DangerKey = "danger";
        public static readonly string WarningKey = "warning";
        public static readonly string GrayKey = "gray";
        public static readonly string DarkKey = "dark";

        public bool IsInverted { get; set; }
        public bool IsGradient { get; set; }
        public bool IsShadow => ShadowType.Undifined != Shadow;
        public ShadowType Shadow { get; set; }

        public readonly static ThemeStore DEFAULT = new ThemeStore(
                                                        primary: "#40e583",
                                                        secondary: "#002c85",
                                                        success: "#40e583",
                                                        info: "#2c82e0",
                                                        danger: "#e34b4a",
                                                        warning: "#ffc200",
                                                        gray: "#babfc2",
                                                        dark: "#34495e",
                                                        isInverted: false,
                                                        isGradient: true,
                                                        ShadowType.Large);

        public readonly static ThemeStore CORPORATE = new ThemeStore(
                                                        primary: "#6c7fee",
                                                        secondary: "#6e7ff1",
                                                        success: "#8ddc88",
                                                        info: "#71baff",
                                                        danger: "#ffd652",
                                                        warning: "#ffc200",
                                                        gray: "#8396a5",
                                                        dark: "#34495e",
                                                        isInverted: true,
                                                        isGradient: false,
                                                        ShadowType.Small);
        public ThemeStore(
            string primary,
            string secondary,
            string success,
            string info,
            string danger,
            string warning,
            string gray,
            string dark,
            bool isInverted,
            bool isGradient,
            ShadowType shadow = ShadowType.Undifined)
        {
            Add(PrimaryKey, primary);
            Add(SecondaryKey, secondary);
            Add(SuccessKey, success);
            Add(InfoKey, info);
            Add(DangerKey, danger);
            Add(WarningKey, warning);
            Add(GrayKey, gray);
            Add(DarkKey, dark);

            IsInverted = isInverted;
            IsGradient = isGradient;
            Shadow = shadow;
       }
    }

    public enum ShadowType
    {
        Undifined,
        Small,
        Large
    }
}