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
            return false;
        }

        public bool IsSoftKeyboardShowing(Control? control)
        {
            return false;
        }

        public bool ShowKeyboard(Control? control)
        {
            return false;
        }
    }
}
