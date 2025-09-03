using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates text replacement methods when the text width exceed the
    /// container's width.
    /// </summary>
    public enum TextEllipsisType
    {
        /// <summary>
        /// No replacement of the text with an ellipsis is done when the text width exceed the
        /// container's width.
        /// </summary>
        None,

        /// <summary>
        /// Replace the end of the text with an ellipsis when the text width exceed the
        /// container's width.
        /// </summary>
        End,

        /// <summary>
        /// Replace the beginning of the text with an ellipsis when the text width exceed the
        /// container's width.
        /// </summary>
        Start,

        /// <summary>
        /// Replace the middle of the text with an ellipsis when the text width exceed the
        /// container's width.
        /// </summary>
        Middle,
    }
}