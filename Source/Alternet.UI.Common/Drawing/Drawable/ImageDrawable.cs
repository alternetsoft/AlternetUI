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
    /// Implements image drawing.
    /// </summary>
    public class ImageDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets svg image to draw.
        /// </summary>
        public SvgImageInfo? SvgImage;

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
        /// Gets whether or not to stretch this object. Default is <c>true</c>.
        /// </summary>
        public bool Stretch = true;

        /// <summary>
        /// Gets whether or not to center this object vertically. Default is <c>true</c>.
        /// </summary>
        public bool CenterVert = true;

        /// <summary>
        /// Gets whether or not to center this object horizontally. Default is <c>true</c>.
        /// </summary>
        public bool CenterHorz = true;

        /// <summary>
        /// Gets whether or not center this object horizontally or vertically.
        /// </summary>
        public bool CenterHorzOrVert => CenterHorz || CenterVert;

        /// <summary>
        /// Gets image to draw.
        /// </summary>
        /// <returns></returns>
        public virtual Image? GetImage()
        {
            var image = SvgImage?.AsImage;
            image ??= Image ?? ImageSet?.AsImage(ImageSet.DefaultSize);
            return image;
        }

        /// <summary>
        /// Performs default drawing of the image.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void DefaultDrawImage(AbstractControl control, Graphics dc)
        {
            if (!Visible)
                return;

            var image = GetImage();
            if (Image.IsNullOrEmpty(image))
                return;

            if (Size.AnyIsEmptyOrNegative)
            {
                dc.DrawImage(image, Location);
                return;
            }

            if (Stretch)
            {
                if (SourceRect is null)
                    dc.DrawImage(image, Bounds);
                else
                    dc.DrawImage(image, Bounds, SourceRect.Value);
            }
            else
            {
                if (CenterHorzOrVert)
                {
                    var imageRect = SourceRect ?? image.BoundsDip(control);
                    var destRect = Bounds;
                    var centeredRect = imageRect.CenterIn(destRect, CenterHorz, CenterVert);
                    if (SourceRect is null)
                        dc.DrawImage(image, centeredRect);
                    else
                        dc.DrawImage(image, centeredRect, SourceRect.Value);
                }
                else
                    dc.DrawImage(image, Location);
            }
        }

        /// <inheritdoc/>
        public override void Draw(AbstractControl control, Graphics dc)
        {
            DefaultDrawImage(control, dc);
        }
    }
}