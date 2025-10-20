using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using SkiaSharp;

#pragma warning disable
#if ALTERNETUI
namespace Alternet.Skia
#else
namespace Alternet.Common.Skia
#endif
#pragma warning restore
{
    /// <summary>
    /// Provides utility methods and helpers for working with SkiaSharp, a 2D graphics library.
    /// This class contains methods that do not use Alternet.UI.
    /// </summary>
    /// <remarks>This class contains static methods designed to simplify common tasks
    /// when using SkiaSharp. It
    /// is intended to be a central place for reusable SkiaSharp-related functionality.</remarks>
    public static class SkiaHelper
    {
        /// <summary>
        /// Gets <see cref="MathF.PI"/> divided by 180f.
        /// </summary>
        public const float DegToRadF = MathF.PI / 180f;

        /// <summary>
        /// Gets 180f divided by <see cref="MathF.PI"/>.
        /// </summary>
        public const float RadToDegF = 180f / MathF.PI;

        /// <summary>
        /// Represents the name of the SkiaSharp canvas method used to draw points.
        /// </summary>
        /// <remarks>This constant is typically used as an identifier or key for referencing the
        /// "sk_canvas_draw_points" method in SkiaSharp, a 2D graphics library.</remarks>
        public const string SKCanvasDrawPointsName = "sk_canvas_draw_points";

        /// <summary>
        /// Gets or sets default value for <see cref="SKPaint.IsAntialias"/> property
        /// when <see cref="SKPaint"/> is created by the conversion methods.
        /// </summary>
        public static bool DefaultAntialiasing = true;

        /// <summary>
        /// Represents the maximum number of spans that can be stored in a stack.
        /// </summary>
        /// <remarks>This constant defines the upper limit for the size of a span stack. It is used to
        /// prevent excessive memory usage or stack overflow scenarios when working with spans.</remarks>
        public static int SpanStackLimit = 256;

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
        /// Represents a method that performs an action on a read-only span
        /// of elements of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>This delegate is commonly used to define operations that work on spans of data
        /// without modifying the underlying memory.</remarks>
        /// <typeparam name="T">The type of the elements in the read-only span.</typeparam>
        /// <param name="span">The read-only span of elements to process.</param>
        public delegate void ReadOnlySpanAction<T>(ReadOnlySpan<T> span);

        /// <summary>
        /// Occurs when <see cref="SKPaint"/> instance is created by one of the create methods.
        /// </summary>
        public static event EventHandler? PaintCreated;

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
        public static float DefaultFontSize
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
        /// Returns the size of a sequence of identical characters when drawn with the specified font.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the character will be measured.</param>
        /// <param name="ch">The character to measure.</param>
        /// <param name="count">The number of times the character is repeated.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SKSize"/> structure representing the width and height of the repeated characters.
        /// </returns>
        public static SKSize CharSize(
            this SKCanvas canvas,
            char ch,
            int count,
            SKFont font)
        {
            if (count == 1)
                return canvas.CharSize(ch, font);
            if (count <= 0)
                return SKSize.Empty;

            SKSize result = SKSize.Empty;

            SkiaHelper.InvokeWithFilledSpan(
                count,
                ch,
                span =>
                {
                    result = canvas.GetTextExtent(span, font);
                });

            return result;
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

#if !NET5_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Gets text size.
        /// </summary>
        /// <param name="canvas">Drawing context.</param>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font.</param>
        /// <returns></returns>
        public static SKSize GetTextExtent(
            this SKCanvas canvas,
            string text,
            SKFont font)
        {
            var measureResult = font.MeasureText(text);

            SKSize result = new(
                measureResult,
                MathF.Abs(font.Metrics.Top) + MathF.Abs(font.Metrics.Bottom));

            return result;
        }
#endif

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
        /// Draws a dotted line on the specified graphics context, alternating between two colors.
        /// </summary>
        /// <remarks>This method supports drawing horizontal or vertical dotted lines only.
        /// The line alternates between <paramref name="color1"/> and
        /// <paramref name="color2"/> (or transparency if <paramref name="color2"/> is
        /// null). The size of the dots is determined by the
        /// <paramref name="size"/> parameter.</remarks>
        /// <param name="dc">The <see cref="SKCanvas"/> object used to draw the line.</param>
        /// <param name="x1">The starting x-coordinate of the line.</param>
        /// <param name="y1">The starting y-coordinate of the line.</param>
        /// <param name="x2">The ending x-coordinate of the line.</param>
        /// <param name="y2">The ending y-coordinate of the line.</param>
        /// <param name="color1">The primary color used for the dots in the line.</param>
        /// <param name="color2">The secondary color used for the dots in the line.
        /// If set to null, the line will
        /// alternate between the primary color and transparency.</param>
        /// <param name="size">The size of each dot in pixels. Defaults to 1.
        /// Larger values result in thicker dots.</param>
        public static void DrawDotLine(
            SKCanvas dc,
            float x1,
            float y1,
            float x2,
            float y2,
            SKPaint color1,
            SKPaint? color2,
            int size = 1)
        {
            var pxSize = size;

            bool isTransparent = color2 is null;
            if (x1 == x2)
            {
                if (pxSize > 1)
                {
                    int oldStart = 0;
                    SKPaint? oldColor = null;
                    SKPaint? color;

                    for (int i = 0; i < y2 - y1; i++)
                    {
                        if ((i + y1) % (size * 2) < size)
                            color = color1;
                        else
                            color = color2;

                        if (i != 0 && color != oldColor)
                        {
                            if (oldColor != null)
                            {
                                DrawVertLine(
                                    dc,
                                    oldColor,
                                    new SKPoint(x1, oldStart + y1),
                                    i - oldStart,
                                    1);
                            }

                            oldStart = i;
                            oldColor = color;
                        }
                    }

                    if (oldColor != null && oldStart + y1 < y2)
                    {
                        DrawVertLine(
                            dc,
                            oldColor,
                            new SKPoint(x1, oldStart + y1),
                            y2 - oldStart - y1,
                            1);
                    }
                }
                else
                {
                    for (int i = 0; i < y2 - y1; i++)
                    {
                        if ((i + y1) % (size * 2) < size)
                            DrawHorzLine(dc, color1, new SKPoint(x1, i + y1), 1, 1);
                        else
                            if (color2 is not null)
                            DrawHorzLine(dc, color2, new SKPoint(x1, i + y1), 1, 1);
                    }
                }
            }
            else
            if (y1 == y2)
            {
                if (pxSize > 1)
                {
                    int oldStart = 0;
                    SKPaint? oldColor = null;
                    SKPaint? color;

                    for (int i = 0; i < x2 - x1; i++)
                    {
                        if ((i + y1) % (size * 2) < size)
                            color = color1;
                        else
                            color = color2;

                        if (i != 0 && color != oldColor)
                        {
                            if (oldColor != null)
                            {
                                DrawHorzLine(
                                    dc,
                                    oldColor,
                                    new SKPoint(oldStart + x1, y1),
                                    i - oldStart,
                                    1);
                            }

                            oldStart = i;
                            oldColor = color;
                        }
                    }

                    if (oldColor != null && oldStart + y1 < y2)
                    {
                        DrawHorzLine(
                            dc,
                            oldColor,
                            new SKPoint(oldStart + x1, y1),
                            x2 - x1 - oldStart,
                            1);
                    }
                }
                else
                {
                    for (int i = 0; i < x2 - x1; i++)
                    {
                        if ((i + y1) % (size * 2) < size)
                            DrawHorzLine(dc, color1, new SKPoint(i + x1, y1), 1, 1);
                        else
                            if (color2 is not null)
                            DrawHorzLine(dc, color2, new SKPoint(i + x1, y1), 1, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a horizontal line on the specified canvas.
        /// </summary>
        /// <param name="dc">The <see cref="SKCanvas"/> on which the line will be drawn.</param>
        /// <param name="brush">The <see cref="SKPaint"/> used to define the color, style,
        /// and other properties of the line.</param>
        /// <param name="point">The starting point of the line, represented as a <see cref="SKPoint"/>.</param>
        /// <param name="length">The length of the horizontal line.</param>
        /// <param name="width">The width of the horizontal line.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawHorzLine(
            SKCanvas dc,
            SKPaint brush,
            SKPoint point,
            float length,
            float width)
        {
            var rect = SKRect.Create(point, new SKSize(length, width));
            dc.DrawRect(rect, brush);
        }

        /// <summary>
        /// Draws a border around the specified rectangle using the provided paint and border width.
        /// </summary>
        /// <param name="dc">The <see cref="SKCanvas"/> on which the border will be drawn.</param>
        /// <param name="paint">The <see cref="SKPaint"/> used to define the appearance of the border.</param>
        /// <param name="rect">The <see cref="SKRect"/> representing the rectangle
        /// around which the border will be drawn.</param>
        /// <param name="borderWidth">The width of the border. The default value is 1.</param>
        public static void DrawBorderWithPaint(
            SKCanvas dc,
            SKPaint paint,
            SKRect rect,
            float borderWidth = 1)
        {
            dc.DrawRect(GetTopLineRect(rect, borderWidth), paint);
            dc.DrawRect(GetBottomLineRect(rect, borderWidth), paint);
            dc.DrawRect(GetLeftLineRect(rect, borderWidth), paint);
            dc.DrawRect(GetRightLineRect(rect, borderWidth), paint);
        }

        /// <summary>
        /// Gets rectangle of the left border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static SKRect GetLeftLineRect(this SKRect rect, float width)
        {
            var point = new SKPoint(rect.Left, rect.Top);
            var size = new SKSize(width, rect.Height);
            return SKRect.Create(point, size);
        }

        /// <summary>
        /// Creates a new <see cref="SKPath"/> from a collection of drawing points,
        /// optionally specifying the fill mode.
        /// </summary>
        /// <remarks>The method creates a closed path by connecting the points in the order they appear in
        /// the span,  and then closing the path to form a polygon. The <paramref name="fillMode"/>
        /// determines how the
        /// interior  of the polygon is filled.</remarks>
        /// <param name="points">A read-only span of <see cref="System.Drawing.Point"/> representing
        /// the vertices of the path.  Must contain
        /// at least three points to create a valid path.</param>
        /// <param name="fillMode">The fill mode to use for the path. Defaults to <see cref="SkiaFillMode.Alternate"/>.</param>
        /// <returns>An <see cref="SKPath"/> representing the closed polygon defined by the provided points,  or <see
        /// langword="null"/> if the <paramref name="points"/> span is empty
        /// or contains fewer than three points.</returns>
        public static SKPath? GetPathFromSystemPoints(
            ReadOnlySpan<System.Drawing.Point> points,
            SkiaFillMode fillMode = SkiaFillMode.Alternate)
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
        /// Converts <see cref="SkiaFillMode"/> to <see cref="SKPathFillType"/>.
        /// </summary>
        /// <param name="fillMode">Value to convert.</param>
        /// <returns></returns>
        public static SKPathFillType ToSkia(this SkiaFillMode fillMode)
        {
            switch (fillMode)
            {
                case SkiaFillMode.Alternate:
                    return SKPathFillType.EvenOdd;
                case SkiaFillMode.Winding:
                    return SKPathFillType.Winding;
                default:
                    return SKPathFillType.EvenOdd;
            }
        }

        /// <summary>
        /// Gets rectangle of the right border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static SKRect GetRightLineRect(this SKRect rect, float width)
        {
            var point = new SKPoint(rect.Right - width, rect.Top);
            var size = new SKSize(width, rect.Height);
            return SKRect.Create(point, size);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static SKRect GetBottomLineRect(this SKRect rect, float width)
        {
            var point = new SKPoint(rect.Left, rect.Bottom - width);
            var size = new SKSize(rect.Width, width);
            return SKRect.Create(point, size);
        }

        /// <summary>
        /// Gets rectangle of the top border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static SKRect GetTopLineRect(this SKRect rect, float width)
        {
            var point = new SKPoint(rect.Left, rect.Top);
            var size = new SKSize(rect.Width, width);
            return SKRect.Create(point, size);
        }

        /// <summary>
        /// Draws a vertical line on the specified canvas.
        /// </summary>
        /// <param name="dc">The <see cref="SKCanvas"/> on which the vertical line will be drawn.</param>
        /// <param name="brush">The <see cref="SKPaint"/> used to define the color, style,
        /// and other properties of the line.</param>
        /// <param name="point">The starting point of the vertical line, representing the
        /// top-left corner of the line's bounding rectangle.</param>
        /// <param name="length">The length of the vertical line, measured in the coordinate
        /// space of the canvas.</param>
        /// <param name="width">The width of the vertical line, measured in the coordinate
        /// space of the canvas.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawVertLine(
            SKCanvas dc,
            SKPaint brush,
            SKPoint point,
            float length,
            float width)
        {
            var rect = SKRect.Create(point, new SKSize(width, length));
            dc.DrawRect(rect, brush);
        }

        /// <summary>
        /// Checks whether array of <see cref="SKPoint"/> parameter is ok.
        /// </summary>
        /// <param name="points">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugBezierPointsAssert(ReadOnlySpan<SKPoint> points)
        {
            var length = points.Length;

            if (length == 0)
                return;

            if ((length - 1) % 3 != 0)
            {
                throw new ArgumentException(
                    "The number of points should be a multiple of 3 plus 1, such as 4, 7, or 10.",
                    nameof(points));
            }
        }

        /// <summary>
        /// Draws a series of Bezier splines from an array of <see cref="SKPoint"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="SKPaint"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="points">
        /// Array of <see cref="SKPoint"/> structures that represent the points that
        /// determine the curve.
        /// The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.
        /// </param>
        /// <param name="canvas">Drawing context.</param>
        public static void DrawBeziers(this SKCanvas canvas, SKPaint pen, ReadOnlySpan<SKPoint> points)
        {
            var pointsCount = points.Length;
            DebugBezierPointsAssert(points);

            SKPath path = new();
            path.MoveTo(points[0]);

            for (int i = 1; i <= pointsCount - 3; i += 3)
            {
                path.CubicTo(points[i], points[i + 1], points[i + 2]);
            }

            canvas.DrawPath(path, pen);
        }

        /// <summary>
        /// Draws the specified text on the canvas at the given location using the
        /// specified font and paint settings.
        /// </summary>
        /// <remarks>The method measures the text using the specified font and adjusts the drawing
        /// position to account for font metrics. If a background color is provided,
        /// a rectangle is drawn behind the
        /// text to match its dimensions.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the text will be drawn.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="s">The text to draw, represented as a <see cref="ReadOnlySpan{Char}"/>.
        /// If the span is empty, no text will be
        /// drawn.</param>
        /// <param name="location">The <see cref="SKPoint"/> specifying the top-left corner
        /// where the text will be drawn.</param>
        /// <param name="font">The <see cref="SKFont"/> to use for rendering the text. Cannot
        /// be <see langword="null"/>.</param>
        /// <param name="foreColor">The <see cref="SKPaint"/> used to define the foreground
        /// color and style of the text. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="backColor">An optional <see cref="SKPaint"/> used to define
        /// the background color and style. If provided, a rectangle
        /// will be drawn behind the text. If <see langword="null"/>,
        /// no background will be drawn.</param>
        public static void DrawText(
            SKCanvas canvas,
            ReadOnlySpan<char> s,
            SKPoint location,
            SKFont font,
            SKPaint foreColor,
            SKPaint? backColor = null)
        {
            if (s.Length == 0)
                return;

            float x = location.X;
            float y = location.Y;

            var offsetX = 0;
            var offsetY = MathF.Abs(font.Metrics.Top);

            if (backColor is not null)
            {
                var measureResult = font.MeasureText(s);

                var rect = SKRect.Create(
                    measureResult,
                    MathF.Abs(font.Metrics.Top) + MathF.Abs(font.Metrics.Bottom));
                rect.Offset(x, y);

                canvas.DrawRect(rect, backColor);
            }

            using var blob = SKTextBlob.Create(s, font);
            canvas.DrawText(blob, x + offsetX, y + offsetY, foreColor);
        }

        /// <summary>
        /// Fills the specified rectangular area on the canvas with a linear gradient.
        /// </summary>
        /// <remarks>This method creates a linear gradient between <paramref name="point1"/> and
        /// <paramref name="point2"/> and fills the specified rectangle with it.
        /// If the gradient points are identical, the
        /// rectangle is filled with a solid color instead.</remarks>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the gradient will be drawn.
        /// Cannot be <c>null</c>.</param>
        /// <param name="rect">The rectangular area to fill, specified as a <see cref="SKRect"/>.
        /// If the width or height is less than or
        /// equal to zero, the method does nothing.</param>
        /// <param name="beginColor">The starting color of the gradient.</param>
        /// <param name="endColor">The ending color of the gradient.</param>
        /// <param name="point1">The starting point of the gradient, specified as a <see cref="SKPoint"/>.</param>
        /// <param name="point2">The ending point of the gradient, specified as a <see cref="SKPoint"/>.
        /// If this point coincides with
        /// <paramref name="point1"/>, the method falls back to a solid fill
        /// using <paramref name="beginColor"/>.</param>
        public static void FillGradient(
            SKCanvas canvas,
            SKRect rect,
            SKColor beginColor,
            SKColor endColor,
            SKPoint point1,
            SKPoint point2)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                return;

            // If the two gradient points coincide, fall back to a solid fill.
            if (MathF.Abs(point1.X - point2.X) < float.Epsilon && MathF.Abs(point1.Y - point2.Y) < float.Epsilon)
            {
                using var solid = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true,
                    Color = beginColor,
                };
                canvas.DrawRect(rect, solid);
                return;
            }

            // Create shader and paint, ensure proper disposal.
            using var shader = SKShader.CreateLinearGradient(
                point1,
                point2,
                new[] { beginColor, endColor },
                new float[] { 0f, 1f },
                SKShaderTileMode.Clamp);

            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Shader = shader,
            };

            canvas.DrawRect(rect, paint);
        }

        /// <summary>
        /// Generates an array of points representing a wave pattern within the specified rectangular area.
        /// </summary>
        /// <remarks>The method ensures that the wave pattern fits within the bounds of the specified
        /// rectangle. The horizontal spacing and vertical offsets of the wave are determined by internal scaling
        /// factors.</remarks>
        /// <param name="rect">The rectangular area within which the wave pattern is generated.</param>
        /// <returns>An array of <see cref="SKPoint"/> objects representing the points of the wave pattern.
        /// The wave spans the
        /// horizontal range of the rectangle and alternates vertically to create the pattern.</returns>
        public static SKPoint[] GetSKPointsForDrawWave(SKRectI rect)
        {
            int minSize = 4;
            int offset = 6;

            int left = rect.Left - (rect.Left % offset);
            int i = rect.Right % offset;
            int right = (i != 0) ? rect.Right + (offset - i) : rect.Right;

            int scale = 2;
            int size = (right - left) / scale;

            offset = 3;

            if (size < minSize)
                size = minSize;
            else
            {
                i = (int)((size - minSize) / offset);
                if ((size - minSize) % offset != 0)
                    i++;
                size = minSize + (i * offset);
            }

            SKPoint[] pts = new SKPoint[size];
            for (int index = 0; index < size; index++)
            {
                pts[index].X = left + (index * scale);
                pts[index].Y = rect.Bottom - 1;
                switch (index % 3)
                {
                    case 0:
                        {
                            pts[index].Y -= scale;
                            break;
                        }

                    case 2:
                        {
                            pts[index].Y += scale;
                            break;
                        }
                }
            }

            return pts;
        }

        /// <summary>
        /// Recreates a bitmap-backed <see cref="SKCanvas"/> with the specified scale factor and size.
        /// </summary>
        /// <remarks>This method ensures that the <paramref name="measureCanvas"/> is properly recreated
        /// when necessary, such as when the canvas is null or the scale factor has changed.
        /// If the canvas does not need to be recreated, the method leaves the existing canvas unchanged.</remarks>
        /// <param name="measureCanvas">A reference to the <see cref="SKCanvas"/> to be recreated.
        /// If the canvas is null or the scale factor has
        /// changed, it will be replaced with a new instance.</param>
        /// <param name="measureCanvasScale">A reference to the scale factor associated
        /// with the <paramref name="measureCanvas"/>. This value will be
        /// updated to match the new scale factor if the canvas is recreated.</param>
        /// <param name="scaleFactor">The scale factor to apply to the new canvas.
        /// This determines the resolution of the bitmap backing the
        /// canvas.</param>
        /// <param name="size">The dimensions of the canvas, specified as an <see cref="SKSize"/>.</param>
        /// <param name="isTransparent">A boolean value indicating whether
        /// the canvas should have a transparent background. The default value is
        /// <see langword="true"/>.</param>
        public static void RecreateBitmapCanvas(
            ref SKCanvas? measureCanvas,
            ref float? measureCanvasScale,
            float scaleFactor,
            SKSize size,
            bool isTransparent = true)
        {
            if (measureCanvas is null)
            {
                measureCanvas = SkiaHelper.CreateBitmapCanvas(size, scaleFactor, isTransparent);
                measureCanvasScale = scaleFactor;
            }
            else
            {
                if (measureCanvasScale != scaleFactor)
                {
                    measureCanvas.Dispose();
                    measureCanvas = SkiaHelper.CreateBitmapCanvas(size, scaleFactor, isTransparent);
                    measureCanvasScale = scaleFactor;
                }
            }
        }

        /// <summary>
        /// Creates canvas on the bitmap with the specified size and scaling factor.
        /// </summary>
        /// <param name="scaleFactor">Scaling factor.</param>
        /// <param name="size">Size of the bitmap.</param>
        /// <param name="isTransparent">Whether canvas is transparent.</param>
        /// <returns></returns>
        public static SKCanvas CreateBitmapCanvas(
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
        /// Creates canvas on the specified bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap to create canvas on.</param>
        /// <param name="scaleFactor">Initial scale factor for the canvas.</param>
        /// <returns></returns>
        public static SKCanvas CreateBitmapCanvas(SKBitmap bitmap, float scaleFactor)
        {
            var result = new SKCanvas(bitmap);
            result.Scale(scaleFactor);
            return result;
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

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PixelFromDip(float value, float scaleFactor)
        {
            if (scaleFactor == 1)
                return (int)value;
            return (int)MathF.Round(value * scaleFactor);
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKSizeI PixelFromDip(SKSize value, float scaleFactor)
        {
            return new(PixelFromDip(value.Width, scaleFactor), PixelFromDip(value.Height, scaleFactor));
        }

        /// <summary>
        /// This method is called by all <see cref="SKPaint"/> create methods.
        /// It raises <see cref="PaintCreated"/> event and initializes <see cref="SKPaint"/>
        /// instance properties with the default values.
        /// </summary>
        /// <param name="paint"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPaintDefaults(SKPaint paint)
        {
            paint.IsAntialias = SkiaHelper.DefaultAntialiasing;
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
        /// Draws a line on the canvas using a dash style similar to System.Drawing.Pen.DashStyle.
        /// This helper creates and configures an SKPaint internally (caller does not own/dispose it).
        /// </summary>
        /// <param name="canvas">Canvas to draw on (not null).</param>
        /// <param name="p1">Start point in canvas coordinates.</param>
        /// <param name="p2">End point in canvas coordinates.</param>
        /// <param name="color">Stroke color.</param>
        /// <param name="strokeWidth">Stroke width in canvas units (use 0 for hairline).</param>
        /// <param name="dashStyle">Dash style from System.Drawing.Drawing2D.DashStyle.</param>
        /// <param name="customPattern">
        /// If dashStyle == DashStyle.Custom, this supplies the pattern as
        /// device pixels: {on, off, on, off, ...}.
        /// If null when Custom is requested, falls back to a dotted pattern.
        /// </param>
        /// <param name="dashOffset">
        /// Phase/offset in device pixels. This is converted to canvas units
        /// using the canvas transform so animation/phase
        /// matches device-space expectations.
        /// </param>
        /// <param name="antialiasing">Whether to enable antialiasing on the paint.</param>
        public static void DrawDashedLine(
            SKCanvas canvas,
            SKPoint p1,
            SKPoint p2,
            SKColor color,
            float strokeWidth = 1f,
            SkiaDashStyle dashStyle = SkiaDashStyle.Solid,
            float[]? customPattern = null,
            float dashOffset = 0f,
            bool antialiasing = true)
        {
            if (canvas is null) throw new ArgumentNullException(nameof(canvas));

            // Create paint
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = strokeWidth,
                IsAntialias = antialiasing,
                StrokeJoin = SKStrokeJoin.Miter,
            };

            // Compute effective canvas scale so dash lengths/offsets specified in "device pixels"
            // are converted to canvas units. This keeps dash pattern visually consistent across DPI/scale.
            float scale = SkiaHelper.GetAverageCanvasScale(canvas);
            if (scale <= 0f) scale = 1f; // safety

            // Common base patterns (device-pixel units). These approximate GDI+ defaults.
            // We will scale them by strokeWidth (so wider pens yield proportionally longer dashes).
            // You can tweak these constants to better match your exact target renderer.
            const float baseDash = 4f;
            const float baseGap = 2f;
            const float baseDot = 1f;

            // pattern described in device pixels (on, off, on, off...)
            float[]? patternDevice;

            switch (dashStyle)
            {
                case SkiaDashStyle.Solid:
                case SkiaDashStyle.Custom when customPattern == null || customPattern.Length == 0:
                    patternDevice = null;
                    break;

                case SkiaDashStyle.Dash:
                    patternDevice = new[]
                    {
                        baseDash * strokeWidth,
                        baseGap * strokeWidth,
                    };
                    break;

                case SkiaDashStyle.Dot:
                    // short "on" + gap, with round caps produces dots
                    patternDevice = new[]
                    {
                        baseDot * strokeWidth,
                        baseGap * strokeWidth,
                    };
                    paint.StrokeCap = SKStrokeCap.Round;
                    break;

                case SkiaDashStyle.DashDot:
                    patternDevice = new[]
                    {
                        baseDash * strokeWidth,
                        baseGap * strokeWidth,
                        baseDot * strokeWidth,
                        baseGap * strokeWidth,
                    };
                    paint.StrokeCap = SKStrokeCap.Round;
                    break;
                case SkiaDashStyle.DashDotDot:
                    patternDevice = new[]
                    {
                        baseDash * strokeWidth,
                        baseGap * strokeWidth,
                        baseDot * strokeWidth,
                        baseGap * strokeWidth,
                        baseDot * strokeWidth,
                        baseGap * strokeWidth,
                    };
                    paint.StrokeCap = SKStrokeCap.Round;
                    break;
                case SkiaDashStyle.Custom:
                    // customPattern is expected in device pixels; clone defensively
                    if (customPattern != null && customPattern.Length > 0)
                    {
                        patternDevice = new float[customPattern.Length];
                        Array.Copy(customPattern, patternDevice, customPattern.Length);
                    }
                    else
                    {
                        // fallback to dot if nothing provided
                        patternDevice = new[] { baseDot * strokeWidth, baseGap * strokeWidth };
                        paint.StrokeCap = SKStrokeCap.Round;
                    }

                    break;

                default:
                    patternDevice = null;
                    break;
            }

            if (patternDevice != null)
            {
                // convert pattern from device pixels into canvas units by dividing by scale
                var patternCanvas = new float[patternDevice.Length];
                for (int i = 0; i < patternDevice.Length; i++)
                    patternCanvas[i] = patternDevice[i] / scale;

                // convert dashOffset (device pixels) to canvas units as well
                float phaseCanvas = dashOffset / scale;

                paint.PathEffect = SKPathEffect.CreateDash(patternCanvas, phaseCanvas);
            }

            // Draw the line
            canvas.DrawLine(p1, p2, paint);
        }

        /// <summary>
        /// Allocates a <see cref="Span{Char}"/> of the specified length,
        /// fills it with the given character,
        /// and invokes the provided action with the resulting <see cref="ReadOnlySpan{Char}"/>.
        /// Uses stack allocation if the span size is below the threshold; otherwise falls back to heap.
        /// </summary>
        /// <param name="count">The number of characters to allocate and fill.</param>
        /// <param name="ch">The character to fill the span with.</param>
        /// <param name="action">The callback to invoke with the filled span.</param>
        /// <param name="invokeOnEmpty">
        /// If <c>true</c>, the action will be invoked with an empty span
        /// when <paramref name="count"/> is zero or negative.
        /// If <c>false</c>, the action will be skipped in that case.
        /// </param>
        public static void InvokeWithFilledSpan(
            int count,
            char ch,
            ReadOnlySpanAction<char> action,
            bool invokeOnEmpty = false)
        {
            if (count <= 0)
            {
                if (invokeOnEmpty)
                    action(ReadOnlySpan<char>.Empty);
                return;
            }

            if (count <= SpanStackLimit)
            {
                Span<char> buffer = stackalloc char[count];
                buffer.Fill(ch);
                action(buffer);
            }
            else
            {
                char[] buffer = new char[count];
                buffer.Fill(ch);
                action(buffer);
            }
        }

        /// <summary>
        /// Sets all elements in the specified array to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="array">The array whose elements will be set. Cannot be <see langword="null"/>.</param>
        /// <param name="value">The value to assign to each element of the array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(this T[] array, T value)
        {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            Array.Fill(array, value);
#else
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
#endif
        }

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToDegrees(float radians)
        {
            var angleDegrees = radians * RadToDegF;

            // Ensure angle is in [0, 360)
            if (angleDegrees < 0f)
                angleDegrees += 360f;

            return angleDegrees;
        }

        /// <summary>
        /// Rounds the specified floating-point value up to the nearest integer and returns
        /// the result as an <see cref="int"/>.
        /// </summary>
        /// <remarks>This method performs a fast ceiling operation by adding a small offset to the input
        /// value before truncating it to an integer. It is optimized for performance
        /// and may not handle edge cases such
        /// as very large or special floating-point values (e.g., NaN, infinity).</remarks>
        /// <param name="value">The single-precision floating-point value to round up.</param>
        /// <returns>The smallest integer greater than or equal to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FastCeilToInt(float value)
        {
            return (int)(value + 0.9999999f);
        }

        /// <summary>
        /// Converts the specified UTF-16 character span to UTF-8 and invokes
        /// the provided action with a pointer to the
        /// UTF-8 data and its length.
        /// </summary>
        /// <remarks>This method uses stack allocation for small UTF-8 buffers
        /// and heap allocation for
        /// larger ones. Callers should ensure that the action does not store
        /// or use the pointer outside the scope of
        /// the action.</remarks>
        /// <param name="input">The UTF-16 character span to be converted to UTF-8.
        /// If the span is empty, the action is invoked with a null
        /// pointer and a length of 0.</param>
        /// <param name="action">The action to invoke with the UTF-8 data.
        /// The action receives a pointer to the UTF-8-encoded bytes and the
        /// number of bytes in the UTF-8 data. The pointer is valid only
        /// for the duration of the action's execution.</param>
        public static unsafe void InvokeWithUTF8Span(ReadOnlySpan<char> input, Action<IntPtr, int> action)
        {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            if (input.IsEmpty)
            {
                action(IntPtr.Zero, 0);
                return;
            }

            var utf8 = Encoding.UTF8;
            int byteCount = utf8.GetByteCount(input);

            if (byteCount <= SpanStackLimit)
            {
                Span<byte> buffer = stackalloc byte[byteCount];
                utf8.GetBytes(input, buffer);
                fixed (byte* ptr = buffer)
                {
                    action((IntPtr)ptr, byteCount);
                }
            }
            else
            {
                byte[] buffer = new byte[byteCount];
                utf8.GetBytes(input, buffer);
                fixed (byte* ptr = buffer)
                {
                    action((IntPtr)ptr, byteCount);
                }
            }
#else
            var s = input.ToString();

            if (string.IsNullOrEmpty(s))
            {
                action(IntPtr.Zero, 0);
                return;
            }

            var utf8 = System.Text.Encoding.UTF8;
            byte[] buffer = utf8.GetBytes(s);
            fixed (byte* ptr = buffer)
            {
                action((IntPtr)ptr, buffer.Length);
            }
#endif
        }
    }
}
