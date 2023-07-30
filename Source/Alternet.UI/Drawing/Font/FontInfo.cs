using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines font information. This is different from <see cref="Font"/> as
    /// contains only font properties and no font handle.
    /// </summary>
    public struct FontInfo
    {
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
        /// Gets the <see cref="FontFamily"/> name associated with the font.
        /// </summary>
        /// <value>
        /// The <see cref="FontFamily"/> name associated with the font
        /// </value>
        public FontFamily FontFamily { get; set; } = FontFamily.GenericDefault;

        /// <summary>
        /// Gets the em-size, in points, of the font.
        /// </summary>
        /// <value>The em-size, in points, of the font.</value>
        public double SizeInPoints { get; set; }

        /// <summary>
        /// Gets font style information.
        /// </summary>
        /// <value>A <see cref="FontStyle"/> enumeration that contains
        /// font style information.</value>
        public FontStyle Style { get; set; } = FontStyle.Regular;
    }
}
