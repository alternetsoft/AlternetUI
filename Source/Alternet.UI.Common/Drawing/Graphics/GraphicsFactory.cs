using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly AdvDictionary<double, Graphics> memoryCanvases = new();

        private static bool ImageBitsFormatsLoaded = false;
        private static ImageBitsFormat nativeBitsFormat;
        private static ImageBitsFormat alphaBitsFormat;
        private static ImageBitsFormat genericBitsFormat;

        /// <summary>
        /// Gets or sets default dpi value used in conversions pixels from/to device-independent units.
        /// </summary>
        public static int DefaultDPI = 96;

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

        public static ImageBitsFormat GetBitsFormat(ImageBitsFormatKind kind)
        {
            switch (kind)
            {
                case ImageBitsFormatKind.Native:
                    return NativeBitsFormat;
                case ImageBitsFormatKind.Alpha:
                    return AlphaBitsFormat;
                case ImageBitsFormatKind.Generic:
                case ImageBitsFormatKind.Unknown:
                default:
                    return GenericBitsFormat;
            }
        }

        public static unsafe RGBAValue[] PixelsArgbToRgba(
            int width,
            int height,
            ARGBValue* buffer,
            bool opaque)
        {
            var numPixels = width * height;

            if (numPixels <= 0)
                return Array.Empty<RGBAValue>();

            var numBytes = numPixels * 4;

            var result = new RGBAValue[numPixels];

            fixed(RGBAValue* ptrDestColor = result)
            {
                IntPtr ptrDest = (IntPtr)ptrDestColor;
                IntPtr ptrSource = ((IntPtr)buffer) + 1;

                BaseMemory.Move(dest: ptrDest, src: ptrSource, count: numBytes - 1);

                if (opaque)
                {
                    for (int i = 0; i < numPixels; i++)
                    {
                        ptrDestColor[i].A = 255;
                    }
                }
                else
                {
                    for (int i = 0; i < numPixels; i++)
                    {
                        ptrDestColor[i].A = buffer[i].A;
                    }
                }
            }

            return result;
        }

        public static Graphics GetOrCreateMemoryCanvas(double? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);
            var result = memoryCanvases.GetOrCreate(factor, () => CreateMemoryCanvas(factor));
            return result;
        }

        public static Graphics CreateMemoryCanvas(Image image)
        {
            return Handler.CreateMemoryCanvas(image);
        }

        public static Graphics CreateMemoryCanvas(double? scaleFactor = null)
        {
            return Handler.CreateMemoryCanvas(ScaleFactorOrDefault(scaleFactor));
        }

        public static ISkiaSurface CreateSkiaSurface(Image image, ImageLockMode lockMode)
        {
            Debug.Assert(image.IsOk, "Image.IsOk == true is required.");

            if (image.Handler is SkiaImageHandler skiaHandler)
                return new SkiaSurfaceOnSkia(skiaHandler.Bitmap, lockMode);

            Debug.Assert(!image.HasMask, "Image.HasMask == false is required.");

            var formatKind = image.Handler.BitsFormat;
            var format = GraphicsFactory.GetBitsFormat(formatKind);

            if (!image.HasAlpha || App.IsMacOS || formatKind == ImageBitsFormatKind.Unknown
                || format.ColorType == SKColorType.Unknown)
                return CreateUsingGenericImage();

            return new SkiaSurfaceOnBitmap(image, lockMode);

            ISkiaSurface CreateUsingGenericImage()
            {
                App.DebugLogIf("CreateSkiaSurface for image using GenericImage", false);

                SKBitmap bitmap = Image.ToSkia(image, lockMode.CanRead());

                var result = new SkiaSurfaceOnSkia(bitmap, lockMode);

                result.Disposed += (s, e) =>
                {
                    if (lockMode.CanWrite())
                        image.Assign(bitmap);
                };

                return result;
            }
        }

        public static ISkiaSurface CreateSkiaSurface(GenericImage image, ImageLockMode lockMode)
        {
            SKBitmap bitmap = GenericImage.ToSkia(image, lockMode.CanRead());

            var result = new SkiaSurfaceOnSkia(bitmap, lockMode);

            result.Disposed += (s, e) =>
            {
                if(lockMode.CanWrite())
                    image.Assign(bitmap);
            };

            return result;
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
            paint.StrokeWidth = (float)(pen.Width);
            paint.IsStroke = true;
            return paint;
        }

        public static Coord ScaleFactorFromDpi(int dpi)
        {
            if (dpi == DefaultDPI || dpi <=0)
                return 1;
            return (Coord)dpi / (Coord)DefaultDPI;
        }

        public static int ScaleFactorToDpi(Coord scaleFactor)
        {
            if (scaleFactor == 1)
                return DefaultDPI;
            return (int)(scaleFactor * DefaultDPI);
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI PixelFromDip(SizeD value, Coord? scaleFactor = null)
        {
            return new(PixelFromDip(value.Width, scaleFactor), PixelFromDip(value.Height, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI PixelFromDip(PointD value, Coord? scaleFactor = null)
        {
            return new(PixelFromDip(value.X, scaleFactor), PixelFromDip(value.Y, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectI PixelFromDip(RectD value, Coord? scaleFactor = null)
        {
            return new(PixelFromDip(value.Location, scaleFactor), PixelFromDip(value.Size, scaleFactor));
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD PixelToDip(SizeI value, Coord? scaleFactor = null)
        {
            return new(PixelToDip(value.Width, scaleFactor), PixelToDip(value.Height, scaleFactor));
        }

        public static Coord ScaleFactorOrDefault(Coord? scaleFactor = null)
        {
            return scaleFactor ?? Display.MaxScaleFactor;
        }

        public static RectD[] PixelToDip(RectI[] rects, Coord? scaleFactor = null)
        {
            var length = rects.Length;
            var result = new RectD[length];
            for (int i = 0; i < length; i++)
                result[i] = rects[i].PixelToDip(scaleFactor);
            return result;
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD PixelToDip(PointI value, Coord? scaleFactor = null)
        {
            return new(PixelToDip(value.X, scaleFactor), PixelToDip(value.Y, scaleFactor));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD PixelToDip(RectI value, Coord? scaleFactor = null)
        {
            return new(PixelToDip(value.Location, scaleFactor), PixelToDip(value.Size, scaleFactor));
        }

        public static int PixelFromDip(Coord value, Coord? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);

            if (factor == 1)
                return (int)value;
            return (int)Math.Round(value * factor);
        }

        public static Coord PixelToDip(int value, Coord? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);

            if (factor == 1)
                return value;
            else
                return value / factor;
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

            SKFont skiaFont = new(typeFace, (float)(font.SizeInDips));
            return skiaFont;
        }
    }
}
