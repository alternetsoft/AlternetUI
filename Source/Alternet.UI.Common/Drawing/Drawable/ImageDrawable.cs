using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a drawable object that can display an image, including support for SVG images
    /// and various image states (enabled/disabled). This class provides properties to specify the image source,
    /// color, size, and alignment options. It also includes methods to retrieve the appropriate image based
    /// on the control's state and to perform default drawing of the image within a specified control and drawing context.
    /// </summary>
    public partial class ImageDrawable : BaseDrawable
    {
        private SvgImageInfo svgImageInfo;
        private bool stretch = false;

        /// <summary>
        /// Gets a value indicating whether the object has an associated image.
        /// </summary>
        public bool IsImageSpecified => SvgImage is not null || Image is not null
            || ImageSet is not null || Icon is not null;

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
        /// Gets or sets the horizontal corner radius for rounded corners.
        /// </summary>
        public float CornerRadiusX { get; set; } = 5.0f;

        /// <summary>
        /// Gets or sets the vertical corner radius for rounded corners.
        /// </summary>
        public float CornerRadiusY { get; set; } = 5.0f;

        /// <summary>
        /// Gets or sets a value indicating whether the image should be painted with round corners.
        /// </summary>
        public bool UseCornerRadius { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? Image { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? ImageSet { get; set; }

        /// <summary>
        /// Gets or sets images to draw. This property is used to specify different images for different
        /// visual states of the control, such as normal, hovered, pressed, and disabled states.
        /// It allows for a more dynamic and responsive user interface by providing appropriate
        /// visual feedback based on the control's state.
        /// </summary>
        public ControlStateImages? Images { get; set; }

        /// <summary>
        /// Gets or sets image sets to draw. This property is used to specify different image sets for different
        /// visual states of the control, such as normal, hovered, pressed, and disabled states.
        /// It allows for a more dynamic and responsive user interface by providing appropriate
        /// visual feedback based on the control's state.
        /// </summary>
        public ControlStateImageSets? ImageSets { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public Image? DisabledImage { get; set; }

        /// <summary>
        /// Gets or sets image to draw.
        /// </summary>
        public ImageSet? DisabledImageSet { get; set; }

        /// <summary>
        /// Gets or sets icon to draw.
        /// </summary>
        public IconSet? Icon { get; set; }

        /// <summary>
        /// Gets or sets horizontal alignment option that specifies
        /// how the image should be aligned within the available space.
        /// This property is used when the image is smaller than the available space.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        /// <summary>
        /// Gets or sets vertical alignment option that specifies how
        /// the image should be aligned within the available space.
        /// This property is used when the image is smaller than the available space.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

        /// <summary>
        /// Gets or sets whether to center this object vertically. Default is <c>true</c>.
        /// This property is used when the image is smaller than the available space,
        /// and it determines whether the image should be centered vertically or aligned to the top.
        /// There is more advanced centering logic via the <see cref="VerticalAlignment"/>
        /// and <see cref="HorizontalAlignment"/> properties.
        /// </summary>
        public bool CenterVert
        {
            get => VerticalAlignment == VerticalAlignment.Center;
            set => VerticalAlignment = value ? VerticalAlignment.Center : VerticalAlignment.Top;
        }

        /// <summary>
        /// Gets or sets whether to center this object horizontally. Default is <c>true</c>.
        /// This property is used when the image is smaller than the available space,
        /// and it determines whether the image should be centered horizontally or aligned to the left.
        /// There is more advanced centering logic via the <see cref="VerticalAlignment"/>
        /// and <see cref="HorizontalAlignment"/> properties.
        /// </summary>
        public bool CenterHorz
        {
            get => HorizontalAlignment == HorizontalAlignment.Center;
            set => HorizontalAlignment = value ? HorizontalAlignment.Center : HorizontalAlignment.Left;
        }

        /// <summary>
        /// Gets or sets whether image is aligned to the left-top of the available space.
        /// </summary>
        public bool IsLeftTopAligned => HorizontalAlignment == HorizontalAlignment.Left
            && VerticalAlignment == VerticalAlignment.Top;

        /// <summary>
        /// Gets whether or not to stretch this object.
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
        /// Gets or sets image size fallback options that determine how to handle
        /// cases where the specified image size is not available in the <see cref="IconSet"/> or other containers.
        /// </summary>
        public virtual ImageSizeFallbackOptions? SizeFallbackOptions { get; set; }

        /// <summary>
        /// Gets image to draw.
        /// </summary>
        /// <returns></returns>
        public virtual Image? GetImage(AbstractControl control, bool isDark)
        {
            if (Icon is not null)
            {
                var iconImage = Icon.GetImageWithFallback(SizeFallbackOptions);

                if (!Enabled)
                {
                    iconImage = iconImage?.ToGrayScaleCached();
                }

                return iconImage;
            }

            var sz = SvgSize ?? ToolBarUtils.GetDefaultImageSize(control).Width;

            Image? GetImage(VisualControlState? state = null)
            {
                state ??= VisualState;

                Image? image = null;

                if (SvgImage is not null)
                {
                    if (SvgColor is null)
                        image = SvgImage.AsNormalImage(sz, isDark);
                    else
                        image = SvgImage.ImageWithColor(sz, SvgColor);
                }

                image ??= Images?.GetObjectOrNull(state.Value) ?? Image;

                if (image is null)
                {
                    var imageSet = ImageSets?.GetObjectOrNull(state.Value) ?? ImageSet;
                    image = imageSet?.AsImage(imageSet.DefaultSize);
                }

                return image;
            }

            Image? GetDisabledImage()
            {
                var image = SvgImage?.AsDisabledImage(sz, isDark);
                image ??= DisabledImage ?? DisabledImageSet?.AsImage(DisabledImageSet.DefaultSize);
                image ??= GetImage(VisualControlState.Normal)?.ToGrayScaleCached();
                return image;
            }

            if (Enabled)
            {
                return GetImage();
            }
            else
            {
                return GetDisabledImage();
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

            RectD locationRect = new(Location, image.SizeDip(control));

            if (Size.AnyIsEmptyOrNegative)
            {
                InternalPaint(() => dc.DrawImage(image, Location), locationRect);
                return;
            }

            if (Stretch)
            {
                InternalPaint(() => dc.DrawImage(image, Bounds), Bounds);
                return;
            }

            if (IsLeftTopAligned)
            {
                InternalPaint(() => dc.DrawImage(image, Location), locationRect);
                return;
            }

            var destRect = Bounds;
            var imageRect = image.BoundsDip(control);

            var alignedRect = AlignUtils.AlignRectInRect(
                imageRect,
                destRect,
                HorizontalAlignment,
                VerticalAlignment,
                shrinkSize: false);

            InternalPaint(() => dc.DrawImage(image, alignedRect.Location), alignedRect);

            void InternalPaint(Action paintAction, RectD rect)
            {
                if (UseCornerRadius)
                {
                    using var builder = new SKPathBuilder();

                    builder.AddRoundRect(new SKRect(rect.Left, rect.Top, rect.Right, rect.Bottom),
                                        CornerRadiusX, CornerRadiusY);

                    dc.Canvas.Save();

                    var path = builder.Detach();

                    dc.Canvas.ClipPath(path);
                    paintAction();
                    dc.Canvas.Restore();
                }
                else
                {
                    paintAction();
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            DefaultDrawImage(control, dc);
        }
    }
}