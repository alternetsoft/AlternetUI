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
    }
}
