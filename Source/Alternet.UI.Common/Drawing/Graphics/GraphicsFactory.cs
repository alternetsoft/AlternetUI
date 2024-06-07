using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static bool ImageBitsFormatsLoaded = false;
        private static ImageBitsFormat nativeBitsFormat;
        private static ImageBitsFormat alphaBitsFormat;
        private static ImageBitsFormat genericBitsFormat;

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

        public static ImageBitsFormat NativeBitsFormat
        {
            get
            {
                LoadImageBitsFormats();
                return nativeBitsFormat;
            }

            set
            {
                LoadImageBitsFormats();
                nativeBitsFormat = value;
            }
        }

        public static ImageBitsFormat AlphaBitsFormat
        {
            get
            {
                LoadImageBitsFormats();
                return alphaBitsFormat;
            }

            set
            {
                LoadImageBitsFormats();
                alphaBitsFormat = value;
            }
        }

        public static ImageBitsFormat GenericBitsFormat
        {
            get
            {
                LoadImageBitsFormats();
                return genericBitsFormat;
            }

            set
            {
                LoadImageBitsFormats();
                genericBitsFormat = value;
            }
        }

        private static void LoadImageBitsFormats()
        {
            if (ImageBitsFormatsLoaded)
                return;
            ImageBitsFormatsLoaded = true;
            nativeBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Native);
            alphaBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Alpha);
            genericBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Generic);
        }

        public static ISkiaSurface CreateSkiaBitmapData(ILockImageBits image)
        {
            return new SkiaSurfaceOnBitmap(image);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coord ScaleFactorFromDpi(int dpi)
        {
            if (dpi == 96 || dpi <=0)
                return 1;
            return (Coord)dpi / (Coord)96;
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI PixelFromDip(SizeD value, Coord scaleFactor)
        {
            return new(PixelFromDip(value.Width, scaleFactor), PixelFromDip(value.Height, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI PixelFromDip(PointD value, Coord scaleFactor)
        {
            return new(PixelFromDip(value.X, scaleFactor), PixelFromDip(value.Y, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectI PixelFromDip(RectD value, Coord scaleFactor)
        {
            return new(PixelFromDip(value.Location, scaleFactor), PixelFromDip(value.Size, scaleFactor));
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD PixelToDip(SizeI value, Coord scaleFactor)
        {
            return new(PixelToDip(value.Width, scaleFactor), PixelToDip(value.Height, scaleFactor));
        }

        public static RectD[] PixelToDip(RectI[] rects, Coord scaleFactor)
        {
            var length = rects.Length;
            var result = new RectD[length];
            for (int i = 0; i < length; i++)
                result[i] = rects[i].PixelToDip(scaleFactor);
            return result;
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD PixelToDip(PointI value, Coord scaleFactor)
        {
            return new(PixelToDip(value.X, scaleFactor), PixelToDip(value.Y, scaleFactor));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD PixelToDip(RectI value, Coord scaleFactor)
        {
            return new(PixelToDip(value.Location, scaleFactor), PixelToDip(value.Size, scaleFactor));
        }

        public static int PixelFromDip(Coord value, Coord scaleFactor)
        {
            if (scaleFactor == 1)
                return (int)value;
            return (int)Math.Round(value * scaleFactor);
        }

        public static Coord PixelToDip(int value, Coord scaleFactor)
        {
            if (scaleFactor == 1)
                return value;
            else
                return value / scaleFactor;
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
