using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Extends <see cref="IGraphics"/> with additional functionality implemented
    /// on WxWidgets platform.
    /// </summary>
    [Obsolete("Depends on WxWdigets")]
    public interface IWxGraphics : IGraphics
    {
        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/> and
        /// maximum width.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="maximumWidth">Maximum width of the string in device-independent units.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units, of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        SizeD MeasureText(string text, Font font, Coord maximumWidth);

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/>,
        /// maximum width and <see cref="TextFormat"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="maximumWidth">Maximum width of the string in device-independent units.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units, of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        SizeD MeasureText(
            string text,
            Font font,
            Coord maximumWidth,
            TextFormat format);

        /*/// <summary>
        /// Draws text using the specified drawing parameters and text format.
        /// </summary>
        /// <param name="text">Text string,</param>
        /// <param name="font">Text font.</param>
        /// <param name="brush">Brush.</param>
        /// <param name="bounds">Bounds in which text is drawn.</param>
        /// <param name="format">Text format.</param>
        void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format);*/
    }
}
