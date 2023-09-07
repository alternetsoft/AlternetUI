using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines font information. This is different from <see cref="Font"/> as
    /// contains only font properties and no font handle.
    /// </summary>
    public struct FontInfo
    {
        private FontFamily fontFamily = DefaultFontFamily;

        /// <summary>
        /// Creates <see cref="FontInfo"/> instance.
        /// </summary>
        public FontInfo()
        {
        }

        /// <summary>
        /// Creates <see cref="FontInfo"/> instance using <see cref="Font"/>
        /// properties.
        /// </summary>
        public FontInfo(Font font)
        {
            FontFamily = font.FontFamily;
            Style = font.Style;
            SizeInPoints = font.SizeInPoints;
        }

        /// <summary>
        /// Gets or sets default font family for the <see cref="FontInfo"/> instances.
        /// </summary>
        public static FontFamily DefaultFontFamily { get; set; } = FontFamily.GenericDefault;

        /// <summary>
        /// Gets or sets default font size for the <see cref="FontInfo"/> instances.
        /// </summary>
        public static double DefaultSizeInPoints { get; set; } = Font.Default.SizeInPoints;

        /// <summary>
        /// Gets the <see cref="FontFamily"/> name associated with the font.
        /// </summary>
        /// <value>
        /// The <see cref="FontFamily"/> name associated with the font
        /// </value>
        public FontFamily FontFamily
        {
            readonly get
            {
                return fontFamily;
            }

            set
            {
                fontFamily = value ??
                    throw new ArgumentNullException(nameof(FontFamily));
            }
        }

        /// <summary>
        /// Gets the em-size, in points, of the font.
        /// </summary>
        /// <value>The em-size, in points, of the font.</value>
        public double SizeInPoints { get; set; } = DefaultSizeInPoints;

        /// <summary>
        /// Gets font style information.
        /// </summary>
        /// <value>A <see cref="FontStyle"/> enumeration that contains
        /// font style information.</value>
        public FontStyle Style { get; set; } = FontStyle.Regular;

        /// <summary>
        /// Implicit operator conversion from the <see cref="Font"/> instance.
        /// </summary>
        /// <param name="font">Font instance to get properties from.</param>
        public static implicit operator FontInfo(Font font)
        {
            return new(font);
        }

        /// <summary>
        /// Implicit operator conversion to the <see cref="Font"/> instance.
        /// </summary>
        /// <param name="fontInfo"><see cref="FontInfo"/> instance to get
        /// properties from.</param>
        public static implicit operator Font(FontInfo fontInfo)
        {
            return new(fontInfo);
        }

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "[{0}: Name='{1}', Size={2}, Style={3}]",
                GetType().Name,
                FontFamily.Name,
                SizeInPoints,
                Style);
        }
    }
}
