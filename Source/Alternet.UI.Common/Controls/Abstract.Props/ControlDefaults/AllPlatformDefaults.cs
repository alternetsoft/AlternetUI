using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains platform related default property values.
    /// </summary>
    public static class AllPlatformDefaults
    {
        /// <summary>
        /// Defines default control property values for the current platfrom.
        /// </summary>
        /// <remarks>
        /// This property returns <see cref="PlatformWindows"/>,
        /// <see cref="PlatformLinux"/> or <see cref="PlatformMacOs"/> depending
        /// from the operating system on which application is executed.
        /// </remarks>
        public static readonly PlatformDefaults PlatformCurrent;

        /// <summary>
        /// Defines default control property values for all the platforms. This
        /// is used if default property value for the current platform
        /// is not specified.
        /// </summary>
        public static readonly PlatformDefaults PlatformAny = new();

        /// <summary>
        /// Defines default control property values for Windows platfrom.
        /// </summary>
        public static readonly PlatformDefaults PlatformWindows = new();

        /// <summary>
        /// Defines default control property values for Linux platfrom.
        /// </summary>
        public static readonly PlatformDefaults PlatformLinux = new();

        /// <summary>
        /// Defines default control property values for macOs platfrom.
        /// </summary>
        public static readonly PlatformDefaults PlatformMacOs = new();

        internal const int DefaultIncFontSizeHighDpi = 1;

        internal const int DefaultIncFontSize = 1;

        static AllPlatformDefaults()
        {
            if (App.IsWindowsOS)
            {
                PlatformCurrent = PlatformWindows;
                InitWindows();
                return;
            }

            if (App.IsLinuxOS)
            {
                PlatformCurrent = PlatformLinux;
                InitLinux();
                return;
            }

            if (App.IsMacOS)
            {
                PlatformCurrent = PlatformMacOs;
                InitMacOs();
                return;
            }

            PlatformCurrent = PlatformAny;

            void InitCommon()
            {
                var platform = PlatformCurrent;
                platform.IncFontSizeHighDpi = DefaultIncFontSizeHighDpi;
                platform.IncFontSize = DefaultIncFontSize;
            }

            void InitLinuxOrMacOs()
            {
            }

            void InitLinux()
            {
                var platform = PlatformLinux;

                platform.AdjustTextBoxesHeight = true;
                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;
                InitLinuxOrMacOs();
                InitCommon();
            }

            void InitWindows()
            {
                var platform = PlatformWindows;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;
                InitCommon();
            }

            void InitMacOs()
            {
                var platform = PlatformMacOs;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;

                InitLinuxOrMacOs();
                InitCommon();
            }
        }
     }
}