using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract implementation of the <see cref="IKeyboardHandler"/> interface with
    /// mappings registration between platfrom keys and <see cref="Key"/>.
    /// </summary>
    /// <typeparam name="T">Type of the platform key.</typeparam>
    public abstract class PlatformKeyboardHandler<T> : PlatformKeyMapping<T>, IKeyboardHandler
        where T : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformKeyboardHandler{T}"/> class.
        /// </summary>
        public PlatformKeyboardHandler(T maxPlatformKey, Key maxKey, bool registerDefaults = true)
            : base(maxPlatformKey, maxKey)
        {
            if(registerDefaults)
                RegisterKeyMappings();
        }

        /// <inheritdoc/>
        public virtual bool? KeyboardPresent => null;

        /// <inheritdoc/>
        public abstract IKeyboardVisibilityService? VisibilityService { get; }

        /// <summary>
        /// Registers default key mappings between platform keys and <see cref="Key"/>.
        /// </summary>
        public virtual void RegisterKeyMappings()
        {
        }

        /// <inheritdoc/>
        public abstract KeyStates GetKeyStatesFromSystem(Key key);

        /// <inheritdoc/>
        public virtual bool HideKeyboard(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool ShowKeyboard(AbstractControl? control)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsValidKey(Key key)
        {
            return (int)key >= 0 && (int)key <= DestMaxValueAsInt;
        }
    }
}
