using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines how the container control will be splitted.
    /// </summary>
    public enum SplitterPanelSplitMethod
    {
        /// <summary>
        /// Container will be splitted manually by the programmer.
        /// </summary>
        Manual,

        /// <summary>
        /// First two visible child controls of the container
        /// will be splitted horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// First two visible child controls of the container
        /// will be splitted vertically.
        /// </summary>
        Vertical,

        /// <summary>
        /// Container will be initialized as unsplitted
        /// with the first visible control.
        /// </summary>
        Unsplitted,
    }
}
