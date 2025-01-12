using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to execute named command with parameters.
    /// </summary>
    public interface IDoCommand
    {
        /// <summary>
        /// Executes a command with the specified name and parameters.
        /// </summary>
        /// <param name="cmdName">
        /// Name of the command to execute.
        /// </param>
        /// <param name="args">
        /// Parameters of the command.
        /// </param>
        /// <returns>
        /// A <see cref="object"/> representing the result of the command execution.
        /// </returns>
        object? DoCommand(string cmdName, params object?[] args);
    }
}
