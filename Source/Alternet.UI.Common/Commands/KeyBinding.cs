// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Description: The KeyBinding class is used by the developer to create Keyboard Input Bindings
//                  See spec at : http://avalon/coreui/Specs/Commanding(new).mht
// * KeyBinding class serves the purpose of Input Bindings for Keyboard Device.
using System;
using System.ComponentModel;
using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// KeyBinding - Implements InputBinding (generic InputGesture-Command map)
    ///         KeyBinding acts like a map for KeyGesture and Commands.
    ///         Most of the logic is in InputBinding and KeyGesture, this only
    ///         facilitates user  to add Key/Modifiers directly without going in
    ///         KeyGesture path. Also it provides the KeyGestureTypeConverter
    ///         on the Gesture property to have KeyGesture, like Ctrl+X, Alt+V
    ///         defined in Markup as Gesture="Ctrl+X" working
    /// </summary>
    public class KeyBinding : InputBinding
    {
        /// <summary>
        ///     Dependency Property for Modifiers
        /// </summary>
        public static readonly DependencyProperty ModifiersProperty =
            DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyBinding), new UIPropertyMetadata(ModifierKeys.None, new PropertyChangedCallback(OnModifiersPropertyChanged)));

        /// <summary>
        ///     Dependency Property for Key
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(
                "Key",
                typeof(Key),
                typeof(KeyBinding),
                new UIPropertyMetadata(
                    Key.None,
                    new PropertyChangedCallback(OnKeyPropertyChanged)));

        private bool settingGesture = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBinding"/> class.
        /// </summary>
        public KeyBinding()
            : base()
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
        ///     Key
        /// </summary>
        public Key Key
        {
            get
            {
                return (Key)GetValue(KeyProperty);
            }

            set
            {
                SetValue(KeyProperty, value);
            }
        }

        /// <summary>
        /// KeyGesture Override, to ensure type-safety and provide a
        ///  TypeConverter for KeyGesture
        /// </summary>
        [TypeConverter(typeof(KeyGestureConverter))]
        [ValueSerializer(typeof(KeyGestureValueSerializer))]
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
        ///     Modifiers
        /// </summary>
        public ModifierKeys Modifiers
        {
            get
            {
                return (ModifierKeys)GetValue(ModifiersProperty);
            }

            set
            {
                SetValue(ModifiersProperty, value);
            }
        }

        private static void OnModifiersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KeyBinding keyBinding = (KeyBinding)d;
            keyBinding.SynchronizeGestureFromProperties(keyBinding.Key, (ModifierKeys)e.NewValue);
        }

        private static void OnKeyPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            KeyBinding keyBinding = (KeyBinding)d;
            keyBinding.SynchronizeGestureFromProperties((Key)e.NewValue, keyBinding.Modifiers);
        }

        /// <summary>
        ///     Synchronized Properties from Gesture
        /// </summary>
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

        /// <summary>
        ///     Synchronized Gesture from properties
        /// </summary>
        private void SynchronizeGestureFromProperties(Key key, ModifierKeys modifiers)
        {
            if (!settingGesture)
            {
                settingGesture = true;
                try
                {
                    Gesture = new KeyGesture(key, modifiers, /*validateGesture = */ false);
                }
                finally
                {
                    settingGesture = false;
                }
            }
        }
    }
}
