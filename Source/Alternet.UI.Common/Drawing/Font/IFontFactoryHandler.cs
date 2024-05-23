using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IFontFactoryHandler
    {
        /// <summary>
        /// Gets or sets default font encoding.
        /// </summary>
        /// <returns></returns>
        FontEncoding DefaultFontEncoding { get; set; }

        /// <summary>
        /// Sets default font.
        /// </summary>
        void SetDefaultFont(Font value);

        /// <summary>
        /// Creates default native mono font.
        /// </summary>
        /// <returns></returns>
        IFontHandler CreateDefaultMonoFont();

        /// <summary>
        /// Creates native font.
        /// </summary>
        /// <returns></returns>
        IFontHandler CreateFont();

        /// <summary>
        /// Creates default native font.
        /// </summary>
        /// <returns></returns>
        IFontHandler CreateDefaultFont();

        /// <summary>
        /// Creates system font specified with <see cref="SystemSettingsFont"/>.
        /// </summary>
        /// <param name="systemFont">System font identifier.</param>
        /// <returns></returns>
        Font CreateSystemFont(SystemSettingsFont systemFont);

        /// <summary>
        /// Creates native font using other font properties.
        /// </summary>
        /// <returns></returns>
        IFontHandler CreateFont(Font font);

        /// <summary>
        /// Returns a string array that contains all font families names
        /// currently available in the system.
        /// </summary>
        string[] GetFontFamiliesNames();

        /// <summary>
        /// Gets whether font family is installed on this computer.
        /// </summary>
        bool IsFontFamilyValid(string name);

        /// <summary>
        /// Gets the name of the font family specified using <see cref="GenericFontFamily"/>.
        /// </summary>
        string GetFontFamilyName(GenericFontFamily genericFamily);
    }
}
