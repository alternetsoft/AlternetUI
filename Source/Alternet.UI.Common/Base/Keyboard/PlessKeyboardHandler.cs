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
        public bool? KeyboardPresent => null;

        /// <inheritdoc/>
        public IKeyboardVisibilityService? VisibilityService { get; set; }

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
        public bool HideKeyboard(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool ShowKeyboard(AbstractControl? control)
        {
            return false;
        }
    }
}
