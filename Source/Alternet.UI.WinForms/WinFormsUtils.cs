namespace Alternet.UI.WinForms
{
    public static class WinFormsUtils
    {
        public static Alternet.UI.SystemColorMode ApplicationColorMode
        {
            get
            {
                return (Alternet.UI.SystemColorMode)Application.ColorMode;
            }

            set
            {
                Application.SetColorMode((System.Windows.Forms.SystemColorMode)value);
            }
        }

        public static Alternet.UI.SystemColorMode ApplicationSystemColorMode
        {
            get
            {
                return (Alternet.UI.SystemColorMode)Application.SystemColorMode;
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
