using System.Runtime.InteropServices;

using SkiaSharp;

namespace Alternet.UI.WinForms
{
    public static class WinFormsUtils
    {
        /// <summary>
        /// Gets or sets the color mode for the application.
        /// </summary>
        /// <remarks>Use this property to retrieve or modify the application's color mode, which
        /// determines the visual appearance of the application. Setting this property updates the color mode
        /// globally for the application.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationColorMode
        {
            get
            {
                return (Alternet.UI.SystemColorModeType)Application.ColorMode;
            }

            set
            {
                Application.SetColorMode((System.Windows.Forms.SystemColorMode)value);
            }
        }

        /// <summary>
        /// Gets the current system color mode used by the application.
        /// </summary>
        /// <remarks>This property reflects the application's system color mode, which determines the
        /// color scheme (e.g., light or dark mode) used by the application. The value is derived from the underlying
        /// system settings.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationSystemColorMode
        {
            get
            {
                return (Alternet.UI.SystemColorModeType)Application.SystemColorMode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the application is currently in dark mode.
        /// </summary>
        public static bool ApplicationIsDarkMode
        {
            get
            {
                return Application.IsDarkModeEnabled;
            }
        }
    }
}
