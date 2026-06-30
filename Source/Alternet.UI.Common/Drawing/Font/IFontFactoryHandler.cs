using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to create and manage fonts.
    /// </summary>
    public interface IFontFactoryHandler : IDisposable
    {
        /// <summary>
        /// Gets or sets default font encoding.
        /// </summary>
        /// <returns></returns>
        FontEncoding DefaultFontEncoding { get; set; }

        /// <summary>
        /// Gets default font size.
        /// </summary>
        /// <returns></returns>
        float GetDefaultFontSize();

        /// <summary>
        /// Gets default font name.
        /// </summary>
        /// <returns></returns>
        string GetDefaultFontName();

        /// <summary>
        /// Gets default monospace font name.
        /// </summary>
        /// <returns></returns>
        string GetDefaultMonoFontName();

        /// <summary>
        /// Gets whether or not to allow an empty font names.
        /// </summary>
        bool AllowNullFontName { get; }

        /// <summary>
        /// Sets default font.
        /// </summary>
        void SetDefaultFont(Font value);

        /// <summary>
        /// Creates native font handler.
        /// </summary>
        /// <returns></returns>
        IFontHandler CreateFontHandler();

        /// <summary>
        /// Creates default font.
        /// </summary>
        /// <returns></returns>
        Font CreateDefaultFont();

        /// <summary>
        /// Creates system font specified with <see cref="SystemSettingsFont"/>.
        /// </summary>
        /// <param name="systemFont">System font identifier.</param>
        /// <returns></returns>
        Font? CreateSystemFont(SystemSettingsFont systemFont);

        /// <summary>
        /// Returns a string array that contains all font families names
        /// currently available in the system.
        /// </summary>
        IEnumerable<string> GetFontFamiliesNames();

        /// <summary>
        /// Gets the name of the font family specified using <see cref="GenericFontFamily"/>.
        /// </summary>
        string GetFontFamilyName(GenericFontFamily genericFamily);
    }
}
