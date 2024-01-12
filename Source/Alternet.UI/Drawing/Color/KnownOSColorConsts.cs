using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines color constants for the control parts in the different operating systems.
    /// </summary>
    public static class KnownOSColorConsts
    {
        /// <summary>
        /// Defines color constants for the control parts under Windows when dark color scheme
        /// is selected.
        /// </summary>
        public static class WindowsDark
        {
            /// <summary>
            /// Gets splitter color in File Explorer.
            /// </summary>
            public static Color ExplorerSplitter = Color.FromRgb(56, 56, 56);

            /// <summary>
            /// Gets main pane background color in File Explorer.
            /// </summary>
            public static Color ExplorerMainBack = Color.FromRgb(25, 25, 25);

            /// <summary>
            /// Gets preview pane background color in File Explorer.
            /// </summary>
            public static Color ExplorerPreviewBack = Color.FromRgb(32, 32, 32);
        }

        /// <summary>
        /// Defines color constants for the control parts under Windows when light color scheme
        /// is selected.
        /// </summary>
        public static class WindowsLight
        {
            /// <summary>
            /// Gets splitter color in File Explorer.
            /// </summary>
            public static Color ExplorerSplitter = Color.FromRgb(247, 247, 247);

            /// <summary>
            /// Gets main pane background color in File Explorer.
            /// </summary>
            public static Color ExplorerMainBack = Color.FromRgb(255, 255, 255);

            /// <summary>
            /// Gets preview pane background color in File Explorer.
            /// </summary>
            public static Color ExplorerPreviewBack = Color.FromRgb(255, 255, 255);
        }
    }
}
