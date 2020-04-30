using System;
using System.Collections.Generic;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public class ThemeDictionary : Dictionary<string, string>
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
        public bool IsShadow => SizeType.Undefined != ShadowSize;
        public SizeType ShadowSize { get; set; }

        public Action OnToggle { get; set; }

        public string Name { get; set; }

        public readonly static ThemeDictionary DEFAULT = new ThemeDictionary(
                                                            name: "Original",
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
                                                            SizeType.Large);

        public readonly static ThemeDictionary CORPORATE = new ThemeDictionary(
                                                            name: "Corporate",
                                                            primary: "#6c7fee",
                                                            secondary: "#6e7ff1",
                                                            success: "#8ddc88",
                                                            info: "#71baff",
                                                            danger: "#f8706d",
                                                            warning: "#ffd652",
                                                            gray: "#8396a5",
                                                            dark: "#34495e",
                                                            isInverted: true,
                                                            isGradient: false,
                                                            SizeType.Small);
        public ThemeDictionary(
            string name,
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
            SizeType shadow = SizeType.Undefined)
        {
            Add(PrimaryKey, primary);
            Add(SecondaryKey, secondary);
            Add(SuccessKey, success);
            Add(InfoKey, info);
            Add(DangerKey, danger);
            Add(WarningKey, warning);
            Add(GrayKey, gray);
            Add(DarkKey, dark);

            Name = name;
            IsInverted = isInverted;
            IsGradient = isGradient;
            ShadowSize = shadow;
       }
    }
}