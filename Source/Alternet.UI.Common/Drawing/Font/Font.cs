using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Alternet.UI;
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
        public Font(object nativeFont)
        {
            NativeObject = nativeFont;
        }

        internal Font(
             GenericFontFamily? genericFamily,
             string? familyName,
             double emSize,
             FontStyle style,
             GraphicsUnit unit = GraphicsUnit.Point,
             byte gdiCharSet = 1)
        {
            NativeDrawing.FontParams prm = new()
            {
                GenericFamily = genericFamily,
                FamilyName = familyName,
                Size = emSize,
                Style = style,
                Unit = GraphicsUnit.Point,
                GdiCharSet = 1,
            };

            NativeObject = NativeDrawing.Default.CreateFont();
            NativeDrawing.Default.UpdateFont(NativeObject, prm);
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
        public string? OriginalFontName => baseFont?.Name ?? Name;

        /// <summary>
        /// Gets the pixel size.
        /// </summary>
        public SizeI SizeInPixels => NativeDrawing.Default.GetFontSizeInPixels(NativeObject);

        /// <summary>
        /// Gets whether font size is in pixels.
        /// </summary>
        /// <returns></returns>
        public bool IsUsingSizeInPixels
            => NativeDrawing.Default.GetFontIsUsingSizeInPixels(NativeObject);

        /// <summary>
        /// Gets the em-size of this <see cref="Font" /> measured in the units specified by
        /// the <see cref="Font.Unit" /> property.</summary>
        /// <returns>The em-size of this <see cref="Font" />.</returns>
        public double Size
        {
            get
            {
                if (IsUsingSizeInPixels)
                    return SizeInPixels.Height;
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
        public byte GdiCharSet => 1;

        /// <summary>
        /// Gets the unit of measure for this <see cref="Font" />.</summary>
        /// <returns>A value that represents the unit of
        /// measure for this <see cref="Font" />.</returns>
        public GraphicsUnit Unit
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
        public Font Base
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
        public int NumericWeight => NativeDrawing.Default.GetFontNumericWeight(NativeObject);

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
        public bool IsFixedWidth => NativeDrawing.Default.GetFontIsFixedWidth(NativeObject);

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <returns></returns>
        public FontWeight Weight => NativeDrawing.Default.GetFontWeight(NativeObject);

        /// <summary>
        /// Returns bold version of the font.
        /// </summary>
        /// <remarks>
        /// Bold style is added to the original font style. For example if font is italic,
        /// this property will return both italic and bold font.
        /// </remarks>
        [Browsable(false)]
        public Font AsBold
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
        public Font AsUnderlined
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
        public FontStyle Style
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetFontStyle(NativeObject);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is bold.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is bold;
        /// otherwise, <c>false</c>.</value>
        public bool IsBold => (Style & FontStyle.Bold) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is italic.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is italic;
        /// otherwise, <c>false</c>.</value>
        public bool IsItalic => (Style & FontStyle.Italic) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// specifies a horizontal line through the font.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> has a horizontal
        /// line through it; otherwise, <c>false</c>.</value>
        public bool IsStrikethrough => NativeDrawing.Default.GetFontStrikethrough(NativeObject);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined;
        /// otherwise, <c>false</c>.</value>
        public bool IsUnderlined => NativeDrawing.Default.GetFontUnderlined(NativeObject);

        /// <summary>
        /// Gets the em-size, in points, of this <see cref="Font"/>.
        /// </summary>
        /// <value>The em-size, in points, of this <see cref="Font"/>.</value>
        public double SizeInPoints
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetFontSizeInPoints(NativeObject);
            }
        }

        /// <summary>
        /// Gets the <see cref="FontFamily"/> associated with this
        /// <see cref="Font"/>.
        /// </summary>
        /// <value>The <see cref="FontFamily"/> associated with this
        /// <see cref="Font"/>.</value>
        public FontFamily FontFamily
        {
            get
            {
                return fontFamily ??= new FontFamily(Name);
            }
        }

        /// <summary>
        /// Gets native font.
        /// </summary>
        public object NativeObject { get; private set; }

        /// <summary>
        /// Gets a <see cref="bool"/> value that indicates whether this <see cref="Font" /> is derived from
        /// a vertical font.</summary>
        /// <returns>
        /// <see langword="true" /> if this <see cref="Font" /> is derived from a vertical font;
        /// otherwise, <see langword="false" />.</returns>
        [Browsable(false)]
        public bool GdiVerticalFont => gdiVerticalFont ??= IsVerticalName(Name);

        /// <summary>
        /// Gets the font family name of this <see cref="Font"/>.
        /// </summary>
        /// <value>A string representation of the font family name
        /// of this <see cref="Font"/>.</value>
        public string Name
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetFontName(NativeObject);
            }
        }

        /// <summary>
        /// Gets or sets the default font encoding.
        /// </summary>
        internal static int DefaultEncoding
        {
            get => NativeDrawing.Default.GetDefaultFontEncoding();
            set => NativeDrawing.Default.SetDefaultFontEncoding(value);
        }

        /// <summary>
        /// Returns the encoding of this font.
        /// </summary>
        /// <remarks>
        /// Note that under Linux the returned value is always UTF8.
        /// </remarks>
        internal int Encoding => NativeDrawing.Default.GetFontEncoding(NativeObject);

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
            hashCode ??= NativeDrawing.Default.GetFontInfoDesc(NativeObject).GetHashCode();
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
            return NativeDrawing.Default.FontEquals(NativeObject, other.NativeObject);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            CheckDisposed();
            return NativeDrawing.Default.FontToString(NativeObject);
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
            var nativeFont = NativeDrawing.Default.CreateFont(NativeObject);
            var result = new Font(nativeFont);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Font"/> from native font.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Font? FromInternal(object? font)
        {
            if (font is null)
                return null;
            return new Font(font);
        }

        internal static Font CreateDefaultMonoFont()
        {
            var family = FontFamily.GenericMonospace;
            var fontGenericMonospace = new Font(family, Default.SizeInPoints);
            return fontGenericMonospace;
        }

        internal static Font CreateDefaultFont()
        {
            var nativeFont = NativeDrawing.Default.CreateDefaultFont();
            return new Font(nativeFont);
        }

        protected object CreateNativeObject()
        {
            return NativeDrawing.Default.CreateFont();
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            if (NativeObject is not null)
            {
                ((IDisposable)NativeObject).Dispose();
                NativeObject = null;
            }
        }
    }
}