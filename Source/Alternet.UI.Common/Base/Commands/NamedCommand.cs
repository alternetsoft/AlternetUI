using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements named command which is executed using
    /// <see cref="NamedCommands.Execute"/>.
    /// </summary>
    public class NamedCommand : Command
    {
        private string commandName;
        private ICommand? command;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedCommand"/> class.
        /// </summary>
        /// <param name="command">Command name.</param>
        public NamedCommand(string? command)
        {
            this.commandName = command ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets command name.
        /// </summary>
        public virtual string CommandName
        {
            get => commandName;

            set
            {
                if (commandName == value)
                    return;
                commandName = value;
                command = null;
                RaiseCanExecuteChanged();
            }
        }

        private ICommand Command
        {
            get
            {
                if (command is null)
                {
                    command = NamedCommands.Default.GetCommand(CommandName);
                    command.CanExecuteChanged += (s, e) =>
                    {
                        RaiseCanExecuteChanged();
                    };
                }

                return command;
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(object? parameter)
        {
            return Command.CanExecute(parameter);
        }

        /// <inheritdoc/>
        public override void Execute(object? parameter)
        {
            Command.Execute(parameter);
        }
    }
}
