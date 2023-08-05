using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Underline types that can be used in
    /// <see cref="ITextBoxTextAttr.SetFontUnderlinedEx"/>.
    /// </summary>
    public enum TextBoxTextAttrUnderlineType
    {
        /// <summary>
        /// No underline is specified.
        /// </summary>
        None,

        /// <summary>
        /// Solid underline.
        /// </summary>
        Solid,

        /// <summary>
        /// Double underline.
        /// </summary>
        Double,

        /// <summary>
        /// Special underline.
        /// </summary>
        Special,
    }
}
