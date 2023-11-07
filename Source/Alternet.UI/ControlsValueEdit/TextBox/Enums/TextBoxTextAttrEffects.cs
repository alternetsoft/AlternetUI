using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Styles for <see cref="ITextBoxTextAttr.SetTextEffects"/>
    /// </summary>
    [Flags]
    public enum TextBoxTextAttrEffects
    {
        /// <summary>
        /// No text effects are specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Capitalises text when displayed (leaving the case of the actual
        /// buffer text unchanged.
        /// </summary>
        Capitals = 0x00000001,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SmallCapitals = 0x00000002,

        /// <summary>
        /// Draws a line through text.
        /// </summary>
        Strikethrough = 0x00000004,

        /// <summary>
        /// Not implemented.
        /// </summary>
        DoubleStrikethrough = 0x00000008,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Shadow = 0x00000010,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Emboss = 0x00000020,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Outline = 0x00000040,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Engrave = 0x00000080,

        /// <summary>
        /// Text effect "Superscript".
        /// </summary>
        Superscript = 0x00000100,

        /// <summary>
        /// Text effect "Subscript".
        /// </summary>
        Subscript = 0x00000200,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Rtl = 0x00000400,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SuppressHyphenation = 0x00001000,
    }
}