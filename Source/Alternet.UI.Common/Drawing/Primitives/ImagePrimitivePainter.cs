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
        /// Gets or sets destination point where to draw the image.
        /// </summary>
        public PointD DestPoint;

        /// <summary>
        /// Gets or sets destination image size.
        /// </summary>
        public SizeD? Size;

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
        /// Gets or sets destination rectangle where to draw the image.
        /// </summary>
        public virtual RectD DestRect
        {
            get
            {
                if(Size is null)
                {
                    return new RectD(DestPoint, Drawing.SizeD.Empty);
                }
                else
                    return new RectD(DestPoint, Size.Value);
            }

            set
            {
                DestPoint = value.TopLeft;
                Size = value.Size;
            }
        }

        /// <summary>
        /// Gets whether or not center this object horizontally or vertically.
        /// </summary>
        public virtual bool CenterHorzOrVert => CenterHorz || CenterVert;

        /// <summary>
        /// Draws image specified in the primitive properties.
        /// </summary>
        /// <param name="control">Control in which image is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void Draw(Control control, Graphics dc)
        {
            var image = Image;
            image ??= ImageSet?.AsImage(ImageSet.DefaultSize);

            if (Image.IsNullOrEmpty(image) || !Visible)
                return;

            if(Size is null)
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
    }
}