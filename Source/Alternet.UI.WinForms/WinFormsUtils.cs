namespace Alternet.UI.WinForms
{
    public static class WinFormsUtils
    {
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

        public static Alternet.UI.SystemColorModeType ApplicationSystemColorMode
        {
            get
            {
                return (Alternet.UI.SystemColorModeType)Application.SystemColorMode;
            }
        }

        public static bool ApplicationIsDarkMode
        {
            get
            {
                return Application.IsDarkModeEnabled;
            }
        }
    }
}
