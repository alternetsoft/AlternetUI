using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IKeyboardHandler : IDisposable
    {
        /// <summary>
        /// Gets the current state of the specified key from the device
        /// from the underlying system
        /// </summary>
        /// <param name="key">
        /// Key to get the state of.
        /// </param>
        /// <returns>
        /// The state of the specified key.
        /// </returns>
        KeyStates GetKeyStatesFromSystem(Key key);

        bool HideKeyboard(Control? control);

        bool ShowKeyboard(Control? control);

        bool IsSoftKeyboardShowing(Control? control);

        bool IsValidKey(Key key);
    }
}
