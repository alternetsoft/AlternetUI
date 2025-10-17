using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using SkiaSharp;

namespace Alternet.Skia
{
    /// <summary>
    /// Provides utility methods and helpers for working with SkiaSharp, a 2D graphics library.
    /// This class contains methods that do not use Alternet.UI.
    /// </summary>
    /// <remarks>This class contains static methods designed to simplify common tasks when using SkiaSharp. It
    /// is intended to be a central place for reusable SkiaSharp-related functionality.</remarks>
    public static class SkiaHelper
    {
        /// <summary>
        /// Represents the name of the SkiaSharp canvas method used to draw points.
        /// </summary>
        /// <remarks>This constant is typically used as an identifier or key for referencing the
        /// "sk_canvas_draw_points" method in SkiaSharp, a 2D graphics library.</remarks>
        public const string SKCanvasDrawPointsName = "sk_canvas_draw_points";

        /// <summary>
        /// A sample string containing characters commonly used to test whether a font is monospaced.
        /// </summary>
        /// <remarks>This string includes a variety of characters with differing widths in proportional
        /// fonts, such as  narrow characters ('i', 'l') and wide characters ('m', 'W'). It is intended for use in
        /// scenarios where determining if a font renders all characters with equal width is necessary.</remarks>
        public static string SampleCharsForIsMonospace = "iIl1. ,:;`'\"!l/\\|[](){}<>=-+*mwW@#%&0123456789";

        /// <summary>
        /// Represents the name of the SkiaSharp native library.
        /// </summary>
        public static string NativeLibraryName = "libSkiaSharp";

        private static float defaultFontSize = 10;
        private static string? defaultMonoFontName;
        private static SKFont? defaultSkiaFont;
        private static SKTypeface? defaultTypeFace;
        private static SKColorFilter? grayscaleColorFilter;
        private static SKCanvas? nullCanvas;
        private static string[]? fontFamilies;

        static SkiaHelper()
        {
        }

        /// <summary>
        /// Represents a delegate for drawing points on a canvas using the specified mode,
        /// point data, and paint.
        /// </summary>
        /// <remarks>This delegate is used to invoke unmanaged code for drawing points on a canvas.
        /// The caller is
        /// responsible for ensuring that all pointers are valid and that the memory they
        /// reference remains accessible for
        /// the duration of the call.</remarks>
        /// <param name="canvas">A pointer to the canvas on which the points will be drawn.
        /// Must not be <see langword="IntPtr.Zero"/>.</param>
        /// <param name="mode">The mode that determines how the points are interpreted and drawn.
        /// See <see cref="SKPointMode"/> for available
        /// modes.</param>
        /// <param name="count">A pointer to the number of points to draw. Must not be
        /// <see langword="IntPtr.Zero"/>.</param>
        /// <param name="points">A pointer to the array of points to be drawn.
        /// The array must contain at least the number of points specified by
        /// <paramref name="count"/>. Must not be <see langword="IntPtr.Zero"/>.</param>
        /// <param name="paint">A pointer to the paint object that defines the style
        /// and color of the points. Must not be <see
        /// langword="IntPtr.Zero"/>.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SkCanvasDrawPointsDelegate(
            IntPtr canvas,
            SKPointMode mode,
            IntPtr count,
            IntPtr points,
            IntPtr paint);

        /// <summary>
        /// Gets a null SKCanvas instance that performs no drawing operations.
        /// </summary>
        /// <remarks>This property provides a placeholder <see cref="SKCanvas"/>
        /// that can be used in
        /// scenarios where a non-null canvas is required, but no actual drawing is needed.
        /// The returned canvas does not
        /// render any output and is intended for use as a no-op implementation.</remarks>
        public static SKCanvas NullCanvas => nullCanvas ??= CreateNullCanvas(1, 1);

