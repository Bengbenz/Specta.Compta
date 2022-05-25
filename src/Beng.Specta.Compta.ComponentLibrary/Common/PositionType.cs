using System.ComponentModel;

namespace Beng.Specta.Compta.ComponentLibrary.Common;

public enum PositionType
{
    [Description("top")]
    Top,

    [Description("top-start")]
    TopStart,

    [Description("top-end")]
    TopEnd,

    [Description("right")]
    Right,
        
    [Description("right-start")]
    RightStart,

    [Description("right-end")]
    RightEnd,

    [Description("bottom")]
    Bottom,

    [Description("bottom-start")]
    BottomStart,

    [Description("bottom-end")]
    BottomEnd,

    [Description("left")]
    Left,

    [Description("left-start")]
    LeftStart,

    [Description("left-end")]
    LeftEnd,

    [Description("auto")]
    Auto,

    [Description("auto-start")]
    AutoStart,

    [Description("auto-end")]
    AutoEnd,
}