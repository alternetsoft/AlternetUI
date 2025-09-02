using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Contains key information.
    /// </summary>
    public partial class KeyInfo : BaseObject
    {
        /// <summary>
        /// Gets an empty <see cref="KeyInfo"/> object.
        /// </summary>
        public static readonly KeyInfo Empty = new();

        /// <summary>
        /// Represents the character used to separate modifiers and keys in a key combination.
        /// </summary>
        public static char ModifiersAndKeySeparator = '+';

        /// <summary>
        /// Represents the character used to separate modifier keys and the main key in display text.
        /// </summary>
        public static char ModifiersAndKeySeparatorForDisplay = '+';

        private static IndexedValues<Key, string>? customKeyLabels;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyInfo"/> class.
        /// </summary>
        public KeyInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyInfo"/> class.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="modifiers">Key modifier.</param>
        /// <param name="os"><see cref="OperatingSystems"/> in which this key combination
        /// is available.</param>
        public KeyInfo(
            Key key,
            ModifierKeys modifiers = ModifierKeys.None,
            OperatingSystems os = OperatingSystems.Any)
        {
            Key = key;
            Modifiers = modifiers;
            BackendOS = os;
        }

        /// <summary>
        /// Gets or sets <see cref="OperatingSystems"/> in which this key combination is available.
        /// </summary>
        public virtual OperatingSystems BackendOS { get; set; }

        /// <summary>
        /// Gets or sets key value.
        /// </summary>
        public virtual Key Key { get; set; }

        /// <summary>
        /// Gets or sets key modifiers.
        /// </summary>
        public virtual ModifierKeys Modifiers { get; set; }

        /// <summary>
        /// Runs action if any of the keys is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="action">Action to run.</param>
        /// <param name="setHandled">Specifies whether to set event arguments Handled property.</param>
        /// <returns><c>true</c> if key is pressed; <c>false</c> otherwise.</returns>
        /// <param name="keys">Array of keys.</param>
        public static bool Run(
            KeyInfo[] keys,
            KeyEventArgs e,
            Action? action = null,
            bool setHandled = true)
        {
            foreach (var key in keys)
            {
                if (key.Run(e, action, setHandled))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns filtered <see cref="KeyInfo"/> list. Result has only items which are
        /// registered for the current backend OS.
        /// </summary>
        /// <param name="keys">Keys.</param>
        public static KeyInfo[] FilterBackendOs(IEnumerable<KeyInfo>? keys)
        {
            if (keys is null)
                return Array.Empty<KeyInfo>();

            List<KeyInfo> result = new();

            foreach (var key in keys)
            {
                if (key.BackendOS.HasFlag(App.BackendOS))
                    result.Add(key);
            }

#pragma warning disable
            return result.ToArray();
#pragma warning restore
        }

        /// <summary>
        /// Registers custom labels for the <see cref="Key"/> enum.
        /// </summary>
        public static void RegisterCustomKeyLabels()
        {
            if (customKeyLabels is not null)
                return;
            SetCustomKeyLabel(Key.D0, "0");
            SetCustomKeyLabel(Key.D1, "1");
            SetCustomKeyLabel(Key.D2, "2");
            SetCustomKeyLabel(Key.D3, "3");
            SetCustomKeyLabel(Key.D4, "4");
            SetCustomKeyLabel(Key.D5, "5");
            SetCustomKeyLabel(Key.D6, "6");
            SetCustomKeyLabel(Key.D7, "7");
            SetCustomKeyLabel(Key.D8, "8");
            SetCustomKeyLabel(Key.D9, "9");
            SetCustomKeyLabel(Key.Slash, "/");
            SetCustomKeyLabel(Key.Backslash, @"\");
        }

        /// <summary>
        /// Sets custom key label.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="label">Key label.</param>
        public static void SetCustomKeyLabel(Key key, string label)
        {
            customKeyLabels ??= new();
            customKeyLabels[key] = label;
        }

        /// <summary>
        /// Gets custom label of the key.
        /// </summary>
        /// <param name="key">Key.</param>
        public static string GetCustomKeyLabel(Key key)
        {
            RegisterCustomKeyLabels();
            var result = customKeyLabels![key] ?? key.ToString();
            return result;
        }

        /// <summary>
        /// Converts the current key and modifier combination to its string representation.
        /// </summary>
        /// <param name="forUser">A value indicating whether the string representation
        /// should be formatted for user display. If <see langword="true"/>, the output
        /// is tailored for user-friendly display; otherwise, it is formatted for internal
        /// or programmatic use.</param>
        /// <returns>A string that represents the key and modifier combination.
        /// If modifiers are present, they are included in
        /// the output, separated by the appropriate separator based
        /// on the <paramref name="forUser"/> value.</returns>
        public virtual string ToString(bool forUser)
        {
            var keyText = GetCustomKeyLabel(Key);
            if (Modifiers != ModifierKeys.None)
            {
                var separator = forUser ? ModifiersAndKeySeparatorForDisplay : ModifiersAndKeySeparator;

                var modifiersText = ModifierKeysConverter.ToString(Modifiers, forUser);
                keyText = $"{modifiersText}{separator}{keyText}";
            }

            return keyText;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Checks <paramref name="e"/> event arguments on whether this key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual bool IsPressed(KeyEventArgs e) => e.Key == Key && e.ModifierKeys == Modifiers;

        /// <summary>
        /// Runs action if this key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="action">Action to run.</param>
        /// <param name="setHandled">Specifies whether to set event arguments Handled property.</param>
        /// <returns><c>true</c> if key is pressed; <c>false</c> otherwise.</returns>
        public virtual bool Run(KeyEventArgs e, Action? action = null, bool setHandled = true)
        {
            if (!BackendOS.HasFlag(App.BackendOS))
                return false;

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
