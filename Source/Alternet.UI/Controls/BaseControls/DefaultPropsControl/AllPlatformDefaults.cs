using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains platform related default property values.
    /// </summary>
    public static class AllPlatformDefaults
    {
        static AllPlatformDefaults()
        {
            var minCheckBoxMargin = new Thickness(3);

            PlatformLinux.Controls.RadioButton.MinMargin = minCheckBoxMargin;
            PlatformLinux.Controls.CheckBox.MinMargin = minCheckBoxMargin;
            PlatformLinux.AdjustTextBoxesHeight = true;

            PlatformLinux.RichToolTipBackgroundColor = Color.White;
            PlatformLinux.RichToolTipForegroundColor = Color.Black;
            PlatformLinux.RichToolTipTitleForegroundColor = Color.Navy;
        }

        /// <summary>
        /// Defines default control property values for all the platforms. This
        /// is used if default property value for the current platform
        /// is not specified.
        /// </summary>
        public static PlatformDefaults PlatformAny { get; } = new();

        /// <summary>
        /// Defines default control property values for Windows platfrom.
        /// </summary>
        public static PlatformDefaults PlatformWindows { get; } = new();

        /// <summary>
        /// Defines default control property values for Linux platfrom.
        /// </summary>
        public static PlatformDefaults PlatformLinux { get; } = new();

        /// <summary>
        /// Defines default control property values for macOs platfrom.
        /// </summary>
        public static PlatformDefaults PlatformMacOs { get; } = new();

        /// <summary>
        /// Defines default control property values for the current platfrom.
        /// </summary>
        /// <remarks>
        /// This property returns <see cref="PlatformWindows"/>,
        /// <see cref="PlatformLinux"/> or <see cref="PlatformMacOs"/> depending
        /// from the operating system on which application is executed.
        /// </remarks>
        public static PlatformDefaults PlatformCurrent { get; } =
            GetPlatformCurrent();

        /// <summary>
        /// Returns default property value for the control on the
        /// specific platform.
        /// </summary>
        /// <param name="control">Control which default property value
        /// is returned.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public static object? GetPropValue(ControlId control, ControlDefaultsId prop)
        {
            object? result = GetPropValue(PlatformCurrent, control, prop);
            result ??= GetPropValue(PlatformAny, control, prop);
            return result;
        }

        internal static object? GetPropValue(
            PlatformDefaults platform,
            ControlId control,
            ControlDefaultsId prop)
        {
            object? result = platform.Controls.GetPropValue(control, prop);
            return result;
        }

        internal static Thickness GetAsThickness(
            ControlId control,
            ControlDefaultsId prop)
        {
            Thickness? result =
                (Thickness?)AllPlatformDefaults.GetPropValue(control, prop);
            if (result == null)
                return Thickness.Empty;
            else
                return result.Value;
        }

        private static PlatformDefaults GetPlatformCurrent()
        {
            if (Application.IsWindowsOS)
                return PlatformWindows;
            if (Application.IsLinuxOS)
                return PlatformLinux;
            if (Application.IsMacOs)
                return PlatformMacOs;
            return PlatformAny;
        }
    }
}