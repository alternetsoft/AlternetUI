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

        /// <summary>
        /// Occurs when the <see cref="Style"/> property changes.
        /// </summary>
        public event EventHandler? StyleChanged;

        /// <summary>
        /// Gets or sets style of the status bar panel.
        /// </summary>
        public virtual StatusBarPanelStyle Style
        {
            get
            {
                return HasBorder ? StatusBarPanelStyle.Normal : StatusBarPanelStyle.Flat;
            }

            set
            {
                if (Style == value)
                    return;

                if (value == StatusBarPanelStyle.Normal)
                    HasBorder = true;
                else
                    HasBorder = false;

                StyleChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(nameof(Style));
            }
        }

        /// <summary>
        /// Creates copy of this <see cref="StatusBarPanel"/>.
        /// </summary>
        public virtual StatusBarPanel Clone()
        {
            var result = new StatusBarPanel();
            result.Assign(this);
            return result;
        }
    }
}