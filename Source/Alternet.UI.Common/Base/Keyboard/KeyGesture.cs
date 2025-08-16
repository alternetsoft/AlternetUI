using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.UI.Localization;
using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// Contains key and key modifiers combination.
    /// </summary>
    [TypeConverter(typeof(KeyGestureConverter))]
    public class KeyGesture : InputGesture
    {
        /// <summary>
        /// Represents the delimiter character used to separate multiple gestures in a gesture string.
        /// </summary>
        /// <remarks>This constant is typically used when parsing or constructing strings that contain
        /// multiple gestures.</remarks>
        public const char MultipleGestureDelimiter = ';';

        private readonly ModifierKeys modifiers = ModifierKeys.None;
        private readonly Key key = Key.None;
        private readonly string displayString;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class
        /// with key value.
        /// </summary>
        /// <param name="key">Key definition.</param>
        public KeyGesture(Key key)
            : this(key, ModifierKeys.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class
        /// with key and key modifiers values.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        public KeyGesture(Key key, ModifierKeys modifiers)
            : this(key, modifiers, string.Empty, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class
        /// with key, key modifiers and display string values.
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
        /// Initializes a new instance of the <see cref="KeyGesture"/> class.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        /// <param name="validateGesture">If true, throws an exception
        /// if the key and modifier are not valid.</param>
        public KeyGesture(Key key, ModifierKeys modifiers, bool validateGesture)
            : this(key, modifiers, string.Empty, validateGesture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class.
        /// </summary>
        /// <param name="key">Key definition.</param>
        /// <param name="modifiers">Modifiers for the key.</param>
        /// <param name="displayString">Display string for the
        /// key and modifiers.</param>
        /// <param name="validateGesture">If true, throws an exception if
        /// the key and modifier are not valid.</param>
        public KeyGesture(
            Key key,
            ModifierKeys modifiers,
            string displayString,
            bool validateGesture)
        {
            if (DebugUtils.IsDebugDefined)
            {
                if (!ModifierKeysConverter.IsDefinedModifierKeys(modifiers))
                {
                    App.LogError(new InvalidEnumArgumentException(
                        "modifiers",
                        (int)modifiers,
                        typeof(ModifierKeys)));
                }

                if (!IsDefinedKey(key))
                {
                    App.LogError(new InvalidEnumArgumentException("key", (int)key, typeof(Key)));
                }

                if (validateGesture && !IsValid(key, modifiers))
                {
                    var exception = new NotSupportedException(
                        string.Format(ErrorMessages.Default.KeyGestureInvalid, modifiers, key));
                    App.LogError(exception);
                }
            }

            this.modifiers = modifiers;
            this.key = key;
            this.displayString = displayString ?? string.Empty;
        }

        /// <summary>
        /// Gets key modifiers associated with this <see cref="KeyGesture"/>
        /// (Alt, Control, Shift, etc.)
        /// </summary>
        public virtual ModifierKeys Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        /// <summary>
        /// Gets key associated with this <see cref="KeyGesture"/>.
        /// </summary>
        public virtual Key Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Gets display string for the end user.
        /// </summary>
        public virtual string DisplayString
        {
            get
            {
                return displayString;
            }
        }

        /// <summary>
        /// Implicit operator conversion from the <see cref="string"/> to
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
        /// Checks for valid enum, as any int can be casted to the enum.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns></returns>
        public static bool IsDefinedKey(Key key)
        {
            return key >= Key.None && key <= Key.Max;
        }

        /// <summary>
        /// Checks whether key and key modifiers combination is valid.
        /// </summary>
        public static bool IsValid(Key key, ModifierKeys modifiers)
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
                // condition to avoid conflicting with TextInput.
                if ((modifiers & (ModifierKeys.Control | ModifierKeys.Alt |
                    ModifierKeys.Windows)) != 0)
                {
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
        /// Gets whether <see cref="Key"/> and <see cref="Modifiers"/>
        /// are equal to the values specified in the parameters.
        /// </summary>
        public virtual bool HasKey(Key key, ModifierKeys modifiers)
        {
            if (Key == key && Modifiers == modifiers)
                return true;
            return false;
        }

        /// <summary>
        /// Checks whether <see cref="Key"/> and <see cref="Modifiers"/> combination is valid.
        /// </summary>
        public virtual bool IsValid()
        {
            var result = ModifierKeysConverter.IsDefinedModifierKeys(Modifiers)
                && IsDefinedKey(Key) && IsValid(Key, Modifiers);
            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return KeyGestureConverter.Default.ConvertToString(this);
            }
            catch (Exception)
            {
                return base.ToString();
            }
        }

        /// <summary>
        /// Returns a string that can be used to display the KeyGesture.  If the
        /// DisplayString was set by the constructor, it is returned.  Otherwise
        /// a suitable string is created from the Key and Modifiers, with any
        /// conversions being governed by the given CultureInfo.
        /// </summary>
        /// <param name="culture">the culture used when creating a string
        /// from Key and Modifiers</param>
        public virtual string? GetDisplayStringForCulture(CultureInfo culture)
        {
            // return the DisplayString, if it was set by the ctor
            if (!string.IsNullOrEmpty(displayString))
            {
                return displayString;
            }

            // otherwise use the type converter
            return (string?)KeyGestureConverter.Default.ConvertTo(
                null,
                culture,
                this,
                typeof(string));
        }

        /// <summary>
        /// Decodes the strings keyGestures and displayStrings, creating a sequence
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

            return KeyGestureConverter.Default.ConvertFromInvariantString(
                keyGestureToken) as KeyGesture;
        }
    }
}