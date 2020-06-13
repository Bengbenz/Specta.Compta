using System.ComponentModel;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public enum TriggerType
    {
        [Description("none")]
        None,

        [Description("click")]
        Click,

        [Description("hover")]
        Hover,

        [Description("mouseenter focus")]
        Mouseenter // use for tippy
    }
}