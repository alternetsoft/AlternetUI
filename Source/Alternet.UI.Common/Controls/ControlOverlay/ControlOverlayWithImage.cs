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

        /// <summary>
        /// Gets the width of the image in device-independent pixels (DIPs).
        /// </summary>
        public virtual float ImageWidthInDips
        {
            get
            {
                return Image != null ? GraphicsFactory.PixelToDip(Image.Width, Container?.ScaleFactor) : 0;
            }
        }

        /// <summary>
        /// Gets the height of the image in device-independent pixels (DIPs).
        /// </summary>
        public virtual float ImageHeightInDips
        {
            get
            {
                return Image != null ? GraphicsFactory.PixelToDip(Image.Height, Container?.ScaleFactor) : 0;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the right edge of the image, relative to its location.
        /// </summary>
        public virtual float Right
        {
            get
            {
                return Location.X + ImageWidthInDips;
            }

            set
            {
                Location = new PointD(value - ImageWidthInDips, Location.Y);
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the bottom edge of the image, relative to its location.
        /// </summary>
        public virtual float Bottom
        {
            get
            {
                return Location.Y + ImageHeightInDips;
            }
            set
            {
                Location = new PointD(Location.X, value - ImageHeightInDips);
            }
        }

        /// <inheritdoc/>
        public override void OnPaint(AbstractControl control, PaintEventArgs e)
        {
            if (Image is null)
                return;
            e.Graphics.DrawImage(Image, Location);
        }
    }
}
