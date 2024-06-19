using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PlessKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        public KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }

        public bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.Max;
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
