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
        private string command;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedCommand"/> class.
        /// </summary>
        /// <param name="command">Command name.</param>
        public NamedCommand(string? command)
        {
            this.command = command ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets command name.
        /// </summary>
        public virtual string CommandName
        {
            get => command;

            set
            {
                if (command == value)
                    return;
                command = value;
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(object? parameter)
        {
            return NamedCommands.Default.CanExecute(CommandName, parameter);
        }

        /// <inheritdoc/>
        public override void Execute(object? parameter)
        {
            NamedCommands.Default.Execute(CommandName, parameter);
        }
    }
}
