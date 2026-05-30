using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates the types of the individual panel in the status bar.
    /// </summary>
    public enum StatusBarPanelKind
    {
        /// <summary>
        /// The panel is a text panel that displays text.
        /// </summary>
        Text,

        /// <summary>
        /// The panel is a separator that displays a vertical line to separate other panels.
        /// The <see cref="StatusBarPanel.Text"/> and other properties are ignored for this panel.
        /// </summary>
        Separator,
    }
}
