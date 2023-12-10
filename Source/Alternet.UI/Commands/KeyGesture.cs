// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Description: The KeyGesture class is used by the developer to create
// Keyboard Input Bindings
// See spec at : http://avalon/coreUI/Specs/Commanding%20--%20design.htm
// KeyGesture class serves the purpose of Input Bindings for Keyboard Device.
// one will be passing the instance of this around with CommandLink
// to represent Accelerator Keys
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// Key and Modifier combination.
    /// Can be set on properties of KeyBinding.
    /// </summary>
    [TypeConverter(typeof(KeyGestureConverter))]
    [ValueSerializer(typeof(KeyGestureValueSerializer))]
    public class KeyGesture : InputGesture
    {
        private const char MultipleGestureDelimiter = ';';

        private static readonly TypeConverter KeyGestureConverter =
            new KeyGestureConverter();

        private readonly ModifierKeys fmodifiers = ModifierKeys.None;

        private readonly Key fkey = Key.None;

        private readonly string fdisplayString;

        /// <summary>
        /// Creates <see cref="KeyGesture"/> instance with key value.
        /// </summary>
        /// <param name="key">Key definition.</param>
        public KeyGesture(Key key)
            : this(key, ModifierKeys.None)
        {
        }

        /// <summary>
        /// Creates <see cref="KeyGesture"/> instance with key and key
        /// modifiers values.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        public KeyGesture(Key key, ModifierKeys modifiers)
            : this(key, modifiers, string.Empty, true)
        {
        }

        /// <summary>
        /// Creates <see cref="KeyGesture"/> instance with key, key
        /// modifiers and display string values.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        /// <param name="displayString">Display string for the key with
        /// modifiers.</param>
        public KeyGesture(Key key, ModifierKeys modifiers, string displayString)
            : this(key, modifiers, displayString, true)
        {
        }

        /// <summary>
        /// Internal constructor used by KeyBinding to avoid key and
        /// modifier validation
        /// This allows setting KeyBinding.Key and KeyBinding.Modifiers without
        /// regard to order.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        /// <param name="validateGesture">If true, throws an exception
        /// if the key and modifier are not valid</param>
        internal KeyGesture(Key key, ModifierKeys modifiers, bool validateGesture)
            : this(key, modifiers, string.Empty, validateGesture)
        {
        }

        /// <summary>
        /// Private constructor that does the real work.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        /// <param name="displayString">Display string for the
        /// key and modifiers </param>
        /// <param name="validateGesture">If true, throws an exception if
        /// the key and modifier are not valid</param>
        private KeyGesture(
            Key key,
            ModifierKeys modifiers,
            string displayString,
            bool validateGesture)
        {
            if (!ModifierKeysConverter.IsDefinedModifierKeys(modifiers))
            {
                throw new InvalidEnumArgumentException(
                    "modifiers",
                    (int)modifiers,
                    typeof(ModifierKeys));
            }

            if (!IsDefinedKey(key))
                throw new InvalidEnumArgumentException("key", (int)key, typeof(Key));
            if (validateGesture && !IsValid(key, modifiers))
            {
                throw new NotSupportedException(
                    SR.Get(SRID.KeyGesture_Invalid, modifiers, key));
            }

            fmodifiers = modifiers;
            fkey = key;
            fdisplayString = displayString ??
                throw new ArgumentNullException(nameof(displayString));
        }

        /// <summary>
        /// Gets key modifiers asociated with this <see cref="KeyGesture"/>
        /// (Alt, Control, Shift, etc.)
        /// </summary>
        public ModifierKeys Modifiers
        {
            get
            {
                return fmodifiers;
            }
        }

        /// <summary>
        /// Gets key asociated with this <see cref="KeyGesture"/>.
        /// </summary>
        public Key Key
        {
            get
            {
                return fkey;
            }
        }

        /// <summary>
        /// Gets display string for the end user.
        /// </summary>
        public string DisplayString
        {
            get
            {
                return fdisplayString;
            }
        }

        /// <summary>
        /// Implicit operator convertion from the <see cref="string"/> to
        /// the <see cref="KeyGesture"/>.
        /// </summary>
        /// <param name="s">String representation of the <see cref="KeyGesture"/>.
        /// </param>
        public static implicit operator KeyGesture(string s)
        {
            KeyGesture? result = UI.KeyGestureConverter.FromString(s);
            return result ?? throw new FormatException();
        }

        /// <summary>
        /// Returns a string that can be used to display the KeyGesture.  If the
        /// DisplayString was set by the constructor, it is returned.  Otherwise
        /// a suitable string is created from the Key and Modifiers, with any
        /// conversions being governed by the given CultureInfo.
        /// </summary>
        /// <param name="culture">the culture used when creating a string
        /// from Key and Modifiers</param>
        public string? GetDisplayStringForCulture(CultureInfo culture)
        {
            // return the DisplayString, if it was set by the ctor
            if (!string.IsNullOrEmpty(fdisplayString))
            {
                return fdisplayString;
            }

            // otherwise use the type converter
            return (string?)KeyGestureConverter.ConvertTo(
                null,
                culture,
                this,
                typeof(string));
        }

        /// <summary>
        /// Compares InputEventArgs with current Input
        /// </summary>
        /// <param name="targetElement">the element to receive the command</param>
        /// <param name="inputEventArgs">inputEventArgs to compare to</param>
        /// <returns>True - KeyGesture matches, false otherwise.
        /// </returns>
        public override bool Matches(
            object targetElement,
            InputEventArgs inputEventArgs)
        {
            if (inputEventArgs is KeyEventArgs keyEventArgs &&
                IsDefinedKey(keyEventArgs.Key))
            {
                return ((int)Key == (int)keyEventArgs.Key) &&
                    (this.Modifiers == Keyboard.Modifiers);
            }

            return false;
        }

        // Check for Valid enum, as any int can be casted to the enum.
        internal static bool IsDefinedKey(Key key)
        {
            return key >= Key.None && key <= Key.Menu;
        }

        /// <summary>
        /// Is Valid Keyboard input to process for commands
        /// </summary>
        internal static bool IsValid(Key key, ModifierKeys modifiers)
        {
            // Don't enforce any rules on the Function keys or on the number pad keys.
            if (!((key >= Key.F1 && key <= Key.F24) ||
                (key >= Key.NumPad0 && key <= Key.NumPadSlash)))
            {
                // We check whether Control/Alt/Windows key is down for modifiers.
                // We don't check
                // for shift at this time as Shift with any combination
                // is already covered in above check.
                // Shift alone as modifier case, we defer to the next
                // condition to avoid conflicing with TextInput.
                if ((modifiers & (ModifierKeys.Control | ModifierKeys.Alt |
                    ModifierKeys.Windows)) != 0)
                {
                    /*//switch(key)
                    //{
                    //    case Key.LeftCtrl:
                    //    case Key.RightCtrl:
                    //    case Key.LeftAlt:
                    //    case Key.RightAlt:
                    //    case Key.LWin:
                    //    case Key.RWin:
                    //        return false;
                    //    default:*/
                    return true;
                }
                else if ((key >= Key.D0 && key <= Key.D9) ||
                    (key >= Key.A && key <= Key.Z))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Decode the strings keyGestures and displayStrings, creating a sequence
        /// of KeyGestures.  Add each KeyGesture to the given InputGestureCollection.
        /// The two input strings typically come from a resource file.
        /// </summary>
        internal static void AddGesturesFromResourceStrings(
            string keyGestures,
            string displayStrings,
            ICollection<InputGesture> gestures)
        {
            while (!string.IsNullOrEmpty(keyGestures))
            {
                string keyGestureToken;
                string keyDisplayString;

                // break apart first gesture from the rest
                int index = keyGestures.IndexOf(MultipleGestureDelimiter);
                if (index >= 0)
                {
                    // multiple gestures exist
#pragma warning disable
                    keyGestureToken = keyGestures.Substring(0, index);
                    keyGestures = keyGestures.Substring(index + 1);
#pragma warning restore
                }
                else
                {
                    keyGestureToken = keyGestures;
                    keyGestures = string.Empty;
                }

                // similarly, break apart first display string from the rest
                index = displayStrings.IndexOf(MultipleGestureDelimiter);
                if (index >= 0)
                {
                    // multiple display strings exist
#pragma warning disable
                    keyDisplayString = displayStrings.Substring(0, index);
                    displayStrings = displayStrings.Substring(index + 1);
#pragma warning restore
                }
                else
                {
                    keyDisplayString = displayStrings;
                    displayStrings = string.Empty;
                }

                var keyGesture = CreateFromResourceStrings(keyGestureToken, keyDisplayString);

                if (keyGesture != null)
                {
                    gestures.Add(keyGesture);
                }
            }
        }

        internal static KeyGesture? CreateFromResourceStrings(
            string keyGestureToken,
            string keyDisplayString)
        {
            // combine the gesture and the display string, producing a string
            // that the type converter will recognize
            if (!string.IsNullOrEmpty(keyDisplayString))
            {
                keyGestureToken +=
                    UI.KeyGestureConverter.DisplayStringSeparator + keyDisplayString;
            }

            return KeyGestureConverter.ConvertFromInvariantString(
                keyGestureToken) as KeyGesture;
        }
    }
}