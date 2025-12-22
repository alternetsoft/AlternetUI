using System;
using System.Collections.Generic;
using System.Text;

#if ANDROID
using Android.App;
using Android.Content.Res;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Android.Views.InputMethods;
#endif

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for keyboard detection and management.
    /// </summary>
    public static class MauiKeyboardUtils
    {
        /// <summary>
        /// Override value for testing or other purposes. If set, this value will be returned by
        /// <see cref="IsHardwareKeyboardConnected"/> instead of checking the actual hardware state.
        /// </summary>
        public static bool? IsHardwareKeyboardConnectedOverride;

        /// <summary>
        /// Determines whether a hardware keyboard is currently connected to the device.
        /// </summary>
        /// <remarks>This method checks the current device configuration to detect the presence of a
        /// physical (hardware) keyboard. On devices without a hardware keyboard, or if the configuration cannot be
        /// determined, the method returns false.</remarks>
        /// <returns>true if a hardware keyboard is connected; otherwise, false.</returns>
        public static bool IsHardwareKeyboardConnected()
        {
            return IsHardwareKeyboardConnectedOverride ?? IsHardwareKeyboardConnectedInternal();
        }

#if WINDOWS
        private static bool IsHardwareKeyboardConnectedInternal()
        {
            return !Alternet.UI.MauiWindowsUtils.IsTabletMode();
        }
#endif

#if MACCATALYST
        private static bool IsHardwareKeyboardConnectedInternal()
        {
            return true;
        }
#endif

#if ANDROID
        private static bool IsHardwareKeyboardConnectedInternal()
        {
            Configuration? config = Android.App.Application.Context.Resources?.Configuration;

            if (config == null)
                return false;

            return config.Keyboard == KeyboardType.Qwerty;
        }
#endif

#if IOS
        private static bool IsHardwareKeyboardConnectedInternal()
        {
            // Not reliably possible on iOS
            return false;
        }
#endif

    }
}
