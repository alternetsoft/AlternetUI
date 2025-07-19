using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the toolbars.
    /// </summary>
    public class ToolBarUtils
    {
        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 96 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 16. Suggested values are 16, 24, 32, 48.
        /// </remarks>
        public static int DefaultImageSize96dpi { get; set; } = 16;

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 144 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 24. Suggested values are 16, 24, 32, 48.
        /// </remarks>
        public static int DefaultImageSize144dpi { get; set; } = 24;

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 192 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 32. Suggested values are 16, 24, 32, 48.
        /// </remarks>
        public static int DefaultImageSize192dpi { get; set; } = 32;

        /// <summary>
        /// Defines the default size of toolbar images for displays
        /// with 288 DPI.
        /// </summary>
        /// <remarks>
        /// The size is the width and height of the image in pixels.
        /// You can use it to change the default size of toolbar buttons
        /// for more comfortable use.
        /// </remarks>
        /// <remarks>
        /// DPI stands for dots per inch and affects on clarity and crispness
        /// of the display. DPI is determined by the size of the screen and
        /// its resolution.
        /// </remarks>
        /// <remarks>
        /// Default property value is 48. Suggested values are 16, 24, 32, 48.
        /// </remarks>
        public static int DefaultImageSize288dpi { get; set; } = 48;

        /// <summary>
        /// Returns suggested toolbar image size depending on the given DPI value.
        /// </summary>
        /// <param name="deviceDpi">DPI for which default image size is
        /// returned.</param>
        public static int GetDefaultImageSize(Coord deviceDpi)
        {
            decimal deviceDpiRatio = (decimal)deviceDpi / 96m;

            if (deviceDpi <= 96 || deviceDpiRatio < 1.5m)
                return DefaultImageSize96dpi;
            if (deviceDpiRatio < 2m)
                return DefaultImageSize144dpi;
            if (deviceDpiRatio < 3m)
                return DefaultImageSize192dpi;
            return DefaultImageSize288dpi;
        }

        /// <summary>
        /// Returns suggested toolbar image size in pixels depending on the given DPI value.
        /// </summary>
        /// <param name="control">Control's <see cref="AbstractControl.GetDPI"/> method
        /// is used to get DPI settings.</param>
        public static SizeI GetDefaultImageSize(AbstractControl? control = null)
        {
            control = control?.Root ?? Window.Default;
            SizeD dpi;
            if (control is null || control.DisposingOrDisposed)
                dpi = Window.DefaultDPI ?? Display.Primary.DPI;
            else
                dpi = control.GetDPI();
            var result = ToolBarUtils.GetDefaultImageSize(dpi);
            return result;
        }

        /// <inheritdoc cref="GetDefaultImageSize(Coord)"/>
        public static SizeI GetDefaultImageSize(SizeD deviceDpi)
        {
            var width = GetDefaultImageSize(deviceDpi.Width);
            var height = GetDefaultImageSize(deviceDpi.Height);
            return new(width, height);
        }

        /// <summary>
        /// Gets the normal and disabled images as <see cref="ImageSet"/> from the specified
        /// <see cref="SvgImage"/> and <see cref="KnownButton"/>.
        /// </summary>
        /// <param name="btn">The known button for which images are retrieved.</param>
        /// <param name="control">The control which affects image rendering and is used
        /// to get scale factor.</param>
        /// <param name="svg">The SVG image to process. Optional. If not specified,
        /// the SVG image is retrieved based on the known button image.</param>
        /// <param name="size">The size of the image. If not specified, the default
        /// size for the toolbar images is used.</param>
        /// <returns>A tuple containing normal and disabled image sets.</returns>
        public static (ImageSet? Normal, ImageSet? Disabled) GetNormalAndDisabledSvg(
            SvgImage? svg,
            KnownButton? btn,
            AbstractControl control,
            int? size = null)
        {
            if (svg is null)
            {
                if (btn is null)
                {
                    return (null, null);
                }

                var info = KnownButtons.GetInfo(btn.Value);
                svg = info?.SvgImage;
            }

            if (svg is null)
            {
                return (null, null);
            }

            size ??= ToolBarUtils.GetDefaultImageSize(control).Width;

            var normalColor = control.GetSvgColor(KnownSvgColor.Normal);
            var disabledColor = control.GetSvgColor(KnownSvgColor.Disabled);

            var normalImage = svg.ImageSetWithColor(size.Value, normalColor);
            var disabledImage = svg.ImageSetWithColor(size.Value, disabledColor);

            return (normalImage, disabledImage);
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified url which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="AbstractControl.GetSvgColor"/>.
        /// </summary>
        /// <param name="size">Image size in pixels. If it is not specified,
        /// <see cref="ToolBarUtils.GetDefaultImageSize(AbstractControl)"/> is
        /// used to get image size.</param>
        /// <param name="control">Control which <see cref="AbstractControl.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        /// <param name="url">"embres" or "file" url with svg image data.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            string url,
            AbstractControl control,
            SizeI? size = null)
        {
            size ??= ToolBarUtils.GetDefaultImageSize(control);

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
        /// <see cref="AbstractControl.GetDPI"/> and
        /// <see cref="ToolBarUtils.GetDefaultImageSize(Coord)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="AbstractControl.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="ImageSet"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet FromSvgUrlForToolbar(
            string url,
            AbstractControl control,
            Color? color = null)
        {
            var imageSize = ToolBarUtils.GetDefaultImageSize(control);
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height, color);
            return result;
        }
    }
}
