using System;
using System.Diagnostics;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to <see cref="ICommandSource"/>.
    /// </summary>
    public static class CommandHelpers
    {
        /// <summary>
        /// Gets whether <paramref name="commandSource"/> can be executed.
        /// </summary>
        /// <param name="commandSource">Command source.</param>
        /// <returns></returns>
        public static bool CanExecuteCommandSource(ICommandSource commandSource)
        {
            var command = commandSource.Command;
            if (command != null)
            {
                var parameter = commandSource.CommandParameter;
                return command.CanExecute(parameter);
            }

            return false;
        }

        /// <summary>
        /// Executes command specified with <see cref="ICommandSource"/>
        /// </summary>
        /// <param name="commandSource">Command to execute.</param>
        public static void ExecuteCommandSource(ICommandSource commandSource)
        {
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
    }
}
