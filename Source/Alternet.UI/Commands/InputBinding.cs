// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Security;
using Alternet.UI.Internal;

namespace Alternet.UI
{
    /// <summary>
    /// InputBinding - InputGesture and ICommand combination.
    /// Used to specify the binding between Gesture and Command at Element level.
    /// </summary>
    public class InputBinding : DependencyObject, ICommandSource
    {
        /// <summary>
        /// Dependency property for <see cref="CommandTarget"/>.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(InputBinding));

        /// <summary>
        /// Dependency property for <see cref="Command"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(InputBinding),
                new UIPropertyMetadata(null, new PropertyChangedCallback(OnCommandPropertyChanged)));

        /// <summary>
        /// Dependency property for <see cref="CommandParameter"/>.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InputBinding));

        private static readonly object DataLock = new();

        private InputGesture? gesture = null;

        // Fields to implement DO's inheritance context
        private DependencyObject? inheritanceContext = null;
        private bool hasMultipleInheritanceContexts = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBinding"/> class.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="gesture">Input Gesture.</param>
        public InputBinding(ICommand command, InputGesture gesture)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.gesture = gesture ?? throw new ArgumentNullException(nameof(gesture));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBinding"/> class.
        /// </summary>
        protected InputBinding()
        {
        }

        /// <summary>
        /// Command Object associated
        /// </summary>
        [TypeConverter(
            "System.Windows.Input.CommandConverter, PresentationFramework, Version="
            + BuildInfo.WCP_VERSION + ", Culture=neutral, PublicKeyToken=" +
            BuildInfo.WCP_PUBLIC_KEY_TOKEN + ", Custom=null")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        ///     A parameter for the command.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }

            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Where the command should be raised.
        /// </summary>
        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }

            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }

        /// <summary>
        /// <see cref="InputGesture"/> associated with the <see cref="Command"/>.
        /// </summary>
        public virtual InputGesture? Gesture
        {
            // We would like to make this getter non-virtual but that's not legal
            // in C#.  Luckily there is no security issue with leaving it virtual.
            get
            {
                return gesture;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                lock (DataLock)
                {
                    gesture = value;
                }
            }
        }

        /// <summary>
        /// Says if the current instance has multiple InheritanceContexts.
        /// </summary>
        internal override bool HasMultipleInheritanceContexts => hasMultipleInheritanceContexts;

        /// <summary>
        /// Defines the DO's inheritance context.
        /// </summary>
        internal override DependencyObject? InheritanceContext => inheritanceContext;

        internal string ManagedCommandId { get; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Receives a new inheritance context.
        /// </summary>
        internal override void AddInheritanceContext(
            DependencyObject context,
            DependencyProperty property)
        {
            InheritanceContextHelper.AddInheritanceContext(
                context,
                this,
                ref hasMultipleInheritanceContexts,
                ref inheritanceContext);
        }

        /// <summary>
        /// Removes an inheritance context.
        /// </summary>
        internal override void RemoveInheritanceContext(
            DependencyObject context,
            DependencyProperty property)
        {
            InheritanceContextHelper.RemoveInheritanceContext(
                context,
                this,
                ref hasMultipleInheritanceContexts,
                ref inheritanceContext);
        }

        /// <summary>
        ///     Property changed callback for Command property
        /// </summary>
        private static void OnCommandPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
