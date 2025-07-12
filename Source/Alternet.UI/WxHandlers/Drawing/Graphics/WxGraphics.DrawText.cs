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

            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                default);

            return result;
        }

        public SizeD GetTextExtent(string text, Font font, Control? control)
        {
            if (text is null || text.Length == 0)
                return SizeD.Empty;

            IntPtr controlPtr = default;

            if(control is not null)
            {
                var wxHandler = control.Handler as WxControlHandler;
                controlPtr = wxHandler?.NativeControl.WxWidget ?? default;
            }

            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                controlPtr);
            return result;
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DoInsideClipped(TransformRectToNative(bounds), () =>
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

            double angle;

            if (GetNoTransformToNative())
                angle = 0;
            else
                angle = Transform.GetRotationAngleInRadians();

            font = TransformFontSizeToNative(font);
            dc.DrawText(
                text,
                TransformPointToNative(location),
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                angle);
        }

        protected virtual bool GetNoTransformToNative()
        {
            if (!WxGlobalSettings.InternalGraphicsTransform || !HasTransform)
                return true;
            return false;
        }

        protected virtual PointD[] TransformPointsToNative(PointD[] points)
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
