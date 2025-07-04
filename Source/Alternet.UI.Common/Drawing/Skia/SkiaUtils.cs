﻿using System;
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
        private static FontSize defaultFontSize = 10;
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
        /// Resets the clipping rectangle of the specified <see cref="SKCanvas"/> to its default bounds.
        /// </summary>
        /// <remarks>This method sets the clipping rectangle to match the full device
        /// bounds of the canvas. It ensures that subsequent drawing operations are
        /// not restricted by a previously applied clipping region.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> instance whose clipping
        /// rectangle will be reset. Cannot be <see langword="null"/>.</param>
        public static void ResetClipRect(SKCanvas canvas)
        {
            canvas.ClipRect(
                new SKRect(0, 0, canvas.LocalClipBounds.Width, canvas.LocalClipBounds.Height));
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
    }
}
