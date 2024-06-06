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
    /// <summary>
    /// Provides access to the graphics factory.
    /// </summary>
    public static class GraphicsFactory
    {
        public static SKColorType LockBitsColorType;

        public static SKAlphaType LockBitsAlphaType;

        public static Func<Font, SKPaint> FontToFillPaint
            = (font) => GraphicsFactory.CreateFillPaint(font.SkiaFont);

        public static Func<Font, SKPaint> FontToStrokeAndFillPaint
            = (font) => GraphicsFactory.CreateStrokeAndFillPaint(font.SkiaFont);

        public static Func<Font, SKPaint> FontToStrokePaint
            = (font) => GraphicsFactory.CreateStrokePaint(font.SkiaFont);

        public static Func<Color, SKPaint> ColorToFillPaint
            = (color) => GraphicsFactory.CreateFillPaint(color);

        public static Func<Color, SKPaint> ColorToStrokeAndFillPaint
            = (color) => GraphicsFactory.CreateStrokeAndFillPaint(color);

        public static Func<Color, SKPaint> ColorToStrokePaint
            = (color) => GraphicsFactory.CreateStrokePaint(color);

        public static Func<Pen, SKPaint> PenToPaint = DefaultPenToPaint;

        public static Func<Font, SKFont> FontToSkiaFont = DefaultFontToSkiaFont;

        public static bool DefaultAntialias = true;

        private static IGraphicsFactoryHandler? handler;
        public static SKFilterQuality DefaultScaleQuality = SKFilterQuality.High;

        static GraphicsFactory()
        {
            if (App.IsWindowsOS)
            {
                LockBitsColorType = SKColorType.Bgra8888;
                LockBitsAlphaType = SKAlphaType.Premul;
                return;
            }

            if (App.IsLinuxOS)
            {
                LockBitsColorType = SKColorType.Rgba8888;
                LockBitsAlphaType = SKAlphaType.Unpremul;
                return;
            }

            if (App.IsMacOS)
            {
                LockBitsColorType = SKColorType.Rgba8888;
                LockBitsAlphaType = SKAlphaType.Premul;
            }
        }

        public static IGraphicsFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateGraphicsFactoryHandler();

            set => handler = value;
        }

        public static void SetPaintDefaults(SKPaint paint)
        {
            paint.IsAntialias = GraphicsFactory.DefaultAntialias;
        }

        public static SKPaint CreateFillPaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.Fill;
            return result;
        }

        public static SKPaint CreateStrokePaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.Stroke;
            return result;
        }

        public static SKPaint CreateStrokeAndFillPaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.StrokeAndFill;
            return result;
        }

        public static SKPaint CreateStrokePaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.Stroke;
            return result;
        }

        public static SKPaint CreateFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.Fill;
            return result;
        }

        public static SKPaint CreateStrokeAndFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.StrokeAndFill;
            return result;
        }

        public static SKPaint DefaultPenToPaint(Pen pen)
        {
            var paint = GraphicsFactory.CreateStrokePaint(pen.Color);
            paint.StrokeCap = pen.LineCap.ToSkia();
            paint.StrokeJoin = pen.LineJoin.ToSkia();
            paint.StrokeWidth = (float)(pen.Width * Display.Default.ScaleFactor);
            paint.IsStroke = true;
            return paint;
        }

        public static SKFont DefaultFontToSkiaFont(Font font)
        {
            SKFontStyleWeight skiaWeight = (SKFontStyleWeight)font.Weight;
            SKFontStyleSlant skiaSlant = font.IsItalic ?
                SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

            var typeFace = SKTypeface.FromFamilyName(
                font.Name,
                skiaWeight,
                SKFontStyleWidth.Normal,
                skiaSlant);

            SKFont skiaFont = new(typeFace, (float)(font.SizeInPixels / Display.Default.ScaleFactor));
            return skiaFont;
        }
    }
}