        /// <summary>
        /// Gets or sets the paint object used to render the focus rectangle.
        /// </summary>
        public static SKPaint FocusRectPaint { get; set; } = CreateFocusRectPaint(SKColors.Black);

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
                if (grayscaleColorFilter is null)
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DefaultTypeFace.FamilyName;

            set
            {
                if (DefaultFontName == value)
                    return;
                DefaultTypeFace = SKTypeface.FromFamilyName(value) ?? SKTypeface.Default;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the default monospaced font name has been assigned.
        /// </summary>
        public static bool IsDefaultMonoFontNameAssigned
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => defaultMonoFontName is not null;
        }

        /// <summary>
        /// Gets or sets default fixed-pitch font name.
        /// </summary>
        public static string DefaultMonoFontName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => defaultMonoFontName ?? DefaultTypeFace.FamilyName;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// Creates dummy <see cref="SKSurface"/> object ignores any painting.
        /// </summary>
        /// <param name="width">Surface width.</param>
        /// <param name="height">Surface height.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKSurface CreateNullSurface(int width, int height)
        {
            return SKSurface.CreateNull(width, height);
        }

        /// <summary>
        /// Creates dummy <see cref="SKCanvas"/> object which performs not painting.
        /// </summary>
        /// <param name="width">Surface width.</param>
        /// <param name="height">Surface height.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKCanvas CreateNullCanvas(int width, int height)
        {
            var surface = CreateNullSurface(width, height);
            return surface.Canvas;
        }

        /// <summary>
        /// Gets all installed font families as enumerable.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetFontFamiliesNames()
        {
            return FontFamilies;
        }

        /// <summary>
        /// Creates default font.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKFont CreateDefaultFont()
        {
            return new SKFont(DefaultTypeFace, (float)DefaultFontSize);
        }

        /// <summary>
        /// Resets loaded font families.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetFonts()
        {
            fontFamilies = null;
        }

        /// <summary>
        /// Returns true if the <see cref="SKFont"/> appears to be monospaced.
        /// It first checks <c>Typeface.IsFixedPitch</c> if available, then measures glyph advance widths.
        /// </summary>
        /// <param name="font">The <see cref="SKFont"/> to test (must not be null).</param>
        /// <param name="tolerance">Maximum allowed difference (in device/pixel units)
        /// between widest and narrowest glyph to consider the font monospace. Default 0.5f.</param>
        public static bool IsMonospace(SKFont font, float tolerance = 0.5f)
        {
            if (font is null) throw new ArgumentNullException(nameof(font));
            if (tolerance < 0) throw new ArgumentOutOfRangeException(nameof(tolerance));

            var tf = font.Typeface;
            if (tf != null)
            {
                try
                {
                    if (tf.IsFixedPitch)
                        return true;
                    return false;
                }
                catch
                {
                }
            }

            var widths = new float[SkiaHelper.SampleCharsForIsMonospace.Length];

            for (int i = 0; i < SkiaHelper.SampleCharsForIsMonospace.Length; i++)
            {
                string s = SkiaHelper.SampleCharsForIsMonospace[i].ToString();
                widths[i] = font.MeasureText(s);
            }

            float min = widths.Min();
            float max = widths.Max();

            return (max - min) <= tolerance;
        }

        /// <summary>
        /// Gets whether or not specified font is supported in SkiaSharp.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFamilySkia(string name)
        {
            var index = Array.BinarySearch<string>(FontFamilies, name);
            return index >= 0;
        }

        /// <summary>
        /// Gets whether or not specified bitmap is ok.
        /// </summary>
        /// <param name="bitmap">Bitmap to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BitmapIsOk(SKBitmap? bitmap)
        {
            return bitmap is not null && bitmap.ReadyToDraw && bitmap.Height > 0 && bitmap.Width > 0;
        }

