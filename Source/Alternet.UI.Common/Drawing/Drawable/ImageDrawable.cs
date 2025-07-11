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
        public SvgImageInfo SvgImage;

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? Image;

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? ImageSet;

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? DisabledImage;

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? DisabledImageSet;

        /// <summary>
        /// Gets whether or not to center this object vertically. Default is <c>true</c>.
        /// </summary>
        public bool CenterVert = true;

        /// <summary>
        /// Gets whether or not to center this object horizontally. Default is <c>true</c>.
        /// </summary>
        public bool CenterHorz = true;

        private bool stretch = false;

        /// <summary>
        /// Gets whether or not to stretch this object. Default is <c>true</c>.
        /// </summary>
        public virtual bool Stretch
        {
            get => stretch;

            set
            {
                stretch = value;
            }
        }

        /// <summary>
        /// Gets whether or not center this object horizontally or vertically.
        /// </summary>
        public bool CenterHorzOrVert => CenterHorz || CenterVert;

        /// <summary>
        /// Gets image to draw.
        /// </summary>
        /// <returns></returns>
        public virtual Image? GetImage(bool isDark)
        {
            Image? GetNormalImage()
            {
                Image? image = null;

                if(SvgImage.SvgImage is not null)
                {
                    if(SvgImage.SvgColor is null)
                        image = SvgImage.SvgImage.AsNormalImage(SvgImage.SvgSize, isDark);
                    else
                        image = SvgImage.SvgImage.ImageWithColor(SvgImage.SvgSize, SvgImage.SvgColor);
                }

                image ??= Image ?? ImageSet?.AsImage(ImageSet.DefaultSize);
                return image;
            }

            if (Enabled)
            {
                return GetNormalImage();
            }
            else
            {
                var image = SvgImage.SvgImage?.AsDisabledImage(SvgImage.SvgSize, isDark);
                image ??= DisabledImage ?? DisabledImageSet?.AsImage(DisabledImageSet.DefaultSize);
                image ??= GetNormalImage();
                return image;
            }
        }

        /// <summary>
        /// Sets svg size. Implemented for the convenience.
        /// </summary>
        /// <param name="size">New svg image width and height.</param>
        public virtual void SetSvgSize(int size)
        {
            SvgImage.SvgSize = size;
        }

        /// <summary>
        /// Gets preferred size in device-independent units.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <returns></returns>
        public virtual SizeD GetPreferredSize(AbstractControl control)
        {
            SizeD result;

            var image = GetImage(control.IsDarkBackground);

            if (image is not null)
                result = image.SizeDip(control.ScaleFactor);
            else
                result = SizeD.Empty;

            return result;
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

            var image = GetImage(control.IsDarkBackground);
            if (Image.IsNullOrEmpty(image))
                return;

            if (Size.AnyIsEmptyOrNegative)
            {
                dc.DrawImage(image, Location);
                return;
            }

            if (Stretch)
            {
                    dc.DrawImage(image, Bounds);
            }
            else
            {
                if (CenterHorzOrVert)
                {
                    var imageRect = image.BoundsDip(control);
                    var destRect = Bounds;
                    var centeredRect = imageRect.CenterIn(destRect, CenterHorz, CenterVert);
                    dc.DrawImage(image, centeredRect.Location);
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