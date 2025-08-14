using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an overlay control that displays an image at a specified location.
    /// </summary>
    /// <remarks>This class extends <see cref="ControlOverlay"/> to provide
    /// functionality for rendering an
    /// image on a control. The image is drawn at the location specified
    /// by the <see cref="Location"/> property.</remarks>
    public class ControlOverlayWithImage : ControlOverlay
    {
        /// <summary>
        /// Gets or sets the image to display in the overlay.
        /// </summary>
        public virtual Image? Image { get; set; }

        /// <summary>
        /// Gets or sets the location where the image is drawn.
        /// </summary>
        public virtual PointD Location { get; set; }

        /// <inheritdoc/>
        public override void OnPaint(AbstractControl control, PaintEventArgs e)
        {
            if (Image is null)
                return;
            e.Graphics.DrawImage(Image, Location);
        }
    }
}
