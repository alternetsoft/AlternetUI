using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties relater to the keyboard, mouse, touch and
    /// other input methods.
    /// </summary>
    public static class InputUtils
    {
        /// <summary>
        /// Combines two <see cref="KeyStates"/> values into one.
        /// This is useful, for example, when getting key modifiers (Alt, Control, Shift) state
        /// on platforms where left and right modifier keys have different codes.
        /// </summary>
        /// <param name="state1">First key state to combine.</param>
        /// <param name="state2">Second key state to combine.</param>
        /// <returns></returns>
        public static KeyStates Combine(KeyStates state1, KeyStates state2)
        {
            KeyStates result = KeyStates.None;

            if (state1.HasFlag(KeyStates.Down) || state2.HasFlag(KeyStates.Down))
                result |= KeyStates.Down;

            if (state1.HasFlag(KeyStates.Toggled) || state2.HasFlag(KeyStates.Toggled))
                result |= KeyStates.Toggled;

            return result;
        }

        /// <summary>
        /// Gets whether the specified key is digit.
        /// </summary>
        /// <param name="key">Key to test.</param>
        /// <returns></returns>
        /// <remarks>
        /// There is also <see cref="IsNumPadDigit"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigit(Key key)
        {
            return key >= Key.D0 && key <= Key.D9;
        }

        /// <summary>
        /// Gets whether the specified key is a NumPad digit.
        /// </summary>
        /// <param name="key">Key to test.</param>
        /// <returns></returns>
        /// <remarks>
        /// There is also <see cref="IsDigit"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumPadDigit(Key key)
        {
            return key >= Key.NumPad0 && key <= Key.NumPad9;
        }
    }
}
