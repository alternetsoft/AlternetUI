using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a particular format for text, including font face, size, and style attributes. This class cannot be inherited.
    /// </summary>
    public sealed class Font : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font familty name and size in points.
        /// </summary>
        /// <param name="familyName">A string representation of the font family for the new Font.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or equal to 0, evaluates to infinity or is not a valid number.</exception>
        public Font(string familyName, float emSize) : this(new FontFamily(familyName), emSize)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Font"/> using a specified font family and size in points.
        /// </summary>
        /// <param name="family">The <see cref="FontFamily"/> of the new <see cref="Font"/>.</param>
        /// <param name="emSize">The em-size, in points, of the new font.</param>
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or equal to 0, evaluates to infinity or is not a valid number.</exception>
        public Font(FontFamily family, float emSize)
        {
            if (emSize <= 0 || float.IsInfinity(emSize) || float.IsNaN(emSize))
                throw new ArgumentException(nameof(emSize));

            NativeFont = new Native.Font();
            NativeFont.Initialize(
                family.GenericFamily == null ? Native.GenericFontFamily.None : (Native.GenericFontFamily)family.GenericFamily,
                family.Name,
                emSize);
        }

        internal Font(Native.Font nativeFont)
        {
            NativeFont = nativeFont;
        }

        internal Native.Font NativeFont { get; private set; }

        internal static Font CreateDefaultFont()
        {
            var nativeFont = new Native.Font();
            nativeFont.InitializeWithDefaultFont();
            return new Font(nativeFont);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Font"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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