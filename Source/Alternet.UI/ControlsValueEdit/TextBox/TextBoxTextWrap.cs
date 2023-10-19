using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies word wrap style fot the multiline <see cref="TextBox"/> controls.
    /// </summary>
    public enum TextBoxTextWrap
    {
        /// <summary>
        /// Wrap the lines at word boundaries or at any other
        /// character if there are words longer than the window width (this is the default).
        /// </summary>
        Best = 0,

        /// <summary>
        /// Wrap the lines too long to be shown entirely at word
        /// boundaries (wxUniv, Windows, Linux, MacOs).
        /// </summary>
        Word = 1,

        /// <summary>
        /// Wrap the lines too long to be shown entirely
        /// at any position (wxUniv, Linux, MacOs).
        /// </summary>
        Char = 2,

        /// <summary>
        /// Don't wrap at all, show horizontal scrollbar instead.
        /// </summary>
        None = 3,
    }
}