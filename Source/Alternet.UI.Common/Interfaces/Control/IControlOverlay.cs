using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for painting overlays on controls.
    /// Implement this interface to provide custom overlay rendering logic
    /// for a specific <see cref="AbstractControl"/>.
    /// </summary>
    public interface IControlOverlay
    {
        /// <summary>
        /// Gets the unique identifier for the overlay.
        /// </summary>
        ObjectUniqueId UniqueId { get; }

        /// <summary>
        /// Gets or sets the flags that control the behavior and appearance of the overlay.
        /// </summary>
        ControlOverlayFlags Flags { get; set; }

        /// <summary>
        /// Paints the overlay on the specified control.
        /// </summary>
        /// <param name="control">The control to paint the overlay on.</param>
        /// <param name="e">The paint event arguments.</param>
        void OnPaint(AbstractControl control, PaintEventArgs e);
    }
}