        /// <summary>
        /// Creates an SKPaint configured to draw a focus-like dashed rectangle (similar to WinForms DrawFocusRectangle).
        /// The returned SKPaint must be disposed by the caller.
        /// </summary>
        /// <param name="color">Primary color used for the stroke (when xor==true, result depends on blend and background).</param>
        /// <param name="strokeWidth">Stroke width in canvas units (use 1 for a typical hairline).</param>
        /// <param name="dashOn">Dash "on" length (small value produces dot-like strokes, e.g. 1).</param>
        /// <param name="gap">Dash "off" length (space between dots).</param>
        /// <param name="phase">Dash phase offset (use to create marching-ants animation).</param>
        /// <param name="useXorBlend">If true, sets paint.BlendMode = SKBlendMode.Xor to try to mimic GDI+
        /// XOR drawing (backend dependent).</param>
        /// <param name="antialiasing">Whether to enable antialiasing for the paint.</param>
        /// <param name="strokeCap">Stroke cap to use. Round produces rounded dots for short on-lengths;
        /// Square produces rectangular dashes.</param>
        /// <returns>An SKPaint instance configured for focus-rect drawing. Caller must dispose it.</returns>
        public static SKPaint CreateFocusRectPaint(
            SKColor color,
            float strokeWidth = 1f,
            float dashOn = 1f,
            float gap = 1f,
            float phase = 0f,
            bool useXorBlend = true,
            bool antialiasing = false,
            SKStrokeCap strokeCap = SKStrokeCap.Square)
        {
            if (dashOn <= 0f) dashOn = 1f;
            if (gap < 0f) gap = 0f;

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = antialiasing,
                Color = color,
                StrokeWidth = strokeWidth,
                StrokeCap = strokeCap,
                StrokeJoin = SKStrokeJoin.Miter,
            };

            paint.PathEffect = SKPathEffect.CreateDash(new[] { dashOn, gap }, phase);

            if (useXorBlend)
            {
                paint.BlendMode = SKBlendMode.Xor;
            }

