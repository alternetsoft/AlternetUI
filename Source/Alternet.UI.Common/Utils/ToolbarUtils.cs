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
    public class ToolbarUtils
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
        public static int GetDefaultImageSize(double deviceDpi)
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

        /// <inheritdoc cref="GetDefaultImageSize(double)"/>
        public static SizeI GetDefaultImageSize(SizeD deviceDpi)
        {
            var width = GetDefaultImageSize(deviceDpi.Width);
            var height = GetDefaultImageSize(deviceDpi.Height);
            return new(width, height);
        }
    }
}
