using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods related to the control drawing.
    /// </summary>
    public static class ControlUtils
    {
        /// <summary>
        /// Gets <see cref="IReadOnlyFontAndColor"/> instance for the specified
        /// <paramref name="method"/>.
        /// </summary>
        /// <param name="method">Type of the colors to get.</param>
        /// <param name="control">Control instance. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributes"/>.</param>
        /// <param name="renderSize">Rendering size. Used when <paramref name="method"/>
        /// is <see cref="ResetColorType.DefaultAttributesTextBox"/> or other similar values.
        /// You can skip this parameter as on most os it is ignored.</param>
        /// <returns></returns>
        public static IReadOnlyFontAndColor GetResetColors(
            ResetColorType method,
            Control? control = null,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            switch (method)
            {
                case ResetColorType.Auto:
                    return FontAndColor.Null;
                case ResetColorType.NullColor:
                    return FontAndColor.Null;
                case ResetColorType.EmptyColor:
                    return FontAndColor.Empty;
                case ResetColorType.DefaultAttributes:
                    return control?.GetDefaultFontAndColor() ?? FontAndColor.Null;
                case ResetColorType.DefaultAttributesTextBox:
                    return Control.GetStaticDefaultFontAndColor(ControlTypeId.TextBox, renderSize);
                case ResetColorType.DefaultAttributesListBox:
                    return Control.GetStaticDefaultFontAndColor(ControlTypeId.ListBox, renderSize);
                case ResetColorType.DefaultAttributesButton:
                    return Control.GetStaticDefaultFontAndColor(ControlTypeId.Button, renderSize);
                case ResetColorType.ColorMenu:
                    return FontAndColor.SystemColorMenu;
                case ResetColorType.ColorActiveCaption:
                    return FontAndColor.SystemColorActiveCaption;
                case ResetColorType.ColorInactiveCaption:
                    return FontAndColor.SystemColorInactiveCaption;
                case ResetColorType.ColorInfo:
                    return FontAndColor.SystemColorInfo;
                case ResetColorType.ColorWindow:
                    return FontAndColor.SystemColorWindow;
                case ResetColorType.ColorHighlight:
                    return FontAndColor.SystemColorHighlight;
                case ResetColorType.ColorButtonFace:
                    return FontAndColor.SystemColorButtonFace;
                default:
                    return FontAndColor.Null;
            }
        }

        /// <summary>
        /// Draws border in the specified rectangle of the drawing context.
        /// </summary>
        /// <param name="control">Control in which drawing is performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle.</param>
        /// <param name="border">Border settings.</param>
        public static void DrawBorder(Control? control, Graphics dc, RectD rect, BorderSettings? border)
        {
            if (border is null)
                return;

            border.InvokePaint(dc, rect);

            if (!border.DrawDefaultBorder)
                return;

            var radius = border.GetUniformCornerRadius(rect);
            var defaultColor = ControlUtils.GetDefaultBorderColor(control);

            if (radius != null)
            {
                dc.DrawRoundedRectangle(
                    border.Top.GetPen(defaultColor),
                    rect.InflatedBy(-1, -1),
                    radius.Value);
                return;
            }

            var topColor = border.Top.Color ?? defaultColor;
            var bottomColor = border.Bottom.Color ?? defaultColor;
            var leftColor = border.Left.Color ?? defaultColor;
            var rightColor = border.Right.Color ?? defaultColor;

            if (border.Top.Width > 0 && border.ColorIsOk(topColor))
            {
                dc.FillRectangle(topColor.AsBrush, border.GetTopRectangle(rect));
            }

            if (border.Bottom.Width > 0 && border.ColorIsOk(bottomColor))
            {
                dc.FillRectangle(bottomColor.AsBrush, border.GetBottomRectangle(rect));
            }

            if (border.Left.Width > 0 && border.ColorIsOk(leftColor))
            {
                dc.FillRectangle(leftColor.AsBrush, border.GetLeftRectangle(rect));
            }

            if (border.Right.Width > 0 && border.ColorIsOk(rightColor))
            {
                dc.FillRectangle(rightColor.AsBrush, border.GetRightRectangle(rect));
            }
        }

        /// <summary>
        /// Gets default border color if no colors are specified for the border.
        /// </summary>
        /// <param name="control">Control which background color is checked to get whether
        /// it is dark.</param>
        /// <returns></returns>
        public static Color GetDefaultBorderColor(Control? control)
        {
            if (control is null)
                return BorderSettings.DefaultBorderColor;
            var isDark = control.IsDarkBackground;
            return Border.DefaultColor?.Get(isDark)
                ?? TabControl.GetDefaultInteriorBorderColor(isDark);
        }

        /// <summary>
        /// Returns the value of a system metric, or -1 if the metric is not supported on
        /// the current system.
        /// </summary>
        /// <param name="index">System metric identifier.</param>
        /// <param name="control">Control for which metric is requested (optional).</param>
        /// <remarks>
        /// The value of <paramref name="control"/> determines if the metric returned is a global
        /// value or a control based value, in which case it might determine the widget, the
        /// display the window is on, or something similar. The window given should be as close
        /// to the metric as possible (e.g.a <see cref="Window"/> in case of
        /// the <see cref="SystemSettingsMetric.CaptionY"/> metric).
        /// </remarks>
        /// <remarks>
        /// Specifying the <paramref name="control"/> parameter is encouraged, because some
        /// metrics on some ports are not supported without one,or they might be capable of
        /// reporting better values if given one. If a control does not make sense for a metric,
        /// one should still be given, as for example it might determine which displays
        /// cursor width is requested with <see cref="SystemSettingsMetric.CursorX"/>.
        /// </remarks>
        public static int GetMetric(SystemSettingsMetric index, Control? control = default)
        {
            IntPtr wx = control?.WxWidget ?? default;
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, wx);
        }

        /// <summary>
        /// Draws sliced image with the specified
        /// <see cref="NinePatchImagePaintParams"/> parameters. This method can be used,
        /// for example, for drawing complex button bakgrounds using predefined templates.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where to draw.</param>
        /// <param name="e">Draw parameters.</param>
        /// <remarks>
        /// Source image is sliced into 9 pieces. All parts of the image except corners
        /// (top-left, top-right, bottom-right, bottom-left parts) are used
        /// by <see cref="TextureBrush"/> to fill larger destination rectangle.
        /// </remarks>
        /// <remarks>
        /// Issue with details is here:
        /// <see href="https://github.com/alternetsoft/AlternetUI/issues/115"/>.
        /// </remarks>
        public static void DrawImageSliced(this Graphics canvas, NinePatchImagePaintParams e)
        {
            var src = e.SourceRect;
            var dst = e.DestRect;
            var patchSrc = e.PatchRect;

            var offsetX = patchSrc.X - src.X;
            var offsetY = patchSrc.Y - src.Y;

            RectI patchDst = patchSrc;

            NineRects srcNine = new(src, patchSrc);

            patchDst.X = dst.X + offsetX;
            patchDst.Y = dst.Y + offsetY;
            patchDst.Width = dst.Width - (src.Width - patchSrc.Width);
            patchDst.Height = dst.Height - (src.Height - patchSrc.Height);

            NineRects dstNine = new(dst, patchDst);

            CopyRect(srcNine.Center, dstNine.Center);
            CopyRect(srcNine.TopCenter, dstNine.TopCenter);
            CopyRect(srcNine.BottomCenter, dstNine.BottomCenter);
            CopyRect(srcNine.CenterLeft, dstNine.CenterLeft);
            CopyRect(srcNine.CenterRight, dstNine.CenterRight);

            canvas.DrawImageI(e.Image, dstNine.TopLeft, srcNine.TopLeft);
            canvas.DrawImageI(e.Image, dstNine.TopRight, srcNine.TopRight);
            canvas.DrawImageI(e.Image, dstNine.BottomLeft, srcNine.BottomLeft);
            canvas.DrawImageI(e.Image, dstNine.BottomRight, srcNine.BottomRight);

            void CopyRect(RectI srcRect, RectI dstRect)
            {
                if (e.Tile)
                {
                    var subImage = e.Image.GetSubBitmap(srcRect);
                    var brush = subImage.AsBrush;
                    canvas.FillRectangleI(brush, dstRect);
                }
                else
                {
                    canvas.DrawImageI(e.Image, dstRect, srcRect);
                }
            }
        }

        /// <inheritdoc cref="CustomControlPainter.GetCheckBoxSize"/>
        public static SizeD GetCheckBoxSize(
            Control control,
            CheckState checkState,
            GenericControlState controlState)
        {
            return CustomControlPainter.Current.GetCheckBoxSize(control, checkState, controlState);
        }

        /// <inheritdoc cref="CustomControlPainter.DrawCheckBox"/>
        public static void DrawCheckBox(
            this Graphics canvas,
            Control control,
            RectD rect,
            CheckState checkState,
            GenericControlState controlState)
        {
            CustomControlPainter.Current.DrawCheckBox(
                control,
                canvas,
                rect,
                checkState,
                controlState);
        }

        /// <summary>
        /// Fills rectangle background and draws its border using the specified border settings.
        /// </summary>
        /// <param name="dc"><see cref="Graphics"/> where to draw.</param>
        /// <param name="rect">Rectangle.</param>
        /// <param name="brush">Brush to fill the rectangle.</param>
        /// <param name="border">Border settings.</param>
        /// <param name="hasBorder">Whether border is painted.</param>
        /// <param name="control">Control in which border is painted. Optional.</param>
        public static void FillBorderRectangle(
            this Graphics dc,
            RectD rect,
            Brush? brush,
            BorderSettings? border,
            bool hasBorder = true,
            Control? control = null)
        {
            if (brush is null && border is null)
                return;

            var radius = border?.GetUniformCornerRadius(rect);

            if (radius is not null && brush is not null)
            {
                var color = border?.Color;
                if (border is null || color is null || !hasBorder)
                {
                    dc.FillRoundedRectangle(brush, rect.InflatedBy(-1, -1), radius.Value);
                }
                else
                {
                    dc.RoundedRectangle(
                        color.AsPen,
                        brush,
                        rect.InflatedBy(-1, -1),
                        radius.Value);
                }

                return;
            }

            if (brush != null)
            {
                dc.FillRectangle(brush, rect);
            }

            if (hasBorder && border is not null)
            {
                DrawBorder(control, dc, rect, border);
            }
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="Control.GetSvgColor"/>.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="control">Control which <see cref="Control.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            Stream stream,
            SizeI size,
            Control control)
        {
            var image = ImageSet.FromSvgStream(
                stream,
                size,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified url which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="Control.GetSvgColor"/>.
        /// </summary>
        /// <param name="size">Image size in pixels. If it is not specified,
        /// <see cref="ToolBar.GetDefaultImageSize(Control)"/> is used to get image size.</param>
        /// <param name="control">Control which <see cref="Control.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        /// <param name="url">"embres" or "file" url with svg image data.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            string url,
            Control control,
            SizeI? size = null)
        {
            size ??= ToolBar.GetDefaultImageSize(control);

            using var stream = ResourceLoader.StreamFromUrl(url);
            var image = ImageSet.FromSvgStream(
                stream,
                size.Value,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolbarUtils.GetDefaultImageSize(double)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="ImageSet"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet FromSvgUrlForToolbar(string url, Control control, Color? color = null)
        {
            var imageSize = ToolBar.GetDefaultImageSize(control);
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height, color);
            return result;
        }
    }
}
