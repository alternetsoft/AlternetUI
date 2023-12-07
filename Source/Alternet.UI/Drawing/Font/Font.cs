using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and
    /// style attributes. This class cannot be inherited.
    /// </summary>
    [DebuggerDisplay("{ToInfoString()}")]
    public sealed class Font : IDisposable, IEquatable<Font>
    {
        private static Font? defaultFont;

        private readonly string? originalFontName;
        private bool isDisposed;
        private Font? asBold;
        private Font? asUnderlined;
        private int? hashCode;
        private bool? gdiVerticalFont;

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a <see cref="FontInfo"/>.
        /// </summary>
        public Font(FontInfo fontInfo)
            : this(
                  fontInfo.FontFamily,
                  fontInfo.SizeInPoints,
                  fontInfo.Style)
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
            : this(new FontFamily(familyName), emSize, style)
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
        {
            emSize = CheckSize(emSize);

            NativeFont = new UI.Native.Font();
            NativeFont.Initialize(
                ToNativeGenericFamily(family.GenericFamily),
                family.Name,
                emSize,
                (UI.Native.FontStyle)style);
        }

        /// <summary>
        /// Initializes a new <see cref="Font" /> that uses the specified existing <see cref="Font" />
        /// and <see cref="FontStyle" /> enumeration.</summary>
        /// <param name="prototype">The existing <see cref="Font" /> from which to create the
        /// new <see cref="Font" />.</param>
        /// <param name="newStyle">The <see cref="FontStyle" /> to apply to the
        /// new <see cref="Font" />. Multiple values of the <see cref="FontStyle" /> enumeration can be
        /// combined with the <see langword="OR" /> operator.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(Font prototype, FontStyle newStyle)
        {
            NativeFont = new UI.Native.Font();
            originalFontName = prototype.OriginalFontName;
            Initialize(prototype.FontFamily, prototype.Size, newStyle, prototype.Unit, 1, gdiVerticalFont: false);
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
                  prototype.FontFamily,
                  newSize,
                  prototype.Style)
        {
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, and unit.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(FontFamily family, double emSize, FontStyle style, GraphicsUnit unit)
        {
            NativeFont = new UI.Native.Font();
            Initialize(family, emSize, style, unit, 1, gdiVerticalFont: false);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit, and character set.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a
        ///  GDI character set to use for the new font. Currently ignored.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(FontFamily family, double emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
        {
            NativeFont = new UI.Native.Font();
            Initialize(family, emSize, style, unit, gdiCharSet, gdiVerticalFont: false);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit,
        /// and character set.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a
        ///  GDI character set to use for this font. Currently ignored.</param>
        /// <param name="gdiVerticalFont">A Boolean value indicating whether the new font is derived from
        /// a GDI vertical font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(FontFamily family, double emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            NativeFont = new UI.Native.Font();
            Initialize(family, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, unit, and character set.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by
        /// the <paramref name="unit" /> parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a GDI character set to use for
        /// this font.  Currently ignored.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(string familyName, double emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
        {
            NativeFont = new UI.Native.Font();
            Initialize(familyName, emSize, style, unit, gdiCharSet, IsVerticalName(familyName));
        }

        /// <summary>Initializes a new <see cref="Font" /> using the specified size, style, unit, and
        /// character set.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <param name="gdiCharSet">A <see cref="byte" /> that specifies a GDI character set to use
        /// for this font.  Currently ignored.</param>
        /// <param name="gdiVerticalFont">A Boolean value indicating whether the new <see cref="Font" /> is
        /// derived from a GDI vertical font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(string familyName, double emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            NativeFont = new UI.Native.Font();
            Initialize(familyName, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size and unit. Sets the style
        /// to <see cref="FontStyle.Regular" />.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(FontFamily family, float emSize, GraphicsUnit unit)
        {
            NativeFont = new UI.Native.Font();
            Initialize(family, emSize, FontStyle.Regular, unit, 1, gdiVerticalFont: false);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size.</summary>
        /// <param name="family">The <see cref="FontFamily" /> of the new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(FontFamily family, double emSize)
        {
            NativeFont = new UI.Native.Font();
            Initialize(family, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, gdiVerticalFont: false);
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size, style, and unit.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the
        /// new <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="style">The <see cref="FontStyle" /> of the new font.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(string familyName, double emSize, FontStyle style, GraphicsUnit unit)
        {
            NativeFont = new UI.Native.Font();
            Initialize(familyName, emSize, style, unit, 1, IsVerticalName(familyName));
        }

        /// <summary>Initializes a new <see cref="Font" /> using a specified size and unit. The style
        /// is set to <see cref="FontStyle.Regular" />.</summary>
        /// <param name="familyName">A string representation of the <see cref="FontFamily" /> for the new
        /// <see cref="Font" />.</param>
        /// <param name="emSize">The em-size of the new font in the units specified by the <paramref name="unit" />
        /// parameter.</param>
        /// <param name="unit">The <see cref="GraphicsUnit" /> of the new font.</param>
        /// <remarks>
        /// If bad parameters are passed to the font constructor, error message is output to log
        /// and font is created with default parameters. No exceptions are raised.
        /// </remarks>
        public Font(string familyName, double emSize, GraphicsUnit unit)
        {
            NativeFont = new UI.Native.Font();
            Initialize(familyName, emSize, FontStyle.Regular, unit, 1, IsVerticalName(familyName));
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
        {
            NativeFont = new UI.Native.Font();
            Initialize(familyName, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, IsVerticalName(familyName));
        }

        internal Font(UI.Native.Font nativeFont)
        {
            NativeFont = nativeFont;
        }

        /// <summary>
        /// Gets the default font used in the application.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for the application. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font Default => defaultFont ??= Font.CreateDefaultFont();

        /// <summary>
        /// Gets the name of the font originally specified.
        /// </summary>
        /// <returns>
        /// The string representing the name of the font originally specified.
        /// </returns>
        [Browsable(false)]
        public string? OriginalFontName => originalFontName ?? Name;

        /// <summary>
        /// Gets the pixel size.
        /// </summary>
        public Int32Size SizeInPixels => NativeFont.GetPixelSize();

        /// <summary>
        /// Gets whether font size is in pixels.
        /// </summary>
        /// <returns></returns>
        public bool IsUsingSizeInPixels => NativeFont.IsUsingSizeInPixels();

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
        /// <returns>A <see cref="GraphicsUnit" /> that represents the unit of measure for
        /// this <see cref="Font" />.</returns>
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
        /// Gets the font weight as an integer value.
        /// </summary>
        /// <remarks>
        ///  See <see cref="FontWeight"/> for the numeric weight values.
        /// </remarks>
        public int NumericWeight => NativeFont.GetNumericWeight();

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
        public bool IsFixedWidth => NativeFont.IsFixedWidth();

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <returns></returns>
        public FontWeight Weight => (FontWeight)NativeFont.GetWeight();

        /// <summary>
        /// Returns bold version of the font.
        /// </summary>
        [Browsable(false)]
        public Font AsBold
        {
            get
            {
                if (IsBold)
                    return this;

                if (asBold == null)
                    asBold = new(FontFamily, SizeInPoints, FontStyle.Bold);

                return asBold;
            }
        }

        /// <summary>
        /// Returns underlined version of the font.
        /// </summary>
        [Browsable(false)]
        public Font AsUnderlined
        {
            get
            {
                if (IsUnderlined)
                    return this;

                if (asUnderlined == null)
                {
                    asUnderlined = new(FontFamily, SizeInPoints,
                        FontStyle.Underlined);
                }

                return asUnderlined;
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
                return (FontStyle)NativeFont.Style;
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
        public bool IsStrikethrough => NativeFont.GetStrikethrough();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined;
        /// otherwise, <c>false</c>.</value>
        public bool IsUnderlined => NativeFont.GetUnderlined();

        /// <summary>
        /// Gets the em-size, in points, of this <see cref="Font"/>.
        /// </summary>
        /// <value>The em-size, in points, of this <see cref="Font"/>.</value>
        public double SizeInPoints
        {
            get
            {
                CheckDisposed();
                return NativeFont.SizeInPoints;
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
                CheckDisposed();
                return new FontFamily(NativeFont.Name);
            }
        }

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
                return NativeFont.Name;
            }
        }

        /// <summary>
        /// Gets or sets the default font encoding.
        /// </summary>
        internal static int DefaultEncoding
        {
            get => UI.Native.Font.GetDefaultEncoding();
            set => UI.Native.Font.SetDefaultEncoding(value);
        }

        /// <summary>
        /// Returns the encoding of this font.
        /// </summary>
        /// <remarks>
        /// Note that under Linux the returned value is always UTF8.
        /// </remarks>
        internal int Encoding => NativeFont.GetEncoding();

        internal UI.Native.Font NativeFont { get; private set; }

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
            if (result.Equals(name, sizeInPoints, style))
                return result;
            return new(name, sizeInPoints, style);
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
        /// Releases all resources used by this <see cref="Font"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a smaller version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is divided by 1.2, the factor of 1.2 being inspired by the
        /// W3C CSS specification.
        /// </remarks>
        /// <returns></returns>
        public Font Smaller()
        {
            return new(this, SizeInPoints / 1.2);
        }

        /// <summary>
        /// Returns a larger version of this font.
        /// </summary>
        /// <remarks>
        /// The font size is divided by 1.2, the factor of 1.2 being inspired by the
        /// W3C CSS specification.
        /// </remarks>
        /// <returns></returns>
        public Font Larger()
        {
            return new(this, SizeInPoints * 1.2);
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
        public Font Scaled(double scaleFactor)
        {
            return new(this, SizeInPoints * scaleFactor);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            CheckDisposed();
            hashCode ??= NativeFont.Serialize().GetHashCode();
            return hashCode.Value;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Font? other)
        {
            if (other == null)
                return false;

            CheckDisposed();
            return NativeFont.IsEqualTo(other.NativeFont);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            CheckDisposed();
            return NativeFont.Description;
        }

        /// <summary>
        /// Returns a human-readable string representation of this <see cref="Font"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Font"/>.</returns>
        public string ToInfoString()
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
        public bool Equals(string name, double sizeInPoints, FontStyle style)
        {
            return Name == name && SizeInPoints == sizeInPoints && Style == style;
        }

        internal static UI.Native.GenericFontFamily ToNativeGenericFamily(
            GenericFontFamily? value)
        {
            return value == null ?
                UI.Native.GenericFontFamily.None :
                (UI.Native.GenericFontFamily)value;
        }

        internal static double CheckSize(double emSize)
        {
            if (emSize <= 0 || double.IsInfinity(emSize) || double.IsNaN(emSize))
            {
                Application.LogError("Invalid font size {emSize}, using default font size.");
                return Font.Default.Size;

                /*throw new ArgumentException(
                    ErrorMessages.Default.InvalidParameter,
                    nameof(emSize));*/
            }

            return emSize;
        }

        internal static Font? FromInternal(UI.Native.Font? font)
        {
            if (font is null)
                return null;
            return new Font(font);
        }

        internal static Font CreateDefaultFont()
        {
            var nativeFont = new UI.Native.Font();
            nativeFont.InitializeWithDefaultFont();
            return new Font(nativeFont);
        }

        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeFont.Dispose();
                    NativeFont = null!;
                }

                isDisposed = true;
            }
        }

        private void Initialize(
            string familyName,
            double emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet,
            bool gdiVerticalFont)
        {
            Initialize(
                new FontFamily(familyName),
                emSize,
                style,
                unit,
                gdiCharSet,
                gdiVerticalFont);
        }

        private void Initialize(
            FontFamily family,
            double emSize,
            FontStyle style,
            GraphicsUnit unit,
            byte gdiCharSet,
            bool gdiVerticalFont)
        {
            if (unit != GraphicsUnit.Point)
            {
                Application.LogError("Invalid font unit, using default font size");
                unit = GraphicsUnit.Point;
                emSize = Font.Default.Size;
                // throw new ArgumentException(nameof(unit));
            }

            if (family == null)
            {
                Application.LogError("Font family is null, using default font.");
                family = FontFamily.GenericDefault;
                // throw new ArgumentNullException(nameof(family));
            }

            if (double.IsNaN(emSize) || double.IsInfinity(emSize) || emSize <= 0f)
            {
                Application.LogError("Invalid font size, using default font size.");
                unit = GraphicsUnit.Point;
                emSize = Font.Default.Size;

                /*var s = string.Format(
                        ErrorMessages.Default.InvalidBoundArgument,
                        nameof(emSize),
                        emSize,
                        0,
                        "System.Double.MaxValue");
                throw new ArgumentException(s, nameof(emSize));*/
            }

            NativeFont.Initialize(
               ToNativeGenericFamily(family.GenericFamily),
               family.Name,
               emSize,
               (UI.Native.FontStyle)style);
        }
    }
}