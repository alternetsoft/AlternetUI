using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known style themes for the scrollbar controls.
    /// </summary>
    public enum KnownScrollBarTheme
    {
        /// <summary>
        /// System color theme is used.
        /// </summary>
        System,

        /// <summary>
        /// 'Visual Studio Dark' theme is used if control has dark background
        /// and 'Visual Studio Light' theme is used if control has light background.
        /// </summary>
        VisualStudioAuto,

        /// <summary>
        /// 'Visual Studio Light' theme is used.
        /// </summary>
        VisualStudioLight,

        /// <summary>
        /// 'Visual Studio Dark' theme is used.
        /// </summary>
        VisualStudioDark,

        /// <summary>
        /// 'Windows Dark' theme is used if control has dark background
        /// and 'Windows Light' theme is used if control has light background.
        /// </summary>
        WindowsAuto,

        /// <summary>
        /// 'Windows Dark' theme is used.
        /// </summary>
        WindowsDark,

        /// <summary>
        /// 'Windows Light' theme is used.
        /// </summary>
        WindowsLight,

        /// <summary>
        /// 'Maui Dark' theme is used if control has dark background
        /// and 'Maui Light' theme is used if control has light background.
        /// </summary>
        MauiAuto,

        /// <summary>
        /// 'Maui Light' theme is used.
        /// </summary>
        MauiLight,

        /// <summary>
        /// 'Maui Dark' theme is used.
        /// </summary>
        MauiDark,
    }
}
