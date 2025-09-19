using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with font.
    /// </summary>
    public interface IFontHandler : IDisposable
    {
        /// <summary>
        /// Gets a description string that represents the font.
        /// </summary>
        /// <returns></returns>
        string Description { get; }

        /// <summary>
        /// Gets name of the font.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the em-size, in points, of the font.
        /// </summary>
        /// <returns></returns>
        FontScalar SizeInPoints { get; }

        /// <summary>
        /// Gets font encoding.
        /// </summary>
        /// <returns></returns>
        FontEncoding GetEncoding(Font font);

        /// <summary>
        /// Gets font size in pixels.
        /// </summary>
        int GetPixelSize(Font font);

        /// <summary>
        /// Gets whether font is using size in pixels.
        /// </summary>
        bool IsUsingSizeInPixels(Font font);

        /// <summary>
        /// Gets font weight.
        /// </summary>
        /// <returns></returns>
        int GetNumericWeight(Font font);

        /// <summary>
        /// Gets whether font is a fixed width (monospaced) font.
        /// </summary>
        bool IsFixedWidth(Font font);

        /// <summary>
        /// Gets font weight.
        /// </summary>
        /// <returns></returns>
        FontWeight GetWeight();

        /// <summary>
        /// Gets a value that indicates whether font is strikethrough.
        /// </summary>
        /// <returns></returns>
        bool GetStrikethrough();

        /// <summary>
        /// Gets a value that indicates whether font is underlined.
        /// </summary>
        /// <returns></returns>
        bool GetUnderlined();

        /// <summary>
        /// Gets a value that indicates whether font is italic.
        /// </summary>
        /// <returns></returns>
        bool GetItalic();

        /// <summary>
        /// Gets font as serialized string.
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// Updates font properties.
        /// </summary>
        void Update(Font font, FontParams prm);

        /// <summary>
        /// Indicates whether font is equal to another font.
        /// </summary>
        /// <returns></returns>
        bool Equals(Font font);

        /// <summary>
        /// Contains font properties.
        /// </summary>
        public class FontParams
        {
            /// <summary>
            /// Gets or sets <see cref="GenericFontFamily"/> of the font.
            /// </summary>
            public GenericFontFamily? GenericFamily;

            /// <summary>
            /// Gets or sets font family name.
            /// </summary>
            public string? FamilyName;

            /// <summary>
            /// Gets or sets font size.
            /// </summary>
            public FontScalar Size;

            /// <summary>
            /// Gets or sets font style.
            /// </summary>
            public FontStyle Style = FontStyle.Regular;

            /// <summary>
            /// Gets or sets <see cref="GraphicsUnit"/> of the font.
            /// </summary>
            public GraphicsUnit Unit = GraphicsUnit.Point;

            /// <summary>
            /// Gets or sets char set of the font.
            /// </summary>
            public byte GdiCharSet = 1;

            /// <summary>
            /// Initializes a new instance of the <see cref="FontParams"/> class.
            /// </summary>
            public FontParams()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FontParams"/> class and sets
            /// its properties using the specified font.
            /// </summary>
            /// <param name="font">Font.</param>
            public FontParams(Font font)
            {
                FamilyName = font.Name;
                Size = font.SizeInPoints;
                Style = font.Style;
                GdiCharSet = font.GdiCharSet;
            }
        }
    }
}
