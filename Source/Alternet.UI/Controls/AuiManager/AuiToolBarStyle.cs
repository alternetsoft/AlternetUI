using System;

namespace Alternet.UI
{
    public enum AuiToolBarStyle
    {
        Text = 1 << 0,

        NoTooltips = 1 << 1,

        NoAutoResize = 1 << 2,

        Gripper = 1 << 3,

        Overflow = 1 << 4,

        Vertical = 1 << 5,

        HorzLayout = 1 << 6,

        Horizontal = 1 << 7,

        PlainBackground = 1 << 8,

        HorzText = HorzLayout | Text,

        OrientationMask = Vertical | Horizontal,

        DefaultStyle = 0,
    }
}