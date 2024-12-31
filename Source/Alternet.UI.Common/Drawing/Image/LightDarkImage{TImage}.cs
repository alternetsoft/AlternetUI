using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains image for the light and dark color themes.
    /// </summary>
    public class LightDarkImage<TImage>
        where TImage : class
    {
        /// <summary>
        /// Gets or sets image for the light color theme.
        /// </summary>
        public TImage? Light;

        /// <summary>
        /// Gets or sets image for the dark color theme.
        /// </summary>
        public TImage? Dark;

        /// <summary>
        /// Gets image for the specified light/dark flag.
        /// </summary>
        /// <param name="isDark">The light/dark flag for which to get the image.</param>
        /// <returns></returns>
        public virtual TImage? GetImage(bool isDark)
        {
            if (isDark)
                return Dark;
            return Light;
        }

        /// <summary>
        /// Sets image for the specified light/dark flag.
        /// </summary>
        /// <param name="isDark">The light/dark flag for which to set the image.</param>
        /// <param name="value">New image value.</param>
        public virtual void SetImage(bool isDark, TImage? value)
        {
            if (isDark)
                Dark = value;
            else
                Light = value;
        }
    }
}
