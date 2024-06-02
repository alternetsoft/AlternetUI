using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
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
        FontSize SizeInPoints { get; }

        /// <summary>
        /// Gets font encoding.
        /// </summary>
        /// <returns></returns>
        FontEncoding GetEncoding();

        /// <summary>
        /// Gets font size in pixels.
        /// </summary>
        int GetPixelSize();

        /// <summary>
        /// Gets whether font is using size in pixels.
        /// </summary>
        bool IsUsingSizeInPixels();

        /// <summary>
        /// Gets font weight.
        /// </summary>
        /// <returns></returns>
        int GetNumericWeight();

        /// <summary>
        /// Gets whether font is a fixed width (monospaced) font.
        /// </summary>
        bool IsFixedWidth();

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
        void Update(FontParams prm);

        /// <summary>
        /// Indicates whether font is equal to another font.
        /// </summary>
        /// <returns></returns>
        bool Equals(Font font);

        public class FontParams
        {
            public GenericFontFamily? GenericFamily;

            public string? FamilyName;

            public FontSize Size;

            public FontStyle Style = FontStyle.Regular;

            public GraphicsUnit Unit = GraphicsUnit.Point;

            public byte GdiCharSet = 1;

            public FontParams()
            {
            }

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
