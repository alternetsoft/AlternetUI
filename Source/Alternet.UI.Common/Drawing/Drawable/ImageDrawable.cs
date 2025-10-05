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
        private SvgImageInfo svgImageInfo;
        private bool stretch = false;

        /// <summary>
        /// Gets a value indicating whether the object has an associated image.
        /// </summary>
        public bool IsImageSpecified => SvgImage is not null || Image is not null || ImageSet is not null;

        /// <summary>
        /// Gets or sets the SVG image associated with this instance.
        /// </summary>
        public SvgImage? SvgImage
        {
            get => svgImageInfo.SvgImage;

            set
            {
                svgImageInfo.SvgImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to render the SVG image.
        /// </summary>
        public Color? SvgColor
        {
            get => svgImageInfo.SvgColor;
            set => svgImageInfo.SvgColor = value;
        }

        /// <summary>
        /// Gets or sets the size of the SVG image in pixels.
        /// </summary>
        public int? SvgSize
        {
            get => svgImageInfo.SvgSize;
            set => svgImageInfo.SvgSize = value;
        }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? Image { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? ImageSet { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? DisabledImage { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? DisabledImageSet { get; set; }

        /// <summary>
        /// Gets whether or not to center this object vertically. Default is <c>true</c>.
        /// </summary>
        public bool CenterVert { get; set; } = true;

        /// <summary>
        /// Gets whether or not to center this object horizontally. Default is <c>true</c>.
        /// </summary>
        public bool CenterHorz { get; set; } = true;

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
        public virtual Image? GetImage(AbstractControl control, bool isDark)
        {
            var sz = SvgSize ?? ToolBarUtils.GetDefaultImageSize(control).Width;

            Image? GetNormalImage()
            {
                Image? image = null;

                if(SvgImage is not null)
                {
                    if(SvgColor is null)
                       image = SvgImage.AsNormalImage(sz, isDark);
                    else
                        image = SvgImage.ImageWithColor(sz, SvgColor);
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
                var image = SvgImage?.AsDisabledImage(sz, isDark);
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
            SvgSize = size;
        }

        /// <summary>
        /// Resets any cached images associated with the current instance.
        /// </summary>
        /// <remarks>This method clears the cached images to ensure that any
        /// subsequent operations use
        /// updated or refreshed image data. It is typically used when the
        /// underlying image source has changed and the
        /// cache needs to be invalidated.</remarks>
        public void ResetCachedImages()
        {
            svgImageInfo.ResetCachedImages();
        }

        /// <summary>
        /// Gets preferred size in device-independent units.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <returns></returns>
        public virtual SizeD GetPreferredSize(AbstractControl control)
        {
            SizeD result;

            var image = GetImage(control, control.IsDarkBackground);

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

            var image = GetImage(control, control.IsDarkBackground);
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
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            DefaultDrawImage(control, dc);
        }
    }
}