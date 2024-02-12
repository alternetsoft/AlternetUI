using System;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for the keyboard event arguments.
    /// </summary>
    public class KeyboardEventArgs : HandledEventArgs
    {
        private readonly Control originalTarget;
        private Control currentTarget;

        /// <summary>
        ///     Initializes a new instance of the KeyboardEventArgs class.
        /// </summary>
        internal KeyboardEventArgs(Control originalTarget)
        {
            this.originalTarget = originalTarget;
            this.currentTarget = originalTarget;
            KeyboardDevice = Keyboard.PrimaryDevice;
        }

        /// <summary>
        /// Gets current target control for the event.
        /// </summary>
        public Control CurrentTarget
        {
            get => currentTarget;
            internal set => currentTarget = value;
        }

        /// <summary>
        /// Gets original target control for the event.
        /// </summary>
        public Control OriginalTarget => originalTarget;

        /// <summary>
        /// Gets logical keyboard device associated with this event.
        /// </summary>
        public KeyboardDevice KeyboardDevice { get; }
    }
}