using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements background and border painting.
    /// Additionally it can paint image inside the rectangle.
    /// </summary>
    public class RectangleDrawable : ImageDrawable
    {
        /// <summary>
        /// Gets or sets brush to fill the rectangle.
        /// </summary>
        public Brush? Brush;

        /// <summary>
        /// Gets or sets border settings.
        /// </summary>
        public BorderSettings? Border;

        /// <summary>
        /// Gets or sets whether border is painted. Default is <c>true</c>.
        /// </summary>
        public bool HasBorder = true;

        /// <summary>
        /// Gets or sets whether image is painted. Default is <c>false</c>.
        /// </summary>
        public bool HasImage = false;

        /// <summary>
        /// Performs default background and border painting.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void DefaultDrawBackAndBorder(Control control, Graphics dc)
        {
            if (!Visible)
                return;
            dc.FillBorderRectangle(
                Bounds,
                Brush,
                Border,
                HasBorder,
                control);
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;
            DefaultDrawBackAndBorder(control, dc);
            if(HasImage)
                DefaultDrawImage(control, dc);
        }
    }
}