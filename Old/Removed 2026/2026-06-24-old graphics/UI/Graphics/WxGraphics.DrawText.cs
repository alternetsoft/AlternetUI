using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Skia;
using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxGraphics
    {
        public override IGraphicsPathHandler CreateGraphicsPathHandler()
        {
            var result = new UI.Native.GraphicsPath();
            result.Initialize((UI.Native.DrawingContext)NativeObject);
            return result;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(ReadOnlySpan<char> text, Font font)
        {
            if (text.Length == 0)
                return SizeD.Empty;

            return StringUtils.InvokeWithResult(text, span =>
            {
                return dc.GetTextExtentSimple(span, (UI.Native.Font)font.Handler, default);
            });
        }

        /// <inheritdoc/>
        public override void DrawText(ReadOnlySpan<char> text, Font font, Brush brush, RectD bounds)
        {
            var rect = TransformRectToNative(bounds);

            Save();

            try
            {
                ClipRect(rect);
                DrawText(text, font, brush, rect.Location);
            }
            finally
            {
                Restore();
            }
        }

        /// <inheritdoc/>
        public override void DrawText(
            ReadOnlySpan<char> text,
            Font font,
            Brush brush,
            PointD origin)
        {
            DrawText(text, origin, font, brush.AsColor, Color.Empty);
        }

        /// <inheritdoc/>
        public override void DrawText(
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            DrawTextWithAngle(
                text,
                location,
                font,
                foreColor,
                backColor,
                0);
        }

        public override void DrawTextWithAngle(
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            if (!foreColor.IsOk)
                return;

            Coord angle2;

            if (GetNoTransformToNative())
                angle2 = 0;
            else
                angle2 = Transform.GetRotationAngleInRadians();

            UI.Native.Brush brush;
            bool useBrush;

            if (backColor.IsOk && !backColor.IsEmpty)
            {
                brush = (UI.Native.Brush)backColor.AsBrush.Handler;
                useBrush = true;
            }
            else
            {
                brush = (UI.Native.Brush)Brush.Transparent.Handler;
                useBrush = false;
            }

            font = TransformFontSizeToNative(font);

            StringUtils.InvokeWithNativeText(text, span =>
            {
                dc.DrawText(
                    span,
                    TransformPointToNative(location),
                    (UI.Native.Font)font.Handler,
                    foreColor,
                    brush,
                    MathUtils.ToRadians(angle) + angle2,
                    useBrush);
            });
        }

        protected virtual bool GetNoTransformToNative()
        {
            if (!WxGlobalSettings.InternalGraphicsTransform || !HasTransform)
                return true;
            return false;
        }

        protected virtual ReadOnlySpan<PointD> TransformPointsToNative(ReadOnlySpan<PointD> points)
        {
            if (GetNoTransformToNative())
                return points;

            PointD[] result = new PointD[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                result[i] = Transform.TransformPoint(points[i]);
            }

            return result;
        }

        protected virtual PointD TransformPointToNative(PointD value)
        {
            if(GetNoTransformToNative())
                return value;

            return Transform.TransformPoint(value);
        }

        protected virtual Font TransformFontSizeToNative(Font value)
        {
            if (GetNoTransformToNative())
                return value;
            var scaledSize = Transform.TransformSize(value.Size);
            return value.WithSize(scaledSize.Height);
        }

        protected virtual SizeD TransformSizeToNative(SizeD value)
        {
            if (GetNoTransformToNative())
                return value;
            return Transform.TransformSize(value);
        }

        protected RectD TransformRectToNative(RectD value)
        {
            return (TransformPointToNative(value.Location), TransformSizeToNative(value.Size));
        }
    }
}
