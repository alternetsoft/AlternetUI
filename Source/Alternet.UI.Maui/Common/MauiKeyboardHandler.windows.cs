using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.UI.Xaml;

using Windows.UI.ViewManagement;
using Windows.UI.ViewManagement.Core;

namespace Alternet.UI
{
    public class MauiKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        static MauiKeyboardHandler()
        {

        }

        public KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }

        public bool HideKeyboard(Control? control)
        {
            return HideKeyboard();
        }

        public bool IsSoftKeyboardShowing(Control? control)
        {
            return IsSoftKeyboardShowing();
        }

        public bool ShowKeyboard(Control? control)
        {
            return ShowKeyboard();
        }

        private static bool HideKeyboard()
        {
            if (TryGetInputPane(out CoreInputView? inputPane))
            {
                return inputPane.TryHide();
            }

            return false;
        }

        private static bool ShowKeyboard()
        {
            if (TryGetInputPane(out CoreInputView? inputPane))
            {
                return inputPane.TryShow();
            }

            return false;
        }

        private static bool IsSoftKeyboardShowing()
        {
            if (TryGetInputPane(out InputPane? inputPane))
            {
                return inputPane.Visible;
            }

            return false;
        }

        private static bool TryGetInputPane([NotNullWhen(true)] out CoreInputView? inputPane)
        {
            inputPane = CoreInputView.GetForCurrentView();
            return true;
        }

        private static bool TryGetInputPane([NotNullWhen(true)] out InputPane? inputPane)
        {
            nint mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            if (mainWindowHandle == IntPtr.Zero)
            {
                inputPane = null;
                return false;
            }

            inputPane = InputPaneInterop.GetForWindow(mainWindowHandle);
            return true;
        }
    }
}
