using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all <see cref="AuiNotebook"/> options.
    /// </summary>
    [Flags]
    internal enum AuiNotebookCreateStyle
    {
        /// <summary>
        /// With this style, tabs are drawn along the top of the notebook.
        /// </summary>
        Top = 1 << 0,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Left = 1 << 1,

        /// <summary>
        /// Not implemented.
        /// </summary>
        Right = 1 << 2,

        /// <summary>
        /// With this style, tabs are drawn along the bottom of the notebook.
        /// </summary>
        Bottom = 1 << 3,

        /// <summary>
        /// Allows the tab control to be split by dragging a tab.
        /// </summary>
        TabSplit = 1 << 4,

        /// <summary>
        /// Allows a tab to be moved horizontally by dragging.
        /// </summary>
        TabMove = 1 << 5,

        /// <summary>
        /// Allows a tab to be moved to another tab control.
        /// </summary>
        TabExternalMove = 1 << 6,

        /// <summary>
        /// With this style, all tabs have the same width.
        /// </summary>
        TabFixedWidth = 1 << 7,

        /// <summary>
        /// With this style, left and right scroll buttons are displayed.
        /// </summary>
        ScrollButtons = 1 << 8,

        /// <summary>
        /// With this style, a drop-down list of windows is available.
        /// </summary>
        WindowListButton = 1 << 9,

        /// <summary>
        /// With this style, a close button is available on the tab bar.
        /// </summary>
        CloseButton = 1 << 10,

        /// <summary>
        /// With this style, the close button is visible on the active tab.
        /// </summary>
        CloseOnActiveTab = 1 << 11,

        /// <summary>
        /// With this style, the close button is visible on all tabs.
        /// </summary>
        CloseOnAllTabs = 1 << 12,

        /// <summary>
        /// With this style, middle click on a tab closes the tab.
        /// </summary>
        MiddleClickClose = 1 << 13,

        /// <summary>
        /// Default style of the <see cref="AuiNotebook"/>.
        /// </summary>
        DefaultStyle = Top | TabSplit | TabMove | ScrollButtons |
            CloseOnActiveTab | MiddleClickClose,
    }
}