using System;
using System.Collections.Generic;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an individual item that is displayed within a status bar.
    /// </summary>
    public class StatusBarPanel : Control
    {
        private string text = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class.
        /// </summary>
        public StatusBarPanel()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='StatusBarPanel'/> class with the specified text for the status bar item.
        /// </summary>
        public StatusBarPanel(string text)
        {
            Text = text;
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.StatusBarPanel;

        /// <summary>
        /// Gets a <see cref="StatusBarPanelHandler"/> associated with this class.
        /// </summary>
        public new StatusBarPanelHandler Handler
        {
            get
            {
                CheckDisposed();
                return (StatusBarPanelHandler)base.Handler;
            }
        }

        internal override bool IsDummy => true;

        /// <summary>
        /// Gets or sets a value indicating the text displayed in the status bar panel.
        /// </summary>
        public string Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();

                if (value == text)
                    return;

                text = value;
                TextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateStatusBarPanelHandler(this);
        }
    }
}