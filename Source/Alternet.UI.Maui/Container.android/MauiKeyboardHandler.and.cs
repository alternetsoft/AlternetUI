using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Alternet.UI
{
#if ANDROID
    internal class MauiKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        static MauiKeyboardHandler()
        {
        }

        /// <inheritdoc/>
        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.MaxMaui;
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
#endif
}
