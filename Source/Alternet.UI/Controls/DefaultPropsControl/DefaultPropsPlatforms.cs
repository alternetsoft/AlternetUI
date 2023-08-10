using System;

namespace Alternet.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultPropsPlatforms
    {
        static DefaultPropsPlatforms()
        {
            var minCheckBoxMargin = new Thickness(3);

            //PlatformLinux.Controls.RadioButton.MinMargin = minCheckBoxMargin;
            //PlatformLinux.Controls.CheckBox.MinMargin = minCheckBoxMargin;
        }

        /// <summary>
        /// Defines default control property values for all the platforms. This
        /// is used if default property value for the current platform
        /// is not specified.
        /// </summary>
        public static DefaultPropsPlatform PlatformAny { get; } = new();

        /// <summary>
        /// Defines default control property values for Windows platfrom.
        /// </summary>
        public static DefaultPropsPlatform PlatformWindows { get; } = new();

        /// <summary>
        /// Defines default control property values for Linux platfrom.
        /// </summary>
        public static DefaultPropsPlatform PlatformLinux { get; } = new();

        /// <summary>
        /// Defines default control property values for macOs platfrom.
        /// </summary>
        public static DefaultPropsPlatform PlatformMacOs { get; } = new();

        /// <summary>
        /// Defines default control property values for the current platfrom.
        /// </summary>
        /// <remarks>
        /// This property returns <see cref="PlatformWindows"/>,
        /// <see cref="PlatformLinux"/> or <see cref="PlatformMacOs"/> depending
        /// from the operating system on which application is executed.
        /// </remarks>
        public static DefaultPropsPlatform PlatformCurrent { get; } =
            GetPlatformCurrent();

        /// <summary>
        /// Returns default property value for the control on the
        /// specific platform.
        /// </summary>
        /// <param name="control">Control which default property value
        /// is returned.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public static object? GetPropValue(AllControls control, AllControlProps prop)
        {
            object? result = GetPropValue(PlatformCurrent, control, prop);
            result ??= GetPropValue(PlatformAny, control, prop);
            return result;
        }

        internal static object? GetPropValue(
            DefaultPropsPlatform platform,
            AllControls control,
            AllControlProps prop)
        {
            object? result = platform.Controls.GetPropValue(control, prop);
            return result;
        }

        internal static Thickness GetAsThickness(
            AllControls control,
            AllControlProps prop)
        {
            Thickness? result =
                (Thickness?)DefaultPropsPlatforms.GetPropValue(control, prop);
            if (result == null)
                return Thickness.Empty;
            else
                return result.Value;
        }

        private static DefaultPropsPlatform GetPlatformCurrent()
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