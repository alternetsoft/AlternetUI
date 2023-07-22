using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines automatic behavior of the <see cref="SplitterPanel"/> control.
    /// </summary>
    public enum SplitterPanelSplitMethod
    {
        /// <summary>
        /// <see cref="SplitterPanel"/> will be splitted manually by the programmer
        /// using <see cref="SplitterPanel.SplitVertical"/>,
        /// <see cref="SplitterPanel.SplitHorizontal"/> or
        /// <see cref="SplitterPanel.InitUnsplitted"/> methods.
        /// </summary>
        Manual,

        /// <summary>
        /// First two visible child controls of the <see cref="SplitterPanel"/>
        /// will be splitted horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// First two visible child controls of the <see cref="SplitterPanel"/>
        /// will be splitted vertically.
        /// </summary>
        Vertical,

        /// <summary>
        /// <see cref="SplitterPanel"/> will be initialized as unsplitted
        /// with the first visible control.
        /// </summary>
        Unsplitted,
    }
}
