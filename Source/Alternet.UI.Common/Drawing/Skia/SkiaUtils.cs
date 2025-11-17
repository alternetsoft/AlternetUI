using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Skia;
using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static methods and properties related to SkiaSharp drawing.
    /// </summary>
    public static partial class SkiaUtils
    {
        /// <summary>
        /// Gets a lazily initialized delegate for the SkiaSharp canvas draw points function.
        /// </summary>
        /// <remarks>This property provides a delegate that represents the native SkiaSharp
        /// function for
        /// drawing points on a canvas.
        /// The delegate is initialized only when accessed for the first time, using the
        /// native library symbol lookup.</remarks>
        public static readonly LazyStruct<SkiaHelper.SkCanvasDrawPointsDelegate?> CanvasDrawPointsNative = new(() =>
        {
            return GetNativeLibrarySymbol<SkiaHelper.SkCanvasDrawPointsDelegate>(SkiaHelper.SKCanvasDrawPointsName);
        });

        /// <summary>
        /// Contains <see cref="SKPaint"/> object used when images are painted with the specific
        /// <see cref="InterpolationMode"/>.
        /// </summary>
        public static readonly EnumArray<InterpolationMode, SKPaint?> InterpolationModePaints = new();

        private static IntPtr? nativeLibraryHandle;

        /// <summary>
        /// Gets the handle to the SkiaSharp native library.
        /// </summary>
        /// <remarks>The handle can be used to interact with the native library directly,
        /// such as for invoking unmanaged functions.</remarks>
        public static IntPtr NativeLibraryHandle =>
            nativeLibraryHandle ??= LibraryLoader.TryLoadLocalLibrary<SKCanvas>(SkiaHelper.NativeLibraryName);

        /// <summary>
        /// Converts an array of <see cref="System.Drawing.PointF"/> to <see cref="SKPoint"/>.
        /// </summary>
        /// <param name="pointsF">Source <see cref="System.Drawing.PointF"/> array (may be null).</param>
        /// <returns>Array of <see cref="SKPoint"/>. Returns an empty array when input is null or empty.</returns>
        public static SKPoint[] ToSkiaPoints(System.Drawing.PointF[] pointsF)
        {
            if (pointsF == null || pointsF.Length == 0)
                return Array.Empty<SKPoint>();

            var skPoints = new SKPoint[pointsF.Length];
            for (int i = 0; i < pointsF.Length; i++)
            {
                skPoints[i] = new SKPoint(pointsF[i].X, pointsF[i].Y);
            }

            return skPoints;
        }

        /// <summary>
        /// Converts the specified <see cref="Image"/> to a grayscale version.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> to convert to grayscale.</param>
        /// <returns>
        /// A new <see cref="Image"/> instance containing the grayscale version of the input image.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image ConvertToGrayscale(Image image)
        {
            var result = SkiaHelper.ConvertToGrayscale((SKBitmap)image);
            return (Bitmap)result;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            SKSize size,
            float scaleFactor,
            bool isTransparent = true)
        {
            var boundsInPixels = SkiaHelper.PixelFromDip(size, scaleFactor);

            SKBitmap bitmap;

            if (size == SKSize.Empty)
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
        /// Recreates a cached bitmap canvas with the specified size, scale factor,
        /// and transparency settings.
        /// </summary>
        /// <remarks>This method ensures that the cached bitmap canvas matches the specified parameters.
        /// If the existing canvas does not match, it is disposed and replaced with a new instance.
        /// The method is designed to optimize resource usage by
        /// reusing the existing canvas when possible.</remarks>
        /// <param name="cachedCanvas">A reference to the cached bitmap canvas. If the canvas
        /// is <see langword="null"/> or does not match the
        /// specified parameters, it will be replaced with a new instance.</param>
        /// <param name="size">The dimensions of the bitmap canvas,
        /// specified as a <see cref="SizeD"/> structure.</param>
        /// <param name="scaleFactor">The scaling factor to apply
        /// to the bitmap canvas, specified
        /// as a <see cref="Coord"/> value.</param>
        /// <param name="isTransparent">A value indicating whether
        /// the bitmap canvas should support transparency.
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
        public static SkiaGraphics CreateBitmapCanvas(SKBitmap bitmap, float scaleFactor)
        {
            var result = new SkiaGraphics(bitmap);
            result.OriginalScaleFactor = scaleFactor;
            result.Canvas.Scale(scaleFactor);
            return result;
        }

        /// <summary>
        /// Gets font family name for the specified <see cref="GenericFontFamily"/> value.
        /// </summary>
        /// <param name="genericFamily">Generic font family.</param>
        /// <returns></returns>
        public static string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.Default)
                return SkiaHelper.DefaultFontName;

            if (genericFamily == GenericFontFamily.Monospace)
            {
                if (!SkiaHelper.IsDefaultMonoFontNameAssigned)
                {
                    var result = FontFactory.GetSampleFixedPitchFont();
                    if (result is not null)
                    {
                        SkiaHelper.DefaultMonoFontName = result;
                        return result;
                    }
                    else
                        return SkiaHelper.DefaultFontName;
                }
                else
                {
                    return SkiaHelper.DefaultMonoFontName;
                }
            }

            var nameAndSize = FontFactory.GetSampleNameAndSize(genericFamily);

            if (!FontFamily.IsFamilyValid(nameAndSize.Name))
                return SkiaHelper.DefaultFontName;

            return nameAndSize.Name;
        }

        /// <summary>
        /// Gets <see cref="SKPath"/> from array of points and fill mode.
        /// </summary>
        /// <param name="points">The array of points.</param>
        /// <param name="fillMode">The fill mode.</param>
        /// <returns></returns>
        public static SKPath? GetPathFromPoints(ReadOnlySpan<PointD> points, FillMode fillMode = FillMode.Alternate)
        {
            if (points.Length < 3)
                return null;

            var path = new SKPath
            {
                FillType = fillMode.ToSkia(),
            };

            // Move to the first point
            path.MoveTo(points[0].X, points[0].Y);

            // Draw lines between the points
            for (int i = 1; i < points.Length; i++)
            {
                path.LineTo(points[i].X, points[i].Y);
            }

            // Ensures the polygon is closed
            path.Close();

            return path;
        }

        /// <summary>
        /// Saves the specified image to the user's Downloads folder with a unique filename.
        /// Image is saved in PNG format.
        /// </summary>
        /// <remarks>The image is saved in PNG format. A unique filename is automatically generated to
        /// avoid overwriting existing files in the Downloads folder.</remarks>
        /// <param name="image">The image to save. Must not be <see langword="null"/>.</param>
        public static void SaveImageToDownloads(SKImage image)
        {
            var path = PathUtils.GenerateUniqueFilenameInDownloads("image.png");
            SaveImageToPng(image, path);
        }

        /// <summary>
        /// Saves the specified bitmap to a PNG file at the given path.
        /// </summary>
        /// <param name="bitmap">The bitmap image to be saved. Cannot be null.</param>
        /// <param name="fileName">The path and file name where the PNG image will be saved. Must be a valid file path.</param>
        public static void SaveBitmapToPng(SKBitmap bitmap, string fileName)
        {
            using var image = SKImage.FromBitmap(bitmap);
            SaveImageToPng(image, fileName);
        }

        /// <summary>
        /// Saves an <see cref="SKImage"/> instance to a file in PNG format.
        /// </summary>
        /// <param name="image">The <see cref="SKImage"/> to save.</param>
        /// <param name="fileName">The destination file path for the PNG image.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="image"/>
        /// or <paramref name="fileName"/> is null.</exception>
        /// <exception cref="IOException">Thrown if the file cannot be written.</exception>
        public static void SaveImageToPng(SKImage image, string fileName)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(fileName);
            data.SaveTo(stream);
        }

        /// <summary>
        /// Draws a circle on the specified <see cref="SKCanvas"/> using the provided pen,
        /// brush, center point, and radius.
        /// </summary>
        /// <remarks>If both <paramref name="pen"/> and <paramref name="brush"/> are <c>null</c>, no
        /// circle will be drawn. The method uses SkiaSharp to render the circle,
        /// and the appearance of the circle
        /// depends on the styles defined by the <paramref name="pen"/>
        /// and <paramref name="brush"/>.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the circle will be drawn.
        /// Cannot be <c>null</c>.</param>
        /// <param name="pen">The <see cref="Pen"/> that defines the stroke style of the circle.
        /// Can be <c>null</c> if no stroke is
        /// desired.</param>
        /// <param name="brush">The <see cref="Brush"/> that defines the fill style of the circle.
        /// Can be <c>null</c> if no fill is desired.</param>
        /// <param name="center">The <see cref="PointD"/> representing the center of the circle.</param>
        /// <param name="radius">The radius of the circle, specified as a <see cref="Coord"/>.
        /// Must be a non-negative value.</param>
        public static void GraphicsCircle(SKCanvas canvas, Pen pen, Brush brush, PointD center, Coord radius)
        {
            var (fill, stroke) = SkiaUtils.GetFillAndStrokePaint(pen, brush);
            var radiusF = (float)radius;
            SKPoint centerF = center;
            if (fill is not null)
                canvas.DrawCircle(centerF, radiusF, fill);
            if (stroke is not null)
                canvas.DrawCircle(centerF, radiusF, stroke);
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> pair for the specified brush and pen.
        /// </summary>
        /// <param name="pen">Pen to use.</param>
        /// <param name="brush">Brush to use.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (SKPaint? Fill, SKPaint? Stroke) GetFillAndStrokePaint(Pen? pen, Brush? brush)
        {
            return (brush?.SkiaPaint, pen?.SkiaPaint);
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
        /// Draws the specified test message on the canvas at the given location,
        /// using default styling if no parameters are provided.
        /// </summary>
        /// <remarks>This method uses default font and color settings to render the text. The text is
        /// drawn with a green foreground and a yellow background.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the text will be drawn.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="text">The text to draw. If <see langword="null"/>, the default text
        /// "Hello from SkiaSharp" will be used.</param>
        /// <param name="location">The location where the text will be drawn,
        /// specified as a <see cref="PointD"/>. If <see langword="null"/>,
        /// the default location (10, 10) will be used.</param>
        /// <param name="font">The <see cref="Font"/> to use for drawing the text.
        /// Can be <c>null</c> if the default font is desired.</param>
        /// <param name="foreColor">The <see cref="Color"/> to use for the text foreground.
        /// Can be <c>null</c> if the default color is desired.</param>
        /// <param name="backColor">The <see cref="Color"/> to use for the text background.
        /// Can be <c>null</c> if the default color is desired.</param>
        public static void DrawHelloText(
            this SKCanvas canvas,
            string? text = null,
            PointD? location = null,
            Font? font = null,
            Color? foreColor = null,
            Color? backColor = null)
        {
            DrawText(
                canvas,
                text ?? "Hello from SkiaSharp",
                location ?? (10, 10),
                font ?? Font.Default,
                foreColor: foreColor ?? Color.Green,
                backColor: backColor ?? Color.Yellow);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(
            this SKCanvas canvas,
            ReadOnlySpan<char> s,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            SkiaHelper.DrawText(
                canvas,
                s,
                location,
                font,
                foreColor.AsStrokeAndFillPaint,
                backColor.IsOk ? backColor.AsFillPaint : null);
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
        /// Draws a series of connected cubic Bezier curves on the specified canvas.
        /// </summary>
        /// <remarks>This method creates a path consisting of one or more cubic Bezier curves, starting at
        /// the first point in <paramref name="points"/>  and using each subsequent
        /// group of three points to define the
        /// control points and endpoint of each curve. The resulting path is
        /// drawn on the specified <paramref
        /// name="canvas"/> using the provided <paramref name="paint"/>.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the Bezier
        /// curves will be drawn.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="paint">The <see cref="SKPaint"/> used to define the style,
        /// color, and other properties
        /// of the drawn curves. Cannot
        /// be <see langword="null"/>.</param>
        /// <param name="points">A read-only span of <see cref="PointD"/> representing
        /// the control points for the Bezier curves. The first
        /// point specifies the starting point, and every subsequent group of three points defines
        /// a cubic Bezier curve.
        /// The total number of points must be at least 4 and a multiple of 3
        /// plus 1 (e.g., 4, 7, 10, etc.).</param>
        public static void DrawBeziers(this SKCanvas canvas, SKPaint paint, ReadOnlySpan<PointD> points)
        {
            var pointsCount = points.Length;
            Graphics.DebugBezierPointsAssert(points);

            SKPath path = new();
            path.MoveTo(points[0]);

            for (int i = 1; i <= pointsCount - 3; i += 3)
            {
                path.CubicTo(points[i], points[i + 1], points[i + 2]);
            }

            canvas.DrawPath(path, paint);
        }

        /// <summary>
        /// Saves the contents of the specified <see cref="SKSurface"/> to a file in
        /// the specified image format.
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
        /// Draws a series of points on the specified canvas using the given mode, points, and paint.
        /// </summary>
        /// <remarks>This method is a wrapper for invoking the native SkiaSharp function
        /// to draw points on
        /// a canvas. Ensure that all pointers passed to this method are valid and properly
        /// initialized to avoid
        /// undefined behavior.</remarks>
        /// <param name="canvas">A pointer to the native canvas on which the points will be drawn.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="mode">The mode that determines how the points are drawn,
        /// such as individual points, lines, or polygonal paths.</param>
        /// <param name="count">A pointer to the number of points to draw.
        /// Must be a valid, non-negative integer.</param>
        /// <param name="points">A pointer to the array of points to be drawn.
        /// The array must contain at least the number of points specified
        /// by <paramref name="count"/>.</param>
        /// <param name="paint">A pointer to the paint object that defines the style
        /// and color used to draw the points. Cannot be <see langword="null"/>.</param>
        public static bool SkCanvasDrawPointsNative(
            IntPtr canvas,
            SKPointMode mode,
            IntPtr count,
            IntPtr points,
            IntPtr paint)
        {
            var symbol = CanvasDrawPointsNative.Value;
            if (symbol is null)
                return false;
            symbol(canvas, mode, count, points, paint);
            return true;
        }

        /// <summary>
        /// Checks whether the type LibraryLoader exists in the
        /// SkiaSharp namespace of the loaded SkiaSharp assembly.
        /// </summary>
        public static bool IsLibraryLoaderExists()
        {
            var skiaApiType = KnownTypes.SkiaSharpSkiaApi.Value;
            if (skiaApiType == null)
                return false;

            var assembly = skiaApiType.Assembly;

            var libraryLoaderType = assembly.GetType(
                "SkiaSharp.LibraryLoader",
                throwOnError: false,
                ignoreCase: false);
            return libraryLoaderType != null;
        }

        /// <summary>
        /// Draws a debug text label at the bottom-left corner of the specified bounds.
        /// </summary>
        /// <remarks>The text is drawn with a default font and styled with
        /// a light yellow background, a
        /// dark gray border, and black foreground text.
        /// Ensure that the <paramref name="graphics"/> object is valid and
        /// properly initialized before calling this method.</remarks>
        /// <param name="graphics">The <see cref="SKCanvas"/>
        /// on which the text will be drawn. This cannot be <see langword="null"/>.</param>
        /// <param name="text">The text to display. This cannot be <see langword="null"/>
        /// or empty.</param>
        /// <param name="bounds">The bounding rectangle that defines
        /// the area for positioning the text. The text will be drawn near the
        /// bottom-left corner of this rectangle.</param>
        public static void DrawDebugTextAtCorner(
            SKCanvas graphics,
            string text,
            Drawing.RectD bounds)
        {
            var font = UI.Control.DefaultFont.Scaled(Display.MaxScaleFactor);
            var foreColor = Drawing.Color.Black;
            var backColor = Drawing.Color.LightYellow;

            var textSize = graphics.GetTextExtent(text, font);

            var padding = 4;
            var rect = Drawing.RectD.FromLTRB(
                bounds.Left + padding,
                bounds.BottomLeft.Y - textSize.Height - padding,
                bounds.Left + textSize.Width + (2 * padding),
                bounds.BottomLeft.Y - padding);

            DrawText(
                graphics,
                text,
                rect.Location,
                font,
                foreColor,
                backColor);
        }

        /// <summary>
        /// Retrieves a delegate of the specified type that represents
        /// a symbol with the given name from the SkiaSharp native library.
        /// </summary>
        /// <remarks>This method attempts to locate the specified symbol in the native library
        /// and create a delegate of the specified type.
        /// Ensure that the native library is loaded and the handle is valid before
        /// calling this method.</remarks>
        /// <typeparam name="T">The type of the delegate to retrieve. Must be a delegate type.</typeparam>
        /// <param name="name">The name of the symbol to locate in the native library.</param>
        /// <returns>An instance of the delegate of type <typeparamref name="T"/>
        /// representing the symbol,  or <see langword="null"/>
        /// if the symbol is not found or the native library handle is not initialized.</returns>
        public static T? GetNativeLibrarySymbol<T>(string name)
            where T : Delegate
        {
            if (NativeLibraryHandle == IntPtr.Zero)
                return null;

            return LibraryLoader.TryGetSymbolDelegate<T>(NativeLibraryHandle, name);
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
    }
}
