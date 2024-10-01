using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to access keyboard.
    /// </summary>
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

        /// <summary>
        /// Hide on-screen keyboard for the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        bool HideKeyboard(Control? control);

        /// <summary>
        /// Show on-screen keyboard for the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        bool ShowKeyboard(Control? control);

        /// <summary>
        /// Gets whether on-screen keyboard is shown for the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        bool IsSoftKeyboardShowing(Control? control);

        /// <summary>
        /// Checks whether key is valid for the current platform.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns></returns>
        bool IsValidKey(Key key);
    }
}
