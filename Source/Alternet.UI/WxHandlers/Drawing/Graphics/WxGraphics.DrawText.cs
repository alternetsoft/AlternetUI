using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxGraphics
    {
        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            if (text is null || text.Length == 0)
                return SizeD.Empty;

            var dpi = GetDPI().Width;

            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                default);
            return result;
        }

        /// <inheritdoc/>
        public override void DrawRotatedText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            if (!foreColor.IsOk)
                return;

            font = TransformFontForDrawText(font);
            location = TransformPointForDrawText(location);

            ToPixels(ref location, unit);

            font = TransformFontForDrawText(font);
            dc.DrawRotatedTextI(
                text,
                location.ToPoint(),
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                angle);
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DoInsideClipped(bounds, () =>
            {
                DrawText(text, font, brush, bounds.Location);
            });
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
            DrawText(text, origin, font, brush.AsColor, Color.Empty);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            if (!foreColor.IsOk)
                return;
            font = TransformFontForDrawText(font);
            dc.DrawText(
                text,
                TransformPointForDrawText(location),
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool GetNoTransformForDrawText()
        {
            return App.IsWindowsOS || !HasTransform || WxGlobals.NoTransformForDrawText;
        }

        private PointD TransformPointForDrawText(PointD value)
        {
            if(GetNoTransformForDrawText())
                return value;
            return Transform.TransformPoint(value);
        }

        private Font TransformFontForDrawText(Font value)
        {
            if (GetNoTransformForDrawText())
                return value;
            var scaledSize = Transform.TransformSize(value.Size);
            return value.WithSize(scaledSize.Height);
        }

        private SizeD TransformSizeForDrawText(SizeD value)
        {
            if (GetNoTransformForDrawText())
                return value;
            return Transform.TransformSize(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RectD TransformRectForDrawText(RectD value)
        {
            return (TransformPointForDrawText(value.Location), TransformSizeForDrawText(value.Size));
        }
    }
}
