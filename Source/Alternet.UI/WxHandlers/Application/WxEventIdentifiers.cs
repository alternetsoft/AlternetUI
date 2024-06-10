using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal enum WxEventIdentifiers
    {
        None = -1,

        MouseMove = 0,
        MouseWheel = 1,

        MouseDoubleClickLeft = 2,
        MouseDoubleClickMiddle = 3,
        MouseDoubleClickRight = 4,
        MouseDoubleClickXButton1 = 5,
        MouseDoubleClickXButton2 = 6,

        MouseDownLeft = 7,
        MouseDownMiddle = 8,
        MouseDownRight = 9,
        MouseDownXButton1 = 10,
        MouseDownXButton2 = 11,

        MouseUpLeft = 12,
        MouseUpMiddle = 13,
        MouseUpRight = 14,
        MouseUpXButton1 = 15,
        MouseUpXButton2 = 16,

        Char = 17,
        CharHook = 18,
        KeyDown  =19,
        KeyUp = 20,

        Max = KeyUp,
    }
}
