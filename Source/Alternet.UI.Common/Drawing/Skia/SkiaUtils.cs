using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.UI.Extensions;
using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static methods and properties related to SkiaSharp drawing.
    /// </summary>
    public static class SkiaUtils
    {
        /// <summary>
        /// Contains <see cref="SKPaint"/> object used when images are painted with the specific
        /// <see cref="InterpolationMode"/>.
        /// </summary>
        public static readonly EnumArray<InterpolationMode, SKPaint?> InterpolationModePaints = new();

        private static string[]? fontFamilies;
        private static FontScalar defaultFontSize = 10;
        private static string? defaultMonoFontName;
        private static SKFont? defaultSkiaFont;
        private static SKTypeface? defaultTypeFace;
        private static SKColorFilter? grayscaleColorFilter;

        static SkiaUtils()
        {
        }

        /// <summary>
        /// Gets or sets default font for use with SkiaSharp.
        /// </summary>
        public static SKFont DefaultFont
        {
            get => defaultSkiaFont ??= CreateDefaultFont();
            set => defaultSkiaFont = value ?? CreateDefaultFont();
        }

        /// <summary>
        /// Gets or sets default font size for use with SkiaSharp.
        /// </summary>
        public static Coord DefaultFontSize
        {
            get => defaultSkiaFont?.Size ?? defaultFontSize;

            set
            {
                if (DefaultFontSize == value)
                    return;
                defaultFontSize = value;
                defaultSkiaFont = null;
            }
        }

        /// <summary>
        /// Gets or sets default <see cref="SKTypeface"/> using to create default SkiaSharp font.
        /// </summary>
        public static SKTypeface DefaultTypeFace
        {
            get
            {
                return defaultSkiaFont?.Typeface ?? defaultTypeFace ?? SKTypeface.Default;
            }

            set
            {
                if (DefaultTypeFace == value)
                    return;
                defaultTypeFace = value;
                defaultSkiaFont = null;
            }
        }

        /// <summary>
        /// Gets or sets default font name for use with SkiaSharp.
        /// </summary>
        public static string DefaultFontName
        {
            get => DefaultTypeFace.FamilyName;

            set
            {
                if (DefaultFontName == value)
                    return;
                DefaultTypeFace = SKTypeface.FromFamilyName(value) ?? SKTypeface.Default;
            }
        }

        /// <summary>
        /// Gets or sets default fixed-pitch font name.
        /// </summary>
        public static string DefaultMonoFontName
        {
            get => defaultMonoFontName ?? DefaultTypeFace.FamilyName;
            set => defaultMonoFontName = value;
        }

        /// <summary>
        /// Gets all installed font families as array of strings.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the <see cref="SKColorFilter"/> used to convert images to grayscale.
        /// </summary>
        /// <remarks>
        /// This property provides a reusable grayscale color filter for SkiaSharp drawing operations.
        /// The filter is lazily initialized on first access using a color matrix that converts
        /// colors to grayscale by applying weighted sums to the red, green, and blue channels.
        /// You can also set this property to use a custom <see cref="SKColorFilter"/>.
        /// </remarks>
        public static SKColorFilter GrayscaleColorFilter
        {
            get
            {
                if(grayscaleColorFilter is null)
                {
                    grayscaleColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                    {
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0,     0,     0,     1, 0,
                    });
                }

                return grayscaleColorFilter;
            }

            set
            {
                grayscaleColorFilter = value;
            }
        }

        /// <summary>
        /// Gets all installed font families as enumerable.
        /// </summary>
        public static IEnumerable<string> GetFontFamiliesNames()
        {
            return FontFamilies;
        }

        /// <summary>
        /// Converts an <see cref="SKImage"/> to an <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="image">The <see cref="SKImage"/> to convert.</param>
        /// <returns>
        /// A new <see cref="SKBitmap"/> instance containing the pixel data
        /// from the specified <paramref name="image"/>.
        /// </returns>
        public static SKBitmap ImageToBitmap(SKImage image)
        {
            SKBitmap result = new (image.Width, image.Height);
            image.ReadPixels(result.Info, result.GetPixels(), result.RowBytes, 0, 0);
            return result;
        }

        /// <summary>
        /// Converts the specified <see cref="Image"/> to a grayscale version.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> to convert to grayscale.</param>
        /// <returns>
        /// A new <see cref="Image"/> instance containing the grayscale version of the input image.
        /// </returns>
        public static Image ConvertToGrayscale(Image image)
        {
            var result = SkiaUtils.ConvertToGrayscale((SKBitmap)image);
            return (Bitmap)result;
        }

        /// <summary>
        /// Converts the specified <see cref="SKBitmap"/> to a grayscale version using a color filter.
        /// </summary>
        /// <param name="bitmap">The <see cref="SKBitmap"/> to convert to grayscale.</param>
        /// <returns>
        /// A new <see cref="SKBitmap"/> instance containing the grayscale version of the input bitmap.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="bitmap"/> parameter is <c>null</c>.
        /// </exception>
        public static SKBitmap ConvertToGrayscale(SKBitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            using SKImage image = SKImage.FromBitmap(bitmap);
            using SKSurface surface = SKSurface.Create(image.Info);
            using SKCanvas canvas = surface.Canvas;

            using SKPaint paint = new() { ColorFilter = GrayscaleColorFilter };
            canvas.DrawBitmap(bitmap, 0, 0, paint);

            var resultImage = surface.Snapshot();

            return ImageToBitmap(resultImage);
        }

        /// <summary>
        /// Gets whether or not specified font is supported in SkiaSharp.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <returns></returns>
        public static bool IsFamilySkia(string name)
        {
            var index = Array.BinarySearch<string>(FontFamilies, name);
            return index >= 0;
        }

        /// <summary>
        /// Creates <see cref="SkiaGraphics"/> on the specified canvas
        /// with the specified scaling factor. The canvas is additionally scaled by the factor.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which to create the graphics.</param>
        /// <param name="scaleFactor">The scaling factor.</param>
        /// <returns></returns>
        public static SkiaGraphics CreateSkiaGraphicsOnCanvas(SKCanvas canvas, float scaleFactor)
        {
            var graphics = new Drawing.SkiaGraphics(canvas);

            graphics.OriginalScaleFactor = scaleFactor;
            graphics.UseUnscaledDrawImage = true;
            graphics.InitialMatrix = canvas.TotalMatrix;

            canvas.Scale(scaleFactor);

            return graphics;
        }

        /// <summary>
        /// Creates measure canvas for the specified scaling factor.
        /// </summary>
        /// <param name="scaleFactor">Scaling factor.</param>
        /// <returns></returns>
        public static SkiaGraphics CreateMeasureCanvas(Coord scaleFactor)
        {
            return CreateBitmapCanvas(SizeD.Empty, scaleFactor, true);
        }

        /// <summary>
        /// Creates canvas on the bitmap with the specified size and scaling factor.
        /// </summary>
        /// <param name="scaleFactor">Scaling factor.</param>
        /// <param name="size">Size of the bitmap.</param>
        /// <param name="isTransparent">Whether canvas is transparent.</param>
        /// <returns></returns>
        public static SkiaGraphics CreateBitmapCanvas(
            SizeD size,
            Coord scaleFactor,
            bool isTransparent = true)
        {
            var boundsInPixels = GraphicsFactory.PixelFromDip(size);

            SKBitmap bitmap;

            if (size == SizeD.Empty)
            {
                bitmap = new();
            }
            else
            {
                bitmap = new(boundsInPixels.Width, boundsInPixels.Height, !isTransparent);
            }

            var result = CreateBitmapCanvas(bitmap, scaleFactor);
            return result;
        }

        /// <summary>
        /// Recreates a cached bitmap canvas with the specified size, scale factor, and transparency settings.
        /// </summary>
        /// <remarks>This method ensures that the cached bitmap canvas matches the specified parameters.
        /// If the existing canvas does not match, it is disposed and replaced with a new instance.
        /// The method is designed to optimize resource usage by reusing the existing canvas when possible.</remarks>
        /// <param name="cachedCanvas">A reference to the cached bitmap canvas. If the canvas
        /// is <see langword="null"/> or does not match the
        /// specified parameters, it will be replaced with a new instance.</param>
        /// <param name="size">The dimensions of the bitmap canvas, specified as a <see cref="SizeD"/> structure.</param>
        /// <param name="scaleFactor">The scaling factor to apply to the bitmap canvas, specified
        /// as a <see cref="Coord"/> value.</param>
        /// <param name="isTransparent">A value indicating whether the bitmap canvas should support transparency.
        /// The default value is <see langword="true"/>.</param>
        public static void RecreateBitmapCanvas(
            ref BitmapCanvasCached? cachedCanvas,
            SizeD size,
            Coord scaleFactor,
            bool isTransparent = true)
        {
            if(cachedCanvas is null)
            {
                cachedCanvas = CreateNew();
                return;
            }

            if(cachedCanvas.Equals(size, scaleFactor, isTransparent))
                return;

            cachedCanvas.Dispose();
            cachedCanvas = CreateNew();

            BitmapCanvasCached CreateNew()
            {
                var result = new BitmapCanvasCached(size, scaleFactor, isTransparent);
                result.Graphics = CreateBitmapCanvas(size, scaleFactor, isTransparent);
                return result;
            }
        }

        /// <summary>
        /// Creates canvas on the specified bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap to create canvas on.</param>
        /// <param name="scaleFactor">Initial scale factor for the canvas.</param>
        /// <returns></returns>
        public static SkiaGraphics CreateBitmapCanvas(SKBitmap bitmap, Coord scaleFactor)
        {
            var result = new SkiaGraphics(bitmap);
            result.OriginalScaleFactor = (float)scaleFactor;
            result.Canvas.Scale((float)scaleFactor);
            return result;
        }

        /// <summary>
        /// Gets whether or not specified bitmap is ok.
        /// </summary>
        /// <param name="bitmap">Bitmap to check.</param>
        /// <returns></returns>
        public static bool BitmapIsOk(SKBitmap? bitmap)
        {
            return bitmap is not null && bitmap.ReadyToDraw && bitmap.Height > 0 && bitmap.Width > 0;
        }

        /// <summary>
        /// Resets loaded font families.
        /// </summary>
        public static void ResetFonts()
        {
            fontFamilies = null;
        }

        /// <summary>
        /// Gets font family name for the specified <see cref="GenericFontFamily"/> value.
        /// </summary>
        /// <param name="genericFamily">Generic font family.</param>
        /// <returns></returns>
        public static string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.Default)
                return SkiaUtils.DefaultFontName;

            if (genericFamily == GenericFontFamily.Monospace)
            {
                if(defaultMonoFontName is null)
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

        /// <summary>
        /// Creates default font.
        /// </summary>
        /// <returns></returns>
        public static SKFont CreateDefaultFont()
        {
            return new SKFont(DefaultTypeFace, (float)DefaultFontSize);
        }

        /// <summary>
        /// Converts <see cref="FillMode"/> to <see cref="SKPathFillType"/>.
        /// </summary>
        /// <param name="fillMode">Value to convert.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets text size.
        /// </summary>
        /// <param name="canvas">Drawing context.</param>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font.</param>
        /// <returns></returns>
        public static SizeD GetTextExtent(
            this SKCanvas canvas,
            string text,
            Font font)
        {
            var skFont = (SKFont)font;

            var measureResult = skFont.MeasureText(text);

            SizeD result = new(
                measureResult,
                skFont.Metrics.Top.Abs() + skFont.Metrics.Bottom.Abs());

            return result;
        }

        /// <summary>
        /// Draws text with the specified parameters.
        /// </summary>
        /// <param name="canvas">Drawing context.</param>
        /// <param name="s">Text string to draw.</param>
        /// <param name="location">Location where text is drawn on the canvas.</param>
        /// <param name="font">Font.</param>
        /// <param name="foreColor">Foreground color.</param>
        /// <param name="backColor">Background color.
        /// Pass <see cref="Color.Empty"/> to have transparent background under the text.</param>
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

            var offsetX = 0;
            var offsetY = Math.Abs(skFont.Metrics.Top);

            var measureResult = skFont.MeasureText(s);

            var rect = SKRect.Create(
                measureResult,
                skFont.Metrics.Top.Abs() + skFont.Metrics.Bottom.Abs());
            rect.Offset(x, y);

            if (backColor.IsOk)
                canvas.DrawRect(rect, backColor.AsFillPaint);

            canvas.DrawText(s, x + offsetX, y + offsetY, font, foreColor.AsStrokeAndFillPaint);
        }

        /// <summary>
        /// Draws a Bezier spline defined by four <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="startPoint"><see cref="PointD"/> structure that represents the starting
        /// point of the curve.</param>
        /// <param name="controlPoint1"><see cref="PointD"/> structure that represents the first
        /// control point for the curve.</param>
        /// <param name="controlPoint2"><see cref="PointD"/> structure that represents the second
        /// control point for the curve.</param>
        /// <param name="endPoint"><see cref="PointD"/> structure that represents the ending point
        /// of the curve.</param>
        /// <param name="canvas">Drawing context.</param>
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

        /// <summary>
        /// Draws a series of Bezier splines from an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="points">
        /// Array of <see cref="PointD"/> structures that represent the points that
        /// determine the curve.
        /// The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.
        /// </param>
        /// <param name="canvas">Drawing context.</param>
        public static void DrawBeziers(this SKCanvas canvas, Pen pen, ReadOnlySpan<PointD> points)
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

        /// <summary>
        /// Creates dummy <see cref="SKSurface"/> object ignores any painting.
        /// </summary>
        /// <param name="width">Surface width.</param>
        /// <param name="height">Surface height.</param>
        /// <returns></returns>
        public static SKSurface CreateNullSurface(int width = 0, int height = 0)
        {
            return SKSurface.CreateNull(width, height);
        }

        /// <summary>
        /// Creates dummy <see cref="SKCanvas"/> object which performs not painting.
        /// </summary>
        /// <param name="width">Surface width.</param>
        /// <param name="height">Surface height.</param>
        /// <returns></returns>
        public static SKCanvas CreateNullCanvas(int width = 0, int height = 0)
        {
            var surface = CreateNullSurface(width, height);
            return surface.Canvas;
        }

        /// <summary>
        /// Saves the contents of the specified <see cref="SKSurface"/> to a file in the specified image format.
        /// </summary>
        /// <remarks>This method captures the current state of the <paramref name="surface"/> and encodes
        /// it into the specified image format. The resulting image is then saved to the file at
        /// <paramref
        /// name="filePath"/>.</remarks>
        /// <param name="surface">The <see cref="SKSurface"/> containing the image to save.</param>
        /// <param name="filePath">The path of the file where the image will be saved.
        /// The file will be overwritten if it already exists.</param>
        /// <param name="format">The format in which to encode the image. The default
        /// is <see cref="SKEncodedImageFormat.Png"/>.</param>
        /// <param name="quality">The quality of the encoded image, ranging from 0 (lowest) to 100 (highest).
        /// This parameter is ignored for
        /// formats that do not support quality settings. The default is 100.</param>
        public static void SaveSurfaceToFile(
            SKSurface surface,
            string filePath,
            SKEncodedImageFormat format = SKEncodedImageFormat.Png,
            int quality = 100)
        {
            using var image = surface.Snapshot();
            using var data = image.Encode(format, quality);
            using var stream = File.OpenWrite(filePath);
            data.SaveTo(stream);
        }

        /// <summary>
        /// Gets bounds of all the rectangles in the collection.
        /// </summary>
        /// <param name="rects">Collection of the rectangles</param>
        /// <returns></returns>
        public static RectI? GetBounds(IEnumerable<RectI> rects)
        {
            SKRegion region = new();
            var hasRects = false;

            foreach(var rect in rects)
            {
                region.Op(rect, SKRegionOperation.Union);
                hasRects = true;
            }

            if (hasRects)
            {
                var result = region.Bounds;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Creates <see cref="SKCanvas"/> on the memory buffer and calls specified action.
        /// </summary>
        /// <param name="width">Width of the image data.</param>
        /// <param name="height">Height of the image data.</param>
        /// <param name="scan0">The pointer to an in memory-buffer that can hold
        /// the image as specified.</param>
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

        /// <summary>
        /// Represents a cached bitmap canvas, including its graphics context, size,
        /// scale factor, and transparency.
        /// </summary>
        public class BitmapCanvasCached : IDisposable
        {
            /// <summary>
            /// Gets or sets the <see cref="SkiaGraphics"/> instance used for drawing on the bitmap.
            /// </summary>
            public SkiaGraphics? Graphics;

            /// <summary>
            /// Gets or sets the size of the bitmap canvas in device-independent units.
            /// </summary>
            public SizeD Size;

            /// <summary>
            /// Gets or sets the scale factor used for the bitmap canvas.
            /// </summary>
            public Coord ScaleFactor;

            /// <summary>
            /// Gets or sets a value indicating whether the bitmap canvas is transparent.
            /// </summary>
            public bool IsTransparent;

            /// <summary>
            /// Gets or sets a value indicating whether the <see cref="Graphics"/>
            /// instance should be disposed when this instance is disposed.
            /// </summary>
            public bool DisposeGraphics = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="BitmapCanvasCached"/> class with default values.
            /// </summary>
            public BitmapCanvasCached()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BitmapCanvasCached"/> class
            /// with the specified size, scale factor, and transparency.
            /// </summary>
            /// <param name="size">The size of the bitmap canvas.</param>
            /// <param name="scaleFactor">The scale factor for the bitmap canvas.</param>
            /// <param name="isTransparent">Indicates whether the bitmap canvas is transparent.</param>
            public BitmapCanvasCached(SizeD size, Coord scaleFactor, bool isTransparent = true)
            {
                Size = size;
                ScaleFactor = scaleFactor;
                IsTransparent = isTransparent;
            }

            /// <summary>
            /// Determines whether the specified specified size, scale factor,
            /// and transparency flag are equal to those used in the current instance.
            /// </summary>
            /// <param name="size">The size to compare with the value stored in the current instance.</param>
            /// <param name="scaleFactor">The scale factor to compare with the value
            /// stored in the current instance.</param>
            /// <param name="isTransparent">A value indicating whether the transparency flag to compare
            /// matches the value stored in the current instance.</param>
            /// <returns><see langword="true"/> if the specified <see cref="SizeD"/>, <see cref="Coord"/>,
            /// and transparency flag  are equal to the values stored in the current instance;
            /// otherwise, <see langword="false"/>.</returns>
            public bool Equals(SizeD size, Coord scaleFactor, bool isTransparent)
            {
                return Size == size && ScaleFactor == scaleFactor && IsTransparent == isTransparent;
            }

            /// <summary>
            /// Releases resources used by the <see cref="BitmapCanvasCached"/> class,
            /// optionally disposing the <see cref="Graphics"/> instance.
            /// </summary>
            public void Dispose()
            {
                if (!DisposeGraphics)
                    return;
                Graphics?.Dispose();
                Graphics = null;
            }
        }
    }
}
