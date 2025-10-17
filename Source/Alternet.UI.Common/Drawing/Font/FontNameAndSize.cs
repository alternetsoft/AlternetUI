using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Skia;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains font name and size.
    /// </summary>
    public struct FontNameAndSize
    {
        /// <summary>
        /// Font name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Font size.
        /// </summary>
        public FontScalar Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontNameAndSize"/> struct.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="size">Font size.</param>
        public FontNameAndSize(string name, FontScalar size)
        {
            Name = name;
            Size = size;
        }

        /// <summary>
        /// Gets <see cref="FontNameAndSize"/> for the <see cref="Font.Default"/>.
        /// </summary>
        public static FontNameAndSize Default => (FontNameAndSize)Font.Default;

        /// <summary>
        /// Converts <see cref="FontNameAndSize"/> to <see cref="FontInfo"/>.
        /// </summary>
        public static implicit operator FontInfo(FontNameAndSize value)
        {
            return new(value.Name, value.Size);
        }

        /// <summary>
        /// Returns <paramref name="value"/> if font is SkiaSharp compatible, otherwise
        /// returns <see cref="Default"/>.
        /// </summary>
        /// <param name="value">Font name and size.</param>
        /// <returns></returns>
        public static FontNameAndSize SkiaOrDefault(FontNameAndSize value)
        {
            if (SkiaHelper.IsFamilySkia(value.Name))
                return value;
            return Default;
        }
    }
}
