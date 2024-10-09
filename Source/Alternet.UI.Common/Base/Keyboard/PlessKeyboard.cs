using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the keyboard.
    /// </summary>
    public static class PlessKeyboard
    {
        private static readonly BitArray keys = new((int)Key.Max);

        /// <summary>
        /// Gets state of the specified key which was previously saved in memory.
        /// </summary>
        /// <param name="key">Key to get state for.</param>
        /// <returns></returns>
        public static KeyStates GetKeyStatesFromMemory(Key key)
        {
            if(key > Key.Max)
                return KeyStates.None;
            var isPressed = keys.Get((int)key);
            return isPressed ? KeyStates.Down : KeyStates.None;
        }

        /// <summary>
        /// Updates state of the specified key in memory.
        /// </summary>
        /// <param name="e">Key information.</param>
        /// <param name="isDown">Whether key is down.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateKeyStateInMemory(KeyEventArgs e, bool isDown)
        {
            UpdateKeyStateInMemory(e.Key, isDown);
        }

        /// <summary>
        /// Updates state of the specified key in memory.
        /// </summary>
        /// <param name="key">Key to set state for.</param>
        /// <param name="isDown">Whether key is down.</param>
        public static void UpdateKeyStateInMemory(Key key, bool isDown)
        {
            if (key > Key.Max)
                return;
            keys.Set((int)key, isDown);
        }

        /// <summary>
        /// Resets all key states in memory, setting their state to <see cref="KeyStates.None"/>.
        /// </summary>
        public static void ResetKeysStatesInMemory()
        {
            keys.SetAll(false);
        }
    }
}
