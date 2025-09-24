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
