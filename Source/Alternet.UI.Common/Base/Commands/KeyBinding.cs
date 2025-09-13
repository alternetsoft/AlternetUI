using System;
using System.ComponentModel;

using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// KeyBinding acts like a map for <see cref="KeyGesture"/> and commands.
    /// Most of the logic is in <see cref="InputBinding"/> and <see cref="KeyGesture"/>,
    /// this only facilitates user to add key and modifiers directly without going in
    /// <see cref="KeyGesture"/> path. Also it provides the type converter
    /// on the <see cref="Gesture"/> property.
    /// </summary>
    public partial class KeyBinding : InputBinding
    {
        /// <summary>
        /// Represents an empty key binding with no associated keys or actions.
        /// </summary>
        /// <remarks>This field provides a default, immutable instance of a key binding
        /// that can be used to represent the absence of any key binding.
        /// It is equivalent to a "null object" pattern for key bindings.</remarks>
        public static readonly KeyBinding Empty = new EmptyKeyBinding();

        private bool settingGesture = false;
        private ModifierKeys modifiers = ModifierKeys.None;
        private Key key = Key.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        public KeyBinding()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        /// <param name="command">Associated command.</param>
        /// <param name="gesture">Associated <see cref="KeyGesture"/>.</param>
        public KeyBinding(ICommand command, KeyGesture gesture)
            : base(command, gesture)
        {
            SynchronizePropertiesFromGesture(gesture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        /// <param name="command">Associated command.</param>
        /// <param name="modifiers">Key modifiers.</param>
        /// <param name="key">Key.</param>
        public KeyBinding(ICommand command, Key key, ModifierKeys modifiers)
            : this(command, new KeyGesture(key, modifiers))
        {
        }

        /// <summary>
        /// Gets or sets key.
        /// </summary>
        public virtual Key Key
        {
            get
            {
                return key;
            }

            set
            {
                if (key == value)
                    return;
                key = value;
                SynchronizeGestureFromProperties(value, Modifiers);
            }
        }

        /// <inheritdoc/>
        [TypeConverter(typeof(KeyGestureConverter))]
        public override InputGesture? Gesture
        {
            get
            {
                return base.Gesture as KeyGesture;
            }

            set
            {
                if (value is KeyGesture keyGesture)
                {
                    base.Gesture = value;
                    SynchronizePropertiesFromGesture(keyGesture);
                }
                else
                {
                    throw new ArgumentException(
                        SR.Get(SRID.InputBinding_ExpectedInputGesture, typeof(KeyGesture)));
                }
            }
        }

        /// <summary>
        /// Gets or sets key modifiers.
        /// </summary>
        public virtual ModifierKeys Modifiers
        {
            get
            {
                return modifiers;
            }

            set
            {
                if (modifiers == value)
                    return;
                modifiers = value;
                SynchronizeGestureFromProperties(Key, value);
            }
        }

        /// <inheritdoc/>
        public override bool HasKey(Key key, ModifierKeys modifiers)
        {
            return base.HasKey(key, modifiers);
        }

        private void SynchronizePropertiesFromGesture(KeyGesture keyGesture)
        {
            if (!settingGesture)
            {
                settingGesture = true;
                try
                {
                    Key = keyGesture.Key;
                    Modifiers = keyGesture.Modifiers;
                }
                finally
                {
                    settingGesture = false;
                }
            }
        }

        private void SynchronizeGestureFromProperties(Key key, ModifierKeys modifiers)
        {
            if (!settingGesture)
            {
                settingGesture = true;
                try
                {
                    Gesture = new KeyGesture(key, modifiers, validateGesture: false);
                }
                finally
                {
                    settingGesture = false;
                }
            }
        }

        internal class EmptyKeyBinding : KeyBinding
        {
            public override Key Key
            {
                get => Key.None;
                set
                {
                }
            }

            public override ModifierKeys Modifiers
            {
                get => ModifierKeys.None;
                set
                {
                }
            }

            public override ICommand? Command
            {
                get => null;
                set
                {
                }
            }

            public override object? CommandParameter
            {
                get => null;
                set
                {
                }
            }

            public override bool IsActive
            {
                get => false;
                set
                {
                }
            }
            public override object? CommandTarget
            {
                get => null;
                set
                {
                }
            }

            public override InputGesture? Gesture
            {
                get => null;
                set
                {
                }
            }
        }
    }
}