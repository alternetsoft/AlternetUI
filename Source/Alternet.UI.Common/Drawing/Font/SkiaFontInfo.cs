using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents font information used to create <see cref="SKFont"/>.
    /// Contains weight, slant, width, family name and size.
    /// </summary>
    public struct SkiaFontInfo
    {
        /// <summary>
        /// Gets or sets the font weight (for example: Thin, Normal, Bold).
        /// </summary>
        public SKFontStyleWeight Weight;

        /// <summary>
        /// Gets or sets the font slant (upright, italic, or oblique).
        /// </summary>
        public SKFontStyleSlant Slant;

        /// <summary>
        /// Gets or sets the font family name (for example: "Segoe UI", "Arial").
        /// </summary>
        public string? Name;

        /// <summary>
        /// Gets or sets the font width (stretch). Defaults to <see cref="SKFontStyleWidth.Normal"/>.
        /// </summary>
        public SKFontStyleWidth Width = SKFontStyleWidth.Normal;

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public float SizeInDips;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontInfo"/> struct.
        /// This parameterless constructor preserves explicit default initialization semantics.
        /// </summary>
        public SkiaFontInfo()
        {
        }

        /// <summary>
        /// Creates a new <see cref="SKFont"/> instance using the specified font properties.
        /// </summary>
        /// <remarks>If the <see cref="Name"/> property is null or empty, the default typeface is used.
        /// The created font is initialized with the specified size and applies
        /// default settings as defined by <see cref="SkiaFontDefaults.AssignDefaults(SKFont)"/>.</remarks>
        /// <returns>A new <see cref="SKFont"/> instance configured with the specified typeface,
        /// size, and default settings.</returns>
        public readonly SKFont CreateFont()
        {
            SKTypeface typeFace;

            if(string.IsNullOrEmpty(Name))
                typeFace = SKTypeface.Default;
            else
                typeFace = SKTypeface.FromFamilyName(Name, Weight, Width, Slant);

            SKFont skiaFont = new(typeFace, SizeInDips);

            SkiaFontDefaults.AssignDefaults(skiaFont);

            return skiaFont;
        }
    }
}