using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which allow to execute and handle
    /// named commands.
    /// </summary>
    public class NamedCommands
    {
        /// <summary>
        /// Gets 'App.Log' command.
        /// </summary>
        public static readonly ICommand CommandAppLog;

        /// <summary>
        /// Gets or sets default named commands provider.
        /// </summary>
        public static NamedCommands Default = new();

        private readonly FlagsAndAttributes<string, CommandEntry?> commands = new();

        static NamedCommands()
        {
            CommandAppLog = Default.Register(
                "App.Log",
                (p) =>
                {
                    App.Log(p);
                });
        }

        /// <summary>
        /// Gets permanent <see cref="ICommand"/> for the specified name.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <returns></returns>
        /// <remarks>
        /// The result is not changed after multiple calls to 'Register' methods.
        /// When new <see cref="ICommand"/> is registered for the given name, the result
        /// of <see cref="GetCommand"/> obtained previously still can be used to call the
        /// newly registered command.
        /// </remarks>
        public virtual ICommand GetCommand(string name)
        {
            return GetEntry(name);
        }

        /// <summary>
        /// Registers command with the specified name and implementation.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Function to get whether command can be executed.</param>
        public virtual ICommand Register(
            string name,
            Command.ExecuteDelegate execute,
            Command.CanExecuteDelegate? canExecute = null)
        {
            Command command = new(execute, canExecute);
            Register(name, command);
            return command;
        }

        /// <summary>
        /// Registers command with the specified name and implementation.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="execute">Action to execute.</param>
        public virtual ICommand Register(
            string name,
            Action execute)
        {
            return Register(name, (param) => execute());
        }

        /// <summary>
        /// Registers command with the specified name and implementation.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="command">Command provider.</param>
        public virtual void Register(string name, ICommand? command)
        {
            var entry = GetEntry(name);
            entry.Command = command;
        }

        /// <summary>
        /// Returns whether the command can be executed.
        /// </summary>
        /// <param name="parameter">A parameter that may be used in executing the command.
        /// This parameter may be ignored by some implementations.</param>
        /// <returns>True if the command can be executed with the given parameter and current
        /// state; False otherwise.</returns>
        /// <param name="name">Command name.</param>
        public virtual bool CanExecute(string name, object? parameter = null)
        {
            var commandEntry = commands.GetAttribute(name);
            return commandEntry?.Command?.CanExecute(parameter) ?? false;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">A parameter that may be used when the command is executed.
        /// This parameter may be ignored by some implementations.</param>
        /// <param name="name">Command name.</param>
        public virtual void Execute(string name, object? parameter = null)
        {
            var commandEntry = commands.GetAttribute(name);
            commandEntry?.Command?.Execute(parameter);
        }

        private CommandEntry GetEntry(string name)
        {
            var commandEntry = commands.GetAttribute(name);

            if (commandEntry is null)
            {
                commandEntry = new();
                commands.SetAttribute(name, commandEntry);
            }

            return commandEntry;
        }

        private class CommandEntry : ICommand
        {
            private ICommand? command;

            public event EventHandler? CanExecuteChanged;

            public ICommand? Command
            {
                get
                {
                    return command;
                }

                set
                {
                    if (command == value)
                        return;

                    if (command is not null)
                    {
                        command.CanExecuteChanged -= Command_CanExecuteChanged;
                    }

                    command = value;
                    if(command is not null)
                    {
                        command.CanExecuteChanged += Command_CanExecuteChanged;
                    }

                    RaiseCanExecuteChanged();
                }
            }

            public bool CanExecute(object? parameter = null)
            {
                return Command?.CanExecute(parameter) ?? false;
            }

            public void Execute(object? parameter = null)
            {
                Command?.Execute(parameter);
            }

            private void Command_CanExecuteChanged(object? sender, EventArgs e)
            {
                RaiseCanExecuteChanged();
            }

            private void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
