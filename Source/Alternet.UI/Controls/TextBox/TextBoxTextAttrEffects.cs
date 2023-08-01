using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    public enum TextBoxTextAttrEffects
    {
        None = 0x00000000,

        Capitals = 0x00000001,

        SmallCapitals = 0x00000002,

        Strikethrough = 0x00000004,

        DoubleStrikethrough = 0x00000008,

        Shadow = 0x00000010,

        Emboss = 0x00000020,

        Outline = 0x00000040,

        Engrave = 0x00000080,

        Superscript = 0x00000100,

        Subscript = 0x00000200,

        Rtl = 0x00000400,

        SuppressHyphenation = 0x00001000,
    }
}
