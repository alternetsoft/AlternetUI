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
            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                default);
            return result;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            var dc = (UI.Native.DrawingContext)NativeObject;
            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                WxApplicationHandler.WxWidget(control));
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
            DebugColorAssert(foreColor);

            location = TransformPointForDrawText(location);

            ToPixels(ref location, unit);

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
            DebugColorAssert(foreColor);
            dc.DrawText(
                text,
                TransformPointForDrawText(location),
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor);
        }

        /// <inheritdoc/>
        public override RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            GenericAlignment alignment = GenericAlignment.TopLeft,
            int indexAccel = -1)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor, nameof(foreColor));
            return dc.DrawLabel(
                text,
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                (UI.Native.Image?)image?.Handler,
                TransformRectForDrawText(rect),
                (int)alignment,
                indexAccel);
        }

        private PointD TransformPointForDrawText(PointD value)
        {
            if(App.IsWindowsOS || !HasTransform)
                return value;
            return Transform.TransformPoint(value);
        }

        private SizeD TransformSizeForDrawText(SizeD value)
        {
            if (App.IsWindowsOS || !HasTransform)
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
