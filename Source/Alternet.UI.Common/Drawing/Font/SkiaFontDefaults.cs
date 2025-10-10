using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides default values for SkiaSharp font properties used when
    /// converting a native font to an <see cref="SKFont"/>.
    /// </summary>
    /// <remarks>This class defines static fields that represent default settings for various
    /// <see cref="SKFont"/> properties, such as scaling, subpixel precision, hinting, and edging. These defaults are
    /// applied during the conversion process from a native font to an <see cref="SKFont"/> to ensure consistent
    /// rendering behavior.</remarks>
    public static class SkiaFontDefaults
    {
        /// <summary>
        /// Gets or sets default value for <see cref="SKFont.ScaleX"/> which is used
        /// when <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        public static float TextScaleX = 1.0f;

        /// <summary>
        /// Gets or sets default value for <see cref="SKFont.Subpixel"/> which is used
        /// when <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        /// <remarks>
        /// It allows to control whether glyph positioning uses subpixel precision — meaning
        /// characters can be placed at fractional pixel coordinates rather than whole integers.
        /// When it is <c>true</c>, SkiaSharp allows glyphs to be positioned with greater
        /// accuracy, especially useful for: small font sizes, high-DPI displays, precise
        /// text layout. This improves horizontal alignment and spacing, making text
        /// appear smoother and more natural.
        /// </remarks>
        public static bool Subpixel = true;

        /// <summary>
        /// Gets or sets default value for <see cref="SKFont.ForceAutoHinting"/> which is used
        /// when <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        /// <remarks>
        /// Forces the font engine (especially FreeType-based platforms) to apply automatic
        /// hinting to glyphs. Hinting improves legibility at small sizes by aligning glyph
        /// outlines to pixel grids.
        /// </remarks>
        public static bool ForceAutoHinting = true;

        /// <summary>
        /// Gets or sets default value for <see cref="SKFont.Hinting"/> which is used
        /// when <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        /// <remarks>
        /// This affects glyph shape adjustments to align better with pixel grids.
        /// It's about how much the font is "nudged" to look sharper at small sizes.
        /// <see cref="SKFontHinting.None"/>:
        /// No hinting — glyphs retain their original shape,
        /// may look blurry at small sizes.
        /// <see cref="SKFontHinting.Slight"/>:
        /// Minimal adjustments — preserves more of the original design.
        /// <see cref="SKFontHinting.Normal"/>:
        /// Balanced hinting — improves legibility without heavy distortion.
        /// <see cref="SKFontHinting.Full"/>:
        /// Aggressive hinting — maximizes sharpness, may distort glyph proportions.
        /// </remarks>
        /// <remarks>
        /// For small UI text use <see cref="SKFontHinting.Normal"/>.
        /// For print or high-DPI use <see cref="SKFontHinting.Slight"/>.
        /// For bitmap-style rendering use <see cref="SKFontHinting.None"/>.
        /// </remarks>
        public static SKFontHinting Hinting = SKFontHinting.Full;

        /// <summary>
        /// Gets or sets default value for <see cref="SKFont.Edging"/> which is used
        /// when <see cref="Font"/> is converted to <see cref="SKFont"/>.
        /// </summary>
        /// <remarks>
        /// This controls how glyph edges are rendered — whether they are aliased,
        /// anti-aliased, or subpixel-rendered.
        /// <see cref="SKFontEdging.Alias"/>:
        /// No smoothing — edges are jagged and pixelated.
        /// <see cref="SKFontEdging.Antialias"/>:
        /// Smooths edges using grayscale blending.
        /// <see cref="SKFontEdging.SubpixelAntialias"/>:
        /// Uses RGB subpixels for sharper text on LCD screens (best for UI clarity).
        /// </remarks>
        /// <remarks>
        /// For small UI text use <see cref="SKFontEdging.SubpixelAntialias"/>.
        /// For print or high-DPI use <see cref="SKFontEdging.Antialias"/>.
        /// For bitmap-style rendering use <see cref="SKFontEdging.Alias"/>.
        /// </remarks>
        public static SKFontEdging Edging = SKFontEdging.Antialias;

        /// <summary>
        /// Assigns default settings to the specified <see cref="SKFont"/> instance.
        /// </summary>
        /// <remarks>This method sets various properties of the <see cref="SKFont"/> instance, such as
        /// subpixel rendering,  hinting, edging, scaling, and auto-hinting, to predefined default values.</remarks>
        /// <param name="font">The <see cref="SKFont"/> instance to which the default settings will be applied.
        /// This parameter cannot be <see langword="null"/>.</param>
        public static void AssignDefaults(SKFont font)
        {
            font.Subpixel = Subpixel;
            font.Hinting = Hinting;
            font.Edging = Edging;
            font.ScaleX = TextScaleX;
            font.ForceAutoHinting = ForceAutoHinting;
        }
    }
}
