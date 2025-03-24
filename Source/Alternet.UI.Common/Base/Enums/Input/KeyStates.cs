using System;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates the states that keyboard keys can be in.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum KeyStates : byte
    {
        /// <summary>
        /// No state (same as up).
        /// </summary>
        None = 0,

        /// <summary>
        /// The key is down.
        /// </summary>
        Down = 1,

        /// <summary>
        /// The key is toggled on.
        /// </summary>
        Toggled = 2,
    }
}