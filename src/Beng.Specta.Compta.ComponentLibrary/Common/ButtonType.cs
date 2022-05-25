using System.ComponentModel;

namespace Beng.Specta.Compta.ComponentLibrary.Common;

public enum ButtonType
{
    [Description("a")]
    Link,

    [Description("nav-link")]
    NavLink,

    [Description("button")]
    Button,

    [Description("input")]
    Input,

    [Description("submit")]
    Submit,

    [Description("reset")]
    Reset,

    [Description("checkbox")]
    Checkbox,

    [Description("radio")]
    Radio
}