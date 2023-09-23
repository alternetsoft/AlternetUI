using System;
using System.ComponentModel;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and
    /// style attributes. This class cannot be inherited.
    /// </summary>
    public sealed class Font : IDisposable, IEquatable<Font>
    {
        private static Font? defaultFont;

        private bool isDisposed;
        private Font? asBold;
        private Font? asUnderlined;
        private int? hashCode;

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
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or
        /// equal to 0, evaluates to infinity or is not a valid number.</exception>
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
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or
        /// equal to 0, evaluates to infinity or is not a valid number.</exception>
        public Font(
            FontFamily family,
            double emSize,
            FontStyle style = FontStyle.Regular)
        {
            if (emSize <= 0 || double.IsInfinity(emSize) || double.IsNaN(emSize))
            {
                throw new ArgumentException(
                    ErrorMessages.Default.InvalidParameter,
                    nameof(emSize));
            }

            NativeFont = new UI.Native.Font();
            NativeFont.Initialize(
                ToNativeGenericFamily(family.GenericFamily),
                family.Name,
                emSize,
                (UI.Native.FontStyle)style);
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
        /// Returns bold version of the font.
        /// </summary>
        [Browsable(false)]
        public Font AsBold
        {
            get
            {
                if (IsBold)
                    return this;

                if(asBold == null)
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
        public bool IsStrikethrough => (Style & FontStyle.Strikethrough) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/>
        /// is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined;
        /// otherwise, <c>false</c>.</value>
        public bool IsUnderlined => (Style & FontStyle.Underlined) != 0;

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
        /// Gets the font family name of this <see cref="Font"/>.
        /// </summary>
        /// <value>A string representation of the font family name
        /// of this Font.</value>
        public string Name
        {
            get
            {
                CheckDisposed();
                return NativeFont.Name;
            }
        }

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
        /// Creates new font or gets <see cref="Font.Default"/> if its properties
        /// are equal to the specified.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="sizeInPoints">Font size.</param>
        /// <param name="style">Font style</param>
        public static Font GetDefaultOrNew(string name, double sizeInPoints, FontStyle style)
        {
            var result = Font.Default;
            if (result.Equals(name, sizeInPoints, style))
                return result;
            return new(name, sizeInPoints, style);
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
    }
}