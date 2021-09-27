using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and style attributes. This class cannot be inherited.
    /// </summary>
    public sealed class Font : IDisposable, IEquatable<Font>
    {
        private bool isDisposed;

        private int? hashCode;

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font familty name, size in points and style.
        /// </summary>
        /// <param name="familyName">A string representation of the font family for the new Font.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font.</param>
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or equal to 0, evaluates to infinity or is not a valid number.</exception>
        public Font(string familyName, float emSize, FontStyle style = FontStyle.Regular) : this(new FontFamily(familyName), emSize, style)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font family, size in points and style.
        /// </summary>
        /// <param name="family">The <see cref="FontFamily"/> of the new <see cref="Font"/>.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font.</param>
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or equal to 0, evaluates to infinity or is not a valid number.</exception>
        public Font(FontFamily family, float emSize, FontStyle style = FontStyle.Regular)
        {
            if (emSize <= 0 || float.IsInfinity(emSize) || float.IsNaN(emSize))
                throw new ArgumentException(nameof(emSize));

            NativeFont = new UI.Native.Font();
            NativeFont.Initialize(
                family.GenericFamily == null ? UI.Native.GenericFontFamily.None : (UI.Native.GenericFontFamily)family.GenericFamily,
                family.Name,
                emSize,
                (UI.Native.FontStyle)style);
        }

        internal Font(UI.Native.Font nativeFont)
        {
            NativeFont = nativeFont;
        }

        /// <summary>
        /// Gets style information for this <see cref="Font"/>.
        /// </summary>
        /// <value>A <see cref="FontStyle"/> enumeration that contains style information for this <see cref="Font"/>.</value>
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
        /// <value><c>true</c> if this <see cref="Font"/> is bold; otherwise, <c>false</c>.</value>
        public bool IsBold => (Style & FontStyle.Bold) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is italic.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is italic; otherwise, <c>false</c>.</value>
        public bool IsItalic => (Style & FontStyle.Italic) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> specifies a horizontal line through the font.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> has a horizontal line through it; otherwise, <c>false</c>.</value>
        public bool IsStrikethrough => (Style & FontStyle.Strikethrough) != 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Font"/> is underlined.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Font"/> is underlined; otherwise, <c>false</c>.</value>
        public bool IsUnderlined => (Style & FontStyle.Underlined) != 0;

        /// <summary>
        /// Gets the em-size, in points, of this <see cref="Font"/>.
        /// </summary>
        /// <value>The em-size, in points, of this <see cref="Font"/>.</value>
        public float SizeInPoints
        {
            get
            {
                CheckDisposed();
                return NativeFont.SizeInPoints;
            }
        }

        /// <summary>
        /// Gets the <see cref="FontFamily"/> associated with this <see cref="Font"/>.
        /// </summary>
        /// <value>The <see cref="FontFamily"/> associated with this <see cref="Font"/>.</value>
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
        /// <value>A string representation of the font family name of this Font.</value>
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
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
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
            if (hashCode == null)
                hashCode = NativeFont.Serialize().GetHashCode();
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
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            var font = obj as Font;

            if (ReferenceEquals(font, null))
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(font);
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