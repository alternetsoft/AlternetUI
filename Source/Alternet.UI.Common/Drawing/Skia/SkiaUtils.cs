using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.UI.Extensions;
using SkiaSharp;

namespace Alternet.Drawing
{
    public static class SkiaUtils
    {
        private static string[]? fontFamilies;
        private static FontSize defaultFontSize = 12;
        private static string? defaultFontName;
        private static string? defaultMonoFontName;
        private static SKFont? defaultSkiaFont;

        public static SKFont DefaultFont
        {
            get => defaultSkiaFont ??= CreateDefaultFont();
            set => defaultSkiaFont = value ?? CreateDefaultFont();
        }

        public static double DefaultFontSize
        {
            get => defaultFontSize;

            set
            {
                defaultFontSize = value;
            }
        }

        public static string DefaultFontName
        {
            get => defaultFontName ?? SKTypeface.Default.FamilyName;
            set => defaultFontName = value;
        }

        public static string DefaultMonoFontName
        {
            get => defaultMonoFontName ?? SKTypeface.Default.FamilyName;
            set => defaultMonoFontName = value;
        }

        public static string[] FontFamilies
        {
            get
            {
                if (fontFamilies is null)
                {
                    fontFamilies = SKFontManager.Default.GetFontFamilies();
                    Array.Sort(fontFamilies);
                }

                return fontFamilies;
            }
        }

        public static IEnumerable<string> GetFontFamiliesNames()
        {
            return FontFamilies;
        }

        public static bool IsFamilySkia(string name)
        {
            var index = Array.BinarySearch<string>(FontFamilies, name);
            return index >= 0;
        }

        public static bool BitmapIsOk(SKBitmap? bitmap)
        {
            return bitmap is not null && bitmap.ReadyToDraw && bitmap.Height > 0 && bitmap.Width > 0;
        }

        public static void ResetFonts()
        {
            fontFamilies = null;
        }

        public static string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.Default)
                return SkiaUtils.DefaultFontName;

            if (genericFamily == GenericFontFamily.Monospace)
            {
                if(DefaultMonoFontName is null)
                {
                    var result = FontFactory.GetSampleFixedPitchFont();
                    if (result is not null)
                    {
                        DefaultMonoFontName = result;
                        return result;
                    }
                    else
                        return SkiaUtils.DefaultFontName;
                }
                else
                {
                    return SkiaUtils.DefaultMonoFontName;
                }
            }

            var nameAndSize = FontFactory.GetSampleNameAndSize(genericFamily);

            if (!FontFamily.IsFamilyValid(nameAndSize.Name))
                return SkiaUtils.DefaultFontName;

            return nameAndSize.Name;
        }

        public static SKFont CreateDefaultFont()
        {
            return new SKFont(SKTypeface.Default, (float)DefaultFontSize);
        }

        public static SKPathFillType ToSkia(this FillMode fillMode)
        {
            switch (fillMode)
            {
                case FillMode.Alternate:
                    return SKPathFillType.EvenOdd;
                case FillMode.Winding:
                    return SKPathFillType.Winding;
                default:
                    return SKPathFillType.EvenOdd;
            }
        }

        public static SizeD GetTextExtent(
            this SKCanvas canvas,
            string text,
            Font font)
        {
            var skFont = (SKFont)font;
            var skFontPaint = font.AsStrokeAndFillPaint;

            var measure = SKRect.Empty;
            var measureResult = skFontPaint.MeasureText(text, ref measure);

            SizeD result = new(
                measureResult,
                skFont.Metrics.Top.Abs() + skFont.Metrics.Bottom.Abs());

            return result;
        }

        public static void DrawText(
            this SKCanvas canvas,
            string s,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            float x = (float)location.X;
            float y = (float)location.Y;

            var skFont = (SKFont)font;
            var skFontPaint = font.AsStrokeAndFillPaint;

            var offsetX = 0;
            var offsetY = Math.Abs(skFont.Metrics.Top);

            var measure = SKRect.Empty;
            var measureResult = skFontPaint.MeasureText(s, ref measure);

            var rect = SKRect.Create(
                measureResult,
                skFont.Metrics.Top.Abs() + skFont.Metrics.Bottom.Abs());
            rect.Offset(x, y);

            if (backColor.IsOk)
                canvas.DrawRect(rect, backColor.AsFillPaint);

            canvas.DrawText(s, x + offsetX, y + offsetY, font, foreColor.AsStrokeAndFillPaint);
        }

        public static void DrawBezier(
            this SKCanvas canvas,
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            Graphics.DebugPenAssert(pen);
            SKPath path = new();
            path.MoveTo(startPoint);
            path.CubicTo(controlPoint1, controlPoint2, endPoint);
            canvas.DrawPath(path, pen);
        }

        public static void DrawBeziers(this SKCanvas canvas, Pen pen, PointD[] points)
        {
            var pointsCount = points.Length;
            Graphics.DebugPenAssert(pen);
            Graphics.DebugBezierPointsAssert(points);

            SKPath path = new();
            path.MoveTo(points[0]);

            for (int i = 1; i <= pointsCount - 3; i += 3)
            {
                path.CubicTo(points[i], points[i + 1], points[i + 2]);
            }

            canvas.DrawPath(path, pen);
        }

        public static SKSurface CreateNullSurface(int width = 0, int height = 0)
        {
            return SKSurface.CreateNull(width, height);
        }

        public static SKCanvas CreateNullCanvas(int width = 0, int height = 0)
        {
            var surface = CreateNullSurface(width, height);
            return surface.Canvas;
        }

        /// <summary>
        /// Creates <see cref="SKCanvas"/> on the memory buffer and calls specified action.
        /// </summary>
        /// <param name="width">Width of the image data.</param>
        /// <param name="height">Height of the image data.</param>
        /// <param name="scan0">The pointer to an in memory-buffer that can hold the image as specified.</param>
        /// <param name="stride">The number of bytes per row in the pixel buffer.</param>
        /// <param name="dpi">Dpi (dots per inch).</param>
        /// <param name="onRender">Render action.</param>
        internal static void DrawOnPtr(
            int width,
            int height,
            IntPtr scan0,
            int stride,
            float dpi,
            Action<SKSurface> onRender)
        {
            var info = new SKImageInfo(
                width,
                height,
                SKImageInfo.PlatformColorType,
                SKAlphaType.Unpremul);

            using var surface = SKSurface.Create(info, scan0, Math.Abs(stride));
            var canvas = surface.Canvas;
            canvas.Scale(dpi / 96.0f);
            onRender(surface);
            canvas.Flush();
        }

        internal static void SampleDrawText(SKCanvas canvas, string text, SKRect rect, SKPaint paint)
        {
            float spaceWidth = paint.MeasureText(" ");
            float wordX = rect.Left;
            float wordY = rect.Top + paint.TextSize;
            foreach (string word in text.Split(' '))
            {
                float wordWidth = paint.MeasureText(word);
                if (wordWidth <= rect.Right - wordX)
                {
                    canvas.DrawText(word, wordX, wordY, paint);
                    wordX += wordWidth + spaceWidth;
                }
                else
                {
                    wordY += paint.FontSpacing;
                    wordX = rect.Left;
                }
            }
        }
    }
}
