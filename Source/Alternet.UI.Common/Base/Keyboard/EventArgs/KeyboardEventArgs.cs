using System;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for the keyboard event arguments.
    /// </summary>
    public class KeyboardEventArgs : HandledEventArgs
    {
        private object originalTarget;
        private object currentTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardEventArgs"/> class.
        /// </summary>
        public KeyboardEventArgs()
        {
            this.originalTarget = AssemblyUtils.Default;
            this.currentTarget = AssemblyUtils.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardEventArgs"/> class
        /// with the specified original target object.
        /// </summary>
        /// <param name="originalTarget">Original target object which received the event.</param>
        public KeyboardEventArgs(object originalTarget)
        {
            this.originalTarget = originalTarget;
            this.currentTarget = originalTarget;
        }

        /// <summary>
        /// Gets or sets current target control for the event.
        /// </summary>
        public virtual object CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }

        /// <summary>
        /// Gets or sets original target control for the event.
        /// </summary>
        public virtual object OriginalTarget
        {
            get
            {
                return originalTarget;
            }

            set
            {
                originalTarget = value;
            }
        }
    }
}