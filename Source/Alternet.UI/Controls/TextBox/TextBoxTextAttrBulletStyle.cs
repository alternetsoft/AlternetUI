using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    public enum TextBoxTextAttrBulletStyle
    {
        None = 0x00000000,

        Arabic = 0x00000001,

        LettersUpper = 0x00000002,

        LettersLower = 0x00000004,

        RomanUpper = 0x00000008,

        RomanLower = 0x00000010,

        Symbol = 0x00000020,

        Bitmap = 0x00000040,

        Parentheses = 0x00000080,

        Period = 0x00000100,

        Standard = 0x00000200,

        RightParenthesis = 0x00000400,

        Outline = 0x00000800,

        AlignLeft = 0x00000000,

        AlignRight = 0x00001000,

        AlignCentre = 0x00002000,

        Continuation = 0x00004000,
    }
}
