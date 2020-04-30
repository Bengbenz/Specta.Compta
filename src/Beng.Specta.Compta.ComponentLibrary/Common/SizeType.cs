using System.ComponentModel;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public enum SizeType
    {
        [Description("undefined")]
        Undefined,

        [Description("xs")]
        ExtraSmall,

        [Description("sm")]
        Small,

        [Description("md")]
        Medium,

        [Description("lg")]
        Large,
        
        [Description("xl")]
        ExtraLarge
    }
}