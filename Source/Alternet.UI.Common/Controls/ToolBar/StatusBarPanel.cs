using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel in a <see cref="StatusBar"/> control. A status bar can contain multiple panels,
    /// each of which can display text, images, or other content. The <see cref="StatusBarPanel"/> class
    /// provides properties and methods to customize the appearance and behavior of each panel.
    /// Use <see cref="BarPanel.Kind"/> property to specify the type of the panel, such as text, image, separator, etc.
    /// </summary>
    public partial class StatusBarPanel : BarPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class.
        /// </summary>
        public StatusBarPanel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class with the specified kind.
        /// </summary>
        /// <param name="kind">The kind of the status bar panel.</param>
        public StatusBarPanel(BarPanelKind kind)
            : base(kind)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class with the
        /// specified text for the status bar item.
        /// </summary>
        /// <param name="text">The text for the status bar panel.</param>
        public StatusBarPanel(string text)
            : base(text)
        {
        }
    }
}