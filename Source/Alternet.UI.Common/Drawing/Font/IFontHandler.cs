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
        /// Returns a string that represents native font.
        /// </summary>
        /// <returns></returns>
        string Description { get; }

        /// <summary>
        /// Gets native font name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets style information for the native font.
        /// </summary>
        /// <returns></returns>
        FontStyle Style { get; }

        /// <summary>
        /// Gets the em-size, in points, of the native font.
        /// </summary>
        /// <returns></returns>
        double SizeInPoints { get; }

        /// <summary>
        /// Gets native font encoding.
        /// </summary>
        /// <returns></returns>
        int GetEncoding();

        /// <summary>
        /// Gets native font size in pixels.
        /// </summary>
        SizeI GetPixelSize();

        /// <summary>
        /// Gets whether native font is using size in pixels.
        /// </summary>
        bool IsUsingSizeInPixels();

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <returns></returns>
        int GetNumericWeight();

        /// <summary>
        /// Gets whether native font is a fixed width (or monospaced) font.
        /// </summary>
        bool IsFixedWidth();

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <returns></returns>
        FontWeight GetWeight();

        /// <summary>
        /// Gets a value that indicates whether native font is strikethrough.
        /// </summary>
        /// <returns></returns>
        bool GetStrikethrough();

        /// <summary>
        /// Gets a value that indicates whether native font is underlined.
        /// </summary>
        /// <returns></returns>
        bool GetUnderlined();

        /// <summary>
        /// Gets native font description string.
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// Updates native font properties.
        /// </summary>
        void Update(FontParams prm);

        /// <summary>
        /// Indicates whether native font is equal to another native font.
        /// </summary>
        /// <returns></returns>
        bool Equals(Font font);

        public struct FontParams
        {
            public GenericFontFamily? GenericFamily;
            public string? FamilyName;
            public double Size;
            public FontStyle Style = FontStyle.Regular;
            public GraphicsUnit Unit = GraphicsUnit.Point;
            public byte GdiCharSet = 1;

            public FontParams()
            {
            }
        }
    }
}
