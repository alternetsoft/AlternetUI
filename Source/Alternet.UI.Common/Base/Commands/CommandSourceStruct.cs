using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a method that determines whether a given <see cref="ICommandSource"/>
    /// can execute a command.
    /// </summary>
    /// <param name="commandSource">The source of the command to evaluate.
    /// Must not be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the specified <see cref="ICommandSource"/>
    /// can execute the command; otherwise, <see langword="false"/>.</returns>
    public delegate bool CanExecuteCommandSourceDelegate(ICommandSource commandSource);

    /// <summary>
    /// Represents a method that executes a command using the specified command source.
    /// </summary>
    /// <remarks>This delegate is typically used to define a callback or handler
    /// for executing commands
    /// provided by an <see cref="ICommandSource"/>. The implementation of the
    /// delegate determines how the command is
    /// processed.</remarks>
    /// <param name="commandSource">The source of the command to be executed.
    /// This parameter cannot be <see langword="null"/>.</param>
    public delegate void ExecuteCommandSourceDelegate(ICommandSource commandSource);

    /// <summary>
    /// Helper structure that contains command and parameter. It can be used as a field
    /// in classes that need <see cref="Command"/> and <see cref="CommandParameter"/>
    /// properties in order to reuse the common functionality.
    /// </summary>
    public struct CommandSourceStruct : ICommandSource
    {
        /// <summary>
        /// Optional override for executing a command source.
        /// If set, this delegate will be called
        /// instead of the default execution logic
        /// in <see cref="ExecuteCommandSource(ICommandSource)"/>.
        /// </summary>
        public static ExecuteCommandSourceDelegate? ExecuteCommandSourceOverride;

        /// <summary>
        /// Optional override for checking if a command source can execute.
        /// If set, this delegate will be called
        /// instead of the default logic in <see cref="CanExecuteCommandSource(ICommandSource)"/>.
        /// </summary>
        public static CanExecuteCommandSourceDelegate? CanExecuteCommandSourceOverride;

        /// <summary>
        /// Occurs when <see cref="Command"/> is changed or
        /// <see cref="ICommand.CanExecute"/> is changed.
        /// </summary>
        public Action? Changed;

        private readonly object? container;

        private ICommand? command;
        private object? commandParameter;
        private object? commandTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSourceStruct"/> struct.
        /// </summary>
        /// <param name="container">Container of the struct.</param>
        public CommandSourceStruct(object container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets whether command can be executed.
        /// </summary>
        [Browsable(false)]
        public readonly bool CanExecute
        {
            get
            {
                if (Command is null)
                    return true;
                return CanExecuteCommandSource(this);
            }
        }

        /// <summary>
        /// Get or sets target object where the command should be raised.
        /// </summary>
        public object? CommandTarget
        {
            readonly get
            {
                return commandTarget ?? container;
            }

            set
            {
                if (commandTarget == value)
                    return;
                commandTarget = value;
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
        /// Gets whether <paramref name="commandSource"/> can be executed.
        /// </summary>
        /// <param name="commandSource">Command source.</param>
        /// <returns></returns>
        public static bool CanExecuteCommandSource(ICommandSource commandSource)
        {
            if (CanExecuteCommandSourceOverride is not null)
            {
                return CanExecuteCommandSourceOverride(commandSource);
            }

            try
            {
                UI.Command.CurrentTarget = commandSource.CommandTarget;
                var command = commandSource.Command;
                if (command != null)
                {
                    var parameter = commandSource.CommandParameter;
                    return command.CanExecute(parameter);
                }

                return false;
            }
            finally
            {
                UI.Command.CurrentTarget = null;
            }
        }

        /// <summary>
        /// Executes command specified with <see cref="ICommandSource"/>
        /// </summary>
        /// <param name="commandSource">Command to execute.</param>
        public static void ExecuteCommandSource(ICommandSource commandSource)
        {
            if (ExecuteCommandSourceOverride is not null)
            {
                ExecuteCommandSourceOverride(commandSource);
                return;
            }

            try
            {
                UI.Command.CurrentTarget = commandSource.CommandTarget;

                var command = commandSource.Command;
                if (command != null)
                {
                    var parameter = commandSource.CommandParameter;
                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                }
            }
            finally
            {
                UI.Command.CurrentTarget = null;
            }
        }

        /// <summary>
        /// Executes command if it is specified.
        /// </summary>
        public readonly void Execute()
        {
            ExecuteCommandSource(this);
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
