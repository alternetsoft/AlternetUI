using System.Runtime.InteropServices;

using SkiaSharp;

namespace Alternet.UI.WinForms
{
    public static class WinFormsUtils
    {
#if NET9_0_OR_GREATER
#else
        private static Alternet.UI.SystemColorModeType applicationSystemColorMode
            = SystemColorModeType.Classic;
        private static Alternet.UI.SystemColorModeType applicationColorMode
            = Alternet.UI.SystemColorModeType.Classic;
        private static bool isDarkModeEnabled = false;
#endif

        /// <summary>
        /// Determines whether the specified <see cref="Alternet.Drawing.Font"/>
        /// and <see cref="System.Drawing.Font"/>
        /// instances represent the same font.
        /// </summary>
        /// <remarks>The comparison checks the font name, size (in points),
        /// and style to determine
        /// equality.</remarks>
        /// <param name="uiFont">The <see cref="Alternet.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both fonts are <see langword="null"/>
        /// or if they represent the same font;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool Equals(Alternet.Drawing.Font? uiFont, System.Drawing.Font? winFont)
        {
            if (uiFont == null && winFont == null)
                return true;
            if (uiFont == null || winFont == null)
                return false;

            return uiFont.Equals(
                winFont.Name,
                (float)winFont.SizeInPoints,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Font"/> object to an
        /// <see cref="Alternet.Drawing.Font"/> object.
        /// </summary>
        /// <remarks>The conversion maps the font name, size (in points),
        /// and style from the <see cref="System.Drawing.Font"/> to the corresponding
        /// properties of the <see cref="Alternet.Drawing.Font"/>.</remarks>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> to convert.
        /// Can be <see langword="null"/>.</param>
        /// <returns>An <see cref="Alternet.Drawing.Font"/> object that represents
        /// the converted font, or <see langword="null"/>
        /// if <paramref name="winFont"/> is <see langword="null"/>.</returns>
        public static Alternet.Drawing.Font? ToAlternet(this System.Drawing.Font? winFont)
        {
            if (winFont is null)
                return null;
            return new Alternet.Drawing.Font(
                winFont.Name,
                (float)winFont.SizeInPoints,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }

        /// <summary>
        /// Ensures that the specified <see cref="Alternet.Drawing.Font"/> instance corresponds
        /// to the given <see cref="System.Drawing.Font"/> instance.
        /// </summary>
        /// <remarks>This method ensures that the <paramref name="uiFont"/> parameter
        /// represents the same font as <paramref name="winFont"/>. If <paramref name="uiFont"/> is
        /// already equivalent to <paramref name="winFont"/>, no changes are made.</remarks>
        /// <param name="uiFont">A reference to an <see cref="Alternet.Drawing.Font"/> instance.
        /// If <paramref name="winFont"/> is <see langword="null"/>,
        /// this will be set to <see langword="null"/>. Otherwise, it will be updated to match
        /// <paramref name="winFont"/> if they are not already equivalent.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to synchronize with.
        /// If <see langword="null"/>, <paramref name="uiFont"/>
        /// will be set to <see langword="null"/>.</param>
        public static void EnsureFont(ref Alternet.Drawing.Font? uiFont, System.Drawing.Font? winFont)
        {
            if (winFont == null)
            {
                uiFont = null;
                return;
            }

            if(uiFont is null)
            {
                uiFont = winFont.ToAlternet();
            }
            else
            {
                if (!Equals(uiFont, winFont))
                    uiFont = winFont.ToAlternet();
            }
        }

        /// <summary>
        /// Gets or sets the color mode for the application.
        /// </summary>
        /// <remarks>Use this property to retrieve or modify the application's color mode, which
        /// determines the visual appearance of the application.
        /// Setting this property updates the color mode
        /// globally for the application.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationColorMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return (Alternet.UI.SystemColorModeType)Application.ColorMode;
#else
                return applicationColorMode;
#endif
            }

            set
            {
#if NET9_0_OR_GREATER
                Application.SetColorMode((System.Windows.Forms.SystemColorMode)value);
#else
                applicationColorMode = value;
#endif
            }
        }

        /// <summary>
        /// Gets the current system color mode used by the application.
        /// </summary>
        /// <remarks>This property reflects the application's system color mode, which determines the
        /// color scheme (e.g., light or dark mode) used by the application.
        /// The value is derived from the underlying
        /// system settings.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationSystemColorMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return (Alternet.UI.SystemColorModeType)Application.SystemColorMode;
#else
                return applicationSystemColorMode;
#endif
            }

#if NET9_0_OR_GREATER
#else
            set
            {
                applicationSystemColorMode = value;
            }
#endif
        }

        /// <summary>
        /// Gets a value indicating whether the application is currently in dark mode.
        /// </summary>
        public static bool ApplicationIsDarkMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return Application.IsDarkModeEnabled;
#else
                return isDarkModeEnabled;
#endif
            }

#if NET9_0_OR_GREATER
#else
            set
            {
                isDarkModeEnabled = value;
            }
#endif
        }
    }
}
