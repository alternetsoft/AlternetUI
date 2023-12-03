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

        static AllPlatformDefaults()
        {
            if (Application.IsWindowsOS)
            {
                PlatformCurrent = PlatformWindows;
                InitWindows();
                return;
            }

            if (Application.IsLinuxOS)
            {
                PlatformCurrent = PlatformLinux;
                InitLinux();
                return;
            }

            if (Application.IsMacOS)
            {
                PlatformCurrent = PlatformMacOs;
                InitMacOs();
                return;
            }

            PlatformCurrent = PlatformAny;

            void InitLinux()
            {
                var platform = PlatformLinux;

                platform.AllowButtonHasBorder = false;
                platform.AllowButtonBackground = false;

                var minMargin = new Thickness(3);
                platform.Controls.RadioButton.MinMargin = minMargin;
                platform.Controls.Button.MinMargin = minMargin;
                platform.Controls.CheckBox.MinMargin = minMargin;
                platform.AdjustTextBoxesHeight = true;
                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;

                if (SystemSettings.AppearanceIsDark)
                {
                    platform.RichToolTipBackgroundColor = Color.FromArgb(39, 39, 39);
                    platform.RichToolTipForegroundColor = Color.White;
                    platform.RichToolTipTitleForegroundColor = Color.FromRgb(156, 220, 254);
                }
                else
                {
                    platform.RichToolTipBackgroundColor = Color.White;
                    platform.RichToolTipForegroundColor = Color.Black;
                    platform.RichToolTipTitleForegroundColor = Color.Navy;
                }
            }

            void InitWindows()
            {
                var platform = PlatformWindows;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;
                platform.AllowButtonHasBorder = false;
                platform.Controls.Button.MinMargin = 3;
            }

            void InitMacOs()
            {
                var platform = PlatformMacOs;

                platform.TextBoxUrlClickModifiers = ModifierKeys.Control;

                platform.AllowButtonHasBorder = false;
                platform.AllowButtonBackground = false;
                platform.AllowButtonForeground = false;

                platform.Controls.MultilineTextBox.HasBorderOnBlack = false;
                platform.Controls.ListBox.HasBorderOnBlack = false;
                platform.Controls.CheckListBox.HasBorderOnBlack = false;
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

        internal static object? GetPropValue(
            PlatformDefaults platform,
            ControlTypeId control,
            ControlDefaultsId prop)
        {
            object? result = platform.Controls.GetPropValue(control, prop);
            return result;
        }

        internal static Thickness GetAsThickness(
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