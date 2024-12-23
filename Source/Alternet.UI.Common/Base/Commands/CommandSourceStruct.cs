using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Helper structure that contains command and parameter. It can be used as a field
    /// in classes that need <see cref="Command"/> and <see cref="CommandParameter"/>
    /// properties in order to reuse the common functionality.
    /// </summary>
    public struct CommandSourceStruct : ICommandSource
    {
        /// <summary>
        /// Occurs when <see cref="Command"/> is changed or
        /// <see cref="ICommand.CanExecute"/> is changed.
        /// </summary>
        public Action? Changed;

        private ICommand? command;
        private object? commandParameter;

        /// <summary>
        /// Gets whether command can be executed.
        /// </summary>
        public readonly bool CanExecute
        {
            get
            {
                if (Command is null)
                    return true;
                return CommandHelpers.CanExecuteCommandSource(this);
            }
        }

        /// <inheritdoc cref="ICommandSource.CommandParameter"/>
        public ICommand? Command
        {
            readonly get
            {
                return command;
            }

            set
            {
                if (command == value)
                    return;
                var oldCommand = command;
                command = value;
                OnCommandChanged(oldCommand, value);
            }
        }

        /// <inheritdoc cref="ICommandSource.CommandParameter"/>
        public object? CommandParameter
        {
            readonly get
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
        /// Executes command if it is specified.
        /// </summary>
        public readonly void Execute()
        {
            CommandHelpers.ExecuteCommandSource(this);
        }

        /// <summary>
        /// Called when <see cref="Command"/> property is changed.
        /// </summary>
        private readonly void OnCommandChanged(ICommand? oldCommand, ICommand? newCommand)
        {
            if (oldCommand != null)
                UnhookCommand(oldCommand);

            if (newCommand != null)
                HookCommand(newCommand);

            UpdateCanExecute();
        }

        private readonly void UnhookCommand(ICommand command)
        {
            command.CanExecuteChanged -= OnCanExecuteChanged;
            UpdateCanExecute();
        }

        private readonly void HookCommand(ICommand command)
        {
            command.CanExecuteChanged += OnCanExecuteChanged;
            UpdateCanExecute();
        }

        private readonly void OnCanExecuteChanged(object? sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        private readonly void UpdateCanExecute()
        {
            Changed?.Invoke();
        }
    }
}
