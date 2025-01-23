using System;
using System.Runtime.CompilerServices;

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
        /// Gets whether hardware keyboard is present.
        /// </summary>
        public static bool? IsKeyboardPresent
        {
            get
            {
                return Handler.KeyboardPresent;
            }
        }

        /// <summary>
        /// Gets whether ALT key is currently pressed.
        /// </summary>
        public static bool IsAltPressed
        {
            get
            {
                return IsKeyDown(Key.Alt);
            }
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
        /// Returns whether or not the specified key state is down.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyDown(KeyStates keyStates)
        {
            return (keyStates & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Returns whether or not the specified key state is toggled.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyToggled(KeyStates keyStates)
        {
            return (keyStates & KeyStates.Toggled) == KeyStates.Toggled;
        }

        /// <summary>
        /// Returns whether or not the specified key is down.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyDown(Key key)
        {
            return IsKeyDown(GetKeyStates(key));
        }

        /// <summary>
        /// Returns whether or not the specified key is up.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyUp(Key key)
        {
            return !IsKeyDown(key);
        }

        /// <summary>
        /// Returns whether or not the specified key is toggled.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyToggled(Key key)
        {
            return IsKeyToggled(GetKeyStates(key));
        }

        /// <summary>
        /// Returns the state of the specified key.
        /// </summary>
        public static KeyStates GetKeyStates(Key key)
        {
            if (!IsValidKey(key))
                return KeyStates.None;
            try
            {
                return Keyboard.Handler.GetKeyStatesFromSystem(key);
            }
            catch(Exception e)
            {
                App.DebugLogError(e);
                return KeyStates.None;
            }
        }

        /// <summary>
        /// Gets whether specified key is valid for the current platform.
        /// </summary>
        /// <param name="key">key to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidKey(Key key)
        {
            return Handler.IsValidKey(key);
        }

        /// <summary>
        /// Converts <paramref name="isRepeat"/> boolean to repeat count.
        /// </summary>
        /// <param name="isRepeat">Whether key was repeated.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint IsRepeatToRepeatCount(bool isRepeat)
        {
            if (isRepeat)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Toggles on-screen keyboard visibility for the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToggleKeyboardVisibility(AbstractControl? control)
        {
            var visible = IsSoftKeyboardShowing(control);
            if (visible)
                return HideKeyboard(control);
            else
                return ShowKeyboard(control);
        }

        /// <inheritdoc cref="IKeyboardHandler.HideKeyboard"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideKeyboard(AbstractControl? control)
        {
            return Handler.HideKeyboard(control);
        }

        /// <inheritdoc cref="IKeyboardHandler.ShowKeyboard"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShowKeyboard(AbstractControl? control)
        {
            return Handler.ShowKeyboard(control);
        }

        /// <inheritdoc cref="IKeyboardHandler.IsSoftKeyboardShowing"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSoftKeyboardShowing(AbstractControl? control)
        {
            return Handler.IsSoftKeyboardShowing(control);
        }
    }
}