using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IKeyboardHandler"/> provider.
    /// </summary>
    public class PlessKeyboardHandler : DisposableObject, IKeyboardHandler
    {
        /// <inheritdoc/>
        public KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates.None;
        }

        /// <inheritdoc/>
        public bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None && (int)key <= (int)Key.Max;
        }

        /// <inheritdoc/>
        public bool HideKeyboard(Control? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool IsSoftKeyboardShowing(Control? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool ShowKeyboard(Control? control)
        {
            return false;
        }
    }
}
