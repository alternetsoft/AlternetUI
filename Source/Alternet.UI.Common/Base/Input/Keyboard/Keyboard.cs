using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the keyboard device.
    /// </summary>
    public static class Keyboard
    {
        private static IKeyboardHandler? handler;

        /// <summary>
        /// Gets or sets handler.
        /// </summary>
        public static IKeyboardHandler Handler
        {
            get => handler ??= App.Handler.CreateKeyboardHandler();

            set => handler = value;
        }

        /// <summary>
        /// Gets the set of modifier keys currently pressed (Control, Alt, Shift, etc.).
        /// </summary>
        public static ModifierKeys Modifiers
        {
            get
            {
                ModifierKeys modifiers = ModifierKeys.None;
                if (IsKeyDown(Key.Alt))
                {
                    modifiers |= ModifierKeys.Alt;
                }

                if (IsKeyDown(Key.Control))
                {
                    modifiers |= ModifierKeys.Control;
                }

                if (IsKeyDown(Key.Shift))
                {
                    modifiers |= ModifierKeys.Shift;
                }

                if (IsKeyDown(Key.Windows))
                {
                    modifiers |= ModifierKeys.Windows;
                }

                return modifiers;
            }
        }

        /// <summary>
        /// Gets the set of raw modifier keys currently pressed (Control, Alt, Shift, etc.).
        /// </summary>
        public static RawModifierKeys RawModifiers
        {
            get
            {
                RawModifierKeys modifiers = RawModifierKeys.None;
                if (IsKeyDown(Key.Alt))
                {
                    modifiers |= RawModifierKeys.Alt;
                }

                if (IsKeyDown(Key.Control))
                {
                    modifiers |= RawModifierKeys.Control;
                }

                if (IsKeyDown(Key.Shift))
                {
                    modifiers |= RawModifierKeys.Shift;
                }

                if (IsKeyDown(Key.Windows))
                {
                    modifiers |= RawModifierKeys.Windows;
                }

                if (IsKeyDown(Key.MacCommand))
                {
                    modifiers |= RawModifierKeys.MacCommand;
                }

                if (IsKeyDown(Key.MacOption))
                {
                    modifiers |= RawModifierKeys.MacOption;
                }

                if (IsKeyDown(Key.MacControl))
                {
                    modifiers |= RawModifierKeys.MacControl;
                }

                return modifiers;
            }
        }

        /// <summary>
        /// Returns whether or not the specified key is down.
        /// </summary>
        public static bool IsKeyDown(Key key)
        {
            var result = (GetKeyStates(key) & KeyStates.Down) == KeyStates.Down;
            return result;
        }

        /// <summary>
        /// Returns whether or not the specified key is up.
        /// </summary>
        public static bool IsKeyUp(Key key)
        {
            return !IsKeyDown(key);
        }

        /// <summary>
        /// Returns whether or not the specified key is toggled.
        /// </summary>
        public static bool IsKeyToggled(Key key)
        {
            return (GetKeyStates(key) & KeyStates.Toggled) == KeyStates.Toggled;
        }

        /// <summary>
        /// Returns the state of the specified key.
        /// </summary>
        public static KeyStates GetKeyStates(Key key)
        {
            if (!IsValidKey(key))
                return KeyStates.None;
            return Keyboard.Handler.GetKeyStatesFromSystem(key);
        }

        public static bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None/* && (int)key <= (int)Key.OemClear*/;
        }

        public static uint IsRepeatToRepeatCount(bool isRepeat)
        {
            if (isRepeat)
                return 1;
            else
                return 0;
        }
    }
}