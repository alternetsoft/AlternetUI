using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the appearance settings for an application.
    /// </summary>
    /// <remarks>
    /// This enumeration defines the visual theme options that
    /// an application can use. It includes
    /// options for using the system default appearance,
    /// a light theme, or a dark theme.
    /// </remarks>
    public enum ApplicationAppearance
    {
        /// <summary>
        /// Use system default appearance.
        /// </summary>
        System,

        /// <summary>
        /// Use light appearance.
        /// </summary>
        Light,

        /// <summary>
        /// Use dark appearance.
        /// </summary>
        Dark,
    }
}
