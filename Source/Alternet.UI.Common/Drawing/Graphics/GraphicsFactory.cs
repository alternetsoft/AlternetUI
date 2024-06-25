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
        /// <summary>
        /// Gets or sets default dpi value used when pixels are converted from/to device-independent units.
        /// </summary>
        public static int DefaultDPI = 96;

        /// <summary>
        /// Gets or sets default scaling quality used when images are scaled.
        /// </summary>
        public static SKFilterQuality DefaultScaleQuality = SKFilterQuality.High;

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Font"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.Fill"/> style.
        /// </summary>
        public static Func<Font, SKPaint> FontToFillPaint
            = (font) => GraphicsFactory.CreateFillPaint(font.SkiaFont);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Font"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.StrokeAndFill"/> style.
        /// </summary>
        public static Func<Font, SKPaint> FontToStrokeAndFillPaint
            = (font) => GraphicsFactory.CreateStrokeAndFillPaint(font.SkiaFont);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Font"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.Stroke"/> style.
        /// </summary>
        public static Func<Font, SKPaint> FontToStrokePaint
            = (font) => GraphicsFactory.CreateStrokePaint(font.SkiaFont);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Color"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.Fill"/> style.
        /// </summary>
        public static Func<Color, SKPaint> ColorToFillPaint
            = (color) => GraphicsFactory.CreateFillPaint(color);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Color"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.StrokeAndFill"/> style.
        /// </summary>
        public static Func<Color, SKPaint> ColorToStrokeAndFillPaint
            = (color) => GraphicsFactory.CreateStrokeAndFillPaint(color);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Color"/> is converted to <see cref="SKPaint"/>
        /// with <see cref="SKPaintStyle.StrokeAndFill"/> style.
        /// </summary>
        public static Func<Color, SKPaint> ColorToStrokePaint
            = (color) => GraphicsFactory.CreateStrokePaint(color);

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Pen"/> is converted to <see cref="SKPaint"/>.
        /// </summary>
        public static Func<Pen, SKPaint> PenToPaint = DefaultPenToPaint;

        /// <summary>
        /// Gets or sets function which is used when
        /// <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        public static Func<Font, SKFont> FontToSkiaFont = DefaultFontToSkiaFont;

        /// <summary>
        /// Gets or sets default value for <see cref="SKPaint.IsAntialias"/> property
        /// when <see cref="SKPaint"/> is created by the conversion methods.
        /// </summary>
        public static bool DefaultAntialias = true;

        private static readonly AdvDictionary<double, Graphics> MemoryCanvases = new();

        private static bool imageBitsFormatsLoaded = false;
        private static ImageBitsFormat nativeBitsFormat;
        private static ImageBitsFormat alphaBitsFormat;
        private static ImageBitsFormat genericBitsFormat;
        private static IGraphicsFactoryHandler? handler;

        static GraphicsFactory()
        {
        }

        /// <summary>
        /// Occurs when <see cref="SKPaint"/> instance is created by one of the create methods
        /// implemented in the <see cref="GraphicsFactory"/>.
        /// </summary>
        public static event EventHandler? PaintCreated;

        /// <summary>
        /// Gets or sets <see cref="IGraphicsFactoryHandler"/> object which is used internally
        /// to perform all the operations.
        /// </summary>
        public static IGraphicsFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateGraphicsFactoryHandler();

            set => handler = value;
        }

        /// <summary>
        /// Gets or sets <see cref="ImageBitsFormat"/> for the opaque images.
        /// </summary>
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

        /// <summary>
        /// Gets or sets <see cref="ImageBitsFormat"/> for the images with alpha chanell.
        /// </summary>
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

        /// <summary>
        /// Gets or sets <see cref="ImageBitsFormat"/> for the <see cref="GenericImage"/> images.
        /// </summary>
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

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the specified <see cref="ImageBitsFormatKind"/>.
        /// </summary>
        /// <param name="kind">Kind of the image bits format.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts ARGB to RGBA pixels.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="buffer">Buffer with pixels.</param>
        /// <param name="opaque">Is image opaque or not.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets memory canvas for the specified scale factor. This canvas can be used
        /// to perform text measure.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        public static Graphics GetOrCreateMemoryCanvas(double? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);
            var result = MemoryCanvases.GetOrCreate(factor, () => CreateMemoryCanvas(factor));
            return result;
        }

        /// <summary>
        /// Creates memory canvas for the specified image.
        /// </summary>
        /// <param name="image">Image on which canvas is created.</param>
        /// <returns></returns>
        public static Graphics CreateMemoryCanvas(Image image)
        {
            return Handler.CreateMemoryCanvas(image);
        }

        /// <summary>
        /// Creates memory canvas for the specified scale factor. This canvas can be used
        /// to perform text measure.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        public static Graphics CreateMemoryCanvas(double? scaleFactor = null)
        {
            return Handler.CreateMemoryCanvas(ScaleFactorOrDefault(scaleFactor));
        }

        /// <summary>
        /// Creates <see cref="ISkiaSurface"/> object for the specified image.
        /// </summary>
        /// <param name="image">Image on which <see cref="ISkiaSurface"/> object is created.</param>
        /// <param name="lockMode">Image lock mode.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates <see cref="ISkiaSurface"/> object for the specified generic image.
        /// </summary>
        /// <param name="image">Image on which <see cref="ISkiaSurface"/> object is created.</param>
        /// <param name="lockMode">Image lock mode.</param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is called by all <see cref="SKPaint"/> create methods.
        /// It raises <see cref="PaintCreated"/> event and initializes <see cref="SKPaint"/>
        /// instance properties with the default values.
        /// </summary>
        /// <param name="paint"></param>
        public static void SetPaintDefaults(SKPaint paint)
        {
            paint.IsAntialias = GraphicsFactory.DefaultAntialias;
            PaintCreated?.Invoke(paint, EventArgs.Empty);
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.Fill"/> style
        /// for the specified <see cref="SKColor"/> value.
        /// </summary>
        /// <param name="color">Color for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateFillPaint(SKColor color)
        {
            var result = new SKPaint();
            result.Color = color;
            result.Style = SKPaintStyle.Fill;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.Stroke"/> style
        /// for the specified <see cref="SKColor"/> value.
        /// </summary>
        /// <param name="color">Color for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateStrokePaint(SKColor color)
        {
            var result = new SKPaint();
            result.Color = color;
            result.Style = SKPaintStyle.Stroke;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.StrokeAndFill"/> style
        /// for the specified <see cref="SKColor"/> value.
        /// </summary>
        /// <param name="color">Color for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateStrokeAndFillPaint(SKColor color)
        {
            var result = new SKPaint();
            result.Color = color;
            result.Style = SKPaintStyle.StrokeAndFill;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.Stroke"/> style
        /// for the specified <see cref="SKFont"/> value.
        /// </summary>
        /// <param name="font">Font for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateStrokePaint(SKFont font)
        {
            var result = new SKPaint(font);
            result.Style = SKPaintStyle.Stroke;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.Fill"/> style
        /// for the specified <see cref="SKFont"/> value.
        /// </summary>
        /// <param name="font">Font for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            result.Style = SKPaintStyle.Fill;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> with <see cref="SKPaintStyle.StrokeAndFill"/> style
        /// for the specified <see cref="SKFont"/> value.
        /// </summary>
        /// <param name="font">Font for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint CreateStrokeAndFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            result.Style = SKPaintStyle.StrokeAndFill;
            SetPaintDefaults(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/>
        /// for the specified <see cref="Pen"/> value.
        /// </summary>
        /// <param name="pen">Pen for which <see cref="SKPaint"/> is created.</param>
        /// <returns></returns>
        public static SKPaint DefaultPenToPaint(Pen pen)
        {
            var paint = GraphicsFactory.CreateStrokePaint(pen.Color);
            paint.StrokeCap = pen.LineCap.ToSkia();
            paint.StrokeJoin = pen.LineJoin.ToSkia();
            paint.StrokeWidth = (float)pen.Width;
            paint.IsStroke = true;
            return paint;
        }

        /// <summary>
        /// Converts dpi to scale factor.
        /// </summary>
        /// <param name="dpi">Dpi value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Uses <see cref="DefaultDPI"/> for the conversion.
        /// </remarks>
        public static Coord ScaleFactorFromDpi(int dpi)
        {
            if (dpi == DefaultDPI || dpi <= 0)
                return 1;
            return (Coord)dpi / (Coord)DefaultDPI;
        }

        /// <summary>
        /// Converts scale factor to dpi.
        /// </summary>
        /// <param name="scaleFactor">Scale factor value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Uses <see cref="DefaultDPI"/> for the conversion.
        /// </remarks>
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
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI PixelFromDip(SizeD value, Coord? scaleFactor = null)
        {
            return new(PixelFromDip(value.Width, scaleFactor), PixelFromDip(value.Height, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
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
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
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
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD PixelToDip(SizeI value, Coord? scaleFactor = null)
        {
            return new(PixelToDip(value.Width, scaleFactor), PixelToDip(value.Height, scaleFactor));
        }

        /// <summary>
        /// Validates <paramref name="scaleFactor"/> and returns default value if it is not specified.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        public static Coord ScaleFactorOrDefault(Coord? scaleFactor = null)
        {
            return scaleFactor ?? Display.MaxScaleFactor;
        }

        /// <summary>
        /// Converts array of <see cref="RectI"/> to device-independent units.
        /// </summary>
        /// <param name="rects">Array of <see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
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
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
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
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD PixelToDip(RectI value, Coord? scaleFactor = null)
        {
            return new(PixelToDip(value.Location, scaleFactor), PixelToDip(value.Size, scaleFactor));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        public static int PixelFromDip(Coord value, Coord? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);

            if (factor == 1)
                return (int)value;
            return (int)Math.Round(value * factor);
        }

        /// <summary>
        /// Converts pixels to device-independent units.
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        public static Coord PixelToDip(int value, Coord? scaleFactor = null)
        {
            var factor = ScaleFactorOrDefault(scaleFactor);

            if (factor == 1)
                return value;
            else
                return value / factor;
        }

        /// <summary>
        /// Default implementation of the <see cref="Font"/> to <see cref="SKFont"/> conversion.
        /// </summary>
        /// <param name="font">Font to convert.</param>
        /// <returns></returns>
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

            SKFont skiaFont = new(typeFace, (float)font.SizeInDips);
            return skiaFont;
        }

        private static void LoadImageBitsFormats()
        {
            if (imageBitsFormatsLoaded)
                return;
            imageBitsFormatsLoaded = true;
            nativeBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Native);
            alphaBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Alpha);
            genericBitsFormat = Handler.GetImageBitsFormat(ImageBitsFormatKind.Generic);
        }
    }
}
