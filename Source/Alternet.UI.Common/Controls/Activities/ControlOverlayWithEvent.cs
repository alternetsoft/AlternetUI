using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control overlay that provides paint event handling
    /// via both an event and an action delegate.
    /// </summary>
    public class ControlOverlayWithEvent : ControlOverlay
    {
        /// <summary>
        /// Gets or sets the action to be executed during the paint event.
        /// </summary>
        public Action<AbstractControl, PaintEventArgs>? PaintAction { get; set; }

        /// <summary>
        /// Occurs when the overlay is painted.
        /// </summary>
        public event EventHandler<PaintEventArgs>? Paint;

        /// <inheritdoc/>
        public override void OnPaint(AbstractControl control, PaintEventArgs e)
        {
            PaintAction?.Invoke(control, e);
            Paint?.Invoke(this, e);
        }
    }
}
