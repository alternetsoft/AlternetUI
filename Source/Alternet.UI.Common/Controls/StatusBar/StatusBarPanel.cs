using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a status bar.
    /// </summary>
    [ControlCategory("Hidden")]
    public partial class StatusBarPanel : BaseControlItem
    {
        private readonly BaseConcurrentStack<string> textStack = new();
        private string text = string.Empty;
        private float width = -1;
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
        /// Gets or sets a value indicating the text displayed in the status bar panel.
        /// </summary>
        public virtual string Text
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
                RaisePropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Gets or sets width of the status bar panel.
        /// </summary>
        /// <remarks>
        /// See more details in <see cref="StatusBar.SetStatusWidths"/> on how to define
        /// automatically resizable panels.
        /// </remarks>
        public virtual float Width
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
                RaisePropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// Gets or sets style of the status bar panel.
        /// </summary>
        public virtual StatusBarPanelStyle Style
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

        /// <summary>
        /// Pushes the current <see cref="Text"/> value onto the stack and sets a new text.
        /// </summary>
        /// <param name="text">The new text to set.</param>
        public virtual void PushText(string text)
        {
            textStack.Push(this.text);
            Text = text;
        }

        /// <summary>
        /// Restores the previous text value that was saved by the last call to <see cref="PushText"/>.
        /// </summary>
        public virtual void PopText()
        {
            if (textStack.TryPop(out var value))
                Text = value;
        }

        /// <summary>
        /// Gets the rectangle that represents the bounds of this <see cref="StatusBarPanel"/> within the status bar.
        /// </summary>
        /// <returns></returns>
        public virtual RectD GetRect()
        {
            return RectD.Empty;
        }

        /// <summary>
        /// Assigns properties from another <see cref="StatusBarPanel"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public virtual void Assign(StatusBarPanel item)
        {
            text = item.Text;
            width = item.Width;
            style = item.Style;
            Tag = item.Tag;
            RaisePropertyChanged();
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