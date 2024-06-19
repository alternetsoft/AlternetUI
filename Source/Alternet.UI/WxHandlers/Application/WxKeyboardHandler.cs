using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.MaxWxWidgets;
        }

        public virtual KeyStates GetKeyStatesFromSystem(Key key)
        {
            return WxApplicationHandler.NativeKeyboard.GetKeyState(key);
        }

        public virtual bool HideKeyboard(Control? control)
        {
            return false;
        }

        public virtual bool IsSoftKeyboardShowing(Control? control)
        {
            return false;
        }

        public virtual bool ShowKeyboard(Control? control)
        {
            return false;
        }
    }
}
