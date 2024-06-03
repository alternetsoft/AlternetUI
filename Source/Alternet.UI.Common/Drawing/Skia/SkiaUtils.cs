using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI;
using Alternet.UI.Extensions;

namespace Alternet.Drawing
{
    public static class SkiaUtils
    {
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

        public static string[] GetFontFamiliesNames()
        {
            return SKFontManager.Default.GetFontFamilies();
        }

        public static string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.Default)
                return SkiaUtils.DefaultFontName;

            var (name, _) = FontFamily.GetSampleFontNameAndSize(genericFamily);

            if (!FontFamily.IsFamilyValid(name))
                name = SkiaUtils.DefaultFontName;

            return name;
        }

        public static SKFont CreateDefaultFont()
        {
            return new SKFont(SKTypeface.Default, (float)DefaultFontSize);
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

        public static SizeD GetTextExtent(
            this SKCanvas canvas,
            string text,
            Font font,
            out Coord? descent,
            out Coord? externalLeading,
            IControl? control = null)
        {
            descent = null;
            externalLeading = null;
            return canvas.GetTextExtent(text, font);
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
    }
}
