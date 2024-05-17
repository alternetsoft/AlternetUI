using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and
    /// style attributes. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("{ToInfoString()}")]
    public class Font : DisposableObject, IEquatable<Font>
    {
        private static Font? defaultFont;
        private static Font? defaultMonoFont;

        private FontStyle? style;
        private int? hashCode;
        private bool? gdiVerticalFont;
        private Font[]? fonts;
        private Font? baseFont;
        private FontFamily? fontFamily;

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a <see cref="FontInfo"/>.
        /// </summary>
        public Font(FontInfo fontInfo)
            : this(fontInfo.Name, fontInfo.SizeInPoints, fontInfo.Style)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font familty
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
        public Font(
            string familyName,
            double emSize,
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
        public Font(
            FontFamily family,
            double emSize,
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
        public Font(Font prototype, double newSize)
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
        public Font(FontFamily family, double emSize, FontStyle style, GraphicsUnit unit)
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
        public Font(
            FontFamily family,
            double emSize,
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
        public Font(
            FontFamily family,
            double emSize,
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
        public Font(
            string familyName,
            double emSize,
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
        public Font(
            string familyName,
            double emSize,
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
        public Font(FontFamily family, double emSize)
            : this(family?.GenericFamily, family?.Name, emSize, FontStyle.Regular)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, and unit.</summary>
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
        public Font(string familyName, double emSize, FontStyle style, GraphicsUnit unit)
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
        public Font(string familyName, double emSize, GraphicsUnit unit)
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
        public Font(string familyName, double emSize)
            : this(null, familyName, emSize, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font" /> using a specified native font.
        /// </summary>
        /// <param name="nativeFont">Native font instance.</param>
        public Font(IFontHandler handler)
        {
            Handler = handler;
        }

        internal Font(
             GenericFontFamily? genericFamily,
             string? familyName,
             double emSize,
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

            Handler = NativePlatform.Default.FontFactory.CreateFont();
            Handler.Update(prm);
        }

        /// <summary>
        /// Gets the default font used in the application.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for the application. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font Default => defaultFont ??= CreateDefaultFont();

        /// <summary>
        /// Gets the default fixed width font used in the application.
        /// </summary>
        /// <value>
        /// The default fixed width <see cref="Font"/> for the application. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font DefaultMono => defaultMonoFont ??= CreateDefaultMonoFont();

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
        public virtual int SizeInPixels => Handler.GetPixelSize();

        /// <summary>
        /// Gets whether font size is in pixels.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUsingSizeInPixels => Handler.IsUsingSizeInPixels();

        /// <summary>
        /// Gets the em-size of this <see cref="Font" /> measured in the units specified by
        /// the <see cref="Font.Unit" /> property.</summary>
        /// <returns>The em-size of this <see cref="Font" />.</returns>
        public virtual double Size
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
        /// Gets a byte value that specifies the character set that this <see cref="Font" /> uses.
        /// </summary>
        /// <returns>
        /// A byte value that specifies the GDI character set that this
        /// <see cref="Font" /> uses. The default is 1. Currently always returns 1. </returns>
        public virtual byte GdiCharSet => 1;

        /// <summary>
        /// Gets the unit of measure for this <see cref="Font" />.</summary>
        /// <returns>A value that represents the unit of
        /// measure for this <see cref="Font" />.</returns>
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
        public virtual int NumericWeight => Handler.GetNumericWeight();

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
        public virtual bool IsFixedWidth => Handler.IsFixedWidth();

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <returns></returns>
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
                return GetWithStyle(Style | FontStyle.Bold);
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
                return GetWithStyle(Style | FontStyle.Underline);
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
        public virtual bool IsBold => GetIsBold(Handler);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is italic.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is italic;
        /// otherwise, <c>false</c>.</value>
        public virtual bool IsItalic => Handler.GetItalic();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// specifies a horizontal line through the font.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> has a horizontal
        /// line through it; otherwise, <c>false</c>.</value>
        public virtual bool IsStrikethrough => Handler.GetStrikethrough();

        /// <summary>
        /// Same as <see cref="IsStrikethrough"/>.
        /// </summary>
        public bool IsStrikeout => Handler.GetStrikethrough();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined;
        /// otherwise, <c>false</c>.</value>
        public virtual bool IsUnderlined => Handler.GetUnderlined();

        /// <summary>
        /// Gets the em-size, in points, of this <see cref="Font"/>.
        /// </summary>
        /// <value>The em-size, in points, of this <see cref="Font"/>.</value>
        public virtual double SizeInPoints
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
            get => NativePlatform.Default.FontFactory.DefaultFontEncoding;
            set => NativePlatform.Default.FontFactory.DefaultFontEncoding = value;
        }

        /// <summary>
        /// Returns the encoding of this font.
        /// </summary>
        /// <remarks>
        /// Note that under Linux the returned value is always UTF8.
        /// </remarks>
        internal FontEncoding Encoding => Handler.GetEncoding();

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
        public static double CheckSize(double emSize)
        {
            if (emSize <= 0 || double.IsInfinity(emSize) || double.IsNaN(emSize))
            {
                BaseApplication.LogError("Invalid font size {emSize}, using default font size.");
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
            double sizeInPoints,
            FontStyle style,
            Font? defaultFont = null)
        {
            var result = defaultFont ?? Font.Default;
            var sameNameAndSize = result.Name == name && result.SizeInPoints == sizeInPoints;

            if (sameNameAndSize)
                return result.GetWithStyle(style);
            else
                return Get(name, sizeInPoints, style);
        }

        /// <summary>
        /// Gets size of the array with <see cref="FontStyle"/> as index.
        /// </summary>
        /// <returns></returns>
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
        public static bool IsVerticalName(string? familyName)
        {
            if (string.IsNullOrEmpty(familyName))
                return false;
#pragma warning disable
            return familyName[0] == '@';
#pragma warning enable
        }

        /// <summary>
        /// Returns a smaller version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is divided by 1.2, the factor of 1.2 being inspired by the
        /// W3C CSS specification.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Smaller()
        {
            return Get(Name, SizeInPoints / 1.2, Style);
        }

        /// <summary>
        /// Returns a larger version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is divided by 1.2, the factor of 1.2 being inspired by the
        /// W3C CSS specification.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Font Larger()
        {
            return Scaled(1.2);
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
        public virtual Font Scaled(double scaleFactor)
        {
            return Get(this.Name, SizeInPoints * scaleFactor, this.Style);
        }

        public static Font Get(string familyName, double emSize, FontStyle style = FontStyle.Regular)
        {
            return new(familyName, emSize, style);
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
        public virtual Font GetWithStyle(FontStyle style)
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
        public virtual bool Equals(string name, double sizeInPoints, FontStyle style)
        {
            return Name == name && SizeInPoints == sizeInPoints && Style == style;
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Font" />.
        /// </summary>
        public virtual Font Clone()
        {
            var nativeFont = NativePlatform.Default.FontFactory.CreateFont(this);
            var result = new Font(nativeFont);
            return result;
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

        public static void CoerceFontParams(ref IFontHandler.FontParams prm)
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
                BaseApplication.LogError("Font name and family are null, using default font.");
                prm.GenericFamily = Alternet.Drawing.GenericFontFamily.Default;
            }

            prm.Size = Alternet.Drawing.Font.CheckSize(prm.Size);
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

        public static List<string> ToUserString(FontStyle style, FontWeight weight)
        {
            List<string> result = new();

            if(style.HasFlag(FontStyle.Underline))
                result.Add("underlined");
            if (style.HasFlag(FontStyle.Strikeout))
                result.Add("strikethrough");
            if (style.HasFlag(FontStyle.Bold))
                result.Add(ToUserString(weight));
            if (style.HasFlag(FontStyle.Italic))
                result.Add("italic");

            return result;
        }

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

        public static bool GetIsBold(IFontHandler font)
        {
            return font.GetWeight() > FontWeight.Normal;
        }

        public static FontStyle GetStyle(IFontHandler font)
        {
            FontStyle result = FontStyle.Regular;

            if (GetIsBold(font))
                result |= FontStyle.Bold;

            if (font.GetItalic())
                result |= FontStyle.Italic;

            if (font.GetUnderlined())
                result |= FontStyle.Underline;

            if (font.GetStrikethrough())
                result |= FontStyle.Strikeout;

            return result;
        }

        public static List<string> ToUserAsList(IFontHandler font)
        {
            var result = ToUserString(GetStyle(font), font.GetWeight());

            string face = font.Name;
            if (!string.IsNullOrEmpty(face))
            {
                if (face.ContainsSpace() || face.ContainsSemicolon() || face.ContainsComma())
                {
                    // eventually remove quote characters: most systems do not
                    // allow them in a facename anyway so this usually does nothing
                    face = face.Replace("'", "");

                    // make it possible for FromUserString() function to understand
                    // that the different words which compose this facename are
                    // not different adjectives or other data but rather all parts
                    // of the facename
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
            var nativeFont = NativePlatform.Default.FontFactory.CreateDefaultFont();
            return new Font(nativeFont);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            Handler.Dispose();
            Handler = null;
        }
    }
}