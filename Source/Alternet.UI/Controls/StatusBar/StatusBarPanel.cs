using System;
using System.Collections.Generic;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a status bar.
    /// </summary>
    [ControlCategory("Hidden")]
    public class StatusBarPanel : FrameworkElement
    {
        private string text = string.Empty;
        private int width = -1;
        private StatusBarPanelStyle style = StatusBarPanelStyle.Flat;

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class.
        /// </summary>
        public StatusBarPanel()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class with the
        /// specified text for the status bar item.
        /// </summary>
        public StatusBarPanel(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when the <see cref="Width"/> property changes.
        /// </summary>
        public event EventHandler? WidthChanged;

        /// <summary>
        /// Occurs when the <see cref="Style"/> property changes.
        /// </summary>
        public event EventHandler? StyleChanged;

        /// <summary>
        /// Occurs when any of the properties were changed.
        /// </summary>
        public event EventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating the text displayed in the status bar panel.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (value == text)
                    return;

                text = value;
                TextChanged?.Invoke(this, EventArgs.Empty);
                PropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width == value)
                    return;
                width = value;
                WidthChanged?.Invoke(this, EventArgs.Empty);
                PropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public StatusBarPanelStyle Style
        {
            get
            {
                return style;
            }

            set
            {
                if (style == value)
                    return;
                style = value;
                StyleChanged?.Invoke(this, EventArgs.Empty);
                PropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(StatusBarPanel);
            else
                return Text;
        }
    }
}