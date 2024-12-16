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
                var platform = PlatformCurrent;
            }

            void InitLinux()
            {
                var platform = PlatformLinux;

                var minMargin = new Thickness(3);
                platform.Controls.RadioButton.MinMargin = minMargin;
                platform.Controls.Button.MinMargin = minMargin;
                platform.Controls.CheckBox.MinMargin = minMargin;
                platform.AdjustTextBoxesHeight = true;
                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;
                InitLinuxOrMacOs();
                InitCommon();
            }

            void InitWindows()
            {
                var platform = PlatformWindows;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;
                platform.Controls.Button.MinMargin = 3;
                InitCommon();
            }

            void InitMacOs()
            {
                var platform = PlatformMacOs;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;

                platform.Controls.MultilineTextBox.HasBorderOnBlack = false;
                platform.Controls.ListBox.HasBorderOnBlack = false;
                platform.Controls.CheckListBox.HasBorderOnBlack = false;
                InitLinuxOrMacOs();
                InitCommon();
            }
        }

        /// <summary>
        /// Returns default property value for the control on the
        /// specific platform.
        /// </summary>
        /// <param name="control">Control which default property value
        /// is returned.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public static object? GetPropValue(ControlTypeId control, ControlDefaultsId prop)
        {
            object? result = GetPropValue(PlatformCurrent, control, prop);
            result ??= GetPropValue(PlatformAny, control, prop);
            return result;
        }

        /// <summary>
        /// Gets "HasBorder" property override specific to platform and color theme.
        /// </summary>
        /// <param name="controlKind">Control kind.</param>
        /// <returns></returns>
        public static bool? GetHasBorderOverride(ControlTypeId controlKind)
        {
            bool? result;
            if (SystemSettings.IsUsingDarkBackground)
                result = AllPlatformDefaults.PlatformCurrent.Controls[controlKind].HasBorderOnBlack;
            else
                result = AllPlatformDefaults.PlatformCurrent.Controls[controlKind].HasBorderOnWhite;
            return result;
        }

        /// <summary>
        /// Returns default property value for the control on the
        /// specific platform.
        /// </summary>
        /// <param name="control">Control which default property value
        /// is returned.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        /// <param name="platform">Platform.</param>
        public static object? GetPropValue(
            PlatformDefaults platform,
            ControlTypeId control,
            ControlDefaultsId prop)
        {
            object? result = platform.Controls.GetPropValue(control, prop);
            return result;
        }

        /// <summary>
        /// Returns default property value as <see cref="Thickness"/> for the control on the
        /// specific platform.
        /// </summary>
        /// <param name="control">Control which default property value
        /// is returned.</param>
        /// <param name="prop">Property identifier.</param>
        /// <returns></returns>
        public static Thickness GetAsThickness(
            ControlTypeId control,
            ControlDefaultsId prop)
        {
            Thickness? result =
                (Thickness?)AllPlatformDefaults.GetPropValue(control, prop);
            if (result == null)
                return Thickness.Empty;
            else
                return result.Value;
        }
     }
}