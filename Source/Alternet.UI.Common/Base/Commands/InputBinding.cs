using System;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    /// Used to specify the binding between <see cref="InputGesture"/>
    /// and <see cref="ICommand"/>.
    /// </summary>
    public class InputBinding : BaseObjectWithAttr, ICommandSource
    {
        private static readonly object DataLock = new();

        private InputGesture? gesture = null;
        private ICommand? command;
        private object? commandTarget;
        private object? commandParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBinding"/> class with
        /// the specified parameters.
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
        public InputBinding()
        {
        }

        /// <summary>
        /// Gets or sets command associated with the object.
        /// </summary>
        public virtual ICommand? Command
        {
            get
            {
                return command;
            }

            set
            {
                if (command == value)
                    return;
                command = value;
            }
        }

        /// <summary>
        /// Gets or sets a parameter for the command.
        /// </summary>
        public virtual object? CommandParameter
        {
            get
            {
                return commandParameter;
            }

            set
            {
                if (commandParameter == value)
                    return;
                commandParameter = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this binding is active. If binding is not active, it is ignored.
        /// </summary>
        public virtual bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets target where the command should be raised.
        /// </summary>
        public virtual object? CommandTarget
        {
            get
            {
                return commandTarget;
            }

            set
            {
                if (commandTarget == value)
                    return;
                commandTarget = value;
            }
        }

        /// <summary>
        /// <see cref="InputGesture"/> associated with the <see cref="Command"/>.
        /// </summary>
        public virtual InputGesture? Gesture
        {
            get
            {
                return gesture;
            }

            set
            {
                value ??= new InputGesture();

                lock (DataLock)
                {
                    gesture = value;
                }
            }
        }

        /// <summary>
        /// Executes <see cref="Command"/> with the specified <see cref="CommandParameter"/>.
        /// </summary>
        /// <returns>True if command was executed; False if command cannot be executed.</returns>
        public virtual bool Execute()
        {
            if (command == null)
                return false;

            var prm = CommandParameter;

            if (!command.CanExecute(prm))
                return false;

            command.Execute(prm);
            return true;
        }

        /// <summary>
        /// Gets whether <see cref="Gesture"/> has the specified key and key modifiers.
        /// </summary>
        public virtual bool HasKey(Key key, ModifierKeys modifiers)
        {
            if(Gesture is KeyGesture gst)
            {
                if (gst.HasKey(key, modifiers))
                    return true;
            }

            return false;
        }
    }
}