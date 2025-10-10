using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

using Alternet.UI;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and
    /// style attributes. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("{ToInfoString()}")]
    public class Font : DisposableObject, IEquatable<Font>
    {
        /// <summary>
        /// Gets or sets default font size scaling factor for <see cref="Smaller"/>
        /// and <see cref="Larger"/> methods.
        /// </summary>
        /// <remarks>
        /// Default value of 1.2 was inspired by the W3C CSS specification.
        /// </remarks>
        public static FontScalar SmallerLargerSizeScaleFactor = 1.2f;

        private static Font? defaultFont;
        private static Font? defaultMonoFont;

        private SKFont? skiaFont;
        private FontStyle? style;
        private int? hashCode;
        private bool? gdiVerticalFont;
        private Font[]? fonts;
        private Font? baseFont;
        private FontFamily? fontFamily;
        private ObjectUniqueId? uniqueId;

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a <see cref="FontInfo"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(FontInfo fontInfo)
            : this(fontInfo.Name, fontInfo.SizeInPoints, fontInfo.Style)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font family
        /// name, size in points and style.
        /// </summary>
        /// <param name="familyName">A string representation of the font family
        /// for the new Font.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            string familyName,
            FontScalar emSize,
            FontStyle style = FontStyle.Regular)
            : this(null, familyName, emSize, style)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font family,
        /// size in points and style.
        /// </summary>
        /// <param name="family">The <see cref="FontFamily"/> of the new
        /// <see cref="Font"/>.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            FontFamily family,
            FontScalar emSize,
            FontStyle style = FontStyle.Regular)
            : this(
                family.GenericFamily,
                family.Name,
                emSize,
                style)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font" /> that uses the specified existing <see cref="Font" />
        /// and <see cref="FontStyle" /> enumeration.</summary>
        /// <param name="prototype">The existing <see cref="Font" /> from which to create the
        /// new <see cref="Font" />.</param>
        /// <param name="newStyle">The <see cref="FontStyle" /> to apply to the
        /// new <see cref="Font" />. Multiple values of the <see cref="FontStyle" />
        /// enumeration can be
        /// combined with the <see langword="OR" /> operator.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(Font prototype, FontStyle newStyle)
            : this(
                prototype.fontFamily?.GenericFamily,
                prototype.Name,
                prototype.Size,
                newStyle)
        {
            baseFont = prototype;
        }

        /// <summary>
        /// Initializes a new <see cref="Font" /> that uses the specified existing <see cref="Font" />
        /// and <paramref name="newSize"/> parameter.</summary>
        /// <param name="prototype">The existing <see cref="Font" /> from which to create the
        /// new <see cref="Font" />.</param>
        /// <param name="newSize">New size of the font in points.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(Font prototype, FontScalar newSize)
            : this(
                prototype.fontFamily?.GenericFamily,
                prototype.Name,
                newSize,
                prototype.Style)
        {
            baseFont = prototype;
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified
        /// size, style, and unit.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by
        /// the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(FontFamily family, FontScalar emSize, FontStyle style, GraphicsUnit unit)
            : this(family?.GenericFamily, family?.Name, emSize, style, unit)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit,
        /// and character set.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the
        /// <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a
        ///  GDI character set to use for the new font. Currently ignored.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            FontFamily family,
            FontScalar emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet)
            : this(family?.GenericFamily, family?.Name, emSize, style, unit, gdiCharSet)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit,
        /// and character set.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the
        /// <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a
        ///  GDI character set to use for this font. Currently ignored.</param>
        /// <param name="gdiVerticalFont">A Boolean value indicating whether the new font is
        /// derived from
        /// a GDI vertical font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            FontFamily family,
            FontScalar emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet,
            bool gdiVerticalFont)
            : this(family?.GenericFamily, family?.Name, emSize, style, unit, gdiCharSet)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit,
        /// and character set.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by
        /// the <paramref name="unit" /> parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a GDI character set
        /// to use for
        /// this font.  Currently ignored.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            string familyName,
            FontScalar emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet)
            : this(null, familyName, emSize, style, unit, gdiCharSet)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using the specified size, style, unit, and
        /// character set.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by
        /// the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a GDI character
        /// set to use
        /// for this font.  Currently ignored.</param>
        /// <param name="gdiVerticalFont">A Boolean value indicating whether
        /// the new <see cref="Font" /> is
        /// derived from a GDI vertical font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(
            string familyName,
            FontScalar emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet,
            bool gdiVerticalFont)
            : this(null, familyName, emSize, style, unit, gdiCharSet)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified
        /// size and unit. Sets the style
        /// to <see cref="FontStyle.Regular" />.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units
        /// specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(FontFamily family, float emSize, GraphicsUnit unit)
            : this(family?.GenericFamily, family?.Name, emSize, FontStyle.Regular, unit)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(FontFamily family, FontScalar emSize)
            : this(family?.GenericFamily, family?.Name, emSize, FontStyle.Regular)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size,
        /// style, and unit.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified
        /// by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(string familyName, FontScalar emSize, FontStyle style, GraphicsUnit unit)
            : this(null, familyName, emSize, style, unit)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size and unit. The style
        /// is set to <see cref="FontStyle.Regular" />.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" />
        /// for the new
        /// <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by
        /// the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="unit">The unit of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(string familyName, FontScalar emSize, GraphicsUnit unit)
            : this(null, familyName, emSize, FontStyle.Regular, unit)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(string familyName, FontScalar emSize)
            : this(null, familyName, emSize, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font" /> using a specified font handler.
        /// </summary>
        /// <param name="handler">Font handler.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font(IFontHandler handler)
        {
            Handler = handler;
        }

        internal Font(
             GenericFontFamily? genericFamily,
             string? familyName,
             FontScalar emSize,
             FontStyle style,
             GraphicsUnit unit = GraphicsUnit.Point,
             byte gdiCharSet = 1)
        {
            IFontHandler.FontParams prm = new()
            {
                GenericFamily = genericFamily,
                FamilyName = familyName,
                Size = emSize,
                Style = style,
                Unit = GraphicsUnit.Point,
                GdiCharSet = 1,
            };

            Handler = FontFactory.Handler.CreateFontHandler();
            Handler.Update(this, prm);
        }

        /// <summary>
        /// Gets the default font used in the application.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for the application. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font Default
        {
            get => defaultFont ??= CreateDefaultFont();

            set
            {
                if (value is null)
                    defaultFont = CreateDefaultFont();
                else
                    defaultFont = value;
            }
        }

        /// <summary>
        /// Gets the default fixed width font used in the application.
        /// </summary>
        /// <value>
        /// The default fixed width <see cref="Font"/> for the application. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font DefaultMono
        {
            get
            {
                return defaultMonoFont ??= CreateDefaultMonoFont();
            }

            set
            {
                if (value is null)
                    defaultMonoFont = CreateDefaultMonoFont();
                else
                    defaultMonoFont = value;
            }
        }

        /// <summary>
        /// Gets the name of the font originally specified.
        /// </summary>
        /// <returns>
        /// The string representing the name of the font originally specified.
        /// </returns>
        [Browsable(false)]
        public virtual string? OriginalFontName => baseFont?.Name ?? Name;

        /// <summary>
        /// Gets the pixel size.
        /// </summary>
        [Browsable(false)]
        public virtual int SizeInPixels
        {
            get
            {
                return Handler.GetPixelSize(this);
            }
        }

        /// <summary>
        /// Gets the size in dips.
        /// </summary>
        [Browsable(false)]
        public virtual Coord SizeInDips => GraphicsFactory.PixelToDip(SizeInPixels);

        /// <summary>
        /// Gets whether font size is in pixels.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual bool IsUsingSizeInPixels
        {
            get
            {
                return Handler.IsUsingSizeInPixels(this);
            }
        }

        /// <summary>
        /// Gets the em-size of this <see cref="Font" /> measured in the units specified by
        /// the <see cref="Font.Unit" /> property.</summary>
        /// <returns>The em-size of this <see cref="Font" />.</returns>
        public virtual FontScalar Size
        {
            get
            {
                if (IsUsingSizeInPixels)
                    return SizeInPixels;
                else
                    return SizeInPoints;
            }
        }

        /// <summary>
        /// Gets <see cref="SKFontMetrics"/> for this font.
        /// </summary>
        [Browsable(false)]
        public virtual SKFontMetrics SkiaMetrics
        {
            get
            {
                return SkiaFont.Metrics;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SKFont"/> for this font.
        /// </summary>
        [Browsable(false)]
        public virtual SKFont SkiaFont
        {
            get
            {
                return skiaFont ??= GraphicsFactory.FontToSkiaFont(this);
            }

            set
            {
                skiaFont = value;
            }
        }

        /// <summary>
        /// Gets a byte value that specifies the character set that this <see cref="Font" /> uses.
        /// </summary>
        /// <returns>
        /// A byte value that specifies the GDI character set that this
        /// <see cref="Font" /> uses. The default is 1. Currently always returns 1. </returns>
        [Browsable(false)]
        public virtual byte GdiCharSet => 1;

        /// <summary>
        /// Gets the unit of measure for this <see cref="Font" />.</summary>
        /// <returns>A value that represents the unit of
        /// measure for this <see cref="Font" />.</returns>
        [Browsable(false)]
        public virtual GraphicsUnit Unit
        {
            get
            {
                if (IsUsingSizeInPixels)
                    return GraphicsUnit.Pixel;
                else
                    return GraphicsUnit.Point;
            }
        }

        /// <summary>
        /// Gets this font with <see cref="FontStyle.Regular"/> style.
        /// </summary>
        [Browsable(false)]
        public virtual Font Base
        {
            get
            {
                if (Style == FontStyle.Regular)
                    return this;
                return baseFont ??= Get(Name, SizeInPoints, FontStyle.Regular);
            }
        }

        /// <summary>
        /// Gets the font weight as an integer value.
        /// </summary>
        /// <remarks>
        ///  See <see cref="FontWeight"/> for the numeric weight values.
        /// </remarks>
        [Browsable(false)]
        public virtual int NumericWeight
        {
            get => Handler.GetNumericWeight(this);
        }

        /// <summary>
        /// Gets whether this font is a fixed width (or monospaced) font.
        /// </summary>
        /// <returns>Returns <c>true</c> if the font is a fixed width (or monospaced) font,
        /// <c>false</c> if it is a proportional one or font is invalid.</returns>
        /// <remarks>
        /// Note that this function under some platforms is different from just testing for the
        /// font family being equal to <see cref="GenericFontFamily.Monospace"/> because native
        /// platform-specific functions are used for the check (resulting in a more accurate
        /// return value).
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsFixedWidth
        {
            get
            {
                return Handler.IsFixedWidth(this);
            }
        }

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual FontWeight Weight => Handler.GetWeight();

        /// <summary>
        /// Returns bold version of the font.
        /// </summary>
        /// <remarks>
        /// Bold style is added to the original font style. For example if font is italic,
        /// this property will return both italic and bold font.
        /// </remarks>
        [Browsable(false)]
        public virtual Font AsBold
        {
            get
            {
                return WithStyle(Style | FontStyle.Bold);
            }
        }

        /// <summary>
        /// Returns underlined version of the font.
        /// </summary>
        /// <remarks>
        /// Underlined style is added to the original font style. For example if font is italic,
        /// this property will return both italic and underlined font.
        /// </remarks>
        [Browsable(false)]
        public virtual Font AsUnderlined
        {
            get
            {
                return WithStyle(Style | FontStyle.Underline);
            }
        }

        /// <summary>
        /// Gets style information for this <see cref="Font"/>.
        /// </summary>
        /// <value>A <see cref="FontStyle"/> enumeration that contains
        /// style information for this <see cref="Font"/>.</value>
        public virtual FontStyle Style
        {
            get
            {
                return style ??= GetStyle(Handler);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is bold.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is bold;
        /// otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public virtual bool IsBold => GetIsBold(Weight);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is italic.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is italic;
        /// otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public virtual bool IsItalic => Handler.GetItalic();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// specifies a horizontal line through the font.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> has a horizontal
        /// line through it; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public virtual bool IsStrikethrough => Handler.GetStrikethrough();

        /// <summary>
        /// Same as <see cref="IsStrikethrough"/>.
        /// </summary>
        [Browsable(false)]
        public bool IsStrikeout => Handler.GetStrikethrough();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined;
        /// otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public virtual bool IsUnderlined => Handler.GetUnderlined();

        /// <summary>
        /// Gets the em-size, in points, of this <see cref="Font"/>.
        /// </summary>
        /// <value>The em-size, in points, of this <see cref="Font"/>.</value>
        public virtual FontScalar SizeInPoints
        {
            get
            {
                CheckDisposed();
                return Handler.SizeInPoints;
            }
        }

        /// <summary>
        /// Gets the <see cref="FontFamily"/> associated with this
        /// <see cref="Font"/>.
        /// </summary>
        /// <value>The <see cref="FontFamily"/> associated with this
        /// <see cref="Font"/>.</value>
        [Browsable(false)]
        public virtual FontFamily FontFamily
        {
            get
            {
                return fontFamily ??= new FontFamily(Name);
            }
        }

        /// <summary>
        /// Gets native font.
        /// </summary>
        [Browsable(false)]
        public virtual IFontHandler Handler { get; private set; }

        /// <summary>
        /// Gets a <see cref="bool"/> value that indicates whether this
        /// <see cref="Font" /> is derived from
        /// a vertical font.</summary>
        /// <returns>
        /// <see langword="true" /> if this <see cref="Font" /> is derived from a vertical font;
        /// otherwise, <see langword="false" />.</returns>
        [Browsable(false)]
        public virtual bool GdiVerticalFont => gdiVerticalFont ??= IsVerticalName(Name);

        /// <summary>
        /// Gets the font family name of this <see cref="Font"/>.
        /// </summary>
        /// <value>A string representation of the font family name
        /// of this <see cref="Font"/>.</value>
        public virtual string Name
        {
            get
            {
                CheckDisposed();
                return Handler.Name;
            }
        }

        /// <summary>
        /// Gets or sets the default font encoding.
        /// </summary>
        internal static FontEncoding DefaultEncoding
        {
            get => FontFactory.Handler.DefaultFontEncoding;
            set => FontFactory.Handler.DefaultFontEncoding = value;
        }

        /// <summary>
        /// Returns the encoding of this font.
        /// </summary>
        /// <remarks>
        /// Note that under Linux the returned value is always UTF8.
        /// </remarks>
        internal FontEncoding Encoding => Handler.GetEncoding(this);

        /// <summary>
        /// Converts the specified <see cref='Font'/> to a <see cref='SKFont'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKFont(Font font)
        {
            font ??= Font.Default;
            return font.SkiaFont;
        }

        /// <summary>
        /// Converts the specified <see cref='Font'/> to a <see cref='FontNameAndSize'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FontNameAndSize(Font font)
        {
            font ??= Font.Default;
            return new(font.Name, font.SizeInPoints);
        }

        /// <summary>
        /// Converts the specified <see cref='FontNameAndSize'/> to a <see cref='Font'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Font(FontNameAndSize font)
        {
            return new(font.Name, font.Size);
        }

        /// <summary>
        /// Returns a value that indicates whether the two objects are equal.
        /// </summary>
        public static bool operator ==(Font? a, Font? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Returns a value that indicates whether the two objects are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Font? a, Font? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Turns on or off <see cref="FontStyle"/> element(s).
        /// </summary>
        /// <param name="value">Original value.</param>
        /// <param name="element">Element to add or remove.</param>
        /// <param name="add"><c>true</c> to add, <c>false</c> to remove.</param>
        /// <returns>Changed <see cref="FontStyle"/> value with added or removed element.</returns>
        /// <example>
        /// <code>
        /// Style = Font.ChangeFontStyle(Style, FontStyle.Italic, value);
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FontStyle ChangeFontStyle(FontStyle value, FontStyle element, bool add)
        {
            var result = value;
            if (add)
                result |= element;
            else
                result &= ~element;
            return result;
        }

        /// <summary>
        /// If font size is correct, returns it; otherwise returns default font size.
        /// </summary>
        /// <param name="emSize"></param>
        /// <returns></returns>
        public static FontScalar CheckSize(FontScalar emSize)
        {
            if (emSize <= 0 || FontScalar.IsInfinity(emSize) || FontScalar.IsNaN(emSize))
            {
                App.LogError("Invalid font size {emSize}, using default font size.");
                return Font.Default.Size;
            }

            return emSize;
        }

        /// <summary>
        /// Creates new font or gets <paramref name="defaultFont"/> if its properties
        /// are equal to the specified.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="sizeInPoints">Font size.</param>
        /// <param name="style">Font style</param>
        /// <param name="defaultFont">Default font. Optional. If <c>null</c>,
        /// <see cref="Font.Default"/> is used.</param>
        public static Font GetDefaultOrNew(
            string name,
            FontScalar sizeInPoints,
            FontStyle style,
            Font? defaultFont = null)
        {
            var result = defaultFont ?? Font.Default;
            var sameNameAndSize = result.Name == name && result.SizeInPoints == sizeInPoints;

            if (sameNameAndSize)
                return result.WithStyle(style);
            else
                return Get(name, sizeInPoints, style);
        }

        /// <summary>
        /// Gets size of the array with <see cref="FontStyle"/> as index.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetFontStyleArraySize()
        {
            var a = FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout | FontStyle.Underline;
            return (int)a + 1;
        }

        /// <summary>
        /// Gets whether font name is from vertical font (starts with @ char).
        /// </summary>
        /// <param name="familyName">Name of the font.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVerticalName(string? familyName)
        {
            if (string.IsNullOrEmpty(familyName))
                return false;
#pragma warning disable
            return familyName[0] == '@';
#pragma warning enable
        }

        /// <summary>
        /// Returns a unscaled version of this font.
        /// </summary>
        /// <param name="scaleFactor">Font size scaling factor.</param>
        /// <returns></returns>
        /// <remarks>
        /// The font size is divided by the given <paramref name="scaleFactor"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Unscaled(FontScalar scaleFactor)
        {
            return Get(Name, SizeInPoints / scaleFactor, Style);
        }

        /// <summary>
        /// Returns a smaller version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is divided by <see cref="SmallerLargerSizeScaleFactor"/>.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Smaller()
        {
            return Get(Name, SizeInPoints / SmallerLargerSizeScaleFactor, Style);
        }

        /// <summary>
        /// Returns a larger version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is multiplied by <see cref="SmallerLargerSizeScaleFactor"/>.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Larger()
        {
            return Scaled(SmallerLargerSizeScaleFactor);
        }

        /// <summary>
        /// Returns a scaled version of this font.
        /// </summary>
        /// <param name="scaleFactor">Font size scaling factor.</param>
        /// <returns></returns>
        /// <remarks>
        /// The font size is multiplied by the given <paramref name="scaleFactor"/>
        /// (which may be less than 1 to create a smaller version of the font).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Scaled(FontScalar scaleFactor)
        {
            return Get(this.Name, SizeInPoints * scaleFactor, this.Style);
        }

        /// <summary>
        /// Creates font with the specified parameters.
        /// </summary>
        /// <param name="familyName">A string representation of the font family
        /// for the new font.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Font Get(string familyName, FontScalar emSize, FontStyle style = FontStyle.Regular)
        {
            return new(familyName, emSize, style);
        }

        [Obsolete("Use Font.WithStyle")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font GetWithStyle(FontStyle style)
        {
            return WithStyle(style);
        }

        /// <summary>
        /// Returns font with same parameters, but with different name.
        /// </summary>
        /// <param name="name">New font name.</param>
        /// <returns></returns>
        public virtual Font WithName(string name)
        {
            FontInfo info = this;
            info.Name = name;
            return new(info);
        }

        /// <summary>
        /// Converts the current font settings to a <see cref="SkiaFontInfo"/> object.
        /// </summary>
        /// <remarks>The resulting <see cref="SkiaFontInfo"/> includes the font's weight, slant, name, 
        /// and size based on the current settings.</remarks>
        /// <returns>A <see cref="SkiaFontInfo"/> object representing the font's attributes.</returns>
        public virtual SkiaFontInfo ToSkiaFontInfo()
        {
            SkiaFontInfo result = new();

            result.Weight = (SKFontStyleWeight)Weight;
            result.Slant = IsItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
            result.Name = Name;
            result.SizeInDips = SizeInDips;
            return result;
        }

        /// <summary>
        /// Returns this font with same parameters, but with different size.
        /// </summary>
        /// <param name="sizeInPoints">New font size in points.</param>
        /// <returns></returns>
        public virtual Font WithSize(Coord sizeInPoints)
        {
            FontInfo info = this;
            info.SizeInPoints = sizeInPoints;
            return new(info);
        }

        /// <summary>
        /// Returns this font with same parameters, but with incremented size.
        /// </summary>
        /// <param name="sizeInPoints">Value on which size of this font is incremented.</param>
        /// <returns></returns>
        public virtual Font IncSize(Coord sizeInPoints = 1)
        {
            FontInfo info = this;
            info.SizeInPoints = Size + sizeInPoints;
            return new(info);
        }

        /// <summary>
        /// Returns font with same parameters, but with different bold style.
        /// </summary>
        /// <param name="isBold">Whether returned font is bold or not.</param>
        /// <returns></returns>
        public virtual Font WithBold(bool isBold)
        {
            if (isBold)
                return AsBold;
            var style = ChangeFontStyle(Style, FontStyle.Bold, false);
            return WithStyle(style);
        }

        /// <summary>
        /// Returns font with same name and size, but with different <see cref="FontStyle"/>.
        /// </summary>
        /// <param name="style">New font style.</param>
        /// <returns></returns>
        /// <remarks>
        /// This function saves created fonts in the base font and is memory efficient.
        /// creates 
        /// </remarks>
        public virtual Font WithStyle(FontStyle style)
        {
            if (Style == style)
                return this;
            if (Style == FontStyle.Regular)
                return Internal(this);
            return Internal(Base);

            Font Internal(Font fnt)
            {
                if (style == FontStyle.Regular)
                    return fnt;
                fnt.fonts ??= new Font[GetFontStyleArraySize()];
                Font result = fnt.fonts[(int)style];
                if (result is not null)
                    return result;
                result = Get(Name, SizeInPoints, style);
                fnt.fonts[(int)style] = result;
                return result;
            }
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            CheckDisposed();
            hashCode ??= Handler.Serialize().GetHashCode();
            return hashCode.Value;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public virtual bool Equals(Font? other)
        {
            if (other == null)
                return false;

            CheckDisposed();
            return Handler.Equals(other);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            CheckDisposed();
            return ToUserString(Handler);
        }

        /// <summary>
        /// Returns a human-readable string representation of this <see cref="Font"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Font"/>.</returns>
        public virtual string ToInfoString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "[{0}: Name={1}, Size={2}, PixelSize={3}, Style={4}]",
                GetType().Name,
                Name,
                SizeInPoints,
                SizeInPixels,
                Style);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Font font)
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(font);
        }

        /// <summary>
        /// Indicates whether the current objects properties are equal to the
        /// specified properties.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="sizeInPoints">Font size.</param>
        /// <param name="style">Font style.</param>
        /// <returns></returns>
        public virtual bool Equals(string name, FontScalar sizeInPoints, FontStyle style)
        {
            return Name == name && SizeInPoints == sizeInPoints && Style == style;
        }

        /// <summary>
        /// Determines whether the current font matches the specified name, style,
        /// and size within a close tolerance.
        /// </summary>
        /// <param name="name">The name of the font to compare.</param>
        /// <param name="sizeInPoints">The size of the font, in points, to compare.</param>
        /// <param name="style">The style of the font to compare.</param>
        /// <returns><see langword="true"/> if the current font's name and style exactly match
        /// the specified values and size is within a close tolerance; 
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool IsClose(string name, FontScalar sizeInPoints, FontStyle style)
        {
            return Name == name && Style == style && MathUtils.AreClose(SizeInPoints, sizeInPoints);
        }

        /// <summary>
        /// Gets unique id of this object.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId UniqueId
        {
            get
            {
                return uniqueId ??= new();
            }
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Font" />.
        /// </summary>
        public virtual Font Clone()
        {
            var result = FontFactory.Handler.CreateFontHandler();
            IFontHandler.FontParams prm = new(this);
            result.Update(this, prm);
            return new Font(result);
        }

        /// <summary>
        /// Creates <see cref="Font"/> from native font.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Font? FromInternal(IFontHandler? font)
        {
            if (font is null)
                return null;
            return new Font(font);
        }

        /// <summary>
        /// Helper function for converting <see cref="FontWeight"/> enum value
        /// to the numeric weight.
        /// </summary>
        /// <param name="value">Font weight.</param>
        /// <returns></returns>
        public static int GetNumericWeightOf(FontWeight value)
        {
            int weight = ConvertFromLegacyWeightIfNecessary((int)value);

            Debug.Assert(weight > (int)FontWeight.Invalid);
            Debug.Assert(weight <= (int)FontWeight.Max);
            Debug.Assert(weight % 100 == 0);

            return weight;

            int ConvertFromLegacyWeightIfNecessary(int weight)
            {
                switch (weight)
                {
                    case 90: return (int)FontWeight.Normal;
                    case 91: return (int)FontWeight.Light;
                    case 92: return (int)FontWeight.Bold;
                    default: return weight;
                }
            }
        }

        /// <summary>
        /// Adjusts the specified font parameters to ensure they conform to expected values.
        /// </summary>
        /// <remarks>This method ensures that the font size is converted to points
        /// if it is specified in a
        /// different unit, and that a default font family is assigned if both the
        /// generic family and specific family
        /// name are null. Additionally, the font size is validated to ensure
        /// it falls within acceptable bounds.</remarks>
        /// <param name="prm">The font parameters to coerce. The
        /// <see cref="IFontHandler.FontParams"/> object must specify the font size,
        /// unit, and optionally the font family or generic family.</param>
        public static void CoerceFontParams(IFontHandler.FontParams prm)
        {
            if (prm.Unit != GraphicsUnit.Point)
            {
                prm.Size = GraphicsUnitConverter.Convert(
                    prm.Unit,
                    GraphicsUnit.Point,
                    Display.Primary.DPI.Height,
                    prm.Size);
            }

            if (prm.GenericFamily == null && prm.FamilyName == null)
            {
                if (!FontFactory.Handler.AllowNullFontName)
                    App.LogError("Font name and family are null, using default font.");
                prm.GenericFamily = Alternet.Drawing.GenericFontFamily.Default;
            }

            prm.Size = Alternet.Drawing.Font.CheckSize(prm.Size);
        }

        /// <summary>
        /// Helper function for converting arbitrary numeric width to the closest
        /// value of <see cref="SKFontStyleWidth"/> enum.
        /// </summary>
        /// <param name="width">Numeric width.</param>
        /// <returns></returns>
        public static SKFontStyleWidth? GetSkiaWidthClosestToNumericValue(int width)
        {
            if (width > 0 && width <= (int)SKFontStyleWidth.UltraExpanded)
                return (SKFontStyleWidth)width;
            return null;
        }

        /// <summary>
        /// Helper function for converting arbitrary numeric weight to the closest
        /// value of <see cref="FontWeight"/> enum.
        /// </summary>
        /// <param name="numWeight">Numeric weight.</param>
        /// <returns></returns>
        public static FontWeight GetWeightClosestToNumericValue(int numWeight)
        {
            Debug.Assert(numWeight > 0);
            Debug.Assert(numWeight <= 1000);

            // round to nearest hundredth = FontWeight.* constant
            int weight = ((numWeight + 50) / 100) * 100;

            if (weight < (int)FontWeight.Thin)
                weight = (int)FontWeight.Thin;
            if (weight > (int)FontWeight.Max)
                weight = (int)FontWeight.Max;

            return (FontWeight)weight;
        }

        /// <summary>
        /// Converts the specified <see cref="FontWeight"/> value to its corresponding
        /// user-friendly string representation.
        /// </summary>
        /// <param name="weight">The <see cref="FontWeight"/> value to convert.</param>
        /// <returns>A string representing the specified font weight. For example,
        /// "thin" for <see cref="FontWeight.Thin"/> or
        /// "bold" for <see cref="FontWeight.Bold"/>.
        /// Returns an empty string if the font weight is <see cref="FontWeight.Normal"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the
        /// <paramref name="weight"/> value is not a recognized <see cref="FontWeight"/>.</exception>
        public static string ToUserString(FontWeight weight)
        {
            switch (weight)
            {
                default:
                    throw new ArgumentException("Unknown font weight", nameof(weight));
                case FontWeight.Normal:
                    return string.Empty;
                case FontWeight.Thin:
                    return "thin";
                case FontWeight.ExtraLight:
                    return "extra light";
                case FontWeight.Light:
                    return "light";
                case FontWeight.Medium:
                    return "medium";
                case FontWeight.SemiBold:
                    return "semi bold";
                case FontWeight.Bold:
                    return "bold";
                case FontWeight.ExtraBold:
                    return "extra bold";
                case FontWeight.Heavy:
                    return "heavy";
                case FontWeight.ExtraHeavy:
                    return "extra heavy";
            }
        }

        /// <summary>
        /// Converts the specified font style and weight into a list
        /// of user-friendly string descriptions.
        /// </summary>
        /// <remarks>The method checks for specific flags in the <paramref name="style"/> parameter and
        /// adds corresponding descriptions to the result. If the <see cref="FontStyle.Bold"/>
        /// flag is set, the <paramref name="weight"/> parameter is used
        /// to generate a description for the font weight.</remarks>
        /// <param name="style">The <see cref="FontStyle"/> value representing
        /// the font styles to convert. Multiple styles can be combined
        /// using bitwise operations.</param>
        /// <param name="weight">The <see cref="FontWeight"/> value
        /// representing the font weight to include in the description if the <see
        /// cref="FontStyle.Bold"/> flag is set.</param>
        /// <returns>A list of strings describing the font styles and weight.
        /// The list may include "underlined", "strikethrough",
        /// "italic", and a description of the font weight if applicable.</returns>
        public static List<string> ToUserString(FontStyle style, FontWeight weight)
        {
            List<string> result = new();

            if (style.HasFlag(FontStyle.Underline))
                result.Add("underlined");
            if (style.HasFlag(FontStyle.Strikeout))
                result.Add("strikethrough");
            if (style.HasFlag(FontStyle.Bold))
                result.Add(ToUserString(weight));
            if (style.HasFlag(FontStyle.Italic))
                result.Add("italic");

            return result;
        }

        /// <summary>
        /// Converts a <see cref="GenericFontFamily"/> value to its corresponding
        /// user-friendly string representation.
        /// </summary>
        /// <param name="family">The <see cref="GenericFontFamily"/> value to convert.</param>
        /// <returns>A string representing the specified font family. Returns an empty string
        /// for <see cref="GenericFontFamily.None"/> or <see cref="GenericFontFamily.Default"/>.
        /// Returns "swiss" for <see cref="GenericFontFamily.SansSerif"/>,
        /// "roman" for <see cref="GenericFontFamily.Serif"/>,
        /// and "teletype" for <see cref="GenericFontFamily.Monospace"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="family"/>
        /// is not a recognized <see cref="GenericFontFamily"/> value.</exception>
        public static string ToUserString(GenericFontFamily family)
        {
            switch (family)
            {
                case GenericFontFamily.None:
                case GenericFontFamily.Default:
                    return string.Empty;
                case GenericFontFamily.SansSerif:
                    return "swiss";
                case GenericFontFamily.Serif:
                    return "roman";
                case GenericFontFamily.Monospace:
                    return "teletype";
                default:
                    throw new ArgumentException("Unknown font family");
            }
        }

        /// <summary>
        /// Converts the specified font information to a user-friendly string representation.
        /// </summary>
        /// <param name="font">The font to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A string that represents the font in a user-friendly format.</returns>
        public static string ToUserString(IFontHandler font)
        {
            List<string> list = ToUserAsList(font);

            var result = StringUtils.ToString(
                list,
                null,
                null,
                StringUtils.OneSpace);

            return result;
        }

        /// <summary>
        /// Determines whether the specified font weight is considered bold.
        /// </summary>
        /// <param name="weight">The font weight to evaluate.</param>
        /// <returns><see langword="true"/> if the specified font weight
        /// is greater than <see cref="FontWeight.Normal"/>;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetIsBold(FontWeight weight)
        {
            return weight > FontWeight.Normal;
        }

        /// <summary>
        /// Determines the combined <see cref="FontStyle"/> based on the attributes of the specified font.
        /// </summary>
        /// <remarks>The returned <see cref="FontStyle"/> is a bitwise combination of applicable styles.
        /// For example, if the font is both bold and italic, the result will include both
        /// <see cref="FontStyle.Bold"/>
        /// and <see cref="FontStyle.Italic"/>.</remarks>
        /// <param name="font">An object implementing <see cref="IFontHandler"/> that provides
        /// font attributes such as weight, italic,
        /// underline, and strikethrough.</param>
        /// <returns>A <see cref="FontStyle"/> value representing the combination of font styles
        /// (e.g., <see cref="FontStyle.Bold"/>, <see cref="FontStyle.Italic"/>, etc.)
        /// that are applicable to the specified font.
        /// Returns <see cref="FontStyle.Regular"/> if no specific styles are applied.</returns>
        public static FontStyle GetStyle(IFontHandler font)
        {
            FontStyle result = FontStyle.Regular;

            if (GetIsBold(font.GetWeight()))
                result |= FontStyle.Bold;

            if (font.GetItalic())
                result |= FontStyle.Italic;

            if (font.GetUnderlined())
                result |= FontStyle.Underline;

            if (font.GetStrikethrough())
                result |= FontStyle.Strikeout;

            return result;
        }

        /// <summary>
        /// Converts the specified font's attributes into a list of strings representing
        /// its user-readable properties.
        /// </summary>
        /// <remarks>The method ensures that the font name is properly formatted for user readability.
        /// If the font name contains spaces, semicolons, or commas, it is enclosed in single
        /// quotes to preserve its
        /// integrity as a single entity.</remarks>
        /// <param name="font">An object implementing <see cref="IFontHandler"/>
        /// that provides the font's attributes, such as name, weight, and size.</param>
        /// <returns>A list of strings representing the font's user-readable properties.
        /// The list typically includes the font's style, weight, name, and size in points.</returns>
        public static List<string> ToUserAsList(IFontHandler font)
        {
            var result = ToUserString(GetStyle(font), font.GetWeight());

            string face = font.Name;
            if (!string.IsNullOrEmpty(face))
            {
                if (face.ContainsSpace() || face.ContainsSemicolon() || face.ContainsComma())
                {
                    // eventually remove quote characters: most systems do not
                    // allow them in a face name anyway so this usually does nothing
                    face = face.Replace("'", "");

                    // make it possible for FromUserString() function to understand
                    // that the different words which compose this face name are
                    // not different adjectives or other data but rather all parts
                    // of the face name
                    face = "'" + face + "'";
                }

                result.Add(face);
            }

            int size = (int)font.SizeInPoints;
            result.Add(size.ToString());

            return result;
        }

        internal static Font CreateDefaultMonoFont()
        {
            var family = FontFamily.GenericMonospace;
            var fontGenericMonospace = new Font(family, Default.SizeInPoints);
            return fontGenericMonospace;
        }

        internal static Font CreateDefaultFont()
        {
            var nativeFont = FontFactory.Handler.CreateDefaultFontHandler();
            return new Font(nativeFont);
        }

        /// <summary>
        /// Resets the cached <see cref="SKFont"/> for this font instance.
        /// </summary>
        public virtual void ResetSkiaFont()
        {
            skiaFont = null;

            if(fonts is not null)
            { 
                foreach (var f in fonts)
                {
                    f?.ResetSkiaFont();
                }
            }

            baseFont?.ResetSkiaFont();
        }

        /// <summary>
        /// Calculates the width of the character "x" when rendered with the current font settings.
        /// </summary>
        /// <param name="dc">The <see cref="Graphics"/> context used to measure the text.</param>
        /// <returns>A <see cref="FontScalar"/> representing the width of the character "x"
        /// in the current font.</returns>
        public virtual FontScalar GetWidth(Graphics dc)
        {
            return dc.MeasureText(StringUtils.SymbolLowercaseX, this).Width;
        }

        /// <summary>
        /// Calculates the height of the font when rendered in the specified graphics context.
        /// Uses the height of the characters "Wg" as a reference.
        /// </summary>
        /// <param name="dc">The graphics context used to measure the font height.</param>
        /// <returns>The height of the font as a <see cref="FontScalar"/> value.</returns>
        public virtual FontScalar GetHeight(Graphics dc)
        {
            return dc.MeasureText(StringUtils.SymbolWg, this).Height;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            Handler.Dispose();
            Handler = null;
        }
    }
}