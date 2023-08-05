using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Gets or sets how text is aligned in the control.
    /// </summary>
    public enum TextBoxTextAttrAlignment
    {
        /// <summary>
        /// Text has default alignment.
        /// </summary>
        Default,

        /// <summary>
        /// Text is aligned on the left side of the control.
        /// </summary>
        Left,

        /// <summary>
        /// Text is aligned in the center of the control.
        /// </summary>
        Center,

        /// <summary>
        ///  Text is aligned on the right side of the control.
        /// </summary>
        Right,

        /// <summary>
        /// Text is justified in the control. When you justify text, space is
        /// added between words so that both edges of each line are aligned
        /// with both margins. The last line is aligned left.
        /// </summary>
        Justified,
    }
}
