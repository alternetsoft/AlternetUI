using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines different bullet styles.
    /// </summary>
    [Flags]
    public enum TextBoxTextAttrBulletStyle
    {
        /// <summary>
        /// List bullet style is not specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// List bullets are numbers.
        /// </summary>
        Arabic = 0x00000001,

        /// <summary>
        /// List bullets are letters in upper case.
        /// </summary>
        LettersUpper = 0x00000002,

        /// <summary>
        /// List bullets are letters in lower case.
        /// </summary>
        LettersLower = 0x00000004,

        /// <summary>
        /// List bullets are roman digits in upper case.
        /// </summary>
        RomanUpper = 0x00000008,

        /// <summary>
        /// List bullets are roman digits in lower case.
        /// </summary>
        RomanLower = 0x00000010,

        /// <summary>
        /// List bullets are symbols.
        /// </summary>
        Symbol = 0x00000020,

        /// <summary>
        /// This style is not supported.
        /// </summary>
        Bitmap = 0x00000040,

        /// <summary>
        /// List bullet style flag "Parenthesis".
        /// </summary>
        Parentheses = 0x00000080,

        /// <summary>
        /// Adds period after list item bullet.
        /// </summary>
        Period = 0x00000100,

        /// <summary>
        /// List bullets are using standard style.
        /// </summary>
        Standard = 0x00000200,

        /// <summary>
        /// List bullet style flag "RightParenthesis".
        /// </summary>
        RightParenthesis = 0x00000400,

        /// <summary>
        /// List bullet style flag "Outline".
        /// </summary>
        Outline = 0x00000800,

        /// <summary>
        /// List bullet style flag "AlignLeft".
        /// </summary>
        AlignLeft = 0x00000000,

        /// <summary>
        /// List bullet style flag "AlignRight".
        /// </summary>
        AlignRight = 0x00001000,

        /// <summary>
        /// List bullet style flag "AlignCentre".
        /// </summary>
        AlignCentre = 0x00002000,

        /// <summary>
        /// List bullet style flag "Continuation".
        /// </summary>
        Continuation = 0x00004000,
    }
}
