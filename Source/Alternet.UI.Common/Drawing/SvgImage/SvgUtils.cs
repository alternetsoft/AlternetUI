using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static methods related to svg images handling.
    /// </summary>
    public static class SvgUtils
    {
        /// <summary>
        /// Gets or sets a function that overrides the default calculation of SVG size.
        /// </summary>
        /// <remarks>The function takes a <see cref="Coord"/> object
        /// and an integer as input parameters,
        /// and returns an integer representing the calculated size.
        /// If this property is set to <see langword="null"/>,
        /// the default size calculation logic will be used.</remarks>
        public static Func<Coord, int, object?, int>? GetSvgSizeOverride;

        /// <summary>
        /// Calculates the size of an SVG element based on a scaling factor and a base size.
        /// </summary>
        /// <param name="scaleFactor">The scaling factor to apply to the base size.
        /// Must be a valid <see cref="Coord"/> value.</param>
        /// <param name="baseSize">The base size of the SVG element.
        /// Defaults to 16 if not specified.</param>
        /// <param name="context">The context object.</param>
        /// <returns>The calculated size of the SVG element as an integer.</returns>
        public static int GetSvgSize(Coord scaleFactor, int baseSize = 16, object? context = null)
        {
            if (GetSvgSizeOverride != null)
                return GetSvgSizeOverride(scaleFactor, baseSize, context);
            var result = (int)(scaleFactor * baseSize);
            return result;
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="AbstractControl.GetSvgColor"/>.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="control">Control which <see cref="AbstractControl.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        public static (ImageSet Normal, ImageSet Disabled) GetNormalAndDisabledSvg(
            Stream stream,
            SizeI size,
            AbstractControl control)
        {
            var image = ImageSet.FromSvgStream(
                stream,
                size,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
        }

        /// <summary>
        /// Changes fill color of the svg data.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="color">New fill color.</param>
        /// <remarks>
        /// If <paramref name="color"/> is <c>null</c> or equal to <see cref="Color.Black"/> input
        /// stream returned as is. If color is provided, function returns <see cref="MemoryStream"/>
        /// with converted data.
        /// </remarks>
        /// <exception cref="InvalidDataException">Error in svg data.</exception>
        internal static Stream ChangeFillColor(Stream stream, Color? color)
        {
            if (color is null || color.IsBlack)
                return stream;

            const string findText = "<svg";

            var s = StreamUtils.StringFromStream(stream);
            var hexColor = color.RGBHex;
            var insertText = $" fill=\"{hexColor}\"";
            var index = s.IndexOf(findText);
            if (index < 0)
                throw new InvalidDataException();
            var changed = s.Insert(index + 4, insertText);
            var memoryStream = new MemoryStream();
            StreamUtils.StringToStream(memoryStream, changed);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
