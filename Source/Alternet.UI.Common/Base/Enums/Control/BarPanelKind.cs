using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates the types of the individual panel in the status bar or toolbar.
    /// </summary>
    public enum BarPanelKind
    {
        /// <summary>
        /// The panel displays text.
        /// </summary>
        Text,

        /// <summary>
        /// The panel is a separator that displays a vertical line to separate other panels.
        /// The <see cref="StatusBarPanel.Text"/> and other properties are ignored for this panel.
        /// </summary>
        Separator,

        /// <summary>
        /// The panel is a picture box.
        /// </summary>
        PictureBox,

        /// <summary>
        /// The panel is a speed button.
        /// </summary>
        SpeedButton,

        /// <summary>
        /// The panel is a text only button without an image.
        /// </summary>
        TextButton,

        /// <summary>
        /// The panel is a progress bar panel that displays a progress bar.
        /// </summary>
        ProgressBar,

        /// <summary>
        /// The panel is a spacer that takes up space between other panels.
        /// The <see cref="StatusBarPanel.Text"/> and other properties are ignored for this panel.
        /// </summary>
        Spacer,

        /// <summary>
        /// The panel is a custom control.
        /// </summary>
        Control,
    }
}