            return paint;
        }

        /// <summary>
        /// Draws a focus-like dashed rectangle (similar to WinForms DrawFocusRectangle).
        /// Uses CreateFocusRectPaint internally.
        /// </summary>
        /// <param name="canvas">Target canvas (must not be null).</param>
        /// <param name="rect">Rectangle in canvas coordinates.</param>
        /// <param name="color">Primary color used for the stroke (when xor==true, result depends on blend and background).</param>
        /// <param name="strokeWidth">Stroke width in canvas units (use 1 for a typical hairline).</param>
        /// <param name="dashOn">Dash "on" length (small value produces dot-like strokes, e.g. 1).</param>
        /// <param name="gap">Dash "off" length (space between dots).</param>
        /// <param name="phase">Dash phase offset (use to create marching-ants animation).</param>
        /// <param name="useXorBlend">If true, sets paint.BlendMode = SKBlendMode.Xor
        /// to try to mimic GDI+ XOR drawing (backend dependent).</param>
        /// <param name="antialiasing">Whether to enable antialiasing for the paint.</param>
        public static void DrawFocusRect(
            SKCanvas canvas,
            SKRect rect,
            SKColor color,
            float strokeWidth = 1f,
            float dashOn = 1f,
            float gap = 1f,
            float phase = 0f,
            bool useXorBlend = true,
            bool antialiasing = false)
        {
            if (canvas is null) throw new ArgumentNullException(nameof(canvas));

            using var paint = CreateFocusRectPaint(
                color,
                strokeWidth,
                dashOn,
                gap,
                phase,
                useXorBlend,
                antialiasing,
                SKStrokeCap.Square);

            // Pixel-snapping trick for crisp 1px strokes on integer-aligned coordinates
            canvas.Save();
            canvas.Translate(0.5f, 0.5f);
            canvas.DrawRect(rect, paint);
            canvas.Restore();
        }

        /// <summary>
        /// Convenience helper that animates the dash phase to produce the "marching ants" effect.
        /// Call this from your paint loop and pass a time value (e.g. milliseconds).
        /// Uses CreateFocusRectPaint internally.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="rect">Rectangle in canvas coordinates.</param>
        /// <param name="color">Stroke color.</param>
        /// <param name="timeMs">Elapsed time in milliseconds (used to compute animated phase).</param>
        /// <param name="strokeWidth">Stroke width.</param>
        /// <param name="dashOn">Dash "on" length.</param>
        /// <param name="gap">Dash "off" length.</param>
        /// <param name="periodMs">Period in milliseconds for one
        /// full dash-cycle shift (smaller -> faster animation).</param>
        /// <param name="useXorBlend">Whether to use XOR blend mode.</param>
        /// <param name="antialiasing">Whether to enable antialiasing.</param>
        public static void DrawAnimatedFocusRect(
            SKCanvas canvas,
            SKRect rect,
            SKColor color,
            long timeMs,
            float strokeWidth = 1f,
            float dashOn = 1f,
            float gap = 1f,
            float periodMs = 250f,
            bool useXorBlend = true,
            bool antialiasing = false)
        {
            if (canvas is null) throw new ArgumentNullException(nameof(canvas));
            if (periodMs <= 0) periodMs = 250f;

            float cycle = dashOn + gap;

            // Normalize time to [0..cycle)
            float phase = (float)((timeMs % (long)periodMs) / periodMs) * cycle;

            using var paint = CreateFocusRectPaint(
                color,
                strokeWidth,
                dashOn,
                gap,
                phase,
                useXorBlend,
                antialiasing,
                SKStrokeCap.Square);

            canvas.Save();
            canvas.Translate(0.5f, 0.5f);
            canvas.DrawRect(rect, paint);
            canvas.Restore();
        }

        /// <summary>
        /// Convenience: apply the pixel-snapping translate to the canvas.
        /// It saves the canvas state and returns an IDisposable that restores when disposed:
        /// using (ApplyPixelSnapping(canvas)) { ... }  // paints inside will be offset
        /// </summary>
        /// <param name="canvas">The canvas to translate.</param>
        /// <returns>An SKAutoCanvasRestore that will restore the canvas when disposed.</returns>
        public static SKAutoCanvasRestore ApplyPixelSnapping(SKCanvas canvas)
        {
            if (canvas is null) throw new ArgumentNullException(nameof(canvas));
            var restore = new SKAutoCanvasRestore(canvas, true);
            var offset = GetPixelSnappingOffset(canvas);
            canvas.Translate(offset.X, offset.Y);
            return restore;
        }

        /// <summary>
        /// Measures the size of a single character when drawn using the specified font.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the character will be measured.</param>
        /// <param name="ch">The character to measure.</param>
        /// <param name="font">The <see cref="SKFont"/> used to measure the character.</param>
        /// <returns>A <see cref="SKSize"/> representing the width and height of the character.</returns>
        public static SKSize CharSize(
            this SKCanvas canvas,
            char ch,
            SKFont font)
        {
            Span<char> buffer = stackalloc char[1];
            buffer[0] = ch;
            return canvas.GetTextExtent(buffer, font);
        }

        /// <summary>
        /// Gets text size.
        /// </summary>
        /// <param name="canvas">Drawing context.</param>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font.</param>
        /// <returns></returns>
        public static SKSize GetTextExtent(
            this SKCanvas canvas,
            ReadOnlySpan<char> text,
            SKFont font)
        {
            var measureResult = font.MeasureText(text);

            SKSize result = new(
                measureResult,
                MathF.Abs(font.Metrics.Top) + MathF.Abs(font.Metrics.Bottom));

            return result;
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

            using SKPaint paint = new() { ColorFilter = SkiaHelper.GrayscaleColorFilter };
            canvas.DrawBitmap(bitmap, 0, 0, paint);

            var resultImage = surface.Snapshot();

            return ImageToBitmap(resultImage);
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
            SKBitmap result = new(image.Width, image.Height);
            image.ReadPixels(result.Info, result.GetPixels(), result.RowBytes, 0, 0);
            return result;
        }

        /// <summary>
        /// Draws a focus rectangle on the specified canvas using the given color.
        /// <see cref="SkiaHelper.FocusRectPaint"/> is used to draw the rectangle.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the focus rectangle will be drawn.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="rect">The <see cref="SKRect"/> defining the bounds of the focus rectangle.</param>
        /// <param name="color">The <see cref="SKColor"/> to use for the focus rectangle.</param>
        public static void DrawFocusRect(SKCanvas canvas, SKRect rect, SKColor color)
        {
            FocusRectPaint.Color = color;
            using var snapping = ApplyPixelSnapping(canvas);
            canvas.DrawRect(rect, FocusRectPaint);
        }

        /// <summary>
        /// Converts an array of <see cref="System.Drawing.Point"/> to <see cref="SKPoint"/>.
        /// </summary>
        /// <param name="points">Source points (may be null).</param>
        /// <returns>Array of <see cref="SKPoint"/>. Returns an empty array when input is null or empty.</returns>
        public static SKPoint[] ToSkiaPoints(System.Drawing.Point[] points)
        {
            if (points == null || points.Length == 0)
                return Array.Empty<SKPoint>();

            var skPoints = new SKPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                skPoints[i] = new SKPoint(points[i].X, points[i].Y);
            }

            return skPoints;
        }

        /// <summary>
        /// Returns an approximated average scale factor of the canvas transform (works with rotation/skew).
        /// Use this to convert "device-pixel" lengths into canvas units: canvasUnits = devicePixels / scale.
        /// </summary>
        public static float GetAverageCanvasScale(SKCanvas canvas)
        {
            SKMatrix m = canvas.TotalMatrix;

            // effective scale in X = sqrt(scaleX^2 + skewX^2)
            // effective scale in Y = sqrt(skewY^2 + scaleY^2)
            float scaleX = MathF.Sqrt((m.ScaleX * m.ScaleX) + (m.SkewX * m.SkewX));
            float scaleY = MathF.Sqrt((m.SkewY * m.SkewY) + (m.ScaleY * m.ScaleY));

            // average both axes — acceptable for line dash patterns
            float avg = (scaleX + scaleY) * 0.5f;
            if (avg <= 0f) avg = 1f;
            return avg;
        }

        /// <summary>
        /// Computes the pixel-snapping offset (in canvas/logical coordinates) that aligns
        /// 1px strokes to device pixel centers, taking the canvas TotalMatrix into account.
        /// Returns an SKPoint { X = offsetX, Y = offsetY } where:
        ///   offsetX = 0.5f / effectiveScaleX
        ///   offsetY = 0.5f / effectiveScaleY
        /// Use: canvas.Save(); canvas.Translate(offset.X, offset.Y); ... canvas.Restore();
        /// </summary>
        /// <param name="canvas">The SKCanvas to inspect (must not be null).</param>
        /// <returns>Pixel-snapping offset in canvas coordinates.</returns>
        public static SKPoint GetPixelSnappingOffset(SKCanvas canvas)
        {
            if (canvas is null) throw new ArgumentNullException(nameof(canvas));

            // compute effective scale from the current transform (works with rotation/skew)
            SKMatrix m = canvas.TotalMatrix;
            float scaleX = MathF.Sqrt((m.ScaleX * m.ScaleX) + (m.SkewX * m.SkewX));
            float scaleY = MathF.Sqrt((m.SkewY * m.SkewY) + (m.ScaleY * m.ScaleY));

            // avoid divide-by-zero
            if (scaleX <= 0f) scaleX = 1f;
            if (scaleY <= 0f) scaleY = 1f;

            float offsetX = 0.5f / scaleX;
            float offsetY = 0.5f / scaleY;

            return new SKPoint(offsetX, offsetY);
        }
    }
}
