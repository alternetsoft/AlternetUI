using System;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for the keyboard event arguments.
    /// </summary>
    public class KeyboardEventArgs : HandledEventArgs
    {
        private readonly object originalTarget;
        private object currentTarget;

        public KeyboardEventArgs()
        {
            this.originalTarget = AssemblyUtils.Default;
            this.currentTarget = AssemblyUtils.Default;
        }

        /// <summary>
        ///     Initializes a new instance of the KeyboardEventArgs class.
        /// </summary>
        public KeyboardEventArgs(object originalTarget)
        {
            this.originalTarget = originalTarget;
            this.currentTarget = originalTarget;
        }

        /// <summary>
        /// Gets current target control for the event.
        /// </summary>
        public object CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }

        /// <summary>
        /// Gets original target control for the event.
        /// </summary>
        public object OriginalTarget => originalTarget;
    }
}