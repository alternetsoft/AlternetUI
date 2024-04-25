using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        /// <summary>
        /// Updates native font properties.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <param name="prm">Font properties.</param>
        /// <returns></returns>
        public abstract void UpdateFont(object font, FontParams prm);

        /// <summary>
        /// Creates default native mono font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateDefaultMonoFont();

        /// <summary>
        /// Creates native font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateFont();

        /// <summary>
        /// Creates default native font.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateDefaultFont();

        /// <summary>
        /// Creates system font specified with <see cref="SystemSettingsFont"/>.
        /// </summary>
        /// <param name="systemFont">System font identifier.</param>
        /// <returns></returns>
        public abstract Font CreateSystemFont(SystemSettingsFont systemFont);

        /// <summary>
        /// Creates native font using other font properties.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateFont(object font);

        /// <summary>
        /// Returns a string array that contains all font families names
        /// currently available in the system.
        /// </summary>
        public abstract string[] GetFontFamiliesNames();

        /// <summary>
        /// Gets whether font family is installed on this computer.
        /// </summary>
        public abstract bool IsFontFamilyValid(string name);

        /// <summary>
        /// Gets the name of the font family specified using <see cref="GenericFontFamily"/>.
        /// </summary>
        public abstract string GetFontFamilyName(GenericFontFamily genericFamily);

        /// <summary>
        /// Gets default font encoding.
        /// </summary>
        /// <returns></returns>
        public abstract int GetDefaultFontEncoding();

        /// <summary>
        /// Sets default font encoding.
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetDefaultFontEncoding(int value);

        /// <summary>
        /// Gets native font name.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract string GetFontName(object font);

        /// <summary>
        /// Gets native font encoding.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract int GetFontEncoding(object font);

        /// <summary>
        /// Gets native font size in pixels.
        /// </summary>
        public abstract SizeI GetFontSizeInPixels(object font);

        /// <summary>
        /// Gets whether native font is using size in pixels.
        /// </summary>
        public abstract bool GetFontIsUsingSizeInPixels(object font);

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public abstract int GetFontNumericWeight(object font);

        /// <summary>
        /// Gets whether native font is a fixed width (or monospaced) font.
        /// </summary>
        public abstract bool GetFontIsFixedWidth(object font);

        /// <summary>
        /// Gets native font weight.
        /// </summary>
        /// <returns></returns>
        public abstract FontWeight GetFontWeight(object font);

        /// <summary>
        /// Gets style information for the native font.
        /// </summary>
        /// <param name="font">Native font instance.</param>
        /// <returns></returns>
        public abstract FontStyle GetFontStyle(object font);

        /// <summary>
        /// Gets a value that indicates whether native font is strikethrough.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetFontStrikethrough(object font);

        /// <summary>
        /// Gets a value that indicates whether native font is underlined.
        /// </summary>
        /// <returns></returns>
        public abstract bool GetFontUnderlined(object font);

        /// <summary>
        /// Gets the em-size, in points, of the native font.
        /// </summary>
        /// <returns></returns>
        public abstract double GetFontSizeInPoints(object font);

        /// <summary>
        /// Gets native font description string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetFontInfoDesc(object font);

        /// <summary>
        /// Indicates whether native font is equal to another native font.
        /// </summary>
        /// <returns></returns>
        public abstract bool FontEquals(object font1, object font2);

        /// <summary>
        /// Returns a string that represents native font.
        /// </summary>
        /// <returns></returns>
        public abstract string FontToString(object font);

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
