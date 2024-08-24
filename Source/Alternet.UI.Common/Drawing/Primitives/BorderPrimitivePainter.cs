using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Border primitive painter. Additionally to border painting, it can paint background and image.
    /// </summary>
    public class BorderPrimitivePainter : ImagePrimitivePainter
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
        /// Performs default drawing of the border primitive.
        /// </summary>
        /// <param name="control">Control in which primitive is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void DefaultDrawBorder(Control control, Graphics dc)
        {
            if (!Visible)
                return;
            dc.FillBorderRectangle(
                DestRect,
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
            DefaultDrawBorder(control, dc);
            if(HasImage)
                DefaultDrawImage(control, dc);
        }
    }
}