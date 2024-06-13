using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the keyboard device.
    /// </summary>
    public static class Keyboard
    {
        private static KeyboardDevice keyboardDevice = KeyboardDevice.Default;
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
        ///     The primary keyboard device.
        /// </summary>
        public static KeyboardDevice PrimaryDevice
        {
            get
            {
                return keyboardDevice;
            }

            set
            {
                keyboardDevice = value;
            }
        }

        /// <summary>
        ///     The set of modifier keys currently pressed.
        /// </summary>
        public static ModifierKeys Modifiers
        {
            get
            {
                return Keyboard.PrimaryDevice.Modifiers;
            }
        }

        /// <summary>
        ///     The set of raw modifier keys currently pressed.
        /// </summary>
        public static RawModifierKeys RawModifiers
        {
            get
            {
                return Keyboard.PrimaryDevice.RawModifiers;
            }
        }

        /// <summary>
        ///     Returns whether or not the specified key is down.
        /// </summary>
        public static bool IsKeyDown(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyDown(key);
        }

        /// <summary>
        ///     Returns whether or not the specified key is up.
        /// </summary>
        public static bool IsKeyUp(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyUp(key);
        }

        /// <summary>
        ///     Returns whether or not the specified key is toggled.
        /// </summary>
        public static bool IsKeyToggled(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyToggled(key);
        }

        /// <summary>
        ///     Returns the state of the specified key.
        /// </summary>
        public static KeyStates GetKeyStates(Key key)
        {
            return Keyboard.PrimaryDevice.GetKeyStates(key);
        }

        public static void ReportKeyDown(Key key, bool isRepeat, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, isRepeat, Keyboard.PrimaryDevice);
            control.BubbleKeyDown(eventArgs);
            handled = eventArgs.Handled;
        }

        public static void ReportKeyUp(Key key, bool isRepeat, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyEventArgs(control, key, isRepeat, Keyboard.PrimaryDevice);
            control.BubbleKeyUp(eventArgs);
            handled = eventArgs.Handled;
        }

        public static void ReportTextInput(char keyChar, out bool handled)
        {
            var control = Control.GetFocusedControl();
            if (control is null)
            {
                handled = false;
                return;
            }

            var eventArgs = new KeyPressEventArgs(control, keyChar, Keyboard.PrimaryDevice);
            control.BubbleKeyPress(eventArgs);
            handled = eventArgs.Handled;
        }

        // Check for Valid enum, as any int can be casted to the enum.
        internal static bool IsValidKey(Key key)
        {
            return (int)key >= (int)Key.None/* && (int)key <= (int)Key.OemClear*/;
        }
    }
}