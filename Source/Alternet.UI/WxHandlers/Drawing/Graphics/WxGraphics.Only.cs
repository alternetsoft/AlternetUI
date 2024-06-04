using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxGraphics
    {
        /// <inheritdoc/>
        public void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            dc.DrawTextAtRect(
                text,
                bounds,
                (UI.Native.Font)font.Handler,
                (UI.Native.Brush)brush.Handler,
                format.HorizontalAlignment,
                format.VerticalAlignment,
                format.Trimming,
                format.Wrapping);
        }

        /// <summary>
        /// Gets text measurement.
        /// </summary>
        public SizeD MeasureText(string text, Font font, Coord maximumWidth)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                maximumWidth,
                TextWrapping.Character);
        }

        /// <summary>
        /// Gets text measurement.
        /// </summary>
        public SizeD MeasureText(
            string text,
            Font font,
            Coord maximumWidth,
            TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugFormatAssert(format);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                maximumWidth,
                format.Wrapping);
        }

        /*/// <inheritdoc/>
        public override SizeD MeasureText(string text, Font font)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                double.NaN,
                TextWrapping.None);
        }*/

        /*/// <summary>
        /// If supported by the platform and the type of <see cref="Graphics"/>,
        /// fetches the contents
        /// of the graphics, or a subset of it, as an <see cref="Image"/>.
        /// </summary>
        /// <param name="subrect">Subset rectangle or <c>null</c> to get full image.
        /// Rectangle is specified in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image GetAsBitmapI(RectI? subrect = default)
        {
            var result = dc.GetAsBitmapI(subrect ?? RectI.Empty);
            return new Bitmap(result);
        }*/
    }
}
