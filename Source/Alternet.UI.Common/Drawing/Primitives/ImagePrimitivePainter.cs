using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.UI.Extensions;

namespace Alternet.Drawing
{
    /// <summary>
    /// Image primitive painter.
    /// </summary>
    public class ImagePrimitivePainter : PrimitivePainter
    {
        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? Image;

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? ImageSet;

        /// <summary>
        /// Gets or sets source rectangle which specifies part of the image to draw.
        /// </summary>
        public RectI? SourceRect = null;

        /// <summary>
        /// Performs default drawing of the image primitive.
        /// </summary>
        /// <param name="control">Control in which primitive is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void DefaultDrawImage(Control control, Graphics dc)
        {
            var image = Image;
            image ??= ImageSet?.AsImage(ImageSet.DefaultSize);

            if (Image.IsNullOrEmpty(image) || !Visible)
                return;

            if (Size is null)
            {
                dc.DrawImage(image, DestPoint);
                return;
            }

            if (Stretch)
            {
                if (SourceRect is null)
                    dc.DrawImage(image, DestRect);
                else
                    dc.DrawImage(image, DestRect, SourceRect.Value);
            }
            else
            {
                if (CenterHorzOrVert)
                {
                    var imageRect = SourceRect ?? image.BoundsDip(control);
                    var destRect = DestRect;
                    var centeredRect = imageRect.CenterIn(destRect, CenterHorz, CenterVert);
                    if (SourceRect is null)
                        dc.DrawImage(image, centeredRect);
                    else
                        dc.DrawImage(image, centeredRect, SourceRect.Value);
                }
                else
                    dc.DrawImage(image, DestPoint);
            }
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            DefaultDrawImage(control, dc);
        }
    }
}