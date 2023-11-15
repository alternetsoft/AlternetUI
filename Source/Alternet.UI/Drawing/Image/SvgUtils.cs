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
            if (color is null || color.Value.IsBlack)
                return stream;

            const string findText = "<svg";

            var s = StreamUtils.StringFromStream(stream);
            var hexColor = color.Value.RGBHex;
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
