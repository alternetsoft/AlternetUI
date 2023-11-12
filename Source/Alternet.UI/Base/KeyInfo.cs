using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains key information.
    /// </summary>
    public class KeyInfo
    {
        private static bool _registeredCustomKeyLabels = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyInfo"/> class.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="modifiers">Key modifier.</param>
        public KeyInfo(Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
            Key = key;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Gets or sets key value.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets key modifiers.
        /// </summary>
        public ModifierKeys Modifiers { get; set; }

        /// <summary>
        /// Registers custom labels for the <see cref="Key"/> enum.
        /// </summary>
        public static void RegisterCustomKeyLabels()
        {
            if (_registeredCustomKeyLabels)
                return;
            _registeredCustomKeyLabels = true;
            var choices = PropertyGrid.GetChoices<Key>();
            choices.SetLabelForValue(Key.D0, "0");
            choices.SetLabelForValue(Key.D1, "1");
            choices.SetLabelForValue(Key.D2, "2");
            choices.SetLabelForValue(Key.D3, "3");
            choices.SetLabelForValue(Key.D4, "4");
            choices.SetLabelForValue(Key.D5, "5");
            choices.SetLabelForValue(Key.D6, "6");
            choices.SetLabelForValue(Key.D7, "7");
            choices.SetLabelForValue(Key.D8, "8");
            choices.SetLabelForValue(Key.D9, "9");
            choices.SetLabelForValue(Key.Slash, "/");
            choices.SetLabelForValue(Key.Backslash, @"\");
        }

        /// <summary>
        /// Gets custom label of the key.
        /// </summary>
        /// <param name="key">Key.</param>
        public static string GetCustomKeyLabel(Key key)
        {
            RegisterCustomKeyLabels();
            var choices = PropertyGrid.GetChoices<Key>();
            return choices.GetLabelFromValue((int)key) ?? key.ToString();
        }

        /// <summary>
        /// Runs action if any of the keys is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="action">Action to run.</param>
        /// <param name="setHandled">Specifies whether to set event arguments Handled property.</param>
        /// <returns><c>true</c> if key is pressed; <c>false</c> otherwise.</returns>
        /// <param name="keys">Array of keys.</param>
        public static bool Run(KeyInfo[] keys, KeyEventArgs e, Action? action = null, bool setHandled = true)
        {
            foreach (var key in keys)
            {
                if (key.Run(e, action, setHandled))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current object.</returns>
        public override string ToString()
        {
            var keyText = GetCustomKeyLabel(Key);
            if(Modifiers != ModifierKeys.None)
            {
                var modifiersText = ModifierKeysConverter.ToString(Modifiers);
                keyText = $"{modifiersText}+{keyText}";
            }

            return keyText;
        }

        /// <summary>
        /// Checks <paramref name="e"/> event arguments on whether this key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public bool IsPressed(KeyEventArgs e) => e.Key == Key && e.Modifiers == Modifiers;

        /// <summary>
        /// Runs action if this key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="action">Action to run.</param>
        /// <param name="setHandled">Specifies whether to set event arguments Handled property.</param>
        /// <returns><c>true</c> if key is pressed; <c>false</c> otherwise.</returns>
        public bool Run(KeyEventArgs e, Action? action = null, bool setHandled = true)
        {
            var result = IsPressed(e);
            if (result)
            {
                action?.Invoke();
                if (setHandled)
                    e.Handled = true;
            }

            return result;
        }
    }
}
