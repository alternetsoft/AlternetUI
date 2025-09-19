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
        private static FontFamily defaultFontFamily = FontFamily.GenericDefault;
        private object nameOrFamily = DefaultFontFamily;
        private FontScalar sizeInPoints = DefaultSizeInPoints;

        /// <summary>
        /// Initializes a new <see cref="FontInfo"/> using a specified font family
        /// name, size in points and style.
        /// </summary>
        /// <param name="familyName">A string representation of the font family
        /// for the new <see cref="FontInfo"/>.</param>
        /// <param name="emSize">The em-size, in points.</param>
        /// <param name="style">The <see cref="FontStyle"/> of the new font info.</param>
        /// <exception cref="ArgumentException"><c>emSize</c> is less than or
        /// equal to 0, evaluates to infinity or is not a valid number.</exception>
        public FontInfo(
            string familyName,
            FontScalar emSize,
            FontStyle style = FontStyle.Regular)
        {
            Font.CheckSize(emSize);
            Name = familyName;
            SizeInPoints = emSize;
            Style = style;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontInfo"/> struct.
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
            nameOrFamily = font.Name;
            Style = font.Style;
            SizeInPoints = font.SizeInPoints;
        }

        /// <summary>
        /// Gets or sets default font family for the <see cref="FontInfo"/> instances.
        /// </summary>
        public static FontFamily DefaultFontFamily
        {
            get => defaultFontFamily;
            set
            {
                value ??= FontFamily.GenericDefault;
                defaultFontFamily = value;
            }
        }

        /// <summary>
        /// Gets or sets default font size for the <see cref="FontInfo"/> instances.
        /// </summary>
        public static FontScalar DefaultSizeInPoints { get; set; } = Font.Default.SizeInPoints;

        /// <summary>
        /// Gets the <see cref="FontFamily"/> name associated with the font.
        /// </summary>
        /// <value>
        /// The <see cref="FontFamily"/> name associated with the font
        /// </value>
        public FontFamily FontFamily
        {
            get
            {
                if (nameOrFamily is string v)
                    nameOrFamily = new FontFamily(v);
                return (FontFamily)nameOrFamily;
            }

            set
            {
                if (nameOrFamily == value)
                    return;
                nameOrFamily = value ?? DefaultFontFamily;
            }
        }

        /// <summary>
        /// Gets the font family name of this <see cref="FontInfo"/>.
        /// </summary>
        /// <value>A string representation of the font family name
        /// of this <see cref="FontInfo"/>.</value>
        public string Name
        {
            readonly get
            {
                if (nameOrFamily is FontFamily v)
                    return v.Name;
                return (string)nameOrFamily;
            }

            set
            {
                if(string.IsNullOrEmpty(value))
                    nameOrFamily = DefaultFontFamily;
                else
                    nameOrFamily = value;
            }
        }

        /// <summary>
        /// Gets the em-size, in points, of the font.
        /// </summary>
        /// <value>The em-size, in points, of the font.</value>
        public FontScalar SizeInPoints
        {
            readonly get
            {
                return sizeInPoints;
            }

            set
            {
                Font.CheckSize(value);
                sizeInPoints = value;
            }
        }

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
                Name,
                SizeInPoints,
                Style);
        }
    }
}
