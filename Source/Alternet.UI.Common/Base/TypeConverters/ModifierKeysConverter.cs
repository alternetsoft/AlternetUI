using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string and the
    /// <see cref="ModifierKeys"/>.
    /// </summary>
    public class ModifierKeysConverter : BaseTypeConverter
    {
        /// <summary>
        /// Gets or sets default <see cref="ModifierKeysConverter"/> implementation.
        /// </summary>
        public static TypeConverter Default = new ModifierKeysConverter();

        /// <summary>
        /// Represents the delimiter character used to separate modifiers in a string.
        /// </summary>
        /// <remarks>This field is commonly used in scenarios where multiple modifiers need to be
        /// concatenated and later parsed. The default value is the '+' character.</remarks>
        public static char ModifierDelimiter = '+';

        /// <summary>
        /// Represents the delimiter character used to separate modifiers in display text.
        /// </summary>
        public static char DisplayTextModifierDelimiter = '+';

        private static readonly ModifierKeys ModifierKeysFlag =
            ModifierKeys.Windows | ModifierKeys.Shift |
            ModifierKeys.Alt | ModifierKeys.Control;

        private static string? modifierDisplayTextControl;
        private static string? modifierDisplayTextShift;
        private static string? modifierDisplayTextAlt;
        private static string? modifierDisplayTextWindows;

        /// <summary>
        /// Gets or sets the display text used to represent the control key modifier.
        /// </summary>
        /// <remarks>This property allows customization of the text used to represent
        /// the control key modifier. If not explicitly set, the default value is determined
        /// based on the operating system.</remarks>
        public static string ModifierDisplayTextControl
        {
            get
            {
                if (modifierDisplayTextControl != null)
                    return modifierDisplayTextControl;

                if (App.IsMacOS)
                    return StringUtils.MacCommandKeyTitle;
                else
                    return "Ctrl";
            }

            set
            {
                modifierDisplayTextControl = value;
            }
        }

        /// <summary>
        /// Gets or sets the display text used to represent the "Shift" modifier key.
        /// </summary>
        public static string ModifierDisplayTextShift
        {
            get
            {
                return modifierDisplayTextShift ?? "Shift";
            }

            set
            {
                modifierDisplayTextShift = value;
            }
        }

        /// <summary>
        /// Gets or sets the alternative display text for the "Alt" modifier key.
        /// </summary>
        public static string ModifierDisplayTextAlt
        {
            get
            {
                return modifierDisplayTextAlt ?? "Alt";
            }

            set
            {
                modifierDisplayTextAlt = value;
            }
        }

        /// <summary>
        /// Gets or sets the display text for the Windows key modifier.
        /// </summary>
        public static string ModifierDisplayTextWindows
        {
            get
            {
                return modifierDisplayTextWindows ?? "Windows";
            }

            set
            {
                modifierDisplayTextWindows = value;
            }
        }

        /// <summary>
        /// Check for valid enum, as any int can be casted to the enum.
        /// </summary>
        public static bool IsDefinedModifierKeys(ModifierKeys modifierKeys)
        {
            return modifierKeys == ModifierKeys.None ||
                (((int)modifierKeys & ~((int)ModifierKeysFlag)) == 0);
        }

        /// <summary>
        /// Parses string representation of the key modifiers.
        /// </summary>
        /// <param name="source">String value to parse.</param>
        /// <returns>Enumeration with key modifiers.</returns>
        public static ModifierKeys FromString(string source)
        {
            string modifiersToken = ((string)source).Trim();
            ModifierKeys modifiers =
                GetModifierKeys(modifiersToken, CultureInfo.InvariantCulture);
            return modifiers;
        }

        /// <summary>
        /// Converts <see cref="ModifierKeys"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <param name="forUser">Result string is for user or not.</param>
        /// <returns>A <see cref="string"/> representation
        /// of <see cref="ModifierKeys"/> value.</returns>
        public static string ToString(ModifierKeys modifiers, bool forUser = false)
        {
            string strModifiers = string.Empty;

            char delimiter = forUser ?
                DisplayTextModifierDelimiter : ModifierDelimiter;

            Add(ModifierKeys.Control);
            Add(ModifierKeys.Alt);
            Add(ModifierKeys.Windows);
            Add(ModifierKeys.Shift);

            return strModifiers;

            void Add(ModifierKeys key)
            {
                if ((modifiers & key) == key)
                {
                    if (strModifiers.Length > 0)
                        strModifiers += delimiter;

                    strModifiers += MatchModifiers(key, forUser);
                }
            }
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (context?.Instance is ModifierKeys keys)
                {
                    return IsDefinedModifierKeys(keys);
                }
                else
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override object ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object source)
        {
            if (source is string v)
                return FromString(v);
            throw GetConvertFromException(source);
        }

        /// <inheritdoc/>
        public override object ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (value is null)
                return string.Empty;

            if (destinationType == typeof(string))
            {
                ModifierKeys modifiers = (ModifierKeys)value;

                if (!IsDefinedModifierKeys(modifiers))
                {
                    throw new InvalidEnumArgumentException(
                        nameof(value), (int)modifiers, typeof(ModifierKeys));
                }
                else
                    return ToString(modifiers);
            }

            throw GetConvertToException(value, destinationType);
        }

        internal static string MatchModifiers(ModifierKeys modifierKeys, bool forUser = false)
        {
            string modifiers = string.Empty;
            switch (modifierKeys)
            {
                case ModifierKeys.Control:
                    modifiers = ModifierDisplayTextControl;
                    break;
                case ModifierKeys.Shift:
                    modifiers = ModifierDisplayTextShift;
                    break;
                case ModifierKeys.Alt:
                    modifiers = ModifierDisplayTextAlt;
                    break;
                case ModifierKeys.Windows:
                    modifiers = ModifierDisplayTextWindows;
                    break;
            }

            return modifiers;
        }

        private static ModifierKeys GetModifierKeys(
            string modifiersToken,
            CultureInfo culture)
        {
            ModifierKeys modifiers = ModifierKeys.None;
            if (modifiersToken.Length != 0)
            {
                int offset;
                do
                {
                    offset = modifiersToken.IndexOf(ModifierDelimiter);
                    string token = (offset < 0)
                        ? modifiersToken : modifiersToken.Substring(0, offset);
                    token = token.Trim();
                    token = token.ToUpper(culture);

                    if (token == string.Empty)
                        break;

                    switch (token)
                    {
                        case "CONTROL":
                        case "CTRL":
                            modifiers |= ModifierKeys.Control;
                            break;

                        case "SHIFT":
                            modifiers |= ModifierKeys.Shift;
                            break;

                        case "ALT":
                            modifiers |= ModifierKeys.Alt;
                            break;

                        case "WINDOWS":
                        case "WIN":
                            modifiers |= ModifierKeys.Windows;
                            break;

                        default:
                            throw new NotSupportedException(
                                string.Format("Unsupported modifier {0}.", token));
                    }

                    modifiersToken = modifiersToken.Substring(offset + 1);
                }
                while (offset != -1);
            }

            return modifiers;
        }
    }
}
