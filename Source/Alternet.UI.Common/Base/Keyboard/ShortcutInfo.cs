using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains information about the shortcut.
    /// </summary>
    public partial class ShortcutInfo : BaseObject
    {
        private KeyInfo[]? keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutInfo"/> class.
        /// </summary>
        public ShortcutInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutInfo"/> class.
        /// </summary>
        /// <param name="keys">Default value of the <see cref="Keys"/> property.</param>
        public ShortcutInfo(Keys keys)
        {
            Keys = keys;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutInfo"/> class.
        /// </summary>
        /// <param name="keyGesture">Default value of the <see cref="KeyGesture"/> property.</param>
        public ShortcutInfo(KeyGesture? keyGesture)
        {
            KeyGesture = keyGesture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutInfo"/> class.
        /// </summary>
        /// <param name="keyInfo">Default value of the <see cref="KeyInfo"/> property.</param>
        public ShortcutInfo(KeyInfo[]? keyInfo)
        {
            KeyInfo = keyInfo;
        }

        /// <summary>
        /// Gets or sets the associated shortcut keys.
        /// </summary>
        /// <returns>
        /// One of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [Browsable(false)]
        public virtual Keys Keys
        {
            get
            {
                if (KeyGesture is null)
                    return Keys.None;
                var result = KeyGesture.Key.ToKeys(KeyGesture.Modifiers);
                return result;
            }

            set
            {
                var key = value.ToKey();
                var modifiers = value.ToModifiers();
                KeyGesture = new(key, modifiers);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyGesture? KeyGesture
        {
            get
            {
                if (keys is null || keys.Length == 0)
                    return null;
                return new(keys[0].Key, keys[0].Modifiers);
            }

            set
            {
                if (value is null)
                    KeyInfo = null;
                else
                    KeyInfo = new KeyInfo[] { new(value.Key, value.Modifiers) };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the associated shortcut key.
        /// </summary>
        [Browsable(false)]
        public virtual KeyInfo[]? KeyInfo
        {
            get
            {
                return keys;
            }

            set
            {
                if (keys == value)
                    return;

                keys = value;
            }
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="Keys"/> to <see cref="ShortcutInfo"/>.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator ShortcutInfo?(Keys value)
        {
            if (value == Keys.None)
                return null;
            ShortcutInfo result = new();
            result.Keys = value;
            return result;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ShortcutInfo"/>
        /// to <see cref="Keys"/>.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator Keys(ShortcutInfo? value)
        {
            var result = value?.Keys ?? Keys.None;
            return result;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ShortcutInfo"/>
        /// to <see cref="KeyGesture"/>.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator KeyGesture?(ShortcutInfo? value)
        {
            return value?.KeyGesture;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="KeyGesture"/>
        /// to <see cref="ShortcutInfo"/>.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator ShortcutInfo?(KeyGesture? value)
        {
            ShortcutInfo? shortcut;

            if (value is null)
                shortcut = null;
            else
            {
                shortcut = new();
                shortcut.KeyGesture = value;
            }

            return shortcut;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="ShortcutInfo"/>
        /// to <see cref="KeyInfo"/> array.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator KeyInfo[]?(ShortcutInfo? value)
        {
            return value?.KeyInfo;
        }

        /// <summary>
        /// Implicit conversion operator from <see cref="KeyInfo"/> array
        /// to <see cref="ShortcutInfo"/>.
        /// </summary>
        /// <param name="value">Value to convert from.</param>
        public static implicit operator ShortcutInfo?(KeyInfo[]? value)
        {
            ShortcutInfo? shortcut;

            if (value is null)
                shortcut = null;
            else
            {
                shortcut = new();
                shortcut.KeyInfo = value;
            }

            return shortcut;
        }

        /// <summary>
        /// Retrieves platform-specific key information based on the current operating system.
        /// </summary>
        /// <remarks>The returned key information is filtered to include only those keys relevant to the
        /// backend operating system. This method can return <see langword="null"/> if the filtering process
        /// results in no applicable keys.</remarks>
        /// <returns>An array of <see cref="UI.KeyInfo"/> objects representing the filtered key information
        /// specific to the current platform, or <see langword="null"/> if no keys are available.</returns>
        public virtual UI.KeyInfo[]? GetPlatformSpecificKeys()
        {
            var filteredKeys = Alternet.UI.KeyInfo.FilterBackendOs(KeyInfo);
            return filteredKeys;
        }

        /// <summary>
        /// Retrieves the first platform-specific key, if available.
        /// </summary>
        /// <remarks>This method returns the first key from the collection of platform-specific keys.
        /// If no platform-specific keys are available, it returns <see langword="null"/>.</remarks>
        /// <returns>The first platform-specific key as a <see cref="UI.KeyInfo"/> object,
        /// or <see langword="null"/> if no platform-specific keys are found.</returns>
        public virtual UI.KeyInfo? GetFirstPlatformSpecificKey()
        {
            var filteredKeys = GetPlatformSpecificKeys();
            if (filteredKeys is not null && filteredKeys.Length > 0)
                return filteredKeys[0];
            return null;
        }

        /// <summary>
        /// Converts the object to its string representation based on the specified formatting options.
        /// </summary>
        /// <remarks>The method applies the specified formatting options to the first valid key in the
        /// filtered key information. If a template is specified in the <paramref name="options"/>,
        /// it is used to format the output string.</remarks>
        /// <param name="options">The formatting options to apply when generating the string representation.
        /// This includes settings such as
        /// whether to use a template and user-specific formatting preferences.</param>
        /// <returns>A string representation of the object based on the provided <paramref name="options"/>,
        /// or <see langword="null"/> if no valid key information is available.</returns>
        public virtual string? ToString(FormatOptions options)
        {
            var key = GetFirstPlatformSpecificKey();

            if (key is null)
                return null;

            if (options.UseTemplate)
            {
                var result = string.Format(options.Template, key.ToString(options.ForUser));
                return result;
            }
            else
            {
                return key.ToString(options.ForUser);
            }
        }

        /// <summary>
        /// Specifies formatting options for converting shortcut information to a string.
        /// </summary>
        public struct FormatOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether to use a template for formatting.
            /// </summary>
            public bool UseTemplate { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the string representation should be formatted for user display.
            /// </summary>
            public bool ForUser { get; set; }

            /// <summary>
            /// Gets or sets the template string used for formatting, if <see cref="UseTemplate"/> is <c>true</c>.
            /// </summary>
            public string? Template { get; set; }

            /// <summary>
            /// Gets or sets an object that contains data associated with this instance.
            /// </summary>
            /// <remarks>This property can be used to store additional information related to the
            /// instance, such as metadata or user-defined data. The type is <see cref="object"/>,
            /// allowing any type of data to be assigned.</remarks>
            public object? Tag { get; set; }
        }
    }
}
