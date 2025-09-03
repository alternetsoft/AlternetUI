using System;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    /// The delegate to use for handlers that receive KeyEventArgs.
    /// </summary>
    public delegate void KeyEventHandler(object? sender, KeyEventArgs e);

    /// <summary>
    /// Contains information about key presses and key states.
    /// </summary>
    public class KeyEventArgs : CustomKeyEventArgs
    {
        private KeyStates keyStates;
        private bool suppressKeyPress;
        private uint repeatCount;

        /// <summary>
        /// Constructs an instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        public KeyEventArgs()
        {
        }

        /// <summary>
        /// Constructs an instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="modifiers">The set of modifier keys pressed
        /// at the time when event was received.</param>
        /// <param name="keyStates">The state of the key referenced by the event.</param>
        /// <param name="repeatCount">Number of repeated key presses.</param>
        /// <param name="originalTarget">Original target object which received the event.</param>
        public KeyEventArgs(
            object originalTarget,
            Key key,
            KeyStates keyStates,
            ModifierKeys modifiers,
            uint repeatCount)
            : base(originalTarget, key, modifiers)
        {
            this.keyStates = keyStates;
            this.repeatCount = repeatCount;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the key event should be passed on to
        /// the underlying control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the key event should not be sent
        /// to the control; otherwise, <see langword="false" />.
        /// </returns>
        public virtual bool SuppressKeyPress
        {
            get
            {
                return suppressKeyPress;
            }

            set
            {
                suppressKeyPress = value;
                Handled = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the key referenced by the event.
        /// </summary>
        public virtual KeyStates KeyStates
        {
            get
            {
                return keyStates;
            }

            set
            {
                keyStates = value;
            }
        }

        /// <summary>
        /// Gets or sets key repeat count.
        /// </summary>
        public virtual uint RepeatCount
        {
            get => repeatCount;
            set => repeatCount = value;
        }

        /// <summary>
        /// Gets whether the key pressed is a repeated key or not.
        /// </summary>
        public virtual bool IsRepeat
        {
            get
            {
                return repeatCount > 0;
            }
        }

        /// <summary>
        /// Gets whether <see cref="SuppressKeyPress"/> or
        /// <see cref="HandledEventArgs.Handled"/> is True.
        /// </summary>
        public bool IsHandledOrSuppressed => SuppressKeyPress || Handled;

        /// <summary>
        /// Gets whether or not the key referenced by the event is down.
        /// </summary>
        public virtual bool IsDown => keyStates == KeyStates.Down;

        /// <summary>
        /// Gets whether or not the key referenced by the event is up.
        /// </summary>
        public virtual bool IsUp => !IsDown;

        /// <summary>
        /// Gets whether or not the key referenced by the event is toggled.
        /// </summary>
        public virtual bool IsToggled => keyStates == KeyStates.Toggled;

        /// <summary>
        /// Assigns True to <see cref="SuppressKeyPress"/> and <see cref="HandledEventArgs.Handled"/>.
        /// </summary>
        public virtual void Suppressed()
        {
            SuppressKeyPress = true;
            Handled = true;
        }
    }
}