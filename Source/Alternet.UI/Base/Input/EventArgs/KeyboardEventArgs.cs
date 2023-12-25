using System;

namespace Alternet.UI
{
    /// <summary>
    ///     The KeyboardEventArgs class provides access to the logical
    ///     pointer device for all derived event args.
    /// </summary>
    /// <ExternalAPI/> 
    public class KeyboardEventArgs : HandledEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the KeyboardEventArgs class.
        /// </summary>
        /// <param name="keyboard">
        ///     The logical keyboard device associated with this event.
        /// </param>
        public KeyboardEventArgs(KeyboardDevice keyboard)
        {
            KeyboardDevice = keyboard;
        }

        /// <summary>
        ///     Read-only access to the logical keyboard device associated with
        ///     this event.
        /// </summary>
        public KeyboardDevice KeyboardDevice { get; }
    }
}

